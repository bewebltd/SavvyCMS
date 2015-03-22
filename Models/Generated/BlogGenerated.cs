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
// CLASS: Blog
// TABLE: Blog
//-----------------------------------------


	public partial class Blog : ActiveRecord {

		/// <summary>
		/// The list that this Blog is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Blog> GetContainingList() {
			return (ActiveRecordList<Blog>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Blog(): base("Blog", "BlogID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Blog";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "BlogID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property BlogID.
		/// </summary>
		public int ID { get { return (int)fields["BlogID"].ValueObject; } set { fields["BlogID"].ValueObject = value; } }

		// field references
		public partial class BlogFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public BlogFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private BlogFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public BlogFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new BlogFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Blog table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Blog</param>
		/// <returns>An instance of Blog containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Blog table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Blog.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Blog.LoadID(55);</example>
		/// <param name="id">Primary key of Blog</param>
		/// <returns>An instance of Blog containing the data in the record</returns>
		public static Blog LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Blog record = null;
//			record = Blog.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where BlogID=", Sql.Sqlize(id));
//				record = new Blog();
//				if (!record.LoadData(sql)) return otherwise.Execute<Blog>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Blog>(id, "Blog", otherwise);
		}

		/// <summary>
		/// Loads a record from the Blog table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Blog containing the data in the record</returns>
		public static Blog Load(Sql sql) {
				return ActiveRecordLoader.Load<Blog>(sql, "Blog");
		}
		public static Blog Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Blog>(sql, "Blog", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Blog table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Blog containing the data in the record</returns>
		public static Blog Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Blog>(reader, "Blog");
		}
		public static Blog Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Blog>(reader, "Blog", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where BlogID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("BlogID", new ActiveField<int>() { Name = "BlogID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Blog"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Blog"  });

	fields.Add("BodyText", new ActiveField<string>() { Name = "BodyText", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Blog"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Blog"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Blog"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Blog"  });
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
				var rec = ActiveRecordLoader.LoadID<Blog>(id, "Blog", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Blog with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Blog or null if not in cache.</returns>
//		private static Blog GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Blog-" + id) as Blog;
//			return Web.PageGlobals["ActiveRecord-Blog-" + id] as Blog;
//		}

		/// <summary>
		/// Caches this Blog object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Blog-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Blog-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Blog-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Blog objects/records. This is the usual data structure for holding a number of Blog records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class BlogList : ActiveRecordList<Blog> {

		public BlogList() : base() {}
		public BlogList(List<Blog> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Blog to BlogList. 
		/// </summary>
		public static implicit operator BlogList(List<Blog> list) {
			return new BlogList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from BlogList to List-of-Blog. 
		/// </summary>
		public static implicit operator List<Blog>(BlogList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Blog objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Blog records.</returns>
		public static BlogList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where BlogID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Blog objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Blog records.</returns>
		public static BlogList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static BlogList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where BlogID in (", ids.SqlizeNumberList(), ")");
			var result = new BlogList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Blog objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Blog records.</returns>
		public static BlogList Load(Sql sql) {
			var result = new BlogList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Blog objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and BlogID desc.)
		/// </summary>
		public static BlogList LoadAll() {
			var result = new BlogList();
			result.LoadRecords(null);
			return result;
		}
		public static BlogList LoadAll(int itemsPerPage) {
			var result = new BlogList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static BlogList LoadAll(int itemsPerPage, int pageNum) {
			var result = new BlogList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Blog objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static BlogList LoadActive() {
			var result = new BlogList();
			var sql = (new Blog()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static BlogList LoadActive(int itemsPerPage) {
			var result = new BlogList();
			var sql = (new Blog()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static BlogList LoadActive(int itemsPerPage, int pageNum) {
			var result = new BlogList();
			var sql = (new Blog()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static BlogList LoadActivePlusExisting(object existingRecordID) {
			var result = new BlogList();
			var sql = (new Blog()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Blog");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Blog");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Blog()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Blog.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: BlogID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Blog {		
				
		[JetBrains.Annotations.CanBeNull]
		public int BlogID {
			get { return Fields.BlogID.Value; }
			set { fields["BlogID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadByBlogID(int blogIDValue) {
			return ActiveRecordLoader.LoadByField<Blog>("BlogID", blogIDValue, "Blog", Otherwise.Null);
		}

		public partial class BlogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> BlogID {
				get { return (ActiveField<int>)fields["BlogID"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogList LoadByBlogID(int blogIDValue) {
			var sql = new Sql("select * from ","Blog".SqlizeName()," where BlogID=", Sql.Sqlize(blogIDValue));
			return BlogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Blog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Blog>("Title", titleValue, "Blog", Otherwise.Null);
		}

		public partial class BlogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Blog".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return BlogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Blog {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyText {
			get { return Fields.BodyText.Value; }
			set { fields["BodyText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadByBodyText(string bodyTextValue) {
			return ActiveRecordLoader.LoadByField<Blog>("BodyText", bodyTextValue, "Blog", Otherwise.Null);
		}

		public partial class BlogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyText {
				get { return (ActiveField<string>)fields["BodyText"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogList LoadByBodyText(string bodyTextValue) {
			var sql = new Sql("select * from ","Blog".SqlizeName()," where BodyText=", Sql.Sqlize(bodyTextValue));
			return BlogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Blog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Blog>("DateAdded", dateAddedValue, "Blog", Otherwise.Null);
		}

		public partial class BlogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Blog".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return BlogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Blog {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<Blog>("IsPublished", isPublishedValue, "Blog", Otherwise.Null);
		}

		public partial class BlogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","Blog".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return BlogList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Blog {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Blog LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Blog>("DateModified", dateModifiedValue, "Blog", Otherwise.Null);
		}

		public partial class BlogFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Blog".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return BlogList.Load(sql);
		}		
		
	}


}
#endregion