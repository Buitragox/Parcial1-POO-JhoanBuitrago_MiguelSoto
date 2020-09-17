namespace Parcial1.Rules
{
	public class LengthRule : IRule
	{
		public bool CheckRule(object value)
		{
			string strValue = value as string;
			return strValue.Length >= 4;
		}
	}
}