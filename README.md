# A library to access Geocodio (https://geocod.io/), a geocoding service covering the United States and Canada.

What is geocoding?  Per Wiki (https://en.wikipedia.org/wiki/Geocoding):

*Geocoding is the process of taking input text, such as an address or the name of a place, and returning a latitude/longitude location on the Earth's surface for that place. Reverse geocoding, on the other hand, converts geographic coordinates to a description of a location, usually the name of a place or an addressable location.*

A human readable address turns into a point (latitude and longitude) on the Earth's surface, a process known as forward geocoding.  A point of latitude and longitude turns into a human readable address, a process known as reverse geocoding.  Trust the process.

Why, my good fellow, could this be useful?  Maybe you want to know the distance between Las Vegas and Mackinaw City if you were to walk in a direct path along the Earth's surface.  You could reverse geocode the two cities and then use the pair of latitude and longitude corrdinates to calculate the distance.  There are many applications for the information provided through the geocoding process.  Uber and Lyft and Waze and Google Maps could not work without it.

Some code examples.  We can perform individual geocoding operations or send batches.

### Create a client and a field settings object
```c#
var client = new GeoCoderV2("your_api_key");

//Generate the fields settings object to use with our request.
//Geocodio lets you query additional fields such as Census tract or Congressional district.
//We can specify whether to query all of the fields or none of them.  Inidividual fields can be set on or off after creation
//The default is false, so we use true here to turn them all on and query everything!
var fields = GeocodioDataFieldSettings.CreateDataFieldSettings(true);

//Do not query for 2010 Census information
fields["census2010"] = false;
```

### Forward geocoding
```c#
//Forward geocode a single address
var forwardGeocodoResults = await client.ForwardGeocodeAsync("3850 S Las Vegas Blvd, Las Vegas, NV 89109", fields);

//Forward geocode a batch of addresses
var someAddresses = new List<string>();
someAddresses.Add("3850 S Las Vegas Blvd, Las Vegas, NV 89109");
someAddresses.Add("2801 Westwood Dr, Las Vegas, NV 89109");
someAddresses.Add("1352 Rufina Cir, Santa Fe, NM 87507");

var batchforwardGeocodoResults = await client.ForwardGeocodeAsync(someAddresses, fields);

//Get the coordinates.  ForwardGeocodeAsync returns the same type whether single or batch geocoding.
//The GeoCodeInfo item in forwardGeocodoResults.Results[0].Response.Results[0] conains a lot of additional information.
var latitude = forwardGeocodoResults.Results[0].Response.Results[0].Location.Latitude;
var longitude = forwardGeocodoResults.Results[0].Response.Results[0].Location.Longitude;
```

### Reverse geocoding
```c#
//Reverse geocode a single point
var reverseGeocodoResults = await client.ReverseGeocodeAsync("39.362136, -74.418693", fields);

//Reverse geocode a batch of points
var someCoordinates = new List<string>();
someCoordinates.Add("39.362136, -74.418693");
someCoordinates.Add("43.080726, -70.740992");
someCoordinates.Add("37.264944, -115.816437");

var batchReverseGeocodoResults = await client.ReverseGeocodeAsync(someCoordinates, fields);

//Get the address.  ReverseGeocodeAsync returns the same type whether single or batch geocoding.
//The GeoCodeInfo item in reverseGeocodoResults.Results[0].Response.Results[0] conains a lot of additional information.
var address = reverseGeocodoResults.Results[0].Response.Results[0].FormattedAddress;
```

### Notes On The Results From Geocodio
The result items contain a field `Accuracy` which describes the accuracy of the item in the results.  The array is ordered so the most accurate results are listed first.  Geocoding is not a perfect science.  Depending on the areas you query, your results might be 100% accurate of off by a few hundred feet or two miles.  This is due to the way geocoding algorithms work (and all of them are susecptible to erronerous outputs).  In less densely popuated areas, the accuracy can vary more often.

### Exceptions
Both forward and reverse geocoding methods will throw a `GeocodingException` if Geocodio's servers return anything but a 200 OK status code.  This could be for a few reasons, the most common being 403s (check the API key), 422s (you sent something Geocodio can't handle) or 500s (error on Geocodio's side).  These exceptions will bubble up via an `AggregateException` so check the inner exception collection.

### Additional Data Fields
Geocodio offers a collection of additional data fields (Census, Congress, Income, etc) associated with the points or addresses you are geocoding.  The `GeocodioDataFieldSettings` class is used to handle querying these additional data fields.  `GeocodioDataFieldSettings` objects can be created using its static contructor.  By default, all fields are set to a query status of false, which means no additional fields will be queried.  A status of true can be set for all of them when creating a fields object, and you can also turn on/off querying of the individual fields.  A static property `ValidGeocodioFields` contains the valid data field keys.  An exception will be thrown if you try to set or check a key which is not a valid Geocodio data field.  

Set them to true or false dependning on your needs:
```c#
//Indexer syntax:
fields["census2010"] = true;
fields["census2015"] = false;
//Method syntax:
fields.SetFieldQueryStatus("census2011", true);
fields.SetFieldQueryStatus("acs-economics", true)

//We can also check the status of fields.
var toBeQueried = fields.GetFieldQueryStatus("census2011");

//Exception will be thrown, because census1939 is not a valid Geocodio data field key.
var toBeQueried_Exception = fields.GetFieldQueryStatus("census1939");
```

### Note on The American Community Survey (ACS) Data Architecture
The American Community Survey (ACS) is a more frequent but less intense version of the 10 year Census, gathering the same kinds of info but carried out on a regular basis.  See https://en.wikipedia.org/wiki/American_Community_Survey for details.

Geocodio allows you to query a few ACS datasets which include information on topics like income and housing.  Due to the way this data is categorized, each high level category (economics, deomographics, etc) is exposed via a property.  The property exposes a dictionay of dictionaries, where the top level key will be a general grouping, e.g. "Population by age range".  The inner keys are the specific data items with their respective values ("Male: 60 and 61 years", "Female: 30 to 34 years").  The benefit of doing it like this is flexibility: we don't need to have hudreds of properties for every kind of data grouping, and if Geocodio integrates more ACS groupings, they will flow into the dictionaries.

An example, assuming we've queried for the `acs-economics`:

```c#
var forwardGeocodoResults = await client.ForwardGeocodeAsync("3850 S Las Vegas Blvd, Las Vegas, NV 89109", fields);
var result = forwardGeocodoResults.Results[0].Response.Results[0];
//Put in a break point to examine the dictionary exposed by Economics and see how many keys and groupings we get, it's quite a bit!
var dataItem = result.Fields.ACS_Results.Economics["Household Income"]["$60,000 to $74,999"];
```

