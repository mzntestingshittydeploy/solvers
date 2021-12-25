# Solvers Service
This service provides a list of solvers available to run on data sets.
## Prerequisites 
* Docker
* Docker-compose
* Visual Studio 2022

## Quick Start
Instead of installing all the infrastructure on your host machine, you may run it as a dockerized containers.

```sh
docker-compose up -d
```
Then open the solution in Visual Studio 2022 and debug with the "Solvers"-profile. Swagger should automatically open in your default browser.

When you are done, terminate your infrastructure
```sh
docker-compose down
```

## Development
Here are a few tips and tricks to help you developing

### RabbitMq
After booting your infrastructure, you can access the RabbitMq Management at [http://localhost:15672](http://localhost:15672)

username: guest
password: guest

## Deploy