#define ActiveRecord
#define PathAndFile
#define ModificationLog
//#define Debug
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using Beweb;
using System.Reflection;

namespace Beweb {

	public class ActiveRecord : IEquatable<ActiveRecord> {
		private string sourceSqlString;
		public string SourceSqlString { get { return sourceSqlString; } set { sourceSqlString = value; } }
		private string connectionString;
		public string SqlConnectionString { get { return connectionString; } set { connectionString = value; } }
		private string sourceLoadedFrom = "New";
		internal string SourceLoadedFrom { get { return sourceLoadedFrom; } set { sourceLoadedFrom = value; } }


		/// Checks IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible,ShowInNav if those fields exist.
		public static string PossibleFieldNamesForIsActive = "IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible";
		public static string PossibleFieldNamesForName = "Name,Title,Description,FirstName";
		public static string PossibleFieldNamesForSortPosition = "SortPosition,SortOrder,Position";
		public static string PossibleFieldNamesForDateAdded = "TransactionDate,DateAdded,CreateDate,DateCreated";
		public static string PossibleFieldNamesForDateModified = "DateModified,LastModified";
		public static string PossibleFieldNamesForPublishDate = "PublishDate";
		public static string PossibleFieldNamesForExpiryDate = "ExpiryDate";
		// also "PublishDate" and "ExpiryDate" are hard coded
		public static string PossibleFieldNamesForGeoAddress = "Location,Address,StreetAddress,AddressStreet,Address1,Address2,Address3,Address4,Street,Suburb,City,Region,District,Country";

		protected Dictionary<string, ActiveFieldBase> fields = new Dictionary<string, ActiveFieldBase>(StringComparer.InvariantCultureIgnoreCase);

		private bool isNewRecord = true;
		public bool IsNewRecord {
			get { return this.isNewRecord; }
			set { this.isNewRecord = value; }
		}

		private bool wasNewRecord = false;
		public bool WasNewRecord {
			get { return this.wasNewRecord; }
			set { this.wasNewRecord = value; }
		}

		public ActiveFieldBase ID_Field {
			get {

				var keyName = GetPrimaryKeyName();
				var priColField = new ActiveFieldBase();
				if (fields.Count > 0) {
					priColField = fields[keyName];
				} else {
					priColField.Name = keyName;								//20141120jn added to allow this: var ar = new ActiveRecord("newtable","newtable" + "ID");var sql = ar.GetSqlForCreate(); 	sql.Execute();
					priColField.IsAuto = true;
					priColField.Type = typeof(int);
				}
				return priColField;
			}
		}

		/// <summary>
		/// Returns a dictionary of field name-value pairs which can be output as JSON (by JavascriptSerializer or Json return type).
		/// Standard ActiveRecord cannot be output as JSON becasue it is not seralisable.
		/// </summary>
		public Dictionary<string, object> JsonObject {
			get {
				var data = new Dictionary<string, object>();
				foreach (var field in fields) {
					data[field.Key] = field.Value.ValueObject;
				}
				return data;
			}
		}

		// begin advanced
		private ActiveRecordAdvancedFunctions _advanced = null;
		public ActiveRecordAdvancedFunctions Advanced {
			get {
				if (_advanced == null) {
					_advanced = new ActiveRecordAdvancedFunctions(this);
				}
				return _advanced;
			}
		}

		public class ActiveRecordAdvancedFunctions {
			private readonly ActiveRecord record;
			private Dictionary<string, ActiveFieldBase> fields { get { return record.fields; } }

			public ActiveRecordAdvancedFunctions(ActiveRecord activeRecord) {
				record = activeRecord;
			}

			public ImportReport GeocodeAddress() {
				var report = new ImportReport();
				var loc = record;
				if (loc.FieldExists("GeocodedAddress") && loc.FieldExists("Latitude") && loc.FieldExists("Longitude")) {
					string address = record.GetGeoAddress();
					if (loc["GeocodedAddress"].ToString() != address || loc["Latitude"].ValueObject == null || loc["GeocodingResult"].ToString() == "OVER_QUERY_LIMIT") {
						string status = null;
						string url = "http://maps.google.com/maps/api/geocode/xml?address=" + address.UrlEncode() + "&sensor=false";
						string xml = Http.Get(url);
						// <lat>-36.8002310</lat>
						// <lng>174.7769570</lng>
						if (xml.Contains("<lat>")) {
							if (xml.Contains("<location_type>APPROXIMATE</location_type>")) {
								report.AddWarningLine("Mapping Approximate", loc.GetName(), address + " was located automatically on the map, but needs mannual checking as location is approximate only. Please set the location manually in the CMS.", loc.GetAdminFullUrl());
								loc["GeocodingResult"].ValueObject = "Approx";
							} else {
								report.AddSuccessLine("Mapped", loc.GetName(), address + " has been automatically located on the map.", loc.GetAdminFullUrl());
								loc["GeocodingResult"].ValueObject = "Found";
							}
							//if (displayResults) Web.Write("<br>OK: " + shop.StoreName + " add:" + shop.Address);
							string lat = xml.ExtractTextBetween("<lat>", "</lat>").Trim();
							string lng = xml.ExtractTextBetween("<lng>", "</lng>").Trim();
							if (lat.IsNotBlank() && lng.IsNotBlank()) {
								loc["Latitude"].ValueObject = double.Parse(lat);
								loc["Longitude"].ValueObject = double.Parse(lng);
							}
						} else if (xml.Contains("<status>")) {
							status = xml.ExtractTextBetween("<status>", "</status>").Trim();
							report.AddFailedLine("Store Needs Mapping", loc.GetName(), address + " could not be located automatically on the map. Please set the location manually in the CMS. (Further information code: " + status + ")", loc.GetAdminFullUrl());
							loc["GeocodingResult"].ValueObject = status;
						}
						if (status != "OVER_QUERY_LIMIT") {
							loc["GeocodedAddress"].ValueObject = address;
						}
						loc.Save();
					}
				}
				return report;
			}

			public bool HasPublishDates {
				get { return PublishDateField != null; }
			}

			public bool HasExpiryDates {
				get { return ExpiryDateField != null; }
			}

			private ActiveFieldBase _publishDateField = null;

			public ActiveFieldBase PublishDateField {
				get {
					if (_publishDateField == null) {
						foreach (var name in PossibleFieldNamesForPublishDate.Split(',')) {
							if (fields.ContainsKey(name) && fields[name].IsDateField) {
								_publishDateField = fields[name];
								break;
							}
						}
					}
					return _publishDateField;
				}
			}

			private ActiveFieldBase _expiryDateField = null;

			public ActiveFieldBase ExpiryDateField {
				get {
					if (_expiryDateField == null) {
						foreach (var name in PossibleFieldNamesForExpiryDate.Split(',')) {
							if (fields.ContainsKey(name) && fields[name].IsDateField) {
								_expiryDateField = fields[name];
								break;
							}
						}
					}
					return _expiryDateField;
				}
			}

			private ActiveFieldBase _sortPositionField = null;

			public ActiveFieldBase SortPositionField {
				get {
					if (_sortPositionField == null) {
						foreach (string name in PossibleFieldNamesForSortPosition.Split(',')) {
							if (fields.ContainsKey(name) && fields[name].IsNumericField) {
								_sortPositionField = fields[name];
								break;
							}
						}
					}

					return _sortPositionField;
				}
			}

			private List<ActiveFieldBase> _isActiveFields = null;

			/// <summary>
			/// Return all a list of the fields like IsActive, IsPublished, Active, Enabled that are in this record.
			/// If any of these are false the record is considered inactive.
			/// </summary>
			public List<ActiveFieldBase> IsActiveFields {
				get {
					if (_isActiveFields == null) {
						var isActiveFields = new List<ActiveFieldBase>();
						foreach (string possibleFieldName in PossibleFieldNamesForIsActive.Split(',')) {
							if (fields.ContainsKey(possibleFieldName) && (fields[possibleFieldName].Type == typeof(bool) || fields[possibleFieldName].Type == typeof(bool?))) {
								isActiveFields.Add(fields[possibleFieldName]);
							}
						}
						if (_isActiveFields == null) {
							_isActiveFields = isActiveFields; // This list was getting changed while it was being enumerated 
						}
					}
					return _isActiveFields;
				}
			}


			public DateTime? PublishDate {
				get {
					if (PublishDateField != null) {
						return (DateTime?)PublishDateField.ValueObject;
					}
					return null;
				}
			}

			public DateTime? ExpiryDate {
				get {
					if (ExpiryDateField != null) {
						return (DateTime?)ExpiryDateField.ValueObject;
					}
					return null;
				}
			}

			private ActiveFieldBase _dateAddedField = null;

			public ActiveFieldBase DateAddedField {
				get {
					if (_dateAddedField == null) {
						foreach (var name in PossibleFieldNamesForDateAdded.Split(',')) {
							if (fields.ContainsKey(name) && fields[name].IsDateField) {
								_dateAddedField = fields[name];
								break;
							}
						}
					}
					return _dateAddedField;
				}
			}

			public DateTime? DateAdded {
				get {
					if (DateAddedField != null) {
						return (DateTime?)DateAddedField.ValueObject;
					}
					return null;
				}
			}

			private ActiveFieldBase _dateModifiedField = null;

			public ActiveFieldBase DateModifiedField {
				get {
					if (_dateModifiedField == null) {
						foreach (var name in PossibleFieldNamesForDateModified.Split(',')) {
							if (fields.ContainsKey(name) && fields[name].IsDateField) {
								_dateModifiedField = fields[name];
								break;
							}
						}
					}
					return _dateModifiedField;
				}
			}

			public DateTime? DateModified {
				get {
					if (DateModifiedField != null) {
						return (DateTime?)DateModifiedField.ValueObject;
					}
					return null;
				}
			}

			/// <summary>
			/// Set this to true for any models which have expiry dates including the time.
			/// Put code to set it in InitDefaults in the partial model file.
			/// </summary>
			public bool ExpiryDatesHaveTimes { get; set; }

			public string MailMerge(string bodyText) {
				string emailText = bodyText + "";
				string emailTextLower = emailText.ToLower();

				foreach (var field in record.GetFields()) {
					if (emailTextLower.Contains("[" + field.Name.ToLower() + "]")) {
						emailText = emailText.ReplaceInsensitive("[" + field.Name.ToLower() + "]", field.ToString());
					}
				}
				return emailText;
			}

			public void SetTableName(string newTableName) {
				record.SetTableName(newTableName);
			}

		}// end advanced



		//		public static bool TruncateOverLengthData {	get;set;} //default to false;// MN 20130220 - no longer used

		//protected bool hasAutoPK = true;

		protected string tableName = "";
		private string primaryKeyName = "";
		public virtual string GetTableName() { return this.tableName; }
		protected virtual void SetTableName(string newTableName) {
			this.tableName = newTableName;
			foreach (var field in fields.Values) {
				field.TableName = newTableName;
			}
		}
		public virtual string GetFriendlyTableName() { return this.tableName.SplitTitleCase().RightFrom(".dbo."); }
		public virtual string GetPrimaryKeyName() { return this.primaryKeyName; }
		public virtual bool GetIsView() { return false; }

		/// <summary>
		/// Returns most likely useful sort order. You can override.
		/// This returns a string starting with " ORDER BY ".
		/// </summary>
		/// <returns></returns>
		public virtual string GetDefaultOrderBy() {
			if (tableName.IsBlank()) return "";  // don't bother trying to generate an order by clause before we even know the tablename - just load it

			DelimitedString orderBy = new DelimitedString();
			// sort position
			foreach (string possibleFieldName in PossibleFieldNamesForSortPosition.Split(',')) {
				if (fields.ContainsKey(possibleFieldName)) {
					if (fields[possibleFieldName].IsNumericField) {
						orderBy += possibleFieldName;
					}
				}
			}
			// then name
			var nameField = GetNameField();
			if (nameField != null) {
				orderBy += nameField.Name;
			}
			// then date added (newest first)
			foreach (string possibleFieldName in PossibleFieldNamesForDateAdded.Split(',')) {
				if (fields.ContainsKey(possibleFieldName) && fields[possibleFieldName].Type == typeof(DateTime)) {
					orderBy += possibleFieldName + " DESC";
				}
			}
			// then record id (newest first)
			if (orderBy.IsBlank || !orderBy.ToString().ContainsCommaSeparated(ID_Field.Name)) {
				orderBy += ID_Field.Name + " DESC";
			}
			return " ORDER BY " + orderBy.ToString();
		}

		/// <summary>
		/// Returns name/title of the item based on a guess at likely field names or finds first character field.
		/// </summary>
		/// <returns></returns>
		public virtual ActiveField<string> GetNameField() {
			CheckFieldsCollectionIsPopulated();
			foreach (string possibleFieldName in PossibleFieldNamesForName.Split(',')) {
				if (fields.ContainsKey(possibleFieldName) && fields[possibleFieldName].Type == typeof(string) && fields[possibleFieldName].ColumnType != "ntext" && fields[possibleFieldName].ColumnType != "text") {
					// sql server can't order by a ntext field 
					return (ActiveField<string>)fields[possibleFieldName];
				}
			}
			// find first string field
			foreach (var activeField in this.fields.Values) {
				if (activeField.Type == typeof(string) && activeField.ColumnType != "ntext") {
					return (ActiveField<string>)activeField;
				}
			}
			// can't find any name field
			return null;
		}

		/// <summary>
		/// Returns name/title of the item based on a guess at likely field names or finds first character field.
		/// </summary>
		/// <returns></returns>
		public virtual string GetName() {
			var field = GetNameField();
			if (field == null) {
				return "";
			}
			return field.Value;
		}

#if	PathAndFile
		/// <summary>
		/// Returns a standard URL that could be used for viewing this item in the front end of the site.
		/// Format is: {TableName}/{ID}/{CrunchedTitle}.
		/// Example: Destination/123/Cook-Islands
		/// Note: does not append ".aspx" since this is not needed on IIS7. If you are running on IIS6, just add ".aspx" to this.
		/// You can override this in the Model partial class (in Models folder) to produce the correct URL if you have different URL naming.
		/// </summary>
		/// <returns></returns>
		public virtual string GetUrl() {
			return GetUrl(GetTableName());
		}

		public virtual string GetFullUrl() {
			return Web.ResolveUrlFull(GetUrl());
		}

		public virtual string GetFullUrl(string controllerName) {
			return Web.ResolveUrlFull(GetUrl(controllerName));
		}

		/// <summary>
		/// Returns a standard URL that could be used for viewing this item in the front end of the site.
		/// Format is: {TableName}/{ID}/{CrunchedTitle}.
		/// Example: Destination/123/Cook-Islands
		/// Note: does not append ".aspx" since this is not needed on IIS7. If you are running on IIS6, just add ".aspx" to this.
		/// You can override this in the Model partial class (in Models folder) to produce the correct URL if you have different URL naming.
		/// </summary>
		/// <returns></returns>
		public virtual string GetUrl(string controllerName) {
			if (controllerName.StartsWith("?")) {
				// parameter is actually the querystring, so should be appended after
				string queryString = controllerName;
				string url = GetUrl();
				if (url.Contains("?")) {
					queryString = queryString.Replace("?", "&");
				}
				if (url.Contains("#")) {
					queryString += "#";
					url = url.Replace("#", queryString);
				} else {
					url += queryString;
				}
				return url;
			}
			return Web.ResolveUrl("~/" + controllerName + "/" + ID_Field.ValueObject + "/" + PathAndFile.CrunchFileName(GetName()));
		}


		public virtual string GetAdminUrl() {
			return Web.ResolveUrl(Web.AdminRoot + tableName + "Admin/Edit/" + ID_Field.ValueObject);
		}

		public virtual string GetAdminFullUrl() {
			return Web.ResolveUrlFull(GetAdminUrl());
		}


#endif
		public string DumpFields() {
			string result = "";
			result += "Fields: ";
			foreach (var dataitem in this.fields) {
				result += "" + dataitem.Key + "\t";
			}
			result += "\n";
			result += "Data: ";

			foreach (var dataitem in this.fields) {
				result += "" + dataitem.Value + "\t";
			}
			result += "\n";
			return result;
		}
		public ActiveFieldBase GetFieldByIndex(int index) {
			CheckFieldsCollectionIsPopulated();
			int i = 0;
			foreach (var activeField in this.fields.Values) {
				if (i == index) {
					return activeField;
				}
				i++;
			}
			throw new ActiveRecordException("ActiveRecord GetFieldByIndex(): Index out of range. You supplied [" + index + "]. Max is [" + i + "].\nFields that are present are: " + GetFieldNames().Join(", "));
		}

		public ActiveFieldBase GetFieldByName(string fieldName) {
			CheckFieldsCollectionIsPopulated();
			try {
				return fields[fieldName];
			} catch (KeyNotFoundException ex) {
				throw new ActiveRecordException("GetFieldByName: Field is not in the collection. Check the field name for typos and make sure it is either in the table or in the SQL statement. The missing field is [" + fieldName + "].\nFields that are present are: " + GetFieldNames().Join(", "), ex);
			}
		}

		public bool FieldExists(string fieldName) {
			CheckFieldsCollectionIsPopulated();
			return fields.ContainsKey(fieldName);
		}

		public List<string> GetFieldNames() {
			List<string> fieldNames = new List<string>();
			foreach (var entry in fields) {
				fieldNames.Add(entry.Key);
			}
			return fieldNames;
		}

		public List<string> GetTextFieldNames() {
			List<string> fieldNames = new List<string>();
			foreach (var entry in fields) {
				if (entry.Value.IsTextField) {
					fieldNames.Add(entry.Key);
				}
			}
			return fieldNames;
		}

		public virtual List<string> GetGeoAddressFieldNames() {
			List<string> fieldNames = new List<string>();
			foreach (var split in PossibleFieldNamesForGeoAddress.Split(",")) {
				var field = split.Trim();
				if (FieldExists(field)) {
					fieldNames.Add(field);
				}
				if (FieldExists(field + "ID")) {
					fieldNames.Add(field + "ID");
				}

			}
			//foreach (var entry in fields) {
			//	var fieldName = entry.Value.Name.ToLower();
			//	if (!fieldName.Contains("email") &&  !fieldName.Contains("geocodedaddress") && (fieldName.Contains("address") || fieldName.Contains("suburb") || fieldName.Contains("town") || fieldName.Contains("city") || fieldName.Contains("region") || fieldName.Contains("district") || fieldName.Contains("country"))) {
			//		fieldNames.Add(entry.Key);
			//	}
			//}
			return fieldNames;
		}

		public virtual string GetGeoAddress() {
			return GetGeoAddress(null);
		}

		public virtual string GetGeoAddress(string delimiter) {
			if (delimiter == null) {
				delimiter = ", ";
			}
			string address = null;
			foreach (string fieldName in GetGeoAddressFieldNames()) {
				var addressPart = fields[fieldName].ToStringNice();
				if (addressPart.IsNotBlank()) {
					if (address.IsNotBlank()) address += delimiter;
					address += addressPart;
				}
			}
			return address;
		}

		/// <summary>
		/// get field by name indexer
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public ActiveFieldBase this[string fieldName] {
			get { return this.GetFieldByName(fieldName); }
			//set{SetValue(fieldName,value);}
		}

		/*
		//dont uncomment this please!
		private void SetValue(string colName, ActiveFieldBase value)
		{
			fields[colName].ValueObject=value.ValueObject;
		}*/

		/// <summary>
		/// get field by position indexer
		/// </summary>
		/// <param name="columnIndex"></param>
		/// <returns></returns>
		public ActiveFieldBase this[int columnIndex] {
			get { return this.GetFieldByIndex(columnIndex); }
			//set{SetValue(colName,value);}
		}

		/// <summary>
		/// Returns true by default or false if not active. 
		/// Looks for commonly used field names that represent Active/Enabled/Visible state of this item. 
		/// Checks PublishDate and ExpiryDate if those fields exist.
		/// Checks IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible,ShowInNav if those fields exist.
		/// You can override this in the model partial (this will then be used by myRecordList.Active, admin list pages and other routines). If overriding MyModel.GetIsActive() you should also override MyModel.GetSqlWhereActive().
		/// </summary>
		public virtual bool GetIsActive() {
			return GetIsActive(DateTime.Now);
		}

		/// <summary>
		/// Returns true by default or false if not active. 
		/// Looks for commonly used field names that represent Active/Enabled/Visible state of this item. 
		/// Checks PublishDate and ExpiryDate if those fields exist.
		/// Checks IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible,ShowInNav if those fields exist.
		/// You can override this in the model partial (this will then be used by myRecordList.Active, admin list pages and other routines). If overriding MyModel.GetIsActive() you should also override MyModel.GetSqlWhereActive().
		/// </summary>
		public virtual bool GetIsActive(DateTime now) {
			bool isActive = true;		//optimistic

			foreach (var field in Advanced.IsActiveFields) {
				if (!field.ToBool()) {
					isActive = false;
				}
			}

			if (Advanced.HasPublishDates) {
				if (Advanced.PublishDate == null) {
					isActive = false;
				} else {
					DateTime publishDate = Advanced.PublishDate.Value;
					if (publishDate.Date == publishDate && !Advanced.ExpiryDatesHaveTimes) {
						// use whole day logic
						if (now.Date < publishDate) isActive = false;
					} else {
						// check time as well
						if (now < publishDate) isActive = false;
					}
				}
			}


			if (Advanced.HasExpiryDates) {
				if (Advanced.ExpiryDate != null) {
					DateTime expiryDate = Advanced.ExpiryDate.Value;
					if (expiryDate.Date == expiryDate && !Advanced.ExpiryDatesHaveTimes) {
						// use whole day logic
						if (now.Date > expiryDate) isActive = false;
					} else {
						// check time as well
						if (now > expiryDate) isActive = false;
					}
				}
			}


			//foreach (string possibleFieldName in PossibleFieldNamesForIsActive.Split(',')) {
			//	if (fields.ContainsKey(possibleFieldName) && (fields[possibleFieldName].Type == typeof(bool) || fields[possibleFieldName].Type == typeof(bool?))) {
			//		if (!fields[possibleFieldName].ToBool()) {
			//			isActive = false;
			//		}
			//	}
			//}

			//foreach (var name in PossibleFieldNamesForPublishDate.Split(',')) {
			//	if (fields.ContainsKey(name) && fields[name].IsDateField) {
			//		if (fields[name].IsBlank) {
			//			isActive = false;
			//		} else {
			//			DateTime publishDate = (DateTime)fields[name].ValueObject;
			//			if (publishDate.Date == publishDate && !Advanced.ExpiryDatesHaveTimes) {
			//				// use whole day logic
			//				if (now.Date < publishDate) isActive = false;
			//			} else {
			//				// check time as well
			//				if (now < publishDate) isActive = false;
			//			}
			//		}
			//	}
			//}
			//foreach (var name in PossibleFieldNamesForExpiryDate.Split(',')) {
			//	if (fields.ContainsKey(name) && fields[name].IsDateField) {
			//		if (!fields[name].IsBlank) {
			//			DateTime expiryDate = (DateTime)fields[name].ValueObject;
			//			if (expiryDate.Date == expiryDate && !Advanced.ExpiryDatesHaveTimes) {
			//				// use whole day logic
			//				if (now.Date > expiryDate) isActive = false;
			//			} else {
			//				// check time as well
			//				if (now > expiryDate) isActive = false;
			//			}
			//		}
			//	}
			//}
			return isActive;
		}

		// short term cache
		protected void StoreInCache() {
			//Web.Cache.Insert("ActiveRecord-Job-" + this.ID, this);
			ActiveRecordLoader.StoreInCache(this);
		}

		public void RemoveFromCache() {
			ActiveRecordLoader.RemoveFromCache(this);
		}


		/// <summary>
		/// Constructors
		/// </summary>
		public ActiveRecord() {
		}
		public ActiveRecord(string tableName, string primaryKeyName) {
			this.tableName = tableName;
			this.primaryKeyName = primaryKeyName;
		}

		public virtual void InitDefaults() {
			// allow user code to override or not
		}

		// back-reference to the list that this object is a member of
		// (actually could be more than once, so this is just the most recently accessed from)
		[NonSerialized]
		protected object containingList;
		internal void SetContainingList(object newContainingList) {
			containingList = newContainingList;
		}

		//public void SetPropertyValue(string propertyName, object value) {
		//	fields[propertyName].Value = value;
		//}

		//public object GetPropertyValue(string propertyName) {
		//	return fields[propertyName].Value;
		//}

		/// <summary>
		/// Load the data for the given SQL and return true of has data or false if no data found
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>false if no data</returns>
		//public bool LoadData(Sql sql) {
		//  return LoadData(sql, null);
		//}
		public bool LoadData(Sql sql, string connectionString) {
			if (sql.Value.Trim().StartsWith("where ")) {
				sql.Prepend("SELECT * FROM [" + GetTableName() + "]");
			}
			var reader = sql.GetReader(connectionString, (connectionString != null), (connectionString == null));
			bool hasData;
			try {
				hasData = reader.HasRows;
				if (reader.Read()) {//read one
					LoadData(reader);
				}
			} finally {
				reader.Close();
			}
			//			OnAfterLoadData(); -- MN 20111122 moved this into LoadData(reader)
			return hasData;
		}

		/// <summary>
		/// You can override this to do something every time a record is loaded, set some values for example.
		/// </summary>
		protected virtual void OnAfterLoadData() {

		}

		/// <summary>
		/// Load the data for the given SQL and return true of has data or false if no data found
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>false if no data</returns>
		public bool LoadData(Sql sql) {
			return LoadData(sql, Otherwise.Null);
		}

		/// <summary>
		/// Load the data for the given SQL and return true of has data or false if no data found
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="otherwise"></param>
		/// <returns>false if no data</returns>
		public bool LoadData(Sql sql, Otherwise otherwise) {
			if (sql.Value.Trim().ToLower().StartsWith("where ")) {
				if (GetTableName().IsBlank()) {
					throw new ActiveRecordException("LoadData: Cannot load data into this record as the TableName is unknown.\nSql is: " + sql.ToString());
				}
				sql.Prepend("SELECT * FROM ", GetTableName().SqlizeName());
			}
			//check the property of the active record sql conn string, 
			if (SqlConnectionString != null && sql.SqlConnectionString == null) sql.SqlConnectionString = SqlConnectionString;

			bool hasData;
			using (var reader = sql.GetReader()) {
				hasData = reader.HasRows;
				if (reader.Read()) {//read one
					LoadData(reader);
				}
				reader.Close();
				if (!hasData) {
					otherwise.Execute(this);
				}
				reader.Dispose();
			}
			return hasData;
		}

		/// <summary>
		/// Loads the record given the supplied ID. The supplied ID will be converted to the correct type (if possible) before loading.
		/// If the record is found in the database, true is returned an the ActiveRecord object is populated with field values from the database.
		/// If the record is not found in the database, false is returned and the ActiveRecord object remains unchanged.
		/// </summary>
		/// <param name="primaryKeyValue"></param>
		/// <returns></returns>
		public bool LoadDataByID(object primaryKeyValue) {
			// 20130607 - breaking change - this was a security flaw
			// todo - possibly try a convert here? or in sqlize?
			var sql = new Sql("where", GetPrimaryKeyName().SqlizeName(), "=", Sql.Sqlize(primaryKeyValue, ID_Field.Type));
			return LoadData(sql);
		}

		/// <summary>
		/// Could be made public if useful. Just 'internal' to avoid clutter.
		/// </summary>
		internal bool LoadDataByField(string fieldName, object searchValue) {
			Type type = typeof(string);   // string sqlizing will work for most data types anyway
			if (tableName.IsNotBlank() && FieldExists(fieldName)) {
				type = fields[fieldName].Type;   // field will not exist if loading a generic ActiveRecord
			} else if (searchValue.ToString().ContainsOnly("0123456789.-")) {
				type = typeof(decimal);
			}
			var sql = new Sql("where", fieldName.SqlizeName(), "=", Sql.Sqlize(searchValue, type));
			return LoadData(sql);
		}

		public Hashtable GetHashtable() {
			Hashtable returnValue = new Hashtable();
			foreach (var field in fields.Values) {
				// sql joins can return two fields of the same name
				if (!returnValue.ContainsKey(field.Name)) {
					returnValue.Add(field.Name, field.ValueObject);
				}
			}
			return returnValue;
		}

		/// <summary>
		/// initialise the fields array by loading the structure of the current table so you can say : data["blah"].valueobject="asd"; data.save();
		/// </summary>
		public void AddNew() {
			ReadSchemaInfo();
		}

		/// <summary>
		/// Initialise the fields array by loading the structure of the current table so you can say : data["blah"].valueobject="asd"; data.save();
		/// Creates and adds columns for any fields that don't already exist in the fields collection.
		/// </summary>
		public void ReadSchemaInfo() {
			ReadSchemaInfo(null);
		}

		public void ReadSchemaInfo(DbDataReader reader) {
			ActiveRecordGenerator.Table table;
			if (reader == null) {
				if (tableName.IsBlank()) throw new ActiveRecordException("AddNew/ReadSchemaInfo - You need to supply the tableName in the ActiveRecord constructor.");
				table = new ActiveRecordGenerator.Table(tableName, false, SqlConnectionString);
			} else {
				table = new ActiveRecordGenerator.Table(tableName, false, reader, SqlConnectionString);
			}
			foreach (var column in table.Columns) {
				if (!fields.ContainsKey(column.fieldName)) {
					//var field = new ActiveFieldBase() { Name = column.fieldName, ColumnType = column.dbType, Type = column.type, IsAuto = column.isPrimaryKey, MaxLength = column.maxLength, TableName = this.tableName };
					Type openType = typeof(ActiveField<>);
					Type closedType = openType.MakeGenericType(column.type);
					Object typedField = Activator.CreateInstance(closedType);
					ActiveFieldBase field = typedField as ActiveFieldBase;
					field.Name = column.fieldName;
					field.ColumnType = column.dbType;
					field.Type = column.type;
					field.IsAuto = column.isAuto;
					field.MaxLength = column.maxLength;
					field.TableName = column.tableName;
					//var field = new ActiveField<column.type>() { Name = column.fieldName, ColumnType = column.dbType, Type = column.type, IsAuto = column.isPrimaryKey, MaxLength = column.maxLength, TableName = this.tableName };
					fields.Add(column.fieldName, field);
				}
			}

			// check pkname
			if (primaryKeyName.IsBlank()) {
				primaryKeyName = table.PrimaryKey.fieldName;
			}

			//var sql = new Sql("SELECT * FROM ["+tableName+"] where 1=0");

			//var reader = sql.GetReader();
			//bool hasData = reader.HasRows;
			//reader.Read();
			////read structure
			//for (int i = 0; i < reader.FieldCount; i++) {	// for each column
			//  string fieldName = reader.GetName(i);
			//  ActiveFieldBase field;
			//  if (fields.ContainsKey(fieldName)) {
			//    field = fields[fieldName];
			//  } else {
			//    Type cSharpType = reader.GetFieldType(i);
			//    string sqlType = reader.GetDataTypeName(i);
			//    field = new ActiveFieldBase() { Name = fieldName, ColumnType = sqlType, Type = cSharpType, IsAuto = (this.tableName+"ID"==fieldName), MaxLength = 50, TableName = this.tableName };

			//    fields.Add(fieldName, field);
			//  }
			//}
			//reader.Close();
		}

		public bool LoadData(DbDataReader reader) {
			bool wasFound = false;
			if (reader != null) {
				// loop through the Active Field objects in the Active Record object, and get each from db
				//foreach (var field in fields.Values) {
				//	object newValue;
				//	try {
				//	newValue = reader[field.Name];
				//	} catch (IndexOutOfRangeException) {
				//	Exception e = new Exception("Savvy Active Record Error: field is not in the SQL resultset. This could be because:\n1. You are not selecting all columns (you need to select *)\n2. You need to Regenerate Models or \n3. The field has been dropped or not yet added to the database.\nField is [" + GetTableName() + "].[" + field.Name + "]./n" + Logging.DumpFieldsToString(reader));
				//	throw e;
				//	}

				//	field.ValueObject = newValue;
				//	field.OriginalValueObject = newValue;
				//}

				wasFound = true;

				// avoid name clashes by keeping a list of fields done
				List<string> fieldNamesFound = new List<string>();

				for (int i = 0; i < reader.VisibleFieldCount; i++) {	// for each column
					string fieldName = reader.GetName(i);
					if (fieldNamesFound.DoesntContain(fieldName)) {
						fieldNamesFound.Add(fieldName);
						ActiveFieldBase field;
						if (!fields.ContainsKey(fieldName)) {
							SomeFieldsNotInCollectionSoCheckSchema(reader);
						}
						if (fields.ContainsKey(fieldName)) {
							field = fields[fieldName];
						} else {
							// should never happen unless schema check fails for some reason
							throw new ActiveRecordException("should never happen unless schema check fails for some reason - if it ever does, just remove this throw and uncomment this block below");
							/*
							Type cSharpType = reader.GetFieldType(i);
							string sqlType = reader.GetDataTypeName(i);
							field = new ActiveFieldBase() {Name = fieldName, ColumnType = sqlType, Type = cSharpType, IsAuto = (this.tableName + "ID" == fieldName), MaxLength = 50, TableName = this.tableName};

							fields.Add(fieldName, field);
							*/
						}

						field.FromDatabase(reader.GetValue(i));

					} else {
						// field name already exists
						var field = fields[fieldName];
						if (field.ValueObject + "" == reader.GetValue(i) + "") {
							//possibly we will ignore this (e.g. join statement, returns both fks
						} else {
							throw new ActiveRecordException("This SQL resultset contains two fields with the same name but different values. Please modifiy the SQL. You need use AS to alias one of the fields (eg select title, title as booktitle from page inner join book...).\n\nThe field name is: " + fieldName + "\n\nFirst value [" + field.ValueObject + "] Second value [" + reader.GetValue(i) + "]\n\nFields already present are: " + (fieldNamesFound.Join(", ")) + "\n\nLast SQL command: " + Web.PageGlobals["SqlTraceLastQuery"]);
						}
					}
				}
				fieldNamesFound.Clear();

				SourceLoadedFrom = "Database";
			}
			IsNewRecord = false;

			OnAfterLoadData();

			return wasFound;
		}

		private void SomeFieldsNotInCollectionSoCheckSchema(DbDataReader reader) {
			ReadSchemaInfo(reader);
		}


		/// <summary>
		/// dump the contents of the object to a string for debugging
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return JsonStringify();
		}
		public string DumpToString() {
			string result = "";
			if (fields != null) {
				// loop through the Active Field objects in the Active Record object, and get each from db
				foreach (var field in fields.Values) {
					result += "name[" + field.Name + "]";
					if (field.IsNull) {
						result += "value[NULL]\n";
					} else {
						result += "value[" + field.ToString() + "]\n";
					}
					//result+="sqlized["+field.Sqlize()+"]\n";

				}
			} else {
				result = "Fields null";
			}
			return result;
		}

		/// <summary>
		/// dump the contents of the object to a string for debugging
		/// </summary>
		/// <returns></returns>
		public string JsonStringify() {
			return JsonObject.JsonStringify();
			//var result = new StringBuilder();
			//if (fields != null) {
			//	// loop through the Active Field objects in the Active Record object, and get each from db
			//	foreach (var field in fields.Values) {
			//		if (result.Length > 0) {
			//			result.Append(",");
			//		}
			//		result.Append("\"" + field.Name + "\"");
			//		if (field.IsNull) {
			//			result.Append(":null");
			//		} else {
			//			result.Append(":");
			//			result.Append(field.JsonStringify());
			//		}
			//		//result+="sqlized["+field.Sqlize()+"]\n";
			//	}
			//} else {
			//	result.Append("null");
			//}
			//return result.ToString();
		}


		/// <summary>
		/// dump the contents of the object to a string for debugging
		/// </summary>
		/// <returns></returns>
		public string ToCSharpTestData() {
			var result = new StringBuilder();
			if (fields != null) {
				// loop through the Active Field objects in the Active Record object, and get each from db
				result.Append("{");
				foreach (var field in fields.Values) {
					if (field.IsNull) {
						//skip
						//result.Append("=null");
					} else {
						if (result.Length > 1) {
							result.Append(",");
						}
						result.Append(field.Name);
						result.Append("=");
						if (field.IsDateField) {
							result.Append("DateTime.Parse(\"" + field.ToString() + "\")");
						} else if (field.IsTextField) {
							result.Append(field.ToString().JsEnquote());
						} else if (field.IsBooleanField) {
							result.Append(field.ToBool().ToString().ToLower());
						} else {
							result.Append(field.ToString());
						}
					}
				}
				result.Append("}");
			} else {
				result.Append("null");
			}
			return result.ToString();
		}

		/// <summary>
		/// dump the contents of the object to an html TR for debugging
		/// </summary>
		/// <returns></returns>
		public string ToHtmlTableRow(bool includeHeader) {
			string result = "";
			if (fields != null) {
				// loop through the Active Field objects in the Active Record object, and get each from db

				if (includeHeader) {
					result += "<tr>";
					foreach (var field in fields.Values) {
						result += "<td>" + field.Name + "</td>";
					}
					result += "</tr>";
				}
				result += "<tr>";
				foreach (var field in fields.Values) {
					if (field.IsNull) {
						result += "<td>NULL</td>";
					} else {
						result += "<td>" + field.ToString() + "</td>";
					}
					//result+="sqlized["+field.Sqlize()+"]\n";

				}
				result += "</tr>";
			} else {
				result = "Fields null";
			}
			return result;
		}
		/// <summary>
		/// dump the contents of the object to an string for debugging
		/// 
		/// used for log to file / dlog when html is not available
		/// </summary>
		/// <param name="includeHeader"></param>
		/// <returns></returns>
		public string ToTextRow(bool includeHeader) {
			string result = "";
			if (fields != null) {
				// loop through the Active Field objects in the Active Record object, and get each from db

				if (includeHeader) {
					foreach (var field in fields.Values) {
						if (result.IsNotBlank()) result += ", ";
						result += "" + field.Name + "";
					}
					result += "\n";
				}
				foreach (var field in fields.Values) {
					if (result.IsNotBlank()) result += ", ";

					if (field.IsNull) {
						result += "NULL";
					} else {
						result += "" + field.ToString() + "";
					}
					//result+="sqlized["+field.Sqlize()+"]\n";

				}
				result += "\n";
			} else {
				result = "Fields null";
			}
			return result;
		}

		/// <summary>
		/// Save this record to the database. Either adds a new record or saves existing, depending on IsNewRecord.
		/// </summary>
		public virtual void Save() {
			// prepare for save eg resizing image files
			// prep for save before checking length warnings, as prep will trim attachment filename lengths
			foreach (var field in fields.Values) {
				field.PrepareForSave(this);
			}

			// any data type checks or length checks can be made here
			string lengthWarnings = "";
			foreach (var field in fields.Values) {
				if (field.IsTextField && (field.ValueObject + "").Length > field.MaxLength) {
					// MN/JN 20130220 - removed IF statement, too confusing, lets just always do this
					lengthWarnings += "The field " + field.Name + " (in table " + tableName + ") has a maximum length of " +
															field.MaxLength + " characters, but the value entered is " +
															field.ToString().Length + " characters. Please shorten the data you entered.\n";
					// in either case, truncate the value before save in database
					field.ValueObject = (field.ValueObject + "").Left(field.MaxLength);
				}
			}
			if (lengthWarnings != "") {
				var dump = new Logging.DiagnosticData(true);
				dump.AdditionalMessage = "IMPORTANT ERROR: Length Warning - maxlength exceeded:\n" + lengthWarnings;
				dump.AddExtraInfo("Current Record - " + tableName, this.ToString());
				if (!Util.IsBewebOffice && Util.ServerIsLive) {
					// for live users, send an error report in the background but carry on
					Error.PostErrorReport(dump);
				} else {
					// for staging or dev, stop and show error on screen
					Error.DisplayErrorPage(500, "", dump, false);
				}
				// only throw error to user if they are an administrator, otherwise, they don't want to see it - we just truncate and send ourselves an email
				if (Security.IsLoggedIn && Security.IsAdministratorAccess) {
					// note: could use System.ComponentModel.DataAnnotations.ValidationException but this is .NET 4 only so we have made our own Beweb.UserErrorException
					throw new UserErrorException(lengthWarnings);
				}
			}
			// nullable check
			string requiredWarnings = "";
			foreach (var field in fields.Values) {
				if (!field.IsAuto && field.IsDirtySinceSaved && field.IsBlank && !field.AllowNulls) {  // 20120601 MN only check if dirty (ie field was changed from its original value) - otherwise we can end up showing errors if you add a field to the db but not generate models
					if (requiredWarnings != "") requiredWarnings += ", ";
					requiredWarnings += field.FriendlyName;
				}
			}
			if (requiredWarnings != "") {
				// only throw error to user if they are an administrator, otherwise, they don't want to see it - we just truncate and send ourselves an email
				if (Security.IsLoggedIn && Security.IsAdministratorAccess) {
					throw new UserErrorException("The following fields are required: " + requiredWarnings + ". Please enter values for these.");
				}
			}

			// execute SQL statement
			if (this.IsNewRecord) {
				InsertRecord();
				AddToModificationLog("Create");
			} else {
				UpdateRecord();
				AddToModificationLog("Update");
			}
			// clear dirty since last save
			UpdateSavedValues();
			// cache
			this.StoreInCache();
		}

		private void AddToModificationLog(string actionType) {
#if ModificationLog

			if (!Util.GetSettingBool("UseModificationLog", false)) return;
			if (tableName.ToLower() == "modificationlog") return; //skip saving mod logs about modlog!
			if (tableName.ToLower() == "maillog") return; //skip saving mod logs
			if (tableName.ToLower() == "autocompletephrase") return; //skip saving mod logs

			if (!BewebData.TableExists("ModificationLog")) {
				// Primary Key Constraint seems wrong, should be modificationLog_PK throws error otherwise. JC 20140826
				//new Sql("CREATE TABLE [dbo].[ModificationLog]([ModificationLogID] [int] IDENTITY(1,1) NOT NULL, [TableName] [nvarchar](150) NULL, [ActionType] [nvarchar](150) NULL, [UpdateDate] [datetime] NULL, CONSTRAINT [MailLog_PK] PRIMARY KEY NONCLUSTERED ([MailLogID] ASC))").Execute();
				new Sql("CREATE TABLE [dbo].[ModificationLog]([ModificationLogID] [int] IDENTITY(1,1) NOT NULL, [TableName] [nvarchar](150) NULL, [ActionType] [nvarchar](150) NULL, [UpdateDate] [datetime] NULL, CONSTRAINT [ModificationLog_PK] PRIMARY KEY NONCLUSTERED ([ModificationLogID] ASC))").Execute();
			}
			if (!BewebData.FieldExists("ModificationLog", "PersonID")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [PersonID] [int] NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "UserName")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [UserName] [nvarchar](150) NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "ChangeDescription")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [ChangeDescription] [nvarchar](MAX) NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "RecordID")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [RecordID] [int] NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "RecordIDChar")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [RecordIDChar] [nvarchar](150) NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "PageUrl")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [PageUrl] [nvarchar](350) NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "UserIpAddress")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [UserIpAddress] [nvarchar](350) NULL").Execute();
			if (!BewebData.FieldExists("ModificationLog", "UserAgent")) new Sql("ALTER TABLE [dbo].[ModificationLog] add  [UserAgent] [nvarchar](350) NULL").Execute();

			var inclusions = Util.GetSetting("ModificationLogInclusions", "(all)");
			var exclusions = Util.GetSetting("ModificationLogExclusions", "(none)");
			if (inclusions != "(all)") {
				var inclusionList = inclusions.ToLower().Split(new char[] { '|' });
				if (!inclusionList.Contains(tableName.ToLower())) return; // skip
			}
			if (exclusions != "(none)") {
				var exclusionList = exclusions.ToLower().Split(new char[] { '|' });
				if (exclusionList.Contains(tableName.ToLower())) return; // skip
			}
			var changes = this.GetChangesDescription();
			if (changes.IsNotBlank() || actionType == "Delete") {
				// the changes could be blank if the user didn't update anything but clicked on save
				// and the DateModified field is excluded.
				var ml = new ActiveRecord("ModificationLog", "ModificationLogID");
				ml.AddNew();
				if (Util.GetSettingBool("ModificationLogCopyExtraFields", false))//20120919 JN default to false, so i dont bust my existing site!
				{
					ml.UpdateFrom(this);
				}
				ml["UpdateDate"].ValueObject = DateTime.Now;
				ml["TableName"].ValueObject = tableName;
				if (ID_Field.Type == typeof(int)) {
					ml["RecordID"].ValueObject = this.ID_Field.ValueObject;
				} else {
					changes += "Field ID is " + this.ID_Field.ToString() + "\n";
				}
				ml["ActionType"].ValueObject = actionType;
				// if you get in here and there is no value for the changes then this record has been deleted
				string changeDescription = (changes == "")
								? " Deleted a " + tableName.SplitTitleCase().ToLower() //+ "\n(ID: " + this.ID_Field + ")"
								: changes;// + "\n(ID: " + this.ID_Field + ")";
				if (ml.FieldExists("ChangeDescription")) {
					ml["ChangeDescription"].ValueObject = changeDescription;
				} else {
					ml["ChangeDescr"].ValueObject = changeDescription;
				}
				var userFirstname = Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminFirstName"];
				if (userFirstname != null && ml.FieldExists("UserName")) {
					var un = userFirstname + " " +
					Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminLastName"];
					ml["UserName"].ValueObject = un;
				}

				// MN 20140914 - removed this code - needs to check for field exist and ensure not too long
				//if (Security.LoggedInUserID == 0) {
				//	//ml["UserName"].ValueObject = "-";
				//	ml["UserName"].ValueObject = HttpContext.Current.Request.Url.AbsolutePath;
				//} else {

				// person ID
				if (ml.FieldExists("PersonID")) {
					ml["PersonID"].ValueObject = Security.LoggedInUserID;
				}
				ml["PageUrl"].ValueObject = Web.FullRawUrl.Left(350);
				ml["UserIpAddress"].ValueObject = Web.UserIpAddress.Left(350);
				ml["UserAgent"].ValueObject = Web.Request.UserAgent.Left(350);
				ml.Save();
			}
#else
			return;//"Not available."
#endif

		}

		/// <summary>
		/// After you save a record, you should call this if you want to clear the dirty flag. This will prevent the fields being saved again and stop multiple changelogs being added if you are using dirty flags for change logging. This does not happen by default which is so you can still check whether a field was modified even after it has been saved.
		/// </summary>
		public void ClearDirtyFlag() {
			foreach (var field in fields.Values) {
				if (field.IsDirty) {
					field.OriginalValueObject = field.ValueObject;
				}
			}
		}		
		
		public void UpdateSavedValues() {
			foreach (var field in fields.Values) {
				field.SavedValueObject = field.ValueObject;
			}
		}

		/// <summary>
		/// Delete this record in the database. Also delete any attachments and image files.
		/// </summary>
		public virtual void Delete() {
			Delete(true);
		}

		/// <summary>
		/// Delete this record in the database. Parameter specifies whether to delete any attachments and image files.
		/// </summary>
		public virtual void Delete(bool deleteAttachedFiles) {
			foreach (var field in fields.Values) {
				if (!field.IsBlank) field.PrepareForDelete(deleteAttachedFiles);
			}

			this.RemoveFromCache();
			Sql sql = GetSqlForDelete();
			if (SqlConnectionString != null) sql.SqlConnectionString = SqlConnectionString;
			sql.Execute();

			if (Util.GetSettingBool("UseModificationLog", false)) {
				AddToModificationLog("Delete");
			}
		}

		protected Sql GetSqlForDelete() {
			var sql = new Sql("delete from ", GetTableName().SqlizeName(), " where ", GetPrimaryKeyName().SqlizeName(), "=", ID_Field.Sqlize());
			return sql;
		}

		/// <summary>
		/// public so it can be called from structure
		/// </summary>
		/// <returns></returns>
		public Sql GetSqlForInsert() {
			return GetSqlForInsert(false);
		}

		/// <summary>
		/// public so it can be called from structure
		/// </summary>
		/// <returns></returns>
		public Sql GetSqlForInsert(bool generateDataLoadingScript) {
			var sql = new Sql("insert into ", GetTableName().SqlizeName(), " (");
			//foreach (var field in fields.Values) {
			for (int scan = 0; scan < fields.Keys.Count; scan++) {
				var fieldKey = fields.Keys.ElementAt(scan);
				var field = fields.Values.ElementAt(scan);

				if (field.Name == fieldKey) //check dictionary name / value match
				{
					if (!field.IsAuto) {
						sql.Add("[" + field.Name + "]," + ((generateDataLoadingScript) ? "\n" : ""));
					}
				}
			}
			sql.Value = sql.Value.Trim().TrimEnd(',');
			sql.Add(") values " + ((generateDataLoadingScript) ? "\n" : "") + "(");
			//foreach (var field in fields.Values) {
			for (int scan = 0; scan < fields.Keys.Count; scan++) {
				var fieldKey = fields.Keys.ElementAt(scan);
				var field = fields.Values.ElementAt(scan);

				if (field.Name == fieldKey) //check dictionary name / value match
				{
					if (!field.IsAuto) {
						if (field.IsBlank) {
							// MN 20110905 added this set to null business
							sql.Add("null");
						} else {

							if (generateDataLoadingScript && field.IsTextField)//field.Type == typeof(string))
							{
								sql.AddRawSqlString("\n");
								sql.AddRawSqlString("N'" + PrepForDataLoading(sql, field) + "'");

							} else {
								sql.Add(field.Sqlize());
							}

						}
						sql.Add(",");
					}
				}
			}
			sql.Value = sql.Value.Trim().TrimEnd(',');
			sql.Add(")" + ((generateDataLoadingScript) ? ";\n" : ""));
			return sql;
		}

		private string PrepForDataLoading(Sql sql, ActiveFieldBase field) {
			var txtValue = Fmt.ReplaceSQLChars(Fmt.InsertWebsiteBaseUrlPathMarkers(field.ValueObject + ""));
			var result = txtValue.Replace("<br", "\n<br");
			return result;
		}

		protected void InsertRecord() {
			var sql = GetSqlForInsert();
			if (SqlConnectionString != null) sql.SqlConnectionString = SqlConnectionString;
			string pkname = GetPrimaryKeyName();
			if (!this.fields.ContainsKey(pkname)) {
				throw new ActiveRecordException("InsertRecord(): Primary key not found in ActiveRecord. Expecting primary key named [" + pkname + "].\n\nFields that are present are: " + GetFieldNames().Join(", "));
			}
			//if(!this.fields[pkname].IsNumericField){hasAutoPK=false;}
			//if (hasAutoPK) {
			if (this.fields[pkname].IsAuto) {
				//sql.Add("; select @@identity;");
				//var reader = sql.GetReader();
				//reader.NextResult();
				//reader.Read();
				//var newID = reader[0];
				//this.ID.Value = newID;

				// this next line only works for INT primary keys
				this.fields[GetPrimaryKeyName()].ValueObject = BewebData.InsertRecord(sql);
			} else {
				sql.Execute();
			}
			IsNewRecord = false;
			WasNewRecord = true;
		}

		protected void UpdateRecord() {
			var sql = GetSqlForUpdate(false);
			if (SqlConnectionString != null) sql.SqlConnectionString = SqlConnectionString;
			if (sql.Value.IsNotBlank()) {
				sql.Execute();
			}
		}

		public Sql GetSqlForUpdate() {
			return GetSqlForUpdate(false);
		}

		public Sql GetSqlForUpdate(bool generateDataLoadingScript) {
			var sql = new Sql();
			int scan = 0;
			//foreach (var field in fields.Values) {	//use keys, not values ,see below
			for (scan = 0; scan < fields.Keys.Count; scan++) {
				var fieldKey = fields.Keys.ElementAt(scan);
				var field = fields.Values.ElementAt(scan);

				if (field.Name == fieldKey) //check dictionary name / value match
				{
					//if (!field.IsAuto && field.wasPosted)	//note that this is ok, as field.wasPosted is true by default - but set to false by file uploads types when null
					if (!field.IsAuto && (field.IsDirtySinceSaved || generateDataLoadingScript)) // changed from above MN 13-10-2010
					{
						//var newVal = fields.Values[field];
						if (field.IsBlank) {  // MN 20100512 added this set to null business
							sql.Add("[" + field.Name + "] = null");
						} else {
							if (generateDataLoadingScript && field.IsTextField)//field.Type == typeof(string))
							{
								sql.AddRawSqlString("\n");
								sql.AddRawSqlString("[" + field.Name + "] = N'" + PrepForDataLoading(sql, field) + "'");
							} else {
								sql.Add("[" + field.Name + "] = ", field.Sqlize());
							}
						}
						//if(generateDataLoadingScript)sql.AddRawSqlString("\n");
						sql.Add(",");
					}
				} else { //throw?
					//throw new Exception("keyname, valuename mismatch");
				}
			}
			if (sql.Value.IsNotBlank()) {
				sql.PrependSql(new Sql("update ", GetTableName().SqlizeName(), "set"));
				sql.Value = sql.Value.Trim().TrimEnd(',') + ((generateDataLoadingScript) ? "\n" : "");
				sql.Add("where ", GetPrimaryKeyName().SqlizeName(), "=", this.ID_Field.Sqlize());
			}

			return sql;
		}

		public void CreateTableInDatabase() {
			var sql = GetSqlForCreate();
			sql.Execute();
		}

		public Sql GetSqlForCreate() {
			SqlStringBuilder sql;
			if (GetIsView()) {
				// cant do it really as we dont know the underlying sql create statement for this view
				sql = new Sql("");
			} else {
				sql = GetSqlForCreateTable() + GetSqlForCreateColumns();
			}
			return (Sql)sql;
		}

		private Sql GetSqlForCreateTable() {
			var sql = new Sql("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(", GetTableName().SqlizeText(), ") AND type=", "U".SqlizeText(), ")");
			sql.Add("CREATE TABLE ", GetTableName().SqlizeName(), " (", GetPrimaryKeyName().SqlizeName(), ID_Field.GetSqlDataTypeDeclaration());
			sql.Add(", constraint pk_" + GetTableName() + " primary key (", GetPrimaryKeyName().SqlizeName(), "));");

			return sql;
		}

		private Sql GetSqlForCreateColumns() {
			var sql = new Sql();
			foreach (var field in fields.Values) {
				if (!field.IsAuto) {
					sql.Add("IF NOT EXISTS (SELECT * FROM sys.columns where name=", field.Name.SqlizeText(), " and object_id=OBJECT_ID(", field.TableName.SqlizeText(), "))");
					sql.Add("ALTER TABLE", field.TableName.SqlizeName(), "ADD", field.Name.SqlizeName());
					sql.Add(field.GetSqlDataTypeDeclaration());
					sql.Add(";");

					// alter max length if incorrect
					if (field.IsTextField) {
						sql.Add("IF (SELECT max_length FROM sys.columns where name=", field.Name.SqlizeText(), " and object_id=OBJECT_ID(", field.TableName.SqlizeText(), ")) <> " + field.MaxLength);
						sql.Add("ALTER TABLE", field.TableName.SqlizeName(), "ALTER COLUMN", field.Name.SqlizeName());
						sql.Add(field.GetSqlDataTypeDeclaration());
						sql.Add(";");
					}
				}
			}
			return sql;
		}

		/// <summary>
		/// Returns a WHERE clause (beginning with the word "where").
		/// Looks for commonly used field names that represent Active/Enabled/Visible state of this item. 
		/// Checks PublishDate and ExpiryDate if those fields exist.
		/// Checks IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible,ShowInNav if those fields exist.
		/// You can override this in the model partial (this will then be used by MyModel.LoadActive() and other routines). If overriding MyModel.GetSqlWhereActive() you should also override MyModel.GetIsActive().
		/// </summary>
		public virtual Sql GetSqlWhereActive() {
			var sql = new Sql();

			if (fields.Count == 0) throw new ProgrammingErrorException("call to GetSqlWhereActive, no fields. Perhaps call activeRecord.CheckFieldsCollectionIsPopulated();");
			DelimitedString whereClause = new DelimitedString(" and ");
			whereClause.throwExceptionOnStringContainsDelimiter = false;
			foreach (string possibleFieldName in PossibleFieldNamesForIsActive.Split(',')) {
				//if (fields.ContainsKey(possibleFieldName) && fields[possibleFieldName].Type == typeof(bool)) {
				if (fields.ContainsKey(possibleFieldName) && fields[possibleFieldName].IsBooleanField) {		 //change to 'contains', not =type(bool) to handle nullable bools
					whereClause += "[" + GetTableName() + "]." + possibleFieldName + "=1";
				}
			}
			//PublishDate
			foreach (var name in PossibleFieldNamesForPublishDate.Split(',')) {
				if (fields.ContainsKey(name) && fields[name].IsDateField) {
					whereClause += "(" + "[" + GetTableName() + "]." + name + " is not null and GETDATE() > " + "[" + GetTableName() + "]." + name + ")";
				}
			}
			//ExpiryDate
			foreach (var name in PossibleFieldNamesForExpiryDate.Split(',')) {
				if (fields.ContainsKey(name) && fields[name].IsDateField) {
					if (Advanced.ExpiryDatesHaveTimes) {
						whereClause += "(" + "[" + GetTableName() + "]." + name + " is null or GETDATE() < " + "[" + GetTableName() + "]." + name + ")";
					} else {
						whereClause += "(" + "[" + GetTableName() + "]." + name + " is null or GETDATE() < " + "[" + GetTableName() + "]." + name + "+1)";
					}
				}
			}

			if (whereClause.IsBlank) {
				sql.Add("where 1=1");
			} else {
				sql.Add("where (" + whereClause + ")");
			}
			return sql;
		}

		/// <summary>
		/// Same as GetSqlWhereActive() except it also allows you to include an optional existingRecordID. 
		/// This includes all the active records plus the specific record ID.
		/// </summary>		
		public virtual Sql GetSqlWhereActivePlusExisting(object existingRecordID) {
			CheckFieldsCollectionIsPopulated();
			var sql = GetSqlWhereActive();
			if (existingRecordID != null) {
				if (sql.Value != "where 1=1") {
					sql.Value = sql.Value.Insert(6, "(");
					sql.Add(" OR ", GetPrimaryKeyName().SqlizeName(), "=", Sql.Sqlize(existingRecordID), " )");
				}
			}
			return sql;
		}

		public void UpdateFromRequest() {
			UpdateFromRequest(null, null);
		}

		public void UpdateFromRequest(string prefix, string suffix) {
			UpdateFromRequest(prefix, suffix, false);
		}

		public void UpdateFromRequest(string prefix, string suffix, bool tryNameAlone) {
			bool anyPosted = false;
			foreach (var field in GetFields()) {
				bool wasPosted = field.UpdateFromRequest(prefix, suffix);
				anyPosted = anyPosted || wasPosted;
			}
			if (!anyPosted) {
				throw new ActiveRecordException("UpdateFromRequest: no request fields were posted which matched any of the Active Record field names");
			}
		}
		public void UpdateFromHashtable(Hashtable data) {
			if (data.Count == 0) { throw new ActiveRecordException("UpdateFromHashtable: no data fields in hashtable, cannot update"); }
			bool anyFound = false;
			foreach (var field in GetFields()) {
				if (data.ContainsKey(field.Name)) {
					field.FromString(data[field.Name] + "");
					anyFound = true;
				}
			}
			if (!anyFound) {
				throw new ActiveRecordException("UpdateFromHashtable: no data fields in hashtable which matched any of the Active Record field names");
			}
		}

		/// <summary>
		/// Returns a message if there are children of this instance in the database, or null if record able to be deleted.
		/// </summary>
		/// <returns></returns>
		public virtual string CheckForDependentRecords() {
			//// walk all models 
			//string result=null;
			//Assembly assembly = Assembly.GetExecutingAssembly();
			//foreach (Type type in assembly.GetTypes()) {
			//  if (type.Namespace == "Models" && type.IsSubclassOf(typeof(ActiveRecord))) {
			//    ActiveRecord model = (ActiveRecord)Activator.CreateInstance(type);
			//    //result += model.fields.;
			//  }
			//}           

			PropertyInfo[] theseFields = this.GetType().GetProperties();

			foreach (PropertyInfo property in theseFields) {
				//if (property.PropertyType.IsSubclassOf(typeof(ActiveRecordList<>))) {
				if (property.PropertyType.BaseType.Name == typeof(ActiveRecordList<>).Name && property.Name != "ModificationLogs") {
					var childRecords = (IActiveRecordList)property.GetValue(this, null);
					if (childRecords.Count > 0) {
						return "Related records exist (" + property.Name + ")";
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Copy all field values from another record.
		/// </summary>
		/// <param name="otherRecord"></param>
		public void UpdateFrom(ActiveRecord otherRecord) {
			CheckFieldsCollectionIsPopulated();
			foreach (var field in fields.Values) {
				if (!field.IsAuto && otherRecord.fields.ContainsKey(field.Name)) {
					field.ValueObject = otherRecord.fields[field.Name].ValueObject;
				}
			}
		}



		#region oldstuff
		// massive database
		// read in all records
		// use dataset and keep
		// use dataset and dispose
		// use datareader and dispose
		// check speed and memory usage

		//public static T LoadID<T>(int id)
		//					where T : ActiveRecord, new() {
		//	// see if already in cache, else create sql and load
		//	T record = ActiveRecordCache.Get<T>(id);
		//	if (record==null) {
		//		record = new T();
		//		var sql = new Sql("where",record.GetPrimaryKeyName().SqlizeName(),"=", id);
		//		record.LoadData(sql);
		//		ActiveRecordCache.Store<T>(record);
		//	}
		//	return record;
		//}

		//public static T Load<T>(Sql sql)
		//					where T : ActiveRecord, new() {
		//	// have to create the object first so we can load it, but then we can chuck it away if already cached
		//	// cache here is not for performance but just for ensuring we only have single instance per record
		//	T record = new T();
		//	record.LoadData(sql);
		//	T cachedRecord = ActiveRecordCache.Get<T>(record.ID_Field.ValueObject);
		//	if (cachedRecord != null) {
		//	return cachedRecord;
		//	} else {
		//	ActiveRecordCache.Store<T>(record);
		//	return record;
		//	}
		//}

		//public static T Load<T>(DbDataReader reader)
		//						where T : ActiveRecord, new() {
		//	// we already have a reader here so we just check to see if primary key already in cache
		//	// cache here assists performance only slightly, really just for ensuring we only have single instance per record
		//	string pkName = new T().GetPrimaryKeyName();
		//	var pkValue = reader[pkName];
		//	T record = Web.Cache.Remove().Get<T>(pkValue);
		//	if (record != null) {
		//	return record;
		//	} else {
		//	record = new T();
		//	record.LoadData(reader);
		//	ActiveRecordCache.Store<T>(record);
		//	return record;
		//	}
		//}
		#endregion oldstuff

		public Dictionary<string, ActiveFieldBase>.ValueCollection GetFields() {
			CheckFieldsCollectionIsPopulated();
			return fields.Values;
		}

		private bool _isPopulated = false;

		public void CheckFieldsCollectionIsPopulated() {
			if (_isPopulated) {
				return;
			}
			if (fields.Count == 0) {
				CantFindAnyFieldsSoReadStructureFromDatabase();
			}
			// check record is linked
			foreach (var field in fields.Values) {
				if (field.Record == null) {
					field.Record = this;
				}
			}
			_isPopulated = fields.Count > 0;
		}

		private void CantFindAnyFieldsSoReadStructureFromDatabase() {         // silly long name is too look good in stack trace if readschema crashes
			this.ReadSchemaInfo();
		}

		// MN 20110523 - moved these methods to the correct place
		public string GetChangesDescription() {
			var exclusions = Util.GetSetting("ModificationLogFieldExclusions", "(none)");
			string[] exclusionList = (exclusions == "(none)") ? new string[] { } : exclusions.ToLower().Split(new char[] { '|' });
			string changes = "";
			if (this.IsNewRecord || this.WasNewRecord) {
				changes = "Added new " + this.GetFriendlyTableName().ToLower();// + " (ID:" + this.ID_Field + ")";
			} else {
				foreach (var field in fields.Values) {
					if (!exclusionList.Contains(field.Name.ToLower()) && field.Name.ToLower() != "datemodified" && field.Name.ToLower() != "dateadded" && field.Name.ToLower() != "lastmodified") {
						if (field.IsDirtySinceSaved) {
							string change = field.FriendlyName;
							if (field.OriginalValueToString() != "") {
								if (field.IsForeignKey) {
									change += " changed";
								} else {
									change += " changed from \"" + field.OriginalValueToStringNice() + "\"";
								}
							} else {
								change += " set";
							}
							change += " to \"" + field.ToStringNice() + "\"";
							// made this a setting!!!!
							if (Util.GetSettingBool("ModificationLogShowIDs", false)) {
								if (field.IsForeignKey) {
									change += " (\"" + field.ValueObject + "\")";
								}
							}
							changes += change + "\n";
						}
					}
				}
			}
			return changes;
		}

		/// <summary>
		/// returns true if the current record is now dirty.
		/// </summary>
		/// <returns></returns>
		public bool GetDirty() {
			if (this.IsNewRecord) return true;
			foreach (var field in fields.Values) {
				if (field.IsDirty) return true;
			}
			return false;
		}

		static Random randomGenerator = new Random();
		private double hashNum = randomGenerator.NextDouble();
		public override int GetHashCode() {
			string hash = GetTableName() + "-" + ID_Field.ToString();
			// todo - what if new record?
			if (IsNewRecord) {
				hash += hashNum.ToString();
			}
			return hash.GetHashCode();
		}

		public bool Equals(ActiveRecord other) {
			if (ReferenceEquals(null, other)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			return Equals(other.tableName, tableName) && Equals(other.ID_Field.ValueObject, ID_Field.ValueObject);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			if (obj is ActiveRecord) {
				return Equals((ActiveRecord)obj);
			}
			return false;
		}
		//public static void  CheckAttachmentsForPdfText(ActiveRecord record, int recordID) {
		//	//if (record.Fields.Attachment.IsDirty) {
		//		if (record["Attachment"].ToString().Contains(".pdf")) {
		//			PdfToText.PDFParser pdf = new PDFParser();
		//			string outText = "";
		//			if (!record.FieldExists("AttachmentRAWText")) {
		//				(new Sql("ALTER TABLE ", record.GetTableName().SqlizeName(), " ADD [AttachmentRAWText] nvarchar (MAX);")).Execute();
		//			}

		//			if (pdf.ExtractText(Web.MapPath(Web.Attachments) + record["Attachment"].ToString(), ref outText)) {
		//				(new Sql("update ", record.GetTableName().SqlizeName(), "set AttachmentRAWText=", outText.SqlizeText(), " where ",
		//					record.GetPrimaryKeyName().SqlizeName(), "=", recordID, "")).Execute();
		//			}
		//		} else {
		//			//no pdf any more
		//			if (record.FieldExists("AttachmentRAWText")) {
		//				(new Sql("update ", record.GetTableName().SqlizeName(), "set AttachmentRAWText=null where ",
		//					record.GetPrimaryKeyName().SqlizeName(), "=", recordID, "")).Execute();
		//			}
		//		}
		//	//}
		//}

	} // end ActiveRecord class

	/// <summary>
	/// A generic ActiveRecord for use with any table. The type parameter is for the type of primary key (eg int).
	/// Returns true if record found or false if not found.
	/// In future, all model classes might decend from this.
	/// </summary>
	/// <typeparam name="TPrimaryKey"></typeparam>
	public class ActiveRecord<TPrimaryKey> : ActiveRecord {
		public ActiveRecord(string tableName, string primaryKeyName)
			: base(tableName, primaryKeyName) {
		}

		/// <summary>
		/// The primary key value of the record.
		/// </summary>
		public TPrimaryKey ID { get { return (TPrimaryKey)ID_Field.ValueObject; } set { ID_Field.ValueObject = value; } }

		/// <summary>
		/// Loads data into the current ActiveRecord instance from the database given the supplied primary key value.
		/// Does not use the cache.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of record</param>
		/// <returns>True if record was found</returns>
		public bool LoadID(TPrimaryKey id) {
			// see if already in cache, else create sql and load
			var sql = new Sql("where ", GetPrimaryKeyName().SqlizeName(), "=", Sql.Sqlize(id));
			return this.LoadData(sql);
		}
	}

	/// <summary>
	/// Use this whenever you need to refer to an ActiveRecordList and it doesn't matter what type of records it contains.
	/// </summary>
	public interface IActiveRecordList : IEnumerable {
		int LoopIndex { get; }
		int Count { get; }
		string GetTableName();
		void LoadRecords(Sql sql);
		string GetPrimaryKeyName();
		bool FieldExists(string foreignKey);
		TParentActiveRecord GetParentRecord<TParentActiveRecord>() where TParentActiveRecord : ActiveRecord;
		string GetForeignKeyName();
		string GetFriendlyTableName();
		string GetFriendlyTableNamePlural();
	}

	/// <summary>
	/// Contains additional internal members that outside callers don't need to worry about
	/// </summary>
	public interface IActiveRecordListInternal : IActiveRecordList {
		Dictionary<string, int> PrefetchCounter { get; }
	}

	/// <summary>
	/// ActiveRecordList
	/// </summary>
	/// <typeparam name="TActiveRecord"></typeparam>
	public class ActiveRecordList<TActiveRecord> : IActiveRecordList, IActiveRecordListInternal, IEnumerable<TActiveRecord>
														where TActiveRecord : ActiveRecord, new() {

		// properties ------------------------
		public List<TActiveRecord> innerList = new List<TActiveRecord>();
		private List<TActiveRecord> deletedList = new List<TActiveRecord>();
		protected string recordName = typeof(TActiveRecord).Name;
		public Dictionary<string, int> PrefetchCounter { get; private set; }
		private ActiveRecord parentRecord;
		private string foreignKeyName;
		private string sourceSqlString;
		public string SourceSqlString { get { return sourceSqlString; } }
		private int _pageCount;
		public int PageCount {
			get { return _pageCount; }
			internal set { _pageCount = value; }
		}
		private int _totalRowCount;
		public int TotalRowCount {
			get { return _totalRowCount; }
			internal set { _totalRowCount = value; }
		}

		// contructor ------------------------

		public ActiveRecordList() {
			PrefetchCounter = new Dictionary<string, int>();
		}

		public ActiveRecordList(List<TActiveRecord> activeRecords)
			: this() {
			this.innerList = activeRecords;
		}

		public ActiveRecordList(ActiveRecordList<TActiveRecord> activeRecordList)
			: this() {
			activeRecordList.CopyTo(this);
		}
		public void CopyTo(ActiveRecordList<TActiveRecord> newList) {
			newList.innerList = this.innerList;
			newList.deletedList = this.deletedList;
			newList.parentRecord = this.parentRecord;
			newList.foreignKeyName = this.foreignKeyName;
			newList.sourceSqlString = this.sourceSqlString;
			newList.recordName = this.recordName;
		}

		public ActiveRecordList<TActiveRecord> Copy() {
			ActiveRecordList<TActiveRecord> newList = new ActiveRecordList<TActiveRecord>();
			this.CopyTo(newList);
			return newList;
		}
		// methods ------------------------

		private string _tableName = new TActiveRecord().GetTableName();
		private string _primaryKeyName = new TActiveRecord().GetPrimaryKeyName();
		public void SetTableName(string tableName) { _tableName = tableName; }
		public string GetTableName() { return _tableName; }
		public void SetPrimaryKeyName(string primaryKeyName) { _primaryKeyName = primaryKeyName; }
		public string GetPrimaryKeyName() { return _primaryKeyName;}
		public string GetDefaultOrderBy() { return new TActiveRecord().GetDefaultOrderBy(); }
		public string GetFriendlyTableName() { return new TActiveRecord().GetFriendlyTableName(); }
		public string GetFriendlyTableNamePlural() { return GetFriendlyTableName().Plural(); }

		/// <summary>
		/// If this list was loaded by a parent foreign key, the parent record will be available here.
		/// eg for a list of products lazy loaded by myProdCategory.Products, this would be myProdCategory.
		/// </summary>
		/// <typeparam name="TParentActiveRecord">The type you want back eg ProductCategory or ActiveRecord</typeparam>
		public TParentActiveRecord GetParentRecord<TParentActiveRecord>() where TParentActiveRecord : ActiveRecord {
			return (TParentActiveRecord)this.parentRecord;
		}

		/// <summary>
		/// If this list was loaded by a parent foreign key, the ForeignKeyName will be available here.
		/// eg for a list of products lazy loaded by myProdCategory.Products, this would be "ProductCategoryID".
		/// </summary>
		public string GetForeignKeyName() {
			return foreignKeyName;
		}

		public void SetParentBindField<TParentActiveRecord>(TParentActiveRecord parentRecord, string foreignKeyName) where TParentActiveRecord : ActiveRecord {
			this.parentRecord = parentRecord;
			this.foreignKeyName = foreignKeyName;
		}

		/// <summary>
		/// Add a record to the collection
		/// </summary>
		/// <param name="record"></param>
		public void Add(TActiveRecord record) {
			BindToParentRecordIfRequired(record);
			innerList.Add(record);
		}

		/// <summary>
		/// Add a list of records to the collection
		/// </summary>
		/// <param name="records"></param>
		public void AddMany(IEnumerable<TActiveRecord> records) {
			foreach (var record in records) {
				Add(record);
			}
		}

		/// <summary>
		/// Returns a simple list suitable for being returned as Json using the standard Json return type or JavascriptSerializer
		/// The list contains each record as a dictionary
		/// </summary>
		public List<Dictionary<string, object>> JsonList {
			get {
				var data = new List<Dictionary<string, object>>();
				foreach (var rec in innerList) {
					data.Add(rec.JsonObject);
				}
				return data;
			}
		}

		private void BindToParentRecordIfRequired(TActiveRecord record) {
			// bind to parent record if required
			if (parentRecord != null && foreignKeyName.IsNotBlank()) {
				ActiveFieldBase field = record.GetFieldByName(foreignKeyName);
				if (field.ValueObject != parentRecord.ID_Field.ValueObject) {
					field.ValueObject = parentRecord.ID_Field.ValueObject;
				}
			}
		}

		/// <summary>
		/// Mark all elements in this list for deletion (must call on list.Save() afterwards or use parameter true to delete immediately)
		/// </summary>
		public void DeleteAll() {
			DeleteAll(false);
		}

		/// <summary>
		/// Mark all elements in this list for deletion (must call on list.Save() after or use parameter true to delete immediately)
		/// </summary>
		public void DeleteAll(bool deleteImmediately) {
			deletedList.AddRange(innerList);
			innerList.Clear();
			if (deleteImmediately) {
				foreach (var record in deletedList) {
					record.Delete();
				}
			}
		}

		public void Delete(TActiveRecord record) {
			Delete(record, false);
		}

		public void Delete(TActiveRecord record, bool deleteImmediately) {
			if (deleteImmediately) {
				record.Delete();
			} else {
				deletedList.Add(record);
			}
			innerList.Remove(record);
		}

		public List<TActiveRecord> GetDeletedRecords() {
			// return a copy of the deleted list to ensure no records are added to it.
			return new List<TActiveRecord>(deletedList);
		}

		/// <summary>
		/// Saves all records in the list to the database.
		/// </summary>
		public void Save() {
			foreach (var record in innerList) {
				BindToParentRecordIfRequired(record);
				record.Save();
			}
			foreach (var record in deletedList) {
				record.Delete();
			}
		}
		/// <summary>
		/// using the current table name, call update subform
		/// </summary>
		public ActiveRecordList<TActiveRecord> UpdateFromRequest() {
			UpdateFromRequest(GetTableName());
			return this;
		}

		/// <summary>
		/// using the current table name, check that the dfmaxrow exists (and is named correctly) and call update subform
		/// </summary>
		public void UpdateFromRequestSubForm() {
			string subformName = GetTableName();
			if (Web.Request["df_MaxRow__" + subformName].IsNotBlank()) {
				UpdateFromSubform(subformName);
			} else {
				throw new ProgrammingErrorException("df_MaxRow__ field  missing from subformName[" + subformName + "]");
			}
		}

		/// <summary>
		/// Handle csv of values or subform, or delete all from table requestName.
		/// requestName can be the name of subform or a related checkboxes list.
		/// </summary>
		/// <param name="requestName"></param>
		public void UpdateFromRequest(string requestName) {
			if (Web.Request[requestName].IsNotBlank() && Web.Request[requestName + "__reference"].IsNotBlank()) {
				UpdateFromCommaList(requestName);
			} else if (Web.Request["df_MaxRow__" + requestName].IsNotBlank()) {
				UpdateFromSubform(requestName);
			} else {
				DeleteAll();
			}
		}

		private void UpdateFromCommaList(string requestName) {
			// reference is the reference field to find the field value (eg. NewsletterTargetGroupID)
			string reference = Web.Request[requestName + "__reference"];
			if (reference.Contains(",")) {
				// allow people to include two groups of the same checkboxes on the page, they should have the same reference
				reference = reference.Split(",")[0];
			}
			List<string> values = Web.Request[requestName].Split(',').ToList();
			List<TActiveRecord> checkRecords = new List<TActiveRecord>();
			foreach (string value in values) {
				TActiveRecord rec = innerList.FirstOrDefault(b => (b.GetFieldByName(reference) != null && b.GetFieldByName(reference).ToString() == value));
				if (rec == null) {
					TActiveRecord newRecord = new TActiveRecord();
					newRecord.GetFieldByName(reference).FromString(value);   // MN 20130731 - allows non-int types eg strings
					Add(newRecord);

					checkRecords.Add(newRecord);
				} else {
					checkRecords.Add(rec);
				}
			}

			// add orphan records to a deleted list
			List<TActiveRecord> recordsToDelete = new List<TActiveRecord>();
			foreach (TActiveRecord record in innerList) {
				if (!checkRecords.Contains(record)) {
					recordsToDelete.Add(record);
				}
			}

			// delete that deleted list
			foreach (TActiveRecord record in recordsToDelete) {
				Delete(record);

				this.sourceSqlString.ToString();
			}
		}

		private void UpdateFromSubform(string subformName) {
			if (Web.Request["df_MaxRow__" + subformName] == null) { throw new ProgrammingErrorException("df_MaxRow__ field  missing from subformName[" + subformName + "]"); }
			int maxIndex = Web.Request["df_MaxRow__" + subformName].ToInt(innerList.Count);
			for (int lineNum = 0; lineNum <= maxIndex; lineNum++) {
				string suffix = "__" + subformName + "__" + lineNum;
				string recordStatus = Web.Request["df_status" + suffix];
				if (recordStatus.IsBlank()) continue; // not found in request so skip this one (user might have hit back button and maxRow is out of whack)
				if (recordStatus.IsNotBlank() && "existing".Equals(recordStatus)) {
					TActiveRecord record = GetRecordByPk(Web.Request["df_recordId" + suffix]);
					if (record != null) {
						record.UpdateFromRequest(null, suffix);
					} else {
						// MN 20141215 - just treat as a new item and save it rather than throw
						// this is an rare situation where someone else has deleted this row while it was on screen 
						//throw new ProgrammingErrorException("ActiveRecord.UpdateFromSubform() - Subform name["+subformName+"] Master ["+GetTableName()+"] ID["+this.parentRecord.ID_Field.ValueObject+"] Could not find subform record with record ID " + Web.Request["df_recordId" + suffix]);
						record = new TActiveRecord();
						record.UpdateFromRequest(null, suffix);
						Add(record);
					}
				} else if (recordStatus.IsNotBlank() && "new".Equals(recordStatus)) {
					TActiveRecord record = new TActiveRecord();
					record.UpdateFromRequest(null, suffix);
					Add(record);
				} else if (recordStatus.IsNotBlank() && "deleted".Equals(recordStatus)) {
					TActiveRecord record = GetRecordByPk(Web.Request["df_recordId" + suffix]);
					if (record != null) {
						Delete(record);
					} else {
						//throw new Exception("Could not find record with record ID [" + Web.Request["df_recordId" + suffix]+"] named ["+("df_recordId" + suffix)+"]");
					}
				} else if (recordStatus.IsNotBlank() && "ignore".Equals(recordStatus)) {
					//hidden record, dont add it
				} else {
#if Debug
					Logging.DumpFormHTML();
					Web.Flush();

#endif
					throw new ProgrammingErrorException("Do not know how to handle record status [" + recordStatus + "], subformName[" + subformName + "], max[" + (Web.Request["df_MaxRow__" + subformName]) + "], recid[" + (Web.Request["df_recordId" + suffix]) + "]");
				}
			}
		}

		private TActiveRecord GetRecordByPk(object pk) {
			if (pk == null) return null;
			return this.GetSingleRecord(rec => rec.ID_Field.ToString() == pk.ToString());
		}

		/// <summary>
		/// Loads all records into the current instance from database given sql statement. 
		/// If there are already objects in the list, the new objects will be added at the end.
		/// If found in the cache, records will be loaded from the cache.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		public void LoadRecords(Sql sql) {
			LoadRecords(sql, true);   // MN & JB 20110725 - BREAKING CHANGE - this was originally true and we are changing it back to true as it only adds the order by if there is not already an order by and there is no point in having no order by because sql server will then return records in undefined order
		}

		/// <summary>
		/// Loads all records into the current instance from database given sql statement. 
		/// If there are already objects in the list, the new objects will be added at the end.
		/// If found in the cache, records will be loaded from the cache.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <param name="useDefaultOrderBy"></param>
		public void LoadRecords(Sql sql, bool useDefaultOrderBy) {
			if (sql != null) sourceSqlString = sql.Value;
			ActiveRecordLoader.LoadRecords(this, sql, useDefaultOrderBy);
		}

		///// <summary>
		///// Loads all records into the current instance from database given sql statement
		///// </summary>
		///// <param name="sql"></param>
		//public void LoadData(Sql sql) {
		//	if (sql.Value.StartsWith("where ")) {
		//	string tableName = new TActiveRecord().GetTableName();
		//	sql.Prepend("SELECT * FROM " + tableName + "");
		//	}
		//	string pkName = new TActiveRecord().GetPrimaryKeyName();
		//	var reader = sql.GetReader();
		//	while (reader.Read()) {
		//	var pkValue = reader[pkName];
		//	var record = ActiveRecordCache.Get<TActiveRecord>(pkValue);
		//	if (record == null) {
		//		record = new TActiveRecord();
		//		record.LoadData(reader);
		//		ActiveRecordCache.Store<TActiveRecord>(record);
		//	}
		//	this.Add(record);
		//	}
		//	reader.Close();
		//}

		///// <summary>
		///// Loads all records from database given sql statement into a list and returns the list.
		///// </summary>
		///// <param name="sql"></param>
		///// <returns>An instance of a typed list of Active Records</returns>
		//public static ActiveRecordList<TActiveRecord> Load(Sql sql) {
		//	var result = new ActiveRecordList<TActiveRecord>();
		//	result.LoadData(sql);
		//	return result;
		//}

		//public static AdministratorList Load(Sql sql) {
		//	var result = new AdministratorList();
		//	if (sql.Value.StartsWith("where ")) {
		//	sql.Prepend("SELECT * FROM [Administrator]");
		//	}
		//	var reader = sql.GetReader();
		//	while (reader.Read()) {
		//	result.Add(Administrator.Load(reader));
		//	}
		//	return result;
		//}

		///// <summary>
		///// This method populates child models in a collection from the database, used for lazy loading
		///// eg say you have a list of OrderItems, this would allow you to load all Product objects:
		/////	 LoadChildModels<Product>(myOrderItems, "Product", "Product", "ProductID")
		///// 
		///// The ideas is to transparently lazy load in cases like this:
		/////	 foreach (item in myOrderItems) { str += item.Product.Name }
		///// 
		///// </summary>
		///// <returns></returns>
		//public void LoadChildModels<TChildModel>(string childPropertyName, string childTableName, string childPrimaryKeyName) where TChildModel : ActiveRecord, new() {
		//	// get list of child IDs
		//	string targetIDs = "";
		//	foreach (ActiveRecord model in this) {
		//	if (targetIDs != "") targetIDs += ",";
		//	targetIDs += model.GetPropertyValue(model.GetPrimaryKeyName());
		//	}

		//	// load child objects with those IDs as PKs
		//	var sql = new Sql();
		//	// TODO: next line should depend on type of PK
		//	sql.Add("select * from ", childTableName.SqlizeName(), " where ", childPropertyName.SqlizeName(), " in (", targetIDs.SqlizeTextList(), ")");
		//	var childModels = sql.LoadPoo<List<TChildModel>>();

		//	// set references in the models to those new child objects 
		//	// this wont be required if objects are always stored in an Identity Map
		//	foreach (ActiveRecord model in this) {
		//	ActiveRecord thisModel = model;	 // this statement is required by compiler
		//	var childModel = childModels.Find(child => child.GetPropertyValue(childPrimaryKeyName) == thisModel.GetPropertyValue(childPropertyName + "ID"));
		//	model.SetPropertyValue(childPropertyName, childModel);
		//	}
		//}

		/// <summary>
		/// Returns the number of records in the collection
		/// </summary>
		public int Count { get { return innerList.Count; } }
		/// <summary>
		/// Returns the number of records in the collection (same as Count)
		/// </summary>
		public int RecordCount { get { return innerList.Count; } }

		#region Implementation of IEnumerable
		/// <summary>
		/// Returns the zero based index of the current loop	- eg foreach
		/// <example>
		/// foreach(var item in list)
		/// {
		///		//current item index is list.LoopIndex, start at 0
		/// }
		/// </example>
		/// </summary>
		public int LoopIndex { get { return (int)Web.PageGlobals[recordName + "_LoopIndex"]; } }

		/// <summary>
		///	Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<TActiveRecord> GetEnumerator() {
			for (int i = 0; i < innerList.Count; i++) {
				var record = innerList[i];
				record.SetContainingList(this);
				Web.PageGlobals[recordName + "_LoopIndex"] = i;
				yield return record;
			}
		}

		/// <summary>
		///	Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Indexer
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public TActiveRecord this[int index] {
			get {
				return innerList[index];
			}
			set {
				innerList[index] = value;
			}
		}
		public string DumpData() {
			string result = "";
			foreach (var dataitem in this.innerList) {
				result += dataitem.DumpFields();
			}
			return result;
		}

		/// <summary>
		/// Return a record that matches a criteria expressed as a lambda expression.
		/// eg rec => rec.Name=="mike"
		/// </summary>
		/// <param name="filterLambdaExpression"></param>
		/// <returns></returns>
		public TActiveRecord GetSingleRecord(Predicate<TActiveRecord> filterLambdaExpression) {
			return innerList.Find(filterLambdaExpression);
		}

		/// <summary>
		/// Return a list of records that matches a criteria expressed as a lambda expression.
		/// eg rec => rec.Description.Contains("hello")
		/// </summary>
		/// <param name="filterLambdaExpression"></param>
		/// <returns></returns>
		public ActiveRecordList<TActiveRecord> Filter(Predicate<TActiveRecord> filterLambdaExpression) {
			var matchingActiveRecords = innerList.FindAll(filterLambdaExpression);
			var result = this.Copy();
			result.innerList = matchingActiveRecords;
			return result;
		}

		public TActiveRecordList Filter<TActiveRecordList>(Predicate<TActiveRecord> filterLambdaExpression)
			where TActiveRecordList : ActiveRecordList<TActiveRecord>, IActiveRecordList, new() {
			List<TActiveRecord> matchingActiveRecords = innerList.FindAll(filterLambdaExpression);
			var resultList = new TActiveRecordList();
			this.CopyTo(resultList);
			resultList.innerList = matchingActiveRecords;
			return resultList;
		}

		/// <summary>
		/// Return all the active records. This means all those which return true if you call record.GetIsActive() on them.
		/// By default this checks any boolean fields named such as "IsActive", "Active", "Enabled" as well as date fields "PublishDate" and/or "ExpiryDate" to see if the record is active.
		/// </summary>
		/// <returns></returns>
		public ActiveRecordList<TActiveRecord> Active {
			get { return Filter(item => item.GetIsActive()); }
		}

		public bool FieldExists(string fieldName) {
			return this.FirstOrNew().FieldExists(fieldName);
		}

		/// <summary>
		/// Searches for item in list based on lambda expression, and returns first match. If not found returns null.
		/// </summary>
		public TActiveRecord Find(Predicate<TActiveRecord> match) {
			if (Count > 0) {
				return innerList.Find(match);
			} else {
				return null;
			}
		}

		public TActiveRecord FirstOrNew() {
			if (Count > 0) {
				return this[0];
			} else {
				return new TActiveRecord();
			}
		}
		/// <summary>
		/// Return first record in list. If there are none, throw error.
		/// </summary>
		public TActiveRecord First() {
			return First(Otherwise.ProgrammingError);
		}
		public TActiveRecord First(Otherwise otherwise) {
			if (Count > 0) {
				return innerList.First();
			} else {
				return otherwise.Execute<TActiveRecord>();
			}
		}
		/// <summary>
		/// Return last  record in list. If there are none, throw error.
		/// </summary>
		public TActiveRecord Last() {
			return Last(Otherwise.ProgrammingError);
		}
		public TActiveRecord Last(Otherwise otherwise) {
			if (Count > 0) {
				return innerList.Last();
			} else {
				return otherwise.Execute<TActiveRecord>();
			}
		}

		public ActiveRecordList<TActiveRecord> Reverse() {
			this.innerList.Reverse();
			return this;
		}

		public bool Contains(TActiveRecord record) {
			return innerList.Contains(record);
		}

		public bool DoesntContain(TActiveRecord record) {
			return !innerList.Contains(record);
		}

		/// <summary>
		/// implicit conversion (ie requires a cast) from ActiveRecordList to List. Experimental.
		/// </summary>
		public static implicit operator List<TActiveRecord>(ActiveRecordList<TActiveRecord> activeRecordList) {
			return new List<TActiveRecord>(activeRecordList.innerList);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from List to ActiveRecordList. Experimental.
		/// </summary>
		public static implicit operator ActiveRecordList<TActiveRecord>(List<TActiveRecord> list) {
			return new ActiveRecordList<TActiveRecord>(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ActiveRecordList of typed models to List of ActiveRecords. Experimental.
		/// </summary>
		public static implicit operator List<ActiveRecord>(ActiveRecordList<TActiveRecord> list) {
			return list.Cast<ActiveRecord>().ToList();
		}

		#region "notes about enumerator alternative method"
		//class Employees : IEnumerable, IEnumerator {
		//	ArrayList EmpList = new ArrayList();
		//	private int Position = -1;
		//	public void AddEmployee(Employee oEmp) {
		//	EmpList.Add(oEmp);
		//	}
		//	/* Needed since Implementing IEnumerable*/
		//	public IEnumerator GetEnumerator() {
		//	return (IEnumerator)this;
		//	}
		//	/* Needed since Implementing IEnumerator*/
		//	public bool MoveNext() {
		//	if (Position < EmpList.Count - 1) {
		//		++Position;
		//		return true;
		//	}
		//	return false;
		//	}
		//	public void Reset() {
		//	Position = -1;
		//	}
		//	public object Current {
		//	get {
		//		return EmpList[Position];
		//	}
		//	}
		//}
		#endregion


		/// <summary>
		/// dump the contents of the object to an html TR for debugging
		/// </summary>
		/// <returns></returns>
		public string ToHtmlTable() {
			string result = "<table>";
			int i = 0;
			foreach (var rec in innerList) {
				result += rec.ToHtmlTableRow(i == 0);
				i++;
			}
			return result + "</table>";
		}
		public string ToTextTable() {
			string result = "\n";
			int i = 0;
			foreach (var rec in innerList) {
				result += rec.ToTextRow(i == 0);
				i++;
			}
			return result + "\n";
		}

	}

	///// <summary>
	///// Used by ActiveRecord loading routines to ensure that if you try to load the same record twice it uses the one already in memory.
	///// This is to prevent loading the same data and then updating it in several places.
	///// Uses the Martin Fowler pattern "Identity Map"
	///// </summary>
	//public static class ActiveRecordCache {

	//	/// <summary>
	//	/// Looks in the cache to see if we have already loaded the ActiveRecord of the given type with the specified primary key value.
	//	/// Returns null if not found.
	//	/// </summary>
	//	/// <typeparam name="TActiveRecord">The type of ActiveRecord (eg Administrator or NewsItem)</typeparam>
	//	/// <param name="pkValue">The ID or primary key value</param>
	//	/// <returns>Returns the ActiveRecord of the given type or null if not found.</returns>
	//	public static TActiveRecord Get<TActiveRecord>(object pkValue)
	//								 where TActiveRecord : ActiveRecord, new() {
	//	string key = "ActiveRecord-" + typeof(TActiveRecord).Name + "-" + pkValue;
	//	// see if in cache and if not return null
	//	return Web.Cache.Get(key) as TActiveRecord;
	//	}

	//	internal static void Store<TActiveRecord>(TActiveRecord record)
	//								where TActiveRecord : ActiveRecord, new() {
	//	string key = "ActiveRecord-" + typeof(TActiveRecord).Name + "-" + record.ID_Field.ValueObject;
	//	Web.Cache.Insert(key, record);
	//	}
	//}


	public class ActiveRecordException : ApplicationException {
		public ActiveRecordException(string message) : base(message) { }
		public ActiveRecordException(string message, Exception innerException) : base(message, innerException) { }
	}


	/*
	public class ModelBaseLinq {

		// future idea:
		// it may be possible to populate related models from a sql join
		// eg select * from OrderItems inner join Product
		// can we check the base tables of the fields?
		// then read them into the appropriate models


		/// <summary>
		/// Populates child models in a collection from the database, used for lazy loading
		/// eg say you have a list of OrderItems, this would allow you to load all Product objects:
		///	 LoadChildModels<Product>(myOrderItems, "Product", "Product", "ProductID")
		/// 
		/// The ideas is to transparently lazy load in cases like this:
		///	 foreach (item in myOrderItems) { str += item.Product.Name }
		/// 
		/// </summary>
		/// <returns></returns>
		public static void LoadChildModels<TChildModel>(List<ActiveRecord> modelList, string childPropertyName, string childTableName, string childPrimaryKeyName) where TChildModel : new() {
			// get list of child IDs
			string targetIDs = "";
			foreach (var model in modelList) {
				if (targetIDs != "") targetIDs += ",";
				targetIDs += model.GetPropertyValue(childPropertyName + "ID");
			}

			// load child objects with those IDs as PKs
			//	var sql = new Sql("select * from %f1 where %f2 in (%ai3)", childTableName, childPropertyName, targetIDs);
			var sql = new Sql();
			// TODO: next line should depend on type of PK
			sql.Add("select * from ", childTableName.SqlizeName(), " where ", childPropertyName.SqlizeName(), " in (", targetIDs.SqlizeTextList(), ")");
			//			sql.Add(" and keyword like '", Request["keywords"].SqlizeLike("PRODUCT_", "%"), " and 1=", 1);
			var childModels = sql.LoadPoo<List<TChildModel>>();

			// set references in the models to those new child objects 
			// this wont be required if objects are always stored in an Identity Map
			foreach (ActiveRecord model in modelList) {
				ActiveRecord thisModel = model;	 // this statement is required by compiler
				var childModel = childModels.Find(child => child.GetPropertyValue(childPrimaryKeyName) == thisModel.GetPropertyValue(childPropertyName + "ID"));
				model.SetPropertyValue(childPropertyName, childModel);
			}
		}


		// binder stuff
		public System.Collections.Generic.List<Beweb.Forms.SavvyFieldBinder> FieldBinders = new System.Collections.Generic.List<Beweb.Forms.SavvyFieldBinder>();

		protected Beweb.Forms.SavvyFieldBinder CreateFieldBinder(string propertyName) {
			var binder = new Beweb.Forms.SavvyFieldBinder(propertyName);
			this.FieldBinders.Add(binder);
			return binder;
		}

		protected void InitSavvyFieldBinders() {
			foreach (var binder in this.FieldBinders) {
				binder.Init(this);
			}
		}
		// end binder stuff


	}
	*/

	//public interface IActiveRecordLoadSource {
	//	/// <summary>
	//	/// Returns the IDs (primary key values) of any siblings of the given object within the list.
	//	/// eg if there is a list of customers 
	//	/// </summary>
	//	void GetIDsForSiblingOf(object me);

	//	/// <summary>
	//	/// Loads all siblings of the given object within this list.
	//	/// </summary>
	//	/// <param name="me"></param>
	//	void LoadSiblingsOf(object me);
	//}

	/// <summary>
	/// Helper object for internals of ActiveRecord
	/// </summary>
	public class ActiveRecordLoader {
		private static List<string> _classNamesToCache = null;
		public static List<string> ClassNamesToCache {
			get {
				if (_classNamesToCache == null) {
					_classNamesToCache = Util.GetSetting("SavvyActiveRecord_CacheClasses", "").Split('|').ToList();
				}
				return _classNamesToCache;
			}
		}
		private static TimeSpan? _cacheDuration = null;
		public static TimeSpan CacheDuration {
			get {
				if (_cacheDuration == null) {
					double minutes = Util.GetSetting("SavvyActiveRecord_CacheDurationMinutes", "0").ToDouble();
					_cacheDuration = TimeSpan.FromMinutes(minutes);
				}
				return _cacheDuration.Value;
			}
		}

		/// <summary>
		/// Loads all records into the given Active Record List instance from database given sql statement. 
		/// If there are already objects in the list, the new objects will be added at the end.
		/// If found in the cache, records will be loaded from the cache.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		public static void LoadRecords<TActiveRecord>(ActiveRecordList<TActiveRecord> list, Sql sql) where TActiveRecord : ActiveRecord, new() {
			LoadRecords<TActiveRecord>(list, sql, false);
		}

		/// <summary>
		/// Loads all records into the given Active Record List instance from database given sql statement. 
		/// If there are already objects in the list, the new objects will be added at the end.
		/// If found in the cache, records will be loaded from the cache.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		public static void LoadRecords<TActiveRecord>(ActiveRecordList<TActiveRecord> list, Sql sql, bool useDefaultOrderBy) where TActiveRecord : ActiveRecord, new() {
			LoadRecords<TActiveRecord>(list, sql, useDefaultOrderBy, false);
		}

		/// <summary>
		/// Same as LoadRecords except it skips a few things:
		/// (1) does not check for extra fields in the sql that are not in the ActiveRecord class.
		/// (2) does not check the cache, does not store in cache.
		/// This shaves off a few milliseconds if loading a few thousand records. Also try SELECTing only the fields you need.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		public static void LoadRecordsBulk<TActiveRecord>(ActiveRecordList<TActiveRecord> list, Sql sql) where TActiveRecord : ActiveRecord, new() {
			LoadRecords<TActiveRecord>(list, sql, true, true);
		}

		/// <summary>
		/// Loads all records into the given Active Record List instance from database given sql statement. 
		/// If there are already objects in the list, the new objects will be added at the end.
		/// If found in the cache, records will be loaded from the cache.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		public static void LoadRecords<TActiveRecord>(ActiveRecordList<TActiveRecord> list, Sql sql, bool useDefaultOrderBy, bool useFastBulkMode) where TActiveRecord : ActiveRecord, new() {
			string tableName = list.GetTableName();
			string primaryKeyName = list.GetPrimaryKeyName();
			if (sql == null) {
				sql = new Sql("SELECT * FROM " + tableName);
			}
			if (sql.Value.ToLower().StartsWith("where ")) {
				sql.Prepend("SELECT * FROM " + tableName);
			}
			if (useDefaultOrderBy && !sql.Value.ToLower().Contains("order by ") && !sql.Value.TrimEnd().EndsWith(";")) {
				sql.Add(new TActiveRecord().GetDefaultOrderBy());
			}
			using (var reader = sql.GetReader()) {
				int visibleFieldCount = 0;
				string[] fieldNamesArray = null;
				int rec = 1;
				while (reader.Read()) {
					TActiveRecord record;
					if (useFastBulkMode) {
						if (visibleFieldCount == 0) {
							fieldNamesArray = reader.GetFieldNames().ToArray();
							visibleFieldCount = reader.VisibleFieldCount;
						}
						record = ActiveRecordLoader.LoadFastBulk<TActiveRecord>(reader, fieldNamesArray, visibleFieldCount);
					} else {
						record = ActiveRecordLoader.Load<TActiveRecord>(reader, tableName, primaryKeyName, Otherwise.Null);
					}
					record.SourceSqlString = sql.ToString();//save this in active record incase there is an error later
					list.Add(record);
					sql.UpdateTotalRowCount(rec, reader);
					rec++;
					if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
				}
				reader.Close();
				int numLoaded = rec - 1;
				if (numLoaded > 0) {
					sql.LogEndQueryRecordsLoaded("ActiveRecord: " + numLoaded + " " + tableName + " records loaded");
				}
				if (sql.EnablePaging && sql.ResultSetPagingType == Sql.PagingType.sql2005) {
					list.TotalRowCount = sql.TotalRowCount;
					list.PageCount = sql.PageCount;
				}
			}
		}

		/// <summary>
		/// Called by LoadRecordsBulk
		/// </summary>
		private static TActiveRecord LoadFastBulk<TActiveRecord>(DbDataReader reader, string[] fieldNamesArray, int visibleFieldCount) where TActiveRecord : ActiveRecord, new() {
			var record = new TActiveRecord();
			for (int i = 0; i < visibleFieldCount; i++) {	// for each column
				string fieldName = fieldNamesArray[i];
				object value = reader.GetValue(i);
				record[fieldName].ValueObject = value;
				record[fieldName].OriginalValueObject = value;
			}
			record.IsNewRecord = false;
			return record;
		}

		public static TActiveRecord LoadID<TActiveRecord>(object id, string tableName, Otherwise otherwise) where TActiveRecord : ActiveRecord, new() {
			// see if already in cache, else create sql and load
			var record = GetFromCache<TActiveRecord>(id, tableName);
			if (record == null) {
				record = new TActiveRecord();  // MN 20120402 - moved into this if
				if (record.GetPrimaryKeyName().IsBlank()) {
					throw new ActiveRecordException("ActiveRecordLoader.LoadID: Trying to load a record but primary key name is not specified. This cannot be called on ActiveRecord base class.");
				}
				if (!record.LoadDataByID(id)) return otherwise.Execute<TActiveRecord>();
				StoreInCache(record, tableName);
			}
			return record;
		}

		/// <summary>
		/// Load a list of records by whatever given foreign key field name.
		/// Returns serveral records, one for each value in the searchValues list supplied. 
		/// This is for foreign key lookups, so in other words the field value must be unique in the lookup table (eg customer.code or customer.ID).
		/// This method cannot be used where multiple records may have the same field value (eg customer.branch) as it will only return the first one and then think it is cached.
		/// </summary>
		public static IEnumerable<TActiveRecord> LoadRecordsByKeyValues<TActiveRecord>(string fieldName, IEnumerable searchValues, string tableName, Otherwise otherwise) where TActiveRecord : ActiveRecord, new() {
			var result = new List<TActiveRecord>();
			foreach (object searchVal in searchValues) {
				result.Add(LoadByField<TActiveRecord>(fieldName, searchVal, tableName, otherwise));
			}
			return result;
		}

		/// <summary>
		/// Returns a single record with matching field value. 
		/// This is for foreign key lookups, so in other words the field value must be unique in the lookup table (eg customer.code or customer.ID).
		/// This method cannot be used where multiple records may have the same field value (eg customer.branch) as it will only return the first one and then think it is cached.
		/// </summary>
		public static TActiveRecord LoadByField<TActiveRecord>(string fieldName, object searchValue, string tableName, Otherwise otherwise) where TActiveRecord : ActiveRecord, new() {
			// see if already in cache, else create sql and load
			TActiveRecord record = GetFromCacheByField<TActiveRecord>(fieldName, searchValue, tableName);
			if (record == null) {
				record = new TActiveRecord();
				if (record.GetTableName().IsBlank()) {
					record.Advanced.SetTableName(tableName);
				}
				if (!record.LoadDataByField(fieldName, searchValue)) return otherwise.Execute<TActiveRecord>();
				StoreInCache(record, tableName);
			}
			return record;
		}

		/// <summary>
		/// Loads a single record from the given table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ActiveRecord class containing the data in the record</returns>
		public static TActiveRecord Load<TActiveRecord>(Sql sql, string tableName) where TActiveRecord : ActiveRecord, new() {
			return Load<TActiveRecord>(sql, tableName, Otherwise.Null);
		}

		/// <summary>
		/// Loads a single record from the given table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ActiveRecord class containing the data in the record</returns>
		public static TActiveRecord Load<TActiveRecord>(Sql sql, string tableName, Otherwise otherwise) where TActiveRecord : ActiveRecord, new() {
			// have to create & load the object first so we can get its ID, but then we can chuck it away if already cached
			// cache here is not for performance but just for ensuring we only have single instance of each record

			// first load to get ID
			var tempRecord = new TActiveRecord();
			if (!tempRecord.LoadData(sql)) return otherwise.Execute<TActiveRecord>();

			string id = tempRecord.ID_Field.ToString();
			string fieldNames = tempRecord.GetFieldNames().Join(",");
			bool allowCaching = (fieldNames == new TActiveRecord().GetFieldNames().Join(","));
			TActiveRecord record;
			if (allowCaching) {
				record = GetFromCache<TActiveRecord>(id, tableName);
				if (record == null) {
					// not found so use the one we just loaded and cache it
					record = tempRecord;
					StoreInCache(record, tableName);
				}
			} else {
				record = tempRecord;
			}
			return record;
		}

		/// <summary>
		/// Loads a single record given a DataReader containing data from the given table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of given class containing the data in the record</returns>
		public static TActiveRecord Load<TActiveRecord>(DbDataReader reader, string tableName) where TActiveRecord : ActiveRecord, new() {
			return Load<TActiveRecord>(reader, tableName, Otherwise.Null);
		}

		/// <summary>
		/// Loads a single record given a DataReader containing data from the given table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of given class containing the data in the record</returns>
		public static TActiveRecord Load<TActiveRecord>(DbDataReader reader, string tableName, Otherwise otherwise) where TActiveRecord : ActiveRecord, new() {
			var primaryKeyName = new TActiveRecord().GetPrimaryKeyName();
			return Load<TActiveRecord>(reader, tableName, primaryKeyName, otherwise);
		}

		/// <summary>
		/// Loads a single record given a DataReader containing data from the given table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of given class containing the data in the record</returns>
		public static TActiveRecord Load<TActiveRecord>(DbDataReader reader, string tableName, string primaryKeyName, Otherwise otherwise) where TActiveRecord : ActiveRecord, new() {
			// we already have a reader here so we just check to see if primary key already in cache
			// cache here assists performance only slightly, really just for ensuring we only have single instance per record

			bool allowCaching = false;
			TActiveRecord record = null;

			if (tableName.IsNotBlank()) {
				// get ID from reader
				object id;
				var selectedFieldNameList = reader.GetFieldNames();
				selectedFieldNameList.Sort();
				string selectedFieldNames = selectedFieldNameList.Join(", ");

				try {
					id = reader[primaryKeyName];
				} catch (IndexOutOfRangeException ex) {
					throw new ActiveRecordException("Beweb ActiveRecord " + tableName + ".Load() Error: Trying to load a '" + tableName + "' but this does not seem to be the right SQL. Could not find the primary key " + primaryKeyName + " in the DataReader. Check your SQL - are you selecting from the wrong table?\n\nThe following fields were actually found in the DataReader: " + selectedFieldNames + "\n\nLast sql: " + Web.PageGlobals["SqlTraceLastQuery"], ex);
				}

				var generatedFieldNameList = new TActiveRecord().GetFieldNames();
				generatedFieldNameList.Sort();
				allowCaching = (selectedFieldNames.ToLower() == generatedFieldNameList.Join(", ").ToLower());  // only allow caching if "select * from"
				if (allowCaching) {
					record = GetFromCache<TActiveRecord>(id, tableName);
				}
			}
			if (record == null) {
				record = new TActiveRecord();
				if (record.GetType() == typeof (ActiveRecord)) {
					record = (TActiveRecord) new ActiveRecord(tableName, primaryKeyName);
				}
				record.Advanced.SetTableName(tableName);
				if (!record.LoadData(reader)) return otherwise.Execute<TActiveRecord>();
				if (allowCaching) {
					StoreInCache(record, tableName);
				}
			}
			return record;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ActiveRecord with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ActiveRecord or null if not in cache.</returns>
		public static TActiveRecord GetFromCache<TActiveRecord>(object id, string tableName) where TActiveRecord : ActiveRecord {
			//return Web.Cache.Get("ActiveRecord-Person-" + id) as Person;
			if (DisableCaching) return null;
			string key = "ActiveRecord-" + tableName + "-" + id;
			//key += "-" + fieldNames;
			TActiveRecord rec;
			if (ClassNamesToCache.Contains(tableName)) {
				rec = Web.Cache[key] as TActiveRecord;
				if (rec != null) {
					rec.SourceLoadedFrom = "AspNetCache";
				}
			} else {
				rec = Web.PageGlobals[key] as TActiveRecord;
				if (rec != null) {
					rec.SourceLoadedFrom = "PageGlobalsCache";
				}
			}
			if (rec == null) return null;   // not in cache			
			return rec;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ActiveRecord with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ActiveRecord or null if not in cache.</returns>
		public static TActiveRecord GetFromCacheByField<TActiveRecord>(string fieldName, object searchValue, string tableName) where TActiveRecord : ActiveRecord, new() {
			//return Web.Cache.Get("ActiveRecord-Person-" + id) as Person;
			if (DisableCaching) return null;
			TActiveRecord rec = null;

			// check here if primary key and if so skip to use dictionary key approach - probably an optimisation but need to speed test diff
			var primaryKeyName = new TActiveRecord().GetPrimaryKeyName();
			if (fieldName == primaryKeyName) {
				return GetFromCache<TActiveRecord>(searchValue, tableName);
			}

			if (ClassNamesToCache.Contains(tableName)) {
				foreach (DictionaryEntry pageGlobal in Web.Cache) {
					if (pageGlobal.Value is TActiveRecord) {
						var record = (TActiveRecord)pageGlobal.Value;
						if (record[fieldName] != null) {
							//cant find the field in cached record, probably the there was a field added to the table on the database since the models were generated
							if (record[fieldName] != null && record[fieldName].ValueObject != null) {
								if (record[fieldName].ValueObject.Equals(searchValue)) {
									rec = record;
								} else if (record[fieldName].IsTextField && record[fieldName].ValueObject.ToString().ToLowerInvariant() == searchValue.ToString().ToLowerInvariant()) {
									// ignore case for text fields
									rec = record;
								}
							}
						} else {
							RemoveFromCache(record);
						}
					}
				}
				if (rec != null) {
					rec.SourceLoadedFrom = "AspNetCache";
				}
			} else {
				foreach (DictionaryEntry pageGlobal in Web.PageGlobals) {
					if (pageGlobal.Value is TActiveRecord) {
						var record = (TActiveRecord)pageGlobal.Value;
						if (record[fieldName] != null) {
							//cant find the field in cached record, probably the there was a field added to the table on the database since the models were generated
							if (record[fieldName].ValueObject != null && record[fieldName].ValueObject.Equals(searchValue)) {
								rec = record;
							}
						} else {
							RemoveFromCache(record);
						}
					}
				}
				if (rec != null) {
					rec.SourceLoadedFrom = "PageGlobalsCache";
				}
			}

			if (rec == null) return null;   // not in cache			
			return rec;
		}

		public static TActiveRecord GetFromCacheByLambda<TActiveRecord>(Predicate<TActiveRecord> expression, string tableName) where TActiveRecord : ActiveRecord {
			//return Web.Cache.Get("ActiveRecord-Person-" + id) as Person;
			if (DisableCaching) return null;
			TActiveRecord rec = null;
			List<string> strs;

			if (ClassNamesToCache.Contains(tableName)) {
				foreach (DictionaryEntry pageGlobal in Web.Cache) {
					if (pageGlobal.Value is TActiveRecord) {
						var record = (TActiveRecord)pageGlobal.Value;
						if (expression.Invoke(record)) {
							rec = record;
						}
					}
				}
				if (rec != null) {
					rec.SourceLoadedFrom = "AspNetCache";
				}
			} else {
				foreach (DictionaryEntry pageGlobal in Web.PageGlobals) {
					if (pageGlobal.Value is TActiveRecord) {
						var record = (TActiveRecord)pageGlobal.Value;
						if (expression.Invoke(record)) {
							rec = record;
						}
					}
				}
				if (rec != null) {
					rec.SourceLoadedFrom = "PageGlobalsCache";
				}
			}

			if (rec == null) return null;   // not in cache			
			return rec;
		}

		public static bool DisableCaching {
			get { return (bool)(Web.PageGlobals["ActiveRecordDisableCaching"] ?? false); }
			set { Web.PageGlobals["ActiveRecordDisableCaching"] = value; }
		}

		/// <summary>
		/// Caches this ActiveRecord object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
		public static void StoreInCache(ActiveRecord activeRecord) {
			if (DisableCaching) return;
			StoreInCache(activeRecord, activeRecord.GetTableName());
		}

		public static void StoreInCache(ActiveRecord activeRecord, string tableName) {
			//Web.Cache.Insert("ActiveRecord-Person-" + this.ID, this);
			if (DisableCaching) return;
			if (GetFromCache<ActiveRecord>(activeRecord.ID_Field.ToString(), tableName) == null) {
				string key = "ActiveRecord-" + tableName + "-" + activeRecord.ID_Field.ToString();
				//key += "-" + fieldNames;
				if (ClassNamesToCache.Contains(tableName)) {
					Web.Cache.Insert(key, activeRecord, null, Cache.NoAbsoluteExpiration, CacheDuration);
				} else {
					Web.PageGlobals.Add(key, activeRecord);
				}
				//Web.Cache.Add(key, activeRecord, null, new DateTime(2999, 1, 1), new TimeSpan(0, 10, 0), CacheItemPriority.Normal, null);
			}
		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
		public static void RemoveFromCache(ActiveRecord activeRecord) {
			if (DisableCaching) return;
			var tableName = activeRecord.GetTableName();
			string key = "ActiveRecord-" + tableName + "-" + activeRecord.ID_Field.ToString();
			//key += "-" + activeRecord.GetFieldNames().Join(","); //20120531 JN/MN removed as wrong key 
			if (ClassNamesToCache.Contains(tableName)) {
				Web.Cache.Remove(key);
			} else {
				Web.PageGlobals.Remove(key);
			}
		}

		/// <summary>
		/// Unloads all active record object from the cache. If storing items in cache for any length of time, then this should be done after doing a SQL update/delete statement.
		/// </summary>
		public static void ClearCache(string tableName) {
			// by convention we sometimes create a cached list of records called eg PageCache
			// so this will clear any item which has name of tableName+"Cache"
			var key = tableName;
			if (!key.Contains("Cache")) {
				key += "Cache";
			}

			Web.Cache.Remove(key);

			// now our main function is to clear items from active record cache
			// this could be either long term cache using Web.Cache or temporary cache using Web.PageGlobals
			string keyStart = "ActiveRecord-" + tableName + "-";
			if (ClassNamesToCache.Contains(tableName)) {
				Web.Cache.ClearKeysStartingWith(keyStart);
			} else {
				List<string> keys = new List<string>();
				// retrieve application Cache enumerator
				IDictionaryEnumerator enumerator = Web.PageGlobals.GetEnumerator();
				// copy all keys that currently exist in Cache
				while (enumerator.MoveNext()) {
					if (enumerator.Key.ToString().StartsWith(keyStart)) {
						keys.Add(enumerator.Key.ToString());
					}
				}
				// delete every key from cache
				for (int i = 0; i < keys.Count; i++) {
					Web.PageGlobals.Remove(keys[i]);
				}
			}
		}

		//public static void CacheAll<TActiveRecordList>() where TActiveRecordList: IActiveRecordList {
		//  var allPages = TActiveRecordList.LoadAll();
		//  Web.Cache.Insert("Models.Pages", allPages);
		//}

		//public static TActiveRecordList GetCache<TActiveRecordList>() where TActiveRecordList: IActiveRecordList {
		//  var allPages = Web.Cache.Get("Models.Pages") as TActiveRecordList;
		//  return allPages;
		//}


	}



}

/*
 * /// phone notes

 * var sql = new Sql()
 * sql+= "select * from administrator"
 * int adminID = Db.fetch(sql).int()
 * OR 
 * sql.conn = "trackpeng"
 * int adminID = sql.fetch.Int()
 * reader admins = sql.fetch.Reader()

Db class
Fetch property - returns global db connection
Int method

Var Db2 = db.open(cs)
Int x = db2.fetch.int()
Int x = db2.int()

Int x = db2(cs).fetch(sql).int()

Eager / lazy load
In join, look at source table, then load data structure into gen'd class instance

Select * from customer inner join company 
Create customer classes, load customer data

				When loading customer data look for companyid
				In customer class, fill company object with fields returned from query


*/


namespace BewebTest {
	[TestClass]
	public class TestActiveRecord {

		[Beweb.TestMethod]
		public static void TestFromString() {
			//FromString: ContactDate is not a valid value of type System.Nullable`1[System.DateTime]; 2013-10-10 08.30
			var f = new ActiveField<DateTime>();
			f.Type = typeof(DateTime);
			//f.FromString("2013-10-10 08.30");
			f.FromString("2013-10-10 08:30");
			Assert.AreEqual(new DateTime(2013, 10, 10, 8, 30, 0), f.Value);

		}

	}
}