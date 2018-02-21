using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreWebpackManifest
{
    public class WebpackManifest
    {
        private WebpackConfig _config;
        private Dictionary<string, string> _json = null;
        
        public WebpackManifest(WebpackConfig config)
        {
            _config = config;
        }

        public void Read()
        {
            _json = JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadManifest());
        }

        private string ReadManifestFromDev()
        {
            // try to read from node web server
            using (HttpClient client = new HttpClient())
            {
                try 
                {
                    var resp = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _config.GerManifestUrl())).Result;
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
                        $"Request URL: {uri}.\n" +
                        "Please run \"npm run dev\" and try again.\n" + 
                        $"Details: \"{e.Message}\""
                    );
                }  
            }
        }

        private string ReadFromBuild()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), MANIFEST);
            if (!File.Exists(path)) 
            {
                throw new Exception(
                    $"Missing manifest file: {path}.\n" + 
                    "Please check your build output path configuration, and try again\n"
                );
            }
            return File.ReadAllText(path);
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

        public string[] GetPaths(params string[] paths)
        {
            return paths.Select(p => GetPath(p)).ToArray();
        }

        public string GetPath(string name)
        {
            string path = "";
            if (_json != null && _json.TryGetValue(name, out path)) 
            {
                return _isDev ? DEVSERVER + path : path;
            }
            throw new Exception(
                $"Could not locate file \"{name}\" in manifest.\n" + 
                "Please check your build output path configuration, and try again."
            );
        }
    }
}