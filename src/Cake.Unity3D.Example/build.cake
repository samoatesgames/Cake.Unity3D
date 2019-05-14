#reference "../Cake.Unity3D/bin/Debug/net46/Cake.Unity3D.dll"
//#addin nuget:?package=Cake.Unity3D

var target = Argument("target", "Build-Win64");

Task("Build-Win64")
  .Does(() =>
{
	var projectPath = System.IO.Path.GetFullPath("./");
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "x64", "example.exe");
	
	string unityEditorVersion;
	if (!TryGetUnityVersionForProject(projectPath, out unityEditorVersion))
	{
		Error($"Failed to find Unity version for the project '{projectPath}'");
		return;
	}
	
	string unityEditorLocation;
	if (!TryGetUnityInstall(unityEditorVersion, out unityEditorLocation))
	{
		Error($"Failed to find '{unityEditorVersion}' install location, installed versions are:");
		foreach(var version in GetAllUnityInstalls().Keys)
		{
			Error($" - {version}");
		}
		return;
	}
	
	Information($"Using Unity editor located at '{unityEditorLocation}'");
	
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

	string unityEditorVersion;
	if (!TryGetUnityVersionForProject(projectPath, out unityEditorVersion))
	{
		Error($"Failed to find Unity version for the project '{projectPath}'");
		return;
	}
	
	string unityEditorLocation;
	if (!TryGetUnityInstall(unityEditorVersion, out unityEditorLocation))
	{
		Error($"Failed to find '{unityEditorVersion}' install location, installed versions are:");
		foreach(var version in GetAllUnityInstalls().Keys)
		{
			Error($" - {version}");
		}
		return;
	}
	
	Information($"Using Unity editor located at '{unityEditorLocation}'");
	
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
