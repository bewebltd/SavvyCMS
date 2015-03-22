
#define pages
using System.Collections.Generic;
using Beweb;

namespace SavvyMVC.Helpers {

	public class BewebModelState {

		/// <summary>
		/// Adds a ValidationError to the Errors List with the provided error message
		/// </summary>
		public static void AddModelError(string errorMessage) {

			List<ValidationError> errors;

			if (Web.Session["errorList"] == null) {
				errors = new List<ValidationError>();
				Web.Session["errorList"] = errors;

			} else {
				errors = (List<ValidationError>)Web.Session["errorList"];
			}
			var error = new ValidationError(errorMessage);
			errors.Add(error);
		}

		/// <summary>
		/// Adds a ValidationError to the Errors List with the provided active record name and the error message
		/// </summary>
		public static void AddModelError(string record, string errorMessage) {

			List<ValidationError> errors;
			if (Web.Session["errorList"] == null) {
				errors = new List<ValidationError>();
				Web.Session["errorList"] = errors;
			} else {
				errors = (List<ValidationError>)Web.Session["errorList"];
			}
			var error = new ValidationError(record, errorMessage);
			errors.Add(error);
		}

		/// <summary>
		/// Adds a ValidationError to the Errors List
		/// </summary>
		public static void AddModelError(ValidationError error) {

			List<ValidationError> errors;
			if (Web.Session["errorList"] == null) {
				errors = new List<ValidationError>();
				Web.Session["errorList"] = errors;
				Web.Session["errorList"] = errors;
			} else {
				errors = (List<ValidationError>)Web.Session["errorList"];
			}
			errors.Add(error);
		}

		public static bool IsValid() {
			return Web.Session["errorList"] == null;
		}
	}







}
