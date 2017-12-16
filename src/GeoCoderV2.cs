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
		private string _apiBaseUrl = "https://api.geocod.io/v1/";

		private string _forwardGeoCodeBaseUrl = "https://api.geocod.io/v1/geocode/";
		private string _forwardGeoCodequery = "?api_key={0}&q={1}";
		private string _batchForwardGeocodeQuery = "?api_key={0}";

		private string _reverseGeoCodeBaseUrl = "https://api.geocod.io/v1/";
		private string _singleReverseGeoCodeQuery = "reverse?api_key={0}&q={1}";
		private string _batchReverseGeocodeQuery = "reverse?api_key={0}";

		public GeoCoderV2(string apiKey)
		{
			this._apiKey = apiKey;
		}

		public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(string addressToGeocode
		                                                        , QueryCongressional queryCongressional
		                                                        , QueryStateLegislature queryStateLegis
		                                                        , QuerySchoolDistrict querySchool
		                                                        , QueryCensusInfo queryCensus
		                                                        , QueryTimeZone queryTimeZone)
		{

			string fieldQueryString = BuildFieldsQueryString(queryCongressional
			                                                 , queryStateLegis
															 , querySchool
			                                                 , queryCensus
															 , queryTimeZone);

			string json = await MakeForwardGeocodeWebRequest(addressToGeocode, fieldQueryString);

			ForwardGeoCodeResult result = JsonConvert.DeserializeObject<ForwardGeoCodeResult>(json);

			//Wrap result from GeoCodio in BatchForwardGeocodeResult
			BatchForwardGeoCodeRecord record = new BatchForwardGeoCodeRecord
			{
				Query = addressToGeocode,
				Response = result
			};

			return new BatchForwardGeoCodeResult { Results = new BatchForwardGeoCodeRecord[] { record } };
		}

		public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(List<string> inputAddresses, QueryCongressional queryCongressional
																, QueryStateLegislature queryStateLegis
																, QuerySchoolDistrict querySchool
																, QueryCensusInfo queryCensus
																, QueryTimeZone queryTimeZone)
		{
			string fieldQueryString = BuildFieldsQueryString(queryCongressional, queryStateLegis
															 , querySchool, queryCensus
															 , queryTimeZone);

			string jsonDataString = JsonConvert.SerializeObject(inputAddresses);

			string responseData = await BatchForwardGeocodeWebRequest(jsonDataString, fieldQueryString);

			BatchForwardGeoCodeResult results = JsonConvert.DeserializeObject<BatchForwardGeoCodeResult>(responseData);

			return results;
		}

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

		private async Task<string> MakeForwardGeocodeWebRequest(string addressToGeocode, string fieldQueryString)
		{
			
			Uri baseAddress = new Uri(this._forwardGeoCodeBaseUrl);

			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = baseAddress;

			string queryString = PrepareWebQueryString(GeocodingOperationType.SingleForward
												 , addressToGeocode
												 , fieldQueryString);

			HttpResponseMessage response = await httpClient.GetAsync(queryString);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new GeocodingException((int)response.StatusCode);
			}

			return await response.Content.ReadAsStringAsync();
		}

		public async Task<ReverseGeoCodeResult> ReverseGeocodeAsync(string latLong
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

			return result;

		}

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

		public async Task<BatchReverseGeoCodingResult> BatchReverseGeocodeAsync(List<string> inputAddresses
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

		private async Task<string> BatchReverseGeocodeWebRequest(string jsonPostData, string fieldQueryString)
		{

			Uri baseAddress = new Uri(this._apiBaseUrl);

			//Pass empty string as second parameter because locations to reverse geocode
			//is passed as argument to HttpClient
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

		private string PrepareWebQueryString(GeocodingOperationType geocodingOperation
		                                    , string payload
		                                    , string fieldQueryString)
		{
			if (geocodingOperation == GeocodingOperationType.SingleForward)
			{
				//Single forward
				string query = HttpUtility.UrlEncode(payload);
				query = query + fieldQueryString;
				query = String.Format(this._forwardGeoCodequery, this._apiKey, query);
				return query;
			}
			else if (geocodingOperation == GeocodingOperationType.BatchForward)
			{
				//Batch forward
				string query = String.Format(this._batchForwardGeocodeQuery, this._apiKey);
				query = query + fieldQueryString;
				return query;
			}
			else if (geocodingOperation == GeocodingOperationType.SingleReverse)
			{
				//Single reverse
				string queryString = String.Format(this._singleReverseGeoCodeQuery, this._apiKey, payload);
				queryString = queryString + fieldQueryString;
				return queryString;
			}
			else
			{
				//Batch revsere
				string query = String.Format(this._batchReverseGeocodeQuery, this._apiKey);
				query = query + fieldQueryString;
				return query;
			}
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
				query = "&fields=" + query;
				return query;
			}

		}



	}
}
