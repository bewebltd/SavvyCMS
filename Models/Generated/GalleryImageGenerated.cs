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
// CLASS: GalleryImage
// TABLE: GalleryImage
//-----------------------------------------


	public partial class GalleryImage : ActiveRecord {

		/// <summary>
		/// The list that this GalleryImage is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<GalleryImage> GetContainingList() {
			return (ActiveRecordList<GalleryImage>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public GalleryImage(): base("GalleryImage", "GalleryImageID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "GalleryImage";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "GalleryImageID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property GalleryImageID.
		/// </summary>
		public int ID { get { return (int)fields["GalleryImageID"].ValueObject; } set { fields["GalleryImageID"].ValueObject = value; } }

		// field references
		public partial class GalleryImageFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public GalleryImageFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private GalleryImageFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public GalleryImageFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new GalleryImageFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the GalleryImage table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of GalleryImage</param>
		/// <returns>An instance of GalleryImage containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the GalleryImage table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg GalleryImage.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = GalleryImage.LoadID(55);</example>
		/// <param name="id">Primary key of GalleryImage</param>
		/// <returns>An instance of GalleryImage containing the data in the record</returns>
		public static GalleryImage LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			GalleryImage record = null;
//			record = GalleryImage.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where GalleryImageID=", Sql.Sqlize(id));
//				record = new GalleryImage();
//				if (!record.LoadData(sql)) return otherwise.Execute<GalleryImage>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<GalleryImage>(id, "GalleryImage", otherwise);
		}

		/// <summary>
		/// Loads a record from the GalleryImage table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of GalleryImage containing the data in the record</returns>
		public static GalleryImage Load(Sql sql) {
				return ActiveRecordLoader.Load<GalleryImage>(sql, "GalleryImage");
		}
		public static GalleryImage Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GalleryImage>(sql, "GalleryImage", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the GalleryImage table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of GalleryImage containing the data in the record</returns>
		public static GalleryImage Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<GalleryImage>(reader, "GalleryImage");
		}
		public static GalleryImage Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GalleryImage>(reader, "GalleryImage", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where GalleryImageID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("GalleryImageID", new ActiveField<int>() { Name = "GalleryImageID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="GalleryImage"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GalleryImage"  });

	fields.Add("MediaType", new ActiveField<string>() { Name = "MediaType", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="GalleryImage"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="GalleryImage"  });

	fields.Add("PictureCaption", new ActiveField<string>() { Name = "PictureCaption", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GalleryImage"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GalleryImage"  });

	fields.Add("GalleryCategoryID", new ActiveField<int?>() { Name = "GalleryCategoryID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GalleryImage" , GetForeignRecord = () => this.GalleryCategory, ForeignClassName = typeof(Models.GalleryCategory), ForeignTableName = "GalleryCategory", ForeignTableFieldName = "GalleryCategoryID" });

	fields.Add("IsCoverImage", new ActiveField<bool>() { Name = "IsCoverImage", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="GalleryImage"  });

	fields.Add("DateTaken", new ActiveField<System.DateTime?>() { Name = "DateTaken", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryImage"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryImage"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryImage"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryImage"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GalleryImage"  });

	fields.Add("YouTubeVideoID", new ActiveField<string>() { Name = "YouTubeVideoID", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="GalleryImage"  });
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
				var rec = ActiveRecordLoader.LoadID<GalleryImage>(id, "GalleryImage", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the GalleryImage with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct GalleryImage or null if not in cache.</returns>
//		private static GalleryImage GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-GalleryImage-" + id) as GalleryImage;
//			return Web.PageGlobals["ActiveRecord-GalleryImage-" + id] as GalleryImage;
//		}

		/// <summary>
		/// Caches this GalleryImage object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-GalleryImage-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-GalleryImage-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-GalleryImage-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of GalleryImage objects/records. This is the usual data structure for holding a number of GalleryImage records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class GalleryImageList : ActiveRecordList<GalleryImage> {

		public GalleryImageList() : base() {}
		public GalleryImageList(List<GalleryImage> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-GalleryImage to GalleryImageList. 
		/// </summary>
		public static implicit operator GalleryImageList(List<GalleryImage> list) {
			return new GalleryImageList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from GalleryImageList to List-of-GalleryImage. 
		/// </summary>
		public static implicit operator List<GalleryImage>(GalleryImageList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of GalleryImage objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of GalleryImage records.</returns>
		public static GalleryImageList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where GalleryImageID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of GalleryImage objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of GalleryImage records.</returns>
		public static GalleryImageList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static GalleryImageList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where GalleryImageID in (", ids.SqlizeNumberList(), ")");
			var result = new GalleryImageList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of GalleryImage objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of GalleryImage records.</returns>
		public static GalleryImageList Load(Sql sql) {
			var result = new GalleryImageList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all GalleryImage objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and GalleryImageID desc.)
		/// </summary>
		public static GalleryImageList LoadAll() {
			var result = new GalleryImageList();
			result.LoadRecords(null);
			return result;
		}
		public static GalleryImageList LoadAll(int itemsPerPage) {
			var result = new GalleryImageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GalleryImageList LoadAll(int itemsPerPage, int pageNum) {
			var result = new GalleryImageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" GalleryImage objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static GalleryImageList LoadActive() {
			var result = new GalleryImageList();
			var sql = (new GalleryImage()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static GalleryImageList LoadActive(int itemsPerPage) {
			var result = new GalleryImageList();
			var sql = (new GalleryImage()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GalleryImageList LoadActive(int itemsPerPage, int pageNum) {
			var result = new GalleryImageList();
			var sql = (new GalleryImage()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static GalleryImageList LoadActivePlusExisting(object existingRecordID) {
			var result = new GalleryImageList();
			var sql = (new GalleryImage()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM GalleryImage");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM GalleryImage");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new GalleryImage()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = GalleryImage.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: GalleryImageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int GalleryImageID {
			get { return Fields.GalleryImageID.Value; }
			set { fields["GalleryImageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByGalleryImageID(int galleryImageIDValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("GalleryImageID", galleryImageIDValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> GalleryImageID {
				get { return (ActiveField<int>)fields["GalleryImageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByGalleryImageID(int galleryImageIDValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where GalleryImageID=", Sql.Sqlize(galleryImageIDValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("Title", titleValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MediaType
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MediaType {
			get { return Fields.MediaType.Value; }
			set { fields["MediaType"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByMediaType(string mediaTypeValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("MediaType", mediaTypeValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MediaType {
				get { return (ActiveField<string>)fields["MediaType"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByMediaType(string mediaTypeValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where MediaType=", Sql.Sqlize(mediaTypeValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("Picture", pictureValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PictureCaption
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PictureCaption {
			get { return Fields.PictureCaption.Value; }
			set { fields["PictureCaption"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByPictureCaption(string pictureCaptionValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("PictureCaption", pictureCaptionValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PictureCaption {
				get { return (ActiveField<string>)fields["PictureCaption"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByPictureCaption(string pictureCaptionValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where PictureCaption=", Sql.Sqlize(pictureCaptionValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("SortPosition", sortPositionValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: GalleryCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? GalleryCategoryID {
			get { return Fields.GalleryCategoryID.Value; }
			set { fields["GalleryCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByGalleryCategoryID(int? galleryCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("GalleryCategoryID", galleryCategoryIDValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> GalleryCategoryID {
				get { return (ActiveField<int?>)fields["GalleryCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByGalleryCategoryID(int? galleryCategoryIDValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where GalleryCategoryID=", Sql.Sqlize(galleryCategoryIDValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: GalleryCategory
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class GalleryImage {
		[NonSerialized]		
		private GalleryCategory _GalleryCategory;

		[JetBrains.Annotations.CanBeNull]
		public GalleryCategory GalleryCategory
		{
			get
			{
				 // lazy load
				if (this._GalleryCategory == null && this.GalleryCategoryID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("GalleryCategory") && container.PrefetchCounter["GalleryCategory"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.GalleryCategory>("GalleryCategoryID",container.Select(r=>r.GalleryCategoryID).ToList(),"GalleryCategory",Otherwise.Null);
					}
					this._GalleryCategory = Models.GalleryCategory.LoadByGalleryCategoryID(GalleryCategoryID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("GalleryCategory")) {
							container.PrefetchCounter["GalleryCategory"] = 0;
						}
						container.PrefetchCounter["GalleryCategory"]++;
					}
				}
				return this._GalleryCategory;
			}
			set
			{
				this._GalleryCategory = value;
			}
		}
	}

	public partial class GalleryImageList {
		internal int numFetchesOfGalleryCategory = 0;
	}
	
	// define list in partial foreign table class 
	public partial class GalleryCategory {
		[NonSerialized]		
		private GalleryImageList _GalleryImages;
		
		[JetBrains.Annotations.NotNull]
		public GalleryImageList GalleryImages
		{
			get
			{
				// lazy load
				if (this._GalleryImages == null) {
					this._GalleryImages = Models.GalleryImageList.LoadByGalleryCategoryID(this.ID);
					this._GalleryImages.SetParentBindField(this, "GalleryCategoryID");
				}
				return this._GalleryImages;
			}
			set
			{
				this._GalleryImages = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: IsCoverImage
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsCoverImage {
			get { return Fields.IsCoverImage.Value; }
			set { fields["IsCoverImage"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByIsCoverImage(bool isCoverImageValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("IsCoverImage", isCoverImageValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsCoverImage {
				get { return (ActiveField<bool>)fields["IsCoverImage"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByIsCoverImage(bool isCoverImageValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where IsCoverImage=", Sql.Sqlize(isCoverImageValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateTaken
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateTaken {
			get { return Fields.DateTaken.Value; }
			set { fields["DateTaken"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByDateTaken(System.DateTime? dateTakenValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("DateTaken", dateTakenValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateTaken {
				get { return (ActiveField<System.DateTime?>)fields["DateTaken"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByDateTaken(System.DateTime? dateTakenValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where DateTaken=", Sql.Sqlize(dateTakenValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("DateAdded", dateAddedValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("DateModified", dateModifiedValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("PublishDate", publishDateValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("ExpiryDate", expiryDateValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: YouTubeVideoID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GalleryImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string YouTubeVideoID {
			get { return Fields.YouTubeVideoID.Value; }
			set { fields["YouTubeVideoID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GalleryImage LoadByYouTubeVideoID(string youTubeVideoIDValue) {
			return ActiveRecordLoader.LoadByField<GalleryImage>("YouTubeVideoID", youTubeVideoIDValue, "GalleryImage", Otherwise.Null);
		}

		public partial class GalleryImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> YouTubeVideoID {
				get { return (ActiveField<string>)fields["YouTubeVideoID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GalleryImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static GalleryImageList LoadByYouTubeVideoID(string youTubeVideoIDValue) {
			var sql = new Sql("select * from ","GalleryImage".SqlizeName()," where YouTubeVideoID=", Sql.Sqlize(youTubeVideoIDValue));
			return GalleryImageList.Load(sql);
		}		
		
	}


}
#endregion