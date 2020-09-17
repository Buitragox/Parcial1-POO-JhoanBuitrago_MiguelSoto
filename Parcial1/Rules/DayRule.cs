using System;

namespace Parcial1.Rules
{
    public class DayRule : IRule
    {
        public bool CheckRule(object value)
        {
            /*string val = value as string;
            int newValue;*/
			//if(Int32.TryParse(val, out newValue))
            if (value is int)
            {
                int newValue = Convert.ToInt32(value);
                return newValue >= 1 && newValue <= 7;
            }
            else
            {
                return false;
            }
        }
    }
}