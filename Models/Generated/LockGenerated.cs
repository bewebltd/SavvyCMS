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
// CLASS: Lock
// TABLE: Lock
//-----------------------------------------


	public partial class Lock : ActiveRecord {

		/// <summary>
		/// The list that this Lock is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Lock> GetContainingList() {
			return (ActiveRecordList<Lock>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Lock(): base("Lock", "LockID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Lock";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "LockID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property LockID.
		/// </summary>
		public int ID { get { return (int)fields["LockID"].ValueObject; } set { fields["LockID"].ValueObject = value; } }

		// field references
		public partial class LockFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public LockFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private LockFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public LockFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new LockFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Lock table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Lock</param>
		/// <returns>An instance of Lock containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Lock table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Lock.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Lock.LoadID(55);</example>
		/// <param name="id">Primary key of Lock</param>
		/// <returns>An instance of Lock containing the data in the record</returns>
		public static Lock LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Lock record = null;
//			record = Lock.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where LockID=", Sql.Sqlize(id));
//				record = new Lock();
//				if (!record.LoadData(sql)) return otherwise.Execute<Lock>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Lock>(id, "Lock", otherwise);
		}

		/// <summary>
		/// Loads a record from the Lock table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Lock containing the data in the record</returns>
		public static Lock Load(Sql sql) {
				return ActiveRecordLoader.Load<Lock>(sql, "Lock");
		}
		public static Lock Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Lock>(sql, "Lock", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Lock table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Lock containing the data in the record</returns>
		public static Lock Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Lock>(reader, "Lock");
		}
		public static Lock Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Lock>(reader, "Lock", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where LockID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("LockID", new ActiveField<int>() { Name = "LockID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Lock"  });

	fields.Add("TableName", new ActiveField<string>() { Name = "TableName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Lock"  });

	fields.Add("RecordID", new ActiveField<int?>() { Name = "RecordID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Lock"  });

	fields.Add("AdministratorID", new ActiveField<int?>() { Name = "AdministratorID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Lock"  });

	fields.Add("DateLocked", new ActiveField<System.DateTime?>() { Name = "DateLocked", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Lock"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Lock"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Lock"  });
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
				var rec = ActiveRecordLoader.LoadID<Lock>(id, "Lock", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Lock with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Lock or null if not in cache.</returns>
//		private static Lock GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Lock-" + id) as Lock;
//			return Web.PageGlobals["ActiveRecord-Lock-" + id] as Lock;
//		}

		/// <summary>
		/// Caches this Lock object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Lock-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Lock-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Lock-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Lock objects/records. This is the usual data structure for holding a number of Lock records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class LockList : ActiveRecordList<Lock> {

		public LockList() : base() {}
		public LockList(List<Lock> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Lock to LockList. 
		/// </summary>
		public static implicit operator LockList(List<Lock> list) {
			return new LockList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from LockList to List-of-Lock. 
		/// </summary>
		public static implicit operator List<Lock>(LockList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Lock objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Lock records.</returns>
		public static LockList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where LockID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Lock objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Lock records.</returns>
		public static LockList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static LockList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where LockID in (", ids.SqlizeNumberList(), ")");
			var result = new LockList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Lock objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Lock records.</returns>
		public static LockList Load(Sql sql) {
			var result = new LockList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Lock objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and LockID desc.)
		/// </summary>
		public static LockList LoadAll() {
			var result = new LockList();
			result.LoadRecords(null);
			return result;
		}
		public static LockList LoadAll(int itemsPerPage) {
			var result = new LockList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static LockList LoadAll(int itemsPerPage, int pageNum) {
			var result = new LockList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Lock objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static LockList LoadActive() {
			var result = new LockList();
			var sql = (new Lock()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static LockList LoadActive(int itemsPerPage) {
			var result = new LockList();
			var sql = (new Lock()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static LockList LoadActive(int itemsPerPage, int pageNum) {
			var result = new LockList();
			var sql = (new Lock()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static LockList LoadActivePlusExisting(object existingRecordID) {
			var result = new LockList();
			var sql = (new Lock()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Lock");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Lock");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Lock()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Lock.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: LockID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int LockID {
			get { return Fields.LockID.Value; }
			set { fields["LockID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByLockID(int lockIDValue) {
			return ActiveRecordLoader.LoadByField<Lock>("LockID", lockIDValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> LockID {
				get { return (ActiveField<int>)fields["LockID"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByLockID(int lockIDValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where LockID=", Sql.Sqlize(lockIDValue));
			return LockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TableName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TableName {
			get { return Fields.TableName.Value; }
			set { fields["TableName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByTableName(string tableNameValue) {
			return ActiveRecordLoader.LoadByField<Lock>("TableName", tableNameValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TableName {
				get { return (ActiveField<string>)fields["TableName"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByTableName(string tableNameValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where TableName=", Sql.Sqlize(tableNameValue));
			return LockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RecordID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? RecordID {
			get { return Fields.RecordID.Value; }
			set { fields["RecordID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByRecordID(int? recordIDValue) {
			return ActiveRecordLoader.LoadByField<Lock>("RecordID", recordIDValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> RecordID {
				get { return (ActiveField<int?>)fields["RecordID"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByRecordID(int? recordIDValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where RecordID=", Sql.Sqlize(recordIDValue));
			return LockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AdministratorID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? AdministratorID {
			get { return Fields.AdministratorID.Value; }
			set { fields["AdministratorID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByAdministratorID(int? administratorIDValue) {
			return ActiveRecordLoader.LoadByField<Lock>("AdministratorID", administratorIDValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> AdministratorID {
				get { return (ActiveField<int?>)fields["AdministratorID"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByAdministratorID(int? administratorIDValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where AdministratorID=", Sql.Sqlize(administratorIDValue));
			return LockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateLocked
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateLocked {
			get { return Fields.DateLocked.Value; }
			set { fields["DateLocked"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByDateLocked(System.DateTime? dateLockedValue) {
			return ActiveRecordLoader.LoadByField<Lock>("DateLocked", dateLockedValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateLocked {
				get { return (ActiveField<System.DateTime?>)fields["DateLocked"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByDateLocked(System.DateTime? dateLockedValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where DateLocked=", Sql.Sqlize(dateLockedValue));
			return LockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Lock>("DateAdded", dateAddedValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return LockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Lock {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Lock LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Lock>("DateModified", dateModifiedValue, "Lock", Otherwise.Null);
		}

		public partial class LockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class LockList {		
				
		[JetBrains.Annotations.NotNull]
		public static LockList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Lock".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return LockList.Load(sql);
		}		
		
	}


}
#endregion