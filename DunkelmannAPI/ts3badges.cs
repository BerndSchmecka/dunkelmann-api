using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtoBuf;

namespace DunkelmannAPI {
    class ts3badges : IEndpoint {
        const string ts3_badge_url = "https://badges-content.teamspeak.com/list";

        public ts3badges() {

        }

        /*public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ts3_badge_url);
            req.UserAgent = Program.displayableVersion;
            using (HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync()) {
                var responseStr = Serializer.Deserialize<BadgeResponse>(resp.GetResponseStream());
                WebHeaderCollection headers = resp.Headers;
                
                var bodyWithHeaders = new BodyWithHeaders(responseStr, headers);
                return new ResponseInfo(JsonConvert.SerializeObject(bodyWithHeaders), 200);
            }
        }*/

        //Get badge info from ts3_badge_url and return it as a json string using HttpClient
        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", Program.displayableVersion);
            var response = await client.GetAsync(ts3_badge_url);
            var responseStr = Serializer.Deserialize<BadgeResponse>(await response.Content.ReadAsStreamAsync());
            HttpResponseHeaders headers = response.Headers;
            var bodyWithHeaders = new BodyWithHeaders(responseStr, headers);
            return new ResponseInfo(JsonConvert.SerializeObject(bodyWithHeaders), 200);
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