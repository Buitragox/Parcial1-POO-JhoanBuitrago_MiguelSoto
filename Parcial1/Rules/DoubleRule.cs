namespace Parcial1.Rules
{
    public class DoubleRule : IRule
    {
        public bool CheckRule(object value)
        {
            return value is double;
        }
    }
}