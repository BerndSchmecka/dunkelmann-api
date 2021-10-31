using System;
using System.IO;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DunkelmannAPI {
    class status {

        List<Service> serviceList = new List<Service>();

        public status(){
            serviceList.Add(new Service("website", "https://dunkelmann.eu"));
            serviceList.Add(new Service("api", "https://api.dunkelmann.eu/test"));
            serviceList.Add(new Service("matrix", "https://matrix.dunkelmann.eu/_matrix/federation/v1/version"));
            serviceList.Add(new Service("insomnium", "https://dev-matrix.dunkelmann.eu/_matrix/federation/v1/version"));
            serviceList.Add(new Service("cloud", "https://cloud.dunkelmann.eu"));
        }

        public async Task<string> getServiceInfo() {
            List<StatusInfo> infoList = new List<StatusInfo>();
            foreach(Service svc in serviceList){
                var status = await svc.checkService();
                infoList.Add(new StatusInfo(svc.ServiceID, svc.URL, status.Item1, status.Item2));
            }
            return JsonConvert.SerializeObject(infoList);
        }
    }

    class Service {

        public string ServiceID {get; private set;}
        public string URL {get; private set;}

        public Service(string serviceId, string url){
            this.ServiceID = serviceId;
            this.URL = url;
        }

        public async Task<(HttpStatusCode, string)> checkService(){
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.URL);
            try {
                using(HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync()){
                    return (resp.StatusCode, resp.StatusDescription);
                }
            } catch (WebException we){
                var response = we.Response as HttpWebResponse;
                if(response != null){
                    return (response.StatusCode, response.StatusDescription);
                } else {
                    return (0, we.Message);
                }
            } catch (Exception ex){
                return (0, ex.Message);
            }
        }
    }

    class StatusInfo {
        public string id {get; set;}
        public string url {get; set;}
        public HttpStatusCode statusCode {get; set;}
        public string statusDesc {get; set;}

        public StatusInfo(string id, string url, HttpStatusCode statusCode, string statusDesc){
            this.id = id;
            this.url = url;
            this.statusCode = statusCode;
            this.statusDesc = statusDesc;
        }
    }
}