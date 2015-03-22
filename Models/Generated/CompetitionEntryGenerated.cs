//-------------------------------------------------
// Generated Beweb Savvy ActiveRecord Model Classes
// This code was written by a tool.
//-------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Beweb;
using Models;

#region "ActiveRecord generated code"

namespace Models {
//-----------------------------------------
// CLASS: CompetitionEntry
// TABLE: CompetitionEntry
//-----------------------------------------


	public partial class CompetitionEntry : ActiveRecord {

		/// <summary>
		/// The list that this CompetitionEntry is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<CompetitionEntry> GetContainingList() {
			return (ActiveRecordList<CompetitionEntry>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public CompetitionEntry(): base("CompetitionEntry", "CompetitionEntryID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "CompetitionEntry";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "CompetitionEntryID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property CompetitionEntryID.
		/// </summary>
		public int ID { get { return (int)fields["CompetitionEntryID"].ValueObject; } set { fields["CompetitionEntryID"].ValueObject = value; } }

		// field references
		public partial class CompetitionEntryFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public CompetitionEntryFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private CompetitionEntryFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public CompetitionEntryFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new CompetitionEntryFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the CompetitionEntry table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of CompetitionEntry</param>
		/// <returns>An instance of CompetitionEntry containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the CompetitionEntry table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg CompetitionEntry.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = CompetitionEntry.LoadID(55);</example>
		/// <param name="id">Primary key of CompetitionEntry</param>
		/// <returns>An instance of CompetitionEntry containing the data in the record</returns>
		public static CompetitionEntry LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			CompetitionEntry record = null;
//			record = CompetitionEntry.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where CompetitionEntryID=", Sql.Sqlize(id));
//				record = new CompetitionEntry();
//				if (!record.LoadData(sql)) return otherwise.Execute<CompetitionEntry>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<CompetitionEntry>(id, "CompetitionEntry", otherwise);
		}

		/// <summary>
		/// Loads a record from the CompetitionEntry table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of CompetitionEntry containing the data in the record</returns>
		public static CompetitionEntry Load(Sql sql) {
				return ActiveRecordLoader.Load<CompetitionEntry>(sql, "CompetitionEntry");
		}
		public static CompetitionEntry Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<CompetitionEntry>(sql, "CompetitionEntry", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the CompetitionEntry table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of CompetitionEntry containing the data in the record</returns>
		public static CompetitionEntry Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<CompetitionEntry>(reader, "CompetitionEntry");
		}
		public static CompetitionEntry Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<CompetitionEntry>(reader, "CompetitionEntry", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where CompetitionEntryID=", Sql.Sqlize(this.ID));
			this.LoadData(sql);
			this.StoreInCache();
		}

		/// <summary>
		/// moved to models folder
		/// </summary>
		//public void InitDefaults() {
		//	xxxdefaultsxxx;
		//}

		public void SetupFieldMetaData() {
			
	fields.Add("CompetitionEntryID", new ActiveField<int>() { Name = "CompetitionEntryID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="CompetitionEntry"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="CompetitionEntry"  });

	fields.Add("UserIPAddress", new ActiveField<string>() { Name = "UserIPAddress", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="CompetitionEntry"  });

	fields.Add("FirstName", new ActiveField<string>() { Name = "FirstName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="CompetitionEntry"  });

	fields.Add("LastName", new ActiveField<string>() { Name = "LastName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="CompetitionEntry"  });

	fields.Add("Email", new ActiveField<string>() { Name = "Email", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="CompetitionEntry"  });

	fields.Add("Country", new ActiveField<string>() { Name = "Country", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="CompetitionEntry"  });

	fields.Add("CompetitionID", new ActiveField<int?>() { Name = "CompetitionID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="CompetitionEntry" , GetForeignRecord = () => this.Competition, ForeignClassName = typeof(Models.Competition), ForeignTableName = "Competition", ForeignTableFieldName = "CompetitionID" });

	fields.Add("CompetitionTitle", new ActiveField<string>() { Name = "CompetitionTitle", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="CompetitionEntry"  });

	fields.Add("AgreedTerms", new ActiveField<bool>() { Name = "AgreedTerms", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="CompetitionEntry"  });

	fields.Add("OptInFutureEmails", new ActiveField<bool>() { Name = "OptInFutureEmails", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="CompetitionEntry"  });

	fields.Add("Phone", new ActiveField<string>() { Name = "Phone", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="CompetitionEntry"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="CompetitionEntry"  });
;
		}

		/// <summary>
		/// This is a quick way of loading a record by ID and returning its title/name. 
		/// You can pass it a string, int or any type that the primary key is.
		/// You can override GetNameField() if you want to change which field is defined as the title/name, or override GetName() to completely customise what is returned as the name or title.
		/// </summary>
		/// <returns>The name/title of the item with the given ID</returns>
		public static string FetchName(object id) {
			if (id!=null) {
				var rec = ActiveRecordLoader.LoadID<CompetitionEntry>(id, "CompetitionEntry", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the CompetitionEntry with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct CompetitionEntry or null if not in cache.</returns>
//		private static CompetitionEntry GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-CompetitionEntry-" + id) as CompetitionEntry;
//			return Web.PageGlobals["ActiveRecord-CompetitionEntry-" + id] as CompetitionEntry;
//		}

		/// <summary>
		/// Caches this CompetitionEntry object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-CompetitionEntry-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-CompetitionEntry-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-CompetitionEntry-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of CompetitionEntry objects/records. This is the usual data structure for holding a number of CompetitionEntry records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class CompetitionEntryList : ActiveRecordList<CompetitionEntry> {

		public CompetitionEntryList() : base() {}
		public CompetitionEntryList(List<CompetitionEntry> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-CompetitionEntry to CompetitionEntryList. 
		/// </summary>
		public static implicit operator CompetitionEntryList(List<CompetitionEntry> list) {
			return new CompetitionEntryList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from CompetitionEntryList to List-of-CompetitionEntry. 
		/// </summary>
		public static implicit operator List<CompetitionEntry>(CompetitionEntryList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of CompetitionEntry objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of CompetitionEntry records.</returns>
		public static CompetitionEntryList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where CompetitionEntryID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of CompetitionEntry objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of CompetitionEntry records.</returns>
		public static CompetitionEntryList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static CompetitionEntryList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where CompetitionEntryID in (", ids.SqlizeNumberList(), ")");
			var result = new CompetitionEntryList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of CompetitionEntry objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of CompetitionEntry records.</returns>
		public static CompetitionEntryList Load(Sql sql) {
			var result = new CompetitionEntryList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all CompetitionEntry objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and CompetitionEntryID desc.)
		/// </summary>
		public static CompetitionEntryList LoadAll() {
			var result = new CompetitionEntryList();
			result.LoadRecords(null);
			return result;
		}
		public static CompetitionEntryList LoadAll(int itemsPerPage) {
			var result = new CompetitionEntryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CompetitionEntryList LoadAll(int itemsPerPage, int pageNum) {
			var result = new CompetitionEntryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" CompetitionEntry objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static CompetitionEntryList LoadActive() {
			var result = new CompetitionEntryList();
			var sql = (new CompetitionEntry()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static CompetitionEntryList LoadActive(int itemsPerPage) {
			var result = new CompetitionEntryList();
			var sql = (new CompetitionEntry()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CompetitionEntryList LoadActive(int itemsPerPage, int pageNum) {
			var result = new CompetitionEntryList();
			var sql = (new CompetitionEntry()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static CompetitionEntryList LoadActivePlusExisting(object existingRecordID) {
			var result = new CompetitionEntryList();
			var sql = (new CompetitionEntry()).GetSqlWhereActivePlusExisting(existingRecordID);
			result.LoadRecords(sql);
			return result;
		}

		/// <summary>
		/// Loads all records into the current instance from database given sql statement.
		/// If there are already objects in the list, the new objects will be added at the end.
		/// If found in the cache, records will be loaded from the cache.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/* //call base class 20110615
		public new void LoadRecords(Sql sql) {
			if (sql==null) {
				sql = new Sql("SELECT * FROM CompetitionEntry");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM CompetitionEntry");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new CompetitionEntry()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = CompetitionEntry.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: CompetitionEntryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public int CompetitionEntryID {
			get { return Fields.CompetitionEntryID.Value; }
			set { fields["CompetitionEntryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByCompetitionEntryID(int competitionEntryIDValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("CompetitionEntryID", competitionEntryIDValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> CompetitionEntryID {
				get { return (ActiveField<int>)fields["CompetitionEntryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByCompetitionEntryID(int competitionEntryIDValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where CompetitionEntryID=", Sql.Sqlize(competitionEntryIDValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("DateAdded", dateAddedValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UserIPAddress
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UserIPAddress {
			get { return Fields.UserIPAddress.Value; }
			set { fields["UserIPAddress"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByUserIPAddress(string userIPAddressValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("UserIPAddress", userIPAddressValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> UserIPAddress {
				get { return (ActiveField<string>)fields["UserIPAddress"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByUserIPAddress(string userIPAddressValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where UserIPAddress=", Sql.Sqlize(userIPAddressValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FirstName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FirstName {
			get { return Fields.FirstName.Value; }
			set { fields["FirstName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByFirstName(string firstNameValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("FirstName", firstNameValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FirstName {
				get { return (ActiveField<string>)fields["FirstName"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByFirstName(string firstNameValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where FirstName=", Sql.Sqlize(firstNameValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LastName {
			get { return Fields.LastName.Value; }
			set { fields["LastName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByLastName(string lastNameValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("LastName", lastNameValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LastName {
				get { return (ActiveField<string>)fields["LastName"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByLastName(string lastNameValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where LastName=", Sql.Sqlize(lastNameValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Email
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Email {
			get { return Fields.Email.Value; }
			set { fields["Email"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByEmail(string emailValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("Email", emailValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Email {
				get { return (ActiveField<string>)fields["Email"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByEmail(string emailValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where Email=", Sql.Sqlize(emailValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Country
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Country {
			get { return Fields.Country.Value; }
			set { fields["Country"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByCountry(string countryValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("Country", countryValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Country {
				get { return (ActiveField<string>)fields["Country"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByCountry(string countryValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where Country=", Sql.Sqlize(countryValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CompetitionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? CompetitionID {
			get { return Fields.CompetitionID.Value; }
			set { fields["CompetitionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByCompetitionID(int? competitionIDValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("CompetitionID", competitionIDValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> CompetitionID {
				get { return (ActiveField<int?>)fields["CompetitionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByCompetitionID(int? competitionIDValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where CompetitionID=", Sql.Sqlize(competitionIDValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Competition
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class CompetitionEntry {
		[NonSerialized]		
		private Competition _Competition;

		[JetBrains.Annotations.CanBeNull]
		public Competition Competition
		{
			get
			{
				 // lazy load
				if (this._Competition == null && this.CompetitionID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Competition") && container.PrefetchCounter["Competition"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Competition>("CompetitionID",container.Select(r=>r.CompetitionID).ToList(),"Competition",Otherwise.Null);
					}
					this._Competition = Models.Competition.LoadByCompetitionID(CompetitionID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Competition")) {
							container.PrefetchCounter["Competition"] = 0;
						}
						container.PrefetchCounter["Competition"]++;
					}
				}
				return this._Competition;
			}
			set
			{
				this._Competition = value;
			}
		}
	}

	public partial class CompetitionEntryList {
		internal int numFetchesOfCompetition = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Competition {
		[NonSerialized]		
		private CompetitionEntryList _CompetitionEntries;
		
		[JetBrains.Annotations.NotNull]
		public CompetitionEntryList CompetitionEntries
		{
			get
			{
				// lazy load
				if (this._CompetitionEntries == null) {
					this._CompetitionEntries = Models.CompetitionEntryList.LoadByCompetitionID(this.ID);
					this._CompetitionEntries.SetParentBindField(this, "CompetitionID");
				}
				return this._CompetitionEntries;
			}
			set
			{
				this._CompetitionEntries = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: CompetitionTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CompetitionTitle {
			get { return Fields.CompetitionTitle.Value; }
			set { fields["CompetitionTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByCompetitionTitle(string competitionTitleValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("CompetitionTitle", competitionTitleValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CompetitionTitle {
				get { return (ActiveField<string>)fields["CompetitionTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByCompetitionTitle(string competitionTitleValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where CompetitionTitle=", Sql.Sqlize(competitionTitleValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AgreedTerms
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool AgreedTerms {
			get { return Fields.AgreedTerms.Value; }
			set { fields["AgreedTerms"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByAgreedTerms(bool agreedTermsValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("AgreedTerms", agreedTermsValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> AgreedTerms {
				get { return (ActiveField<bool>)fields["AgreedTerms"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByAgreedTerms(bool agreedTermsValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where AgreedTerms=", Sql.Sqlize(agreedTermsValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: OptInFutureEmails
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool OptInFutureEmails {
			get { return Fields.OptInFutureEmails.Value; }
			set { fields["OptInFutureEmails"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByOptInFutureEmails(bool optInFutureEmailsValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("OptInFutureEmails", optInFutureEmailsValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> OptInFutureEmails {
				get { return (ActiveField<bool>)fields["OptInFutureEmails"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByOptInFutureEmails(bool optInFutureEmailsValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where OptInFutureEmails=", Sql.Sqlize(optInFutureEmailsValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Phone
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Phone {
			get { return Fields.Phone.Value; }
			set { fields["Phone"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByPhone(string phoneValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("Phone", phoneValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Phone {
				get { return (ActiveField<string>)fields["Phone"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByPhone(string phoneValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where Phone=", Sql.Sqlize(phoneValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompetitionEntry {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompetitionEntry LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<CompetitionEntry>("DateModified", dateModifiedValue, "CompetitionEntry", Otherwise.Null);
		}

		public partial class CompetitionEntryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionEntryList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionEntryList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","CompetitionEntry".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return CompetitionEntryList.Load(sql);
		}		
		
	}


}
#endregion