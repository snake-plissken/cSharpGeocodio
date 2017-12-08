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
		private string _singleReverseGeoCodeQuery = "reverse?q={0},{1}&api_key={2}";
		private string _batchReverseGeocodeQuery = "reverse?api_key={0}";

		public GeoCoderV2(string apiKey)
		{
			this._apiKey = apiKey;
		}



		//Doesn't make any sense to make this generic...because
		//method is always returning same thing
		public async Task<ForwardGeoCodeResult> ForwardGeocodeAsync(string inputAddress, QueryCongressional queryCongressional
		                                                        , QueryStateLegislature queryStateLegis
		                                                        , QuerySchoolDistrict querySchool
		                                                        , QueryCensusInfo queryCensus
		                                                        , QueryTimeZone queryTimeZone)
		{

			string fieldQueryString = BuildFieldsQueryString(queryCongressional, queryStateLegis
															 , querySchool, queryCensus
															 , queryTimeZone);

			string json = await MakeForwardGeocodeWebRequest(inputAddress, fieldQueryString);

			ForwardGeoCodeResult results = JsonConvert.DeserializeObject<ForwardGeoCodeResult>(json);

			return results;
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

		private async Task<string> BatchForwardGeocodeWebRequest(string jsonDataString, string fieldqueryString)
		{
			Uri baseAddress = new Uri(this._forwardGeoCodeBaseUrl);
			string query = String.Format(this._batchForwardGeocodeQuery, this._apiKey);
			query = query + fieldqueryString;

			HttpClient client = new HttpClient();
			client.BaseAddress = baseAddress;

			StringContent payload = new StringContent(jsonDataString, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync(query, payload);

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new GeocodingException((int)response.StatusCode);
			}

			return await response.Content.ReadAsStringAsync();

		}

		private async Task<string> MakeForwardGeocodeWebRequest(string inputAddress, string fieldQueryString)
		{
			Uri baseAddress = new Uri(this._forwardGeoCodeBaseUrl);

			string query = HttpUtility.UrlEncode(inputAddress);
			query = query + fieldQueryString;
			query = String.Format(this._forwardGeoCodequery, this._apiKey, query);

			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = baseAddress;
			HttpResponseMessage response = await httpClient.GetAsync(query);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new GeocodingException((int)response.StatusCode);
			}

			return await response.Content.ReadAsStringAsync();


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
