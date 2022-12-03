# 5S Audit App

## Overview

5S audits promote more efficient operations. In simple terms, 5S methodology helps keep the workplace organized and clean. 
This app allows you perform an audit to ensure the 5S approach is followed.

This application is still in a early stage of development. See [Roadmap](#roadmpap).

### Features

* Create and browse audits
* API: JWT authentication with refresh tokens
* API: Paging, sorting
* API: Security headers

### Built with

* [React.js](https://reactjs.org/) - frontend (client)
* [.NET](https://dotnet.microsoft.com/en-us/download) - backend (API)

## Getting started

### Prerequisites

Make sure you have installed the following prerequisites:

* [VS Code](https://code.visualstudio.com/) _(optional but preferred)_

* [SQLiteStudio](https://sqlitestudio.pl/) _(or other software to browse SQLite database)_

* [Git](https://git-scm.com/downloads)

```sh
git --version
# => git version 2.34.0.windows.1
```

* [Node.js](https://nodejs.org/en/download/)

```sh
node --version
# => v16.13.0

npm --version
# => 8.1.3
```

* [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet)

```sh
dotnet --version
# => 5.0.404
```

* [EF Core CLI tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

```sh
dotnet ef --version
# => Entity Framework Core .NET Command-line Tools
# => 6.0.1
```

### Installation

1. Clone the repository
```sh
git clone https://github.com/schwastek/5s-audit-app.git
```
2. Install npm packages
```sh
cd client
npm install
```
3. Set `secrets.json` file for API (see [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) tool). For example:
```json
{
  "ConnectionStrings:DefaultConnection": "audits.db",
  "Jwt:TokenKey": "My secret key to sign JWT"
}
```
Secret Manager works only in `Development` mode. For `Staging` mode (tests), you need `appsettings.Staging.json` in the `Api` project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "audits-tests.db"
  },
  "Jwt": {
    "TokenKey": "My secret key to sign jwt"
  }
}
```

### Running application

When you start ASP.NET Core web API, it won't launch React client app automatically. **Both frontend and backend must be started manually.**

Run React frontend app:

```sh
cd client
npm start
```

You should see console output similar to the following:

```sh
Compiled successfully!

You can now view client in the browser.

  Local:            http://localhost:3000        
  On Your Network:  http://192.168.56.1:3000     

Note that the development build is not optimized.
To create a production build, use npm run build.
```

Run ASP.NET Core web API:

```sh
cd Api
dotnet watch run
```

The console output shows messages similar to the following:

```sh
Now listening on: https://localhost:5000
Application started. Press Ctrl+C to shut down.
Hosting environment: Development
Content root path: C:\5sAuditApp\Api
```

To determine the environment, ASP.NET Core reads `ASPNETCORE_ENVIRONMENT` value from [`Api/Properties/launchSettings.json`](./Api/Properties/launchSettings.json) file.
Environment values set in `launchSettings.json` override values set in the system environment.

To set the `ASPNETCORE_ENVIRONMENT` for the current shell session only, use the following commands:

```sh
set ASPNETCORE_ENVIRONMENT=Staging
dotnet watch run --no-launch-profile
```

See [Use multiple environments in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-5.0) for more information.

Note: Sensitive data logging is enabled in every environment (although in the future, it should only be enabled in development environment).

In a web browser, navigate to http://localhost:3000.

Log in with email and password:

* **`john@test.com`**
* **`Pa$$w0rd`**

## Usage

Log in

![Login page](./docs/img/login-page.png)

Create new audit

![New audit](./docs/img/new-audit-page.png)

Browse audits

![Browse audits](./docs/img/browse-audits-page.png)

## Documentation

### API

Please refer to the [API docs](./docs/API.md).

### Database

SQLite database file is created in the special "local" folder for your platform.  
Use Run command below (`WIN+R`) to open the "local" folder on Windows: `C:\Users\{Username}\AppData\Local`.

```
%LocalAppData%
```

Database is seeded in `Development` mode.

### Common commands

```sh
# Add migration
dotnet ef migrations add MyMigration

# Apply migration
dotnet ef database update
```

## Roadmap

- [x] Create audits
- [x] Browse audits
- [x] Login
- [x] Audit actions
- [ ] Registration
- [ ] Action comments
- [ ] Image upload
- [x] Swagger (OpenAPI) documentation
- [x] Unit / Integration tests
- [ ] TypeScript
- [ ] Cache

## License

Distributed under the MIT License. See [`LICENSE.txt`](./LICENSE.txt) for more information.

## Acknowledgments

Resources I find helpful and would like to give credit to:

* Dockx, K. (2019). _Building a RESTful API with ASP.NET Core 3_. Pluralsight. - [Course](https://www.pluralsight.com/courses/asp-dot-net-core-3-restful-api-building) • [GitHub](https://github.com/KevinDockx/BuildingRESTfulAPIAspNetCore3)
* Dockx, K. (2020). _Implementing Advanced RESTful Concerns with ASP.NET Core 3_. Pluralsight. - [Course](https://www.pluralsight.com/courses/asp-dot-net-core-3-advanced-restful-concerns) • [GitHub](https://github.com/KevinDockx/ImplementingAdvancedRESTfulConcernsAspNetCore3)
* Resca, S. (2019). _Hands-On RESTful Web Services with ASP.NET Core 3_. Packt Publishing. - [Book](https://www.packtpub.com/product/hands-on-restful-web-services-with-asp-net-core-3/9781789537611) • [GitHub](https://github.com/PacktPublishing/Hands-On-RESTful-Web-Services-with-ASP.NET-Core-3)
* Rippon, C. (2019). _ASP.NET Core 3 and React_. Packt Publishing. - [Book](https://www.packtpub.com/product/asp-net-core-3-and-react/9781789950229) • [GitHub](https://github.com/PacktPublishing/ASP.NET-Core-3-and-React)
* Cummings, N. (2019). _Complete guide to building an app with .Net Core and React_. Udemy. - [Course](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react/) • [GitHub](https://github.com/TryCatchLearn/Reactivities)
