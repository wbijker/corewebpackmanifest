using Microsoft.Extensions.DependencyInjection;

namespace CoreWebpackManifest
{
    public class WebpackConfig
    {
        // Host, Port and public path is all dev server configurations
        // It should match webpack configuration 
        public string Host { get; set; }
        public int Port { get; set; }
        public string PublicPath { get; set; }
        // Manifest is the name of the file produces from webpack Manifest plugin
        public string Manifest { get; set; }
        
        // Wheter or not to use dev server or build directory
        public bool Development { get; set; }

        // The directory where webpack will dump built files
        public string BuildDirectory { get; set; }

        public WebpackConfig()
        {
            Host = "localhost";
            Port = 8080;
            PublicPath = "webpack";
            Manifest = "manifest.json";
            Development = true;
            BuildDirectory = "build";
        }

        public string GerManifestUrl()
        {
            return $"http://{Host}:{Port}/{PublicPath}/{Manifest}";
        }
    }
}