#load nuget:?package=Spectre.Build&version=0.3.0

////////////////////////////////////////////////////////
// TASKS
////////////////////////////////////////////////////////

Task("Pack-NuGet")
    .PartOf(SpectreTasks.Pack)
    .Does<SpectreData>(data =>
{
    DotNetCorePack("./src/Spectre.Query/Spectre.Query.csproj", new DotNetCorePackSettings {
        Configuration = data.Configuration,
        OutputDirectory = data.Paths.NuGetPackages,
        NoRestore = true,
        NoBuild = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", data.Version.SemanticVersion)
            .WithProperty("AssemblyVersion", data.Version.MajorMinorPatchRevision)
            .WithProperty("FileVersion", data.Version.MajorMinorPatchRevision)
            .WithProperty("PackageVersion", data.Version.SemanticVersion)
    });

    DotNetCorePack("./src/Spectre.Query.AspNetCore/Spectre.Query.AspNetCore.csproj", new DotNetCorePackSettings {
        Configuration = data.Configuration,
        OutputDirectory = data.Paths.NuGetPackages,
        NoRestore = true,
        NoBuild = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", data.Version.SemanticVersion)
            .WithProperty("AssemblyVersion", data.Version.MajorMinorPatchRevision)
            .WithProperty("FileVersion", data.Version.MajorMinorPatchRevision)
            .WithProperty("PackageVersion", data.Version.SemanticVersion)
    });
});

Task("Create-Release")
    .WithCriteria<SpectreData>((context, data) => !data.CI.IsLocal, "Not running locally")
    .Does<SpectreData>((context, data) =>
{
    var username = context.Argument<string>("github-username", null);
    var password = context.Argument<string>("github-password", null);

    if (string.IsNullOrWhiteSpace(username) ||
        string.IsNullOrWhiteSpace(password))
    {
        throw new InvalidOperationException("No GitHub credentials has been provided.");
    }

    context.GitReleaseManagerCreate(
        username,
        password,
        "spectresystems", "spectre.query", 
        new GitReleaseManagerCreateSettings {
            Milestone = $"v{data.Version.MajorMinorPatch}",
            Name = $"v{data.Version.MajorMinorPatch}",
            TargetCommitish = "master"
    });
});

////////////////////////////////////////////////////////
// EXECUTION
////////////////////////////////////////////////////////

Spectre.Build();