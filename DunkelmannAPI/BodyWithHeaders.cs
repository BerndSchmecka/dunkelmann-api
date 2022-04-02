using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;

namespace DunkelmannAPI {
    public class BodyWithHeaders {
        public object body {get; private set;}
        public HttpResponseHeaders headers {get; private set;}

        public BodyWithHeaders(object body, HttpResponseHeaders headers) {
            this.body = body;
            this.headers = headers;
        }
    }
}