#load "./build/data.cake"
#load "./build/versioning.cake"
#load "./build/buildserver.cake"

#tool "nuget:https://api.nuget.org/v3/index.json?package=gitreleasemanager&version=0.7.1"

////////////////////////////////////////////////////////
// ARGUMENTS
////////////////////////////////////////////////////////

var target = Argument("target", "Default");

////////////////////////////////////////////////////////
// SETUP
////////////////////////////////////////////////////////

Setup<BuildData>(context => 
{
    var server = BuildServer.Initialize(context);
    var versioning = BuildVersion.Calculate(context, server);

    context.Information("Version: {0}", versioning.Version);

    return new BuildData {
        Configuration = context.Argument<string>("configuration", "Release"),
        Credentials = new GitHubCredentials(context),
        Versioning = versioning,
        Server = server,
    };
});

////////////////////////////////////////////////////////
// TASKS
////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildData>(data =>
{
    CleanDirectory("./.artifacts");
    CleanDirectories($"./src/**/bin/{data.Configuration}");
    CleanDirectories($"./src/**/obj/{data.Configuration}");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./src/Spectre.Query.sln");
});

Task("Build")
    .IsDependentOn("Restore")
    .Does<BuildData>(data =>
{
    DotNetCoreBuild("./src/Spectre.Query.sln", new DotNetCoreBuildSettings {
        Configuration = "Release",
        Verbosity = DotNetCoreVerbosity.Minimal,
        NoRestore = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error)
            .WithProperty("Version", data.Versioning.SemVersion)
            .WithProperty("AssemblyVersion", data.Versioning.Version)
            .WithProperty("FileVersion", data.Versioning.Version)
            .WithProperty("PackageVersion", data.Versioning.SemVersion)
    });
});

Task("Run-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./src/Spectre.Query.Tests/Spectre.Query.Tests.csproj", new DotNetCoreTestSettings {
        NoBuild = true,
        NoRestore = true,
        Configuration = "Release"
    });
});

Task("Package")
    .IsDependentOn("Run-Tests")
    .Does<BuildData>(data =>
{
    DotNetCorePack("./src/Spectre.Query/Spectre.Query.csproj", new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = "./.artifacts",
        NoRestore = true,
        NoBuild = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", data.Versioning.SemVersion)
            .WithProperty("AssemblyVersion", data.Versioning.Version)
            .WithProperty("FileVersion", data.Versioning.Version)
            .WithProperty("PackageVersion", data.Versioning.SemVersion)
    });

    DotNetCorePack("./src/Spectre.Query.AspNetCore/Spectre.Query.AspNetCore.csproj", new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = "./.artifacts",
        NoRestore = true,
        NoBuild = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", data.Versioning.SemVersion)
            .WithProperty("AssemblyVersion", data.Versioning.Version)
            .WithProperty("FileVersion", data.Versioning.Version)
            .WithProperty("PackageVersion", data.Versioning.SemVersion)
    });
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Package")
    .WithCriteria<BuildData>((context, data) => data.Server.IsRunningOnAppVeyor, "Not running on AppVeyor")
    .Does<BuildData>(data => 
{
    var files = GetFiles("./.artifacts/*.nupkg");
    foreach(var file in files)
    {
        AppVeyor.UploadArtifact(file);
    }
});

Task("Create-Release")
    .WithCriteria<BuildData>((context, data) => !data.Server.IsRunningOnAppVeyor, "Not running on AppVeyor")
    .WithCriteria<BuildData>((context, data) => !data.Server.IsPullRequest, "Won't publish pull requests")
    .WithCriteria<BuildData>((context, data) => data.Server.IsReleaseBranch, "Not on a release branch")
    .Does<BuildData>((context, data) =>
{
    CreateGitHubRelease(context, data);
});

Task("Publish-To-NuGet")
    .IsDependentOn("Package")
    .WithCriteria<BuildData>((context, data) => data.Server.IsRunningOnAppVeyor, "Not running on AppVeyor")
    .WithCriteria<BuildData>((context, data) => !data.Server.IsPullRequest, "Won't publish pull requests")
    .WithCriteria<BuildData>((context, data) => data.Server.IsMasterBranch, "Not on master branch")
    .WithCriteria<BuildData>((context, data) => data.Server.IsTaggedBuild, "Won't published untagged builds")
    .Does<BuildData>(data => 
{
    // Get the API key.
    var apiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrWhiteSpace(apiKey)) {
        throw new CakeException("Could not resolve API key.");
    }

    // Push the package.
    var files = GetFiles("./.artifacts/*.nupkg");
    foreach(var file in files)
    {
        Information(file.FullPath);
        NuGetPush(file, new NuGetPushSettings {
            ApiKey = apiKey,
            Source = "https://nuget.org/api/v2/package"
        });
    }
});

////////////////////////////////////////////////////////
// TARGETS
////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Package");

Task("AppVeyor")
    .IsDependentOn("Publish-To-NuGet");

////////////////////////////////////////////////////////
// EXECUTION
////////////////////////////////////////////////////////

RunTarget(target);