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
// CLASS: ModificationLog
// TABLE: ModificationLog
//-----------------------------------------


	public partial class ModificationLog : ActiveRecord {

		/// <summary>
		/// The list that this ModificationLog is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ModificationLog> GetContainingList() {
			return (ActiveRecordList<ModificationLog>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ModificationLog(): base("ModificationLog", "ModificationLogID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ModificationLog";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ModificationLogID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ModificationLogID.
		/// </summary>
		public int ID { get { return (int)fields["ModificationLogID"].ValueObject; } set { fields["ModificationLogID"].ValueObject = value; } }

		// field references
		public partial class ModificationLogFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ModificationLogFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ModificationLogFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ModificationLogFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ModificationLogFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ModificationLog table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ModificationLog</param>
		/// <returns>An instance of ModificationLog containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ModificationLog table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ModificationLog.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ModificationLog.LoadID(55);</example>
		/// <param name="id">Primary key of ModificationLog</param>
		/// <returns>An instance of ModificationLog containing the data in the record</returns>
		public static ModificationLog LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ModificationLog record = null;
//			record = ModificationLog.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ModificationLogID=", Sql.Sqlize(id));
//				record = new ModificationLog();
//				if (!record.LoadData(sql)) return otherwise.Execute<ModificationLog>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ModificationLog>(id, "ModificationLog", otherwise);
		}

		/// <summary>
		/// Loads a record from the ModificationLog table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ModificationLog containing the data in the record</returns>
		public static ModificationLog Load(Sql sql) {
				return ActiveRecordLoader.Load<ModificationLog>(sql, "ModificationLog");
		}
		public static ModificationLog Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ModificationLog>(sql, "ModificationLog", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ModificationLog table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ModificationLog containing the data in the record</returns>
		public static ModificationLog Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ModificationLog>(reader, "ModificationLog");
		}
		public static ModificationLog Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ModificationLog>(reader, "ModificationLog", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ModificationLogID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ModificationLogID", new ActiveField<int>() { Name = "ModificationLogID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ModificationLog"  });

	fields.Add("UpdateDate", new ActiveField<System.DateTime?>() { Name = "UpdateDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ModificationLog"  });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ModificationLog" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("TableName", new ActiveField<string>() { Name = "TableName", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=80, TableName="ModificationLog"  });

	fields.Add("RecordID", new ActiveField<int?>() { Name = "RecordID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ModificationLog"  });

	fields.Add("ActionType", new ActiveField<string>() { Name = "ActionType", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=10, TableName="ModificationLog"  });

	fields.Add("UserName", new ActiveField<string>() { Name = "UserName", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=80, TableName="ModificationLog"  });

	fields.Add("ChangeDescription", new ActiveField<string>() { Name = "ChangeDescription", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="ModificationLog"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ModificationLog"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ModificationLog"  });

	fields.Add("RecordIDChar", new ActiveField<string>() { Name = "RecordIDChar", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ModificationLog"  });

	fields.Add("PageUrl", new ActiveField<string>() { Name = "PageUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=350, TableName="ModificationLog"  });

	fields.Add("UserIpAddress", new ActiveField<string>() { Name = "UserIpAddress", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=350, TableName="ModificationLog"  });

	fields.Add("UserAgent", new ActiveField<string>() { Name = "UserAgent", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=350, TableName="ModificationLog"  });
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
				var rec = ActiveRecordLoader.LoadID<ModificationLog>(id, "ModificationLog", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ModificationLog with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ModificationLog or null if not in cache.</returns>
//		private static ModificationLog GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ModificationLog-" + id) as ModificationLog;
//			return Web.PageGlobals["ActiveRecord-ModificationLog-" + id] as ModificationLog;
//		}

		/// <summary>
		/// Caches this ModificationLog object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ModificationLog-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ModificationLog-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ModificationLog-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ModificationLog objects/records. This is the usual data structure for holding a number of ModificationLog records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ModificationLogList : ActiveRecordList<ModificationLog> {

		public ModificationLogList() : base() {}
		public ModificationLogList(List<ModificationLog> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ModificationLog to ModificationLogList. 
		/// </summary>
		public static implicit operator ModificationLogList(List<ModificationLog> list) {
			return new ModificationLogList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ModificationLogList to List-of-ModificationLog. 
		/// </summary>
		public static implicit operator List<ModificationLog>(ModificationLogList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ModificationLog objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ModificationLog records.</returns>
		public static ModificationLogList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ModificationLogID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ModificationLog objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ModificationLog records.</returns>
		public static ModificationLogList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ModificationLogList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ModificationLogID in (", ids.SqlizeNumberList(), ")");
			var result = new ModificationLogList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ModificationLog objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ModificationLog records.</returns>
		public static ModificationLogList Load(Sql sql) {
			var result = new ModificationLogList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ModificationLog objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ModificationLogID desc.)
		/// </summary>
		public static ModificationLogList LoadAll() {
			var result = new ModificationLogList();
			result.LoadRecords(null);
			return result;
		}
		public static ModificationLogList LoadAll(int itemsPerPage) {
			var result = new ModificationLogList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ModificationLogList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ModificationLogList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ModificationLog objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ModificationLogList LoadActive() {
			var result = new ModificationLogList();
			var sql = (new ModificationLog()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ModificationLogList LoadActive(int itemsPerPage) {
			var result = new ModificationLogList();
			var sql = (new ModificationLog()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ModificationLogList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ModificationLogList();
			var sql = (new ModificationLog()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ModificationLogList LoadActivePlusExisting(object existingRecordID) {
			var result = new ModificationLogList();
			var sql = (new ModificationLog()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ModificationLog");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ModificationLog");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ModificationLog()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ModificationLog.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ModificationLogID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ModificationLogID {
			get { return Fields.ModificationLogID.Value; }
			set { fields["ModificationLogID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByModificationLogID(int modificationLogIDValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("ModificationLogID", modificationLogIDValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ModificationLogID {
				get { return (ActiveField<int>)fields["ModificationLogID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByModificationLogID(int modificationLogIDValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where ModificationLogID=", Sql.Sqlize(modificationLogIDValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UpdateDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? UpdateDate {
			get { return Fields.UpdateDate.Value; }
			set { fields["UpdateDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByUpdateDate(System.DateTime? updateDateValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("UpdateDate", updateDateValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> UpdateDate {
				get { return (ActiveField<System.DateTime?>)fields["UpdateDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByUpdateDate(System.DateTime? updateDateValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where UpdateDate=", Sql.Sqlize(updateDateValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("PersonID", personIDValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ModificationLog {
		[NonSerialized]		
		private Person _Person;

		[JetBrains.Annotations.CanBeNull]
		public Person Person
		{
			get
			{
				 // lazy load
				if (this._Person == null && this.PersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Person") && container.PrefetchCounter["Person"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.PersonID).ToList(),"Person",Otherwise.Null);
					}
					this._Person = Models.Person.LoadByPersonID(PersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Person")) {
							container.PrefetchCounter["Person"] = 0;
						}
						container.PrefetchCounter["Person"]++;
					}
				}
				return this._Person;
			}
			set
			{
				this._Person = value;
			}
		}
	}

	public partial class ModificationLogList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private ModificationLogList _ModificationLogs;
		
		[JetBrains.Annotations.NotNull]
		public ModificationLogList ModificationLogs
		{
			get
			{
				// lazy load
				if (this._ModificationLogs == null) {
					this._ModificationLogs = Models.ModificationLogList.LoadByPersonID(this.ID);
					this._ModificationLogs.SetParentBindField(this, "PersonID");
				}
				return this._ModificationLogs;
			}
			set
			{
				this._ModificationLogs = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: TableName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TableName {
			get { return Fields.TableName.Value; }
			set { fields["TableName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByTableName(string tableNameValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("TableName", tableNameValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TableName {
				get { return (ActiveField<string>)fields["TableName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByTableName(string tableNameValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where TableName=", Sql.Sqlize(tableNameValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RecordID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? RecordID {
			get { return Fields.RecordID.Value; }
			set { fields["RecordID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByRecordID(int? recordIDValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("RecordID", recordIDValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> RecordID {
				get { return (ActiveField<int?>)fields["RecordID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByRecordID(int? recordIDValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where RecordID=", Sql.Sqlize(recordIDValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ActionType
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ActionType {
			get { return Fields.ActionType.Value; }
			set { fields["ActionType"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByActionType(string actionTypeValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("ActionType", actionTypeValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ActionType {
				get { return (ActiveField<string>)fields["ActionType"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByActionType(string actionTypeValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where ActionType=", Sql.Sqlize(actionTypeValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UserName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UserName {
			get { return Fields.UserName.Value; }
			set { fields["UserName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByUserName(string userNameValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("UserName", userNameValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> UserName {
				get { return (ActiveField<string>)fields["UserName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByUserName(string userNameValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where UserName=", Sql.Sqlize(userNameValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ChangeDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ChangeDescription {
			get { return Fields.ChangeDescription.Value; }
			set { fields["ChangeDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByChangeDescription(string changeDescriptionValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("ChangeDescription", changeDescriptionValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ChangeDescription {
				get { return (ActiveField<string>)fields["ChangeDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByChangeDescription(string changeDescriptionValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where ChangeDescription=", Sql.Sqlize(changeDescriptionValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("DateAdded", dateAddedValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("DateModified", dateModifiedValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RecordIDChar
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string RecordIDChar {
			get { return Fields.RecordIDChar.Value; }
			set { fields["RecordIDChar"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByRecordIDChar(string recordIDCharValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("RecordIDChar", recordIDCharValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> RecordIDChar {
				get { return (ActiveField<string>)fields["RecordIDChar"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByRecordIDChar(string recordIDCharValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where RecordIDChar=", Sql.Sqlize(recordIDCharValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageUrl {
			get { return Fields.PageUrl.Value; }
			set { fields["PageUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByPageUrl(string pageUrlValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("PageUrl", pageUrlValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageUrl {
				get { return (ActiveField<string>)fields["PageUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByPageUrl(string pageUrlValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where PageUrl=", Sql.Sqlize(pageUrlValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UserIpAddress
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UserIpAddress {
			get { return Fields.UserIpAddress.Value; }
			set { fields["UserIpAddress"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByUserIpAddress(string userIpAddressValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("UserIpAddress", userIpAddressValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> UserIpAddress {
				get { return (ActiveField<string>)fields["UserIpAddress"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByUserIpAddress(string userIpAddressValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where UserIpAddress=", Sql.Sqlize(userIpAddressValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UserAgent
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ModificationLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UserAgent {
			get { return Fields.UserAgent.Value; }
			set { fields["UserAgent"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ModificationLog LoadByUserAgent(string userAgentValue) {
			return ActiveRecordLoader.LoadByField<ModificationLog>("UserAgent", userAgentValue, "ModificationLog", Otherwise.Null);
		}

		public partial class ModificationLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> UserAgent {
				get { return (ActiveField<string>)fields["UserAgent"]; }
			}
		}

	}
	
	// define list class 
	public partial class ModificationLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static ModificationLogList LoadByUserAgent(string userAgentValue) {
			var sql = new Sql("select * from ","ModificationLog".SqlizeName()," where UserAgent=", Sql.Sqlize(userAgentValue));
			return ModificationLogList.Load(sql);
		}		
		
	}


}
#endregion