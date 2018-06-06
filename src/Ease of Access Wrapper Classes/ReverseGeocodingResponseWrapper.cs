using System;
using System.Collections.Generic;

namespace cSharpGeocodio
{
	public static class ReverseGeocodingResponseWrapper
	{
		public static Dictionary<string, GeoCodeInfo[]> MakeReverseResultsDict(BatchReverseGeoCodingResult results)
		{
			Dictionary<string, GeoCodeInfo[]> resultsDict = new Dictionary<string, GeoCodeInfo[]>();

			foreach (BatchReverseGeoCodeResponse response in results.Results)
			{
				if (response.Response.Results.Length > 0)
				{
					if (resultsDict.ContainsKey(response.Query))
					{
						//How to handle dupe key?  Make end user worry about it?
					}
					else
					{
						resultsDict.Add(response.Query, response.Response.Results);
					}
				}
				else
				{
					resultsDict.Add(response.Query, new GeoCodeInfo[0]);
				}
			}

			return resultsDict;
		}
	}
}
