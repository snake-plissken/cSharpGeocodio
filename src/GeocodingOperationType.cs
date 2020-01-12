using System;
namespace cSharpGeocodio
{
    /// <summary>
    /// Enum used to specift the kind of geocoding operation being performed.  Used interally by the geocoder client.
    /// </summary>
	public enum GeocodingOperationType
	{
		SingleForward
		, BatchForward
		, SingleReverse
		, BatchRevsere
	}
}
