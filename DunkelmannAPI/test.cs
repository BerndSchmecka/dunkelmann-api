using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DunkelmannAPI {
    class test : IEndpoint {
        public async Task<ResponseInfo> generateResponse(RequestInfo info){
            return new ResponseInfo(JsonConvert.SerializeObject(new test_response("If you can see this text, this API works!")), 200);
        }
    }

    class test_response {
        public string testResponse {get; set;}

        public test_response(string resp){
            this.testResponse = resp;
        }
    }
}