using System;
using Newtonsoft.Json;

namespace DunkelmannAPI {
    class test {
        public ResponseInfo testAPI(){
            return new ResponseInfo(UtilMan.DateToHTTPFormat(DateTime.Now), JsonConvert.SerializeObject(new test_response("If you can see this text, this API works!")));
        }
    }

    class test_response {
        public string testResponse {get; set;}

        public test_response(string resp){
            this.testResponse = resp;
        }
    }
}