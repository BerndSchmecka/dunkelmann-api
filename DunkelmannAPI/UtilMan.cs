using System;

namespace DunkelmannAPI {
    public static class UtilMan {
        public static string DateToHTTPFormat(DateTime input){
            return input.ToUniversalTime().ToString("r");
        }
    }
}