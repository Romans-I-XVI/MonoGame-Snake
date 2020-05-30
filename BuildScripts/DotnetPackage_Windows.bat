set starting_dir=%cd%
cd /d %~dp0
cd ..\
dotnet publish Snake.sln -c Release --self-contained true /p:PublishSingleFile=true -r win-x64
dotnet publish Snake.sln -c Release --self-contained true /p:PublishSingleFile=true -r win-x86
cd %starting_dir%
pause
