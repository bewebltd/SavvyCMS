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
// CLASS: NewsRSS
// TABLE: NewsRSS
//-----------------------------------------


	public partial class NewsRSS : ActiveRecord {

		/// <summary>
		/// The list that this NewsRSS is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<NewsRSS> GetContainingList() {
			return (ActiveRecordList<NewsRSS>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public NewsRSS(): base("NewsRSS", "NewsRSSID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "NewsRSS";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "NewsRSSID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property NewsRSSID.
		/// </summary>
		public int ID { get { return (int)fields["NewsRSSID"].ValueObject; } set { fields["NewsRSSID"].ValueObject = value; } }

		// field references
		public partial class NewsRSSFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public NewsRSSFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private NewsRSSFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public NewsRSSFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new NewsRSSFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the NewsRSS table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of NewsRSS</param>
		/// <returns>An instance of NewsRSS containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the NewsRSS table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg NewsRSS.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = NewsRSS.LoadID(55);</example>
		/// <param name="id">Primary key of NewsRSS</param>
		/// <returns>An instance of NewsRSS containing the data in the record</returns>
		public static NewsRSS LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			NewsRSS record = null;
//			record = NewsRSS.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where NewsRSSID=", Sql.Sqlize(id));
//				record = new NewsRSS();
//				if (!record.LoadData(sql)) return otherwise.Execute<NewsRSS>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<NewsRSS>(id, "NewsRSS", otherwise);
		}

		/// <summary>
		/// Loads a record from the NewsRSS table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of NewsRSS containing the data in the record</returns>
		public static NewsRSS Load(Sql sql) {
				return ActiveRecordLoader.Load<NewsRSS>(sql, "NewsRSS");
		}
		public static NewsRSS Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<NewsRSS>(sql, "NewsRSS", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the NewsRSS table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of NewsRSS containing the data in the record</returns>
		public static NewsRSS Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<NewsRSS>(reader, "NewsRSS");
		}
		public static NewsRSS Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<NewsRSS>(reader, "NewsRSS", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where NewsRSSID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("NewsRSSID", new ActiveField<int>() { Name = "NewsRSSID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="NewsRSS"  });

	fields.Add("FeedName", new ActiveField<string>() { Name = "FeedName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="NewsRSS"  });

	fields.Add("Description", new ActiveField<string>() { Name = "Description", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=550, TableName="NewsRSS"  });

	fields.Add("FeedURL", new ActiveField<string>() { Name = "FeedURL", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="NewsRSS"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="NewsRSS"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="NewsRSS"  });

	fields.Add("ContentXML", new ActiveField<string>() { Name = "ContentXML", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="NewsRSS"  });

	fields.Add("LastUpdated", new ActiveField<System.DateTime?>() { Name = "LastUpdated", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="NewsRSS"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="NewsRSS"  });
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
				var rec = ActiveRecordLoader.LoadID<NewsRSS>(id, "NewsRSS", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the NewsRSS with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct NewsRSS or null if not in cache.</returns>
//		private static NewsRSS GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-NewsRSS-" + id) as NewsRSS;
//			return Web.PageGlobals["ActiveRecord-NewsRSS-" + id] as NewsRSS;
//		}

		/// <summary>
		/// Caches this NewsRSS object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-NewsRSS-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-NewsRSS-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-NewsRSS-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of NewsRSS objects/records. This is the usual data structure for holding a number of NewsRSS records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class NewsRSSList : ActiveRecordList<NewsRSS> {

		public NewsRSSList() : base() {}
		public NewsRSSList(List<NewsRSS> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-NewsRSS to NewsRSSList. 
		/// </summary>
		public static implicit operator NewsRSSList(List<NewsRSS> list) {
			return new NewsRSSList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from NewsRSSList to List-of-NewsRSS. 
		/// </summary>
		public static implicit operator List<NewsRSS>(NewsRSSList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of NewsRSS objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of NewsRSS records.</returns>
		public static NewsRSSList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where NewsRSSID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of NewsRSS objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of NewsRSS records.</returns>
		public static NewsRSSList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static NewsRSSList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where NewsRSSID in (", ids.SqlizeNumberList(), ")");
			var result = new NewsRSSList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of NewsRSS objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of NewsRSS records.</returns>
		public static NewsRSSList Load(Sql sql) {
			var result = new NewsRSSList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all NewsRSS objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and NewsRSSID desc.)
		/// </summary>
		public static NewsRSSList LoadAll() {
			var result = new NewsRSSList();
			result.LoadRecords(null);
			return result;
		}
		public static NewsRSSList LoadAll(int itemsPerPage) {
			var result = new NewsRSSList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static NewsRSSList LoadAll(int itemsPerPage, int pageNum) {
			var result = new NewsRSSList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" NewsRSS objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static NewsRSSList LoadActive() {
			var result = new NewsRSSList();
			var sql = (new NewsRSS()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static NewsRSSList LoadActive(int itemsPerPage) {
			var result = new NewsRSSList();
			var sql = (new NewsRSS()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static NewsRSSList LoadActive(int itemsPerPage, int pageNum) {
			var result = new NewsRSSList();
			var sql = (new NewsRSS()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static NewsRSSList LoadActivePlusExisting(object existingRecordID) {
			var result = new NewsRSSList();
			var sql = (new NewsRSS()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM NewsRSS");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM NewsRSS");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new NewsRSS()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = NewsRSS.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: NewsRSSID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public int NewsRSSID {
			get { return Fields.NewsRSSID.Value; }
			set { fields["NewsRSSID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByNewsRSSID(int newsRSSIDValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("NewsRSSID", newsRSSIDValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> NewsRSSID {
				get { return (ActiveField<int>)fields["NewsRSSID"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByNewsRSSID(int newsRSSIDValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where NewsRSSID=", Sql.Sqlize(newsRSSIDValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FeedName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FeedName {
			get { return Fields.FeedName.Value; }
			set { fields["FeedName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByFeedName(string feedNameValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("FeedName", feedNameValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FeedName {
				get { return (ActiveField<string>)fields["FeedName"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByFeedName(string feedNameValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where FeedName=", Sql.Sqlize(feedNameValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Description
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Description {
			get { return Fields.Description.Value; }
			set { fields["Description"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByDescription(string descriptionValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("Description", descriptionValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Description {
				get { return (ActiveField<string>)fields["Description"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByDescription(string descriptionValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where Description=", Sql.Sqlize(descriptionValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FeedURL
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FeedURL {
			get { return Fields.FeedURL.Value; }
			set { fields["FeedURL"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByFeedURL(string feedURLValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("FeedURL", feedURLValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FeedURL {
				get { return (ActiveField<string>)fields["FeedURL"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByFeedURL(string feedURLValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where FeedURL=", Sql.Sqlize(feedURLValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("DateAdded", dateAddedValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("IsPublished", isPublishedValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ContentXML
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ContentXML {
			get { return Fields.ContentXML.Value; }
			set { fields["ContentXML"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByContentXML(string contentXMLValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("ContentXML", contentXMLValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ContentXML {
				get { return (ActiveField<string>)fields["ContentXML"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByContentXML(string contentXMLValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where ContentXML=", Sql.Sqlize(contentXMLValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastUpdated
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? LastUpdated {
			get { return Fields.LastUpdated.Value; }
			set { fields["LastUpdated"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByLastUpdated(System.DateTime? lastUpdatedValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("LastUpdated", lastUpdatedValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> LastUpdated {
				get { return (ActiveField<System.DateTime?>)fields["LastUpdated"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByLastUpdated(System.DateTime? lastUpdatedValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where LastUpdated=", Sql.Sqlize(lastUpdatedValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class NewsRSS {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static NewsRSS LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<NewsRSS>("DateModified", dateModifiedValue, "NewsRSS", Otherwise.Null);
		}

		public partial class NewsRSSFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class NewsRSSList {		
				
		[JetBrains.Annotations.NotNull]
		public static NewsRSSList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","NewsRSS".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return NewsRSSList.Load(sql);
		}		
		
	}


}
#endregion