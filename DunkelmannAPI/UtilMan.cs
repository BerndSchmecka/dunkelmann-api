using System;

namespace DunkelmannAPI {
    public static class UtilMan {
        public static string DateToHTTPFormat(DateTime input){
            return input.ToUniversalTime().ToString("r");
        }

        public static DateTime HTTPFormatToDate(string input){
            try {
                return DateTime.Parse(input);
            } catch (Exception ex) {
                System.Console.WriteLine(ex.Message);
                return DateTime.UnixEpoch;
            }
        }

        public static bool isCached(string? ifModifiedSince, string? lastModified) {
            if (ifModifiedSince == null || lastModified == null){
                return false;
            } else {
                DateTime ims = HTTPFormatToDate(ifModifiedSince);
                DateTime lm = HTTPFormatToDate(lastModified);

                if(ims >= lm) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}