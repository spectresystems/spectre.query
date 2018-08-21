# Spectre.Query

[![NuGet](https://img.shields.io/nuget/v/Spectre.Query.svg)](https://www.nuget.org/packages/Spectre.Query) ![Build Status](https://ci.appveyor.com/api/projects/status/8yx4dod2p6nep3l7/branch/develop?svg=true)

Spectre.Query is a library for doing simplified (safe) querying in Entity Framework Core. Perfect when you want to let end users or APIs search with a SQL-esque language without actually letting them execute any SQL directly (which you never should).

```
ID > 0 AND Year < 2007 AND Comment != null AND (!Seen OR Comment LIKE '%Awesome%')
```

This project is currently under active development and might not be ready for production.

## Table of Contents

1. [Usage](#usage)
1. [Usage (ASP.NET Core)](#usage-aspnet-core)
1. [License](#license)

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
        movie.Map("Genre", e => e.Genre.Name);
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
        options.Configure<Movie>(movie =>
        {
            movie.Map("Id", e => e.MovieId);
            movie.Map("Genre", e => e.Genre.Name);
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
    private readonly MovieContext _context;
    private readonly IQueryProvider<MovieContext> _provider;

    public MovieController(MovieContext context, IQueryProvider<MovieContext> provider)
    {
        _context = context;
        _provider = provider;
    }

    [HttpGet]
    public IActionResult<List<Movie>> Query([FromHeader]string query = "Rating > 80 AND !Seen")
    {
        return _provider.Query<Movie>(_context, query)
            .OrderByDescending(movie => movie.Rating)
            .ToList()
    }
}
```

## License

Copyright © Spectre Systems

Spectre.Query is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/spectresystems/spectre.query/blob/develop/LICENSE).