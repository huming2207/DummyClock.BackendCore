using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DummyClock.BackendCore
{
	[JsonObject]
	public class PtvJsonObject
	{
		[JsonProperty(PropertyName = "departures")]
		public List<Departures> Departures { get; set; }

        [JsonProperty(PropertyName = "has_error")]
        public bool HasError { get; set; }

        [JsonProperty(PropertyName = "error_msg")]
        public string ErrorMessage { get; set; }
	}

	[JsonObject]
	public class Departures
	{
		[JsonProperty(PropertyName = "direction")]
		public string Direction { get; set; }

		[JsonProperty(PropertyName = "est_time")]
		public DateTime? EstTime { get; set; }

		[JsonProperty(PropertyName = "schedule_time")]
		public DateTime? ScheduleTime { get; set; }

		[JsonProperty(PropertyName = "time_left")]
		public string TimeLeft { get; set; }
	}

}
