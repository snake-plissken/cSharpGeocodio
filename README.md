# A library to access Geocodio (https://geocod.io/), a geocoding service.  

What is geocoding?  Per Wiki (https://en.wikipedia.org/wiki/Geocoding):

*Geocoding is the process of taking input text, such as an address or the name of a place, and returning a latitude/longitude location on the Earth's surface for that place. Reverse geocoding, on the other hand, converts geographic coordinates to a description of a location, usually the name of a place or an addressable location.*

A human readable address turns into a point (latitude and longitude) on the Earth's surface, a process known as forward geocoding.  A point of latitude and longitude turns into a human readable address, a process known as reverse geocoding.  Trust the process.

Why, my good fellow, could this be useful?  Maybe you want to know the distance between Las Vegas and Mackinaw City?  You could reverse geocode the two cities and then use the pair of latitude and longitude corrdinates to calculate the distance.  The applications for this are numerous.

Some code examples:

```c#
//Create a client object with your API key
var client = new GeoCoderV2("your_api_key");

//Generate the fields settings object to use with our request.
//Geocodio lets you query additional fields such as Census tract or Congressional district.
//We can specify whether to query all of the fields or none of them.  Inidividual fields can be set after creation
var fields = GeocodioDataFieldSettings.CreateDataFieldSettings(true);
//Do not query for 2010 Census information
fields["census2010"] = false;

//Send the request!
var forwardGeocodoResults = await client.ForwardGeocodeAsync("3850 S Las Vegas Blvd, Las Vegas, NV 89109", fields);
```
