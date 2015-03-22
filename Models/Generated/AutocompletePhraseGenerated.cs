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
// CLASS: AutocompletePhrase
// TABLE: AutocompletePhrase
//-----------------------------------------


	public partial class AutocompletePhrase : ActiveRecord {

		/// <summary>
		/// The list that this AutocompletePhrase is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<AutocompletePhrase> GetContainingList() {
			return (ActiveRecordList<AutocompletePhrase>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public AutocompletePhrase(): base("AutocompletePhrase", "AutocompletePhraseID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "AutocompletePhrase";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "AutocompletePhraseID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property AutocompletePhraseID.
		/// </summary>
		public int ID { get { return (int)fields["AutocompletePhraseID"].ValueObject; } set { fields["AutocompletePhraseID"].ValueObject = value; } }

		// field references
		public partial class AutocompletePhraseFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public AutocompletePhraseFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private AutocompletePhraseFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public AutocompletePhraseFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new AutocompletePhraseFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the AutocompletePhrase table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of AutocompletePhrase</param>
		/// <returns>An instance of AutocompletePhrase containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the AutocompletePhrase table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg AutocompletePhrase.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = AutocompletePhrase.LoadID(55);</example>
		/// <param name="id">Primary key of AutocompletePhrase</param>
		/// <returns>An instance of AutocompletePhrase containing the data in the record</returns>
		public static AutocompletePhrase LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			AutocompletePhrase record = null;
//			record = AutocompletePhrase.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where AutocompletePhraseID=", Sql.Sqlize(id));
//				record = new AutocompletePhrase();
//				if (!record.LoadData(sql)) return otherwise.Execute<AutocompletePhrase>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<AutocompletePhrase>(id, "AutocompletePhrase", otherwise);
		}

		/// <summary>
		/// Loads a record from the AutocompletePhrase table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of AutocompletePhrase containing the data in the record</returns>
		public static AutocompletePhrase Load(Sql sql) {
				return ActiveRecordLoader.Load<AutocompletePhrase>(sql, "AutocompletePhrase");
		}
		public static AutocompletePhrase Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<AutocompletePhrase>(sql, "AutocompletePhrase", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the AutocompletePhrase table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of AutocompletePhrase containing the data in the record</returns>
		public static AutocompletePhrase Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<AutocompletePhrase>(reader, "AutocompletePhrase");
		}
		public static AutocompletePhrase Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<AutocompletePhrase>(reader, "AutocompletePhrase", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where AutocompletePhraseID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("AutocompletePhraseID", new ActiveField<int>() { Name = "AutocompletePhraseID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="AutocompletePhrase"  });

	fields.Add("TableName", new ActiveField<string>() { Name = "TableName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="AutocompletePhrase"  });

	fields.Add("RecordID", new ActiveField<int?>() { Name = "RecordID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="AutocompletePhrase"  });

	fields.Add("Phrase", new ActiveField<string>() { Name = "Phrase", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="AutocompletePhrase"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="AutocompletePhrase"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="AutocompletePhrase"  });
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
				var rec = ActiveRecordLoader.LoadID<AutocompletePhrase>(id, "AutocompletePhrase", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the AutocompletePhrase with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct AutocompletePhrase or null if not in cache.</returns>
//		private static AutocompletePhrase GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-AutocompletePhrase-" + id) as AutocompletePhrase;
//			return Web.PageGlobals["ActiveRecord-AutocompletePhrase-" + id] as AutocompletePhrase;
//		}

		/// <summary>
		/// Caches this AutocompletePhrase object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-AutocompletePhrase-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-AutocompletePhrase-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-AutocompletePhrase-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of AutocompletePhrase objects/records. This is the usual data structure for holding a number of AutocompletePhrase records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class AutocompletePhraseList : ActiveRecordList<AutocompletePhrase> {

		public AutocompletePhraseList() : base() {}
		public AutocompletePhraseList(List<AutocompletePhrase> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-AutocompletePhrase to AutocompletePhraseList. 
		/// </summary>
		public static implicit operator AutocompletePhraseList(List<AutocompletePhrase> list) {
			return new AutocompletePhraseList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from AutocompletePhraseList to List-of-AutocompletePhrase. 
		/// </summary>
		public static implicit operator List<AutocompletePhrase>(AutocompletePhraseList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of AutocompletePhrase objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of AutocompletePhrase records.</returns>
		public static AutocompletePhraseList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where AutocompletePhraseID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of AutocompletePhrase objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of AutocompletePhrase records.</returns>
		public static AutocompletePhraseList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static AutocompletePhraseList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where AutocompletePhraseID in (", ids.SqlizeNumberList(), ")");
			var result = new AutocompletePhraseList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of AutocompletePhrase objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of AutocompletePhrase records.</returns>
		public static AutocompletePhraseList Load(Sql sql) {
			var result = new AutocompletePhraseList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all AutocompletePhrase objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and AutocompletePhraseID desc.)
		/// </summary>
		public static AutocompletePhraseList LoadAll() {
			var result = new AutocompletePhraseList();
			result.LoadRecords(null);
			return result;
		}
		public static AutocompletePhraseList LoadAll(int itemsPerPage) {
			var result = new AutocompletePhraseList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static AutocompletePhraseList LoadAll(int itemsPerPage, int pageNum) {
			var result = new AutocompletePhraseList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" AutocompletePhrase objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static AutocompletePhraseList LoadActive() {
			var result = new AutocompletePhraseList();
			var sql = (new AutocompletePhrase()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static AutocompletePhraseList LoadActive(int itemsPerPage) {
			var result = new AutocompletePhraseList();
			var sql = (new AutocompletePhrase()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static AutocompletePhraseList LoadActive(int itemsPerPage, int pageNum) {
			var result = new AutocompletePhraseList();
			var sql = (new AutocompletePhrase()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static AutocompletePhraseList LoadActivePlusExisting(object existingRecordID) {
			var result = new AutocompletePhraseList();
			var sql = (new AutocompletePhrase()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM AutocompletePhrase");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM AutocompletePhrase");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new AutocompletePhrase()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = AutocompletePhrase.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: AutocompletePhraseID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class AutocompletePhrase {		
				
		[JetBrains.Annotations.CanBeNull]
		public int AutocompletePhraseID {
			get { return Fields.AutocompletePhraseID.Value; }
			set { fields["AutocompletePhraseID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadByAutocompletePhraseID(int autocompletePhraseIDValue) {
			return ActiveRecordLoader.LoadByField<AutocompletePhrase>("AutocompletePhraseID", autocompletePhraseIDValue, "AutocompletePhrase", Otherwise.Null);
		}

		public partial class AutocompletePhraseFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> AutocompletePhraseID {
				get { return (ActiveField<int>)fields["AutocompletePhraseID"]; }
			}
		}

	}
	
	// define list class 
	public partial class AutocompletePhraseList {		
				
		[JetBrains.Annotations.NotNull]
		public static AutocompletePhraseList LoadByAutocompletePhraseID(int autocompletePhraseIDValue) {
			var sql = new Sql("select * from ","AutocompletePhrase".SqlizeName()," where AutocompletePhraseID=", Sql.Sqlize(autocompletePhraseIDValue));
			return AutocompletePhraseList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TableName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class AutocompletePhrase {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TableName {
			get { return Fields.TableName.Value; }
			set { fields["TableName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadByTableName(string tableNameValue) {
			return ActiveRecordLoader.LoadByField<AutocompletePhrase>("TableName", tableNameValue, "AutocompletePhrase", Otherwise.Null);
		}

		public partial class AutocompletePhraseFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TableName {
				get { return (ActiveField<string>)fields["TableName"]; }
			}
		}

	}
	
	// define list class 
	public partial class AutocompletePhraseList {		
				
		[JetBrains.Annotations.NotNull]
		public static AutocompletePhraseList LoadByTableName(string tableNameValue) {
			var sql = new Sql("select * from ","AutocompletePhrase".SqlizeName()," where TableName=", Sql.Sqlize(tableNameValue));
			return AutocompletePhraseList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RecordID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class AutocompletePhrase {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? RecordID {
			get { return Fields.RecordID.Value; }
			set { fields["RecordID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadByRecordID(int? recordIDValue) {
			return ActiveRecordLoader.LoadByField<AutocompletePhrase>("RecordID", recordIDValue, "AutocompletePhrase", Otherwise.Null);
		}

		public partial class AutocompletePhraseFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> RecordID {
				get { return (ActiveField<int?>)fields["RecordID"]; }
			}
		}

	}
	
	// define list class 
	public partial class AutocompletePhraseList {		
				
		[JetBrains.Annotations.NotNull]
		public static AutocompletePhraseList LoadByRecordID(int? recordIDValue) {
			var sql = new Sql("select * from ","AutocompletePhrase".SqlizeName()," where RecordID=", Sql.Sqlize(recordIDValue));
			return AutocompletePhraseList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Phrase
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class AutocompletePhrase {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Phrase {
			get { return Fields.Phrase.Value; }
			set { fields["Phrase"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadByPhrase(string phraseValue) {
			return ActiveRecordLoader.LoadByField<AutocompletePhrase>("Phrase", phraseValue, "AutocompletePhrase", Otherwise.Null);
		}

		public partial class AutocompletePhraseFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Phrase {
				get { return (ActiveField<string>)fields["Phrase"]; }
			}
		}

	}
	
	// define list class 
	public partial class AutocompletePhraseList {		
				
		[JetBrains.Annotations.NotNull]
		public static AutocompletePhraseList LoadByPhrase(string phraseValue) {
			var sql = new Sql("select * from ","AutocompletePhrase".SqlizeName()," where Phrase=", Sql.Sqlize(phraseValue));
			return AutocompletePhraseList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class AutocompletePhrase {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<AutocompletePhrase>("DateAdded", dateAddedValue, "AutocompletePhrase", Otherwise.Null);
		}

		public partial class AutocompletePhraseFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class AutocompletePhraseList {		
				
		[JetBrains.Annotations.NotNull]
		public static AutocompletePhraseList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","AutocompletePhrase".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return AutocompletePhraseList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class AutocompletePhrase {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static AutocompletePhrase LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<AutocompletePhrase>("DateModified", dateModifiedValue, "AutocompletePhrase", Otherwise.Null);
		}

		public partial class AutocompletePhraseFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class AutocompletePhraseList {		
				
		[JetBrains.Annotations.NotNull]
		public static AutocompletePhraseList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","AutocompletePhrase".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return AutocompletePhraseList.Load(sql);
		}		
		
	}


}
#endregion