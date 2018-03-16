using System;
using System.Threading.Tasks;

namespace cSharpGeocodio
{
	public class GeoCodioServerResponse
	{
		private TaskStatus _taskStatus;
		private int _serverResponseCode;
		private string _rawJsonResponseString;

		public GeoCodioServerResponse(TaskStatus taskStatus, int serverResponse, string rawJson)
		{
			this._taskStatus = taskStatus;
			this._serverResponseCode = serverResponse;
			this._rawJsonResponseString = rawJson;
		}

		public TaskStatus TaskStatus
		{
			get { return this._taskStatus; }
		}

		public int ServerResponseCode
		{
			get { return this._serverResponseCode; }
		}

		public string RawJsonResponse
		{
			get { return this._rawJsonResponseString; }
		}

	}
}
