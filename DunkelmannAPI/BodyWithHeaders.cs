using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace DunkelmannAPI {
    public class BodyWithHeaders {
        public object body {get; private set;}
        public Dictionary<string, string> headers {get; private set;}

        public BodyWithHeaders(object body, WebHeaderCollection headers) {
            this.body = body;
            this.headers = new Dictionary<string, string>();

            foreach(string key in headers.AllKeys){
                this.headers.Add(key, headers[key]);
            }
        }
    }
}