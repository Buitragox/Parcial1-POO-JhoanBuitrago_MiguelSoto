using Parcial1.TimeRepresentation;

namespace Parcial1.Rules
{
    public class TimeRule : IRule
    {
        public bool CheckRule(object value)
        {
            if (value is Time)
            {
                Time newTime = value as Time;
                return newTime.Hour >= 7 && newTime.Hour <= 19 && newTime.Minute < 60 && newTime.Minute >= 0;
            }
            else
            {
                return false;
            }
        }
    }
}