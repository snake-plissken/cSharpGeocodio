using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using cSharpGeocodio.ForwardGeocodingObjects;

namespace cSharpGeocodio
{
    public class GeoCoderV2
    {
        private string _apiKey;
        private HttpClient _httpClient;

        const string forwardGeocodeEndpoint = "geocode/";
        const string reverseGeocodeEndpoint = "reverse/";

        public static string ClientGeocodioApiVersion { get; } = "v1.4";

        public static string GeocodioApiBaseUrl { get; } = $"https://api.geocod.io/{ClientGeocodioApiVersion}/";

        public GeoCoderV2(string apiKey)
        {
            this._apiKey = apiKey;
            this._httpClient = new HttpClient();
            this._httpClient.BaseAddress = new Uri(GeocodioApiBaseUrl);
        }

        /// <summary>
        /// Method which handles single geocoding requests.
        /// </summary>
        /// <returns>A single address to Geocodein the form 
        /// "123 Bad Kitty St, Bad Kity City, State ZipCode</returns>
        /// <param name="addressToGeocode">Address to geocode.</param>
        /// <param name="queryCongressional">Query the Congressional info.</param>
        /// <param name="queryStateLegis">Query state legislators info.</param>
        /// <param name="querySchoolDist">Query school diststring info.</param>
        /// <param name="queryCensus">Query census tract info.</param>
        /// <param name="queryTimeZone">Query time zone info.</param>
        public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(string addressToGeocode, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

            string responseData = await SingleForwardGeocodeWebRequest(addressToGeocode, fieldQueryString);

            ForwardGeoCodeResult result = JsonConvert.DeserializeObject<ForwardGeoCodeResult>(responseData);

            //Wrap result from GeoCodio in BatchForwardGeocodeResult because we always want to return a BatchForwardGeoCodeResult
            BatchForwardGeoCodeRecord record = new BatchForwardGeoCodeRecord(addressToGeocode, result);

            return new BatchForwardGeoCodeResult(new BatchForwardGeoCodeRecord[] { record });
        }

        /// <summary>
        /// Method which handles batch geocoding requests.
        /// </summary>
        /// <returns>A batchForwardGeoCodeResult containing the results of the geocoding operation.</returns>
        /// <param name="inputAddresses">A list of input addresses in the form 
        /// "123 Bad Kitty St, Bad Kity City, State ZipCode</param>
        /// <param name="queryCongressional">Query the Congressional info.</param>
        /// <param name="queryStateLegis">Query state legislators info.</param>
        /// <param name="querySchoolDist">Query school diststring info.</param>
        /// <param name="queryCensus">Query census tract info.</param>
        /// <param name="queryTimeZone">Query time zone info.</param>
        public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(List<string> inputAddresses, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

            string jsonDataString = JsonConvert.SerializeObject(inputAddresses);

            string responseData = await BatchForwardGeocodeWebRequest(jsonDataString, fieldQueryString);

            BatchForwardGeoCodeResult results = JsonConvert.DeserializeObject<BatchForwardGeoCodeResult>(responseData);

            return results;
        }

        /// <summary>
        /// Sends a batch forward geocoding requests.
        /// </summary>
        /// <returns>The results of the batch geocoding operation as a string.</returns>
        /// <param name="jsonDataString">Stringified JSON of address list.</param>
        /// <param name="fieldQueryString">Fields we wish to query for this address.</param>
        private async Task<string> BatchForwardGeocodeWebRequest(string jsonDataString, string fieldQueryString)
        {

            string queryString = PrepareWebQueryString(GeocodingOperationType.BatchForward, "", fieldQueryString);

            string url = System.IO.Path.Combine(forwardGeocodeEndpoint, queryString);

            StringContent payload = new StringContent(jsonDataString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this._httpClient.PostAsync(url, payload);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GeocodingException((int)response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Sends a single reverse geocoding requests.
        /// </summary>
        /// <returns>The geocode web request results as a string.</returns>
        /// <param name="addressToGeocode">Address to geocode in the form "2000 Market St, City, State Zip".</param>
        /// <param name="fieldQueryString">Fields we wish to query for this address.</param>
        private async Task<string> SingleForwardGeocodeWebRequest(string addressToGeocode, string fieldQueryString)
        {

            string queryString = PrepareWebQueryString(GeocodingOperationType.SingleForward, addressToGeocode, fieldQueryString);

            string url = System.IO.Path.Combine(forwardGeocodeEndpoint, queryString);

            HttpResponseMessage response = await this._httpClient.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GeocodingException((int)response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Method which handles single reverse geocoding requests.
        /// </summary>
        /// <returns>A BatchReverseGeoCodingResult object holding the results 
        /// of the reverse geocoding operation</returns>
        /// <param name="latLong">A string in the form of "latitude,longitude" to reverse geocode</param>
        /// <param name="queryCongressional">Query the Congressional info.</param>
        /// <param name="queryStateLegis">Query state legislators info.</param>
        /// <param name="querySchoolDist">Query school diststring info.</param>
        /// <param name="queryCensus">Query census tract info.</param>
        /// <param name="queryTimeZone">Query time zone info.</param>
        public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(string latLong, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

            string json = await SingleReverseGeocodeWebRequest(latLong, fieldQueryString);

            ReverseGeoCodeResult result = JsonConvert.DeserializeObject<ReverseGeoCodeResult>(json);

            BatchReverseGeoCodeResponse response = new BatchReverseGeoCodeResponse(latLong, result);

            return new BatchReverseGeoCodingResult(new BatchReverseGeoCodeResponse[] { response });
        }

        /// <summary>
        /// Method which handles batch reverse geocoding requests.
        /// </summary>
        /// <returns>A BatchReverseGeoCodingResult object holding the results 
        /// of the reverse geocoding operation
        /// </returns>
        /// <param name="inputAddresses">List of input addresses in the form "latitiude,longitude".</param>
        /// <param name="queryCongressional">Query the Congressional info.</param>
        /// <param name="queryStateLegis">Query state legislators info.</param>
        /// <param name="querySchoolDist">Query school diststring info.</param>
        /// <param name="queryCensus">Query census tract info.</param>
        /// <param name="queryTimeZone">Query time zone info.</param>
        public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(List<string> inputAddresses, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldsQuery = this.PrepareDataFieldsQueryString(fieldSettings);

            string jsonPostData = JsonConvert.SerializeObject(inputAddresses);

            string json = await BatchReverseGeocodeWebRequest(jsonPostData, fieldsQuery);

            BatchReverseGeoCodingResult result = JsonConvert.DeserializeObject<BatchReverseGeoCodingResult>(json);

            return result;
        }

        /// <summary>
        /// Sends a single point for reverse geocoding.
        /// </summary>
        /// <returns>The reverse geocode web request results as a string.</returns>
        /// <param name="latLong">Lat and long of point to reverse geocode</param>
        /// <param name="fieldQueryString">Fields we wish to query for these points.</param>
        private async Task<string> SingleReverseGeocodeWebRequest(string latLong, string fieldQueryString)
        {

            string queryString = PrepareWebQueryString(GeocodingOperationType.SingleReverse, latLong, fieldQueryString);

            string url = System.IO.Path.Combine(reverseGeocodeEndpoint, queryString);

            HttpResponseMessage response = await this._httpClient.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GeocodingException((int)response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Sends a batch reverse geocoding request.
        /// </summary>
        /// <returns>The reverse geocode web request results as a string.</returns>
        /// <param name="jsonPostData">Stringified JSON data we wish to reverse geocode.</param>
        /// <param name="fieldQueryString">Fields we wish to query for these points.</param>
        private async Task<string> BatchReverseGeocodeWebRequest(string jsonPostData, string fieldQueryString)
        {

            //Pass empty string as second parameter; locations to reverse geocode
            //are passed as payload argument to HttpClient
            string queryString = PrepareWebQueryString(GeocodingOperationType.BatchRevsere, "", fieldQueryString);

            string url = System.IO.Path.Combine(reverseGeocodeEndpoint, queryString);

            StringContent payload = new StringContent(jsonPostData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this._httpClient.PostAsync(url, payload);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GeocodingException((int)response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public string PrepareDataFieldsQueryString(GeocodioDataFieldSettings fieldSettings)
        {

            var fields = new List<string>();

            foreach (var fieldKey in GeocodioDataFieldSettings.ValidGeocodioFields)
            {
                if (fieldSettings.GetFieldQueryStatus(fieldKey))
                {
                    fields.Add(fieldKey);
                }
            }

            if (fields.Count == 0)
            {
                //No fields to query, give 'em an empty string!
                return "";
            }
            else
            {
                return "&fields=" + String.Join(",", fields);
            }
        }

        /// <summary>
        /// Prepares the mandatory parts of the URL for sending a request to Geocodio
        /// </summary>
        /// <returns>A string which is the URL we will access</returns>
        /// <param name="geocodingOperation">The type of Geocoding operation.</param>
        /// <param name="payload">Payload; only used when sending a single reverse geocode operation</param>
        /// <param name="dataFieldsQueryString">The fields we wish to query.</param>
        public string PrepareWebQueryString(GeocodingOperationType geocodingOperation, string payload, string dataFieldsQueryString)
        {
            if (geocodingOperation == GeocodingOperationType.SingleForward)
            {
                //Single forward
                string forwardGeoCodequery = "?api_key={0}&q={1}";

                string query = HttpUtility.UrlEncode(payload);
                query = query + dataFieldsQueryString;
                query = String.Format(forwardGeoCodequery, this._apiKey, query);
                return query;
            }
            else if (geocodingOperation == GeocodingOperationType.BatchForward)
            {
                //Batch forward
                string batchForwardGeocodeQuery = "?api_key={0}";
                string query = String.Format(batchForwardGeocodeQuery, this._apiKey);
                query = query + dataFieldsQueryString;
                return query;
            }
            else if (geocodingOperation == GeocodingOperationType.SingleReverse)
            {
                //Single reverse
                string singleReverseGeoCodeQuery = "?api_key={0}&q={1}";
                string queryString = String.Format(singleReverseGeoCodeQuery, this._apiKey, payload);
                queryString = queryString + dataFieldsQueryString;
                return queryString;
            }
            else
            {
                //Batch revsere
                string batchReverseGeocodeQuery = "?api_key={0}";
                string query = String.Format(batchReverseGeocodeQuery, this._apiKey);
                query = query + dataFieldsQueryString;
                return query;
            }
        }
    }
}
