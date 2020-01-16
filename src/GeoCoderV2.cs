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
    /// <summary>
    /// Used for configuring Geocoder client type.
    /// </summary>
    public enum ApiClientType
    {
        RegularApi,
        HippaApi
    }

    /// <summary>
    /// Client object used to conect to and interface with Geocodio.
    /// </summary>
    public class GeoCoderV2
    {
        private string _apiKey;
        private HttpClient _httpClient;

        const string regularApiBase = "https://api.geocod.io/";
        const string hippaApiBase = "https://api-hipaa.geocod.io/";
        const string forwardGeocodeEndpoint = "geocode/";
        const string reverseGeocodeEndpoint = "reverse/";
        public const string ClientGeocodioApiVersionPrefix = "v1.4";

        public string ClientGeocodioApiUrl { get; }
        public ApiClientType ClientType { get; }

        public GeoCoderV2(string apiKey, ApiClientType geocodioClientType)
        {
            this._apiKey = apiKey;
            if (geocodioClientType == ApiClientType.RegularApi)
            {
                ClientGeocodioApiUrl = System.IO.Path.Combine(regularApiBase, ClientGeocodioApiVersionPrefix);
                ClientType = geocodioClientType;
            }
            else
            {
                ClientGeocodioApiUrl = System.IO.Path.Combine(hippaApiBase, ClientGeocodioApiVersionPrefix);
                ClientType = geocodioClientType;
            }
            this._httpClient = new HttpClient();
            this._httpClient.BaseAddress = new Uri(ClientGeocodioApiUrl);
        }

        /// <summary>
        /// Method used to forward geocode a single address.
        /// </summary>
        /// <param name="addressToGeocode">The address we want to forward geocode.</param>
        /// <param name="fieldSettings">Our field settings object used to determine which additional data fields we want to query.</param>
        /// <returns>The results returned from Geocodio.</returns>
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
        /// Method used to batch forward geocode a bunch of addresses.
        /// </summary>
        /// <param name="inputAddresses">A list of address strings.</param>
        /// <param name="fieldSettings">Our field settings object used to determine which additional data fields we want to query.</param>
        /// <returns>The results returned from Geocodio.</returns>
        public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(List<string> inputAddresses, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

            string jsonDataString = JsonConvert.SerializeObject(inputAddresses);

            string responseData = await BatchForwardGeocodeWebRequest(jsonDataString, fieldQueryString);

            BatchForwardGeoCodeResult results = JsonConvert.DeserializeObject<BatchForwardGeoCodeResult>(responseData);

            return results;
        }

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
        /// Method used to reverse geo-code a single lat-long string in deciaml degress format i.e. "48.434325, -76.434543"
        /// </summary>
        /// <param name="latLong">Our point to reverse geo-code.  String in deciaml degress format i.e. "48.434325, -76.434543"</param>
        /// <param name="fieldSettings">Our field settings object used to determine which additional data fields we want to query.</param>
        /// <returns>The results from Geocodio.</returns>
        public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(string latLong, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

            string json = await SingleReverseGeocodeWebRequest(latLong, fieldQueryString);

            ReverseGeoCodeResult result = JsonConvert.DeserializeObject<ReverseGeoCodeResult>(json);

            BatchReverseGeoCodeResponse response = new BatchReverseGeoCodeResponse(latLong, result);

            return new BatchReverseGeoCodingResult(new BatchReverseGeoCodeResponse[] { response });
        }

        /// <summary>
        /// Method which handles reverse geo-coding a list of lat-long strings in deciaml degress format i.e. "48.434325, -76.434543"
        /// </summary>
        /// <param name="inputAddresses">Our list of points to reverse geo-code.  Strings in deciaml degress format i.e. "48.434325, -76.434543"</param>
        /// <param name="fieldSettings">Our field settings object used to determine which additional data fields we want to query.</param>
        /// <returns>The results from Geocodio.</returns>
        public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(List<string> inputAddresses, GeocodioDataFieldSettings fieldSettings)
        {
            string fieldsQuery = this.PrepareDataFieldsQueryString(fieldSettings);

            string jsonPostData = JsonConvert.SerializeObject(inputAddresses);

            string json = await BatchReverseGeocodeWebRequest(jsonPostData, fieldsQuery);

            BatchReverseGeoCodingResult result = JsonConvert.DeserializeObject<BatchReverseGeoCodingResult>(json);

            return result;
        }

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

        /// <summary>
        /// Method used to prepare the additional (and optional) field components of our query, for things like Census or School district fields.
        /// </summary>
        /// <param name="fieldSettings">Our field settings object used to determine which additional data fields we want to query.</param>
        /// <returns>A formatted string which will be appended to the URL when hitting Geocodio.</returns>
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
        /// <param name="geocodingOperation">The type of Geocoding operation.</param>
        /// <param name="payload">Payload; only used when sending a single reverse geocode operation</param>
        /// <param name="dataFieldsQueryString">The fields we wish to query.</param>
        /// <returns>A string which is the base URL we will access</returns>        /// 
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
