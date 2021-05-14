#!/bash/sh
#Variables
DOTNET_PROGRAM = '~/dotnet/dotnet'

#Get latest sources and updates
echo "Start script."
apt update
git reset --hard
git pull
$DOTNET_PROGRAM tool update --global dotnet-ef

#update API
echo "Start deploying dotnet API."
systemctl stop kestrel-madworldapi.service
cd MadWorld/API
cp ../../../Settings/appsettings.Development.json .
cp ../../../Settings/appsettings.json .
$DOTNET_PROGRAM restore
$DOTNET_PROGRAM ef database update --context MadWorldContext
$DOTNET_PROGRAM ef database update --context AuthenticationContext
$DOTNET_PROGRAM publish --configuration Release --output ../../../../Published/MadWorld/API
systemctl start kestrel-madworldapi.service
echo "Dotnet API is deployed."

#update Website
echo "Start deploying blazor website."
cd ../Website
rm -r /var/www/html/*
$DOTNET_PROGRAM restore
$DOTNET_PROGRAM publish --configuration Release --output /var/www/html
echo "Blazor website is deployed."
echo "The script is finsihed!"