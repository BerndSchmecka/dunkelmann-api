using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtoBuf;

namespace DunkelmannAPI {
    class ts3version : IEndpoint {
        const string ts3_version_url = "https://versions.teamspeak.com/ts3-client-2";

        public ts3version() {

        }

        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ts3_version_url);
            if(info.IfModifiedSince != null){
                req.Headers.Add("If-Modified-Since", info.IfModifiedSince);
            }
            req.UserAgent = Program.displayableVersion;
            using (HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync()) {
                
                string responseStr = "";
                bool cached = resp.StatusCode == HttpStatusCode.NotModified;

                if(!cached) {
                    responseStr = JsonConvert.SerializeObject(Serializer.Deserialize<VersionResponse>(resp.GetResponseStream()));
                }

            string lastModified = "";
            try {
                lastModified = resp.GetResponseHeader("Last-Modified");
            } catch (System.Exception) {
                lastModified = UtilMan.DateToHTTPFormat(Program.epoch);
            }
                return new ResponseInfo(lastModified, responseStr, cached ? 304 : 200);
            }
        }
    }

    [ProtoContract]
    public class VersionResponse {
        [ProtoMember(1)]
        public int value {get; set;}

        [ProtoMember(2)]
        public ChannelObject[] versions {get; set;}

        [ProtoMember(3)]
        public int value2 {get; set;}
    }

    [ProtoContract]
    public class ChannelObject {
        [ProtoMember(1)]
        public string channel {get; set;}

        [ProtoMember(2)]
        public long timestamp {get; set;}

        [ProtoMember(3)]
        public string version {get; set;}
    }
}