namespace Parcial1.Users
{
	public class User
    {
		#region Properties
        public string Name {get; set;}
        protected string password;
        #endregion
        
		#region Getters & Setters
		public string Password
		{
			get {return password;}
		}
		#endregion

		#region Initialize
		public User(){}
		public User(string name, string pass)
		{
			Name = name;
			password = pass;
		}
		#endregion
    }
}