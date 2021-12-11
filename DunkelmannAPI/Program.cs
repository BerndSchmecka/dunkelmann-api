﻿using System;
namespace DunkelmannAPI
{
    class Program
    {
        public static string ERROR_TEMPLATE(string error_message) {return String.Format("<html><head><title>{0}</title></head><body><center><h1>{0}</h1></center><hr><center>{1}</center></body></html>", error_message, displayableVersion);}

        public static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
        public static string displayableVersion = null;

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
