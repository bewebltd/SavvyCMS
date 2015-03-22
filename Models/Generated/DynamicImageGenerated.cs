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
// CLASS: DynamicImage
// TABLE: DynamicImage
//-----------------------------------------


	public partial class DynamicImage : ActiveRecord {

		/// <summary>
		/// The list that this DynamicImage is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<DynamicImage> GetContainingList() {
			return (ActiveRecordList<DynamicImage>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public DynamicImage(): base("DynamicImage", "DynamicImageID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "DynamicImage";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "DynamicImageID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property DynamicImageID.
		/// </summary>
		public int ID { get { return (int)fields["DynamicImageID"].ValueObject; } set { fields["DynamicImageID"].ValueObject = value; } }

		// field references
		public partial class DynamicImageFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public DynamicImageFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private DynamicImageFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public DynamicImageFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new DynamicImageFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the DynamicImage table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of DynamicImage</param>
		/// <returns>An instance of DynamicImage containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the DynamicImage table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg DynamicImage.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = DynamicImage.LoadID(55);</example>
		/// <param name="id">Primary key of DynamicImage</param>
		/// <returns>An instance of DynamicImage containing the data in the record</returns>
		public static DynamicImage LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			DynamicImage record = null;
//			record = DynamicImage.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where DynamicImageID=", Sql.Sqlize(id));
//				record = new DynamicImage();
//				if (!record.LoadData(sql)) return otherwise.Execute<DynamicImage>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<DynamicImage>(id, "DynamicImage", otherwise);
		}

		/// <summary>
		/// Loads a record from the DynamicImage table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of DynamicImage containing the data in the record</returns>
		public static DynamicImage Load(Sql sql) {
				return ActiveRecordLoader.Load<DynamicImage>(sql, "DynamicImage");
		}
		public static DynamicImage Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<DynamicImage>(sql, "DynamicImage", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the DynamicImage table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of DynamicImage containing the data in the record</returns>
		public static DynamicImage Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<DynamicImage>(reader, "DynamicImage");
		}
		public static DynamicImage Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<DynamicImage>(reader, "DynamicImage", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where DynamicImageID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("DynamicImageID", new ActiveField<int>() { Name = "DynamicImageID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="DynamicImage"  });

	fields.Add("ImageUrl", new PictureActiveField() { Name = "ImageUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="DynamicImage"  });

	fields.Add("UniqueKey", new ActiveField<string>() { Name = "UniqueKey", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="DynamicImage"  });

	fields.Add("Version", new ActiveField<int?>() { Name = "Version", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DynamicImage"  });

	fields.Add("Width", new ActiveField<int?>() { Name = "Width", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DynamicImage"  });

	fields.Add("Height", new ActiveField<int?>() { Name = "Height", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DynamicImage"  });

	fields.Add("CropStyle", new ActiveField<string>() { Name = "CropStyle", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=10, TableName="DynamicImage"  });

	fields.Add("OriginalFilename", new ActiveField<string>() { Name = "OriginalFilename", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="DynamicImage"  });

	fields.Add("ImageModDate", new ActiveField<System.DateTime?>() { Name = "ImageModDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DynamicImage"  });

	fields.Add("LastModified", new ActiveField<System.DateTime?>() { Name = "LastModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DynamicImage"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DynamicImage"  });
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
				var rec = ActiveRecordLoader.LoadID<DynamicImage>(id, "DynamicImage", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the DynamicImage with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct DynamicImage or null if not in cache.</returns>
//		private static DynamicImage GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-DynamicImage-" + id) as DynamicImage;
//			return Web.PageGlobals["ActiveRecord-DynamicImage-" + id] as DynamicImage;
//		}

		/// <summary>
		/// Caches this DynamicImage object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-DynamicImage-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-DynamicImage-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-DynamicImage-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of DynamicImage objects/records. This is the usual data structure for holding a number of DynamicImage records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class DynamicImageList : ActiveRecordList<DynamicImage> {

		public DynamicImageList() : base() {}
		public DynamicImageList(List<DynamicImage> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-DynamicImage to DynamicImageList. 
		/// </summary>
		public static implicit operator DynamicImageList(List<DynamicImage> list) {
			return new DynamicImageList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from DynamicImageList to List-of-DynamicImage. 
		/// </summary>
		public static implicit operator List<DynamicImage>(DynamicImageList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of DynamicImage objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of DynamicImage records.</returns>
		public static DynamicImageList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where DynamicImageID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of DynamicImage objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of DynamicImage records.</returns>
		public static DynamicImageList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static DynamicImageList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where DynamicImageID in (", ids.SqlizeNumberList(), ")");
			var result = new DynamicImageList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of DynamicImage objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of DynamicImage records.</returns>
		public static DynamicImageList Load(Sql sql) {
			var result = new DynamicImageList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all DynamicImage objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and DynamicImageID desc.)
		/// </summary>
		public static DynamicImageList LoadAll() {
			var result = new DynamicImageList();
			result.LoadRecords(null);
			return result;
		}
		public static DynamicImageList LoadAll(int itemsPerPage) {
			var result = new DynamicImageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DynamicImageList LoadAll(int itemsPerPage, int pageNum) {
			var result = new DynamicImageList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" DynamicImage objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static DynamicImageList LoadActive() {
			var result = new DynamicImageList();
			var sql = (new DynamicImage()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static DynamicImageList LoadActive(int itemsPerPage) {
			var result = new DynamicImageList();
			var sql = (new DynamicImage()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DynamicImageList LoadActive(int itemsPerPage, int pageNum) {
			var result = new DynamicImageList();
			var sql = (new DynamicImage()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static DynamicImageList LoadActivePlusExisting(object existingRecordID) {
			var result = new DynamicImageList();
			var sql = (new DynamicImage()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM DynamicImage");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM DynamicImage");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new DynamicImage()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = DynamicImage.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: DynamicImageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int DynamicImageID {
			get { return Fields.DynamicImageID.Value; }
			set { fields["DynamicImageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByDynamicImageID(int dynamicImageIDValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("DynamicImageID", dynamicImageIDValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> DynamicImageID {
				get { return (ActiveField<int>)fields["DynamicImageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByDynamicImageID(int dynamicImageIDValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where DynamicImageID=", Sql.Sqlize(dynamicImageIDValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ImageUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ImageUrl {
			get { return Fields.ImageUrl.Value; }
			set { fields["ImageUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByImageUrl(string imageUrlValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("ImageUrl", imageUrlValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField ImageUrl {
				get { return (PictureActiveField)fields["ImageUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByImageUrl(string imageUrlValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where ImageUrl=", Sql.Sqlize(imageUrlValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UniqueKey
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UniqueKey {
			get { return Fields.UniqueKey.Value; }
			set { fields["UniqueKey"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByUniqueKey(string uniqueKeyValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("UniqueKey", uniqueKeyValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> UniqueKey {
				get { return (ActiveField<string>)fields["UniqueKey"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByUniqueKey(string uniqueKeyValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where UniqueKey=", Sql.Sqlize(uniqueKeyValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Version
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? Version {
			get { return Fields.Version.Value; }
			set { fields["Version"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByVersion(int? versionValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("Version", versionValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> Version {
				get { return (ActiveField<int?>)fields["Version"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByVersion(int? versionValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where Version=", Sql.Sqlize(versionValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Width
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? Width {
			get { return Fields.Width.Value; }
			set { fields["Width"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByWidth(int? widthValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("Width", widthValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> Width {
				get { return (ActiveField<int?>)fields["Width"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByWidth(int? widthValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where Width=", Sql.Sqlize(widthValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Height
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? Height {
			get { return Fields.Height.Value; }
			set { fields["Height"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByHeight(int? heightValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("Height", heightValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> Height {
				get { return (ActiveField<int?>)fields["Height"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByHeight(int? heightValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where Height=", Sql.Sqlize(heightValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CropStyle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CropStyle {
			get { return Fields.CropStyle.Value; }
			set { fields["CropStyle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByCropStyle(string cropStyleValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("CropStyle", cropStyleValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CropStyle {
				get { return (ActiveField<string>)fields["CropStyle"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByCropStyle(string cropStyleValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where CropStyle=", Sql.Sqlize(cropStyleValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: OriginalFilename
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public string OriginalFilename {
			get { return Fields.OriginalFilename.Value; }
			set { fields["OriginalFilename"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByOriginalFilename(string originalFilenameValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("OriginalFilename", originalFilenameValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> OriginalFilename {
				get { return (ActiveField<string>)fields["OriginalFilename"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByOriginalFilename(string originalFilenameValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where OriginalFilename=", Sql.Sqlize(originalFilenameValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ImageModDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ImageModDate {
			get { return Fields.ImageModDate.Value; }
			set { fields["ImageModDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByImageModDate(System.DateTime? imageModDateValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("ImageModDate", imageModDateValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ImageModDate {
				get { return (ActiveField<System.DateTime?>)fields["ImageModDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByImageModDate(System.DateTime? imageModDateValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where ImageModDate=", Sql.Sqlize(imageModDateValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? LastModified {
			get { return Fields.LastModified.Value; }
			set { fields["LastModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByLastModified(System.DateTime? lastModifiedValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("LastModified", lastModifiedValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> LastModified {
				get { return (ActiveField<System.DateTime?>)fields["LastModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByLastModified(System.DateTime? lastModifiedValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where LastModified=", Sql.Sqlize(lastModifiedValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DynamicImage {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DynamicImage LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<DynamicImage>("DateAdded", dateAddedValue, "DynamicImage", Otherwise.Null);
		}

		public partial class DynamicImageFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class DynamicImageList {		
				
		[JetBrains.Annotations.NotNull]
		public static DynamicImageList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","DynamicImage".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return DynamicImageList.Load(sql);
		}		
		
	}


}
#endregion