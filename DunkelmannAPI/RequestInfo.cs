using System.Net;

namespace DunkelmannAPI {
    public class RequestInfo {
        public string? IfModifiedSince {get; private set;}

        public RequestInfo(HttpListenerRequest req) {
            this.IfModifiedSince = req.Headers.Get("If-Modified-Since");
        }
    }
}