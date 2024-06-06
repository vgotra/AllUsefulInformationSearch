$directories = Get-ChildItem -Include bin,obj -Recurse -Force -Directory
foreach ($dir in $directories) {
    Write-Output "Deleting $($dir.FullName)"
    Remove-Item -Path $dir.FullName -Recurse -Force -Confirm:$false
}