rem msbuild Nkf.Net-2.0.sln /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v2.0
msbuild proj_4.0\Nkf.Net-4.0.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.0
msbuild proj_4.5\Nkf.Net-4.5.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.5
msbuild proj_4.5.1\Nkf.Net-4.5.1.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.5.1
msbuild proj_std2.0\Nkf.Net.Core.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=2.0
msbuild proj_std2.0\Nkf.Net.Core.sln /t:pack /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg /p:Configuration=Release;TargetFrameworkVersion=2.0

nuget pack Nkf.Net.nuspec
