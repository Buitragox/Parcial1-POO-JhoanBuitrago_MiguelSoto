namespace Parcial1.Rules
{
    public class IntRule : IRule
    {
        public bool CheckRule(object value)
        {
            return value is int;
        }
    }
}