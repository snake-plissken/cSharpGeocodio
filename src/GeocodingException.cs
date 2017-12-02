using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio
{
    public class GeocodingException : Exception
    {
        private int _returnStatusCode;

        public GeocodingException (int returnStatusCode)
        {
            this._returnStatusCode = returnStatusCode;
        }

        public string SimpleErrorMessage
        {
            get
            {
                if (this._returnStatusCode == 403)
                {
                    return "403 Forbidden.  Probably an invalid API key.";
                }
                else if (this._returnStatusCode == 422)
                {
                    return "422 Unprocessable Entry, i.e. an invalid address";
                }
                else if (this._returnStatusCode == 500)
                {
                    return "500 Internal Server Error.  This one's on Geocodio.";
                }
                else
                {
                    return String.Format("Other Unknown Status Code: {0}", _returnStatusCode);
                }
            }
        }
    }



}
