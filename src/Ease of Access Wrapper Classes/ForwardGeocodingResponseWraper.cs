using System;
using System.Collections.Generic;
using cSharpGeocodio.ForwardGeocodingObjects;

namespace cSharpGeocodio
{
	public static class ForwardGeocodingResponseWraper
	{
		public static Dictionary<string, GeoCodeInfo[]> MakeForwardResultsDict(BatchForwardGeoCodeResult results)
		{
			Dictionary<string, GeoCodeInfo[]> resultsDict = new Dictionary<string, GeoCodeInfo[]>();

			foreach (BatchForwardGeoCodeRecord record in results.Results)
			{
				//Make sure have something to use...
				if (record.Response.Results.Length > 0)
				{
					if (resultsDict.ContainsKey(record.Query))
					{
						//How to handle this?  Make end user worry about it?
					}
					else
					{
						resultsDict.Add(record.Query, record.Response.Results);
					}
				}
				//How to handle queries with no results?
				else
				{
					resultsDict.Add(record.Query, new GeoCodeInfo[0]);
				}
			}
			return resultsDict;
		}
	}
}
