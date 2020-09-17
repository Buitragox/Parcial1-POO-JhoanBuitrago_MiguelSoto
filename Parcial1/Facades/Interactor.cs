using System.Collections.Generic;
using System;
using Parcial1.Users;
using Parcial1.Rooms;
using Parcial1.Reservations;
using Parcial1.TimeRepresentation;
using Parcial1.Validators;
using Parcial1.Rules;

namespace Parcial1.Facades
{
    public class Interactor
    {
        #region Methods
        public void Reserve(Classroom room, int day, Time start, Time end, string name, string description)
        {
            Time newStart = new Time()
            {
                Hour = start.Hour,
                Minute = start.Minute
            };
            Time newFinish = new Time()
            {
                Hour = end.Hour,
                Minute = end.Minute
            };
            Reservation reserve = new Reservation()
            {
                Day = day,
                Start = newStart,
                Finish = newFinish,
                Name = name,
                Description = description
            };
            room.ReservationList.Add(reserve);
        }

        public void ModifyAir(Classroom room, double temp, int newOn, int newOff)
        {
            room.AirTemp = temp;
            room.OnAir = newOn;
            room.OffAir = newOff;
        }

        public void ModifyLight(Classroom room, int newOn, int newOff)
        {
            room.OnLight = newOn;
            room.OffLight = newOff;
        }

        public void ModifyOpen(Classroom room, int newOpen)
        {
            room.Open = newOpen;
        }

        public void Maintenance(Classroom room)
        {
            room.InMaintenance = !room.InMaintenance;
            room.ReservationList.Clear();
        }
        #endregion
    }
}