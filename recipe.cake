#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.Unity3D",
                            repositoryOwner: "SamOatesGames",
                            repositoryName: "Cake.Unity3D",
							shouldRunDupFinder: false,
							shouldRunInspectCode: false,
							shouldRunGitVersion: false);

BuildParameters.PrintParameters(Context);

Build.RunDotNetCore();
