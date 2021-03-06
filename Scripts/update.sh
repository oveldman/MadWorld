#!/bash/sh
#Get latest sources and updates
echo "Start script."
apt update
git reset --hard
git pull
dotnet tool update --global dotnet-ef

#update API
echo "Start deploying dotnet API."
systemctl stop kestrel-madworldapi.service
cd ../MadWorld/API
cp ../../../Settings/appsettings.Development.json .
cp ../../../Settings/appsettings.json .
dotnet restore
dotnet ef database update --context MadWorldContext
dotnet ef database update --context AuthenticationContext
dotnet publish --configuration Release --output ../../../../Published/MadWorld/API
systemctl start kestrel-madworldapi.service
echo "Dotnet API is deployed."

#update Website
echo "Start deploying blazor website."
cd ../Website
rm -r /var/www/html/*
dotnet restore
dotnet publish --configuration Release --output /var/www/html
echo "Blazor website is deployed."
echo "The script is finsihed!"