# Basic - Front

This project provides the front end of the `basic` application.

# How to use it?

This project can be run via docker or by downloading and running the code.

## Docker

The basis are:
`docker run -e API_ROOT_URL="https://api.example.com" -p:80:80 ghcr.io/oxybot/basic/front:latest`

With this configuration, the front app will be served on the port 80.

Complete list of the accepted environment variables:

| Name | Status | Usage |
|------|--------|-------|
| `API_ROOT_URL` | Mandatory | Defined the root url of the associated api service (basic-api) |
| `NGINX_SERVER_NAME` | Mandatory | Server name |
| `NGINX_SSL_CERTIFICATE` | Optional | Enable HTTPS on 443 |
| `NGINX_SSL_CERTIFICATE_KEY` | Optional | Enable HTTPS on 443 |

## Build and Run

The project requires (yarn)[] to run.

`yarn` - install the dependencies

`yarn start` - starts the project

To update the environment variables, create a `.env.development.local` file and add your specific configuration.
The `.env.development` file contains the possible environment variables for the project.
