#!/bin/bash

echo "Publishing app..."

dotnet publish ./src/ResolveBridge.Api/ResolveBridge.Api.csproj \
-c Release \
-o ./publish

echo "Uploading to SmarterASP.NET..."

lftp -u "thethreeshub-001","Rosemond123@" win1005.site4now.net <<EOF
mirror -R ./publish /site/wwwroot
bye
EOF

echo "Deployment complete!"