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
// CLASS: GalleryCategory
// TABLE: GalleryCategory
//-----------------------------------------


	public partial class GalleryCategory : ActiveRecord {

		/// <summary>
		/// The list that this GalleryCategory is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<GalleryCategory> GetContainingList() {
			return (ActiveRecordList<GalleryCategory>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public GalleryCategory(): base("GalleryCategory", "GalleryCategoryID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "GalleryCategory";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "GalleryCategoryID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property GalleryCategoryID.
		/// </summary>
		public int ID { get { return (int)fields["GalleryCategoryID"].ValueObject; } set { fields["GalleryCategoryID"].ValueObject = value; } }

		// field references
		public partial class GalleryCategoryFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public GalleryCategoryFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private GalleryCategoryFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public GalleryCategoryFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new GalleryCategoryFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the GalleryCategory table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of GalleryCategory</param>
		/// <returns>An instance of GalleryCategory containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the GalleryCategory table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg GalleryCategory.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = GalleryCategory.LoadID(55);</example>
		/// <param name="id">Primary key of GalleryCategory</param>
		/// <returns>An instance of GalleryCategory containing the data in the record</returns>
		public static GalleryCategory LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			GalleryCategory record = null;
//			record = GalleryCategory.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where GalleryCategoryID=", Sql.Sqlize(id));
//				record = new GalleryCategory();
//				if (!record.LoadData(sql)) return otherwise.Execute<GalleryCategory>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<GalleryCategory>(id, "GalleryCategory", otherwise);
		}

		/// <summary>
		/// Loads a record from the GalleryCategory table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of GalleryCategory containing the data in the record</returns>
		public static GalleryCategory Load(Sql sql) {
				return ActiveRecordLoader.Load<GalleryCategory>(sql, "GalleryCategory");
		}
		public static GalleryCategory Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GalleryCategory>(sql, "GalleryCategory", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the GalleryCategory table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of GalleryCategory containing the data in the record</returns>
		public static GalleryCategory Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<GalleryCategory>(reader, "GalleryCategory");
		}
		public static GalleryCategory Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GalleryCategory>(reader, "GalleryCategory", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where GalleryCategoryID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("GalleryCategoryID", new ActiveField<int>() { Name = "GalleryCategoryID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="GalleryCategory"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GalleryCategory"  });

	fields.Add("PageID", new ActiveField<int?>() { Name = "PageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GalleryCategory" , GetForeignRecord = () => this.Page, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="GalleryCategory"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryCategory"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryCategory"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryCategory"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryCategory"  });
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
				var rec = ActiveRecordLoader.LoadID<GalleryCategory>(id, "GalleryCategory", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the GalleryCategory with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct GalleryCategory or null if not in cache.</returns>
//		private static GalleryCategory GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-GalleryCategory-" + id) as GalleryCategory;
//			return Web.PageGlobals["ActiveRecord-GalleryCategory-" + id] as GalleryCategory;
//		}

		/// <summary>
		/// Caches this GalleryCategory object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-GalleryCategory-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-GalleryCategory-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-GalleryCategory-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of GalleryCategory objects/records. This is the usual data structure for holding a number of GalleryCategory records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class GalleryCategoryList : ActiveRecordList<GalleryCategory> {

		public GalleryCategoryList() : base() {}
		public GalleryCategoryList(List<GalleryCategory> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-GalleryCategory to GalleryCategoryList. 
		/// </summary>
		public static implicit operator GalleryCategoryList(List<GalleryCategory> list) {
			return new GalleryCategoryList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from GalleryCategoryList to List-of-GalleryCategory. 
		/// </summary>
		public static implicit operator List<GalleryCategory>(GalleryCategoryList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of GalleryCategory objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of GalleryCategory records.</returns>
		public static GalleryCategoryList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where GalleryCategoryID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of GalleryCategory objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of GalleryCategory records.</returns>
		public static GalleryCategoryList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static GalleryCategoryList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where GalleryCategoryID in (", ids.SqlizeNumberList(), ")");
			var result = new GalleryCategoryList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of GalleryCategory objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of GalleryCategory records.</returns>
		public static GalleryCategoryList Load(Sql sql) {
			var result = new GalleryCategoryList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all GalleryCategory objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and GalleryCategoryID desc.)
		/// </summary>
		public static GalleryCategoryList LoadAll() {
			var result = new GalleryCategoryList();
			result.LoadRecords(null);
			return result;
		}
		public static GalleryCategoryList LoadAll(int itemsPerPage) {
			var result = new GalleryCategoryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GalleryCategoryList LoadAll(int itemsPerPage, int pageNum) {
			var result = new GalleryCategoryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" GalleryCategory objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static GalleryCategoryList LoadActive() {
			var result = new GalleryCategoryList();
			var sql = (new GalleryCategory()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static GalleryCategoryList LoadActive(int itemsPerPage) {
			var result = new GalleryCategoryList();
			var sql = (new GalleryCategory()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GalleryCategoryList LoadActive(int itemsPerPage, int pageNum) {
			var result = new GalleryCategoryList();
			var sql = (new GalleryCategory()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static GalleryCategoryList LoadActivePlusExisting(object existingRecordID) {
			var result = new GalleryCategoryList();
			var sql = (new GalleryCategory()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM GalleryCategory");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM GalleryCategory");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new GalleryCategory()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = GalleryCategory.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: GalleryCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int GalleryCategoryID {
			get { return Fields.GalleryCategoryID.Value; }
			set { fields["GalleryCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByGalleryCategoryID(int galleryCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("GalleryCategoryID", galleryCategoryIDValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> GalleryCategoryID {
				get { return (ActiveField<int>)fields["GalleryCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByGalleryCategoryID(int galleryCategoryIDValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where GalleryCategoryID=", Sql.Sqlize(galleryCategoryIDValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("Title", titleValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PageID {
			get { return Fields.PageID.Value; }
			set { fields["PageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByPageID(int? pageIDValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("PageID", pageIDValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PageID {
				get { return (ActiveField<int?>)fields["PageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByPageID(int? pageIDValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where PageID=", Sql.Sqlize(pageIDValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Page
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class GalleryCategory {
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

	public partial class GalleryCategoryList {
		internal int numFetchesOfPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private GalleryCategoryList _GalleryCategories;
		
		[JetBrains.Annotations.NotNull]
		public GalleryCategoryList GalleryCategories
		{
			get
			{
				// lazy load
				if (this._GalleryCategories == null) {
					this._GalleryCategories = Models.GalleryCategoryList.LoadByPageID(this.ID);
					this._GalleryCategories.SetParentBindField(this, "PageID");
				}
				return this._GalleryCategories;
			}
			set
			{
				this._GalleryCategories = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("BodyTextHtml", bodyTextHtmlValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("PublishDate", publishDateValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("ExpiryDate", expiryDateValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("DateAdded", dateAddedValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryCategory LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<GalleryCategory>("DateModified", dateModifiedValue, "GalleryCategory", Otherwise.Null);
		}

		public partial class GalleryCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryCategoryList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","GalleryCategory".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return GalleryCategoryList.Load(sql);
		}		
		
	}


}
#endregion