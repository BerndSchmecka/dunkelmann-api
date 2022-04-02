using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

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

        //Join HttpContentHeaders and HttpResponseHeaders into a single Dictionary<string, IEnumerable<string>> object
        public static Dictionary<string, IEnumerable<string>> JoinHeaders(HttpContentHeaders contentHeaders, HttpResponseHeaders responseHeaders) {
            Dictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>();
            foreach (KeyValuePair<string, IEnumerable<string>> entry in contentHeaders) {
                headers.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> entry in responseHeaders) {
                headers.Add(entry.Key, entry.Value);
            }
            return headers;
        }
    }
}