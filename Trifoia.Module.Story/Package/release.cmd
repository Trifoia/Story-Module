del "*.nupkg"
"..\..\oqtane.framework\oqtane.package\nuget.exe" pack Trifoia.Module.Story.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework\Oqtane.Server\Packages\" /Y

