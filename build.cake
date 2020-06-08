#addin "Cake.Docker"

var target = Argument("target", "Run");

Task("Build")
    .Does(() =>
{
	var settings = new DockerComposeBuildSettings
	{
		Files = new []{ "docker-compose.yml", "docker-compose.override.yml" }
	};
	DockerComposeBuild("library.presentation.mvc", "library.api", "library.identity.service");
});

Task("Run")
	.IsDependentOn("Build")
    .Does(() =>
{
	var settings = new DockerComposeUpSettings 
	{
		ForceRecreate = true,
		Files = new []{ "docker-compose.yml", "docker-compose.override.yml" },
		DetachedMode = true 
	};
	DockerComposeUp(settings);	
});

Task("Build-UnitTests")	
    .Does(() =>
{
	DotNetCoreBuild("./tests/Library.Domain.UnitTests/Library.Domain.UnitTests.csproj");
});

Task("RunTests")
	.IsDependentOn("Build-UnitTests")
    .Does(() =>
{
	DotNetCoreTest("./tests/Library.Domain.UnitTests/Library.Domain.UnitTests.csproj");
});

RunTarget(target);