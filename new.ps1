if ( $args.Length -lt 1) {
    Write-Error "Supply a name for your new project"
}
$dir = $args[0]

mkdir $dir
Push-Location $dir
dotnet new console 
dotnet sln .. add .
Remove-Item Program.cs
Copy-Item ../template/*.cs . 
dotnet build --no-restore

Write-Host "$dir created, happy coding !"
