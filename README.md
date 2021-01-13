# ReMikus
ASP.NET Core MVC tag helpers for Laravel Mix.

## Installation

```
PM> Install-Package VocaDb.ReMikus
```

## Usage

In `Startup.cs`:

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
