#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.Unity3D",
                            repositoryOwner: "SamOatesGames",
                            repositoryName: "Cake.Unity3D",
							shouldRunDupFinder: false);

BuildParameters.PrintParameters(Context);

Build.Run();
