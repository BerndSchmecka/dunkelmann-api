using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace DunkelmannAPI {
        public struct ResponseData {
        public HttpListenerResponse Response {get;}
        public byte[] Data {get;}
        public string ContentType {get;}
        public int StatusCode {get;}

        public ResponseData(HttpListenerResponse resp, string data_to_send, string contentType, int statusCode){
            this.Response = resp;
            this.Data = Encoding.UTF8.GetBytes(data_to_send);
            this.ContentType = contentType;
            this.StatusCode = statusCode;

            this.Response.ContentType = this.ContentType;
            this.Response.ContentEncoding = Encoding.UTF8;
            this.Response.ContentLength64 = this.Data.LongLength;
            this.Response.StatusCode = this.StatusCode;
        }
    }

    class ApiServer {
        private static HttpListener listener;
        private static string url = "http://*:8888/";

        private async Task HandleInboundConnections() {
            bool runServer = true;

            while(runServer) {
                HttpListenerContext context = await listener.GetContextAsync();

                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("X-Powered-By", Program.displayableVersion);

                ResponseData rd = new ResponseData(response, Program.ERROR_TEMPLATE("404 Not Found"), "text/html", 404);

                if(request.HttpMethod == "GET"){
                    switch(request.Url.AbsolutePath) {
                        case "/ts3version":
                            try {
                                ts3version ver = new ts3version();
                                rd = new ResponseData(response, await ver.getVersionInfo(), "application/json", 200);
                            } catch (Exception ex) {
                                rd = new ResponseData(response, "{\"errorMessage\": \"" + ex.Message +"\"}", "application/json", 500);
                            }
                            break;
                    }
                } else if (request.HttpMethod == "OPTIONS"){
                    rd = new ResponseData(response, Program.ERROR_TEMPLATE("405 Method Not Allowed"), "text/html", 405);
                } else if (request.HttpMethod == "POST") {
                    rd = new ResponseData(response, Program.ERROR_TEMPLATE("405 Method Not Allowed"), "text/html", 405);
                } else {
                    rd = new ResponseData(response, Program.ERROR_TEMPLATE("405 Method Not Allowed"), "text/html", 405);
                }
                
                await response.OutputStream.WriteAsync(rd.Data, 0, rd.Data.Length);
                response.Close();
            }
        }

        public void StartServer(){
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            Task listenTask = HandleInboundConnections();
            listenTask.GetAwaiter().GetResult();

            listener.Close();
        }
    }
}