# Introduction

Provides simple tools for small and medium companies.

Current features:
- Holidays and working time management
- Contract management

# Run the project

## Using docker or docker-compose

Basic is composed of two main projects:
- The web front: `ghcr.io/oxybot/basic/front`
- The api backend: `ghcr.io/oxybot/basic/api`

## front configuration

`NGINX_SERVER_NAME` - Mandatory

The domain associated with the front project

`NGINX_SSL_CERTIFICATE` - Mandatory

The absolute path to the ssl certificate.

`NGINX_SSL_CERTIFICATE_KEY` - Mandatory

The absolute path to the private key of the ssl certificate.

`API_ROOT_URL` - Mandatory

The public url associated with the api project.

## api configuration

The API project is developed in .net core and can be configured via various methods and in particular environment variables and configuration file (`appsettings.json`).

`BASEURL` - Mandatory

The public url associated with the api project.

`CORS__ORIGINS` - Mandatory

The urls of the websites that are authorized to call the API. The values should be separated by commas (`,`).

# Develop the project

The project can be run as a development environment following three methods:
- GitHub Codespace
- devcontainer
- Local development

## GitHub Codespace

The project is configured for Codespace and provides:
- A configured **vscode** environment
- A pre-configuration of the https configuration for both the front and api projects
- The initialization of the supporting services

To be noted:
- By default, the API project port (7268) will be private and can't be used as such. You should configure this port to be public. Private port doesn't support CORS configuration.
- The API will use a Sql Server database by default, but a MySql server is as well setup and you can switch from one to another by changing the `DatabaseDriver` configuration.

## devcontainer

** TO BE TESTED **

## Local development

Run the `setup-https.sh` / `setup.ps1` script to prepare the https configuration for the project.

Depending of your needs, you could need a few additional services (smtp server, database). You could install those elements locally or use the `docker-compose.yml` file to start those elements for you.

Note: The API project will use (and create) by default a LocalDB instance as its datasource - which will works only on Windows.

## Steps to add a new entity

### Add the model class and create the database structure

Key project: **Basic.DataAccess**

1. Define the Model class inside the Basic.Model project
	- Should be named according to business usage
	- Should inherit from the `BaseModel` class
1. Add any entity framework specificities inside the Context.OnModelCreation method
1. Run the command to add a migration `dotnet ef migrations add`
1. Review the migration code
1. Run the command to update the database `dotnet ef database update`

### Add the web api elements

Key project: **Basic.WebApi**

1. Create the DTO classes in the DTOs folder (`ForList`, `ForView`, `ForEdit`)
1. Create the controller in the Controllers folder
1. Add the automapper config for the new DTOs in the `MappingProfile` class
