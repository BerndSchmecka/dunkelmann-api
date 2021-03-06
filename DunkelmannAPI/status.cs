using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;

namespace DunkelmannAPI {
    class status : IEndpoint {

        List<Service> serviceList = new List<Service>();

        public status(){
            serviceList.Add(new HTTPService("website", "https://dunkelmann.eu"));
            serviceList.Add(new HTTPService("api", "https://api.dunkelmann.eu/test"));
            serviceList.Add(new HTTPService("matrix", "https://matrix.dunkelmann.eu/_matrix/federation/v1/version"));
            serviceList.Add(new HTTPService("insomnium", "https://dev-matrix.dunkelmann.eu/_matrix/federation/v1/version"));
            serviceList.Add(new SMTPService("mail", "smtp://mail.dunkelmann.eu"));
            serviceList.Add(new HTTPService("cloud", "https://cloud.dunkelmann.eu"));
        }

        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            List<StatusInfo> infoList = new List<StatusInfo>();
            foreach(Service svc in serviceList){
                var status = await svc.checkService();
                infoList.Add(new StatusInfo(svc.ServiceID, svc.URL, status.Item1, status.Item2));
            }
            return new ResponseInfo(JsonConvert.SerializeObject(infoList), 200);
        }
    }

    abstract class Service {

        public string ServiceID {get; private set;}
        public string URL {get; private set;}

        public Service(string serviceId, string url){
            this.ServiceID = serviceId;
            this.URL = url;
        }

        public abstract Task<(ushort, string?)> checkService();
    }

    class HTTPService : Service {

        public HTTPService(string serviceId, string url) : base(serviceId, url) {}

        public override async Task<(ushort, string?)> checkService() {
            HttpClient client = new HttpClient();
            try {
                using(HttpResponseMessage resp = await client.GetAsync(this.URL)){
                    return ((ushort)resp.StatusCode, resp.ReasonPhrase);
                }
            } catch (Exception ex){
                return (0, ex.Message);
            }
        }
    }

    class SMTPService : Service {
        public SMTPService(string serviceId, string url) : base(serviceId, url) {}

        public override async Task<(ushort, string?)> checkService() {
            string result = String.Empty;

            try {
                            using(var client = new TcpClient(this.URL.Replace("smtp://", ""), 25))
            using(var stream = client.GetStream()){
                client.SendTimeout = 500;
                client.ReceiveTimeout = 1000;

                var quit_msg = Encoding.UTF8.GetBytes("QUIT\r\n");

                await stream.WriteAsync(quit_msg, 0, quit_msg.Length);

                using (var memory = new MemoryStream()){
                    await stream.CopyToAsync(memory);
                    memory.Position = 0;
                    var data = memory.ToArray();
                    
                    result = Encoding.UTF8.GetString(data);
                }
            }

            string banner = result.Replace("\r\n221 2.0.0 Bye\r\n", "");
            string[] code_desc = banner.Split(" ", 2);

            ushort code = ushort.Parse(code_desc[0]);
            string desc = code_desc[1];

            return (code, desc);
            } catch (Exception ex){
                return (0, ex.Message);
            }
        }
    }

    class StatusInfo {
        public string id {get; set;}
        public string url {get; set;}
        public ushort statusCode  {get; private set;}
        public string? statusDesc {get; set;}

        public StatusInfo(string id, string url, ushort statusCode, string? statusDesc){
            this.id = id;
            this.url = url;
            this.statusCode = statusCode;
            this.statusDesc = statusDesc;
        }
    }
}