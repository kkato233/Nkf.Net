cd proj_std2.0
dotnet build
cd ..

msbuild proj_4.0\Nkf.Net-4.0.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=v4.8 /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg
msbuild proj_std2.0\Nkf.Net.Core.sln /t:Rebuild  /p:Configuration=Release;TargetFrameworkVersion=2.0 /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg

if "%SIGN_FINGERPRINT%"=="" GOTO SKIP_SIGN

signtool sign /n %SIGN_NAME% /tr http://timestamp.comodoca.com/?td=sha256 /td sha256 /fd sha256 /ph /as /sha1 %SIGN_FINGERPRINT%  proj_4.0\bin\Release\Nkf.Net.dll
signtool sign /n %SIGN_NAME% /tr http://timestamp.comodoca.com/?td=sha256 /td sha256 /fd sha256 /ph /as /sha1 %SIGN_FINGERPRINT%  proj_4.0\bin\Release\Nkf.Net.pdb
signtool sign /n %SIGN_NAME% /tr http://timestamp.comodoca.com/?td=sha256 /td sha256 /fd sha256 /ph /as /sha1 %SIGN_FINGERPRINT%  proj_std2.0\bin\Release\netstandard2.0\Nkf.Net.dll
signtool sign /n %SIGN_NAME% /tr http://timestamp.comodoca.com/?td=sha256 /td sha256 /fd sha256 /ph /as /sha1 %SIGN_FINGERPRINT%  proj_std2.0\bin\Release\netstandard2.0\Nkf.Net.pdb

:SKIP_SIGN

nuget pack Nkf.Net.nuspec
nuget pack Nkf.Net.nuspec -Symbols -SymbolPackageFormat snupkg

if "%SIGN_FINGERPRINT%"=="" GOTO SKIP_SIGN_2
nuget sign *.nupkg  -Timestamper http://timestamp.comodoca.com/?td=sha256 -CertificateFingerprint %SIGN_FINGERPRINT%
nuget sign *.snupkg  -Timestamper http://timestamp.comodoca.com/?td=sha256 -CertificateFingerprint %SIGN_FINGERPRINT%

:SKIP_SIGN_2
