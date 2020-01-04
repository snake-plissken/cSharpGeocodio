using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio
{

    public class GeocodioDataFieldSettings
    {
        //https://stackoverflow.com/questions/55485750/how-can-i-create-a-constant-hashset-in-c-sharp
        public static readonly IEnumerable<string> ValidGeocodioFields = new HashSet<string>()
        {
            "cd"
            , "cd113"
            , "cd114"
            , "cd115"
            , "cd116"
            , "stateleg"
            , "school"
            , "census"
            , "census2010"
            , "census2011"
            , "census2012"
            , "census2013"
            , "census2014"
            , "census2015"
            , "census2016"
            , "census2017"
            , "census2018"
            , "census2019"
            , "acs-demographics"
            , "acs-economics"
            , "acs-families"
            , "acs-housing"
            , "acs-social"
            , "timezone"

        };

        private Dictionary<string, bool> _fieldSettings { get; set; }

        private GeocodioDataFieldSettings(Dictionary<string, bool> fieldSettings)
        {
            _fieldSettings = fieldSettings;
        }

        /// <summary>
        /// Create a fields object to use when sending requests to Geocodio.  This object controls which data 
        /// fields (census, Congress, etc.) are queried when sending requests.  Note: if you set true for all fields,
        /// "cd" and "census" will be disabled.  Geocodio treats each as meaning data for the current year (i.e. if you sent
        /// a query in 2019, fields "census2019" and "census" both refer to the same 2019 census.)
        /// </summary>
        /// <param name="defaultAllFieldsStatusToInclude"></param>
        /// <returns></returns>
        public static GeocodioDataFieldSettings CreateDataFieldSettings(bool defaultAllFieldsStatusToInclude = false)
        {
            Dictionary<string, bool> FieldSettings = new Dictionary<string, bool>();

            if (defaultAllFieldsStatusToInclude)
            {
                foreach (var key in ValidGeocodioFields)
                {
                    FieldSettings.Add(key, defaultAllFieldsStatusToInclude);
                }

                //Disable these two fields because we set all fields to true; don't want to dupe requests
                FieldSettings["cd"] = false;
                FieldSettings["census"] = false;
            }
            else
            {
                foreach (var key in ValidGeocodioFields)
                {
                    FieldSettings.Add(key, defaultAllFieldsStatusToInclude);
                }
            }

            return new GeocodioDataFieldSettings(FieldSettings);
        }

        public void SetFieldQueryStatus(string fieldKey, bool includeWhenQuerying)
        {
            if (ValidGeocodioFields.Contains(fieldKey))
            {
                _fieldSettings[fieldKey] = includeWhenQuerying;
            }
            else
            {
                throw new InvalidOperationException($"Field key {fieldKey} is not currently a valid Geocodio version {GeoCoderV2.ClientGeocodioApiVersion} data field key.");
            }
        }

        public bool GetFieldQueryStatus(string fieldKey)
        {
            if (ValidGeocodioFields.Contains(fieldKey))
            {
                return _fieldSettings[fieldKey];
            }
            else
            {
                throw new InvalidOperationException($"Field key {fieldKey} is not currently a valid Geocodio version {GeoCoderV2.ClientGeocodioApiVersion} data field key.");
            }
        }

        public bool this[string fieldKey]
        {
            get { return _fieldSettings.ContainsKey(fieldKey) && _fieldSettings[fieldKey]; }
            set
            {
                if (ValidGeocodioFields.Contains(fieldKey))
                {
                    _fieldSettings[fieldKey] = value;
                }
                else
                {
                    throw new InvalidOperationException($"Field key {fieldKey} is not currently a valid Geocodio version {GeoCoderV2.ClientGeocodioApiVersion} data field key.");
                }
            }
        }
    }
}
