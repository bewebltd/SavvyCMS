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
// CLASS: Testimonial
// TABLE: Testimonial
//-----------------------------------------


	public partial class Testimonial : ActiveRecord {

		/// <summary>
		/// The list that this Testimonial is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Testimonial> GetContainingList() {
			return (ActiveRecordList<Testimonial>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Testimonial(): base("Testimonial", "TestimonialID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Testimonial";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "TestimonialID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property TestimonialID.
		/// </summary>
		public int ID { get { return (int)fields["TestimonialID"].ValueObject; } set { fields["TestimonialID"].ValueObject = value; } }

		// field references
		public partial class TestimonialFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public TestimonialFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private TestimonialFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public TestimonialFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new TestimonialFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Testimonial table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Testimonial</param>
		/// <returns>An instance of Testimonial containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Testimonial table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Testimonial.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Testimonial.LoadID(55);</example>
		/// <param name="id">Primary key of Testimonial</param>
		/// <returns>An instance of Testimonial containing the data in the record</returns>
		public static Testimonial LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Testimonial record = null;
//			record = Testimonial.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where TestimonialID=", Sql.Sqlize(id));
//				record = new Testimonial();
//				if (!record.LoadData(sql)) return otherwise.Execute<Testimonial>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Testimonial>(id, "Testimonial", otherwise);
		}

		/// <summary>
		/// Loads a record from the Testimonial table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Testimonial containing the data in the record</returns>
		public static Testimonial Load(Sql sql) {
				return ActiveRecordLoader.Load<Testimonial>(sql, "Testimonial");
		}
		public static Testimonial Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Testimonial>(sql, "Testimonial", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Testimonial table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Testimonial containing the data in the record</returns>
		public static Testimonial Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Testimonial>(reader, "Testimonial");
		}
		public static Testimonial Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Testimonial>(reader, "Testimonial", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where TestimonialID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("TestimonialID", new ActiveField<int>() { Name = "TestimonialID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Testimonial"  });

	fields.Add("Comments", new ActiveField<string>() { Name = "Comments", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Testimonial"  });

	fields.Add("Author", new ActiveField<string>() { Name = "Author", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Testimonial"  });

	fields.Add("AuthorRole", new ActiveField<string>() { Name = "AuthorRole", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Testimonial"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Testimonial"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Testimonial"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Testimonial"  });
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
				var rec = ActiveRecordLoader.LoadID<Testimonial>(id, "Testimonial", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Testimonial with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Testimonial or null if not in cache.</returns>
//		private static Testimonial GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Testimonial-" + id) as Testimonial;
//			return Web.PageGlobals["ActiveRecord-Testimonial-" + id] as Testimonial;
//		}

		/// <summary>
		/// Caches this Testimonial object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Testimonial-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Testimonial-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Testimonial-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Testimonial objects/records. This is the usual data structure for holding a number of Testimonial records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class TestimonialList : ActiveRecordList<Testimonial> {

		public TestimonialList() : base() {}
		public TestimonialList(List<Testimonial> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Testimonial to TestimonialList. 
		/// </summary>
		public static implicit operator TestimonialList(List<Testimonial> list) {
			return new TestimonialList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from TestimonialList to List-of-Testimonial. 
		/// </summary>
		public static implicit operator List<Testimonial>(TestimonialList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Testimonial objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Testimonial records.</returns>
		public static TestimonialList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where TestimonialID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Testimonial objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Testimonial records.</returns>
		public static TestimonialList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static TestimonialList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where TestimonialID in (", ids.SqlizeNumberList(), ")");
			var result = new TestimonialList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Testimonial objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Testimonial records.</returns>
		public static TestimonialList Load(Sql sql) {
			var result = new TestimonialList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Testimonial objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and TestimonialID desc.)
		/// </summary>
		public static TestimonialList LoadAll() {
			var result = new TestimonialList();
			result.LoadRecords(null);
			return result;
		}
		public static TestimonialList LoadAll(int itemsPerPage) {
			var result = new TestimonialList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static TestimonialList LoadAll(int itemsPerPage, int pageNum) {
			var result = new TestimonialList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Testimonial objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static TestimonialList LoadActive() {
			var result = new TestimonialList();
			var sql = (new Testimonial()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static TestimonialList LoadActive(int itemsPerPage) {
			var result = new TestimonialList();
			var sql = (new Testimonial()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static TestimonialList LoadActive(int itemsPerPage, int pageNum) {
			var result = new TestimonialList();
			var sql = (new Testimonial()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static TestimonialList LoadActivePlusExisting(object existingRecordID) {
			var result = new TestimonialList();
			var sql = (new Testimonial()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Testimonial");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Testimonial");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Testimonial()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Testimonial.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: TestimonialID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public int TestimonialID {
			get { return Fields.TestimonialID.Value; }
			set { fields["TestimonialID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByTestimonialID(int testimonialIDValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("TestimonialID", testimonialIDValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> TestimonialID {
				get { return (ActiveField<int>)fields["TestimonialID"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByTestimonialID(int testimonialIDValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where TestimonialID=", Sql.Sqlize(testimonialIDValue));
			return TestimonialList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Comments
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Comments {
			get { return Fields.Comments.Value; }
			set { fields["Comments"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByComments(string commentsValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("Comments", commentsValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Comments {
				get { return (ActiveField<string>)fields["Comments"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByComments(string commentsValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where Comments=", Sql.Sqlize(commentsValue));
			return TestimonialList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Author
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Author {
			get { return Fields.Author.Value; }
			set { fields["Author"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByAuthor(string authorValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("Author", authorValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Author {
				get { return (ActiveField<string>)fields["Author"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByAuthor(string authorValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where Author=", Sql.Sqlize(authorValue));
			return TestimonialList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AuthorRole
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AuthorRole {
			get { return Fields.AuthorRole.Value; }
			set { fields["AuthorRole"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByAuthorRole(string authorRoleValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("AuthorRole", authorRoleValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AuthorRole {
				get { return (ActiveField<string>)fields["AuthorRole"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByAuthorRole(string authorRoleValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where AuthorRole=", Sql.Sqlize(authorRoleValue));
			return TestimonialList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("IsPublished", isPublishedValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return TestimonialList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("DateAdded", dateAddedValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return TestimonialList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Testimonial {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Testimonial LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Testimonial>("DateModified", dateModifiedValue, "Testimonial", Otherwise.Null);
		}

		public partial class TestimonialFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class TestimonialList {		
				
		[JetBrains.Annotations.NotNull]
		public static TestimonialList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Testimonial".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return TestimonialList.Load(sql);
		}		
		
	}


}
#endregion