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
// CLASS: Page
// TABLE: Page
//-----------------------------------------


	public partial class Page : ActiveRecord {

		/// <summary>
		/// The list that this Page is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Page> GetContainingList() {
			return (ActiveRecordList<Page>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Page(): base("Page", "PageID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Page";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "PageID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property PageID.
		/// </summary>
		public int ID { get { return (int)fields["PageID"].ValueObject; } set { fields["PageID"].ValueObject = value; } }

		// field references
		public partial class PageFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public PageFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private PageFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public PageFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new PageFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Page table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Page</param>
		/// <returns>An instance of Page containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Page LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Page table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Page.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Page.LoadID(55);</example>
		/// <param name="id">Primary key of Page</param>
		/// <returns>An instance of Page containing the data in the record</returns>
		public static Page LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Page record = null;
//			record = Page.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where PageID=", Sql.Sqlize(id));
//				record = new Page();
//				if (!record.LoadData(sql)) return otherwise.Execute<Page>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Page>(id, "Page", otherwise);
		}

		/// <summary>
		/// Loads a record from the Page table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Page containing the data in the record</returns>
		public static Page Load(Sql sql) {
				return ActiveRecordLoader.Load<Page>(sql, "Page");
		}
		public static Page Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Page>(sql, "Page", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Page table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Page containing the data in the record</returns>
		public static Page Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Page>(reader, "Page");
		}
		public static Page Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Page>(reader, "Page", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where PageID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("PageID", new ActiveField<int>() { Name = "PageID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Page"  });

	fields.Add("ParentPageID", new ActiveField<int?>() { Name = "ParentPageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Page" , GetForeignRecord = () => this.ParentPage, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("HistoryPageID", new ActiveField<int?>() { Name = "HistoryPageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Page" , GetForeignRecord = () => this.HistoryPage, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("PageCode", new ActiveField<string>() { Name = "PageCode", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=25, TableName="Page"  });

	fields.Add("CssClass", new ActiveField<string>() { Name = "CssClass", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Page"  });

	fields.Add("SubTitle", new ActiveField<string>() { Name = "SubTitle", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Page"  });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("ShowInMainNav", new ActiveField<bool>() { Name = "ShowInMainNav", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Page"  });

	fields.Add("ShowInSecondaryNav", new ActiveField<bool>() { Name = "ShowInSecondaryNav", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Page"  });

	fields.Add("ShowInFooterNav", new ActiveField<bool>() { Name = "ShowInFooterNav", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Page"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Page"  });

	fields.Add("PageIsALink", new ActiveField<bool>() { Name = "PageIsALink", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Page"  });

	fields.Add("LinkUrlIsExternal", new ActiveField<bool>() { Name = "LinkUrlIsExternal", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Page"  });

	fields.Add("LinkUrl", new ActiveField<string>() { Name = "LinkUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=350, TableName="Page"  });

	fields.Add("NavTitle", new ActiveField<string>() { Name = "NavTitle", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("URLRewriteTitle", new ActiveField<string>() { Name = "URLRewriteTitle", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("PageTitleTag", new ActiveField<string>() { Name = "PageTitleTag", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Page"  });

	fields.Add("MetaKeywords", new ActiveField<string>() { Name = "MetaKeywords", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Page"  });

	fields.Add("MetaDescription", new ActiveField<string>() { Name = "MetaDescription", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Page"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Page"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Page"  });

	fields.Add("TemplateCode", new ActiveField<string>() { Name = "TemplateCode", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("MoreTextHtml", new ActiveField<string>() { Name = "MoreTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("ShowInXMLSitemap", new ActiveField<bool>() { Name = "ShowInXMLSitemap", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Page"  });

	fields.Add("Introduction", new ActiveField<string>() { Name = "Introduction", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("PhotoCredit", new ActiveField<string>() { Name = "PhotoCredit", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("PhotoCaption", new ActiveField<string>() { Name = "PhotoCaption", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Page"  });

	fields.Add("SidebarPicture", new PictureActiveField() { Name = "SidebarPicture", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("SidebarTextHtml", new ActiveField<string>() { Name = "SidebarTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("SidebarTitle", new ActiveField<string>() { Name = "SidebarTitle", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("SidebarPhotoCredit", new ActiveField<string>() { Name = "SidebarPhotoCredit", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("SidebarPhotoCaption", new ActiveField<string>() { Name = "SidebarPhotoCaption", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Page"  });

	fields.Add("RolesAllowed", new ActiveField<string>() { Name = "RolesAllowed", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Page"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Page"  });

	fields.Add("TrackingTags", new ActiveField<string>() { Name = "TrackingTags", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="Page"  });

	fields.Add("FocusKeyword", new ActiveField<string>() { Name = "FocusKeyword", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Page"  });

	fields.Add("DisplayOrder", new ActiveField<string>() { Name = "DisplayOrder", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });

	fields.Add("AdminNotes", new ActiveField<string>() { Name = "AdminNotes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("HistoryChangeNotes", new ActiveField<string>() { Name = "HistoryChangeNotes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("ModifiedByPersonID", new ActiveField<int?>() { Name = "ModifiedByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Page" , GetForeignRecord = () => this.ModifiedByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("RequestApprovalForPersonID", new ActiveField<int?>() { Name = "RequestApprovalForPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Page" , GetForeignRecord = () => this.RequestApprovalForPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("EditorNotes", new ActiveField<string>() { Name = "EditorNotes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Page"  });

	fields.Add("RevisionStatus", new ActiveField<string>() { Name = "RevisionStatus", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Page"  });
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
				var rec = ActiveRecordLoader.LoadID<Page>(id, "Page", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Page with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Page or null if not in cache.</returns>
//		private static Page GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Page-" + id) as Page;
//			return Web.PageGlobals["ActiveRecord-Page-" + id] as Page;
//		}

		/// <summary>
		/// Caches this Page object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Page-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Page-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Page-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Page objects/records. This is the usual data structure for holding a number of Page records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class PageList : ActiveRecordList<Page> {

		public PageList() : base() {}
		public PageList(List<Page> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Page to PageList. 
		/// </summary>
		public static implicit operator PageList(List<Page> list) {
			return new PageList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from PageList to List-of-Page. 
		/// </summary>
		public static implicit operator List<Page>(PageList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Page objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Page records.</returns>
		public static PageList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where PageID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Page objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Page records.</returns>
		public static PageList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static PageList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where PageID in (", ids.SqlizeNumberList(), ")");
			var result = new PageList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Page objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Page records.</returns>
		public static PageList Load(Sql sql) {
			var result = new PageList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Page objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and PageID desc.)
		/// </summary>
		public static PageList LoadAll() {
			var result = new PageList();
			result.LoadRecords(null);
			return result;
		}
		public static PageList LoadAll(int itemsPerPage) {
			var result = new PageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static PageList LoadAll(int itemsPerPage, int pageNum) {
			var result = new PageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Page objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static PageList LoadActive() {
			var result = new PageList();
			var sql = (new Page()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static PageList LoadActive(int itemsPerPage) {
			var result = new PageList();
			var sql = (new Page()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static PageList LoadActive(int itemsPerPage, int pageNum) {
			var result = new PageList();
			var sql = (new Page()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static PageList LoadActivePlusExisting(object existingRecordID) {
			var result = new PageList();
			var sql = (new Page()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Page");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Page");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Page()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Page.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: PageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public int PageID {
			get { return Fields.PageID.Value; }
			set { fields["PageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPageID(int pageIDValue) {
			return ActiveRecordLoader.LoadByField<Page>("PageID", pageIDValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> PageID {
				get { return (ActiveField<int>)fields["PageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPageID(int pageIDValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PageID=", Sql.Sqlize(pageIDValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ParentPageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ParentPageID {
			get { return Fields.ParentPageID.Value; }
			set { fields["ParentPageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByParentPageID(int? parentPageIDValue) {
			return ActiveRecordLoader.LoadByField<Page>("ParentPageID", parentPageIDValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ParentPageID {
				get { return (ActiveField<int?>)fields["ParentPageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByParentPageID(int? parentPageIDValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ParentPageID=", Sql.Sqlize(parentPageIDValue));
			return PageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ParentPage
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Page {
		[NonSerialized]		
		private Page _ParentPage;

		[JetBrains.Annotations.CanBeNull]
		public Page ParentPage
		{
			get
			{
				 // lazy load
				if (this._ParentPage == null && this.ParentPageID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ParentPage") && container.PrefetchCounter["ParentPage"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Page>("PageID",container.Select(r=>r.ParentPageID).ToList(),"Page",Otherwise.Null);
					}
					this._ParentPage = Models.Page.LoadByPageID(ParentPageID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ParentPage")) {
							container.PrefetchCounter["ParentPage"] = 0;
						}
						container.PrefetchCounter["ParentPage"]++;
					}
				}
				return this._ParentPage;
			}
			set
			{
				this._ParentPage = value;
			}
		}
	}

	public partial class PageList {
		internal int numFetchesOfParentPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private PageList _ChildPages;
		
		[JetBrains.Annotations.NotNull]
		public PageList ChildPages
		{
			get
			{
				// lazy load
				if (this._ChildPages == null) {
					this._ChildPages = Models.PageList.LoadByParentPageID(this.ID);
					this._ChildPages.SetParentBindField(this, "ParentPageID");
				}
				return this._ChildPages;
			}
			set
			{
				this._ChildPages = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: HistoryPageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? HistoryPageID {
			get { return Fields.HistoryPageID.Value; }
			set { fields["HistoryPageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByHistoryPageID(int? historyPageIDValue) {
			return ActiveRecordLoader.LoadByField<Page>("HistoryPageID", historyPageIDValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> HistoryPageID {
				get { return (ActiveField<int?>)fields["HistoryPageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByHistoryPageID(int? historyPageIDValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where HistoryPageID=", Sql.Sqlize(historyPageIDValue));
			return PageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: HistoryPage
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Page {
		[NonSerialized]		
		private Page _HistoryPage;

		[JetBrains.Annotations.CanBeNull]
		public Page HistoryPage
		{
			get
			{
				 // lazy load
				if (this._HistoryPage == null && this.HistoryPageID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("HistoryPage") && container.PrefetchCounter["HistoryPage"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Page>("PageID",container.Select(r=>r.HistoryPageID).ToList(),"Page",Otherwise.Null);
					}
					this._HistoryPage = Models.Page.LoadByPageID(HistoryPageID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("HistoryPage")) {
							container.PrefetchCounter["HistoryPage"] = 0;
						}
						container.PrefetchCounter["HistoryPage"]++;
					}
				}
				return this._HistoryPage;
			}
			set
			{
				this._HistoryPage = value;
			}
		}
	}

	public partial class PageList {
		internal int numFetchesOfHistoryPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private PageList _PagesHistory;
		
		[JetBrains.Annotations.NotNull]
		public PageList PagesHistory
		{
			get
			{
				// lazy load
				if (this._PagesHistory == null) {
					this._PagesHistory = Models.PageList.LoadByHistoryPageID(this.ID);
					this._PagesHistory.SetParentBindField(this, "HistoryPageID");
				}
				return this._PagesHistory;
			}
			set
			{
				this._PagesHistory = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: PageCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageCode {
			get { return Fields.PageCode.Value; }
			set { fields["PageCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPageCode(string pageCodeValue) {
			return ActiveRecordLoader.LoadByField<Page>("PageCode", pageCodeValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageCode {
				get { return (ActiveField<string>)fields["PageCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPageCode(string pageCodeValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PageCode=", Sql.Sqlize(pageCodeValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CssClass
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CssClass {
			get { return Fields.CssClass.Value; }
			set { fields["CssClass"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByCssClass(string cssClassValue) {
			return ActiveRecordLoader.LoadByField<Page>("CssClass", cssClassValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CssClass {
				get { return (ActiveField<string>)fields["CssClass"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByCssClass(string cssClassValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where CssClass=", Sql.Sqlize(cssClassValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Page>("Title", titleValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SubTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SubTitle {
			get { return Fields.SubTitle.Value; }
			set { fields["SubTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySubTitle(string subTitleValue) {
			return ActiveRecordLoader.LoadByField<Page>("SubTitle", subTitleValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SubTitle {
				get { return (ActiveField<string>)fields["SubTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySubTitle(string subTitleValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SubTitle=", Sql.Sqlize(subTitleValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<Page>("BodyTextHtml", bodyTextHtmlValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowInMainNav
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowInMainNav {
			get { return Fields.ShowInMainNav.Value; }
			set { fields["ShowInMainNav"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByShowInMainNav(bool showInMainNavValue) {
			return ActiveRecordLoader.LoadByField<Page>("ShowInMainNav", showInMainNavValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowInMainNav {
				get { return (ActiveField<bool>)fields["ShowInMainNav"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByShowInMainNav(bool showInMainNavValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ShowInMainNav=", Sql.Sqlize(showInMainNavValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowInSecondaryNav
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowInSecondaryNav {
			get { return Fields.ShowInSecondaryNav.Value; }
			set { fields["ShowInSecondaryNav"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByShowInSecondaryNav(bool showInSecondaryNavValue) {
			return ActiveRecordLoader.LoadByField<Page>("ShowInSecondaryNav", showInSecondaryNavValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowInSecondaryNav {
				get { return (ActiveField<bool>)fields["ShowInSecondaryNav"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByShowInSecondaryNav(bool showInSecondaryNavValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ShowInSecondaryNav=", Sql.Sqlize(showInSecondaryNavValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowInFooterNav
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowInFooterNav {
			get { return Fields.ShowInFooterNav.Value; }
			set { fields["ShowInFooterNav"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByShowInFooterNav(bool showInFooterNavValue) {
			return ActiveRecordLoader.LoadByField<Page>("ShowInFooterNav", showInFooterNavValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowInFooterNav {
				get { return (ActiveField<bool>)fields["ShowInFooterNav"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByShowInFooterNav(bool showInFooterNavValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ShowInFooterNav=", Sql.Sqlize(showInFooterNavValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<Page>("SortPosition", sortPositionValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageIsALink
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool PageIsALink {
			get { return Fields.PageIsALink.Value; }
			set { fields["PageIsALink"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPageIsALink(bool pageIsALinkValue) {
			return ActiveRecordLoader.LoadByField<Page>("PageIsALink", pageIsALinkValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> PageIsALink {
				get { return (ActiveField<bool>)fields["PageIsALink"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPageIsALink(bool pageIsALinkValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PageIsALink=", Sql.Sqlize(pageIsALinkValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkUrlIsExternal
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool LinkUrlIsExternal {
			get { return Fields.LinkUrlIsExternal.Value; }
			set { fields["LinkUrlIsExternal"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByLinkUrlIsExternal(bool linkUrlIsExternalValue) {
			return ActiveRecordLoader.LoadByField<Page>("LinkUrlIsExternal", linkUrlIsExternalValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> LinkUrlIsExternal {
				get { return (ActiveField<bool>)fields["LinkUrlIsExternal"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByLinkUrlIsExternal(bool linkUrlIsExternalValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where LinkUrlIsExternal=", Sql.Sqlize(linkUrlIsExternalValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkUrl {
			get { return Fields.LinkUrl.Value; }
			set { fields["LinkUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByLinkUrl(string linkUrlValue) {
			return ActiveRecordLoader.LoadByField<Page>("LinkUrl", linkUrlValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkUrl {
				get { return (ActiveField<string>)fields["LinkUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByLinkUrl(string linkUrlValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where LinkUrl=", Sql.Sqlize(linkUrlValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: NavTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string NavTitle {
			get { return Fields.NavTitle.Value; }
			set { fields["NavTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByNavTitle(string navTitleValue) {
			return ActiveRecordLoader.LoadByField<Page>("NavTitle", navTitleValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> NavTitle {
				get { return (ActiveField<string>)fields["NavTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByNavTitle(string navTitleValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where NavTitle=", Sql.Sqlize(navTitleValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: URLRewriteTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string URLRewriteTitle {
			get { return Fields.URLRewriteTitle.Value; }
			set { fields["URLRewriteTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByURLRewriteTitle(string uRLRewriteTitleValue) {
			return ActiveRecordLoader.LoadByField<Page>("URLRewriteTitle", uRLRewriteTitleValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> URLRewriteTitle {
				get { return (ActiveField<string>)fields["URLRewriteTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByURLRewriteTitle(string uRLRewriteTitleValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where URLRewriteTitle=", Sql.Sqlize(uRLRewriteTitleValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageTitleTag
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageTitleTag {
			get { return Fields.PageTitleTag.Value; }
			set { fields["PageTitleTag"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPageTitleTag(string pageTitleTagValue) {
			return ActiveRecordLoader.LoadByField<Page>("PageTitleTag", pageTitleTagValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageTitleTag {
				get { return (ActiveField<string>)fields["PageTitleTag"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPageTitleTag(string pageTitleTagValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PageTitleTag=", Sql.Sqlize(pageTitleTagValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaKeywords
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaKeywords {
			get { return Fields.MetaKeywords.Value; }
			set { fields["MetaKeywords"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByMetaKeywords(string metaKeywordsValue) {
			return ActiveRecordLoader.LoadByField<Page>("MetaKeywords", metaKeywordsValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaKeywords {
				get { return (ActiveField<string>)fields["MetaKeywords"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByMetaKeywords(string metaKeywordsValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where MetaKeywords=", Sql.Sqlize(metaKeywordsValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaDescription {
			get { return Fields.MetaDescription.Value; }
			set { fields["MetaDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByMetaDescription(string metaDescriptionValue) {
			return ActiveRecordLoader.LoadByField<Page>("MetaDescription", metaDescriptionValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaDescription {
				get { return (ActiveField<string>)fields["MetaDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByMetaDescription(string metaDescriptionValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where MetaDescription=", Sql.Sqlize(metaDescriptionValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Page>("DateAdded", dateAddedValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<Page>("PublishDate", publishDateValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<Page>("ExpiryDate", expiryDateValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TemplateCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TemplateCode {
			get { return Fields.TemplateCode.Value; }
			set { fields["TemplateCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByTemplateCode(string templateCodeValue) {
			return ActiveRecordLoader.LoadByField<Page>("TemplateCode", templateCodeValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TemplateCode {
				get { return (ActiveField<string>)fields["TemplateCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByTemplateCode(string templateCodeValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where TemplateCode=", Sql.Sqlize(templateCodeValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<Page>("Picture", pictureValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MoreTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MoreTextHtml {
			get { return Fields.MoreTextHtml.Value; }
			set { fields["MoreTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByMoreTextHtml(string moreTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<Page>("MoreTextHtml", moreTextHtmlValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MoreTextHtml {
				get { return (ActiveField<string>)fields["MoreTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByMoreTextHtml(string moreTextHtmlValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where MoreTextHtml=", Sql.Sqlize(moreTextHtmlValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowInXMLSitemap
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowInXMLSitemap {
			get { return Fields.ShowInXMLSitemap.Value; }
			set { fields["ShowInXMLSitemap"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByShowInXMLSitemap(bool showInXMLSitemapValue) {
			return ActiveRecordLoader.LoadByField<Page>("ShowInXMLSitemap", showInXMLSitemapValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowInXMLSitemap {
				get { return (ActiveField<bool>)fields["ShowInXMLSitemap"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByShowInXMLSitemap(bool showInXMLSitemapValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ShowInXMLSitemap=", Sql.Sqlize(showInXMLSitemapValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Introduction
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Introduction {
			get { return Fields.Introduction.Value; }
			set { fields["Introduction"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByIntroduction(string introductionValue) {
			return ActiveRecordLoader.LoadByField<Page>("Introduction", introductionValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Introduction {
				get { return (ActiveField<string>)fields["Introduction"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByIntroduction(string introductionValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where Introduction=", Sql.Sqlize(introductionValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PhotoCredit
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PhotoCredit {
			get { return Fields.PhotoCredit.Value; }
			set { fields["PhotoCredit"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPhotoCredit(string photoCreditValue) {
			return ActiveRecordLoader.LoadByField<Page>("PhotoCredit", photoCreditValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PhotoCredit {
				get { return (ActiveField<string>)fields["PhotoCredit"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPhotoCredit(string photoCreditValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PhotoCredit=", Sql.Sqlize(photoCreditValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PhotoCaption
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PhotoCaption {
			get { return Fields.PhotoCaption.Value; }
			set { fields["PhotoCaption"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByPhotoCaption(string photoCaptionValue) {
			return ActiveRecordLoader.LoadByField<Page>("PhotoCaption", photoCaptionValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PhotoCaption {
				get { return (ActiveField<string>)fields["PhotoCaption"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByPhotoCaption(string photoCaptionValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where PhotoCaption=", Sql.Sqlize(photoCaptionValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SidebarPicture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SidebarPicture {
			get { return Fields.SidebarPicture.Value; }
			set { fields["SidebarPicture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySidebarPicture(string sidebarPictureValue) {
			return ActiveRecordLoader.LoadByField<Page>("SidebarPicture", sidebarPictureValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField SidebarPicture {
				get { return (PictureActiveField)fields["SidebarPicture"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySidebarPicture(string sidebarPictureValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SidebarPicture=", Sql.Sqlize(sidebarPictureValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SidebarTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SidebarTextHtml {
			get { return Fields.SidebarTextHtml.Value; }
			set { fields["SidebarTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySidebarTextHtml(string sidebarTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<Page>("SidebarTextHtml", sidebarTextHtmlValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SidebarTextHtml {
				get { return (ActiveField<string>)fields["SidebarTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySidebarTextHtml(string sidebarTextHtmlValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SidebarTextHtml=", Sql.Sqlize(sidebarTextHtmlValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SidebarTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SidebarTitle {
			get { return Fields.SidebarTitle.Value; }
			set { fields["SidebarTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySidebarTitle(string sidebarTitleValue) {
			return ActiveRecordLoader.LoadByField<Page>("SidebarTitle", sidebarTitleValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SidebarTitle {
				get { return (ActiveField<string>)fields["SidebarTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySidebarTitle(string sidebarTitleValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SidebarTitle=", Sql.Sqlize(sidebarTitleValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SidebarPhotoCredit
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SidebarPhotoCredit {
			get { return Fields.SidebarPhotoCredit.Value; }
			set { fields["SidebarPhotoCredit"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySidebarPhotoCredit(string sidebarPhotoCreditValue) {
			return ActiveRecordLoader.LoadByField<Page>("SidebarPhotoCredit", sidebarPhotoCreditValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SidebarPhotoCredit {
				get { return (ActiveField<string>)fields["SidebarPhotoCredit"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySidebarPhotoCredit(string sidebarPhotoCreditValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SidebarPhotoCredit=", Sql.Sqlize(sidebarPhotoCreditValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SidebarPhotoCaption
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SidebarPhotoCaption {
			get { return Fields.SidebarPhotoCaption.Value; }
			set { fields["SidebarPhotoCaption"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadBySidebarPhotoCaption(string sidebarPhotoCaptionValue) {
			return ActiveRecordLoader.LoadByField<Page>("SidebarPhotoCaption", sidebarPhotoCaptionValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SidebarPhotoCaption {
				get { return (ActiveField<string>)fields["SidebarPhotoCaption"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadBySidebarPhotoCaption(string sidebarPhotoCaptionValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where SidebarPhotoCaption=", Sql.Sqlize(sidebarPhotoCaptionValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RolesAllowed
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string RolesAllowed {
			get { return Fields.RolesAllowed.Value; }
			set { fields["RolesAllowed"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByRolesAllowed(string rolesAllowedValue) {
			return ActiveRecordLoader.LoadByField<Page>("RolesAllowed", rolesAllowedValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> RolesAllowed {
				get { return (ActiveField<string>)fields["RolesAllowed"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByRolesAllowed(string rolesAllowedValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where RolesAllowed=", Sql.Sqlize(rolesAllowedValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Page>("DateModified", dateModifiedValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TrackingTags
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TrackingTags {
			get { return Fields.TrackingTags.Value; }
			set { fields["TrackingTags"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByTrackingTags(string trackingTagsValue) {
			return ActiveRecordLoader.LoadByField<Page>("TrackingTags", trackingTagsValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TrackingTags {
				get { return (ActiveField<string>)fields["TrackingTags"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByTrackingTags(string trackingTagsValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where TrackingTags=", Sql.Sqlize(trackingTagsValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FocusKeyword
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FocusKeyword {
			get { return Fields.FocusKeyword.Value; }
			set { fields["FocusKeyword"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByFocusKeyword(string focusKeywordValue) {
			return ActiveRecordLoader.LoadByField<Page>("FocusKeyword", focusKeywordValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FocusKeyword {
				get { return (ActiveField<string>)fields["FocusKeyword"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByFocusKeyword(string focusKeywordValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where FocusKeyword=", Sql.Sqlize(focusKeywordValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DisplayOrder
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string DisplayOrder {
			get { return Fields.DisplayOrder.Value; }
			set { fields["DisplayOrder"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByDisplayOrder(string displayOrderValue) {
			return ActiveRecordLoader.LoadByField<Page>("DisplayOrder", displayOrderValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> DisplayOrder {
				get { return (ActiveField<string>)fields["DisplayOrder"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByDisplayOrder(string displayOrderValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where DisplayOrder=", Sql.Sqlize(displayOrderValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AdminNotes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AdminNotes {
			get { return Fields.AdminNotes.Value; }
			set { fields["AdminNotes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByAdminNotes(string adminNotesValue) {
			return ActiveRecordLoader.LoadByField<Page>("AdminNotes", adminNotesValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AdminNotes {
				get { return (ActiveField<string>)fields["AdminNotes"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByAdminNotes(string adminNotesValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where AdminNotes=", Sql.Sqlize(adminNotesValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: HistoryChangeNotes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string HistoryChangeNotes {
			get { return Fields.HistoryChangeNotes.Value; }
			set { fields["HistoryChangeNotes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByHistoryChangeNotes(string historyChangeNotesValue) {
			return ActiveRecordLoader.LoadByField<Page>("HistoryChangeNotes", historyChangeNotesValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> HistoryChangeNotes {
				get { return (ActiveField<string>)fields["HistoryChangeNotes"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByHistoryChangeNotes(string historyChangeNotesValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where HistoryChangeNotes=", Sql.Sqlize(historyChangeNotesValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ModifiedByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ModifiedByPersonID {
			get { return Fields.ModifiedByPersonID.Value; }
			set { fields["ModifiedByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByModifiedByPersonID(int? modifiedByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<Page>("ModifiedByPersonID", modifiedByPersonIDValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ModifiedByPersonID {
				get { return (ActiveField<int?>)fields["ModifiedByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByModifiedByPersonID(int? modifiedByPersonIDValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where ModifiedByPersonID=", Sql.Sqlize(modifiedByPersonIDValue));
			return PageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ModifiedByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Page {
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

	public partial class PageList {
		internal int numFetchesOfModifiedByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private PageList _PagesModifiedBy;
		
		[JetBrains.Annotations.NotNull]
		public PageList PagesModifiedBy
		{
			get
			{
				// lazy load
				if (this._PagesModifiedBy == null) {
					this._PagesModifiedBy = Models.PageList.LoadByModifiedByPersonID(this.ID);
					this._PagesModifiedBy.SetParentBindField(this, "ModifiedByPersonID");
				}
				return this._PagesModifiedBy;
			}
			set
			{
				this._PagesModifiedBy = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: RequestApprovalForPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? RequestApprovalForPersonID {
			get { return Fields.RequestApprovalForPersonID.Value; }
			set { fields["RequestApprovalForPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByRequestApprovalForPersonID(int? requestApprovalForPersonIDValue) {
			return ActiveRecordLoader.LoadByField<Page>("RequestApprovalForPersonID", requestApprovalForPersonIDValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> RequestApprovalForPersonID {
				get { return (ActiveField<int?>)fields["RequestApprovalForPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByRequestApprovalForPersonID(int? requestApprovalForPersonIDValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where RequestApprovalForPersonID=", Sql.Sqlize(requestApprovalForPersonIDValue));
			return PageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: RequestApprovalForPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Page {
		[NonSerialized]		
		private Person _RequestApprovalForPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person RequestApprovalForPerson
		{
			get
			{
				 // lazy load
				if (this._RequestApprovalForPerson == null && this.RequestApprovalForPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("RequestApprovalForPerson") && container.PrefetchCounter["RequestApprovalForPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.RequestApprovalForPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._RequestApprovalForPerson = Models.Person.LoadByPersonID(RequestApprovalForPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("RequestApprovalForPerson")) {
							container.PrefetchCounter["RequestApprovalForPerson"] = 0;
						}
						container.PrefetchCounter["RequestApprovalForPerson"]++;
					}
				}
				return this._RequestApprovalForPerson;
			}
			set
			{
				this._RequestApprovalForPerson = value;
			}
		}
	}

	public partial class PageList {
		internal int numFetchesOfRequestApprovalForPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private PageList _PagesRequestApprovalFor;
		
		[JetBrains.Annotations.NotNull]
		public PageList PagesRequestApprovalFor
		{
			get
			{
				// lazy load
				if (this._PagesRequestApprovalFor == null) {
					this._PagesRequestApprovalFor = Models.PageList.LoadByRequestApprovalForPersonID(this.ID);
					this._PagesRequestApprovalFor.SetParentBindField(this, "RequestApprovalForPersonID");
				}
				return this._PagesRequestApprovalFor;
			}
			set
			{
				this._PagesRequestApprovalFor = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: EditorNotes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EditorNotes {
			get { return Fields.EditorNotes.Value; }
			set { fields["EditorNotes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByEditorNotes(string editorNotesValue) {
			return ActiveRecordLoader.LoadByField<Page>("EditorNotes", editorNotesValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EditorNotes {
				get { return (ActiveField<string>)fields["EditorNotes"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByEditorNotes(string editorNotesValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where EditorNotes=", Sql.Sqlize(editorNotesValue));
			return PageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RevisionStatus
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Page {		
				
		[JetBrains.Annotations.CanBeNull]
		public string RevisionStatus {
			get { return Fields.RevisionStatus.Value; }
			set { fields["RevisionStatus"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Page LoadByRevisionStatus(string revisionStatusValue) {
			return ActiveRecordLoader.LoadByField<Page>("RevisionStatus", revisionStatusValue, "Page", Otherwise.Null);
		}

		public partial class PageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> RevisionStatus {
				get { return (ActiveField<string>)fields["RevisionStatus"]; }
			}
		}

	}
	
	// define list class 
	public partial class PageList {		
				
		[JetBrains.Annotations.NotNull]
		public static PageList LoadByRevisionStatus(string revisionStatusValue) {
			var sql = new Sql("select * from ","Page".SqlizeName()," where RevisionStatus=", Sql.Sqlize(revisionStatusValue));
			return PageList.Load(sql);
		}		
		
	}


}
#endregion