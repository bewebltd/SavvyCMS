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
// CLASS: GenTestHasCat
// TABLE: GenTestHasCat
//-----------------------------------------


	public partial class GenTestHasCat : ActiveRecord {

		/// <summary>
		/// The list that this GenTestHasCat is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<GenTestHasCat> GetContainingList() {
			return (ActiveRecordList<GenTestHasCat>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public GenTestHasCat(): base("GenTestHasCat", "GenTestHasCatID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "GenTestHasCat";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "GenTestHasCatID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property GenTestHasCatID.
		/// </summary>
		public int ID { get { return (int)fields["GenTestHasCatID"].ValueObject; } set { fields["GenTestHasCatID"].ValueObject = value; } }

		// field references
		public partial class GenTestHasCatFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public GenTestHasCatFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private GenTestHasCatFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public GenTestHasCatFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new GenTestHasCatFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the GenTestHasCat table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of GenTestHasCat</param>
		/// <returns>An instance of GenTestHasCat containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static GenTestHasCat LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the GenTestHasCat table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg GenTestHasCat.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = GenTestHasCat.LoadID(55);</example>
		/// <param name="id">Primary key of GenTestHasCat</param>
		/// <returns>An instance of GenTestHasCat containing the data in the record</returns>
		public static GenTestHasCat LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			GenTestHasCat record = null;
//			record = GenTestHasCat.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where GenTestHasCatID=", Sql.Sqlize(id));
//				record = new GenTestHasCat();
//				if (!record.LoadData(sql)) return otherwise.Execute<GenTestHasCat>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<GenTestHasCat>(id, "GenTestHasCat", otherwise);
		}

		/// <summary>
		/// Loads a record from the GenTestHasCat table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of GenTestHasCat containing the data in the record</returns>
		public static GenTestHasCat Load(Sql sql) {
				return ActiveRecordLoader.Load<GenTestHasCat>(sql, "GenTestHasCat");
		}
		public static GenTestHasCat Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GenTestHasCat>(sql, "GenTestHasCat", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the GenTestHasCat table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of GenTestHasCat containing the data in the record</returns>
		public static GenTestHasCat Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<GenTestHasCat>(reader, "GenTestHasCat");
		}
		public static GenTestHasCat Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GenTestHasCat>(reader, "GenTestHasCat", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where GenTestHasCatID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("GenTestHasCatID", new ActiveField<int>() { Name = "GenTestHasCatID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="GenTestHasCat"  });

	fields.Add("GenTestID", new ActiveField<int?>() { Name = "GenTestID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GenTestHasCat" , GetForeignRecord = () => this.GenTest, ForeignClassName = typeof(Models.GenTest), ForeignTableName = "GenTest", ForeignTableFieldName = "GenTestID" });

	fields.Add("GenTestCatID", new ActiveField<int?>() { Name = "GenTestCatID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GenTestHasCat" , GetForeignRecord = () => this.GenTestCat, ForeignClassName = typeof(Models.GenTestCat), ForeignTableName = "GenTestCat", ForeignTableFieldName = "GenTestCatID" });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTestHasCat"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTestHasCat"  });
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
				var rec = ActiveRecordLoader.LoadID<GenTestHasCat>(id, "GenTestHasCat", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the GenTestHasCat with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct GenTestHasCat or null if not in cache.</returns>
//		private static GenTestHasCat GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-GenTestHasCat-" + id) as GenTestHasCat;
//			return Web.PageGlobals["ActiveRecord-GenTestHasCat-" + id] as GenTestHasCat;
//		}

		/// <summary>
		/// Caches this GenTestHasCat object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-GenTestHasCat-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-GenTestHasCat-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-GenTestHasCat-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of GenTestHasCat objects/records. This is the usual data structure for holding a number of GenTestHasCat records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class GenTestHasCatList : ActiveRecordList<GenTestHasCat> {

		public GenTestHasCatList() : base() {}
		public GenTestHasCatList(List<GenTestHasCat> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-GenTestHasCat to GenTestHasCatList. 
		/// </summary>
		public static implicit operator GenTestHasCatList(List<GenTestHasCat> list) {
			return new GenTestHasCatList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from GenTestHasCatList to List-of-GenTestHasCat. 
		/// </summary>
		public static implicit operator List<GenTestHasCat>(GenTestHasCatList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of GenTestHasCat objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of GenTestHasCat records.</returns>
		public static GenTestHasCatList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where GenTestHasCatID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of GenTestHasCat objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of GenTestHasCat records.</returns>
		public static GenTestHasCatList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static GenTestHasCatList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where GenTestHasCatID in (", ids.SqlizeNumberList(), ")");
			var result = new GenTestHasCatList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of GenTestHasCat objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of GenTestHasCat records.</returns>
		public static GenTestHasCatList Load(Sql sql) {
			var result = new GenTestHasCatList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all GenTestHasCat objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and GenTestHasCatID desc.)
		/// </summary>
		public static GenTestHasCatList LoadAll() {
			var result = new GenTestHasCatList();
			result.LoadRecords(null);
			return result;
		}
		public static GenTestHasCatList LoadAll(int itemsPerPage) {
			var result = new GenTestHasCatList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestHasCatList LoadAll(int itemsPerPage, int pageNum) {
			var result = new GenTestHasCatList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" GenTestHasCat objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static GenTestHasCatList LoadActive() {
			var result = new GenTestHasCatList();
			var sql = (new GenTestHasCat()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestHasCatList LoadActive(int itemsPerPage) {
			var result = new GenTestHasCatList();
			var sql = (new GenTestHasCat()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestHasCatList LoadActive(int itemsPerPage, int pageNum) {
			var result = new GenTestHasCatList();
			var sql = (new GenTestHasCat()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static GenTestHasCatList LoadActivePlusExisting(object existingRecordID) {
			var result = new GenTestHasCatList();
			var sql = (new GenTestHasCat()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM GenTestHasCat");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM GenTestHasCat");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new GenTestHasCat()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = GenTestHasCat.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: GenTestHasCatID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestHasCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public int GenTestHasCatID {
			get { return Fields.GenTestHasCatID.Value; }
			set { fields["GenTestHasCatID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestHasCat LoadByGenTestHasCatID(int genTestHasCatIDValue) {
			return ActiveRecordLoader.LoadByField<GenTestHasCat>("GenTestHasCatID", genTestHasCatIDValue, "GenTestHasCat", Otherwise.Null);
		}

		public partial class GenTestHasCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> GenTestHasCatID {
				get { return (ActiveField<int>)fields["GenTestHasCatID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestHasCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestHasCatList LoadByGenTestHasCatID(int genTestHasCatIDValue) {
			var sql = new Sql("select * from ","GenTestHasCat".SqlizeName()," where GenTestHasCatID=", Sql.Sqlize(genTestHasCatIDValue));
			return GenTestHasCatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: GenTestID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestHasCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? GenTestID {
			get { return Fields.GenTestID.Value; }
			set { fields["GenTestID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestHasCat LoadByGenTestID(int? genTestIDValue) {
			return ActiveRecordLoader.LoadByField<GenTestHasCat>("GenTestID", genTestIDValue, "GenTestHasCat", Otherwise.Null);
		}

		public partial class GenTestHasCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> GenTestID {
				get { return (ActiveField<int?>)fields["GenTestID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestHasCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestHasCatList LoadByGenTestID(int? genTestIDValue) {
			var sql = new Sql("select * from ","GenTestHasCat".SqlizeName()," where GenTestID=", Sql.Sqlize(genTestIDValue));
			return GenTestHasCatList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: GenTest
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class GenTestHasCat {
		[NonSerialized]		
		private GenTest _GenTest;

		[JetBrains.Annotations.CanBeNull]
		public GenTest GenTest
		{
			get
			{
				 // lazy load
				if (this._GenTest == null && this.GenTestID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("GenTest") && container.PrefetchCounter["GenTest"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.GenTest>("GenTestID",container.Select(r=>r.GenTestID).ToList(),"GenTest",Otherwise.Null);
					}
					this._GenTest = Models.GenTest.LoadByGenTestID(GenTestID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("GenTest")) {
							container.PrefetchCounter["GenTest"] = 0;
						}
						container.PrefetchCounter["GenTest"]++;
					}
				}
				return this._GenTest;
			}
			set
			{
				this._GenTest = value;
			}
		}
	}

	public partial class GenTestHasCatList {
		internal int numFetchesOfGenTest = 0;
	}
	
	// define list in partial foreign table class 
	public partial class GenTest {
		[NonSerialized]		
		private GenTestHasCatList _GenTestHasCats;
		
		[JetBrains.Annotations.NotNull]
		public GenTestHasCatList GenTestHasCats
		{
			get
			{
				// lazy load
				if (this._GenTestHasCats == null) {
					this._GenTestHasCats = Models.GenTestHasCatList.LoadByGenTestID(this.ID);
					this._GenTestHasCats.SetParentBindField(this, "GenTestID");
				}
				return this._GenTestHasCats;
			}
			set
			{
				this._GenTestHasCats = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: GenTestCatID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestHasCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? GenTestCatID {
			get { return Fields.GenTestCatID.Value; }
			set { fields["GenTestCatID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestHasCat LoadByGenTestCatID(int? genTestCatIDValue) {
			return ActiveRecordLoader.LoadByField<GenTestHasCat>("GenTestCatID", genTestCatIDValue, "GenTestHasCat", Otherwise.Null);
		}

		public partial class GenTestHasCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> GenTestCatID {
				get { return (ActiveField<int?>)fields["GenTestCatID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestHasCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestHasCatList LoadByGenTestCatID(int? genTestCatIDValue) {
			var sql = new Sql("select * from ","GenTestHasCat".SqlizeName()," where GenTestCatID=", Sql.Sqlize(genTestCatIDValue));
			return GenTestHasCatList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: GenTestCat
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class GenTestHasCat {
		[NonSerialized]		
		private GenTestCat _GenTestCat;

		[JetBrains.Annotations.CanBeNull]
		public GenTestCat GenTestCat
		{
			get
			{
				 // lazy load
				if (this._GenTestCat == null && this.GenTestCatID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("GenTestCat") && container.PrefetchCounter["GenTestCat"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.GenTestCat>("GenTestCatID",container.Select(r=>r.GenTestCatID).ToList(),"GenTestCat",Otherwise.Null);
					}
					this._GenTestCat = Models.GenTestCat.LoadByGenTestCatID(GenTestCatID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("GenTestCat")) {
							container.PrefetchCounter["GenTestCat"] = 0;
						}
						container.PrefetchCounter["GenTestCat"]++;
					}
				}
				return this._GenTestCat;
			}
			set
			{
				this._GenTestCat = value;
			}
		}
	}

	public partial class GenTestHasCatList {
		internal int numFetchesOfGenTestCat = 0;
	}
	
	// define list in partial foreign table class 
	public partial class GenTestCat {
		[NonSerialized]		
		private GenTestHasCatList _GenTestHasCats;
		
		[JetBrains.Annotations.NotNull]
		public GenTestHasCatList GenTestHasCats
		{
			get
			{
				// lazy load
				if (this._GenTestHasCats == null) {
					this._GenTestHasCats = Models.GenTestHasCatList.LoadByGenTestCatID(this.ID);
					this._GenTestHasCats.SetParentBindField(this, "GenTestCatID");
				}
				return this._GenTestHasCats;
			}
			set
			{
				this._GenTestHasCats = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestHasCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestHasCat LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<GenTestHasCat>("DateAdded", dateAddedValue, "GenTestHasCat", Otherwise.Null);
		}

		public partial class GenTestHasCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestHasCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestHasCatList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","GenTestHasCat".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return GenTestHasCatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestHasCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestHasCat LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<GenTestHasCat>("DateModified", dateModifiedValue, "GenTestHasCat", Otherwise.Null);
		}

		public partial class GenTestHasCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestHasCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestHasCatList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","GenTestHasCat".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return GenTestHasCatList.Load(sql);
		}		
		
	}


}
#endregion