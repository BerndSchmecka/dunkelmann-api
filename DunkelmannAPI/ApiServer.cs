using System;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace DunkelmannAPI {
        public struct ResponseData {
        public HttpListenerResponse Response {get;}
        public byte[] Data {get;}
        public string ContentType {get;}

        public ResponseData(HttpListenerResponse resp, ResponseInfo data_to_send, string contentType){
            this.Response = resp;
            this.Data = Encoding.UTF8.GetBytes(data_to_send.responsePayload);
            this.ContentType = contentType;

            this.Response.ContentType = this.ContentType;
            this.Response.ContentEncoding = Encoding.UTF8;
            this.Response.ContentLength64 = this.Data.LongLength;
            this.Response.StatusCode = data_to_send.StatusCode;
        }
    }

    class ApiServer {
        private static string url = "http://*:8888/";

        private async Task Listen(string prefix, int maxConcurrentRequests, CancellationToken token){
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();

            var requests = new HashSet<Task>();
            for(int i=0; i < maxConcurrentRequests; i++)
                requests.Add(listener.GetContextAsync());

            while (!token.IsCancellationRequested){
                Task t = await Task.WhenAny(requests);
                requests.Remove(t);

                if (t is Task<HttpListenerContext>){
                    var context = (t as Task<HttpListenerContext>).Result;
                    requests.Add(HandleInboundConnections(context));
                    requests.Add(listener.GetContextAsync());
                }
            }
        }

        private async Task HandleInboundConnections(HttpListenerContext context) {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("X-Powered-By", Program.displayableVersion);

                ResponseData rd = new ResponseData(response, new ResponseInfo(Program.ERROR_TEMPLATE("404 Not Found"), 404), "text/html");

                if(request.HttpMethod == "GET"){
                    switch(request.Url.AbsolutePath) {
                        case "/ts3version":
                            rd = await processRequest(request, response, new ts3version());
                            break;
                        case "/ts5version":
                            rd = await processRequest(request, response, new ts5version());
                            break;
                        case "/ts3badges":
                            rd = await processRequest(request, response, new ts3badges());
                            break;
                        case "/status":
                            rd = await processRequest(request, response, new status());
                            break;
                        case "/test":
                            rd = await processRequest(request, response, new test());
                            break;
                    }
                } else if (request.HttpMethod == "OPTIONS"){
                    rd = new ResponseData(response, new ResponseInfo(Program.ERROR_TEMPLATE("405 Method Not Allowed"), 405), "text/html");
                } else if (request.HttpMethod == "POST") {
                    switch(request.Url.AbsolutePath) {
                        case "/aesdecrypt":
                            rd = await processRequest(request, response, new aesdecrypt(request));
                            break;
                    }
                } else {
                    rd = new ResponseData(response, new ResponseInfo(Program.ERROR_TEMPLATE("405 Method Not Allowed"), 405), "text/html");
                }
                
                await response.OutputStream.WriteAsync(rd.Data, 0, rd.Data.Length);
                response.Close();
        }

        private async Task<ResponseData> processRequest(HttpListenerRequest req, HttpListenerResponse resp, IEndpoint ep) {
            try {
                return new ResponseData(resp, await ep.generateResponse(new RequestInfo(req)), "application/json");
            } catch (WebException wex) {
                if(((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotModified) {
                    return new ResponseData(resp, new ResponseInfo("", 304), "application/json");
                } else {
                    return new ResponseData(resp, new ResponseInfo("{\"errorMessage\": \"" + wex.Message +"\"}", 500), "application/json");
                }
            } catch (Exception ex) {
                return new ResponseData(resp, new ResponseInfo("{\"errorMessage\": \"" + ex.Message +"\"}", 500), "application/json");
            }
        }

        public void StartServer(){
            CancellationToken token = new CancellationToken();
            Task listenTask = Listen(url, 32, token);
            listenTask.GetAwaiter().GetResult();
        }
    }
}