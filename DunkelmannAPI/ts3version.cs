using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtoBuf;

namespace DunkelmannAPI {
    class ts3version : IEndpoint {
        const string ts3_version_url = "https://versions.teamspeak.com/ts3-client-2";

        public ts3version() {

        }

        //Get version info from ts3_version_url and return it as a json string using HttpClient
        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", Program.displayableVersion);
            var response = await client.GetAsync(ts3_version_url);
            var responseStr = Serializer.Deserialize<VersionResponse>(await response.Content.ReadAsStreamAsync());
            HttpContentHeaders contentHeaders = response.Content.Headers;
            HttpResponseHeaders responseHeaders = response.Headers;
            var bodyWithHeaders = new BodyWithHeaders(responseStr, contentHeaders, responseHeaders);
            return new ResponseInfo(JsonConvert.SerializeObject(bodyWithHeaders), 200);
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