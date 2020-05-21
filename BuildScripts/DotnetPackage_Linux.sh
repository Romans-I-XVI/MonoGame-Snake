cd $(dirname "$0")/..
dotnet publish Snake.sln -c Release --self-contained true -p:PublishSingleFile=true -r linux-x64
