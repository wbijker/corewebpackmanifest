using Microsoft.Extensions.DependencyInjection;

namespace CoreWebpackManifest
{
    /*
    Usage has to do with where the a request webpack chunk lies:
    -It can either be served as static content from prebuild webpack
        <script src="dist/app.1230492kad4sd.js"/>
    -or it can be served from webpack dev server.
        <script src="http://localhost:8080/webpack/app.js"/>
    */
    public enum WebpackConfigUsage
    {
        // Autodetect will use build if buld files is available
        // otherwise will use dev server
        AUTODETECT,
        
        // IHostingEnvironment will be used to check if app runs as dev or build
        ENVIRONMENT,

        // Always use development server
        DEVSERVER,

        // Always use prebuild files
        BUILD
    }

    public class WebpackConfig
    {
        // Host, Port and public path is all dev server configurations
        // It should match webpack configuration 
        public string Host { get; set; }
        public int Port { get; set; }
        public string PublicPath { get; set; }
        // Manifest is the name of the file produces from webpack Manifest plugin
        public string Manifest { get; set; }

        public WebpackConfigUsage Usage { get; set; }

        // The directory where webpack will output built files
        public string BuildDirectory { get; set; }

        public WebpackConfig()
        {
            Host = "localhost";
            Port = 8080;
            PublicPath = "webpack";
            Manifest = "manifest.json";
            Usage = WebpackConfigUsage.AUTODETECT;
            BuildDirectory = "build";
        }

        public string GerManifestUrl()
        {
            return $"http://{Host}:{Port}/{PublicPath}/{Manifest}";
        }
    }
}