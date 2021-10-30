using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Newtonsoft.Json;
using ProtoBuf;

namespace DunkelmannAPI {
    class ts3version {
        const string ts3_version_url = "https://versions.teamspeak.com/ts3-client-2";

        public ts3version() {

        }

        public async Task<string> getVersionInfo() {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ts3_version_url);
            req.UserAgent = Program.displayableVersion;
            using (HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync()) {
                string responseStr = JsonConvert.SerializeObject(Serializer.Deserialize<VersionResponse>(resp.GetResponseStream()));
                return responseStr;
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