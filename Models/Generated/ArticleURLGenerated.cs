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
// CLASS: ArticleURL
// TABLE: ArticleURL
//-----------------------------------------


	public partial class ArticleURL : ActiveRecord {

		/// <summary>
		/// The list that this ArticleURL is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ArticleURL> GetContainingList() {
			return (ActiveRecordList<ArticleURL>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ArticleURL(): base("ArticleURL", "ArticleURLID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ArticleURL";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ArticleURLID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ArticleURLID.
		/// </summary>
		public int ID { get { return (int)fields["ArticleURLID"].ValueObject; } set { fields["ArticleURLID"].ValueObject = value; } }

		// field references
		public partial class ArticleURLFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ArticleURLFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ArticleURLFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ArticleURLFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ArticleURLFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ArticleURL table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ArticleURL</param>
		/// <returns>An instance of ArticleURL containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ArticleURL table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ArticleURL.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ArticleURL.LoadID(55);</example>
		/// <param name="id">Primary key of ArticleURL</param>
		/// <returns>An instance of ArticleURL containing the data in the record</returns>
		public static ArticleURL LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ArticleURL record = null;
//			record = ArticleURL.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ArticleURLID=", Sql.Sqlize(id));
//				record = new ArticleURL();
//				if (!record.LoadData(sql)) return otherwise.Execute<ArticleURL>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ArticleURL>(id, "ArticleURL", otherwise);
		}

		/// <summary>
		/// Loads a record from the ArticleURL table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ArticleURL containing the data in the record</returns>
		public static ArticleURL Load(Sql sql) {
				return ActiveRecordLoader.Load<ArticleURL>(sql, "ArticleURL");
		}
		public static ArticleURL Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ArticleURL>(sql, "ArticleURL", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ArticleURL table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ArticleURL containing the data in the record</returns>
		public static ArticleURL Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ArticleURL>(reader, "ArticleURL");
		}
		public static ArticleURL Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ArticleURL>(reader, "ArticleURL", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ArticleURLID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ArticleURLID", new ActiveField<int>() { Name = "ArticleURLID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ArticleURL"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ArticleURL"  });

	fields.Add("Description", new ActiveField<string>() { Name = "Description", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ArticleURL"  });

	fields.Add("URLLink", new ActiveField<string>() { Name = "URLLink", ColumnType = "nchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ArticleURL"  });

	fields.Add("ArticleID", new ActiveField<int?>() { Name = "ArticleID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ArticleURL" , GetForeignRecord = () => this.Article, ForeignClassName = typeof(Models.Article), ForeignTableName = "Article", ForeignTableFieldName = "ArticleID" });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ArticleURL"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ArticleURL"  });

	fields.Add("IsNewWindow", new ActiveField<bool>() { Name = "IsNewWindow", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="ArticleURL"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ArticleURL"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ArticleURL"  });
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
				var rec = ActiveRecordLoader.LoadID<ArticleURL>(id, "ArticleURL", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ArticleURL with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ArticleURL or null if not in cache.</returns>
//		private static ArticleURL GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ArticleURL-" + id) as ArticleURL;
//			return Web.PageGlobals["ActiveRecord-ArticleURL-" + id] as ArticleURL;
//		}

		/// <summary>
		/// Caches this ArticleURL object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ArticleURL-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ArticleURL-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ArticleURL-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ArticleURL objects/records. This is the usual data structure for holding a number of ArticleURL records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ArticleURLList : ActiveRecordList<ArticleURL> {

		public ArticleURLList() : base() {}
		public ArticleURLList(List<ArticleURL> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ArticleURL to ArticleURLList. 
		/// </summary>
		public static implicit operator ArticleURLList(List<ArticleURL> list) {
			return new ArticleURLList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ArticleURLList to List-of-ArticleURL. 
		/// </summary>
		public static implicit operator List<ArticleURL>(ArticleURLList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ArticleURL objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ArticleURL records.</returns>
		public static ArticleURLList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ArticleURLID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ArticleURL objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ArticleURL records.</returns>
		public static ArticleURLList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ArticleURLList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ArticleURLID in (", ids.SqlizeNumberList(), ")");
			var result = new ArticleURLList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ArticleURL objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ArticleURL records.</returns>
		public static ArticleURLList Load(Sql sql) {
			var result = new ArticleURLList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ArticleURL objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ArticleURLID desc.)
		/// </summary>
		public static ArticleURLList LoadAll() {
			var result = new ArticleURLList();
			result.LoadRecords(null);
			return result;
		}
		public static ArticleURLList LoadAll(int itemsPerPage) {
			var result = new ArticleURLList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ArticleURLList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ArticleURLList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ArticleURL objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ArticleURLList LoadActive() {
			var result = new ArticleURLList();
			var sql = (new ArticleURL()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ArticleURLList LoadActive(int itemsPerPage) {
			var result = new ArticleURLList();
			var sql = (new ArticleURL()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ArticleURLList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ArticleURLList();
			var sql = (new ArticleURL()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ArticleURLList LoadActivePlusExisting(object existingRecordID) {
			var result = new ArticleURLList();
			var sql = (new ArticleURL()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ArticleURL");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ArticleURL");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ArticleURL()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ArticleURL.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ArticleURLID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ArticleURLID {
			get { return Fields.ArticleURLID.Value; }
			set { fields["ArticleURLID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByArticleURLID(int articleURLIDValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("ArticleURLID", articleURLIDValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ArticleURLID {
				get { return (ActiveField<int>)fields["ArticleURLID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByArticleURLID(int articleURLIDValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where ArticleURLID=", Sql.Sqlize(articleURLIDValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("Title", titleValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Description
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Description {
			get { return Fields.Description.Value; }
			set { fields["Description"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByDescription(string descriptionValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("Description", descriptionValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Description {
				get { return (ActiveField<string>)fields["Description"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByDescription(string descriptionValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where Description=", Sql.Sqlize(descriptionValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: URLLink
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public string URLLink {
			get { return Fields.URLLink.Value; }
			set { fields["URLLink"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByURLLink(string uRLLinkValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("URLLink", uRLLinkValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> URLLink {
				get { return (ActiveField<string>)fields["URLLink"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByURLLink(string uRLLinkValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where URLLink=", Sql.Sqlize(uRLLinkValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ArticleID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ArticleID {
			get { return Fields.ArticleID.Value; }
			set { fields["ArticleID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByArticleID(int? articleIDValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("ArticleID", articleIDValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ArticleID {
				get { return (ActiveField<int?>)fields["ArticleID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByArticleID(int? articleIDValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where ArticleID=", Sql.Sqlize(articleIDValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Article
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ArticleURL {
		[NonSerialized]		
		private Article _Article;

		[JetBrains.Annotations.CanBeNull]
		public Article Article
		{
			get
			{
				 // lazy load
				if (this._Article == null && this.ArticleID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Article") && container.PrefetchCounter["Article"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Article>("ArticleID",container.Select(r=>r.ArticleID).ToList(),"Article",Otherwise.Null);
					}
					this._Article = Models.Article.LoadByArticleID(ArticleID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Article")) {
							container.PrefetchCounter["Article"] = 0;
						}
						container.PrefetchCounter["Article"]++;
					}
				}
				return this._Article;
			}
			set
			{
				this._Article = value;
			}
		}
	}

	public partial class ArticleURLList {
		internal int numFetchesOfArticle = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Article {
		[NonSerialized]		
		private ArticleURLList _ArticleURLs;
		
		[JetBrains.Annotations.NotNull]
		public ArticleURLList ArticleURLs
		{
			get
			{
				// lazy load
				if (this._ArticleURLs == null) {
					this._ArticleURLs = Models.ArticleURLList.LoadByArticleID(this.ID);
					this._ArticleURLs.SetParentBindField(this, "ArticleID");
				}
				return this._ArticleURLs;
			}
			set
			{
				this._ArticleURLs = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("PublishDate", publishDateValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("ExpiryDate", expiryDateValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsNewWindow
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsNewWindow {
			get { return Fields.IsNewWindow.Value; }
			set { fields["IsNewWindow"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByIsNewWindow(bool isNewWindowValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("IsNewWindow", isNewWindowValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsNewWindow {
				get { return (ActiveField<bool>)fields["IsNewWindow"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByIsNewWindow(bool isNewWindowValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where IsNewWindow=", Sql.Sqlize(isNewWindowValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("DateAdded", dateAddedValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ArticleURL {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ArticleURL LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ArticleURL>("DateModified", dateModifiedValue, "ArticleURL", Otherwise.Null);
		}

		public partial class ArticleURLFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ArticleURLList {		
				
		[JetBrains.Annotations.NotNull]
		public static ArticleURLList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ArticleURL".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ArticleURLList.Load(sql);
		}		
		
	}


}
#endregion