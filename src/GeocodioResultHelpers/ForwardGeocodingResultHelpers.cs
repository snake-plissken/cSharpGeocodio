using System;
using System.Collections.Generic;
using cSharpGeocodio.ForwardGeocodingObjects;

namespace cSharpGeocodio.GeocodioResultHelpers
{
	public static class ForwardGeocodingResultHelpers
	{
        /// <summary>
        /// Transforms batch forward geocode results into a dictionary keyed by the address that was queried.  Puts duplicate requests
        /// into the same list and returns a 0 item list when no results are returned.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
		public static Dictionary<string, List<GeoCodeInfo>> MakeForwardResultsDict(BatchForwardGeoCodeResult results)
		{
            Dictionary<string, List<GeoCodeInfo>> resultsDict = new Dictionary<string, List<GeoCodeInfo>>();

			foreach (BatchForwardGeoCodeRecord record in results.Results)
			{
				//Make sure have something to use...
				if (record.Response.Results.Length > 0)
				{
					if (resultsDict.ContainsKey(record.Query))
					{
                        //Geocoded same plce twice in same request to Geocodio
                        resultsDict[record.Query].AddRange(record.Response.Results);
                    }
					else
					{
                        resultsDict.Add(record.Query, new List<GeoCodeInfo>());
                        resultsDict[record.Query].AddRange(record.Response.Results);
					}
				}
				else
				{
					resultsDict.Add(record.Query, new List<GeoCodeInfo>(0));
				}
			}
			return resultsDict;
		}

	}
}
