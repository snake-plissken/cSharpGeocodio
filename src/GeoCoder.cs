using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;

namespace cSharpGeocodio
{
    /// <summary>
    /// The GeoCoder class.
    /// </summary>
    public class GeoCoder
    {
        private string _apiKey;
		private string _apiBaseUrl = "https://api.geocod.io/v1/";

        private string _forwardGeoCodeBaseUrl = "https://api.geocod.io/v1/geocode";
        private string _forwardGeoCodequery = "?q={0}&api_key={1}";
        private string _reverseGeoCodeBaseUrl = "https://api.geocod.io/v1/";
        private string _singleReverseGeoCodeQuery = "reverse?q={0},{1}&api_key={2}";
        private string _batchReverseGeocodeQuery = "reverse?api_key={0}";

        public GeoCoder(string apiKey)
        {
            this._apiKey = apiKey;
        }
   
        /// <summary>
        /// Forward geocode a single address, synchronously.  Address should be a string in a form
        /// such as "3601 S Broad St, Philadelphia, PA 19148".  The flags can be set to return additional
        /// info
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public string ForwardGeocodeSync (string fullAddress, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string urlEndodedAddress = HttpUtility.UrlEncode(fullAddress);
            string queryString = String.Format(_forwardGeoCodequery, urlEndodedAddress, _apiKey);

            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString += queryFields;

            string url = _forwardGeoCodeBaseUrl + queryString;

            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(_forwardGeoCodeBaseUrl);

            HttpResponseMessage httpResponse = httpClient.GetAsync(queryString).Result;
            int returnStatusCode = (int)httpResponse.StatusCode;
            if (returnStatusCode != 200)
            {
                throw new GeocodingException(returnStatusCode);
            }

            return httpResponse.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Batch forward geocode multiple addresses, synchronously.  Address should be a string in a form
        /// such as "3601 S Broad St, Philadelphia, PA 19148" and contained within a string[].  
        /// The flags can be set to return additional info
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public string ForwardGeocodeSync(string[] addressArray, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string addressFormatter = "\"{0}\"";
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (string address in addressArray)
            {
                sb.Append(String.Format(addressFormatter, address));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            StringContent dataToSend = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");

            string queryString = _forwardGeoCodeBaseUrl + "?api_key=" + _apiKey;
            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString = queryString + queryFields;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_forwardGeoCodeBaseUrl);

            HttpResponseMessage responseMessage = httpClient.PostAsync(queryString, dataToSend).Result;

            int postStatusCode = (int)responseMessage.StatusCode;
            if (postStatusCode != 200)
            {
                throw new GeocodingException(postStatusCode);
            }

            return responseMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Forward geocode a single address, asynchronously.  Address should be a string in a form
        /// such as "3601 S Broad St, Philadelphia, PA 19148".  The flags can be set to return additional
        /// info
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public async Task<string> ForwardGeoCodeAsync (string fullAddress, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string urlEndodedAddress = HttpUtility.UrlEncode(fullAddress);
            string queryString = String.Format(_forwardGeoCodequery, urlEndodedAddress, _apiKey);

            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString += queryFields;

            string url = _forwardGeoCodeBaseUrl + queryString;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_forwardGeoCodeBaseUrl);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(queryString);

            string geoCodingResults = await httpResponse.Content.ReadAsStringAsync();

            return geoCodingResults;
        }
        /// <summary>
        /// Batch forward geocode multiple addresses, asynchronously.  Address should be a string in a form
        /// such as "3601 S Broad St, Philadelphia, PA 19148" and contained within a string[].  
        /// The flags can be set to return additional info
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public async Task<string> ForwardGeoCodeAsync (string[] addressArray, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string addressFormatter = "\"{0}\"";
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (string address in addressArray)
            {
                sb.Append(String.Format(addressFormatter, address));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            StringContent dataToSend = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");

            string queryString = _forwardGeoCodeBaseUrl + "?api_key=" + _apiKey;
            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString = queryString + queryFields;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_forwardGeoCodeBaseUrl);

            HttpResponseMessage responseMessage = await httpClient.PostAsync(queryString, dataToSend);

            string batchGeocodingResults = await responseMessage.Content.ReadAsStringAsync();

            return batchGeocodingResults;
        }

        /// <summary>
        /// Reverse geocode a single point, synchchronously.  Lat and Long inputs are two strings.
        /// Additional boolean flags are used to indicate what Fields you wish to have in the result set
        /// </summary>
        /// <returns>Returns a raw JSON string</returns>
        public string ReverseGeocodeSync (string lat, string lon, bool queryCongressionalDistrict
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string queryString = String.Format(_singleReverseGeoCodeQuery, lat, lon, _apiKey);

            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString += queryFields;

            string url = _reverseGeoCodeBaseUrl + queryString;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_reverseGeoCodeBaseUrl);

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
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
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

            string queryString = String.Format(_batchReverseGeocodeQuery, _apiKey);
            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString = queryString + queryFields;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_reverseGeoCodeBaseUrl);

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
, bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string queryString = String.Format(_singleReverseGeoCodeQuery, lat, lat, _apiKey);

            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                    , querySchoolDistricts, queryTimeZone, queryCensus);

            queryString = queryString + queryFields;

            string url = _reverseGeoCodeBaseUrl + queryString;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_reverseGeoCodeBaseUrl);

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
    , bool queryStateLegislativeDistrict, bool querySchoolDistricts, bool queryTimeZone, bool queryCensus)
        {
            string queryString = String.Format(_batchReverseGeocodeQuery, _apiKey);
            string queryFields = buildFieldQueryString(queryCongressionalDistrict, queryStateLegislativeDistrict
                , querySchoolDistricts, queryTimeZone, queryCensus);
            queryString = queryString + queryFields;
            string url = _reverseGeoCodeBaseUrl + queryString;

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
            httpClient.BaseAddress = new Uri(_reverseGeoCodeBaseUrl);

            string breaker = "ouch";

            HttpResponseMessage responseMessage = await httpClient.PostAsync(queryString, stringJsonData);

            string outputs = await responseMessage.Content.ReadAsStringAsync();
            return outputs;
        }

		public string BuildFieldsQueryString(QueryCongressional queryCongress
		                                     , QueryStateLegislature queryStateLegis
											 , QuerySchoolDistrict querySchoolDist
		                                     , QueryCensusInfo queryCensus
		                                     , QueryTimeZone queryTimeZone)
		{
			string query = "";

			List<string> fields = new List<string>();

			if (queryCongress == QueryCongressional.Yes)
			{
				fields.Add("cd");
			}
			if (queryStateLegis == QueryStateLegislature.Yes)
			{
				fields.Add("stateleg");
			}
			if (querySchoolDist == QuerySchoolDistrict.Yes)
			{
				fields.Add("school");
			}
			if (queryCensus == QueryCensusInfo.Yes)
			{
				fields.Add("census");
			}
			if (queryTimeZone == QueryTimeZone.Yes)
			{
				fields.Add("timezone");
			}

			//No fields queried so return empty string?
			//Or should we just return "fields=" with no values?
			if (fields.Count == 0)
			{
				return query;
			}
			else
			{

				query = String.Join(",", fields);
				query = "fields=" + query;
				return query;
			}

		}

        private string buildFieldQueryString(bool queryCongressionalDistrict
           , bool queryStateLegislativeDistrict, bool querySchoolDistrict, bool queryTimeZone, bool queryCensus)
        {
            string[] queryPieces = new string[5];
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
            if(queryCensus == true)
            {
                queryPieces[4] = "census";
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
