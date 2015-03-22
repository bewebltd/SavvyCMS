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
// CLASS: SavvyAdmin
// TABLE: SavvyAdmin
//-----------------------------------------


	public partial class SavvyAdmin : ActiveRecord {

		/// <summary>
		/// The list that this SavvyAdmin is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<SavvyAdmin> GetContainingList() {
			return (ActiveRecordList<SavvyAdmin>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SavvyAdmin(): base("SavvyAdmin", "SavvyAdminID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "SavvyAdmin";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "SavvyAdminID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property SavvyAdminID.
		/// </summary>
		public int ID { get { return (int)fields["SavvyAdminID"].ValueObject; } set { fields["SavvyAdminID"].ValueObject = value; } }

		// field references
		public partial class SavvyAdminFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public SavvyAdminFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private SavvyAdminFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public SavvyAdminFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new SavvyAdminFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the SavvyAdmin table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of SavvyAdmin</param>
		/// <returns>An instance of SavvyAdmin containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the SavvyAdmin table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg SavvyAdmin.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = SavvyAdmin.LoadID(55);</example>
		/// <param name="id">Primary key of SavvyAdmin</param>
		/// <returns>An instance of SavvyAdmin containing the data in the record</returns>
		public static SavvyAdmin LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			SavvyAdmin record = null;
//			record = SavvyAdmin.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where SavvyAdminID=", Sql.Sqlize(id));
//				record = new SavvyAdmin();
//				if (!record.LoadData(sql)) return otherwise.Execute<SavvyAdmin>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<SavvyAdmin>(id, "SavvyAdmin", otherwise);
		}

		/// <summary>
		/// Loads a record from the SavvyAdmin table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of SavvyAdmin containing the data in the record</returns>
		public static SavvyAdmin Load(Sql sql) {
				return ActiveRecordLoader.Load<SavvyAdmin>(sql, "SavvyAdmin");
		}
		public static SavvyAdmin Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<SavvyAdmin>(sql, "SavvyAdmin", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the SavvyAdmin table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of SavvyAdmin containing the data in the record</returns>
		public static SavvyAdmin Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<SavvyAdmin>(reader, "SavvyAdmin");
		}
		public static SavvyAdmin Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<SavvyAdmin>(reader, "SavvyAdmin", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where SavvyAdminID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("SavvyAdminID", new ActiveField<int>() { Name = "SavvyAdminID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="SavvyAdmin"  });

	fields.Add("ClientLogoPicture", new PictureActiveField() { Name = "ClientLogoPicture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="SavvyAdmin"  });

	fields.Add("ShowSavvyLogo", new ActiveField<bool>() { Name = "ShowSavvyLogo", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="SavvyAdmin"  });

	fields.Add("ClientHelpContactName", new ActiveField<string>() { Name = "ClientHelpContactName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="SavvyAdmin"  });

	fields.Add("ClientHelpContactEmail", new ActiveField<string>() { Name = "ClientHelpContactEmail", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=80, TableName="SavvyAdmin"  });

	fields.Add("HeaderColor", new ActiveField<string>() { Name = "HeaderColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("TextColor", new ActiveField<string>() { Name = "TextColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("LineColor", new ActiveField<string>() { Name = "LineColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("CellColor", new ActiveField<string>() { Name = "CellColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("CellAltColor", new ActiveField<string>() { Name = "CellAltColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("HeaderBackgroundColor", new ActiveField<string>() { Name = "HeaderBackgroundColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("HeaderBackgroundImage", new PictureActiveField() { Name = "HeaderBackgroundImage", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="SavvyAdmin"  });

	fields.Add("LinkTextVisitedColor", new ActiveField<string>() { Name = "LinkTextVisitedColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("LinkTextColor", new ActiveField<string>() { Name = "LinkTextColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("SectionLineColor", new ActiveField<string>() { Name = "SectionLineColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("LabelColor", new ActiveField<string>() { Name = "LabelColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("ListTitlesBackgroundColor", new ActiveField<string>() { Name = "ListTitlesBackgroundColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("FilterBackgroundColor", new ActiveField<string>() { Name = "FilterBackgroundColor", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=15, TableName="SavvyAdmin"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="SavvyAdmin"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="SavvyAdmin"  });
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
				var rec = ActiveRecordLoader.LoadID<SavvyAdmin>(id, "SavvyAdmin", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the SavvyAdmin with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct SavvyAdmin or null if not in cache.</returns>
//		private static SavvyAdmin GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-SavvyAdmin-" + id) as SavvyAdmin;
//			return Web.PageGlobals["ActiveRecord-SavvyAdmin-" + id] as SavvyAdmin;
//		}

		/// <summary>
		/// Caches this SavvyAdmin object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-SavvyAdmin-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-SavvyAdmin-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-SavvyAdmin-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of SavvyAdmin objects/records. This is the usual data structure for holding a number of SavvyAdmin records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class SavvyAdminList : ActiveRecordList<SavvyAdmin> {

		public SavvyAdminList() : base() {}
		public SavvyAdminList(List<SavvyAdmin> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-SavvyAdmin to SavvyAdminList. 
		/// </summary>
		public static implicit operator SavvyAdminList(List<SavvyAdmin> list) {
			return new SavvyAdminList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from SavvyAdminList to List-of-SavvyAdmin. 
		/// </summary>
		public static implicit operator List<SavvyAdmin>(SavvyAdminList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of SavvyAdmin objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of SavvyAdmin records.</returns>
		public static SavvyAdminList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where SavvyAdminID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of SavvyAdmin objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of SavvyAdmin records.</returns>
		public static SavvyAdminList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static SavvyAdminList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where SavvyAdminID in (", ids.SqlizeNumberList(), ")");
			var result = new SavvyAdminList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of SavvyAdmin objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of SavvyAdmin records.</returns>
		public static SavvyAdminList Load(Sql sql) {
			var result = new SavvyAdminList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all SavvyAdmin objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and SavvyAdminID desc.)
		/// </summary>
		public static SavvyAdminList LoadAll() {
			var result = new SavvyAdminList();
			result.LoadRecords(null);
			return result;
		}
		public static SavvyAdminList LoadAll(int itemsPerPage) {
			var result = new SavvyAdminList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static SavvyAdminList LoadAll(int itemsPerPage, int pageNum) {
			var result = new SavvyAdminList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" SavvyAdmin objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static SavvyAdminList LoadActive() {
			var result = new SavvyAdminList();
			var sql = (new SavvyAdmin()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static SavvyAdminList LoadActive(int itemsPerPage) {
			var result = new SavvyAdminList();
			var sql = (new SavvyAdmin()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static SavvyAdminList LoadActive(int itemsPerPage, int pageNum) {
			var result = new SavvyAdminList();
			var sql = (new SavvyAdmin()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static SavvyAdminList LoadActivePlusExisting(object existingRecordID) {
			var result = new SavvyAdminList();
			var sql = (new SavvyAdmin()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM SavvyAdmin");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM SavvyAdmin");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new SavvyAdmin()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = SavvyAdmin.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: SavvyAdminID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public int SavvyAdminID {
			get { return Fields.SavvyAdminID.Value; }
			set { fields["SavvyAdminID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadBySavvyAdminID(int savvyAdminIDValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("SavvyAdminID", savvyAdminIDValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> SavvyAdminID {
				get { return (ActiveField<int>)fields["SavvyAdminID"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadBySavvyAdminID(int savvyAdminIDValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where SavvyAdminID=", Sql.Sqlize(savvyAdminIDValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ClientLogoPicture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ClientLogoPicture {
			get { return Fields.ClientLogoPicture.Value; }
			set { fields["ClientLogoPicture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByClientLogoPicture(string clientLogoPictureValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("ClientLogoPicture", clientLogoPictureValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField ClientLogoPicture {
				get { return (PictureActiveField)fields["ClientLogoPicture"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByClientLogoPicture(string clientLogoPictureValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where ClientLogoPicture=", Sql.Sqlize(clientLogoPictureValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowSavvyLogo
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowSavvyLogo {
			get { return Fields.ShowSavvyLogo.Value; }
			set { fields["ShowSavvyLogo"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByShowSavvyLogo(bool showSavvyLogoValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("ShowSavvyLogo", showSavvyLogoValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowSavvyLogo {
				get { return (ActiveField<bool>)fields["ShowSavvyLogo"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByShowSavvyLogo(bool showSavvyLogoValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where ShowSavvyLogo=", Sql.Sqlize(showSavvyLogoValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ClientHelpContactName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ClientHelpContactName {
			get { return Fields.ClientHelpContactName.Value; }
			set { fields["ClientHelpContactName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByClientHelpContactName(string clientHelpContactNameValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("ClientHelpContactName", clientHelpContactNameValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ClientHelpContactName {
				get { return (ActiveField<string>)fields["ClientHelpContactName"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByClientHelpContactName(string clientHelpContactNameValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where ClientHelpContactName=", Sql.Sqlize(clientHelpContactNameValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ClientHelpContactEmail
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ClientHelpContactEmail {
			get { return Fields.ClientHelpContactEmail.Value; }
			set { fields["ClientHelpContactEmail"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByClientHelpContactEmail(string clientHelpContactEmailValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("ClientHelpContactEmail", clientHelpContactEmailValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ClientHelpContactEmail {
				get { return (ActiveField<string>)fields["ClientHelpContactEmail"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByClientHelpContactEmail(string clientHelpContactEmailValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where ClientHelpContactEmail=", Sql.Sqlize(clientHelpContactEmailValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: HeaderColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string HeaderColor {
			get { return Fields.HeaderColor.Value; }
			set { fields["HeaderColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByHeaderColor(string headerColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("HeaderColor", headerColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> HeaderColor {
				get { return (ActiveField<string>)fields["HeaderColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByHeaderColor(string headerColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where HeaderColor=", Sql.Sqlize(headerColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TextColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TextColor {
			get { return Fields.TextColor.Value; }
			set { fields["TextColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByTextColor(string textColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("TextColor", textColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TextColor {
				get { return (ActiveField<string>)fields["TextColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByTextColor(string textColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where TextColor=", Sql.Sqlize(textColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LineColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LineColor {
			get { return Fields.LineColor.Value; }
			set { fields["LineColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByLineColor(string lineColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("LineColor", lineColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LineColor {
				get { return (ActiveField<string>)fields["LineColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByLineColor(string lineColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where LineColor=", Sql.Sqlize(lineColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CellColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CellColor {
			get { return Fields.CellColor.Value; }
			set { fields["CellColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByCellColor(string cellColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("CellColor", cellColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CellColor {
				get { return (ActiveField<string>)fields["CellColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByCellColor(string cellColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where CellColor=", Sql.Sqlize(cellColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CellAltColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CellAltColor {
			get { return Fields.CellAltColor.Value; }
			set { fields["CellAltColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByCellAltColor(string cellAltColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("CellAltColor", cellAltColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CellAltColor {
				get { return (ActiveField<string>)fields["CellAltColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByCellAltColor(string cellAltColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where CellAltColor=", Sql.Sqlize(cellAltColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: HeaderBackgroundColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string HeaderBackgroundColor {
			get { return Fields.HeaderBackgroundColor.Value; }
			set { fields["HeaderBackgroundColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByHeaderBackgroundColor(string headerBackgroundColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("HeaderBackgroundColor", headerBackgroundColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> HeaderBackgroundColor {
				get { return (ActiveField<string>)fields["HeaderBackgroundColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByHeaderBackgroundColor(string headerBackgroundColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where HeaderBackgroundColor=", Sql.Sqlize(headerBackgroundColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: HeaderBackgroundImage
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string HeaderBackgroundImage {
			get { return Fields.HeaderBackgroundImage.Value; }
			set { fields["HeaderBackgroundImage"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByHeaderBackgroundImage(string headerBackgroundImageValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("HeaderBackgroundImage", headerBackgroundImageValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField HeaderBackgroundImage {
				get { return (PictureActiveField)fields["HeaderBackgroundImage"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByHeaderBackgroundImage(string headerBackgroundImageValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where HeaderBackgroundImage=", Sql.Sqlize(headerBackgroundImageValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkTextVisitedColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkTextVisitedColor {
			get { return Fields.LinkTextVisitedColor.Value; }
			set { fields["LinkTextVisitedColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByLinkTextVisitedColor(string linkTextVisitedColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("LinkTextVisitedColor", linkTextVisitedColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkTextVisitedColor {
				get { return (ActiveField<string>)fields["LinkTextVisitedColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByLinkTextVisitedColor(string linkTextVisitedColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where LinkTextVisitedColor=", Sql.Sqlize(linkTextVisitedColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkTextColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkTextColor {
			get { return Fields.LinkTextColor.Value; }
			set { fields["LinkTextColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByLinkTextColor(string linkTextColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("LinkTextColor", linkTextColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkTextColor {
				get { return (ActiveField<string>)fields["LinkTextColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByLinkTextColor(string linkTextColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where LinkTextColor=", Sql.Sqlize(linkTextColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SectionLineColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SectionLineColor {
			get { return Fields.SectionLineColor.Value; }
			set { fields["SectionLineColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadBySectionLineColor(string sectionLineColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("SectionLineColor", sectionLineColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SectionLineColor {
				get { return (ActiveField<string>)fields["SectionLineColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadBySectionLineColor(string sectionLineColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where SectionLineColor=", Sql.Sqlize(sectionLineColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LabelColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LabelColor {
			get { return Fields.LabelColor.Value; }
			set { fields["LabelColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByLabelColor(string labelColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("LabelColor", labelColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LabelColor {
				get { return (ActiveField<string>)fields["LabelColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByLabelColor(string labelColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where LabelColor=", Sql.Sqlize(labelColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ListTitlesBackgroundColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ListTitlesBackgroundColor {
			get { return Fields.ListTitlesBackgroundColor.Value; }
			set { fields["ListTitlesBackgroundColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByListTitlesBackgroundColor(string listTitlesBackgroundColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("ListTitlesBackgroundColor", listTitlesBackgroundColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ListTitlesBackgroundColor {
				get { return (ActiveField<string>)fields["ListTitlesBackgroundColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByListTitlesBackgroundColor(string listTitlesBackgroundColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where ListTitlesBackgroundColor=", Sql.Sqlize(listTitlesBackgroundColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FilterBackgroundColor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FilterBackgroundColor {
			get { return Fields.FilterBackgroundColor.Value; }
			set { fields["FilterBackgroundColor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByFilterBackgroundColor(string filterBackgroundColorValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("FilterBackgroundColor", filterBackgroundColorValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FilterBackgroundColor {
				get { return (ActiveField<string>)fields["FilterBackgroundColor"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByFilterBackgroundColor(string filterBackgroundColorValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where FilterBackgroundColor=", Sql.Sqlize(filterBackgroundColorValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("DateAdded", dateAddedValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class SavvyAdmin {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static SavvyAdmin LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<SavvyAdmin>("DateModified", dateModifiedValue, "SavvyAdmin", Otherwise.Null);
		}

		public partial class SavvyAdminFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class SavvyAdminList {		
				
		[JetBrains.Annotations.NotNull]
		public static SavvyAdminList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","SavvyAdmin".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return SavvyAdminList.Load(sql);
		}		
		
	}


}
#endregion