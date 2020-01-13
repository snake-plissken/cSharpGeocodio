# A library to access Geocodio (https://geocod.io/), a geocoding service covering the United States and Canada.

What is geocoding?  Per Wiki (https://en.wikipedia.org/wiki/Geocoding):

*Geocoding is the process of taking input text, such as an address or the name of a place, and returning a latitude/longitude location on the Earth's surface for that place. Reverse geocoding, on the other hand, converts geographic coordinates to a description of a location, usually the name of a place or an addressable location.*

A human readable address turns into a point (latitude and longitude) on the Earth's surface, a process known as forward geocoding.  A point of latitude and longitude turns into a human readable address, a process known as reverse geocoding.  Trust the process.

Why, my good fellow, could this be useful?  Maybe you want to know the distance between Las Vegas and Mackinaw City?  You could reverse geocode the two cities and then use the pair of latitude and longitude corrdinates to calculate the distance.  The applications for this are numerous.

Some code examples.  We can perform individual geocoding operations or send batches.

### Create a client and a field settings object
```c#
var client = new GeoCoderV2("your_api_key");

//Generate the fields settings object to use with our request.
//Geocodio lets you query additional fields such as Census tract or Congressional district.
//We can specify whether to query all of the fields or none of them.  Inidividual fields can be set on or off after creation
var fields = GeocodioDataFieldSettings.CreateDataFieldSettings(true);

//Do not query for 2010 Census information
fields["census2010"] = false;
```

### Forward geocoding:
```c#
//Forward geocode a single address
var forwardGeocodoResults = await client.ForwardGeocodeAsync("3850 S Las Vegas Blvd, Las Vegas, NV 89109", fields);

//Forward geocode a batch of addresses
var someAddresses = new List<string>();
someAddresses.Add("3850 S Las Vegas Blvd, Las Vegas, NV 89109");
someAddresses.Add("2801 Westwood Dr, Las Vegas, NV 89109");
someAddresses.Add("1352 Rufina Cir, Santa Fe, NM 87507");

var batchforwardGeocodoResults = await client.ForwardGeocodeAsync(someAddresses, fields);
```

### Reverse geocoding:
```c#
//Reverse geocode a single point
var reverseGeocodoResults = await client.ReverseGeocodeAsync("39.362136, -74.418693", fields);

//Reverse geocode a batch of points
var someCoordinates = new List<string>();
someCoordinates.Add("39.362136, -74.418693");
someCoordinates.Add("43.080726, -70.740992");
someCoordinates.Add("37.264944, -115.816437");

var batchReverseGeocodoResults = await client.ReverseGeocodeAsync(someCoordinates, fields);
```
