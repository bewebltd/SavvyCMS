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
// CLASS: HomepageSlide
// TABLE: HomepageSlide
//-----------------------------------------


	public partial class HomepageSlide : ActiveRecord {

		/// <summary>
		/// The list that this HomepageSlide is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<HomepageSlide> GetContainingList() {
			return (ActiveRecordList<HomepageSlide>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public HomepageSlide(): base("HomepageSlide", "HomepageSlideID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "HomepageSlide";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "HomepageSlideID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property HomepageSlideID.
		/// </summary>
		public int ID { get { return (int)fields["HomepageSlideID"].ValueObject; } set { fields["HomepageSlideID"].ValueObject = value; } }

		// field references
		public partial class HomepageSlideFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public HomepageSlideFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private HomepageSlideFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public HomepageSlideFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new HomepageSlideFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the HomepageSlide table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of HomepageSlide</param>
		/// <returns>An instance of HomepageSlide containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the HomepageSlide table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg HomepageSlide.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = HomepageSlide.LoadID(55);</example>
		/// <param name="id">Primary key of HomepageSlide</param>
		/// <returns>An instance of HomepageSlide containing the data in the record</returns>
		public static HomepageSlide LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			HomepageSlide record = null;
//			record = HomepageSlide.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where HomepageSlideID=", Sql.Sqlize(id));
//				record = new HomepageSlide();
//				if (!record.LoadData(sql)) return otherwise.Execute<HomepageSlide>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<HomepageSlide>(id, "HomepageSlide", otherwise);
		}

		/// <summary>
		/// Loads a record from the HomepageSlide table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of HomepageSlide containing the data in the record</returns>
		public static HomepageSlide Load(Sql sql) {
				return ActiveRecordLoader.Load<HomepageSlide>(sql, "HomepageSlide");
		}
		public static HomepageSlide Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<HomepageSlide>(sql, "HomepageSlide", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the HomepageSlide table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of HomepageSlide containing the data in the record</returns>
		public static HomepageSlide Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<HomepageSlide>(reader, "HomepageSlide");
		}
		public static HomepageSlide Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<HomepageSlide>(reader, "HomepageSlide", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where HomepageSlideID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("HomepageSlideID", new ActiveField<int>() { Name = "HomepageSlideID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="HomepageSlide"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="HomepageSlide"  });

	fields.Add("SlidePicture", new PictureActiveField() { Name = "SlidePicture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="HomepageSlide"  });

	fields.Add("LinkURL", new ActiveField<string>() { Name = "LinkURL", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="HomepageSlide"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="HomepageSlide"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="HomepageSlide"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="HomepageSlide"  });

	fields.Add("Duration", new ActiveField<int?>() { Name = "Duration", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="HomepageSlide"  });

	fields.Add("BodyText", new ActiveField<string>() { Name = "BodyText", ColumnType = "text", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="HomepageSlide"  });

	fields.Add("AltText", new ActiveField<string>() { Name = "AltText", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="HomepageSlide"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="HomepageSlide"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="HomepageSlide"  });
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
				var rec = ActiveRecordLoader.LoadID<HomepageSlide>(id, "HomepageSlide", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the HomepageSlide with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct HomepageSlide or null if not in cache.</returns>
//		private static HomepageSlide GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-HomepageSlide-" + id) as HomepageSlide;
//			return Web.PageGlobals["ActiveRecord-HomepageSlide-" + id] as HomepageSlide;
//		}

		/// <summary>
		/// Caches this HomepageSlide object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-HomepageSlide-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-HomepageSlide-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-HomepageSlide-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of HomepageSlide objects/records. This is the usual data structure for holding a number of HomepageSlide records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class HomepageSlideList : ActiveRecordList<HomepageSlide> {

		public HomepageSlideList() : base() {}
		public HomepageSlideList(List<HomepageSlide> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-HomepageSlide to HomepageSlideList. 
		/// </summary>
		public static implicit operator HomepageSlideList(List<HomepageSlide> list) {
			return new HomepageSlideList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from HomepageSlideList to List-of-HomepageSlide. 
		/// </summary>
		public static implicit operator List<HomepageSlide>(HomepageSlideList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of HomepageSlide objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of HomepageSlide records.</returns>
		public static HomepageSlideList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where HomepageSlideID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of HomepageSlide objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of HomepageSlide records.</returns>
		public static HomepageSlideList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static HomepageSlideList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where HomepageSlideID in (", ids.SqlizeNumberList(), ")");
			var result = new HomepageSlideList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of HomepageSlide objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of HomepageSlide records.</returns>
		public static HomepageSlideList Load(Sql sql) {
			var result = new HomepageSlideList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all HomepageSlide objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and HomepageSlideID desc.)
		/// </summary>
		public static HomepageSlideList LoadAll() {
			var result = new HomepageSlideList();
			result.LoadRecords(null);
			return result;
		}
		public static HomepageSlideList LoadAll(int itemsPerPage) {
			var result = new HomepageSlideList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static HomepageSlideList LoadAll(int itemsPerPage, int pageNum) {
			var result = new HomepageSlideList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" HomepageSlide objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static HomepageSlideList LoadActive() {
			var result = new HomepageSlideList();
			var sql = (new HomepageSlide()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static HomepageSlideList LoadActive(int itemsPerPage) {
			var result = new HomepageSlideList();
			var sql = (new HomepageSlide()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static HomepageSlideList LoadActive(int itemsPerPage, int pageNum) {
			var result = new HomepageSlideList();
			var sql = (new HomepageSlide()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static HomepageSlideList LoadActivePlusExisting(object existingRecordID) {
			var result = new HomepageSlideList();
			var sql = (new HomepageSlide()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM HomepageSlide");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM HomepageSlide");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new HomepageSlide()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = HomepageSlide.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: HomepageSlideID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public int HomepageSlideID {
			get { return Fields.HomepageSlideID.Value; }
			set { fields["HomepageSlideID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByHomepageSlideID(int homepageSlideIDValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("HomepageSlideID", homepageSlideIDValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> HomepageSlideID {
				get { return (ActiveField<int>)fields["HomepageSlideID"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByHomepageSlideID(int homepageSlideIDValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where HomepageSlideID=", Sql.Sqlize(homepageSlideIDValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("Title", titleValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SlidePicture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SlidePicture {
			get { return Fields.SlidePicture.Value; }
			set { fields["SlidePicture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadBySlidePicture(string slidePictureValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("SlidePicture", slidePictureValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField SlidePicture {
				get { return (PictureActiveField)fields["SlidePicture"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadBySlidePicture(string slidePictureValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where SlidePicture=", Sql.Sqlize(slidePictureValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkURL
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkURL {
			get { return Fields.LinkURL.Value; }
			set { fields["LinkURL"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByLinkURL(string linkURLValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("LinkURL", linkURLValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkURL {
				get { return (ActiveField<string>)fields["LinkURL"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByLinkURL(string linkURLValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where LinkURL=", Sql.Sqlize(linkURLValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("SortPosition", sortPositionValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("PublishDate", publishDateValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("ExpiryDate", expiryDateValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Duration
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? Duration {
			get { return Fields.Duration.Value; }
			set { fields["Duration"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByDuration(int? durationValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("Duration", durationValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> Duration {
				get { return (ActiveField<int?>)fields["Duration"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByDuration(int? durationValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where Duration=", Sql.Sqlize(durationValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyText {
			get { return Fields.BodyText.Value; }
			set { fields["BodyText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByBodyText(string bodyTextValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("BodyText", bodyTextValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyText {
				get { return (ActiveField<string>)fields["BodyText"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByBodyText(string bodyTextValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where BodyText=", Sql.Sqlize(bodyTextValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AltText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AltText {
			get { return Fields.AltText.Value; }
			set { fields["AltText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByAltText(string altTextValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("AltText", altTextValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AltText {
				get { return (ActiveField<string>)fields["AltText"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByAltText(string altTextValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where AltText=", Sql.Sqlize(altTextValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("DateAdded", dateAddedValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class HomepageSlide {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static HomepageSlide LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<HomepageSlide>("DateModified", dateModifiedValue, "HomepageSlide", Otherwise.Null);
		}

		public partial class HomepageSlideFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class HomepageSlideList {		
				
		[JetBrains.Annotations.NotNull]
		public static HomepageSlideList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","HomepageSlide".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return HomepageSlideList.Load(sql);
		}		
		
	}


}
#endregion