using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
namespace Beweb {




	/// <summary>
	/// Summary description for Address
	/// </summary>
	public class Address {
		// address types
		public const string ADDRESS_DEFAULT = "default";
		public const string ADDRESS_NORMAL = "normal";

		public int AddressId { get; set; }
		public string Specified { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Altitude { get; set; }
		public int Zoom { get; set; }
		public string Proper { get; set; }
		public int Accuracy { get; set; }
		public string Thoroughfare { get; set; }
		public string Locality { get; set; }
		public string SubAdministrativeArea { get; set; }
		public string AdministrativeArea { get; set; }
		public string PostalCode { get; set; }
		public string CountryName { get; set; }
		public string CountryCode { get; set; }
		public string AddressType { get; set; }

		public Address() {
		}

		public Address(int addressId) {
			Hashtable ht = BewebData.GetValues("SELECT * FROM Address WHERE AddressId=@AddressId",
													 new Parameter("AddressId", TypeCode.Int32, addressId.ToString()));
			if (ht.Count > 0) {
				AddressId = Convert.ToInt32(ht["AddressId"]);
				Specified = ht["Specified"].ToString();
				Latitude = Convert.ToDouble(ht["Latitude"]);
				Longitude = Convert.ToDouble(ht["Longitude"]);
				Altitude = Convert.ToDouble(ht["Altitude"]);
				Zoom = Convert.ToInt32(ht["Zoom"]);
				Proper = ht["Proper"].ToString();
				Accuracy = Convert.ToInt32(ht["Accuracy"]);
				Thoroughfare = ht["Thoroughfare"].ToString();
				Locality = ht["Locality"].ToString();
				SubAdministrativeArea = ht["SubAdministrativeArea"].ToString();
				AdministrativeArea = ht["AdministrativeArea"].ToString();
				PostalCode = ht["PostalCode"].ToString();
				CountryName = ht["CountryName"].ToString();
				CountryCode = ht["CountryCode"].ToString();
				AddressType = ht["AddressType"].ToString();
			}
		}

		public void Insert() {
			// save the new details
			var pc = new ParameterCollection();
			pc.Add("Specified", TypeCode.String, Specified);
			pc.Add("Latitude", TypeCode.Decimal, Latitude.ToString());
			pc.Add("Longitude", TypeCode.Decimal, Longitude.ToString());
			pc.Add("Altitude", TypeCode.Decimal, Altitude.ToString());
			pc.Add("Zoom", TypeCode.Int32, Zoom.ToString());
			pc.Add("Proper", TypeCode.String, Proper);
			pc.Add("Accuracy", TypeCode.Int32, Accuracy.ToString());
			pc.Add("Thoroughfare", TypeCode.String, Thoroughfare);
			pc.Add("Locality", TypeCode.String, Locality);
			pc.Add("SubAdministrativeArea", TypeCode.String, SubAdministrativeArea);
			pc.Add("AdministrativeArea", TypeCode.String, AdministrativeArea);
			pc.Add("PostalCode", TypeCode.String, PostalCode);
			pc.Add("CountryName", TypeCode.String, CountryName);
			pc.Add("CountryCode", TypeCode.String, CountryCode);
			pc.Add("AddressType", TypeCode.String, AddressType);

			AddressId = BewebData.InsertRecord("INSERT INTO Address (Specified, Latitude, Longitude, Altitude, Zoom, Proper, Accuracy, Thoroughfare, Locality, SubAdministrativeArea, AdministrativeArea, PostalCode, CountryName, CountryCode, AddressType) " +
								"VALUES (@Specified, @Latitude, @Longitude, @Altitude, @Zoom, @Proper, @Accuracy, @Thoroughfare, @Locality, @SubAdministrativeArea, @AdministrativeArea, @PostalCode, @CountryName, @CountryCode, @AddressType)", pc);

		}

		public static void Update(string addressId, string latitude, string longitude, string altitude, string zoom) {
			if (String.IsNullOrEmpty(addressId)) {
				// can't update if we don't have an addressId
			} else {
				var pc = new ParameterCollection();
				pc.Add("Latitude", TypeCode.Decimal, latitude);
				pc.Add("Longitude", TypeCode.Decimal, longitude);
				pc.Add("Altitude", TypeCode.Decimal, altitude);
				pc.Add("Zoom", TypeCode.Int32, zoom);
				pc.Add("AddressId", TypeCode.Int32, addressId);

				BewebData.UpdateRecord("UPDATE Address SET Latitude=@Latitude, Longitude=@Longitude, Altitude=@Altitude, Zoom=@Zoom WHERE AddressId=@AddressId", pc);
			}
		}
	}
}