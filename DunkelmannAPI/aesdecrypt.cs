using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Ed25519;

namespace DunkelmannAPI {
    class aesdecrypt : IEndpoint {

        aesdecrypt_request? encrypedMsg;
        string error_msg = "unknown error";

        public aesdecrypt(HttpListenerRequest request) {
            try {
            string text;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }
            this.encrypedMsg = JsonConvert.DeserializeObject<aesdecrypt_request>(text);
            } catch (Exception ex) {
                this.encrypedMsg = null;
                this.error_msg = ex.Message;
            }
        }

        public async Task<ResponseInfo> generateResponse(RequestInfo info) {
            if (this.encrypedMsg != null){
                if (await Signer.ValidateAsync(Convert.FromBase64String(this.encrypedMsg.sign), Encoding.UTF8.GetBytes($"{this.encrypedMsg.iv}dunkelmannMessage{this.encrypedMsg.msg}"), Convert.FromBase64String(Program.ED_PUBLIC_KEY))){
                    Aes aes = Aes.Create();
                    aes.Key = Convert.FromBase64String(Program.AES_STATIC_KEY);
                    var decrypted = aes.DecryptCbc(Convert.FromBase64String(this.encrypedMsg.msg), Convert.FromBase64String(this.encrypedMsg.iv), PaddingMode.PKCS7);
                    return new ResponseInfo(JsonConvert.SerializeObject(new aesdecrypt_response(Encoding.UTF8.GetString(decrypted))), 200);
                } else {
                    throw new Exception("Invalid sign provided!");
                }
            } else {
                throw new Exception(this.error_msg);
            }
        }
    }

    public class aesdecrypt_request {
        public string iv {get; set;}
        public string msg {get; set;}
        public string sign {get; set;}

        public aesdecrypt_request(string iv, string msg, string sign){
            this.iv = iv;
            this.msg = msg;
            this.sign = sign;
        }
    }

    public class aesdecrypt_response {
        public string cleartext {get; set;}

        public aesdecrypt_response(string cleartext){
            this.cleartext = cleartext;
        }
    }
}