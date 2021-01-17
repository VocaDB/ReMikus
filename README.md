# ReMikus
ASP.NET Core MVC tag helpers for [Laravel Mix](https://laravel-mix.com/) and [Inertia.js](https://inertiajs.com/).

## Installation

```
PM> Install-Package VocaDb.ReMikus
```

## Usage

### Laravel Mix

`Startup.cs`:

```csharp
using VocaDb.ReMikus;

public void ConfigureServices(IServiceCollection services)
{
    services.AddLaravelMix();
}
```

By using `remikus` tag helper:

```cshtml
@addTagHelper *, VocaDb.ReMikus

<remikus path="/css/app.css" />
<remikus path="/js/app.js" />
```

By using dependency injection:

```cshtml
@using VocaDb.ReMikus
@inject LaravelMix LaravelMix

<link rel="stylesheet" href="@LaravelMix.GetVersionedPath("/css/app.css")" />
<script src="@LaravelMix.GetVersionedPath("/js/app.js")"></script>
```

### Inertia.js

`HandleInertiaRequests.cs` (optional):
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VocaDb.ReMikus;

public class HandleInertiaRequests
{
    private readonly RequestDelegate _next;

    public HandleInertiaRequests(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, LaravelMix laravelMix)
    {
        // shared data
        Inertia.SharedProps = new Dictionary<string, object>
        {
            { "appName", "VocaDB" },
        };

        // asset versioning
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(laravelMix.ManifestPath);
        var hash = await md5.ComputeHashAsync(stream);
        Inertia.VersionSelector = () => BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

        await _next(context);
    }
}
```

`Startup.cs`:

```csharp
using VocaDb.ReMikus;

public void ConfigureServices(IServiceCollection services)
{
    services.AddInertia();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseMiddleware<HandleInertiaRequests>();/* optional */
    app.UseInertia();
}
```

`Views/App.cshtml`:

```cshtml
@addTagHelper *, VocaDb.ReMikus
<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
        <remikus path="/css/app.css" />
    </head>
    <body>
        <inertia model="@Model" />
        <remikus path="/js/app.js" />
    </body>
</html>
```

## References

- [Compiling Assets (Mix) - Laravel - The PHP Framework For Web Artisans](https://laravel.com/docs/8.x/mix)
- [inertiajs/inertia-laravel: The Laravel adapter for Inertia.js.](https://github.com/inertiajs/inertia-laravel)
- [Nothing-Works/inertia-aspnetcore: The AspNetCore adapter for Inertia.js. https://inertiajs.com](https://github.com/Nothing-Works/inertia-aspnetcore)
