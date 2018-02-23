using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreWebpackManifest
{
    public class WebpackManifest
    {
        private IHostingEnvironment _hosting;
        private WebpackConfig _config;
        private Dictionary<string, string> _json = null;
        private string _buildManifestFile;
        private bool _isDev;

        public WebpackManifest(IOptions<WebpackConfig> config ,IHostingEnvironment hosting)
        {
            _config = config.Value;
            _hosting = hosting;

            string buildPath = Path.Combine(_hosting.ContentRootPath, _config.BuildDirectory);
            _buildManifestFile = Path.Combine(buildPath, _config.Manifest);
        
            _isDev = 
                _config.Usage == WebpackMode.DEVSERVER ||
                (_config.Usage == WebpackMode.AUTODETECT && !File.Exists(_buildManifestFile));
        }

        private void Read()
        {
            _json = JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadManifest());
        }

        private string ReadManifestFromDev()
        {
            // try to read from node web server
            using (HttpClient client = new HttpClient())
            {
                var request = _config.GetUrl(_config.Manifest);
                try 
                {
                    var resp = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, request)).Result;
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return resp.Content.ReadAsStringAsync().Result;
                    }
                    throw new Exception($"Http response code {(int)resp.StatusCode}");
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "The webpack hot module reloading manifest file does not exits.\n" +
                        $"Request URL: {request}.\n" +
                        "Please run \"npm run dev\" and try again.\n" + 
                        $"Details: \"{e.Message}\""
                    );
                }  
            }
        }

        private string ReadFromBuild()
        {
            if (!File.Exists(_buildManifestFile)) 
            {
                throw new Exception(
                    "Manifest file not found.\n" +
                    $"Path: {_buildManifestFile}.\n" + 
                    "Please check your build output, and try again"
                );
            }
            return File.ReadAllText(_buildManifestFile);
        }

        private string ReadManifest()
        {
            if (_isDev)
            {
                return ReadManifestFromDev();   
            }
            // otherwise read from the local path. npm run build should output this file
            return ReadFromBuild();
        }

        public string GetPath(string name)
        {
            if (_json == null) 
            {
                Read();
            }
            string path = "";
            if (_json != null && _json.TryGetValue(name, out path)) 
            {
                return _isDev ? _config.GetUrl(path) : path;
            }
            throw new Exception(
                $"Could not locate file \"{name}\" in manifest.\n" + 
                "Please check your build output path configuration, and try again."
            );
        }
    }
}