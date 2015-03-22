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
// CLASS: News
// TABLE: News
//-----------------------------------------


	public partial class News : ActiveRecord {

		/// <summary>
		/// The list that this News is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<News> GetContainingList() {
			return (ActiveRecordList<News>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public News(): base("News", "NewsID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "News";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "NewsID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property NewsID.
		/// </summary>
		public int ID { get { return (int)fields["NewsID"].ValueObject; } set { fields["NewsID"].ValueObject = value; } }

		// field references
		public partial class NewsFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public NewsFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private NewsFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public NewsFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new NewsFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the News table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of News</param>
		/// <returns>An instance of News containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static News LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the News table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg News.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = News.LoadID(55);</example>
		/// <param name="id">Primary key of News</param>
		/// <returns>An instance of News containing the data in the record</returns>
		public static News LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			News record = null;
//			record = News.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where NewsID=", Sql.Sqlize(id));
//				record = new News();
//				if (!record.LoadData(sql)) return otherwise.Execute<News>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<News>(id, "News", otherwise);
		}

		/// <summary>
		/// Loads a record from the News table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of News containing the data in the record</returns>
		public static News Load(Sql sql) {
				return ActiveRecordLoader.Load<News>(sql, "News");
		}
		public static News Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<News>(sql, "News", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the News table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of News containing the data in the record</returns>
		public static News Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<News>(reader, "News");
		}
		public static News Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<News>(reader, "News", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where NewsID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("NewsID", new ActiveField<int>() { Name = "NewsID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="News"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="News"  });

	fields.Add("IntroductionText", new ActiveField<string>() { Name = "IntroductionText", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="News"  });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="News"  });

	fields.Add("Source", new ActiveField<string>() { Name = "Source", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="News"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="News"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="News"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="News"  });

	fields.Add("LinkUrl", new ActiveField<string>() { Name = "LinkUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="News"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="News"  });

	fields.Add("ArticleDate", new ActiveField<System.DateTime?>() { Name = "ArticleDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="News"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="News"  });

	fields.Add("LargePicture", new PictureActiveField() { Name = "LargePicture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="News"  });

	fields.Add("Attachment", new AttachmentActiveField() { Name = "Attachment", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="News"  });
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
				var rec = ActiveRecordLoader.LoadID<News>(id, "News", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the News with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct News or null if not in cache.</returns>
//		private static News GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-News-" + id) as News;
//			return Web.PageGlobals["ActiveRecord-News-" + id] as News;
//		}

		/// <summary>
		/// Caches this News object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-News-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-News-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-News-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of News objects/records. This is the usual data structure for holding a number of News records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class NewsList : ActiveRecordList<News> {

		public NewsList() : base() {}
		public NewsList(List<News> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-News to NewsList. 
		/// </summary>
		public static implicit operator NewsList(List<News> list) {
			return new NewsList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from NewsList to List-of-News. 
		/// </summary>
		public static implicit operator List<News>(NewsList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of News objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of News records.</returns>
		public static NewsList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where NewsID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of News objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of News records.</returns>
		public static NewsList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static NewsList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where NewsID in (", ids.SqlizeNumberList(), ")");
			var result = new NewsList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of News objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of News records.</returns>
		public static NewsList Load(Sql sql) {
			var result = new NewsList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all News objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and NewsID desc.)
		/// </summary>
		public static NewsList LoadAll() {
			var result = new NewsList();
			result.LoadRecords(null);
			return result;
		}
		public static NewsList LoadAll(int itemsPerPage) {
			var result = new NewsList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static NewsList LoadAll(int itemsPerPage, int pageNum) {
			var result = new NewsList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" News objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static NewsList LoadActive() {
			var result = new NewsList();
			var sql = (new News()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static NewsList LoadActive(int itemsPerPage) {
			var result = new NewsList();
			var sql = (new News()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static NewsList LoadActive(int itemsPerPage, int pageNum) {
			var result = new NewsList();
			var sql = (new News()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static NewsList LoadActivePlusExisting(object existingRecordID) {
			var result = new NewsList();
			var sql = (new News()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM News");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM News");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new News()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = News.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: NewsID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public int NewsID {
			get { return Fields.NewsID.Value; }
			set { fields["NewsID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByNewsID(int newsIDValue) {
			return ActiveRecordLoader.LoadByField<News>("NewsID", newsIDValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> NewsID {
				get { return (ActiveField<int>)fields["NewsID"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByNewsID(int newsIDValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where NewsID=", Sql.Sqlize(newsIDValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<News>("Title", titleValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IntroductionText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string IntroductionText {
			get { return Fields.IntroductionText.Value; }
			set { fields["IntroductionText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByIntroductionText(string introductionTextValue) {
			return ActiveRecordLoader.LoadByField<News>("IntroductionText", introductionTextValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> IntroductionText {
				get { return (ActiveField<string>)fields["IntroductionText"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByIntroductionText(string introductionTextValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where IntroductionText=", Sql.Sqlize(introductionTextValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<News>("BodyTextHtml", bodyTextHtmlValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Source
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Source {
			get { return Fields.Source.Value; }
			set { fields["Source"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadBySource(string sourceValue) {
			return ActiveRecordLoader.LoadByField<News>("Source", sourceValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Source {
				get { return (ActiveField<string>)fields["Source"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadBySource(string sourceValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where Source=", Sql.Sqlize(sourceValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<News>("PublishDate", publishDateValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<News>("ExpiryDate", expiryDateValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<News>("Picture", pictureValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkUrl {
			get { return Fields.LinkUrl.Value; }
			set { fields["LinkUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByLinkUrl(string linkUrlValue) {
			return ActiveRecordLoader.LoadByField<News>("LinkUrl", linkUrlValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkUrl {
				get { return (ActiveField<string>)fields["LinkUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByLinkUrl(string linkUrlValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where LinkUrl=", Sql.Sqlize(linkUrlValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<News>("DateAdded", dateAddedValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ArticleDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ArticleDate {
			get { return Fields.ArticleDate.Value; }
			set { fields["ArticleDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByArticleDate(System.DateTime? articleDateValue) {
			return ActiveRecordLoader.LoadByField<News>("ArticleDate", articleDateValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ArticleDate {
				get { return (ActiveField<System.DateTime?>)fields["ArticleDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByArticleDate(System.DateTime? articleDateValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where ArticleDate=", Sql.Sqlize(articleDateValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<News>("DateModified", dateModifiedValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LargePicture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LargePicture {
			get { return Fields.LargePicture.Value; }
			set { fields["LargePicture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByLargePicture(string largePictureValue) {
			return ActiveRecordLoader.LoadByField<News>("LargePicture", largePictureValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField LargePicture {
				get { return (PictureActiveField)fields["LargePicture"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByLargePicture(string largePictureValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where LargePicture=", Sql.Sqlize(largePictureValue));
			return NewsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Attachment
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class News {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Attachment {
			get { return Fields.Attachment.Value; }
			set { fields["Attachment"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static News LoadByAttachment(string attachmentValue) {
			return ActiveRecordLoader.LoadByField<News>("Attachment", attachmentValue, "News", Otherwise.Null);
		}

		public partial class NewsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField Attachment {
				get { return (AttachmentActiveField)fields["Attachment"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsList LoadByAttachment(string attachmentValue) {
			var sql = new Sql("select * from ","News".SqlizeName()," where Attachment=", Sql.Sqlize(attachmentValue));
			return NewsList.Load(sql);
		}		
		
	}


}
#endregion