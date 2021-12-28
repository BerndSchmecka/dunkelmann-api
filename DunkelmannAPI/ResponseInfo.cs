namespace DunkelmannAPI {
    public class ResponseInfo {
        public string LastModified {get; private set;}
        public string ResponseString {get; private set;}

        public ResponseInfo(string modified, string response) {
            this.LastModified = modified;
            this.ResponseString = response;
        }
    }
}