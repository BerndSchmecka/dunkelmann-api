namespace DunkelmannAPI {
    public class Config {

        public uint configVersion {get; set;}
        public string aesStaticKey {get; set;}
        public string edPublicKey {get; set;}

        public Config(uint configVersion, string aesStaticKey, string edPublicKey) {
            this.configVersion = configVersion;
            this.aesStaticKey = aesStaticKey;
            this.edPublicKey = edPublicKey;
        }
    }
}