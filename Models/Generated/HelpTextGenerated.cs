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
// CLASS: HelpText
// TABLE: HelpText
//-----------------------------------------


	public partial class HelpText : ActiveRecord {

		/// <summary>
		/// The list that this HelpText is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<HelpText> GetContainingList() {
			return (ActiveRecordList<HelpText>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public HelpText(): base("HelpText", "HelpTextID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "HelpText";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "HelpTextID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property HelpTextID.
		/// </summary>
		public int ID { get { return (int)fields["HelpTextID"].ValueObject; } set { fields["HelpTextID"].ValueObject = value; } }

		// field references
		public partial class HelpTextFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public HelpTextFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private HelpTextFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public HelpTextFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new HelpTextFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the HelpText table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of HelpText</param>
		/// <returns>An instance of HelpText containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the HelpText table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg HelpText.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = HelpText.LoadID(55);</example>
		/// <param name="id">Primary key of HelpText</param>
		/// <returns>An instance of HelpText containing the data in the record</returns>
		public static HelpText LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			HelpText record = null;
//			record = HelpText.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where HelpTextID=", Sql.Sqlize(id));
//				record = new HelpText();
//				if (!record.LoadData(sql)) return otherwise.Execute<HelpText>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<HelpText>(id, "HelpText", otherwise);
		}

		/// <summary>
		/// Loads a record from the HelpText table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of HelpText containing the data in the record</returns>
		public static HelpText Load(Sql sql) {
				return ActiveRecordLoader.Load<HelpText>(sql, "HelpText");
		}
		public static HelpText Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<HelpText>(sql, "HelpText", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the HelpText table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of HelpText containing the data in the record</returns>
		public static HelpText Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<HelpText>(reader, "HelpText");
		}
		public static HelpText Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<HelpText>(reader, "HelpText", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where HelpTextID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("HelpTextID", new ActiveField<int>() { Name = "HelpTextID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="HelpText"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="HelpText"  });

	fields.Add("HelpTextCode", new ActiveField<string>() { Name = "HelpTextCode", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="HelpText"  });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="HelpText"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="HelpText"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="HelpText"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="HelpText"  });

	fields.Add("AdminNotes", new ActiveField<string>() { Name = "AdminNotes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="HelpText"  });
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
				var rec = ActiveRecordLoader.LoadID<HelpText>(id, "HelpText", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the HelpText with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct HelpText or null if not in cache.</returns>
//		private static HelpText GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-HelpText-" + id) as HelpText;
//			return Web.PageGlobals["ActiveRecord-HelpText-" + id] as HelpText;
//		}

		/// <summary>
		/// Caches this HelpText object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-HelpText-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-HelpText-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-HelpText-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of HelpText objects/records. This is the usual data structure for holding a number of HelpText records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class HelpTextList : ActiveRecordList<HelpText> {

		public HelpTextList() : base() {}
		public HelpTextList(List<HelpText> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-HelpText to HelpTextList. 
		/// </summary>
		public static implicit operator HelpTextList(List<HelpText> list) {
			return new HelpTextList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from HelpTextList to List-of-HelpText. 
		/// </summary>
		public static implicit operator List<HelpText>(HelpTextList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of HelpText objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of HelpText records.</returns>
		public static HelpTextList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where HelpTextID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of HelpText objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of HelpText records.</returns>
		public static HelpTextList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static HelpTextList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where HelpTextID in (", ids.SqlizeNumberList(), ")");
			var result = new HelpTextList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of HelpText objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of HelpText records.</returns>
		public static HelpTextList Load(Sql sql) {
			var result = new HelpTextList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all HelpText objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and HelpTextID desc.)
		/// </summary>
		public static HelpTextList LoadAll() {
			var result = new HelpTextList();
			result.LoadRecords(null);
			return result;
		}
		public static HelpTextList LoadAll(int itemsPerPage) {
			var result = new HelpTextList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static HelpTextList LoadAll(int itemsPerPage, int pageNum) {
			var result = new HelpTextList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" HelpText objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static HelpTextList LoadActive() {
			var result = new HelpTextList();
			var sql = (new HelpText()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static HelpTextList LoadActive(int itemsPerPage) {
			var result = new HelpTextList();
			var sql = (new HelpText()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static HelpTextList LoadActive(int itemsPerPage, int pageNum) {
			var result = new HelpTextList();
			var sql = (new HelpText()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static HelpTextList LoadActivePlusExisting(object existingRecordID) {
			var result = new HelpTextList();
			var sql = (new HelpText()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM HelpText");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM HelpText");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new HelpText()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = HelpText.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: HelpTextID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public int HelpTextID {
			get { return Fields.HelpTextID.Value; }
			set { fields["HelpTextID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByHelpTextID(int helpTextIDValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("HelpTextID", helpTextIDValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> HelpTextID {
				get { return (ActiveField<int>)fields["HelpTextID"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByHelpTextID(int helpTextIDValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where HelpTextID=", Sql.Sqlize(helpTextIDValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("Title", titleValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: HelpTextCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public string HelpTextCode {
			get { return Fields.HelpTextCode.Value; }
			set { fields["HelpTextCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByHelpTextCode(string helpTextCodeValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("HelpTextCode", helpTextCodeValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> HelpTextCode {
				get { return (ActiveField<string>)fields["HelpTextCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByHelpTextCode(string helpTextCodeValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where HelpTextCode=", Sql.Sqlize(helpTextCodeValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("BodyTextHtml", bodyTextHtmlValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("SortPosition", sortPositionValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("DateAdded", dateAddedValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("DateModified", dateModifiedValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return HelpTextList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AdminNotes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HelpText {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AdminNotes {
			get { return Fields.AdminNotes.Value; }
			set { fields["AdminNotes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HelpText LoadByAdminNotes(string adminNotesValue) {
			return ActiveRecordLoader.LoadByField<HelpText>("AdminNotes", adminNotesValue, "HelpText", Otherwise.Null);
		}

		public partial class HelpTextFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AdminNotes {
				get { return (ActiveField<string>)fields["AdminNotes"]; }
			}
		}

	}
	
	// define list class 
	public partial class HelpTextList {		
				
		[JetBrains.Annotations.NotNull]
		public static HelpTextList LoadByAdminNotes(string adminNotesValue) {
			var sql = new Sql("select * from ","HelpText".SqlizeName()," where AdminNotes=", Sql.Sqlize(adminNotesValue));
			return HelpTextList.Load(sql);
		}		
		
	}


}
#endregion