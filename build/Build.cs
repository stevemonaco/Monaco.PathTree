using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[GitHubActions("ci",
    GitHubActionsImage.WindowsLatest,
    PublishArtifacts = true,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Publish) }
)]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    Project PathTreeProject => Solution.GetProject("Monaco.PathTree");
    Project TestProject => Solution.GetProject("Monaco.PathTree.UnitTests");

    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath TestDirectory => RootDirectory / "test";

    public string CoverageFileName { get; } = "coverage.xml";

    Target Clean => _ => _
        .Executes(() =>
        {
            var dirs = RootDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(x => !x.ToString().Contains("\\build\\"));

            dirs.ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
            EnsureCleanDirectory(TestDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(TestProject)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
                .EnableCollectCoverage()
                .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                .SetCoverletOutput(TestDirectory / CoverageFileName));
        });

    Target Coverage => _ => _
        .DependsOn(Test)
        .TriggeredBy(Test)
        .Consumes(Test)
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetReports(TestDirectory / "*.xml")
                .SetReportTypes(ReportTypes.HtmlInline, ReportTypes.Badges)
                .SetTargetDirectory(TestDirectory / "reports")
                .SetFramework("net5.0"));
        });

    Target Package => _ => _
        .DependsOn(Test)
        .Produces(OutputDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(_ => _
                .SetProject(PathTreeProject)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
                .SetOutputDirectory(OutputDirectory));
        });

    Target Publish => _ => _
        .DependsOn(Package)
        .Produces(OutputDirectory / "*.dll")
        .Executes(() =>
        {
            DotNetPublish(_ => _
                .SetProject(PathTreeProject)
                .EnableNoRestore()
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .SetOutput(OutputDirectory));
        });
}
