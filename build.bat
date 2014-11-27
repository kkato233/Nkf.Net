msbuild Nkf.Net-2.0.sln /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v2.0
msbuild Nkf.Net-4.0.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.0
msbuild Nkf.Net-4.5.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.5
msbuild Nkf.Net-4.5.1.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.5.1
nuget pack Nkf.Net.nuspec