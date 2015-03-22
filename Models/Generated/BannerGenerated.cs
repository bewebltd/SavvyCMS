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
// CLASS: Banner
// TABLE: Banner
//-----------------------------------------


	public partial class Banner : ActiveRecord {

		/// <summary>
		/// The list that this Banner is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Banner> GetContainingList() {
			return (ActiveRecordList<Banner>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Banner(): base("Banner", "BannerID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Banner";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "BannerID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property BannerID.
		/// </summary>
		public int ID { get { return (int)fields["BannerID"].ValueObject; } set { fields["BannerID"].ValueObject = value; } }

		// field references
		public partial class BannerFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public BannerFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private BannerFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public BannerFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new BannerFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Banner table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Banner</param>
		/// <returns>An instance of Banner containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Banner table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Banner.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Banner.LoadID(55);</example>
		/// <param name="id">Primary key of Banner</param>
		/// <returns>An instance of Banner containing the data in the record</returns>
		public static Banner LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Banner record = null;
//			record = Banner.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where BannerID=", Sql.Sqlize(id));
//				record = new Banner();
//				if (!record.LoadData(sql)) return otherwise.Execute<Banner>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Banner>(id, "Banner", otherwise);
		}

		/// <summary>
		/// Loads a record from the Banner table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Banner containing the data in the record</returns>
		public static Banner Load(Sql sql) {
				return ActiveRecordLoader.Load<Banner>(sql, "Banner");
		}
		public static Banner Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Banner>(sql, "Banner", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Banner table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Banner containing the data in the record</returns>
		public static Banner Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Banner>(reader, "Banner");
		}
		public static Banner Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Banner>(reader, "Banner", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where BannerID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("BannerID", new ActiveField<int>() { Name = "BannerID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Banner"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("BannerType", new ActiveField<string>() { Name = "BannerType", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("BannerAttachment", new AttachmentActiveField() { Name = "BannerAttachment", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("BannerLinkType", new ActiveField<string>() { Name = "BannerLinkType", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("UploadAttachment", new AttachmentActiveField() { Name = "UploadAttachment", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("ClickTagURL", new ActiveField<string>() { Name = "ClickTagURL", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="Banner"  });

	fields.Add("StartDate", new ActiveField<System.DateTime?>() { Name = "StartDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Banner"  });

	fields.Add("EndDate", new ActiveField<System.DateTime?>() { Name = "EndDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Banner"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Banner"  });

	fields.Add("Size", new ActiveField<string>() { Name = "Size", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Banner"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Banner"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Banner"  });
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
				var rec = ActiveRecordLoader.LoadID<Banner>(id, "Banner", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Banner with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Banner or null if not in cache.</returns>
//		private static Banner GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Banner-" + id) as Banner;
//			return Web.PageGlobals["ActiveRecord-Banner-" + id] as Banner;
//		}

		/// <summary>
		/// Caches this Banner object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Banner-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Banner-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Banner-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Banner objects/records. This is the usual data structure for holding a number of Banner records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class BannerList : ActiveRecordList<Banner> {

		public BannerList() : base() {}
		public BannerList(List<Banner> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Banner to BannerList. 
		/// </summary>
		public static implicit operator BannerList(List<Banner> list) {
			return new BannerList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from BannerList to List-of-Banner. 
		/// </summary>
		public static implicit operator List<Banner>(BannerList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Banner objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Banner records.</returns>
		public static BannerList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where BannerID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Banner objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Banner records.</returns>
		public static BannerList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static BannerList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where BannerID in (", ids.SqlizeNumberList(), ")");
			var result = new BannerList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Banner objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Banner records.</returns>
		public static BannerList Load(Sql sql) {
			var result = new BannerList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Banner objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and BannerID desc.)
		/// </summary>
		public static BannerList LoadAll() {
			var result = new BannerList();
			result.LoadRecords(null);
			return result;
		}
		public static BannerList LoadAll(int itemsPerPage) {
			var result = new BannerList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static BannerList LoadAll(int itemsPerPage, int pageNum) {
			var result = new BannerList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Banner objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static BannerList LoadActive() {
			var result = new BannerList();
			var sql = (new Banner()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static BannerList LoadActive(int itemsPerPage) {
			var result = new BannerList();
			var sql = (new Banner()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static BannerList LoadActive(int itemsPerPage, int pageNum) {
			var result = new BannerList();
			var sql = (new Banner()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static BannerList LoadActivePlusExisting(object existingRecordID) {
			var result = new BannerList();
			var sql = (new Banner()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Banner");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Banner");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Banner()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Banner.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: BannerID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public int BannerID {
			get { return Fields.BannerID.Value; }
			set { fields["BannerID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByBannerID(int bannerIDValue) {
			return ActiveRecordLoader.LoadByField<Banner>("BannerID", bannerIDValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> BannerID {
				get { return (ActiveField<int>)fields["BannerID"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByBannerID(int bannerIDValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where BannerID=", Sql.Sqlize(bannerIDValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Banner>("Title", titleValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BannerType
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BannerType {
			get { return Fields.BannerType.Value; }
			set { fields["BannerType"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByBannerType(string bannerTypeValue) {
			return ActiveRecordLoader.LoadByField<Banner>("BannerType", bannerTypeValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BannerType {
				get { return (ActiveField<string>)fields["BannerType"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByBannerType(string bannerTypeValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where BannerType=", Sql.Sqlize(bannerTypeValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<Banner>("Picture", pictureValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BannerAttachment
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BannerAttachment {
			get { return Fields.BannerAttachment.Value; }
			set { fields["BannerAttachment"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByBannerAttachment(string bannerAttachmentValue) {
			return ActiveRecordLoader.LoadByField<Banner>("BannerAttachment", bannerAttachmentValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField BannerAttachment {
				get { return (AttachmentActiveField)fields["BannerAttachment"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByBannerAttachment(string bannerAttachmentValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where BannerAttachment=", Sql.Sqlize(bannerAttachmentValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BannerLinkType
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BannerLinkType {
			get { return Fields.BannerLinkType.Value; }
			set { fields["BannerLinkType"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByBannerLinkType(string bannerLinkTypeValue) {
			return ActiveRecordLoader.LoadByField<Banner>("BannerLinkType", bannerLinkTypeValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BannerLinkType {
				get { return (ActiveField<string>)fields["BannerLinkType"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByBannerLinkType(string bannerLinkTypeValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where BannerLinkType=", Sql.Sqlize(bannerLinkTypeValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UploadAttachment
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UploadAttachment {
			get { return Fields.UploadAttachment.Value; }
			set { fields["UploadAttachment"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByUploadAttachment(string uploadAttachmentValue) {
			return ActiveRecordLoader.LoadByField<Banner>("UploadAttachment", uploadAttachmentValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField UploadAttachment {
				get { return (AttachmentActiveField)fields["UploadAttachment"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByUploadAttachment(string uploadAttachmentValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where UploadAttachment=", Sql.Sqlize(uploadAttachmentValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ClickTagURL
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ClickTagURL {
			get { return Fields.ClickTagURL.Value; }
			set { fields["ClickTagURL"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByClickTagURL(string clickTagURLValue) {
			return ActiveRecordLoader.LoadByField<Banner>("ClickTagURL", clickTagURLValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ClickTagURL {
				get { return (ActiveField<string>)fields["ClickTagURL"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByClickTagURL(string clickTagURLValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where ClickTagURL=", Sql.Sqlize(clickTagURLValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: StartDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? StartDate {
			get { return Fields.StartDate.Value; }
			set { fields["StartDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByStartDate(System.DateTime? startDateValue) {
			return ActiveRecordLoader.LoadByField<Banner>("StartDate", startDateValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> StartDate {
				get { return (ActiveField<System.DateTime?>)fields["StartDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByStartDate(System.DateTime? startDateValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where StartDate=", Sql.Sqlize(startDateValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EndDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? EndDate {
			get { return Fields.EndDate.Value; }
			set { fields["EndDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByEndDate(System.DateTime? endDateValue) {
			return ActiveRecordLoader.LoadByField<Banner>("EndDate", endDateValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> EndDate {
				get { return (ActiveField<System.DateTime?>)fields["EndDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByEndDate(System.DateTime? endDateValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where EndDate=", Sql.Sqlize(endDateValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<Banner>("IsPublished", isPublishedValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Size
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Size {
			get { return Fields.Size.Value; }
			set { fields["Size"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadBySize(string sizeValue) {
			return ActiveRecordLoader.LoadByField<Banner>("Size", sizeValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Size {
				get { return (ActiveField<string>)fields["Size"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadBySize(string sizeValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where Size=", Sql.Sqlize(sizeValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Banner>("DateAdded", dateAddedValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return BannerList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Banner {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Banner LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Banner>("DateModified", dateModifiedValue, "Banner", Otherwise.Null);
		}

		public partial class BannerFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class BannerList {		
				
		[JetBrains.Annotations.NotNull]
		public static BannerList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Banner".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return BannerList.Load(sql);
		}		
		
	}


}
#endregion