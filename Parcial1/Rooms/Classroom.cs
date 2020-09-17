using Parcial1.Reservations;
using System.Collections.Generic;

namespace Parcial1.Rooms
{
    public class Classroom
    {
		#region Properties
        public string Id {get; set;}
		public int OnLight {get; set;}
		public int OffLight {get; set;}
		public double AirTemp {get; set;}
		public int OnAir {get; set;}
		public int OffAir {get; set;}
		//Reservation
		public bool InMaintenance {get; set;}
		public int Open {get; set;}
		public List<Reservation> ReservationList;
		#endregion

		#region Initialize
		public Classroom(){}

		public Classroom(string id)
		{
			Id = id;
			OnLight = 5;
			OffLight = 10;
			AirTemp = 23;
			OnAir = 10;
			OffAir = 5;
			InMaintenance = false;
			Open = 15;
			ReservationList = new List<Reservation>();
		}
		#endregion
    }
}