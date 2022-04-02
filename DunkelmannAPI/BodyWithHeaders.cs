using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;

namespace DunkelmannAPI {
    public class BodyWithHeaders {
        public object body {get; private set;}
        public Dictionary<string, IEnumerable<string>> headers {get; private set;}

        public BodyWithHeaders(object body, HttpContentHeaders contentHeaders, HttpResponseHeaders responseHeaders) {
            this.body = body;
            this.headers = UtilMan.JoinHeaders(contentHeaders, responseHeaders);
        }
    }
}