using System;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWebpackManifest
{
    public static class DiExtensions
    {

        public static void UseWebpackManifest(this IServiceCollection services)
        {
            services.UseWebpackManifest(c => new WebpackConfig());
        }

        public static void UseWebpackManifest(this IServiceCollection services, Action<WebpackConfig> config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (config == null) throw new ArgumentNullException(nameof(config));
 
            services.Configure<WebpackConfig>(config);
            services.AddSingleton<WebpackManifest>();
        }

    }
}
