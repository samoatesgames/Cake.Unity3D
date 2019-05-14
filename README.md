# Cake.Unity3D

## About

Unity3D build support for Cake (https://github.com/cake-build/cake).

### Methods

```csharp
/// <summary>
/// Build a provided Unity3D project with the specified build options.
/// </summary>
/// <param name="context">The active cake context.</param>
/// <param name="projectFolder">The absolute path to the Unity3D project to build.</param>
/// <param name="options">The build options to use when building the project.</param>
public static void BuildUnity3DProject(this ICakeContext context, FilePath projectFolder, Unity3DBuildOptions options)
```

```csharp
/// <summary>
/// Locate all installed version of Unity3D.
/// Warning: This currently only works for Windows and has only been tested on Windows 10.
/// </summary>
/// <param name="context">The active cake context.</param>
/// <returns>A dictionary containing 'key' Unity version, 'value' absolute install path</returns>
public static Dictionary<string, string> GetAllUnityInstalls(this ICakeContext context)
```

```csharp
/// <summary>
/// Try and get the absolute install path for a specific Unity3D version.
/// </summary>
/// <param name="context">The active cake context.</param>
/// <param name="version">The version to try and locate.</param>
/// <param name="installPath">If found the absolute install path will be written to this out variable</param>
/// <returns>True if the editor version was found, false otherwise.</returns>
public static bool TryGetUnityInstall(this ICakeContext context, string version, out string installPath)
```

```csharp
/// <summary>
/// Try and get the absolute install path for a specific Unity3D version.
/// </summary>
/// <param name="context">The active cake context.</param>
/// <param name="projectPath">The absolute path to the Unity3D project we want to get the Unity3D version for.</param>
/// <param name="unityVersion">If found the name of the Unity3D version used for the project.</param>
/// <returns>True if the editor version was found, false otherwise.</returns>
public static bool TryGetUnityVersionForProject(this ICakeContext context, string projectPath, out string unityVersion)
```

## Example

```csharp
#addin nuget:?package=Cake.Unity3D

var target = Argument("target", "Build");

Task("Build")
  .Does(() =>
{
	// Presuming the build.cake file is within the Unity3D project folder.
	var projectPath = System.IO.Path.GetFullPath("./");
	
	// The location we want the build application to go
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "x64", "example.exe");
	
	// Get the version of Unity used by the Unity project
	string unityEditorVersion;
	if (!TryGetUnityVersionForProject(projectPath, out unityEditorVersion))
	{
		Error($"Failed to find Unity version for the project '{projectPath}'");
		return;
	}
	
	// Get the absolute path to the Unity3D editor.
	string unityEditorLocation;
	if (!TryGetUnityInstall(unityEditorVersion, out unityEditorLocation)) 
	{
		Error($"Failed to find '{unityEditorVersion}' install location");
		return;
	}
	
	// Create our build options.
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.StandaloneWindows64,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true,
		BuildVersion = "1.0.0"
	};
	
	// Perform the Unity3d build.
	BuildUnity3DProject(projectPath, options);
});

RunTarget(target);
```

## Supported Platforms

### Executing Platforms

Currently the addon only works on Windows and has only been tested on Windows 10.
This is due to how the 'TryGetUnityInstall' works. What it does is look at all shortcuts in your Windows start menu and locate
the Unity3D installs from that. In theory, if you just set the 'UnityEditorLocation' build option to the absolute path to
your Unity3D install on Mac or Linux, this should also work.

### Build Platforms

* StandaloneWindows64
* StandaloneWindows
* WebGL

In theory, all platforms that the executing platform supports should work. However, I haven't test any apart from the above list.
