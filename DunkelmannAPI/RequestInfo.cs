using System.Collections.Specialized;
using System.Net;

namespace DunkelmannAPI {
    public class RequestInfo {
        public NameValueCollection requestHeaders {get;}

        public RequestInfo(HttpListenerRequest req) {
            this.requestHeaders = req.Headers;
        }
    }
}