//#define SystemWebHelpersJson
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
#if SystemWebHelpersJson
using System.Web.Helpers;
#endif

namespace Beweb {
	/// <summary>
	/// Beweb utility helper class for converting things.
	/// </summary>
	public class Conv {

#if SystemWebHelpersJson
		public static dynamic JsonParse(string jsonString) {
			dynamic obj = Json.Decode(jsonString);
			return obj;
		}

		public static string JsonStringify(object obj) {
			return Json.Encode(obj);
		}
#endif

		/// <summary>
		/// Utility method to get default value for a given type. This will be either null, zero, false, or 1 Jan 2001 depeding on whether the type is nullable.
		/// </summary>
		public static object GetDefaultValue(Type type) {
			if (type.IsValueType) {
				return Activator.CreateInstance(type);
			}
			return null;
		}

		/// <summary>
		/// Convert from a string to the required type of value. 
		/// This is used when setting field values from strings (eg user input, http request or importing a file).
		/// Used in the core by ActiveField and Sqlize
		/// It is exposed as a public utility method so it can be used other places too.
		/// </summary>
		public static object FromString(string stringValue, Type correctType) {
			if (stringValue.IsBlank()) {
				// set to default which is null for nullable types or false for bool
				return GetDefaultValue(correctType);
			}
			stringValue = stringValue.Trim();

			// prep value for conversion
			TypeConverter conv = TypeDescriptor.GetConverter(correctType);
			object thisValue = stringValue;

			// do any prep work depending on type
			if (correctType.FullName.Contains("System.Boolean")) {  //this is for bool or nullable bool
				if (thisValue + "" == "1") thisValue = "True";
				if (thisValue + "" == "0") thisValue = "False";
				string lowerVal = (thisValue + "").ToLower();
				if (lowerVal == "yes") thisValue = "True";
				if (lowerVal == "no") thisValue = "False";
				if (lowerVal == "on") thisValue = "True";
				if (lowerVal == "off") thisValue = "False";
				if (lowerVal == "t") thisValue = "True";
				if (lowerVal == "f") thisValue = "False";
			} else if (correctType.FullName.Contains("System.DateTime")) {
				// todo - there must be some prep here for unusual date formats
			} else if (correctType.FullName.Contains("System.Decimal")) {
				thisValue = Fmt.CleanNumber(stringValue);
			} else if (correctType.FullName.Contains("System.Int")) {
				if (stringValue == "on") {   // "on" allows you to select null in a radio button group with int values (eg IDs)
					return GetDefaultValue(correctType);
				}
				thisValue = "" + Fmt.CleanInt(stringValue);
			}

			// now convert it
			if (conv.CanConvertFrom(typeof(string))) {
				try {
					return conv.ConvertFrom(thisValue);

				} catch (Exception e) {
					// MN changed from: catch (FormatException e) {
					string message = "FromString: '" + stringValue + "' is not a valid value of type " + correctType.Name + "; " + e.Message;

					if (!e.Message.Contains("Guid should contain 32 digit")) {
						if (thisValue + "" != "" && !ActiveFieldBase.IgnoreConversionErrors) {
							throw new ActiveRecordException(message, e);
						}
					}
				}
			} else {
				// TODO: Why do we throw an exception here instead of setting "ex"?
				throw new FormatException("No type converter available for type: " + correctType.Name);
			}
			return GetDefaultValue(correctType);
		}


		public static string ObjectToJSON(object obj) {
			var result = new StringBuilder();
			result.Append("{");

			Type objType = obj.GetType();
			string objName = objType.Name;

			PropertyInfo[] properties = objType.GetProperties();

			foreach (PropertyInfo property in properties) {
				if (result.Length > 1) result.Append(","); 
				string propertyName = property.Name;
				result.Append(propertyName.JsonStringify());
				result.Append(":");

				object value = null;
				try {
					value = property.GetValue(obj, null);
				} catch (Exception e) {

				}
				result.Append(value.JsonStringify());
			}
			result.Append("}");

			return result.ToString();
		}
	}
}
