using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class SchoolDistricts
	{
		private SchoolDistricts() { }

		[JsonConstructor]
		public SchoolDistricts(SchoolDistrict unified = null, SchoolDistrict elementary = null,
							   SchoolDistrict secondary = null
							  )
		{
			this.Unified = unified;
			this.Elementary = elementary;
			this.Secondary = secondary;
		}

		public SchoolDistrict Unified { get; set; }
		public SchoolDistrict Elementary { get; set; }
		public SchoolDistrict Secondary { get; set; }
	}
}
