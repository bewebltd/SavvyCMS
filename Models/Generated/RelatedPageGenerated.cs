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
// CLASS: RelatedPage
// TABLE: RelatedPage
//-----------------------------------------


	public partial class RelatedPage : ActiveRecord {

		/// <summary>
		/// The list that this RelatedPage is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<RelatedPage> GetContainingList() {
			return (ActiveRecordList<RelatedPage>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public RelatedPage(): base("RelatedPage", "RelatedPageID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "RelatedPage";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "RelatedPageID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property RelatedPageID.
		/// </summary>
		public int ID { get { return (int)fields["RelatedPageID"].ValueObject; } set { fields["RelatedPageID"].ValueObject = value; } }

		// field references
		public partial class RelatedPageFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public RelatedPageFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private RelatedPageFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public RelatedPageFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new RelatedPageFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the RelatedPage table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of RelatedPage</param>
		/// <returns>An instance of RelatedPage containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the RelatedPage table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg RelatedPage.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = RelatedPage.LoadID(55);</example>
		/// <param name="id">Primary key of RelatedPage</param>
		/// <returns>An instance of RelatedPage containing the data in the record</returns>
		public static RelatedPage LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			RelatedPage record = null;
//			record = RelatedPage.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where RelatedPageID=", Sql.Sqlize(id));
//				record = new RelatedPage();
//				if (!record.LoadData(sql)) return otherwise.Execute<RelatedPage>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<RelatedPage>(id, "RelatedPage", otherwise);
		}

		/// <summary>
		/// Loads a record from the RelatedPage table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of RelatedPage containing the data in the record</returns>
		public static RelatedPage Load(Sql sql) {
				return ActiveRecordLoader.Load<RelatedPage>(sql, "RelatedPage");
		}
		public static RelatedPage Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<RelatedPage>(sql, "RelatedPage", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the RelatedPage table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of RelatedPage containing the data in the record</returns>
		public static RelatedPage Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<RelatedPage>(reader, "RelatedPage");
		}
		public static RelatedPage Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<RelatedPage>(reader, "RelatedPage", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where RelatedPageID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("RelatedPageID", new ActiveField<int>() { Name = "RelatedPageID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="RelatedPage"  });

	fields.Add("PageID", new ActiveField<int?>() { Name = "PageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="RelatedPage" , GetForeignRecord = () => this.Page, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("LinkedPageID", new ActiveField<int?>() { Name = "LinkedPageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="RelatedPage" , GetForeignRecord = () => this.LinkedPage, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="RelatedPage"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="RelatedPage"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="RelatedPage"  });
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
				var rec = ActiveRecordLoader.LoadID<RelatedPage>(id, "RelatedPage", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the RelatedPage with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct RelatedPage or null if not in cache.</returns>
//		private static RelatedPage GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-RelatedPage-" + id) as RelatedPage;
//			return Web.PageGlobals["ActiveRecord-RelatedPage-" + id] as RelatedPage;
//		}

		/// <summary>
		/// Caches this RelatedPage object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-RelatedPage-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-RelatedPage-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-RelatedPage-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of RelatedPage objects/records. This is the usual data structure for holding a number of RelatedPage records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class RelatedPageList : ActiveRecordList<RelatedPage> {

		public RelatedPageList() : base() {}
		public RelatedPageList(List<RelatedPage> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-RelatedPage to RelatedPageList. 
		/// </summary>
		public static implicit operator RelatedPageList(List<RelatedPage> list) {
			return new RelatedPageList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from RelatedPageList to List-of-RelatedPage. 
		/// </summary>
		public static implicit operator List<RelatedPage>(RelatedPageList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of RelatedPage objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of RelatedPage records.</returns>
		public static RelatedPageList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where RelatedPageID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of RelatedPage objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of RelatedPage records.</returns>
		public static RelatedPageList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static RelatedPageList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where RelatedPageID in (", ids.SqlizeNumberList(), ")");
			var result = new RelatedPageList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of RelatedPage objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of RelatedPage records.</returns>
		public static RelatedPageList Load(Sql sql) {
			var result = new RelatedPageList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all RelatedPage objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and RelatedPageID desc.)
		/// </summary>
		public static RelatedPageList LoadAll() {
			var result = new RelatedPageList();
			result.LoadRecords(null);
			return result;
		}
		public static RelatedPageList LoadAll(int itemsPerPage) {
			var result = new RelatedPageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static RelatedPageList LoadAll(int itemsPerPage, int pageNum) {
			var result = new RelatedPageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" RelatedPage objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static RelatedPageList LoadActive() {
			var result = new RelatedPageList();
			var sql = (new RelatedPage()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static RelatedPageList LoadActive(int itemsPerPage) {
			var result = new RelatedPageList();
			var sql = (new RelatedPage()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static RelatedPageList LoadActive(int itemsPerPage, int pageNum) {
			var result = new RelatedPageList();
			var sql = (new RelatedPage()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static RelatedPageList LoadActivePlusExisting(object existingRecordID) {
			var result = new RelatedPageList();
			var sql = (new RelatedPage()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM RelatedPage");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM RelatedPage");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new RelatedPage()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = RelatedPage.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: RelatedPageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class RelatedPage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int RelatedPageID {
			get { return Fields.RelatedPageID.Value; }
			set { fields["RelatedPageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadByRelatedPageID(int relatedPageIDValue) {
			return ActiveRecordLoader.LoadByField<RelatedPage>("RelatedPageID", relatedPageIDValue, "RelatedPage", Otherwise.Null);
		}

		public partial class RelatedPageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> RelatedPageID {
				get { return (ActiveField<int>)fields["RelatedPageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class RelatedPageList {		
				
		[JetBrains.Annotations.NotNull]
		public static RelatedPageList LoadByRelatedPageID(int relatedPageIDValue) {
			var sql = new Sql("select * from ","RelatedPage".SqlizeName()," where RelatedPageID=", Sql.Sqlize(relatedPageIDValue));
			return RelatedPageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class RelatedPage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PageID {
			get { return Fields.PageID.Value; }
			set { fields["PageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadByPageID(int? pageIDValue) {
			return ActiveRecordLoader.LoadByField<RelatedPage>("PageID", pageIDValue, "RelatedPage", Otherwise.Null);
		}

		public partial class RelatedPageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PageID {
				get { return (ActiveField<int?>)fields["PageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class RelatedPageList {		
				
		[JetBrains.Annotations.NotNull]
		public static RelatedPageList LoadByPageID(int? pageIDValue) {
			var sql = new Sql("select * from ","RelatedPage".SqlizeName()," where PageID=", Sql.Sqlize(pageIDValue));
			return RelatedPageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Page
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class RelatedPage {
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

	public partial class RelatedPageList {
		internal int numFetchesOfPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private RelatedPageList _RelatedPages;
		
		[JetBrains.Annotations.NotNull]
		public RelatedPageList RelatedPages
		{
			get
			{
				// lazy load
				if (this._RelatedPages == null) {
					this._RelatedPages = Models.RelatedPageList.LoadByPageID(this.ID);
					this._RelatedPages.SetParentBindField(this, "PageID");
				}
				return this._RelatedPages;
			}
			set
			{
				this._RelatedPages = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: LinkedPageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class RelatedPage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? LinkedPageID {
			get { return Fields.LinkedPageID.Value; }
			set { fields["LinkedPageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadByLinkedPageID(int? linkedPageIDValue) {
			return ActiveRecordLoader.LoadByField<RelatedPage>("LinkedPageID", linkedPageIDValue, "RelatedPage", Otherwise.Null);
		}

		public partial class RelatedPageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> LinkedPageID {
				get { return (ActiveField<int?>)fields["LinkedPageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class RelatedPageList {		
				
		[JetBrains.Annotations.NotNull]
		public static RelatedPageList LoadByLinkedPageID(int? linkedPageIDValue) {
			var sql = new Sql("select * from ","RelatedPage".SqlizeName()," where LinkedPageID=", Sql.Sqlize(linkedPageIDValue));
			return RelatedPageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: LinkedPage
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class RelatedPage {
		[NonSerialized]		
		private Page _LinkedPage;

		[JetBrains.Annotations.CanBeNull]
		public Page LinkedPage
		{
			get
			{
				 // lazy load
				if (this._LinkedPage == null && this.LinkedPageID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("LinkedPage") && container.PrefetchCounter["LinkedPage"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Page>("PageID",container.Select(r=>r.LinkedPageID).ToList(),"Page",Otherwise.Null);
					}
					this._LinkedPage = Models.Page.LoadByPageID(LinkedPageID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("LinkedPage")) {
							container.PrefetchCounter["LinkedPage"] = 0;
						}
						container.PrefetchCounter["LinkedPage"]++;
					}
				}
				return this._LinkedPage;
			}
			set
			{
				this._LinkedPage = value;
			}
		}
	}

	public partial class RelatedPageList {
		internal int numFetchesOfLinkedPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private RelatedPageList _RelatedPagesLinked;
		
		[JetBrains.Annotations.NotNull]
		public RelatedPageList RelatedPagesLinked
		{
			get
			{
				// lazy load
				if (this._RelatedPagesLinked == null) {
					this._RelatedPagesLinked = Models.RelatedPageList.LoadByLinkedPageID(this.ID);
					this._RelatedPagesLinked.SetParentBindField(this, "LinkedPageID");
				}
				return this._RelatedPagesLinked;
			}
			set
			{
				this._RelatedPagesLinked = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class RelatedPage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<RelatedPage>("SortPosition", sortPositionValue, "RelatedPage", Otherwise.Null);
		}

		public partial class RelatedPageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class RelatedPageList {		
				
		[JetBrains.Annotations.NotNull]
		public static RelatedPageList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","RelatedPage".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return RelatedPageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class RelatedPage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<RelatedPage>("DateAdded", dateAddedValue, "RelatedPage", Otherwise.Null);
		}

		public partial class RelatedPageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class RelatedPageList {		
				
		[JetBrains.Annotations.NotNull]
		public static RelatedPageList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","RelatedPage".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return RelatedPageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class RelatedPage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static RelatedPage LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<RelatedPage>("DateModified", dateModifiedValue, "RelatedPage", Otherwise.Null);
		}

		public partial class RelatedPageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class RelatedPageList {		
				
		[JetBrains.Annotations.NotNull]
		public static RelatedPageList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","RelatedPage".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return RelatedPageList.Load(sql);
		}		
		
	}


}
#endregion