# Version 2.0 - 20190114

## High Level Overview
  - Support for Geocodio's v1.4 API
  - Added support for the new Geocodio data fields i.e. ACS, Canadian Census
  - Completely revamped the way additional data fields are queried
  - Improved inner workings of Geocoder client 

## Geocodio Related Changes
  - Added support for ACS Fields
    - Check the documentation as these fields are internally stored as a dictionary of dictionaries due to the way the data is organized
  - Added support for Canadian census fields
  - Added support for _warnings field
  
## cSharpGeocodio Changes
  ### Minor
  - Adjusted wrapper helper methods which transform responses from Geocodio into dictionaries; now they just append duplicate queries to the response collections
  - Removed some detritus i.e. any objects/models which are no longer nedded
  ### Major
  - Geocoder client
    - Changed it to use a single HttpClient per instance, before it was creating a new HttpClient on each method call
    - Cdded better error handling so any non 200 OK Http status codes will throw
    - Changed all geocoding methods to use new field settings object which controls how the client queries for additional data fields (census, congressional district, etc.)
  - GeocodioDataFieldSettings
    - This object now controls how the additional data fields are queried
    - Validates setting and checking the status of the fields against known and valid Geocodio fields
    - Each geocodiong method on the client takes one of these objects as a parameter; we not longer pass in an enum for each additional data field
