using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace GeoCodio
{
    /// <summary>
    /// The GeoCoder class.
    /// </summary>
    class GeoCoder
    {
        private string apiKey;
        private string reverseGeoCodeBaseUrl = "https://api.geocod.io/v1/";
        private string singleReverseGeoCodeQuery = "reverse?q={0},{1}&api_key={2}";
        private string batchReverseGeocodeQuery = "reverse?api_key={0}";

        public GeoCoder(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Reverse geocode a single point, synchchronously.  Lat and Long inputs are two strings.
        /// Additional boolean flags are used to indicate what Fields you wish to have in the result set
        /// </summary>
        /// <returns>Returns a jaw JSON string</returns>
        public string ReverseGeocodeSync (string lat, string lon, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone)
        {
            string queryString = String.Format(singleReverseGeoCodeQuery, lat, lon, apiKey);

            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone);

            queryString += queryFields;

            string url = reverseGeoCodeBaseUrl + queryString;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(reverseGeoCodeBaseUrl);

            HttpResponseMessage httpResponse = httpClient.GetAsync(queryString).Result;
            int returnStatusCode = (int)httpResponse.StatusCode;
            if (returnStatusCode != 200)
            {
                throw new GeocodingException(returnStatusCode);
            }

            return httpResponse.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Batch Reverse Geocode, synchronously.  Input lat-longs should be in the form "lat,long"
        /// and contained within a string[]
        /// Additional boolean flags are used to indicate what Fields you wish to have in the result set
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public string ReverseGeocodeSync (string[] latLongStringsArray, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone)
        {
            string latLongFormatter = "\"{0}\"";

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (string location in latLongStringsArray)
            {
                sb.Append(String.Format(latLongFormatter, location));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            StringContent dataTosend = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");

            string queryString = String.Format(batchReverseGeocodeQuery, apiKey);
            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone);

            queryString = queryString + queryFields;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(reverseGeoCodeBaseUrl);

            HttpResponseMessage responseMessage = httpClient.PostAsync(queryString, dataTosend).Result;

            int postStatusCode = (int)responseMessage.StatusCode;
            if (postStatusCode != 200)
            {
                throw new GeocodingException(postStatusCode);
            }

            return responseMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Reverse geocode a single point, asynchchronously.  Lat and Long inputs are two strings.
        /// Additional boolean flags are used to indicate what Fields you wish to have in the result set
        /// </summary>
        /// <returns>Returns a jaw JSON string</returns>
        public async Task<string> ReverseGeoCodeAsync(string lat, string lon, bool queryCongressionalDistrict
, bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone)
        {
            string queryString = String.Format(singleReverseGeoCodeQuery, lat, lat, apiKey);

            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                    , querySchoolDistricts, queryTimeZone);

            queryString = queryString + queryFields;

            string url = reverseGeoCodeBaseUrl + queryString;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(reverseGeoCodeBaseUrl);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            string geoCodingResults = await httpResponse.Content.ReadAsStringAsync();

            return geoCodingResults;
        }

        /// <summary>
        /// Batch Reverse Geocode, asynchronously.  Input lat-longs should be in the form "lat,long"
        /// and contained within a string[]
        /// Additional boolean flags are used to indicate what Fields you wish to have in the result set
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public async Task<string> ReverseGeoCodeAsync(string[] latLongStringsArray, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone)
        {
            string queryString = String.Format(batchReverseGeocodeQuery, apiKey);
            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone);
            queryString = queryString + queryFields;
            string url = reverseGeoCodeBaseUrl + queryString;

            string latLongFormatter = "\"{0}\"";

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (string latLong in latLongStringsArray)
            {
                sb.Append(String.Format(latLongFormatter, latLong));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            StringContent stringJsonData = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(reverseGeoCodeBaseUrl);

            string breaker = "ouch";

            HttpResponseMessage responseMessage = await httpClient.PostAsync(queryString, stringJsonData);

            string outputs = await responseMessage.Content.ReadAsStringAsync();
            return outputs;
        }

        private string buildFieldQueryString(bool queryCongressionalDistrict
           , bool queryStateLegislativeDistrict, bool querySchoolDistrict, bool queryTimeZone)
        {
            string[] queryPieces = new string[4];
            if (queryCongressionalDistrict == true)
            {
                queryPieces[0] = "cd";
            }
            if (queryStateLegislativeDistrict == true)
            {
                queryPieces[1] = "stateleg";
            }
            if (querySchoolDistrict == true)
            {
                queryPieces[2] = "school";
            }
            if (queryTimeZone == true)
            {
                queryPieces[3] = "timezone";
            }

            string fieldsQuery = "";
            int fieldsCounter = 0;

            foreach (string field in queryPieces)
            {
                if (field != null)
                {
                    fieldsCounter++;
                    fieldsQuery += "," + field;
                }
            }

            if (fieldsCounter > 0)
            {
                fieldsQuery = fieldsQuery.Remove(0, 1);
                return "&fields=" + fieldsQuery;
            }
            else
            {
                return "";
            }

        }

    }
}
