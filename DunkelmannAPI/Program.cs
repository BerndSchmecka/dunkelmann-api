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

        public static string AES_STATIC_KEY = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
        public static string ED_PUBLIC_KEY = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

        static void Main(string[] args)
        {
            #if DEBUG
                System.Console.WriteLine("Mode=Debug");
                displayableVersion = $"Dunkelmann-API/{version} ({buildDate}) RuntimeMode/0 (staging)";
            #else
                System.Console.WriteLine("Mode=Release");
                displayableVersion = $"Dunkelmann-API/{version} ({buildDate}) RuntimeMode/1 (production)";
            #endif

            Console.WriteLine("Starting server...");
            ApiServer server = new ApiServer();
            server.StartServer();
        }
    }
}
