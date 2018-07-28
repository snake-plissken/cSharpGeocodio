using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class StateLegislature
	{
		private StateLegislature() { }

		[JsonConstructor]
		public StateLegislature(StateLegislator house, StateLegislator senate)
		{
			this.House = house;
			this.Senate = senate;
		}

		public StateLegislator House { get; set; }
		public StateLegislator Senate { get; set; }
	}
}
