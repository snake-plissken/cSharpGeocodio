using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Text;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class GeoCoderV2
	{
		private string _apiKey;
		private static string _apiBaseUrl = "https://api.geocod.io/v1.2/";
		private string _forwardGeoCodeBaseUrl = _apiBaseUrl + "geocode/";
		private string _reverseGeoCodeBaseUrl = _apiBaseUrl + "reverse/";

		public GeoCoderV2(string apiKey)
		{
			this._apiKey = apiKey;
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
		public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(string addressToGeocode
		                                                        , QueryCongressional queryCongressional
		                                                        , QueryStateLegislature queryStateLegis
		                                                        , QuerySchoolDistrict querySchoolDist
		                                                        , QueryCensusInfo queryCensus
		                                                        , QueryTimeZone queryTimeZone)
		{

			string fieldQueryString = BuildFieldsQueryString(queryCongressional
			                                                 , queryStateLegis
															 , querySchoolDist
			                                                 , queryCensus
															 , queryTimeZone);

			//string json = await MakeForwardGeocodeWebRequest(addressToGeocode, fieldQueryString);

			GeoCodioServerResponse geoCodioResponse = await MakeForwardGeocodeWebRequest(
				addressToGeocode, fieldQueryString);

			ForwardGeoCodeResult result = JsonConvert.DeserializeObject<ForwardGeoCodeResult>(geoCodioResponse.RawJsonResponse);

			//Wrap result from GeoCodio in BatchForwardGeocodeResult
			BatchForwardGeoCodeRecord record = new BatchForwardGeoCodeRecord
			{
				Query = addressToGeocode,
				Response = result
			};

			return new BatchForwardGeoCodeResult { Results = new BatchForwardGeoCodeRecord[] { record } };
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
		public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(List<string> inputAddresses, QueryCongressional queryCongressional
																, QueryStateLegislature queryStateLegis
																, QuerySchoolDistrict querySchoolDist
																, QueryCensusInfo queryCensus
																, QueryTimeZone queryTimeZone)
		{
			string fieldQueryString = BuildFieldsQueryString(queryCongressional, queryStateLegis
															 , querySchoolDist, queryCensus
															 , queryTimeZone);

			string jsonDataString = JsonConvert.SerializeObject(inputAddresses);
			string responseData;

			responseData = await BatchForwardGeocodeWebRequest(jsonDataString, fieldQueryString);
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

			string queryString = PrepareWebQueryString(GeocodingOperationType.BatchForward
													   , ""
													   , fieldQueryString);

			Uri baseAddress = new Uri(this._forwardGeoCodeBaseUrl);

			HttpClient client = new HttpClient();
			client.BaseAddress = baseAddress;

			StringContent payload = new StringContent(jsonDataString, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync(queryString, payload);

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
		private async Task<GeoCodioServerResponse> MakeForwardGeocodeWebRequest(string addressToGeocode, string fieldQueryString)
		{

			Uri baseAddress = new Uri(this._forwardGeoCodeBaseUrl);

			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = baseAddress;

			string queryString = PrepareWebQueryString(GeocodingOperationType.SingleForward
												 , addressToGeocode
												 , fieldQueryString);

			Task<HttpResponseMessage> responseTask;

			responseTask = httpClient.GetAsync(queryString);

			string rawJson = await responseTask.Result.Content.ReadAsStringAsync();

			//if (response.StatusCode != System.Net.HttpStatusCode.OK)
			//{
			//	throw new GeocodingException((int)response.StatusCode);
			//}

			//return await response.Content.ReadAsStringAsync();

			return GeoCodioServerResponse.MakeServerResponse(
				responseTask.Status, (int)responseTask.Result.StatusCode, rawJson);
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
		public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(string latLong
		                                     , QueryCongressional queryCongressional
											 , QueryStateLegislature queryStateLegis
											 , QuerySchoolDistrict querySchoolDist
											 , QueryCensusInfo queryCensus
											 , QueryTimeZone queryTimeZone)
		{
			string fieldQueryString = BuildFieldsQueryString(queryCongressional, queryStateLegis
															 , querySchoolDist, queryCensus
															 , queryTimeZone);

			string json = await ReverseGeocodeWebRequest(latLong, fieldQueryString);

			ReverseGeoCodeResult result = JsonConvert.DeserializeObject<ReverseGeoCodeResult>(json);

			BatchReverseGeoCodeResponse response = new BatchReverseGeoCodeResponse
			{
				Query = latLong,
				Response = result
			};

			BatchReverseGeoCodingResult results = new BatchReverseGeoCodingResult
			{
				Results = new BatchReverseGeoCodeResponse[] { response }
			};

			return results;
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
		public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(List<string> inputAddresses
											 , QueryCongressional queryCongressional
											 , QueryStateLegislature queryStateLegis
											 , QuerySchoolDistrict querySchoolDist
											 , QueryCensusInfo queryCensus
											 , QueryTimeZone queryTimeZone)
		{
			string fieldsQuery = BuildFieldsQueryString(queryCongressional, queryStateLegis
															 , querySchoolDist, queryCensus
															 , queryTimeZone);


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
		private async Task<string> ReverseGeocodeWebRequest(string latLong, string fieldQueryString)
		{
			Uri baseAddress = new Uri(this._reverseGeoCodeBaseUrl);

			string queryString = PrepareWebQueryString(GeocodingOperationType.SingleReverse
													   , latLong
													   , fieldQueryString);

			HttpClient client = new HttpClient();
			client.BaseAddress = baseAddress;

			HttpResponseMessage response = await client.GetAsync(queryString);

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
			Uri baseAddress = new Uri(this._reverseGeoCodeBaseUrl);

			//Pass empty string as second parameter; locations to reverse geocode
			//are passed as payload argument to HttpClient
			string queryString = PrepareWebQueryString(GeocodingOperationType.BatchRevsere
													   , ""
													   , fieldQueryString);

			HttpClient client = new HttpClient();
			client.BaseAddress = baseAddress;

			StringContent payload = new StringContent(jsonPostData, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync(queryString, payload);

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new GeocodingException((int)response.StatusCode);
			}

			return await response.Content.ReadAsStringAsync();
		}


		/// <summary>
		/// Prepares the mandatory parts of the URL for sending a request to Geocodio
		/// </summary>
		/// <returns>A string which is the URL we will access</returns>
		/// <param name="geocodingOperation">The type of Geocoding operation.</param>
		/// <param name="payload">Payload; only used when sending a single reverse geocode operation</param>
		/// <param name="fieldQueryString">The fields we wish to query.</param>
		public string PrepareWebQueryString(GeocodingOperationType geocodingOperation
		                                    , string payload
		                                    , string fieldQueryString)
		{
			if (geocodingOperation == GeocodingOperationType.SingleForward)
			{
				//Single forward
				string forwardGeoCodequery = "?api_key={0}&q={1}";

				string query = HttpUtility.UrlEncode(payload);
				query = query + fieldQueryString;
				query = String.Format(forwardGeoCodequery, this._apiKey, query);
				return query;
			}
			else if (geocodingOperation == GeocodingOperationType.BatchForward)
			{
				//Batch forward
				string batchForwardGeocodeQuery = "?api_key={0}";
				string query = String.Format(batchForwardGeocodeQuery, this._apiKey);
				query = query + fieldQueryString;
				return query;
			}
			else if (geocodingOperation == GeocodingOperationType.SingleReverse)
			{
				//Single reverse
				string singleReverseGeoCodeQuery = "?api_key={0}&q={1}";
				string queryString = String.Format(singleReverseGeoCodeQuery, this._apiKey, payload);
				queryString = queryString + fieldQueryString;
				return queryString;
			}
			else
			{
				//Batch revsere
				string batchReverseGeocodeQuery = "?api_key={0}";
				string query = String.Format(batchReverseGeocodeQuery, this._apiKey);
				query = query + fieldQueryString;
				return query;
			}
		}

		/// <summary>
		/// Builds the field query string which will be appended
		/// to the URL.
		/// </summary>
		/// <returns>A string containing the fields to be queried.</returns>
		/// <param name="queryCongress">Query Congreess rep and sens.</param>
		/// <param name="queryStateLegis">Query state legislators.</param>
		/// <param name="querySchoolDist">Query school district.</param>
		/// <param name="queryCensus">Query census tract.</param>
		/// <param name="queryTimeZone">Query time zone.</param>
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
				query = "&fields=" + query;
				return query;
			}

		}



	}
}
