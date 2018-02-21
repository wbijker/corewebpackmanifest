using System;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWebpackManifest
{
    public static class DiExtensions
    {
        public static void UseWebpackManifest(this IServiceCollection services, Action<WebpackConfig> config)
        {
            var defaultConfig = new WebpackConfig();
            config.Invoke(defaultConfig);

            services.AddSingleton(new WebpackManifest(defaultConfig));            
        }

    }
}