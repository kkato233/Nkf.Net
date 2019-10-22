cd proj_std2.0
dotnet build
cd ..
msbuild proj_4.0\Nkf.Net-4.0.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.0
msbuild proj_std2.0\Nkf.Net.Core.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=2.0

nuget pack Nkf.Net.nuspec
