cd proj_std2.0
dotnet build
cd ..

msbuild proj_4.0\Nkf.Net-4.0.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.8 /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg
msbuild proj_std2.0\Nkf.Net.Core.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=2.0 /t:pack /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg

nuget pack Nkf.Net.nuspec
nuget pack Nkf.Net.nuspec -Symbols -SymbolPackageFormat snupkg

