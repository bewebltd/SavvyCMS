using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beweb;
using RestSharp;
using RestSharp.Serializers;
using Site.SiteCustom;
using Ubiquity.uSuite3.ApiV2.Public.Args.Forms;
using Ubiquity.uSuite3.ApiV2.Public.Database;
using Ubiquity.uSuite3.ApiV2.Public.Forms;
using Http = Beweb.Http;

namespace Site.Controllers {
	public class UbiquityEngage : Controller {

		private FormPostArgs formFields;

		public string APIToken { get; set; }
		public string FormID { get; set; }
		private List<EngageField> EngageFields { get; set; }
		private List<EngageField> EngageCourseFields { get; set; }
		//public string referenceID { get { return formFields.ReferenceID; } set { formFields.ReferenceID = value; } }
		//public bool disableAutoComms { get { return formFields.DisableAutoComms; } set { formFields.DisableAutoComms = value; } }
		//public bool disableContactHistory { get { return formFields.DisableContactHistory; } set { formFields.DisableContactHistory = value; } }
		//public string itemXml { get { return formFields.ItemXml; } set { formFields.ItemXml = value; } }
		//public string source { get { return formFields.Source; } set { formFields.Source = value; } }
		//public string formAction { get { return formFields.FormAction; } set { formFields.FormAction = value; } }

		public UbiquityEngage() {
			APIToken = Util.GetNamedSetting("EngageAPIToken", "");
			FormID = Util.GetNamedSetting("EngageAPIFormIDAddOrUpdate", "");

			EngageFields = new List<EngageField> {
				new EngageField { DBFieldID = "l_3Jjp7HgkueNwjRbYA1Ww", FormFieldID = "", Name = "Contact ID", BewebName = "EngageContactID" },
				new EngageField { DBFieldID = "wo4oW_ObGU-nZwjRbYA-nw", FormFieldID = "lo124vTBvEG3iQjRmZFWyA", Name = "Contact Type", BewebName = "ContactType" },
				new EngageField { DBFieldID = "wJFZJDNtNU-8fAjRbYBLjA", FormFieldID = "0hzbZz3wqk2f9wjRmZFjOw", Name = "First Name", BewebName = "FirstName" },
				new EngageField { DBFieldID = "Zw0zz0EbDUugJgjRbYBQIg", FormFieldID = "k6aMm7O330G4owjRmZFl_g", Name = "Last Name", BewebName = "LastName" },
				new EngageField { DBFieldID = "5rf7K_4uQEiSWgjRbYMhBg", FormFieldID = "cPSvElZpL0WrgQjRmZGS_w", Name = "Registration No", BewebName = "PGDBNumber" },
				new EngageField { DBFieldID = "Pip9yYs0c0WJbQjRbYDt-w", FormFieldID = "npYquLX3pESkQAjRmZGYvQ", Name = "Email", BewebName = "Email" },
				new EngageField { DBFieldID = "mVdauM09TkS5bwjRbYDlAg", FormFieldID = "3fgwbcsYukO9lQjRmZGWmA", Name = "Mobile", BewebName = "Phone" },
				new EngageField { DBFieldID = "fYB3aelgskC8sgjRbYBWjg", FormFieldID = "Bd6TjIDdmkm4SwjRmZF-sQ", Name = "Company Name", BewebName = "Company" },
				new EngageField { DBFieldID = "LT73Tz1Qh0W1yQjRbYD53w", FormFieldID = "R-7wq7zd-UimyAjRmZGphw", Name = "Address 1", BewebName = "StreetAddress" },
				new EngageField { DBFieldID = "rmPG4foGJESVnQjRbYFf2A", FormFieldID = "yF3sGxfcb0C1ggjRmZGrvA", Name = "Address 2", BewebName = "Suburb" },
				new EngageField { DBFieldID = "Mt0vgcmgJUiywwjRbYFuKw", FormFieldID = "06gPciEXI0-lggjRmZGvCg", Name = "City", BewebName = "City" },
				new EngageField { DBFieldID = "hCTx3yrx_0K9mwjRbYF0GA", FormFieldID = "H7dwS0yrOEmstQjRmZGxLQ", Name = "Postcode", BewebName = "PostCode" },
				new EngageField { DBFieldID = "DLSIkV41M0afbgjRbYMEhg", FormFieldID = "ChMGkE5Jo0GLJQjRmZGy0g", Name = "Country", BewebName = "Country" },
				new EngageField { DBFieldID = "DTpTQ8eZMEmZcgjRn9vnwQ", FormFieldID = "", Name = "Region", BewebName = "Region" },
				new EngageField { DBFieldID = "ZSLc4Q-w_kOhewjReU0w-A", FormFieldID = "oDsuOBpXgUmvkAjRmZHRgQ", Name = "Role", BewebName = "Role" },
				new EngageField { DBFieldID = "MOsQ9S2tTEqA9gjRbYCfBA", FormFieldID = "RzS_djSo20GJ2wjRmZHOTw", Name = "Job Title", BewebName = "Occupations" },
				new EngageField { DBFieldID = "8LoG2t4gt0OOVwjRbYMxDw", FormFieldID = "YHm7Fdc-mkertwjRmZHJcA", Name = "Preferred Supplier", BewebName = "PreferredBrand" },
				new EngageField { DBFieldID = "I_N2gTsw3UimwQjRbYM_AA", FormFieldID = "", Name = "Preferred Supplier - Specific", BewebName = "PreferredBrand" },
				new EngageField { DBFieldID = "aSLntSasmE-wsgjRbYQJ2w", FormFieldID = "SEqTdI_PO0CP5AjRmZHdDQ", Name = "Email Permission", BewebName = "OptInEmail" },
				new EngageField { DBFieldID = "4pehkcbvt02-ugjRbYQZ6w", FormFieldID = "lm9GrTuYmUKUewjRmZHgtQ", Name = "Mobile Permission", BewebName = "OptInSMS" },
				new EngageField { DBFieldID = "M6J88QrpGEeRkAjRmZIb2g", FormFieldID = "h0ie89cDUU64DQjRmZIeew", Name = "Interests", BewebName = "Interests" },
				new EngageField { DBFieldID = "RTEPM7By5UqZrQjRkC3aAg", FormFieldID = "", Name = "Opted Out", BewebName = "" },
				new EngageField { DBFieldID = "lIWpX0Vgdkah1AjR0uhc8g", FormFieldID = "", Name = "Tradesmart Password", BewebName = "" },
				new EngageField { DBFieldID = "KBzozbOx8UKYHwjR0uiMHg", FormFieldID = "", Name = "Tradesmart Password Created", BewebName = "" },
				new EngageField { DBFieldID = "C37GjjDTz0mnxQjR0uh7TA", FormFieldID = "", Name = "Tradesmart Last Logged In Date", BewebName = "" },
				new EngageField { DBFieldID = "YEiPwawEykieywjRYoqGVw", FormFieldID = "", Name = "Last Modified", BewebName = "" },
				new EngageField { DBFieldID = "NhyPiIzu_EG7ZAjSBHSlvg", FormFieldID = "", Name = "Registration Form Source", BewebName = "Source" }
			};

			EngageCourseFields = new List<EngageField> {
				new EngageField { DBFieldID = "jpn5KJEoJ0WZvwjRqqkRLQ", Name = "Reference ID", BewebName = "EngageReferenceID" },
				new EngageField { DBFieldID = "S5aUHb-d2kmnAgjRqqk9ow", Name = "Course Name", BewebName = "CourseName" },
				new EngageField { DBFieldID = "MtabnyroyUyM-gjRqqlBgQ", Name = "Status", BewebName = "Status" },
				new EngageField { DBFieldID = "reakb1cYZUWkggjRqucswA", Name = "Date Started", BewebName = "DateStarted" },
				new EngageField { DBFieldID = "OH3vdSE2OU63kgjRquc6GA", Name = "Date Completed", BewebName = "DateCompleted" }
			};

		}

		public class EngageField {
			public string DBFieldID;
			public string FormFieldID;
			public string Name;
			public string BewebName;
		}

		public List<dynamic> GetEngageContacts() {
			var engageContacts = new List<dynamic>();

			const string apiBaseUrl = "https://api.ubiquity.co.nz";
			var url = string.Format("{0}/database/contacts?format=json&apiToken={1}", apiBaseUrl, APIToken);
			var json = Http.Get(url);

			dynamic obj = JsonHelper.Parse(json);

			// Load all contacts from Engage
			while (obj.totalReturned > 0) {
				foreach (var selectedContact in obj.selectedContacts) {
					engageContacts.Add(selectedContact);
				}
				json = Http.Get(url + "&skip=" + obj.next);
				obj = JsonHelper.Parse(json);
			}

			return engageContacts;
		}

		public string GetValueByFieldName(dynamic data, string fieldName) {
			var fieldID = GetFieldID(fieldName);
			foreach (var field in data) {
				if (field.fieldID == fieldID) {
					return field.value;
				}
			}
			return null;
		}

		public string GetFieldID(string fieldName) {
			return EngageFields.Where(f => f.Name == fieldName).Select(f => f.DBFieldID).FirstOrDefault();
		}

		public string GetFormFieldID(string fieldName) {
			return EngageFields.Where(f => f.Name == fieldName).Select(f => f.FormFieldID).FirstOrDefault();
		}

		public string GetCourseFieldID(string fieldName) {
			return EngageCourseFields.Where(f => f.Name == fieldName).Select(f => f.DBFieldID).FirstOrDefault();
		}

		public dynamic GetPersonByEmail(string email) {
			//Build a new Base Rest Client 
			APIToken = Util.GetNamedSetting("EngageAPIToken");
			var collection = new WebHeaderCollection();
			collection.Add("Authorization", string.Format("Engage {0}", APIToken));
			string filterString = "{\"filterString\": \"[" + GetFieldID("Email") + "] eq '" + email + "'\"}";
			var content = Http.Post("https://api.ubiquity.co.nz/database/contacts/query/", filterString, "Authorization", string.Format("Engage {0}", APIToken));

			var json = content;
			dynamic obj = JsonHelper.Parse(json);
			return obj.totalReturned > 0 ? obj.selectedContacts[0] : null;
		}

		/// <summary>
		/// Create or Update a contact
		/// </summary>
		/// <param name="referenceID"></param>
		/// <returns></returns>
		public Contact Send(Guid? referenceID) {
			//Build a new Base Rest Client 
			var client = new RestClient("https://api.ubiquity.co.nz");
			//create a request
			RestRequest request;

			// New contact
			if (referenceID == null) {
				request = new RestRequest("database/contacts", Method.POST);
			} else {
				request = new RestRequest("database/contacts/{referenceID}", Method.PUT);
				request.AddUrlSegment("referenceID", referenceID.ToString());
				formFields.ReferenceID = referenceID.ToString();
			}

			//Set Authorization Header, or QueryParam ApiToken
			request.AddHeader("Authorization", string.Format("Engage {0}", APIToken));

			request.RequestFormat = DataFormat.Json;
			request.AddBody(formFields);
			var result = client.Execute<Contact>(request);
			try {
				if (result.ErrorMessage.IsNotBlank()) {
					Logging.dlog("Engage debugger - Error: " + result.ErrorMessage);
				} else {
					return result.Data;
				}
			} catch (Exception e) {
				if (result == null) {
					Error.SendExceptionEmail(e, "Ubiquity Engage: Form submit returned no result.");
				} else {
					Error.SendExceptionEmail(e, "Ubiquity Engage " + result.Content);
				}
			}

			return null;
		}

		/// <summary>
		/// Create or Update a transaction (only course atm)
		/// </summary>
		/// <param name="referenceID"></param>
		/// <returns></returns>
		public EngageTransaction Send(Guid contactReferenceID, string transactionalDatabaseID, string transactionID) {

			//Build a new Base Rest Client 
			var client = new RestClient("https://api.ubiquity.co.nz");
			//create a request
			RestRequest request;

			// New transaction
			if (transactionID == null) {
				request = new RestRequest("database/contacts/{referenceID}/transactions/{transactionalDatabaseID}", Method.POST);
			} else {
				request = new RestRequest("database/contacts/{referenceID}/transactions/{transactionalDatabaseID}/{transactionID}", Method.PUT);
				request.AddUrlSegment("transactionID", transactionID);
			}

			request.AddUrlSegment("referenceID", contactReferenceID.ToString());
			request.AddUrlSegment("transactionalDatabaseID", transactionalDatabaseID);

			//Set Authorization Header, or QueryParam ApiToken
			request.AddHeader("Authorization", string.Format("Engage {0}", APIToken));
			//request.AddUrlSegment("apiToken", _apiToken);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(formFields);
			var json = client.Execute(request).Content;

			if (json.IsNotBlank() && json.ToLower().Contains("not found")) {
				Logging.dlog("EngageTransaction Send (Parameters): " + contactReferenceID + ", " + transactionalDatabaseID + ", " + transactionID);
				Logging.dlog("EngageTransaction Send (JSON): " + json);
				return null;
			}
			
			var result = JsonHelper.Parse(json);
			try {
				var transaction = new EngageTransaction();
				transaction.ReferenceID = new Guid(result.referenceID);
				transaction.TransactionID = result.transactionID.ToString();
				transaction.Source = result.source.ToString();
				return transaction;
			} catch { }

			return null;
		}

		public class EngageTransaction {
			public Guid? ReferenceID;
			public string TransactionID;
			public string Source;
		}

		public EngageValidation Validate() {
			//Build a new Base Rest Client 
			var client = new RestClient("https://api.ubiquity.co.nz");
			//create a request
			var request = new RestRequest("forms/{formID}/validate", Method.POST);
			//Set Authorization Header, or QueryParam ApiToken
			request.AddHeader("Authorization", string.Format("Engage {0}", APIToken));
			//request.AddUrlSegment("apiToken", _apiToken);
			request.AddUrlSegment("formID", FormID);
			request.RequestFormat = DataFormat.Json;
			request.AddBody(formFields);
			var result = client.Execute<PostValidationResult>(request);
			var validation = new EngageValidation();
			validation.IsValid = true;
			try {
				PostValidationResult resultData = result.Data;

				foreach (var fieldValidation in resultData.FieldValidation) {
					foreach (var error in fieldValidation.Error) {
						validation.IsValid = false;
						validation.Errors.Add(error);
					}
				}

			} catch (Exception e) {
				validation.IsValid = false;
				validation.Errors.Add(e.Message);
				if (result == null) {
					Error.SendExceptionEmail(e, "Ubiquity Engage: Form submit returned no result.");
				} else {
					Error.SendExceptionEmail(e, "Ubiquity Engage " + result.Content);
				}
			}

			return validation;
		}

		public class EngageValidation {
			public bool IsValid;
			public List<string> Errors = new List<string>();
		}

		public void AddField(string fieldID, string value) {
			AddField(fieldID, value, "");
		}

		public void AddField(string fieldID, string value, string name) {
			if (formFields == null) {
				formFields = new FormPostArgs();
				formFields.Data = new List<FieldValue>();
			}
			formFields.Data.Add(new FieldValue() { FieldID = fieldID, Value = value, Name = name });
		}

		public bool ReferenceIDIsValid(string refID) {
			const string apiBaseUrl = "https://api.ubiquity.co.nz";
			var url = string.Format("{0}/database/contacts/{1}?format=json&apiToken={2}", apiBaseUrl, refID, APIToken);

			try {
				Http.Get(url);
				return true;
			} catch {
				// Will throw an exception when the refID couldn't be found
			}

			return false;
		}

		public void Unsubscribe(string refID) {
			//get member ref
			string url = "/database/contacts/" + refID;

			var collection = new WebHeaderCollection();
			collection.Add("Authorization", string.Format("Engage {0}", APIToken));
			var obj = new {
				ReferenceID = refID,
				Data = new object[] {
					new {
						FieldID = GetFieldID("Opted Out"), 
						Name = "Opted Out", 
						Value = "true"
					},
					new {
						FieldID = GetFieldID("Email Permission"), 
						Name = "Email Permission", 
						Value = "false"
					}
				},
				ItemXml = (string)null,
				Source = (string)null,
				FormAction = (string)null
			};

			try {
				var response = Http.Request(Http.PUT, "https://api.ubiquity.co.nz" + url, new JsonSerializer().Serialize(obj), collection);
			} catch (Exception e) {
				var email = new ElectronicMail();
				email.Subject = "Engage Error - Unsubscribing member " + refID;
				string body = "";
				body += @"
	ReferenceID: " + refID + @"
	Action Taken: Unsubscribing" + @"
	Errors as follows: " + e.Message + @"
	Data:
	" + new JsonSerializer().Serialize(obj);
				email.BodyPlain = body;
				email.ToAddress = "andre@beweb.co.nz";
				email.FromAddress = SendEMail.EmailFromAddress;
				email.FromName = SendEMail.EmailFromName;
				email.Send(false);
			}

		}

		public dynamic GetEngageModifiedContacts(DateTime since, int limit) {
			var engageContacts = new List<dynamic>();

			//Build a new Base Rest Client 
			var collection = new WebHeaderCollection();
			collection.Add("Authorization", string.Format("Engage {0}", APIToken));
			const string url = "https://api.ubiquity.co.nz/database/contacts/query/";

			var numberOfRecords = Math.Min(limit, 150);
			string filterString = "{{\"filterString\": \"[" + GetFieldID("Last Modified") + "] ge '" + Fmt.DateTime(since, Fmt.DateTimePrecision.Second) + "'\", \"sortFields\": [{{ \"column\": \"" + GetFieldID("Last Modified") + "\", \"direction\": \"Ascending\"}}], \"numberOfRecords\": " + numberOfRecords + ", \"startRecord\": {0}}}";
			var obj = JsonHelper.Parse("{\"next\": 0}");

			do {
				var content = Http.Post(url, string.Format(filterString, obj.next), "Authorization", string.Format("Engage {0}", APIToken));
				obj = JsonHelper.Parse(content);

				if (obj.selectedContacts.Length == 0) {
					break;
				}

				foreach (var selectedContact in obj.selectedContacts) {
					engageContacts.Add(selectedContact);
					if (engageContacts.Count >= limit) break;
				}
			} while (obj.next < obj.totalContacts && engageContacts.Count < limit);

			return engageContacts;
		}

		public void UpdateLastLoginDate(Guid referenceID) {

			string url = "/database/contacts/" + referenceID;

			var collection = new WebHeaderCollection();
			collection.Add("Authorization", string.Format("Engage {0}", APIToken));

			var obj = new {
				ReferenceID = referenceID,
				Data = new object[] {
					new {
						FieldID = GetFieldID("Tradesmart Last Logged In Date"), 
						Name = "Tradesmart Last Logged In Date", 
						Value = DateTime.Now.FmtDate()
					}
				},
				ItemXml = (string)null,
				Source = (string)null,
				FormAction = (string)null
			};

			try {
				var result = Http.Request(Http.PUT, "https://api.ubiquity.co.nz" + url, new JsonSerializer().Serialize(obj), collection);
			} catch(Exception ex) {  }

		}

	}

}

