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
// CLASS: UrlRedirect
// TABLE: UrlRedirect
//-----------------------------------------


	public partial class UrlRedirect : ActiveRecord {

		/// <summary>
		/// The list that this UrlRedirect is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<UrlRedirect> GetContainingList() {
			return (ActiveRecordList<UrlRedirect>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public UrlRedirect(): base("UrlRedirect", "UrlRedirectID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "UrlRedirect";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "UrlRedirectID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property UrlRedirectID.
		/// </summary>
		public int ID { get { return (int)fields["UrlRedirectID"].ValueObject; } set { fields["UrlRedirectID"].ValueObject = value; } }

		// field references
		public partial class UrlRedirectFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public UrlRedirectFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private UrlRedirectFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public UrlRedirectFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new UrlRedirectFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the UrlRedirect table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of UrlRedirect</param>
		/// <returns>An instance of UrlRedirect containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the UrlRedirect table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg UrlRedirect.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = UrlRedirect.LoadID(55);</example>
		/// <param name="id">Primary key of UrlRedirect</param>
		/// <returns>An instance of UrlRedirect containing the data in the record</returns>
		public static UrlRedirect LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			UrlRedirect record = null;
//			record = UrlRedirect.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where UrlRedirectID=", Sql.Sqlize(id));
//				record = new UrlRedirect();
//				if (!record.LoadData(sql)) return otherwise.Execute<UrlRedirect>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<UrlRedirect>(id, "UrlRedirect", otherwise);
		}

		/// <summary>
		/// Loads a record from the UrlRedirect table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of UrlRedirect containing the data in the record</returns>
		public static UrlRedirect Load(Sql sql) {
				return ActiveRecordLoader.Load<UrlRedirect>(sql, "UrlRedirect");
		}
		public static UrlRedirect Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<UrlRedirect>(sql, "UrlRedirect", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the UrlRedirect table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of UrlRedirect containing the data in the record</returns>
		public static UrlRedirect Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<UrlRedirect>(reader, "UrlRedirect");
		}
		public static UrlRedirect Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<UrlRedirect>(reader, "UrlRedirect", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where UrlRedirectID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("UrlRedirectID", new ActiveField<int>() { Name = "UrlRedirectID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="UrlRedirect"  });

	fields.Add("RedirectFromUrl", new ActiveField<string>() { Name = "RedirectFromUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="UrlRedirect"  });

	fields.Add("RedirectToUrl", new ActiveField<string>() { Name = "RedirectToUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="UrlRedirect"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="UrlRedirect"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="UrlRedirect"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="UrlRedirect"  });
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
				var rec = ActiveRecordLoader.LoadID<UrlRedirect>(id, "UrlRedirect", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the UrlRedirect with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct UrlRedirect or null if not in cache.</returns>
//		private static UrlRedirect GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-UrlRedirect-" + id) as UrlRedirect;
//			return Web.PageGlobals["ActiveRecord-UrlRedirect-" + id] as UrlRedirect;
//		}

		/// <summary>
		/// Caches this UrlRedirect object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-UrlRedirect-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-UrlRedirect-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-UrlRedirect-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of UrlRedirect objects/records. This is the usual data structure for holding a number of UrlRedirect records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class UrlRedirectList : ActiveRecordList<UrlRedirect> {

		public UrlRedirectList() : base() {}
		public UrlRedirectList(List<UrlRedirect> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-UrlRedirect to UrlRedirectList. 
		/// </summary>
		public static implicit operator UrlRedirectList(List<UrlRedirect> list) {
			return new UrlRedirectList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from UrlRedirectList to List-of-UrlRedirect. 
		/// </summary>
		public static implicit operator List<UrlRedirect>(UrlRedirectList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of UrlRedirect objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of UrlRedirect records.</returns>
		public static UrlRedirectList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where UrlRedirectID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of UrlRedirect objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of UrlRedirect records.</returns>
		public static UrlRedirectList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static UrlRedirectList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where UrlRedirectID in (", ids.SqlizeNumberList(), ")");
			var result = new UrlRedirectList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of UrlRedirect objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of UrlRedirect records.</returns>
		public static UrlRedirectList Load(Sql sql) {
			var result = new UrlRedirectList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all UrlRedirect objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and UrlRedirectID desc.)
		/// </summary>
		public static UrlRedirectList LoadAll() {
			var result = new UrlRedirectList();
			result.LoadRecords(null);
			return result;
		}
		public static UrlRedirectList LoadAll(int itemsPerPage) {
			var result = new UrlRedirectList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static UrlRedirectList LoadAll(int itemsPerPage, int pageNum) {
			var result = new UrlRedirectList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" UrlRedirect objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static UrlRedirectList LoadActive() {
			var result = new UrlRedirectList();
			var sql = (new UrlRedirect()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static UrlRedirectList LoadActive(int itemsPerPage) {
			var result = new UrlRedirectList();
			var sql = (new UrlRedirect()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static UrlRedirectList LoadActive(int itemsPerPage, int pageNum) {
			var result = new UrlRedirectList();
			var sql = (new UrlRedirect()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static UrlRedirectList LoadActivePlusExisting(object existingRecordID) {
			var result = new UrlRedirectList();
			var sql = (new UrlRedirect()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM UrlRedirect");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM UrlRedirect");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new UrlRedirect()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = UrlRedirect.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: UrlRedirectID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class UrlRedirect {		
				
		[JetBrains.Annotations.CanBeNull]
		public int UrlRedirectID {
			get { return Fields.UrlRedirectID.Value; }
			set { fields["UrlRedirectID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadByUrlRedirectID(int urlRedirectIDValue) {
			return ActiveRecordLoader.LoadByField<UrlRedirect>("UrlRedirectID", urlRedirectIDValue, "UrlRedirect", Otherwise.Null);
		}

		public partial class UrlRedirectFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> UrlRedirectID {
				get { return (ActiveField<int>)fields["UrlRedirectID"]; }
			}
		}

	}
	
	// define list class 
	public partial class UrlRedirectList {		
				
		[JetBrains.Annotations.NotNull]
		public static UrlRedirectList LoadByUrlRedirectID(int urlRedirectIDValue) {
			var sql = new Sql("select * from ","UrlRedirect".SqlizeName()," where UrlRedirectID=", Sql.Sqlize(urlRedirectIDValue));
			return UrlRedirectList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RedirectFromUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class UrlRedirect {		
				
		[JetBrains.Annotations.CanBeNull]
		public string RedirectFromUrl {
			get { return Fields.RedirectFromUrl.Value; }
			set { fields["RedirectFromUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadByRedirectFromUrl(string redirectFromUrlValue) {
			return ActiveRecordLoader.LoadByField<UrlRedirect>("RedirectFromUrl", redirectFromUrlValue, "UrlRedirect", Otherwise.Null);
		}

		public partial class UrlRedirectFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> RedirectFromUrl {
				get { return (ActiveField<string>)fields["RedirectFromUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class UrlRedirectList {		
				
		[JetBrains.Annotations.NotNull]
		public static UrlRedirectList LoadByRedirectFromUrl(string redirectFromUrlValue) {
			var sql = new Sql("select * from ","UrlRedirect".SqlizeName()," where RedirectFromUrl=", Sql.Sqlize(redirectFromUrlValue));
			return UrlRedirectList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RedirectToUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class UrlRedirect {		
				
		[JetBrains.Annotations.CanBeNull]
		public string RedirectToUrl {
			get { return Fields.RedirectToUrl.Value; }
			set { fields["RedirectToUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadByRedirectToUrl(string redirectToUrlValue) {
			return ActiveRecordLoader.LoadByField<UrlRedirect>("RedirectToUrl", redirectToUrlValue, "UrlRedirect", Otherwise.Null);
		}

		public partial class UrlRedirectFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> RedirectToUrl {
				get { return (ActiveField<string>)fields["RedirectToUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class UrlRedirectList {		
				
		[JetBrains.Annotations.NotNull]
		public static UrlRedirectList LoadByRedirectToUrl(string redirectToUrlValue) {
			var sql = new Sql("select * from ","UrlRedirect".SqlizeName()," where RedirectToUrl=", Sql.Sqlize(redirectToUrlValue));
			return UrlRedirectList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class UrlRedirect {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<UrlRedirect>("IsActive", isActiveValue, "UrlRedirect", Otherwise.Null);
		}

		public partial class UrlRedirectFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class UrlRedirectList {		
				
		[JetBrains.Annotations.NotNull]
		public static UrlRedirectList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","UrlRedirect".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return UrlRedirectList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class UrlRedirect {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<UrlRedirect>("DateAdded", dateAddedValue, "UrlRedirect", Otherwise.Null);
		}

		public partial class UrlRedirectFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class UrlRedirectList {		
				
		[JetBrains.Annotations.NotNull]
		public static UrlRedirectList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","UrlRedirect".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return UrlRedirectList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class UrlRedirect {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static UrlRedirect LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<UrlRedirect>("DateModified", dateModifiedValue, "UrlRedirect", Otherwise.Null);
		}

		public partial class UrlRedirectFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class UrlRedirectList {		
				
		[JetBrains.Annotations.NotNull]
		public static UrlRedirectList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","UrlRedirect".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return UrlRedirectList.Load(sql);
		}		
		
	}


}
#endregion