# Kittitas - Roslyn compiler host

Kittitas is a dotnet global tool that hosts the [Roslyn](https://github.com/dotnet/roslyn) and [MSBuild](http://github.com/dotnet/msbuild) APIs in a single process, making it easier to debug components that run in the roslyn compiler pipeline, such as Analyzers and Source Generators.

![Nuget](https://img.shields.io/nuget/v/Kittitas)
![GitHub](https://img.shields.io/github/license/chsienki/kittitas)

## Installation

Kittitas is distributed as a dotnet global tool. Simply run the following command to install:

```bat
dotnet tool install --global kittitas --version 0.0.4-alpha
```

## Usage

Either pass the name of the project you want to build:

```bat
dotnet kittitas <projectFile.csproj>
```

Or run kittitas from a directory containing a single `.csproj`

```bat
cd projectDir
dotnet kittitas
```

Kittitas supports options that make attaching a debugger easier. Run `dotnet kittitas --help` to see a full list of options.

```bat
Usage:
  Kittitas [options] [<ProjectFile>]

Arguments:
  <ProjectFile>    The project file to build, or empty to build from the current directory. [default: ]

Options:
  -w, --wait        Waits for a debugger to attach before continuing [default: False]
  -a, --attach      Attempts to attach a debugger before continuing [default: False]
  --version         Show version information
  -?, -h, --help    Show help and usage information
```

## Kittitas.SDK

Kittitas include an [MSBuild SDK](https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2019) designed to make running Kittitas from Visual Studio easier.

To use the SDK, create an empty `.csproj` alongside the project you want to run Kittitas on. Add the following contents into the newly created project file:

```xml
<Project Sdk="Kittitas.SDK/0.0.4-alpha">
  <ItemGroup>
    <LaunchProject Include="YourApp.csproj" />
  </ItemGroup>
</Project>
```

Replacing `YourApp.csproj` with the path to the project you want to run Kittitas with.

This will create a `launchSettings.json` under the hood that is configured to start Kittitas with the specified project. Selecting the newly added project as the startup project will add a `DebugRoslynComponent` debug target.

![DebugRoslynComponent debug target inside Visual Studio 2019](./img/debug_target.jpg)

This will run Kittitas against the specified project, without actually building anything first, allowing you to debug the build process itself.

## FAQs

**Whats the difference between this and `dotnet build` / `mbuild.exe` / `csc.exe`**: Kittitas isn't designed to replace builds performed by the regular tools, but as a supplementary tool to make it easier to debug components that run inside of them.

**Where is the output?**: Kittitas doesn't currently produce any output, all compilation is performed in-memory.

**Why are builds slower?**: Kittitas doesn't use the compiler server or parallel build nodes; by design everything runs in a single process to make debugging easier.

**My build failed under Kittitas**: Kittitas uses the MSBuild and Roslyn APIs with defaults and without any particular intelligence. Customized or complicated projects may cause the build to fail. Please file an issue or PR if you come across a project that fails under Kittitas.

**Why the custom SDK / empty project?**: Visual Studio will build an out of date project before launching it in the debugger. Roslyn components (especially source generators) can themselves cause build failures, leading to a chicken and egg situation where you want to debug the failing component but can't start Kittitas because of the failure you want to debug. The Kittitas SDK removes all build targets from the empty project, meaning it will always successfully 'build' instantaneously, and allow you to debug the build of the project you're actually interested in.

**Whats with the name?**: [Kittitas](https://en.wikipedia.org/wiki/Kittitas_County,_Washington) is the name of the county in WA that contains [Roslyn](https://en.wikipedia.org/wiki/Roslyn,_Washington). Get it?
