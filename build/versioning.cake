#tool nuget:?package=GitVersion.CommandLine&version=3.6.2
#load "./buildserver.cake"

public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }

    public static BuildVersion Calculate(ICakeContext context, BuildServer server)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        string version = context.Argument<string>("ver", null);
        string semVersion = version;
        
        if (version != null) 
        {
            return new BuildVersion
            {
                Version = version,
                SemVersion = semVersion
            };
        }

        if (context.IsRunningOnWindows())
        {
            context.Information("Calculating Semantic Version");

            if (server.IsRunningOnAppVeyor)
            {
                // Update AppVeyor version number.
                context.GitVersion(new GitVersionSettings{
                    OutputType = GitVersionOutput.BuildServer,
                });
            }
        }

        GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
        });

        version = assertedVersions.MajorMinorPatch;
        semVersion = assertedVersions.LegacySemVerPadded;

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("Fetching version from solution info...");
            version = ReadSolutionInfoVersion(context);
            semVersion = version;
        }

        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion
        };
    }

    public static string ReadSolutionInfoVersion(ICakeContext context)
    {
        var solutionInfo = context.ParseAssemblyInfo("./src/SolutionInfo.cs");
        if (!string.IsNullOrEmpty(solutionInfo.AssemblyVersion))
        {
            return solutionInfo.AssemblyVersion;
        }
        throw new CakeException("Could not parse version.");
    }
}