using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DunkelmannAPI {

    struct PlatformURL {
        public string url {get; private set;}
        public string platform_name {get; private set;}

        public PlatformURL(string platform_name){
            this.url = $"https://update.teamspeak.com/{platform_name}/x64/latest/info.json";
            this.platform_name = platform_name;
        }
    }

    class ts5version {
        static PlatformURL[] ts5_version_urls = {new PlatformURL("windows"),
                                           new PlatformURL("linux"),
                                           new PlatformURL("mac")};

        public ts5version() {

        }

        public async Task<string> getVersionInfo() {
            List<TS5PlatformInfo> list = new List<TS5PlatformInfo>();
            foreach(PlatformURL purl in ts5_version_urls){
                list.Add(await getPlatformVersionInfo(purl.url, purl.platform_name));
            }
            TS5VersionResponse resp = new TS5VersionResponse(list);
            return JsonConvert.SerializeObject(resp);
        }

        private async Task<TS5PlatformInfo> getPlatformVersionInfo(string platform_url, string platform_name) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(platform_url);
            req.UserAgent = "teamspeak.downloader/1.0";
            req.Headers.Add("Authorization", "Basic dGVhbXNwZWFrNTpMRlo2Wl5rdkdyblh+YW4sJEwjNGd4TDMnYTcvYVtbJl83PmF0fUEzQVJSR1k=");
            using (HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync()) {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream())){
                    TS5PlatformInfo response = new TS5PlatformInfo(JsonConvert.DeserializeObject<TS5VersionInfo>(await sr.ReadToEndAsync()), platform_name);
                    return response;
                }
            }
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

        public TS5PlatformInfo(TS5VersionInfo info, string name) {
            this.platformInfo = info;
            this.platformName = name;
        }
    }

    public class TS5VersionInfo {
        public string version {get; set;}
        public long timestamp {get; set;}
        public string version_string {get; set;}
    }
}