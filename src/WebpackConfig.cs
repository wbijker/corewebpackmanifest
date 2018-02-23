using System;

namespace CoreWebpackManifest
{
    /*
    WebpackMode has to do with where a request webpack chunk lies:
    It can either be served as static content from prebuild webpack
        <script src="dist/app.1230492kad4sd.js"/>
    or it can be served from webpack dev server.
        <script src="http://localhost:8080/webpack/app.js"/>
    */
    public enum WebpackMode
    {
        // Autodetect will use build if buld files is available
        // otherwise will use dev server
        AUTODETECT,
        
        // Always use development server
        DEVSERVER,

        // Always use prebuild files
        BUILD
    }

    public class WebpackConfig
    {
        // Combine schema, host, port ad public path in one Uri
        public Uri DevServer { get; set; }
        
        // Manifest is the name of the file produced from webpack Manifest plugin
        public string Manifest { get; set; }

        public WebpackMode Usage { get; set; }

        // The directory where webpack will output built files
        public string BuildDirectory { get; set; }

        public WebpackConfig()
        {
            // The defaults
            DevServer = new Uri("http://localhost:8080/webpack/");
            Manifest = "manifest.json";
            Usage = WebpackMode.AUTODETECT;
            BuildDirectory = "build";
        }

        public string GetUrl(string path)
        {
            return (new Uri(DevServer, path)).ToString();
        }
    }
}