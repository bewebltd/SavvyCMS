using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class Document {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Document object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			PublishDate = DateTime.Today;

			DateAdded = DateTime.Now;
			AddedByPersonID = Security.LoggedInUserID;
			ModifiedByPersonID = Security.LoggedInUserID;
			if (Web.Request["categoryid"] + "" != "") {
				DocumentCategoryID = Web.Request["categoryid"].ToInt();
			}


		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
		public override string GetDefaultOrderBy() {
			string orderBy = " Order By DateModified desc";
			return orderBy;
		}

	/*	public override string GetUrl() {
			return base.GetUrl();
		}
		*/
		public string GetDownloadUrl {
			get{return Web.BaseUrl + "DownloadDocument/" + Crypto.EncryptID(ID);}
		}
		public string GetEditUrl {
			get { return Web.AdminRoot + "DocumentAdmin/Edit/" + ID; }
		}


		public string UploadedByName {
			get {
				var person = AddedByPerson;
				if (person!=null) {
					return person.FullName;
				}
				return "";
			}
		}

		public string UploadedByNameDate {
			get {
				string result = "on " + DateAdded.FmtDateTime();
				var uploadedBy = UploadedByName;
				if (uploadedBy.IsNotBlank()) {
					result += " by " + uploadedBy;
				}
				return result;
			}
		}

		public string ModifiedByName {
			get {
				var person = ModifiedByPerson;
				if (person != null) {
					return person.FullName;
				}
				return "";
			}
		}

		public string ModifiedByNameDate {
			get {
				string result = "on " + DateModified.FmtDateTime();
				var uploadedBy = ModifiedByName;
				if (uploadedBy.IsNotBlank()) {
					result += " by " + uploadedBy;
				}
				return result;
			}
		}


		public static string GetFileType(string filename) {
			string filetype = "";
			String[] extensionArr = filename.Split('.');
			filetype = extensionArr[extensionArr.Length - 1].ToLower();
			return filetype;
		}

		public static string SearchableFieldNames {
			get { return new Document().GetTextFieldNames().Join(","); }
		}

		public static int TotalDownloadCount {
			get { return BewebData.GetValueInt(new Sql("Select count(*) from DocumentDownload")).ToInt(); }
		}
	
	
	public override void Save() {
			//AutocompletePhrase.AddPhrase(this, Fields.Title);
			base.Save();
		}

		public override void Delete() {
			//AutocompletePhrase.DeletePhrase(this);
			base.Delete();
		}
	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestDocument {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 13 May 2013 5:25:37pm ]