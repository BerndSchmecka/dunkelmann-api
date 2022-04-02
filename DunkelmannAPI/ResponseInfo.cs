using System.Collections.Specialized;

namespace DunkelmannAPI {
    public class ResponseInfo {
        public string responsePayload {get; private set;}
        public int StatusCode {get; private set;}

        public ResponseInfo(string payload, int code) {
            this.responsePayload = payload;
            this.StatusCode = code;
        }
    }
}