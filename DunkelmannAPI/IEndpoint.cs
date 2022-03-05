using System.Threading.Tasks;

namespace DunkelmannAPI {
    public interface IEndpoint {
        public Task<ResponseInfo> generateResponse(RequestInfo info);
    }
}