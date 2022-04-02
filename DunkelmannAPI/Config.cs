namespace DunkelmannAPI {
    public class Config {
        public string aesStaticKey {get; set;}
        public string edPublicKey {get; set;}

        public Config(string aesStaticKey, string edPublicKey) {
            this.aesStaticKey = aesStaticKey;
            this.edPublicKey = edPublicKey;
        }
    }
}