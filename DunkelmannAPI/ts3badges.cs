using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtoBuf;

namespace DunkelmannAPI {
    class ts3badges : IEndpoint {
        const string ts3_badge_url = "https://badges-content.teamspeak.com/list";

        public ts3badges() {

        }

        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ts3_badge_url);
            if(info.IfModifiedSince != null){
                req.Headers.Add("If-Modified-Since", info.IfModifiedSince);
            }
            req.UserAgent = Program.displayableVersion;
            using (HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync()) {
                string responseStr = "";
                bool cached = resp.StatusCode == HttpStatusCode.NotModified;

                if(!cached) {
                    responseStr = JsonConvert.SerializeObject(Serializer.Deserialize<BadgeResponse>(resp.GetResponseStream()));
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
    public class BadgeResponse {
        [ProtoMember(1)]
        public int value {get; set;}

        [ProtoMember(2)]
        public long timestamp {get; set;}

        [ProtoMember(3)]
        public BadgeObject[] badges {get; set;}
    }

    [ProtoContract]
    public class BadgeObject {
        [ProtoMember(1)]
        public string uuid { get; set; }

        [ProtoMember(2)]
        public string name { get; set; }

        [ProtoMember(3)]
        public string url { get; set; }

        [ProtoMember(4)]
        public string description { get; set; }

        [ProtoMember(5)]
        public long timestamp { get; set; }

        [ProtoMember(6)]
        public long value { get; set; }
    }
}