# SmartDevicesNetwork API

This project provides API to smart device nodes and network emulation

## Create release build and image after release:

```
dotnet build --configuration Release
```

## Docker Image build(manually - no need to trigger after release build):

```
docker build -t smart-devices-network-api .
```

## Docker Image run:

```
docker run -p 9010:8080 smart-devices-network-api
```


## Docker Compose run:

```
docker-compose up -d --build
```