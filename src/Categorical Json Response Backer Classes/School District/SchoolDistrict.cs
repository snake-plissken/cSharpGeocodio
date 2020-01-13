using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class SchoolDistrict
    {
        private SchoolDistrict() { }

        public SchoolDistrict(string name, string lea_code, string gradelow, string gradehigh)
        {
            this.Name = name;
            this.LEA_Code = lea_code;
            this.GradeLow = gradelow;
            this.GradeHigh = gradehigh;
        }

        public string Name { get; set; }
        [JsonProperty("lea_code")]
        public string LEA_Code { get; set; }
        [JsonProperty("grade_low")]
        public string GradeLow { get; set; }
        [JsonProperty("grade_high")]
        public string GradeHigh { get; set; }
    }


}
