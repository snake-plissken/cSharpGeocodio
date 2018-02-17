# A wrapper to access Geocodio, a geocoding service.  

## Website for the service: https://geocod.io/

Usage and examples are below.

Forward Gecoding:

```c#
GeoCoderV2 geoCoder = new GeoCoderV2('Your Grocodio API key.  You get 2500 free lookups per day!');

//
//Forward geocode a single address:
//
string singleAddress = "2100 East Market Street, Philadelphia, PA 19103";

Task<BatchForwardGeoCodeResult> singleAddress = await geoCoder.ForwardGeocodeAsync(singleAddress
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.No
, QueryCensusInfo.No
, QueryTimeZone.No);
                         
BatchFowardGeocodeResult singleResult = singleAddress.Result;

//Geocodio will often return multiple items in the Results 
//property of the Response object, ordered by the most accurate.
Location singleLatLong = single_result.Response.Results[0].Location;

//
//Batch forward geocoding
//
//Make a list of addresses...
Task<BatchForwardGeoCodeResult> batchGeocode = await geoCoder.ForwardGeocodeAsync(list_of_addresses
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.No
, QueryCensusInfo.No
, QueryTimeZone.No);
                         
BatchForwardGeocodeResult batchResults = batchGeocode.Result;
//Iterate through collection of results.
//When batch geocoding, Geocodio returns the results in the same
//order as found in the list_of_addresses you pass to the ForwardGeocodeAsync method.
for(int i = 0; i++; i < batchResults.Results)
{
    BatchForwardGeoCodeRecord geoCodedItem = batchResults.Results[i];
    string addressWhichWasGeocoded = geoCodedItem.Query;
    Location latLong = geoCodedItem.Response.Results[0].Location;
    //Add to database, add to queue, some other operation;
}
```

Reverse Geocoding:

```c#
//
//Reverse geocode a single point
//

string singlePoint = "39.9373426,-75.1865927";

Task<BatchReverseGeoCodingResult> reversePoint = await gc.ReverseGeocodeAsync(singlePoint
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.Yes
, QueryCensusInfo.No
, QueryTimeZone.No);

BatchReverseGeocodingResult singlePointResult = reversePoint.Result;
ReverseGeocodeResult reverseGeocodeResult = singlePointResult.Results[0].Response.Results;
string addressOfPoint = reverseGeocodeResult.Results[0].FormattedAddress;

//
//Batch reverse geocoding
//

List<string> batchReverseInputs = new List<string>();
//Add points to list...
Task<BatchReverseGeoCodingResult> batchReverse = await gc.ReverseGeocodeAsync(batchReverseInputs
, QueryCongressional.No
, QueryStateLegislature.No
, QuerySchoolDistrict.Yes
, QueryCensusInfo.No
, QueryTimeZone.No);

BatchReverseGeoCodingResult batchReverseResults = batchReverse.Result;
//Iterate through the results
//As with forward geocoding, results are returned in the same order as found
//in the list of LatLong points we passed to the ReverseGeocodeAsync
for (int i = 0; i < batchReverseResults.Length; i++)
{
    BatchReverseGeoCodeResponse batchResponse = batchReverseResults.Results[i];
    ReverseGeoCodeResult resultsFromOneQuery = batchResponse.Response;
    //Here, the actual results of the operation, ordered in terms
    //of accuray
    GeoCodeInfo[] geoCodedInfo = resultsFromOneQuery.Results;
}

```

Design Notes/Stuff:

1. Remaining To-Do:
  * Add ease of access properties and fields to geocoding responses.  BatchForwardGeoCodeResult and BatchReverseGeoCodingResult are clunky little guys at the moment.
  * While the JSON backer classes have been cleaned up, the backer classes for the optional fields (e.g Census Tract Info) still need to be cleaned up
2. Design Info/Notes/Thoughts:
  * The Christmas 2017 commits made some substantial changes.  My apologies if they broke anything but I was pretty sure no one was using this.
  * The goals were with these changes were to condense the geocoding functions and to return the same types from the forward and reverse mthods, respectively, i.e. the forward and batch forward methods each return a `Task<BatchForwardGeoCodeResult>` object
  * I considered writing the geocoding methods to take interfaces but it seemed fine to just use strings or lists of strings instead
  * I also thought of using named/optional parameters in these methods but I felt it disguised, too much, the intent when calling these methods.  It's easier to see what your call is doing when you see the field enumerations declared in the method calls
