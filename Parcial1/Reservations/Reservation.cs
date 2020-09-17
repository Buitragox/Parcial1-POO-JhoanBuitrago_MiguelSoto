using Parcial1.TimeRepresentation;

namespace Parcial1.Reservations
{
    public class Reservation
    {
        #region Properties
        public int Day {get; set;}
        public Time Start {get; set;}
        public Time Finish {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        #endregion
    }
}