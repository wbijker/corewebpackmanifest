Utility library and taghelper to bridge the gap between dotnet core and webpack. 

You need to use the Webpack manigest plugin (https://www.npmjs.com/package/webpack-manifest-plugin) in your webpack build process.

To use this service, register the webpack manifest service and provide some options:
Startup.cs
```cs
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.UseWebpackManifest(config => {
                config.BuildDirectory = "wwwroot";
                config.DevServer = new Uri("http://localhost:8080/webpack");
                config.Manifest = "manifest.json";
                config.Usage = WebpackMode.AUTODETECT;
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // ....
        }
    }
```

Register taghelpers. Either at the top of your view or in _ViewImports.cshtml:
```cshtml
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, CoreWebpackManifest
```

Then in your view use the webpack tag helpers to help you with the format of the paths of your style and script sources:
```cshtml
<html>
<head>
    <link webpack-chunk="global" />  
</head>
<body>
    <div id="app">
    </div>

    <script webpack-chunk="vendor"></script>
    <script webpack-chunk="common"></script>
    <script webpack-chunk="app"></script>
</body>
</html>
```

Generated HTML if development server is reachable and running:
```html
<html>
<head>
    <link href="http://localhost:8080/webpack/global.css" rel="stylesheet" />
</head>
<body>
    <div id="app">
    </div>

    <script src="http://localhost:8080/webpack/vendor.js"></script>
    <script src="http://localhost:8080/webpack/common.js"></script>
    <script src="http://localhost:8080/webpack/app.js"></script>
</body>
</html>
```

Otherwise production mode:
```html
<html>
<head>
    <link href="/wwwroot/dist/global.12da.css" rel="stylesheet" />
</head>
<body>
    <div id="app">
    </div>

    <script src="/wwwroot/dist/vendor.8kfn.js"></script>
    <script src="/wwwroot/dist/common.nmo1js"></script>
    <script src="/wwwroot/dist/app.kk2q.js"></script>
</body>
</html>
```
