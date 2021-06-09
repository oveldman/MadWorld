FROM ubuntu:latest

RUN apt-get update
RUN apt-get -y install gnupg
RUN apt-get -y install curl
RUN curl -sSL https://packages.microsoft.com/config/ubuntu/20.04/prod.list | tee /etc/apt/sources.list.d/microsoft-prod.list
RUN curl -sSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN apt-get update
RUN apt-get install blobfuse
RUN sh azure-storage-fuse/build.sh
RUN mkdir /mnt/resource/blobfusetmp -p
RUN touch ~/fuse_connection.cfg
RUN echo "accountName ${ACCOUNTNAME}" >> ~/fuse_connection.cfg
RUN echo "accountKey ${ACCOUNTKEY}" >> ~/fuse_connection.cfg
RUN echo "containerName ${CONTAINERNAME}" >> ~/fuse_connection.cfg
RUN mkdir ${CONTAINERNAME}
RUN blobfuse ~/mycontainer --tmp-path=/mnt/resource/blobfusetmp  --config-file=/path/to/fuse_connection.cfg -o attr_timeout=240 -o entry_timeout=240 -o negative_timeout=120