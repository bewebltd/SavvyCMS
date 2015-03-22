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
// CLASS: Article
// TABLE: Article
//-----------------------------------------


	public partial class Article : ActiveRecord {

		/// <summary>
		/// The list that this Article is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Article> GetContainingList() {
			return (ActiveRecordList<Article>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Article(): base("Article", "ArticleID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Article";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ArticleID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ArticleID.
		/// </summary>
		public int ID { get { return (int)fields["ArticleID"].ValueObject; } set { fields["ArticleID"].ValueObject = value; } }

		// field references
		public partial class ArticleFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ArticleFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ArticleFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ArticleFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ArticleFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Article table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Article</param>
		/// <returns>An instance of Article containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Article LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Article table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Article.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Article.LoadID(55);</example>
		/// <param name="id">Primary key of Article</param>
		/// <returns>An instance of Article containing the data in the record</returns>
		public static Article LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Article record = null;
//			record = Article.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ArticleID=", Sql.Sqlize(id));
//				record = new Article();
//				if (!record.LoadData(sql)) return otherwise.Execute<Article>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Article>(id, "Article", otherwise);
		}

		/// <summary>
		/// Loads a record from the Article table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Article containing the data in the record</returns>
		public static Article Load(Sql sql) {
				return ActiveRecordLoader.Load<Article>(sql, "Article");
		}
		public static Article Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Article>(sql, "Article", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Article table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Article containing the data in the record</returns>
		public static Article Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Article>(reader, "Article");
		}
		public static Article Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Article>(reader, "Article", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ArticleID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ArticleID", new ActiveField<int>() { Name = "ArticleID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Article"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Article"  });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Article"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Article"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Article"  });

	fields.Add("PageID", new ActiveField<int?>() { Name = "PageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Article" , GetForeignRecord = () => this.Page, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Article"  });

	fields.Add("PhotoCaption", new ActiveField<string>() { Name = "PhotoCaption", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Article"  });

	fields.Add("YouTubeVideoID", new ActiveField<string>() { Name = "YouTubeVideoID", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="Article"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Article"  });

	fields.Add("MetaKeywords", new ActiveField<string>() { Name = "MetaKeywords", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Article"  });

	fields.Add("ShowArticleTitle", new ActiveField<bool>() { Name = "ShowArticleTitle", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Article"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Article"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Article"  });

	fields.Add("Template", new ActiveField<string>() { Name = "Template", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Article"  });

	fields.Add("ShowArticleAuthor", new ActiveField<bool>() { Name = "ShowArticleAuthor", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Article"  });

	fields.Add("ArticleDate", new ActiveField<System.DateTime?>() { Name = "ArticleDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Article"  });

	fields.Add("Author", new ActiveField<string>() { Name = "Author", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Article"  });

	fields.Add("ShowOnLatestArticles", new ActiveField<bool>() { Name = "ShowOnLatestArticles", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Article"  });
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
				var rec = ActiveRecordLoader.LoadID<Article>(id, "Article", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Article with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Article or null if not in cache.</returns>
//		private static Article GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Article-" + id) as Article;
//			return Web.PageGlobals["ActiveRecord-Article-" + id] as Article;
//		}

		/// <summary>
		/// Caches this Article object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Article-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Article-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Article-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Article objects/records. This is the usual data structure for holding a number of Article records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ArticleList : ActiveRecordList<Article> {

		public ArticleList() : base() {}
		public ArticleList(List<Article> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Article to ArticleList. 
		/// </summary>
		public static implicit operator ArticleList(List<Article> list) {
			return new ArticleList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ArticleList to List-of-Article. 
		/// </summary>
		public static implicit operator List<Article>(ArticleList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Article objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Article records.</returns>
		public static ArticleList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ArticleID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Article objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Article records.</returns>
		public static ArticleList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ArticleList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ArticleID in (", ids.SqlizeNumberList(), ")");
			var result = new ArticleList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Article objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Article records.</returns>
		public static ArticleList Load(Sql sql) {
			var result = new ArticleList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Article objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ArticleID desc.)
		/// </summary>
		public static ArticleList LoadAll() {
			var result = new ArticleList();
			result.LoadRecords(null);
			return result;
		}
		public static ArticleList LoadAll(int itemsPerPage) {
			var result = new ArticleList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ArticleList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ArticleList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Article objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ArticleList LoadActive() {
			var result = new ArticleList();
			var sql = (new Article()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ArticleList LoadActive(int itemsPerPage) {
			var result = new ArticleList();
			var sql = (new Article()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ArticleList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ArticleList();
			var sql = (new Article()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ArticleList LoadActivePlusExisting(object existingRecordID) {
			var result = new ArticleList();
			var sql = (new Article()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Article");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Article");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Article()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Article.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ArticleID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ArticleID {
			get { return Fields.ArticleID.Value; }
			set { fields["ArticleID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByArticleID(int articleIDValue) {
			return ActiveRecordLoader.LoadByField<Article>("ArticleID", articleIDValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ArticleID {
				get { return (ActiveField<int>)fields["ArticleID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByArticleID(int articleIDValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where ArticleID=", Sql.Sqlize(articleIDValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Article>("Title", titleValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<Article>("BodyTextHtml", bodyTextHtmlValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<Article>("PublishDate", publishDateValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<Article>("ExpiryDate", expiryDateValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PageID {
			get { return Fields.PageID.Value; }
			set { fields["PageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByPageID(int? pageIDValue) {
			return ActiveRecordLoader.LoadByField<Article>("PageID", pageIDValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PageID {
				get { return (ActiveField<int?>)fields["PageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByPageID(int? pageIDValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where PageID=", Sql.Sqlize(pageIDValue));
			return ArticleList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Page
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Article {
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

	public partial class ArticleList {
		internal int numFetchesOfPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private ArticleList _Articles;
		
		[JetBrains.Annotations.NotNull]
		public ArticleList Articles
		{
			get
			{
				// lazy load
				if (this._Articles == null) {
					this._Articles = Models.ArticleList.LoadByPageID(this.ID);
					this._Articles.SetParentBindField(this, "PageID");
				}
				return this._Articles;
			}
			set
			{
				this._Articles = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<Article>("Picture", pictureValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PhotoCaption
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PhotoCaption {
			get { return Fields.PhotoCaption.Value; }
			set { fields["PhotoCaption"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByPhotoCaption(string photoCaptionValue) {
			return ActiveRecordLoader.LoadByField<Article>("PhotoCaption", photoCaptionValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PhotoCaption {
				get { return (ActiveField<string>)fields["PhotoCaption"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByPhotoCaption(string photoCaptionValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where PhotoCaption=", Sql.Sqlize(photoCaptionValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: YouTubeVideoID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string YouTubeVideoID {
			get { return Fields.YouTubeVideoID.Value; }
			set { fields["YouTubeVideoID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByYouTubeVideoID(string youTubeVideoIDValue) {
			return ActiveRecordLoader.LoadByField<Article>("YouTubeVideoID", youTubeVideoIDValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> YouTubeVideoID {
				get { return (ActiveField<string>)fields["YouTubeVideoID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByYouTubeVideoID(string youTubeVideoIDValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where YouTubeVideoID=", Sql.Sqlize(youTubeVideoIDValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<Article>("SortPosition", sortPositionValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaKeywords
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaKeywords {
			get { return Fields.MetaKeywords.Value; }
			set { fields["MetaKeywords"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByMetaKeywords(string metaKeywordsValue) {
			return ActiveRecordLoader.LoadByField<Article>("MetaKeywords", metaKeywordsValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaKeywords {
				get { return (ActiveField<string>)fields["MetaKeywords"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByMetaKeywords(string metaKeywordsValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where MetaKeywords=", Sql.Sqlize(metaKeywordsValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowArticleTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowArticleTitle {
			get { return Fields.ShowArticleTitle.Value; }
			set { fields["ShowArticleTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByShowArticleTitle(bool showArticleTitleValue) {
			return ActiveRecordLoader.LoadByField<Article>("ShowArticleTitle", showArticleTitleValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowArticleTitle {
				get { return (ActiveField<bool>)fields["ShowArticleTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByShowArticleTitle(bool showArticleTitleValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where ShowArticleTitle=", Sql.Sqlize(showArticleTitleValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Article>("DateAdded", dateAddedValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Article>("DateModified", dateModifiedValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Template
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Template {
			get { return Fields.Template.Value; }
			set { fields["Template"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByTemplate(string templateValue) {
			return ActiveRecordLoader.LoadByField<Article>("Template", templateValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Template {
				get { return (ActiveField<string>)fields["Template"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByTemplate(string templateValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where Template=", Sql.Sqlize(templateValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowArticleAuthor
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowArticleAuthor {
			get { return Fields.ShowArticleAuthor.Value; }
			set { fields["ShowArticleAuthor"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByShowArticleAuthor(bool showArticleAuthorValue) {
			return ActiveRecordLoader.LoadByField<Article>("ShowArticleAuthor", showArticleAuthorValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowArticleAuthor {
				get { return (ActiveField<bool>)fields["ShowArticleAuthor"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByShowArticleAuthor(bool showArticleAuthorValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where ShowArticleAuthor=", Sql.Sqlize(showArticleAuthorValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ArticleDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ArticleDate {
			get { return Fields.ArticleDate.Value; }
			set { fields["ArticleDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByArticleDate(System.DateTime? articleDateValue) {
			return ActiveRecordLoader.LoadByField<Article>("ArticleDate", articleDateValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ArticleDate {
				get { return (ActiveField<System.DateTime?>)fields["ArticleDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByArticleDate(System.DateTime? articleDateValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where ArticleDate=", Sql.Sqlize(articleDateValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Author
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Author {
			get { return Fields.Author.Value; }
			set { fields["Author"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByAuthor(string authorValue) {
			return ActiveRecordLoader.LoadByField<Article>("Author", authorValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Author {
				get { return (ActiveField<string>)fields["Author"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByAuthor(string authorValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where Author=", Sql.Sqlize(authorValue));
			return ArticleList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowOnLatestArticles
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Article {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowOnLatestArticles {
			get { return Fields.ShowOnLatestArticles.Value; }
			set { fields["ShowOnLatestArticles"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Article LoadByShowOnLatestArticles(bool showOnLatestArticlesValue) {
			return ActiveRecordLoader.LoadByField<Article>("ShowOnLatestArticles", showOnLatestArticlesValue, "Article", Otherwise.Null);
		}

		public partial class ArticleFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowOnLatestArticles {
				get { return (ActiveField<bool>)fields["ShowOnLatestArticles"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleList LoadByShowOnLatestArticles(bool showOnLatestArticlesValue) {
			var sql = new Sql("select * from ","Article".SqlizeName()," where ShowOnLatestArticles=", Sql.Sqlize(showOnLatestArticlesValue));
			return ArticleList.Load(sql);
		}		
		
	}


}
#endregion