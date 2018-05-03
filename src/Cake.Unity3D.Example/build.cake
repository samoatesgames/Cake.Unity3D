#reference "../Cake.Unity3D/bin/Debug/net46/Cake.Unity3D.dll"
//#reference "../Cake.Unity3D/bin/Release/net46/Cake.Unity3D.dll"
//#addin nuget:?package=Cake.Unity3D

var target = Argument("target", "Build-Win64");

Task("Build-Win64")
  .Does(() =>
{
	var projectPath = System.IO.Path.GetFullPath("./");
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "x64", "example.exe");
	
	string unityEditorLocation;
	if (!TryGetUnityInstall("Unity 2018.1.0f1 (64-bit)", out unityEditorLocation)) 
	{
		Error("Failed to find 'Unity 2018.1.0f1 (64-bit)' install location");
		return;
	}
	
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.StandaloneWindows64,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true
	};
	
	BuildUnity3DProject(projectPath, options);
});

Task("Build-WebGL")
  .Does(() =>
{
	var projectPath = System.IO.Path.GetFullPath("./");
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "webgl");

	string unityEditorLocation;
	if (!TryGetUnityInstall("Unity 2018.1.0f1 (64-bit)", out unityEditorLocation)) 
	{
		Error("Failed to find 'Unity 2018.1.0f1 (64-bit)' install location");
		return;
	}
	
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.WebGL,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true
	};
	
	BuildUnity3DProject(projectPath, options);
});

RunTarget(target);
