Remove-Item ..\QuickLook.Plugin.GraphvizDotViewer.qlplugin -ErrorAction SilentlyContinue

$files = Get-ChildItem -Path ..\bin\Release\ -Exclude *.pdb,*.xml
Compress-Archive $files ..\QuickLook.Plugin.GraphvizDotViewer.zip
Move-Item ..\QuickLook.Plugin.GraphvizDotViewer.zip ..\QuickLook.Plugin.GraphvizDotViewer.qlplugin