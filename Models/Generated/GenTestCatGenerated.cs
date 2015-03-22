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
// CLASS: GenTestCat
// TABLE: GenTestCat
//-----------------------------------------


	public partial class GenTestCat : ActiveRecord {

		/// <summary>
		/// The list that this GenTestCat is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<GenTestCat> GetContainingList() {
			return (ActiveRecordList<GenTestCat>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public GenTestCat(): base("GenTestCat", "GenTestCatID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "GenTestCat";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "GenTestCatID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property GenTestCatID.
		/// </summary>
		public int ID { get { return (int)fields["GenTestCatID"].ValueObject; } set { fields["GenTestCatID"].ValueObject = value; } }

		// field references
		public partial class GenTestCatFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public GenTestCatFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private GenTestCatFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public GenTestCatFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new GenTestCatFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the GenTestCat table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of GenTestCat</param>
		/// <returns>An instance of GenTestCat containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static GenTestCat LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the GenTestCat table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg GenTestCat.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = GenTestCat.LoadID(55);</example>
		/// <param name="id">Primary key of GenTestCat</param>
		/// <returns>An instance of GenTestCat containing the data in the record</returns>
		public static GenTestCat LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			GenTestCat record = null;
//			record = GenTestCat.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where GenTestCatID=", Sql.Sqlize(id));
//				record = new GenTestCat();
//				if (!record.LoadData(sql)) return otherwise.Execute<GenTestCat>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<GenTestCat>(id, "GenTestCat", otherwise);
		}

		/// <summary>
		/// Loads a record from the GenTestCat table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of GenTestCat containing the data in the record</returns>
		public static GenTestCat Load(Sql sql) {
				return ActiveRecordLoader.Load<GenTestCat>(sql, "GenTestCat");
		}
		public static GenTestCat Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GenTestCat>(sql, "GenTestCat", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the GenTestCat table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of GenTestCat containing the data in the record</returns>
		public static GenTestCat Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<GenTestCat>(reader, "GenTestCat");
		}
		public static GenTestCat Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GenTestCat>(reader, "GenTestCat", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where GenTestCatID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("GenTestCatID", new ActiveField<int>() { Name = "GenTestCatID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="GenTestCat"  });

	fields.Add("Category", new ActiveField<string>() { Name = "Category", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GenTestCat"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTestCat"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTestCat"  });
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
				var rec = ActiveRecordLoader.LoadID<GenTestCat>(id, "GenTestCat", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the GenTestCat with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct GenTestCat or null if not in cache.</returns>
//		private static GenTestCat GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-GenTestCat-" + id) as GenTestCat;
//			return Web.PageGlobals["ActiveRecord-GenTestCat-" + id] as GenTestCat;
//		}

		/// <summary>
		/// Caches this GenTestCat object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-GenTestCat-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-GenTestCat-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-GenTestCat-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of GenTestCat objects/records. This is the usual data structure for holding a number of GenTestCat records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class GenTestCatList : ActiveRecordList<GenTestCat> {

		public GenTestCatList() : base() {}
		public GenTestCatList(List<GenTestCat> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-GenTestCat to GenTestCatList. 
		/// </summary>
		public static implicit operator GenTestCatList(List<GenTestCat> list) {
			return new GenTestCatList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from GenTestCatList to List-of-GenTestCat. 
		/// </summary>
		public static implicit operator List<GenTestCat>(GenTestCatList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of GenTestCat objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of GenTestCat records.</returns>
		public static GenTestCatList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where GenTestCatID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of GenTestCat objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of GenTestCat records.</returns>
		public static GenTestCatList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static GenTestCatList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where GenTestCatID in (", ids.SqlizeNumberList(), ")");
			var result = new GenTestCatList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of GenTestCat objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of GenTestCat records.</returns>
		public static GenTestCatList Load(Sql sql) {
			var result = new GenTestCatList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all GenTestCat objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and GenTestCatID desc.)
		/// </summary>
		public static GenTestCatList LoadAll() {
			var result = new GenTestCatList();
			result.LoadRecords(null);
			return result;
		}
		public static GenTestCatList LoadAll(int itemsPerPage) {
			var result = new GenTestCatList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestCatList LoadAll(int itemsPerPage, int pageNum) {
			var result = new GenTestCatList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" GenTestCat objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static GenTestCatList LoadActive() {
			var result = new GenTestCatList();
			var sql = (new GenTestCat()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestCatList LoadActive(int itemsPerPage) {
			var result = new GenTestCatList();
			var sql = (new GenTestCat()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestCatList LoadActive(int itemsPerPage, int pageNum) {
			var result = new GenTestCatList();
			var sql = (new GenTestCat()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static GenTestCatList LoadActivePlusExisting(object existingRecordID) {
			var result = new GenTestCatList();
			var sql = (new GenTestCat()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM GenTestCat");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM GenTestCat");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new GenTestCat()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = GenTestCat.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: GenTestCatID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public int GenTestCatID {
			get { return Fields.GenTestCatID.Value; }
			set { fields["GenTestCatID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestCat LoadByGenTestCatID(int genTestCatIDValue) {
			return ActiveRecordLoader.LoadByField<GenTestCat>("GenTestCatID", genTestCatIDValue, "GenTestCat", Otherwise.Null);
		}

		public partial class GenTestCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> GenTestCatID {
				get { return (ActiveField<int>)fields["GenTestCatID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestCatList LoadByGenTestCatID(int genTestCatIDValue) {
			var sql = new Sql("select * from ","GenTestCat".SqlizeName()," where GenTestCatID=", Sql.Sqlize(genTestCatIDValue));
			return GenTestCatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Category
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Category {
			get { return Fields.Category.Value; }
			set { fields["Category"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestCat LoadByCategory(string categoryValue) {
			return ActiveRecordLoader.LoadByField<GenTestCat>("Category", categoryValue, "GenTestCat", Otherwise.Null);
		}

		public partial class GenTestCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Category {
				get { return (ActiveField<string>)fields["Category"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestCatList LoadByCategory(string categoryValue) {
			var sql = new Sql("select * from ","GenTestCat".SqlizeName()," where Category=", Sql.Sqlize(categoryValue));
			return GenTestCatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestCat LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<GenTestCat>("DateAdded", dateAddedValue, "GenTestCat", Otherwise.Null);
		}

		public partial class GenTestCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestCatList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","GenTestCat".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return GenTestCatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTestCat {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTestCat LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<GenTestCat>("DateModified", dateModifiedValue, "GenTestCat", Otherwise.Null);
		}

		public partial class GenTestCatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestCatList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestCatList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","GenTestCat".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return GenTestCatList.Load(sql);
		}		
		
	}


}
#endregion