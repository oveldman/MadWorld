#!/bash/sh

apt update
apt upgrade

# Install nginx
apt install nginx

# Install dotnet 5.0
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
apt-get update
apt-get install -y apt-transport-https
apt-get update
apt-get install -y dotnet-sdk-5.0
dotnet tool install --global dotnet-ef

# Install Let's encrypt
apt install snapd
snap install core 
snap refresh core
snap install --classic certbot
ln -s /snap/bin/certbot /usr/bin/certbot
#certbot --nginx (Only when everything is ready!)

# Docker
apt-get update
apt-get install apt-transport-https ca-certificates 
apt-get install curl gnupg lsb-release
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo \
  "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null
apt-get update
apt-get install docker-ce docker-ce-cli containerd.io

# Postgres Docker
docker pull postgres
docker run --name mad-world-db -e POSTGRES_PASSWORD=notmyrealpassword -e POSTGRES_DB=MadWorldDB -d -p 8080:5432 postgres
docker run --name auth-mad-world-db -e POSTGRES_PASSWORD=notmyrealpassword -e POSTGRES_DB=AuthenticationMadWorldDB -d -p 8081:5432 postgres

# Jeager Docker
docker run -d --name jaeger \
  -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 \
  -p 5775:5775/udp \
  -p 6831:6831/udp \
  -p 6832:6832/udp \
  -p 5778:5778 \
  -p 16686:16686 \
  -p 14268:14268 \
  -p 14250:14250 \
  -p 9411:9411 \
  jaegertracing/all-in-one:1.23

# Libgdiplus
apt-get update 
apt-get install -y --no-install-recommends libgdiplus libc6-dev
apt-get clean
rm -rf /var/lib/apt/lists/*