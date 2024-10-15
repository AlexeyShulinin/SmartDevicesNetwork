# SmartDevicesNetwork API

This project provides API to smart device nodes and network emulation

## Create release build:

```
dotnet publish -c Release
```

## Docker Image build:

```
docker build -t smart-devices-network-api .
```

## Docker Image run:

```
docker run -p 9010:8080 smart-devices-network-api
```
