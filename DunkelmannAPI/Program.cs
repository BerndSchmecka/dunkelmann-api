using System;
namespace DunkelmannAPI
{
    class Program
    {
        public static string ERROR_TEMPLATE(string error_message) {return String.Format("<html><head><title>{0}</title></head><body><center><h1>{0}</h1></center><hr><center>{1}</center></body></html>", error_message, displayableVersion);}

        public static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
        public static string displayableVersion = null;

        public static DateTime epoch = new System.DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);

        public static string AES_STATIC_KEY = "";
        public static string ED_PUBLIC_KEY = "";

        static void Main(string[] args)
        {
            #if DEBUG
                System.Console.WriteLine("Mode=Debug");
                displayableVersion = $"Dunkelmann-API/{version} ({buildDate}) RuntimeMode/0 (staging)";
            #else
                System.Console.WriteLine("Mode=Release");
                displayableVersion = $"Dunkelmann-API/{version} ({buildDate}) RuntimeMode/1 (production)";
            #endif

            //read config.json file and serialize to Config object
            var configStream = System.IO.File.OpenRead("config.json");
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(new System.IO.StreamReader(configStream).ReadToEnd());
            configStream.Close();

            //check if config is null, otherwise abort
            if (config == null)
            {
                System.Console.WriteLine("Config is null");
                return;
            }

            //check if configVersion is 1, otherwise abort
            if(config.configVersion != 1)
            {
                System.Console.WriteLine("Config version is not 1, aborting");
                return;
            }

            //set static key
            AES_STATIC_KEY = config.aesStaticKey;
            ED_PUBLIC_KEY = config.edPublicKey;

            //Print out first 5 chars of the keys
            System.Console.WriteLine($"AES_STATIC_KEY: {AES_STATIC_KEY.Substring(0, 5)} ...");
            System.Console.WriteLine($"ED_PUBLIC_KEY: {ED_PUBLIC_KEY.Substring(0, 5)} ...");

            Console.WriteLine("Starting server...");
            ApiServer server = new ApiServer();
            server.StartServer();
        }
    }
}
