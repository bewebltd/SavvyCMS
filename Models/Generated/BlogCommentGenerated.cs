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
// CLASS: BlogComment
// TABLE: BlogComment
//-----------------------------------------


	public partial class BlogComment : ActiveRecord {

		/// <summary>
		/// The list that this BlogComment is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<BlogComment> GetContainingList() {
			return (ActiveRecordList<BlogComment>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public BlogComment(): base("BlogComment", "BlogCommentID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "BlogComment";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "BlogCommentID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property BlogCommentID.
		/// </summary>
		public int ID { get { return (int)fields["BlogCommentID"].ValueObject; } set { fields["BlogCommentID"].ValueObject = value; } }

		// field references
		public partial class BlogCommentFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public BlogCommentFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private BlogCommentFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public BlogCommentFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new BlogCommentFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the BlogComment table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of BlogComment</param>
		/// <returns>An instance of BlogComment containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the BlogComment table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg BlogComment.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = BlogComment.LoadID(55);</example>
		/// <param name="id">Primary key of BlogComment</param>
		/// <returns>An instance of BlogComment containing the data in the record</returns>
		public static BlogComment LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			BlogComment record = null;
//			record = BlogComment.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where BlogCommentID=", Sql.Sqlize(id));
//				record = new BlogComment();
//				if (!record.LoadData(sql)) return otherwise.Execute<BlogComment>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<BlogComment>(id, "BlogComment", otherwise);
		}

		/// <summary>
		/// Loads a record from the BlogComment table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of BlogComment containing the data in the record</returns>
		public static BlogComment Load(Sql sql) {
				return ActiveRecordLoader.Load<BlogComment>(sql, "BlogComment");
		}
		public static BlogComment Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<BlogComment>(sql, "BlogComment", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the BlogComment table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of BlogComment containing the data in the record</returns>
		public static BlogComment Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<BlogComment>(reader, "BlogComment");
		}
		public static BlogComment Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<BlogComment>(reader, "BlogComment", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where BlogCommentID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("BlogCommentID", new ActiveField<int>() { Name = "BlogCommentID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="BlogComment"  });

	fields.Add("BlogID", new ActiveField<int?>() { Name = "BlogID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="BlogComment" , GetForeignRecord = () => this.Blog, ForeignClassName = typeof(Models.Blog), ForeignTableName = "Blog", ForeignTableFieldName = "BlogID" });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="BlogComment"  });

	fields.Add("BodyText", new ActiveField<string>() { Name = "BodyText", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="BlogComment"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="BlogComment"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="BlogComment"  });

	fields.Add("CommentByPersonID", new ActiveField<int?>() { Name = "CommentByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="BlogComment" , GetForeignRecord = () => this.CommentByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("Company", new ActiveField<string>() { Name = "Company", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="BlogComment"  });

	fields.Add("FirstName", new ActiveField<string>() { Name = "FirstName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="BlogComment"  });

	fields.Add("LastName", new ActiveField<string>() { Name = "LastName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="BlogComment"  });

	fields.Add("Email", new ActiveField<string>() { Name = "Email", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=80, TableName="BlogComment"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="BlogComment"  });
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
				var rec = ActiveRecordLoader.LoadID<BlogComment>(id, "BlogComment", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the BlogComment with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct BlogComment or null if not in cache.</returns>
//		private static BlogComment GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-BlogComment-" + id) as BlogComment;
//			return Web.PageGlobals["ActiveRecord-BlogComment-" + id] as BlogComment;
//		}

		/// <summary>
		/// Caches this BlogComment object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-BlogComment-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-BlogComment-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-BlogComment-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of BlogComment objects/records. This is the usual data structure for holding a number of BlogComment records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class BlogCommentList : ActiveRecordList<BlogComment> {

		public BlogCommentList() : base() {}
		public BlogCommentList(List<BlogComment> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-BlogComment to BlogCommentList. 
		/// </summary>
		public static implicit operator BlogCommentList(List<BlogComment> list) {
			return new BlogCommentList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from BlogCommentList to List-of-BlogComment. 
		/// </summary>
		public static implicit operator List<BlogComment>(BlogCommentList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of BlogComment objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of BlogComment records.</returns>
		public static BlogCommentList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where BlogCommentID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of BlogComment objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of BlogComment records.</returns>
		public static BlogCommentList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static BlogCommentList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where BlogCommentID in (", ids.SqlizeNumberList(), ")");
			var result = new BlogCommentList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of BlogComment objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of BlogComment records.</returns>
		public static BlogCommentList Load(Sql sql) {
			var result = new BlogCommentList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all BlogComment objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and BlogCommentID desc.)
		/// </summary>
		public static BlogCommentList LoadAll() {
			var result = new BlogCommentList();
			result.LoadRecords(null);
			return result;
		}
		public static BlogCommentList LoadAll(int itemsPerPage) {
			var result = new BlogCommentList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static BlogCommentList LoadAll(int itemsPerPage, int pageNum) {
			var result = new BlogCommentList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" BlogComment objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static BlogCommentList LoadActive() {
			var result = new BlogCommentList();
			var sql = (new BlogComment()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static BlogCommentList LoadActive(int itemsPerPage) {
			var result = new BlogCommentList();
			var sql = (new BlogComment()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static BlogCommentList LoadActive(int itemsPerPage, int pageNum) {
			var result = new BlogCommentList();
			var sql = (new BlogComment()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static BlogCommentList LoadActivePlusExisting(object existingRecordID) {
			var result = new BlogCommentList();
			var sql = (new BlogComment()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM BlogComment");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM BlogComment");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new BlogComment()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = BlogComment.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: BlogCommentID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int BlogCommentID {
			get { return Fields.BlogCommentID.Value; }
			set { fields["BlogCommentID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByBlogCommentID(int blogCommentIDValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("BlogCommentID", blogCommentIDValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> BlogCommentID {
				get { return (ActiveField<int>)fields["BlogCommentID"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByBlogCommentID(int blogCommentIDValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where BlogCommentID=", Sql.Sqlize(blogCommentIDValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BlogID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? BlogID {
			get { return Fields.BlogID.Value; }
			set { fields["BlogID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByBlogID(int? blogIDValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("BlogID", blogIDValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> BlogID {
				get { return (ActiveField<int?>)fields["BlogID"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByBlogID(int? blogIDValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where BlogID=", Sql.Sqlize(blogIDValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Blog
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class BlogComment {
		[NonSerialized]		
		private Blog _Blog;

		[JetBrains.Annotations.CanBeNull]
		public Blog Blog
		{
			get
			{
				 // lazy load
				if (this._Blog == null && this.BlogID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Blog") && container.PrefetchCounter["Blog"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Blog>("BlogID",container.Select(r=>r.BlogID).ToList(),"Blog",Otherwise.Null);
					}
					this._Blog = Models.Blog.LoadByBlogID(BlogID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Blog")) {
							container.PrefetchCounter["Blog"] = 0;
						}
						container.PrefetchCounter["Blog"]++;
					}
				}
				return this._Blog;
			}
			set
			{
				this._Blog = value;
			}
		}
	}

	public partial class BlogCommentList {
		internal int numFetchesOfBlog = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Blog {
		[NonSerialized]		
		private BlogCommentList _BlogComments;
		
		[JetBrains.Annotations.NotNull]
		public BlogCommentList BlogComments
		{
			get
			{
				// lazy load
				if (this._BlogComments == null) {
					this._BlogComments = Models.BlogCommentList.LoadByBlogID(this.ID);
					this._BlogComments.SetParentBindField(this, "BlogID");
				}
				return this._BlogComments;
			}
			set
			{
				this._BlogComments = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("Title", titleValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyText {
			get { return Fields.BodyText.Value; }
			set { fields["BodyText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByBodyText(string bodyTextValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("BodyText", bodyTextValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyText {
				get { return (ActiveField<string>)fields["BodyText"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByBodyText(string bodyTextValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where BodyText=", Sql.Sqlize(bodyTextValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("DateAdded", dateAddedValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("IsPublished", isPublishedValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CommentByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? CommentByPersonID {
			get { return Fields.CommentByPersonID.Value; }
			set { fields["CommentByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByCommentByPersonID(int? commentByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("CommentByPersonID", commentByPersonIDValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> CommentByPersonID {
				get { return (ActiveField<int?>)fields["CommentByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByCommentByPersonID(int? commentByPersonIDValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where CommentByPersonID=", Sql.Sqlize(commentByPersonIDValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: CommentByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class BlogComment {
		[NonSerialized]		
		private Person _CommentByPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person CommentByPerson
		{
			get
			{
				 // lazy load
				if (this._CommentByPerson == null && this.CommentByPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("CommentByPerson") && container.PrefetchCounter["CommentByPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.CommentByPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._CommentByPerson = Models.Person.LoadByPersonID(CommentByPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("CommentByPerson")) {
							container.PrefetchCounter["CommentByPerson"] = 0;
						}
						container.PrefetchCounter["CommentByPerson"]++;
					}
				}
				return this._CommentByPerson;
			}
			set
			{
				this._CommentByPerson = value;
			}
		}
	}

	public partial class BlogCommentList {
		internal int numFetchesOfCommentByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private BlogCommentList _BlogCommentsCommentBy;
		
		[JetBrains.Annotations.NotNull]
		public BlogCommentList BlogCommentsCommentBy
		{
			get
			{
				// lazy load
				if (this._BlogCommentsCommentBy == null) {
					this._BlogCommentsCommentBy = Models.BlogCommentList.LoadByCommentByPersonID(this.ID);
					this._BlogCommentsCommentBy.SetParentBindField(this, "CommentByPersonID");
				}
				return this._BlogCommentsCommentBy;
			}
			set
			{
				this._BlogCommentsCommentBy = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Company
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Company {
			get { return Fields.Company.Value; }
			set { fields["Company"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByCompany(string companyValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("Company", companyValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Company {
				get { return (ActiveField<string>)fields["Company"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByCompany(string companyValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where Company=", Sql.Sqlize(companyValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FirstName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FirstName {
			get { return Fields.FirstName.Value; }
			set { fields["FirstName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByFirstName(string firstNameValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("FirstName", firstNameValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FirstName {
				get { return (ActiveField<string>)fields["FirstName"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByFirstName(string firstNameValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where FirstName=", Sql.Sqlize(firstNameValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LastName {
			get { return Fields.LastName.Value; }
			set { fields["LastName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByLastName(string lastNameValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("LastName", lastNameValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LastName {
				get { return (ActiveField<string>)fields["LastName"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByLastName(string lastNameValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where LastName=", Sql.Sqlize(lastNameValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Email
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Email {
			get { return Fields.Email.Value; }
			set { fields["Email"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByEmail(string emailValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("Email", emailValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Email {
				get { return (ActiveField<string>)fields["Email"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByEmail(string emailValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where Email=", Sql.Sqlize(emailValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class BlogComment {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static BlogComment LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<BlogComment>("DateModified", dateModifiedValue, "BlogComment", Otherwise.Null);
		}

		public partial class BlogCommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class BlogCommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static BlogCommentList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","BlogComment".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return BlogCommentList.Load(sql);
		}		
		
	}


}
#endregion