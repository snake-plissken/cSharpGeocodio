using System;
using System.Threading.Tasks;

namespace cSharpGeocodio
{
	public class GeoCodioServerResponse
	{
		private TaskStatus _taskStatus;
		private int _serverResponseCode;
		private string _rawJsonResponseString;

		private GeoCodioServerResponse(TaskStatus taskStatus, int serverResponse, string rawJson)
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

		public static GeoCodioServerResponse MakeServerResponse(
			TaskStatus taskStatus, int serverResponse, string rawJson)
		{
			//Aren't the task status and server response mutally exlusive?
			//I.e. if the task faults, we don't get a server response
			//If the task completes, we do?
			return new GeoCodioServerResponse(taskStatus, serverResponse, rawJson);

		}

	}
}
