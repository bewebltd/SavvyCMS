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
// CLASS: MailLog
// TABLE: MailLog
//-----------------------------------------


	public partial class MailLog : ActiveRecord {

		/// <summary>
		/// The list that this MailLog is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<MailLog> GetContainingList() {
			return (ActiveRecordList<MailLog>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public MailLog(): base("MailLog", "MailLogID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "MailLog";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "MailLogID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property MailLogID.
		/// </summary>
		public int ID { get { return (int)fields["MailLogID"].ValueObject; } set { fields["MailLogID"].ValueObject = value; } }

		// field references
		public partial class MailLogFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public MailLogFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private MailLogFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public MailLogFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new MailLogFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the MailLog table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of MailLog</param>
		/// <returns>An instance of MailLog containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the MailLog table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg MailLog.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = MailLog.LoadID(55);</example>
		/// <param name="id">Primary key of MailLog</param>
		/// <returns>An instance of MailLog containing the data in the record</returns>
		public static MailLog LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			MailLog record = null;
//			record = MailLog.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where MailLogID=", Sql.Sqlize(id));
//				record = new MailLog();
//				if (!record.LoadData(sql)) return otherwise.Execute<MailLog>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<MailLog>(id, "MailLog", otherwise);
		}

		/// <summary>
		/// Loads a record from the MailLog table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of MailLog containing the data in the record</returns>
		public static MailLog Load(Sql sql) {
				return ActiveRecordLoader.Load<MailLog>(sql, "MailLog");
		}
		public static MailLog Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<MailLog>(sql, "MailLog", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the MailLog table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of MailLog containing the data in the record</returns>
		public static MailLog Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<MailLog>(reader, "MailLog");
		}
		public static MailLog Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<MailLog>(reader, "MailLog", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where MailLogID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("MailLogID", new ActiveField<int>() { Name = "MailLogID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="MailLog"  });

	fields.Add("EmailTo", new ActiveField<string>() { Name = "EmailTo", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MailLog"  });

	fields.Add("EmailSubject", new ActiveField<string>() { Name = "EmailSubject", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MailLog"  });

	fields.Add("Result", new ActiveField<string>() { Name = "Result", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="MailLog"  });

	fields.Add("DateSent", new ActiveField<System.DateTime?>() { Name = "DateSent", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MailLog"  });

	fields.Add("EmailFrom", new ActiveField<string>() { Name = "EmailFrom", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MailLog"  });

	fields.Add("EmailFromName", new ActiveField<string>() { Name = "EmailFromName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MailLog"  });

	fields.Add("EmailToName", new ActiveField<string>() { Name = "EmailToName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MailLog"  });

	fields.Add("EmailCC", new ActiveField<string>() { Name = "EmailCC", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="MailLog"  });

	fields.Add("EmailBodyPlain", new ActiveField<string>() { Name = "EmailBodyPlain", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="MailLog"  });

	fields.Add("EmailBodyHtml", new ActiveField<string>() { Name = "EmailBodyHtml", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="MailLog"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MailLog"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MailLog"  });

	fields.Add("DateViewTracked", new ActiveField<System.DateTime?>() { Name = "DateViewTracked", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MailLog"  });

	fields.Add("TrackingGUID", new ActiveField<string>() { Name = "TrackingGUID", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="MailLog"  });
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
				var rec = ActiveRecordLoader.LoadID<MailLog>(id, "MailLog", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the MailLog with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct MailLog or null if not in cache.</returns>
//		private static MailLog GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-MailLog-" + id) as MailLog;
//			return Web.PageGlobals["ActiveRecord-MailLog-" + id] as MailLog;
//		}

		/// <summary>
		/// Caches this MailLog object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-MailLog-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-MailLog-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-MailLog-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of MailLog objects/records. This is the usual data structure for holding a number of MailLog records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class MailLogList : ActiveRecordList<MailLog> {

		public MailLogList() : base() {}
		public MailLogList(List<MailLog> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-MailLog to MailLogList. 
		/// </summary>
		public static implicit operator MailLogList(List<MailLog> list) {
			return new MailLogList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from MailLogList to List-of-MailLog. 
		/// </summary>
		public static implicit operator List<MailLog>(MailLogList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of MailLog objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of MailLog records.</returns>
		public static MailLogList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where MailLogID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of MailLog objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of MailLog records.</returns>
		public static MailLogList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static MailLogList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where MailLogID in (", ids.SqlizeNumberList(), ")");
			var result = new MailLogList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of MailLog objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of MailLog records.</returns>
		public static MailLogList Load(Sql sql) {
			var result = new MailLogList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all MailLog objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and MailLogID desc.)
		/// </summary>
		public static MailLogList LoadAll() {
			var result = new MailLogList();
			result.LoadRecords(null);
			return result;
		}
		public static MailLogList LoadAll(int itemsPerPage) {
			var result = new MailLogList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static MailLogList LoadAll(int itemsPerPage, int pageNum) {
			var result = new MailLogList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" MailLog objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static MailLogList LoadActive() {
			var result = new MailLogList();
			var sql = (new MailLog()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static MailLogList LoadActive(int itemsPerPage) {
			var result = new MailLogList();
			var sql = (new MailLog()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static MailLogList LoadActive(int itemsPerPage, int pageNum) {
			var result = new MailLogList();
			var sql = (new MailLog()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static MailLogList LoadActivePlusExisting(object existingRecordID) {
			var result = new MailLogList();
			var sql = (new MailLog()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM MailLog");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM MailLog");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new MailLog()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = MailLog.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: MailLogID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public int MailLogID {
			get { return Fields.MailLogID.Value; }
			set { fields["MailLogID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByMailLogID(int mailLogIDValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("MailLogID", mailLogIDValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> MailLogID {
				get { return (ActiveField<int>)fields["MailLogID"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByMailLogID(int mailLogIDValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where MailLogID=", Sql.Sqlize(mailLogIDValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailTo
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailTo {
			get { return Fields.EmailTo.Value; }
			set { fields["EmailTo"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailTo(string emailToValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailTo", emailToValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailTo {
				get { return (ActiveField<string>)fields["EmailTo"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailTo(string emailToValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailTo=", Sql.Sqlize(emailToValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailSubject
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailSubject {
			get { return Fields.EmailSubject.Value; }
			set { fields["EmailSubject"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailSubject(string emailSubjectValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailSubject", emailSubjectValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailSubject {
				get { return (ActiveField<string>)fields["EmailSubject"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailSubject(string emailSubjectValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailSubject=", Sql.Sqlize(emailSubjectValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Result
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Result {
			get { return Fields.Result.Value; }
			set { fields["Result"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByResult(string resultValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("Result", resultValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Result {
				get { return (ActiveField<string>)fields["Result"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByResult(string resultValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where Result=", Sql.Sqlize(resultValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateSent
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateSent {
			get { return Fields.DateSent.Value; }
			set { fields["DateSent"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByDateSent(System.DateTime? dateSentValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("DateSent", dateSentValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateSent {
				get { return (ActiveField<System.DateTime?>)fields["DateSent"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByDateSent(System.DateTime? dateSentValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where DateSent=", Sql.Sqlize(dateSentValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailFrom
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailFrom {
			get { return Fields.EmailFrom.Value; }
			set { fields["EmailFrom"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailFrom(string emailFromValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailFrom", emailFromValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailFrom {
				get { return (ActiveField<string>)fields["EmailFrom"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailFrom(string emailFromValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailFrom=", Sql.Sqlize(emailFromValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailFromName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailFromName {
			get { return Fields.EmailFromName.Value; }
			set { fields["EmailFromName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailFromName(string emailFromNameValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailFromName", emailFromNameValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailFromName {
				get { return (ActiveField<string>)fields["EmailFromName"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailFromName(string emailFromNameValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailFromName=", Sql.Sqlize(emailFromNameValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailToName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailToName {
			get { return Fields.EmailToName.Value; }
			set { fields["EmailToName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailToName(string emailToNameValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailToName", emailToNameValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailToName {
				get { return (ActiveField<string>)fields["EmailToName"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailToName(string emailToNameValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailToName=", Sql.Sqlize(emailToNameValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailCC
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailCC {
			get { return Fields.EmailCC.Value; }
			set { fields["EmailCC"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailCC(string emailCCValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailCC", emailCCValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailCC {
				get { return (ActiveField<string>)fields["EmailCC"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailCC(string emailCCValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailCC=", Sql.Sqlize(emailCCValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailBodyPlain
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailBodyPlain {
			get { return Fields.EmailBodyPlain.Value; }
			set { fields["EmailBodyPlain"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailBodyPlain(string emailBodyPlainValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailBodyPlain", emailBodyPlainValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailBodyPlain {
				get { return (ActiveField<string>)fields["EmailBodyPlain"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailBodyPlain(string emailBodyPlainValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailBodyPlain=", Sql.Sqlize(emailBodyPlainValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailBodyHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailBodyHtml {
			get { return Fields.EmailBodyHtml.Value; }
			set { fields["EmailBodyHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByEmailBodyHtml(string emailBodyHtmlValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("EmailBodyHtml", emailBodyHtmlValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailBodyHtml {
				get { return (ActiveField<string>)fields["EmailBodyHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByEmailBodyHtml(string emailBodyHtmlValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where EmailBodyHtml=", Sql.Sqlize(emailBodyHtmlValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("DateAdded", dateAddedValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("DateModified", dateModifiedValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateViewTracked
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateViewTracked {
			get { return Fields.DateViewTracked.Value; }
			set { fields["DateViewTracked"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByDateViewTracked(System.DateTime? dateViewTrackedValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("DateViewTracked", dateViewTrackedValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateViewTracked {
				get { return (ActiveField<System.DateTime?>)fields["DateViewTracked"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByDateViewTracked(System.DateTime? dateViewTrackedValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where DateViewTracked=", Sql.Sqlize(dateViewTrackedValue));
			return MailLogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TrackingGUID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MailLog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TrackingGUID {
			get { return Fields.TrackingGUID.Value; }
			set { fields["TrackingGUID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MailLog LoadByTrackingGUID(string trackingGUIDValue) {
			return ActiveRecordLoader.LoadByField<MailLog>("TrackingGUID", trackingGUIDValue, "MailLog", Otherwise.Null);
		}

		public partial class MailLogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TrackingGUID {
				get { return (ActiveField<string>)fields["TrackingGUID"]; }
			}
		}

	}
	
	// define list class 
	public partial class MailLogList {		
				
		[JetBrains.Annotations.NotNull]
		public static MailLogList LoadByTrackingGUID(string trackingGUIDValue) {
			var sql = new Sql("select * from ","MailLog".SqlizeName()," where TrackingGUID=", Sql.Sqlize(trackingGUIDValue));
			return MailLogList.Load(sql);
		}		
		
	}


}
#endregion