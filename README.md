# Spectre.Query

[![NuGet](https://img.shields.io/nuget/v/Spectre.Query.svg)](https://www.nuget.org/packages/Spectre.Query) ![Build Status](https://ci.appveyor.com/api/projects/status/8yx4dod2p6nep3l7/branch/develop?svg=true)

Spectre.Query is a library for doing simplified (safe) querying in Entity Framework Core. Perfect when you want to let end users or APIs search with a SQL-esque language without actually letting them execute any SQL directly (which you never should).

```
ID > 0 AND Year < 2007 AND Comment != null AND !Seen
```

## Table of Contents

1. [Introduction](#introduction)
1. [Known issues](#known-issues)
1. [Usage](#usage)
1. [Usage (ASP.NET Core)](#usage-aspnet-core)
1. [License](#license)

## Introduction

The query engine was originally implemented very quick and dirty for an internal application, and before open sourcing it, the code have been somewhat cleaned up. However, there are still things that will need to be improved. Some functionality was also removed due to the hackish nature of the implementation. See known issues below for more information.

This project is currently under active development and might not be ready for production.

## Known issues

* Navigation properties are not supported. For more complex queries, use [Query Types](https://github.com/aspnet/EntityFramework.Docs/blob/master/entity-framework/core/modeling/query-types.md).

* There's currently no support for non-integer numbers.

## Usage

### 1. Install the NuGet package

```
PM> Install-Package Spectre.Query
```

### 2. Create the query provider

```csharp
var provider = QueryProviderBuilder.Build(context, options =>
{
    options.Configure<Movie>(movie =>
    {
        movie.Map("Id", e => e.MovieId);
        movie.Map("Title", e => e.Name);
        movie.Map("Year", e => e.ReleasedAt);
        movie.Map("Score", e => e.Rating);
        movie.Map("Seen", e => e.Seen);
    });
});
```

The created `IQueryProvider<TContext>` is thread safe and
can be cached for the duration of the application.

### 3. Query the database

```csharp
var movies = provider.Query<Movie>(context, "NOT Seen AND Score > 60").ToList();
```

## Usage (ASP.NET Core)

### 1. Install the NuGet package

```
PM> Install-Package Spectre.Query.AspNetCore
```

### 2. Register the query provider

Start by adding the registrations in your `Startup.cs`.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddQueryProvider<MovieDbContext>(options =>
    {
        options.Entity<Movie>(movie =>
        {
            movie.Map("Id", e => e.MovieId);
            movie.Map("Title", e => e.Name);
            movie.Map("Year", e => e.ReleasedAt);
            movie.Map("Score", e => e.Rating);
            movie.Map("Seen", e => e.Seen);
        });
    });

    // ...
}
```

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // This is not required, but will make sure that all
    // initialization is performed at start up and not at
    // the first time the query provider is used.
    app.UseQueryProvider<MovieDbContext>();

    // ...
}
```

### 3. Query the database

```csharp
[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IQueryProviderSession<MovieContext> _provider;

    public MovieController(IQueryProviderSession<MovieContext> provider)
    {
        _provider = provider;
    }

    [HttpGet]
    public IActionResult<List<Movie>> Query([FromHeader]string query = "Rating > 80 AND !Seen")
    {
        return _provider
            .Query<Movie>(query)
            .OrderByDescending(x => x.Rating)
            .ToList();
    }
}
```

## License

Copyright © Spectre Systems

Spectre.Query is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/spectresystems/spectre.query/blob/develop/LICENSE).