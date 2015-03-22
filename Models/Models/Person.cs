using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class Person {
		public string passwordConfirmed;
		public string pwValue=null;

		public void Init() {
			if(Security.IsSuperAdminAccess||Security.LoggedInUserID==this.ID) {
				pwValue=Security.DecryptPassword(Security.GetStoredPasswordFromDatabase(ID));
			}
			
		}

		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Person object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			IsActive = true;
			DateAdded = DateTime.Now;
			//StartDate = DateTime.Now;
			Role = SecurityRolesCore.Roles.ADMINISTRATOR;
			pwValue = Beweb.RandomPassword.Generate(8);

		}
		public string FullName{get{return FirstName+" "+LastName;}}
		
		// You can put any business logic associated with this entity here
		

		public bool EmailAlreadyInUse() {
			var check = BewebData.GetValue(new Sql("SELECT TOP 1 PersonID FROM Person WHERE Email=", Email.SqlizeText()));
			return check.IsNotBlank();
		}
	}
}

// created: [ 18-May-2010 9:08:24am ]