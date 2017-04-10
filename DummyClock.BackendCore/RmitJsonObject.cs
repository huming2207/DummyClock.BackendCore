using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DummyClock.BackendCore
{
	[JsonObject]
	public class RmitJsonObject
	{
		[JsonProperty(PropertyName = "timetables")]
		public List<Timetables> Timetables { get; set; }

		[JsonProperty(PropertyName = "is_free")]
		public bool IsFreeDay { get; set; }

		[JsonProperty(PropertyName = "has_error")]
		public bool HasError { get; set; }

		[JsonProperty(PropertyName = "error_msg")]
		public string ErrorMessage { get; set; }
	}

	[JsonObject]
	public class Timetables
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "activity")]
		public string Activity { get; set; }

		[JsonProperty(PropertyName = "subject")]
		public string Subject { get; set; }

		[JsonProperty(PropertyName = "catalog_id")]
		public string CatalogNumber { get; set; }

		[JsonProperty(PropertyName = "start_time")]
		public string StartTime { get; set; }

		[JsonProperty(PropertyName = "end_time")]
		public string EndTime { get; set; }
	}

}
