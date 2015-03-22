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
// CLASS: Comment
// TABLE: Comment
//-----------------------------------------


	public partial class Comment : ActiveRecord {

		/// <summary>
		/// The list that this Comment is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Comment> GetContainingList() {
			return (ActiveRecordList<Comment>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Comment(): base("Comment", "CommentID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Comment";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "CommentID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property CommentID.
		/// </summary>
		public int ID { get { return (int)fields["CommentID"].ValueObject; } set { fields["CommentID"].ValueObject = value; } }

		// field references
		public partial class CommentFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public CommentFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private CommentFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public CommentFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new CommentFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Comment table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Comment</param>
		/// <returns>An instance of Comment containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Comment table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Comment.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Comment.LoadID(55);</example>
		/// <param name="id">Primary key of Comment</param>
		/// <returns>An instance of Comment containing the data in the record</returns>
		public static Comment LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Comment record = null;
//			record = Comment.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where CommentID=", Sql.Sqlize(id));
//				record = new Comment();
//				if (!record.LoadData(sql)) return otherwise.Execute<Comment>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Comment>(id, "Comment", otherwise);
		}

		/// <summary>
		/// Loads a record from the Comment table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Comment containing the data in the record</returns>
		public static Comment Load(Sql sql) {
				return ActiveRecordLoader.Load<Comment>(sql, "Comment");
		}
		public static Comment Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Comment>(sql, "Comment", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Comment table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Comment containing the data in the record</returns>
		public static Comment Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Comment>(reader, "Comment");
		}
		public static Comment Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Comment>(reader, "Comment", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where CommentID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("CommentID", new ActiveField<int>() { Name = "CommentID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Comment"  });

	fields.Add("CommentText", new ActiveField<string>() { Name = "CommentText", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Comment"  });

	fields.Add("CommentDate", new ActiveField<System.DateTime?>() { Name = "CommentDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Comment"  });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Comment" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("Status", new ActiveField<string>() { Name = "Status", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="Comment"  });

	fields.Add("ApprovedByPersonID", new ActiveField<int?>() { Name = "ApprovedByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Comment" , GetForeignRecord = () => this.ApprovedByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("ApprovedDate", new ActiveField<System.DateTime?>() { Name = "ApprovedDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Comment"  });

	fields.Add("ParentCommentID", new ActiveField<int?>() { Name = "ParentCommentID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Comment" , GetForeignRecord = () => this.ParentComment, ForeignClassName = typeof(Models.Comment), ForeignTableName = "Comment", ForeignTableFieldName = "CommentID" });

	fields.Add("RecipeID", new ActiveField<int?>() { Name = "RecipeID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Comment"  });

	fields.Add("CommenterName", new ActiveField<string>() { Name = "CommenterName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Comment"  });

	fields.Add("CommenterEmail", new ActiveField<string>() { Name = "CommenterEmail", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=200, TableName="Comment"  });

	fields.Add("DeclineReason", new ActiveField<string>() { Name = "DeclineReason", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Comment"  });

	fields.Add("PersonType", new ActiveField<string>() { Name = "PersonType", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Comment"  });

	fields.Add("CommenterIP", new ActiveField<string>() { Name = "CommenterIP", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Comment"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Comment"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Comment"  });
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
				var rec = ActiveRecordLoader.LoadID<Comment>(id, "Comment", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Comment with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Comment or null if not in cache.</returns>
//		private static Comment GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Comment-" + id) as Comment;
//			return Web.PageGlobals["ActiveRecord-Comment-" + id] as Comment;
//		}

		/// <summary>
		/// Caches this Comment object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Comment-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Comment-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Comment-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Comment objects/records. This is the usual data structure for holding a number of Comment records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class CommentList : ActiveRecordList<Comment> {

		public CommentList() : base() {}
		public CommentList(List<Comment> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Comment to CommentList. 
		/// </summary>
		public static implicit operator CommentList(List<Comment> list) {
			return new CommentList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from CommentList to List-of-Comment. 
		/// </summary>
		public static implicit operator List<Comment>(CommentList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Comment objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Comment records.</returns>
		public static CommentList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where CommentID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Comment objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Comment records.</returns>
		public static CommentList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static CommentList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where CommentID in (", ids.SqlizeNumberList(), ")");
			var result = new CommentList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Comment objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Comment records.</returns>
		public static CommentList Load(Sql sql) {
			var result = new CommentList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Comment objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and CommentID desc.)
		/// </summary>
		public static CommentList LoadAll() {
			var result = new CommentList();
			result.LoadRecords(null);
			return result;
		}
		public static CommentList LoadAll(int itemsPerPage) {
			var result = new CommentList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CommentList LoadAll(int itemsPerPage, int pageNum) {
			var result = new CommentList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Comment objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static CommentList LoadActive() {
			var result = new CommentList();
			var sql = (new Comment()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static CommentList LoadActive(int itemsPerPage) {
			var result = new CommentList();
			var sql = (new Comment()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CommentList LoadActive(int itemsPerPage, int pageNum) {
			var result = new CommentList();
			var sql = (new Comment()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static CommentList LoadActivePlusExisting(object existingRecordID) {
			var result = new CommentList();
			var sql = (new Comment()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Comment");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Comment");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Comment()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Comment.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: CommentID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int CommentID {
			get { return Fields.CommentID.Value; }
			set { fields["CommentID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByCommentID(int commentIDValue) {
			return ActiveRecordLoader.LoadByField<Comment>("CommentID", commentIDValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> CommentID {
				get { return (ActiveField<int>)fields["CommentID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByCommentID(int commentIDValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where CommentID=", Sql.Sqlize(commentIDValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CommentText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CommentText {
			get { return Fields.CommentText.Value; }
			set { fields["CommentText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByCommentText(string commentTextValue) {
			return ActiveRecordLoader.LoadByField<Comment>("CommentText", commentTextValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CommentText {
				get { return (ActiveField<string>)fields["CommentText"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByCommentText(string commentTextValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where CommentText=", Sql.Sqlize(commentTextValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CommentDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? CommentDate {
			get { return Fields.CommentDate.Value; }
			set { fields["CommentDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByCommentDate(System.DateTime? commentDateValue) {
			return ActiveRecordLoader.LoadByField<Comment>("CommentDate", commentDateValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> CommentDate {
				get { return (ActiveField<System.DateTime?>)fields["CommentDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByCommentDate(System.DateTime? commentDateValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where CommentDate=", Sql.Sqlize(commentDateValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<Comment>("PersonID", personIDValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return CommentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Comment {
		[NonSerialized]		
		private Person _Person;

		[JetBrains.Annotations.CanBeNull]
		public Person Person
		{
			get
			{
				 // lazy load
				if (this._Person == null && this.PersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Person") && container.PrefetchCounter["Person"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.PersonID).ToList(),"Person",Otherwise.Null);
					}
					this._Person = Models.Person.LoadByPersonID(PersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Person")) {
							container.PrefetchCounter["Person"] = 0;
						}
						container.PrefetchCounter["Person"]++;
					}
				}
				return this._Person;
			}
			set
			{
				this._Person = value;
			}
		}
	}

	public partial class CommentList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private CommentList _Comments;
		
		[JetBrains.Annotations.NotNull]
		public CommentList Comments
		{
			get
			{
				// lazy load
				if (this._Comments == null) {
					this._Comments = Models.CommentList.LoadByPersonID(this.ID);
					this._Comments.SetParentBindField(this, "PersonID");
				}
				return this._Comments;
			}
			set
			{
				this._Comments = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Status
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Status {
			get { return Fields.Status.Value; }
			set { fields["Status"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByStatus(string statusValue) {
			return ActiveRecordLoader.LoadByField<Comment>("Status", statusValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Status {
				get { return (ActiveField<string>)fields["Status"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByStatus(string statusValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where Status=", Sql.Sqlize(statusValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ApprovedByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ApprovedByPersonID {
			get { return Fields.ApprovedByPersonID.Value; }
			set { fields["ApprovedByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByApprovedByPersonID(int? approvedByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<Comment>("ApprovedByPersonID", approvedByPersonIDValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ApprovedByPersonID {
				get { return (ActiveField<int?>)fields["ApprovedByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByApprovedByPersonID(int? approvedByPersonIDValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where ApprovedByPersonID=", Sql.Sqlize(approvedByPersonIDValue));
			return CommentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ApprovedByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Comment {
		[NonSerialized]		
		private Person _ApprovedByPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person ApprovedByPerson
		{
			get
			{
				 // lazy load
				if (this._ApprovedByPerson == null && this.ApprovedByPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ApprovedByPerson") && container.PrefetchCounter["ApprovedByPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.ApprovedByPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._ApprovedByPerson = Models.Person.LoadByPersonID(ApprovedByPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ApprovedByPerson")) {
							container.PrefetchCounter["ApprovedByPerson"] = 0;
						}
						container.PrefetchCounter["ApprovedByPerson"]++;
					}
				}
				return this._ApprovedByPerson;
			}
			set
			{
				this._ApprovedByPerson = value;
			}
		}
	}

	public partial class CommentList {
		internal int numFetchesOfApprovedByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private CommentList _CommentsApprovedBy;
		
		[JetBrains.Annotations.NotNull]
		public CommentList CommentsApprovedBy
		{
			get
			{
				// lazy load
				if (this._CommentsApprovedBy == null) {
					this._CommentsApprovedBy = Models.CommentList.LoadByApprovedByPersonID(this.ID);
					this._CommentsApprovedBy.SetParentBindField(this, "ApprovedByPersonID");
				}
				return this._CommentsApprovedBy;
			}
			set
			{
				this._CommentsApprovedBy = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: ApprovedDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ApprovedDate {
			get { return Fields.ApprovedDate.Value; }
			set { fields["ApprovedDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByApprovedDate(System.DateTime? approvedDateValue) {
			return ActiveRecordLoader.LoadByField<Comment>("ApprovedDate", approvedDateValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ApprovedDate {
				get { return (ActiveField<System.DateTime?>)fields["ApprovedDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByApprovedDate(System.DateTime? approvedDateValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where ApprovedDate=", Sql.Sqlize(approvedDateValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ParentCommentID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ParentCommentID {
			get { return Fields.ParentCommentID.Value; }
			set { fields["ParentCommentID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByParentCommentID(int? parentCommentIDValue) {
			return ActiveRecordLoader.LoadByField<Comment>("ParentCommentID", parentCommentIDValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ParentCommentID {
				get { return (ActiveField<int?>)fields["ParentCommentID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByParentCommentID(int? parentCommentIDValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where ParentCommentID=", Sql.Sqlize(parentCommentIDValue));
			return CommentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ParentComment
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Comment {
		[NonSerialized]		
		private Comment _ParentComment;

		[JetBrains.Annotations.CanBeNull]
		public Comment ParentComment
		{
			get
			{
				 // lazy load
				if (this._ParentComment == null && this.ParentCommentID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ParentComment") && container.PrefetchCounter["ParentComment"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Comment>("CommentID",container.Select(r=>r.ParentCommentID).ToList(),"Comment",Otherwise.Null);
					}
					this._ParentComment = Models.Comment.LoadByCommentID(ParentCommentID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ParentComment")) {
							container.PrefetchCounter["ParentComment"] = 0;
						}
						container.PrefetchCounter["ParentComment"]++;
					}
				}
				return this._ParentComment;
			}
			set
			{
				this._ParentComment = value;
			}
		}
	}

	public partial class CommentList {
		internal int numFetchesOfParentComment = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Comment {
		[NonSerialized]		
		private CommentList _ChildComments;
		
		[JetBrains.Annotations.NotNull]
		public CommentList ChildComments
		{
			get
			{
				// lazy load
				if (this._ChildComments == null) {
					this._ChildComments = Models.CommentList.LoadByParentCommentID(this.ID);
					this._ChildComments.SetParentBindField(this, "ParentCommentID");
				}
				return this._ChildComments;
			}
			set
			{
				this._ChildComments = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: RecipeID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? RecipeID {
			get { return Fields.RecipeID.Value; }
			set { fields["RecipeID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByRecipeID(int? recipeIDValue) {
			return ActiveRecordLoader.LoadByField<Comment>("RecipeID", recipeIDValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> RecipeID {
				get { return (ActiveField<int?>)fields["RecipeID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByRecipeID(int? recipeIDValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where RecipeID=", Sql.Sqlize(recipeIDValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CommenterName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CommenterName {
			get { return Fields.CommenterName.Value; }
			set { fields["CommenterName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByCommenterName(string commenterNameValue) {
			return ActiveRecordLoader.LoadByField<Comment>("CommenterName", commenterNameValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CommenterName {
				get { return (ActiveField<string>)fields["CommenterName"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByCommenterName(string commenterNameValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where CommenterName=", Sql.Sqlize(commenterNameValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CommenterEmail
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CommenterEmail {
			get { return Fields.CommenterEmail.Value; }
			set { fields["CommenterEmail"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByCommenterEmail(string commenterEmailValue) {
			return ActiveRecordLoader.LoadByField<Comment>("CommenterEmail", commenterEmailValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CommenterEmail {
				get { return (ActiveField<string>)fields["CommenterEmail"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByCommenterEmail(string commenterEmailValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where CommenterEmail=", Sql.Sqlize(commenterEmailValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DeclineReason
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string DeclineReason {
			get { return Fields.DeclineReason.Value; }
			set { fields["DeclineReason"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByDeclineReason(string declineReasonValue) {
			return ActiveRecordLoader.LoadByField<Comment>("DeclineReason", declineReasonValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> DeclineReason {
				get { return (ActiveField<string>)fields["DeclineReason"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByDeclineReason(string declineReasonValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where DeclineReason=", Sql.Sqlize(declineReasonValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonType
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PersonType {
			get { return Fields.PersonType.Value; }
			set { fields["PersonType"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByPersonType(string personTypeValue) {
			return ActiveRecordLoader.LoadByField<Comment>("PersonType", personTypeValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PersonType {
				get { return (ActiveField<string>)fields["PersonType"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByPersonType(string personTypeValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where PersonType=", Sql.Sqlize(personTypeValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CommenterIP
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CommenterIP {
			get { return Fields.CommenterIP.Value; }
			set { fields["CommenterIP"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByCommenterIP(string commenterIPValue) {
			return ActiveRecordLoader.LoadByField<Comment>("CommenterIP", commenterIPValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CommenterIP {
				get { return (ActiveField<string>)fields["CommenterIP"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByCommenterIP(string commenterIPValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where CommenterIP=", Sql.Sqlize(commenterIPValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Comment>("DateAdded", dateAddedValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return CommentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Comment {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Comment LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Comment>("DateModified", dateModifiedValue, "Comment", Otherwise.Null);
		}

		public partial class CommentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class CommentList {		
				
		[JetBrains.Annotations.NotNull]
		public static CommentList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Comment".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return CommentList.Load(sql);
		}		
		
	}


}
#endregion