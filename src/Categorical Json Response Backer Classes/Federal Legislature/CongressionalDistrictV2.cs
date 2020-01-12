using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class CongressionalDistrictV2
	{
		[JsonConstructor]
		public CongressionalDistrictV2(string name, int districtnumber
		                              , string congressnumber, string congressyears
		                              , int proportion
		                               , FederalLegislator[] currentlegislators)
		{
			Name = name;
			DistrictNumber = districtnumber;
			CongressNumber = congressnumber;
			CongressYears = congressyears;
			Proportion = proportion;
			CurrentLegislators = currentlegislators;
		}

		public string Name { get; set; }
		[JsonProperty("district_number")]
		public int DistrictNumber { get; set; }
		[JsonProperty("congress_number")]
		public string CongressNumber { get; set; }
		[JsonProperty("congress_years")]
		public string CongressYears { get; set; }
		public int Proportion { get; set; }
		[JsonProperty("current_legislators")]
		public FederalLegislator[] CurrentLegislators { get; set; }
	}
}
