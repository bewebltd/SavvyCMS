using System;
using Beweb;
using Models;
using Site.Controllers;
namespace Site.SiteCustom {

/// <summary>
/// Stores session variables for the application SessionCustomData each application has to implent this
/// </summary>
public class UserSession {

	//public static Models.Person Person {
	//  get { return (Person)Web.Session["UserPerson"]; }
	//  set { Web.Session["UserPerson"]=value; }
	//}

	public static string UserName {
		get {
			if (Person == null) {
				return "";
			}
			return Person.FullName;     // change this to whatever you like
		}
	}

	public static Models.Person Person {
		get {
			Person result = null;
			if (Web.Request.RawUrl.DoesntContain("scheduledtask")&& Security.IsLoggedIn) {
				if (Web.Session["LoggedInPerson"] != null) {
					return Person.LoadID(Crypto.DecryptID(Web.Session["LoggedInPerson"] + ""));
				}
				//if (Web.Session["LoggedInPerson"] == null) {
				//  Web.Session["LoggedInPerson"] = Person.LoadID(Security.LoggedInUserID);
				//}
				//result = (Person)Web.Session["LoggedInPerson"];
				// MN 20111111 - dont store in session as this does not update properly when someone else bids on your auction - now storing in cache instead
				result = Person.LoadID(Security.LoggedInUserID);
				if (result == null) {
					Web.ErrorMessage = "User not found";
					Web.Redirect(Web.Root+"Security/Logout");
				}
			}
			//if (result==null) {
			//  // default to a blank user - this is useful if getting properties eg UserSession.Person.Email will always exist and will be just null for non-logged-in users 
			//  result = new Person();
			//}
			return result;
		}
	}

	public static string Role {
		get {
			if (Person == null) {
				return "";
			}
			return Person.Role;
		}
	}

		



	// example of checking in role - just a shortcut property
	//public static bool IsSupplier {
	//	get { return Security.IsInRole(SecurityRoles.Roles.SUPPLIER); }
	//}

	// example string session var
	//public static string LastUrl {
	//	get { return Convert.ToString(Web.Session["LastUrl"]); }
	//	set { Web.Session["LastUrl"] = value.ToString(); }
	//}

	// example object session var
	//public static Models.Client CurrentClient {
	//  get {
	//    if (Web.Session["CurrentClient"]==null) {
	//      Web.Session["CurrentClient"] = // load or create object;
	//    }
	//    return (Models.Client)Web.Session["CurrentClient"];
	//  }
	//	set { Web.Session["CurrentClient"] = value; }
	//}


	}
}

