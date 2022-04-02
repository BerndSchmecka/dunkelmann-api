using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;

namespace DunkelmannAPI {

    struct PlatformURL {
        public string url {get; private set;}
        public string platform_name {get; private set;}

        public PlatformURL(string platform_name){
            this.url = $"https://update.teamspeak.com/{platform_name}/x64/latest/info.json";
            this.platform_name = platform_name;
        }
    }

    class ts5version : IEndpoint {
        static PlatformURL[] ts5_version_urls = {new PlatformURL("windows"),
                                           new PlatformURL("linux"),
                                           new PlatformURL("mac")};

        public ts5version() {

        }

        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            List<TS5PlatformInfo> list = new List<TS5PlatformInfo>();
            foreach(PlatformURL purl in ts5_version_urls){
                list.Add(await getPlatformVersionInfo(purl.url, purl.platform_name));
            }
            TS5VersionResponse resp = new TS5VersionResponse(list);

            return new ResponseInfo(JsonConvert.SerializeObject(resp), 200);
        }

        //take a platform_url and a platform_name and return a TS5PlatformInfo
        //use HttpClient to get the json from the url
        //use JsonConvert to deserialize the json
        //return a TS5PlatformInfo
        private async Task<TS5PlatformInfo> getPlatformVersionInfo(string platform_url, string platform_name) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic dGVhbXNwZWFrNTpMRlo2Wl5rdkdyblh+YW4sJEwjNGd4TDMnYTcvYVtbJl83PmF0fUEzQVJSR1k=");
            client.DefaultRequestHeaders.Add("User-Agent", "teamspeak.downloader/1.0");
            HttpResponseMessage response = await client.GetAsync(platform_url);
            TS5PlatformInfo info = new TS5PlatformInfo(JsonConvert.DeserializeObject<TS5VersionInfo>(await response.Content.ReadAsStringAsync()), platform_name, response.Headers);
            return info;
        }
    }

    public class TS5VersionResponse {
        public List<TS5PlatformInfo> versionInfo {get; set;}

        public TS5VersionResponse(List<TS5PlatformInfo> pinfo){
            this.versionInfo = pinfo;
        }
    }

    public class TS5PlatformInfo {
        public string platformName {get; set;}
        public TS5VersionInfo platformInfo {get; set;}
         public HttpResponseHeaders headers {get; private set;}

        public TS5PlatformInfo(TS5VersionInfo info, string name, HttpResponseHeaders wHeaders) {
            this.platformInfo = info;
            this.platformName = name;
            this.headers = wHeaders;
        }
    }

    public class TS5VersionInfo {
        public string version {get; set;}
        public long timestamp {get; set;}
        public string version_string {get; set;}
    }
}