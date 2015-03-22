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
// CLASS: DocumentCategory
// TABLE: DocumentCategory
//-----------------------------------------


	public partial class DocumentCategory : ActiveRecord {

		/// <summary>
		/// The list that this DocumentCategory is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<DocumentCategory> GetContainingList() {
			return (ActiveRecordList<DocumentCategory>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public DocumentCategory(): base("DocumentCategory", "DocumentCategoryID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "DocumentCategory";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "DocumentCategoryID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property DocumentCategoryID.
		/// </summary>
		public int ID { get { return (int)fields["DocumentCategoryID"].ValueObject; } set { fields["DocumentCategoryID"].ValueObject = value; } }

		// field references
		public partial class DocumentCategoryFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public DocumentCategoryFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private DocumentCategoryFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public DocumentCategoryFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new DocumentCategoryFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the DocumentCategory table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of DocumentCategory</param>
		/// <returns>An instance of DocumentCategory containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the DocumentCategory table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg DocumentCategory.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = DocumentCategory.LoadID(55);</example>
		/// <param name="id">Primary key of DocumentCategory</param>
		/// <returns>An instance of DocumentCategory containing the data in the record</returns>
		public static DocumentCategory LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			DocumentCategory record = null;
//			record = DocumentCategory.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where DocumentCategoryID=", Sql.Sqlize(id));
//				record = new DocumentCategory();
//				if (!record.LoadData(sql)) return otherwise.Execute<DocumentCategory>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<DocumentCategory>(id, "DocumentCategory", otherwise);
		}

		/// <summary>
		/// Loads a record from the DocumentCategory table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of DocumentCategory containing the data in the record</returns>
		public static DocumentCategory Load(Sql sql) {
				return ActiveRecordLoader.Load<DocumentCategory>(sql, "DocumentCategory");
		}
		public static DocumentCategory Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<DocumentCategory>(sql, "DocumentCategory", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the DocumentCategory table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of DocumentCategory containing the data in the record</returns>
		public static DocumentCategory Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<DocumentCategory>(reader, "DocumentCategory");
		}
		public static DocumentCategory Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<DocumentCategory>(reader, "DocumentCategory", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where DocumentCategoryID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("DocumentCategoryID", new ActiveField<int>() { Name = "DocumentCategoryID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="DocumentCategory"  });

	fields.Add("ParentDocumentCategoryID", new ActiveField<int?>() { Name = "ParentDocumentCategoryID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentCategory" , GetForeignRecord = () => this.ParentDocumentCategory, ForeignClassName = typeof(Models.DocumentCategory), ForeignTableName = "DocumentCategory", ForeignTableFieldName = "DocumentCategoryID" });

	fields.Add("PageID", new ActiveField<int?>() { Name = "PageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentCategory" , GetForeignRecord = () => this.Page, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="DocumentCategory"  });

	fields.Add("IntroText", new ActiveField<string>() { Name = "IntroText", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=500, TableName="DocumentCategory"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="DocumentCategory"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentCategory"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DocumentCategory"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DocumentCategory"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DocumentCategory"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DocumentCategory"  });

	fields.Add("PageCode", new ActiveField<string>() { Name = "PageCode", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="DocumentCategory"  });

	fields.Add("AddedByPersonID", new ActiveField<int?>() { Name = "AddedByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentCategory" , GetForeignRecord = () => this.AddedByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("ModifiedByPersonID", new ActiveField<int?>() { Name = "ModifiedByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentCategory" , GetForeignRecord = () => this.ModifiedByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });
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
				var rec = ActiveRecordLoader.LoadID<DocumentCategory>(id, "DocumentCategory", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the DocumentCategory with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct DocumentCategory or null if not in cache.</returns>
//		private static DocumentCategory GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-DocumentCategory-" + id) as DocumentCategory;
//			return Web.PageGlobals["ActiveRecord-DocumentCategory-" + id] as DocumentCategory;
//		}

		/// <summary>
		/// Caches this DocumentCategory object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-DocumentCategory-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-DocumentCategory-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-DocumentCategory-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of DocumentCategory objects/records. This is the usual data structure for holding a number of DocumentCategory records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class DocumentCategoryList : ActiveRecordList<DocumentCategory> {

		public DocumentCategoryList() : base() {}
		public DocumentCategoryList(List<DocumentCategory> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-DocumentCategory to DocumentCategoryList. 
		/// </summary>
		public static implicit operator DocumentCategoryList(List<DocumentCategory> list) {
			return new DocumentCategoryList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from DocumentCategoryList to List-of-DocumentCategory. 
		/// </summary>
		public static implicit operator List<DocumentCategory>(DocumentCategoryList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of DocumentCategory objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of DocumentCategory records.</returns>
		public static DocumentCategoryList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where DocumentCategoryID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of DocumentCategory objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of DocumentCategory records.</returns>
		public static DocumentCategoryList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static DocumentCategoryList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where DocumentCategoryID in (", ids.SqlizeNumberList(), ")");
			var result = new DocumentCategoryList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of DocumentCategory objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of DocumentCategory records.</returns>
		public static DocumentCategoryList Load(Sql sql) {
			var result = new DocumentCategoryList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all DocumentCategory objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and DocumentCategoryID desc.)
		/// </summary>
		public static DocumentCategoryList LoadAll() {
			var result = new DocumentCategoryList();
			result.LoadRecords(null);
			return result;
		}
		public static DocumentCategoryList LoadAll(int itemsPerPage) {
			var result = new DocumentCategoryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentCategoryList LoadAll(int itemsPerPage, int pageNum) {
			var result = new DocumentCategoryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" DocumentCategory objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static DocumentCategoryList LoadActive() {
			var result = new DocumentCategoryList();
			var sql = (new DocumentCategory()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentCategoryList LoadActive(int itemsPerPage) {
			var result = new DocumentCategoryList();
			var sql = (new DocumentCategory()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentCategoryList LoadActive(int itemsPerPage, int pageNum) {
			var result = new DocumentCategoryList();
			var sql = (new DocumentCategory()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static DocumentCategoryList LoadActivePlusExisting(object existingRecordID) {
			var result = new DocumentCategoryList();
			var sql = (new DocumentCategory()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM DocumentCategory");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM DocumentCategory");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new DocumentCategory()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = DocumentCategory.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: DocumentCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int DocumentCategoryID {
			get { return Fields.DocumentCategoryID.Value; }
			set { fields["DocumentCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByDocumentCategoryID(int documentCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("DocumentCategoryID", documentCategoryIDValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> DocumentCategoryID {
				get { return (ActiveField<int>)fields["DocumentCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByDocumentCategoryID(int documentCategoryIDValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where DocumentCategoryID=", Sql.Sqlize(documentCategoryIDValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ParentDocumentCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ParentDocumentCategoryID {
			get { return Fields.ParentDocumentCategoryID.Value; }
			set { fields["ParentDocumentCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByParentDocumentCategoryID(int? parentDocumentCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("ParentDocumentCategoryID", parentDocumentCategoryIDValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ParentDocumentCategoryID {
				get { return (ActiveField<int?>)fields["ParentDocumentCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByParentDocumentCategoryID(int? parentDocumentCategoryIDValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where ParentDocumentCategoryID=", Sql.Sqlize(parentDocumentCategoryIDValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ParentDocumentCategory
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class DocumentCategory {
		[NonSerialized]		
		private DocumentCategory _ParentDocumentCategory;

		[JetBrains.Annotations.CanBeNull]
		public DocumentCategory ParentDocumentCategory
		{
			get
			{
				 // lazy load
				if (this._ParentDocumentCategory == null && this.ParentDocumentCategoryID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ParentDocumentCategory") && container.PrefetchCounter["ParentDocumentCategory"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.DocumentCategory>("DocumentCategoryID",container.Select(r=>r.ParentDocumentCategoryID).ToList(),"DocumentCategory",Otherwise.Null);
					}
					this._ParentDocumentCategory = Models.DocumentCategory.LoadByDocumentCategoryID(ParentDocumentCategoryID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ParentDocumentCategory")) {
							container.PrefetchCounter["ParentDocumentCategory"] = 0;
						}
						container.PrefetchCounter["ParentDocumentCategory"]++;
					}
				}
				return this._ParentDocumentCategory;
			}
			set
			{
				this._ParentDocumentCategory = value;
			}
		}
	}

	public partial class DocumentCategoryList {
		internal int numFetchesOfParentDocumentCategory = 0;
	}
	
	// define list in partial foreign table class 
	public partial class DocumentCategory {
		[NonSerialized]		
		private DocumentCategoryList _ChildDocumentCategories;
		
		[JetBrains.Annotations.NotNull]
		public DocumentCategoryList ChildDocumentCategories
		{
			get
			{
				// lazy load
				if (this._ChildDocumentCategories == null) {
					this._ChildDocumentCategories = Models.DocumentCategoryList.LoadByParentDocumentCategoryID(this.ID);
					this._ChildDocumentCategories.SetParentBindField(this, "ParentDocumentCategoryID");
				}
				return this._ChildDocumentCategories;
			}
			set
			{
				this._ChildDocumentCategories = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: PageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PageID {
			get { return Fields.PageID.Value; }
			set { fields["PageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByPageID(int? pageIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("PageID", pageIDValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PageID {
				get { return (ActiveField<int?>)fields["PageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByPageID(int? pageIDValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where PageID=", Sql.Sqlize(pageIDValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Page
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class DocumentCategory {
		[NonSerialized]		
		private Page _Page;

		[JetBrains.Annotations.CanBeNull]
		public Page Page
		{
			get
			{
				 // lazy load
				if (this._Page == null && this.PageID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Page") && container.PrefetchCounter["Page"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Page>("PageID",container.Select(r=>r.PageID).ToList(),"Page",Otherwise.Null);
					}
					this._Page = Models.Page.LoadByPageID(PageID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Page")) {
							container.PrefetchCounter["Page"] = 0;
						}
						container.PrefetchCounter["Page"]++;
					}
				}
				return this._Page;
			}
			set
			{
				this._Page = value;
			}
		}
	}

	public partial class DocumentCategoryList {
		internal int numFetchesOfPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private DocumentCategoryList _DocumentCategories;
		
		[JetBrains.Annotations.NotNull]
		public DocumentCategoryList DocumentCategories
		{
			get
			{
				// lazy load
				if (this._DocumentCategories == null) {
					this._DocumentCategories = Models.DocumentCategoryList.LoadByPageID(this.ID);
					this._DocumentCategories.SetParentBindField(this, "PageID");
				}
				return this._DocumentCategories;
			}
			set
			{
				this._DocumentCategories = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("Title", titleValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IntroText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string IntroText {
			get { return Fields.IntroText.Value; }
			set { fields["IntroText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByIntroText(string introTextValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("IntroText", introTextValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> IntroText {
				get { return (ActiveField<string>)fields["IntroText"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByIntroText(string introTextValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where IntroText=", Sql.Sqlize(introTextValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("IsActive", isActiveValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("SortPosition", sortPositionValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("DateModified", dateModifiedValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("DateAdded", dateAddedValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("PublishDate", publishDateValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("ExpiryDate", expiryDateValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageCode {
			get { return Fields.PageCode.Value; }
			set { fields["PageCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByPageCode(string pageCodeValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("PageCode", pageCodeValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageCode {
				get { return (ActiveField<string>)fields["PageCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByPageCode(string pageCodeValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where PageCode=", Sql.Sqlize(pageCodeValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AddedByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? AddedByPersonID {
			get { return Fields.AddedByPersonID.Value; }
			set { fields["AddedByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByAddedByPersonID(int? addedByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("AddedByPersonID", addedByPersonIDValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> AddedByPersonID {
				get { return (ActiveField<int?>)fields["AddedByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByAddedByPersonID(int? addedByPersonIDValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where AddedByPersonID=", Sql.Sqlize(addedByPersonIDValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: AddedByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class DocumentCategory {
		[NonSerialized]		
		private Person _AddedByPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person AddedByPerson
		{
			get
			{
				 // lazy load
				if (this._AddedByPerson == null && this.AddedByPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("AddedByPerson") && container.PrefetchCounter["AddedByPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.AddedByPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._AddedByPerson = Models.Person.LoadByPersonID(AddedByPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("AddedByPerson")) {
							container.PrefetchCounter["AddedByPerson"] = 0;
						}
						container.PrefetchCounter["AddedByPerson"]++;
					}
				}
				return this._AddedByPerson;
			}
			set
			{
				this._AddedByPerson = value;
			}
		}
	}

	public partial class DocumentCategoryList {
		internal int numFetchesOfAddedByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private DocumentCategoryList _DocumentCategoriesAddedBy;
		
		[JetBrains.Annotations.NotNull]
		public DocumentCategoryList DocumentCategoriesAddedBy
		{
			get
			{
				// lazy load
				if (this._DocumentCategoriesAddedBy == null) {
					this._DocumentCategoriesAddedBy = Models.DocumentCategoryList.LoadByAddedByPersonID(this.ID);
					this._DocumentCategoriesAddedBy.SetParentBindField(this, "AddedByPersonID");
				}
				return this._DocumentCategoriesAddedBy;
			}
			set
			{
				this._DocumentCategoriesAddedBy = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: ModifiedByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ModifiedByPersonID {
			get { return Fields.ModifiedByPersonID.Value; }
			set { fields["ModifiedByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentCategory LoadByModifiedByPersonID(int? modifiedByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentCategory>("ModifiedByPersonID", modifiedByPersonIDValue, "DocumentCategory", Otherwise.Null);
		}

		public partial class DocumentCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ModifiedByPersonID {
				get { return (ActiveField<int?>)fields["ModifiedByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentCategoryList LoadByModifiedByPersonID(int? modifiedByPersonIDValue) {
			var sql = new Sql("select * from ","DocumentCategory".SqlizeName()," where ModifiedByPersonID=", Sql.Sqlize(modifiedByPersonIDValue));
			return DocumentCategoryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ModifiedByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class DocumentCategory {
		[NonSerialized]		
		private Person _ModifiedByPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person ModifiedByPerson
		{
			get
			{
				 // lazy load
				if (this._ModifiedByPerson == null && this.ModifiedByPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ModifiedByPerson") && container.PrefetchCounter["ModifiedByPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.ModifiedByPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._ModifiedByPerson = Models.Person.LoadByPersonID(ModifiedByPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ModifiedByPerson")) {
							container.PrefetchCounter["ModifiedByPerson"] = 0;
						}
						container.PrefetchCounter["ModifiedByPerson"]++;
					}
				}
				return this._ModifiedByPerson;
			}
			set
			{
				this._ModifiedByPerson = value;
			}
		}
	}

	public partial class DocumentCategoryList {
		internal int numFetchesOfModifiedByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private DocumentCategoryList _DocumentCategoriesModifiedBy;
		
		[JetBrains.Annotations.NotNull]
		public DocumentCategoryList DocumentCategoriesModifiedBy
		{
			get
			{
				// lazy load
				if (this._DocumentCategoriesModifiedBy == null) {
					this._DocumentCategoriesModifiedBy = Models.DocumentCategoryList.LoadByModifiedByPersonID(this.ID);
					this._DocumentCategoriesModifiedBy.SetParentBindField(this, "ModifiedByPersonID");
				}
				return this._DocumentCategoriesModifiedBy;
			}
			set
			{
				this._DocumentCategoriesModifiedBy = value;
			}
		}
	}
	
}
#endregion