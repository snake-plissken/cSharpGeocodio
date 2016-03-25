using Newtonsoft.Json;
using System.Collections.Generic;

namespace GeoCodio
{
    public class AddressComponent
    {
        public string Number { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        [JsonProperty("formatted_street")]
        public string FormattedStreet { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }
    
    public class Location
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }
        [JsonProperty("lng")]
        public float Longitude { get; set; }
    }

    public class Fields
    {
        private CongressionalDistrict congressionalDistrict = null;
        private StateLegislature stateLegislature = null;
        private SchoolDistrict schoolDistrict = null;
        private TimeZone timeZone = null;

        [JsonProperty("congressional_district")]
        public CongressionalDistrict CongressionalDistrict
        {
            get
            {
                if (this.congressionalDistrict != null)
                {
                    return this.congressionalDistrict;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.congressionalDistrict = value;
            }
        }

        [JsonProperty("state_legislative_districts")]
        public StateLegislature StateLegislature
        {
            get
            {
                if (this.stateLegislature != null)
                {
                    return this.stateLegislature;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.stateLegislature = value;
            }
        }

        [JsonProperty("school_districts")]
        public SchoolDistrict SchoolDistrict
        {
            get
            {
                if (this.schoolDistrict != null)
                {
                    return this.schoolDistrict;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.schoolDistrict = value;
            }
        }

        public TimeZone TimeZone
        {
            get
            {
                if (this.timeZone != null)
                {
                    return this.timeZone;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.timeZone = value;
            }
        }

    }

    public class CongressionalDistrict
    {
        public string Name { get; set; }
        [JsonProperty("district_number")]
        public int DistrictNumber { get; set; }
        [JsonProperty("congress_number")]
        public string CongressNumber { get; set; }
        [JsonProperty("congress_years")]
        public string CongressYears { get; set; }
    }

    public class Elementary
    {
        public string Name { get; set; }
        [JsonProperty("lea_code")]
        public string LEA_Code { get; set; }
        [JsonProperty("grade_low")]
        public string GradeLow { get; set; }
        [JsonProperty("grade_high")]
        public string GradeHigh { get; set; }
    }

    public class Secondary
    {
        public string Name { get; set; }
        [JsonProperty("lea_code")]
        public string LEA_Code { get; set; }
        [JsonProperty("grade_low")]
        public string GradeLow { get; set; }
        [JsonProperty("grade_high")]
        public string GradeHigh { get; set; }
    }

    public class Unified
    {
        public string Name { get; set; }
        [JsonProperty("lea_code")]
        public string LEA_Code { get; set; }
        [JsonProperty("grade_low")]
        public string GradeLow { get; set; }
        [JsonProperty("grade_high")]
        public string GradeHigh { get; set; }
    }

    public class SchoolDistrict
    {
        private Unified unifiedSchoolDistrict = null;
        private Elementary elementarySchoolDistrict = null;
        private Secondary secondarySchoolDistrict = null;

        public Unified Unified
        {
            get
            {
                if (this.unifiedSchoolDistrict != null)
                {
                    return this.unifiedSchoolDistrict;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.unifiedSchoolDistrict = value;
            }
        }

        public Elementary Elementary
        {
            get
            {
                if (this.elementarySchoolDistrict != null)
                {
                    return this.elementarySchoolDistrict;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.elementarySchoolDistrict = value;
            }
        }

        public Secondary Secondary
        {
            get
            {
                if (this.secondarySchoolDistrict != null)
                {
                    return this.secondarySchoolDistrict;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.secondarySchoolDistrict = value;
            }
        }
    }

    public class TimeZone
    {
        public string Name { get; set; }
        [JsonProperty("utc_offset")]
        public decimal UTC_Offset { get; set; }
        [JsonProperty("observes_dst")]
        public bool Observes_DST { get; set; }
    }

    public class House
    {
        public string Name { get; set; }
        [JsonProperty("district_number")]
        public int DistrictNumber { get; set; }
    }

    public class Senate
    {
        public string Name { get; set; }
        [JsonProperty("district_number")]
        public int DistrictNumber { get; set; }
    }

    public class StateLegislature
    {
        public House House { get; set; }
        public Senate Senate { get; set; }
    }

    public class ForwardGeoCodeAddressComponent
    {
        public string Number { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        [JsonProperty("formatted_street")]
        public string FormattedStreet { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }

    public class ForwardGeoCodeRecord
    {
        [JsonProperty("address_components")]
        public ForwardGeoCodeAddressComponent AddressComponents { get; set; }
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
        public Location Location { get; set; }
        public double Accuracy { get; set; }
        [JsonProperty("accuracy_type")]
        public string AccuracyType { get; set; }
        public string Source { get; set; }
        public Fields Fields { get; set; }

    }

    public class ForwardGeoCodeInput
    {
        [JsonProperty("address_components")]
        public AddressComponent AddressComponents { get; set; }
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
    }

    public class ForwardGeoCodeResult
    {
        public ForwardGeoCodeInput Input { get; set; }
        public ForwardGeoCodeRecord[] Results { get; set; }
    }

    public class ReverseGeoCodedRecord
    {
        [JsonProperty("address_components")]
        public AddressComponent AddressComponents { get; set; }
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
        public Location Location { get; set; }
        public double Accuracy { get; set; }
        [JsonProperty("accuracy_type")]
        public string AccuracyType { get; set; }
        public string Source { get; set; }
        public Fields Fields { get; set; }
    }

    public class SingleReverseGeoCodeResult
    {
        public ReverseGeoCodedRecord[] Results { get; set; }
    }

    public class BatchReverseGeoCodedResponse
    {
        public ReverseGeoCodedRecord[] Results { get; set; }
    }

    public class BatchReverseGeoCodingResult
    {
        public string Query { get; set; }
        public BatchReverseGeoCodedResponse Response { get; set; }
    }

    public class BatchReverseGeoCoding
    {
        public BatchReverseGeoCodingResult[] Results { get; set; }
    }
}
