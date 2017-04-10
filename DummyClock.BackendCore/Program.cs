using System;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using RmiterCore;
using RmiterCore.MyRmit;
using Ptv.Timetable;


namespace DummyClock.BackendCore
{
	class MainClass
	{
		public static void Main(string[] args)
		{


			var ptvJsonObject = _GetPtvObject();

			string ptvJson = JsonConvert.SerializeObject(_GetPtvObject());
			string rmitJson = JsonConvert.SerializeObject(_GetRmitJsonObject());

			File.WriteAllText("ptv.json", ptvJson);
			File.WriteAllText("rmit.json", rmitJson);
		}

		private static RmitJsonObject _GetRmitJsonObject()
		{
			// Initialize return object with some default values...
			var rmitJsonObject = new RmitJsonObject()
			{
				Timetables = new List<Timetables>(),
				IsFreeDay = false, // ...so sad lol
				HasError = false
			};

			//   try
			//   {
			// Initialize login object then login
			var casLogin = new CasLogin();
			var casResult = casLogin.RunCasLogin(Settings.RmitID, Settings.RmitPassword).Result;

			// Initialize portal stuff
			var portal = new MyRmitPortal(casResult.CasCookieContainer);
			var timetableResult = portal.GetCurrentClassTimetable().Result;

			// Get the time. In this array, Monday is 0, Sunday is 6.
			int dayOfWeek = (int)((DateTime.Now.DayOfWeek) + 6) % 7; // Shift the day of week



			// Set to list object
			foreach (var timetableForToday in timetableResult.WeeklyTimetable[dayOfWeek].DailyTimetable)
			{
				var tableContent = new Timetables()
				{
					Title = timetableForToday.Title,
					Activity = timetableForToday.ActivityType,
					Subject = timetableForToday.Subject,
					CatalogNumber = timetableForToday.CatalogNumber,
					StartTime = timetableForToday.StartDisplayable,
					EndTime = timetableForToday.EndDisplayable
				};

				rmitJsonObject.Timetables.Add(tableContent);
			}

			if (rmitJsonObject.Timetables.Count == 0)
			{
				rmitJsonObject.IsFreeDay = true;
			}

			//    }

			//    catch (Exception error)
			//    {
			//        rmitJsonObject.HasError = true;
			//        rmitJsonObject.ErrorMessage = error.Message;
			//    }

			return rmitJsonObject;
		}

		private static PtvJsonObject _GetPtvObject()
		{
			var departureList = new PtvJsonObject();

			//     try
			//     {
			var client = new TimetableClient(
			Settings.PtvDeveloperID,
			Settings.PtvSecurityKey,
			(input, key) =>
			{
				var provider = new HMACSHA1(key);
				var hash = provider.ComputeHash(input);
				return hash;
			});

			var searchResults = client.SearchAsync(Settings.PtvStationName).Result;
			var stopResults = (Stop)searchResults[0];

			var lineResults = client.SearchLineByModeAsync(Settings.PtvLineName, TransportType.Train).Result;
			var departResults = client.GetBroadNextDeparturesAsync(TransportType.Train, stopResults.StopID, 3).Result;
			departureList.Departures = new List<Departures>();

			// Use for loop here to limit the result amount to 3. 
			// In fact alothough I've set the amount limit, PTV API always returns more than 3.
			for (int i = 0; i <= 2; i++)
			{
				var departItem = departResults[i];


				departureList.Departures.Add(new Departures()
				{
					Direction = departItem.Platform.Direction.DirectionName,
					EstTime = departItem.EstimatedTime.Value.ToLocalTime().ToString("HH:mm"),
					ScheduleTime = departItem.ScheduledTime.Value.ToLocalTime().ToString("HH:mm"),
					TimeLeft = ((int)((departItem.EstimatedTime.Value.ToLocalTime() - DateTime.Now).TotalMinutes)).ToString()
				});
			}
			//      }
			//      catch(Exception error)
			//      {
			//          departureList.HasError = true;
			//          departureList.ErrorMessage = error.Message;
			//      }

			return departureList;
		}
	}
}
