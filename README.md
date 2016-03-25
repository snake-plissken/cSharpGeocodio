## A wrapper to access Geocodio, a geocoding service.  

#See: https://geocod.io/

Usage:
'''
//Single forward geocode:
string singleAddress = "2100 East Market Street, Philadelphia, PA 19103";
GeoCoder geoCoder = new geoCoder('Your Grocodio API key.  You get 2500 free lookups per day!');
SingleForwardGeoCodeResult geoCodedAddress = geoCoder.ForwardGeoCodeSync(singleAddress);

//If Results array is length 0, no results were found...
Location latLong = geoCodedAddress.Results[0].Location;

float addressLatitude = latLong.Latitude;
float addressLongitude = latLong.Longitude;
'''

To-Do:
- [x] Forward Geocoding Methods - Done - 3-25-16
- [ ] Better Error Handling For Asynchronous Methods
- [ ] Unit Testies
- [ ] Cleanup of JSON backer classes, they are kind of messy
