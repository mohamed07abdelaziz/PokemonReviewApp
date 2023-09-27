# PokemonReviewApp

Welcome to the Pokemon Review App! This application allows users to review and categorize Pokemon, with each Pokemon having its own category and owner. It also incorporates JWT tokens and Docker containers for database deployment. The application is built using .NET with Code First and follows Object-Oriented Programming (OOP) principles.

## Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Docker](#docker)
- [Authentication](#authentication)
- [Contributing](#contributing)
- [License](#license)

## Features

- Create, read, update, and delete Pokemon.
- Assign Pokemon to specific categories and owners.
- Allow reviewers to add and manage reviews for each Pokemon.
- Implement JWT token-based authentication with refresh tokens.
- Use Docker containers for easy database deployment.
- Follow best practices of .NET development with Code First and OOP.

## Prerequisites

Before you can run the Pokemon Review App, make sure you have the following prerequisites installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (Version X.X.X or higher)
- [Docker](https://www.docker.com/get-started)
- [Git](https://git-scm.com/downloads)

## Getting Started

1. Clone the repository to your local machine:

bash
git clone https://github.com/yourusername/pokemon-review-app.git
cd pokemon-review-app


2. Create a `appsettings.json` file in the project root directory and configure your database connection:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=DESKTOP-AA9MG2L\\SQLEXPRESS;Initial Catalog=DBP1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWT": {

    "key": "szja9vrCKxNdMYMlWsFvfe/gaBm1pTixYZWHesjvGBk=",
    "Issuer": "SecureApi",
    "Audience": "SecureApiUser",
      "DurationInDays": 30
  }
}



3. Build and run the application:

bash
dotnet build
dotnet run


Your app will be available at `http://localhost:5000`.

## Usage

Describe how users can use your application. Include example API requests if applicable. 

## API Documentation

You can find detailed API documentation [here](link-to-api-docs).

## Docker

To deploy the database using Docker, follow these steps:

1. Build the Docker container:

bash
docker build -t pokemon-db .


2. Run the Docker container:

bash
docker run -d -p 8002:1433 --name pokemon-database pokemon-db


The database will be available on port 8001.

## Authentication

The Pokemon Review App uses JWT tokens for authentication. Here's how it works:

- Users can register and log in to obtain access tokens.
- Access tokens expire after a certain period.
- Refresh tokens can be used to obtain new access tokens without requiring the user to log in again.

## Contributing

If you'd like to contribute to this project, please follow our [Contributing Guidelines](CONTRIBUTING.md).

## License

This project is licensed under the [MIT License](LICENSE).


Make sure to replace placeholders like YourConnectionStringHere, YourSecretKey, YourIssuer, YourAudience, and the other placeholders with actual values and update any links to match your project's structure. Also, consider adding more specific details and instructions as needed for your application.
