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
// CLASS: Video
// TABLE: Video
//-----------------------------------------


	public partial class Video : ActiveRecord {

		/// <summary>
		/// The list that this Video is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Video> GetContainingList() {
			return (ActiveRecordList<Video>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Video(): base("Video", "VideoID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Video";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "VideoID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property VideoID.
		/// </summary>
		public int ID { get { return (int)fields["VideoID"].ValueObject; } set { fields["VideoID"].ValueObject = value; } }

		// field references
		public partial class VideoFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public VideoFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private VideoFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public VideoFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new VideoFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Video table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Video</param>
		/// <returns>An instance of Video containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Video LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Video table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Video.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Video.LoadID(55);</example>
		/// <param name="id">Primary key of Video</param>
		/// <returns>An instance of Video containing the data in the record</returns>
		public static Video LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Video record = null;
//			record = Video.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where VideoID=", Sql.Sqlize(id));
//				record = new Video();
//				if (!record.LoadData(sql)) return otherwise.Execute<Video>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Video>(id, "Video", otherwise);
		}

		/// <summary>
		/// Loads a record from the Video table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Video containing the data in the record</returns>
		public static Video Load(Sql sql) {
				return ActiveRecordLoader.Load<Video>(sql, "Video");
		}
		public static Video Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Video>(sql, "Video", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Video table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Video containing the data in the record</returns>
		public static Video Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Video>(reader, "Video");
		}
		public static Video Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Video>(reader, "Video", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where VideoID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("VideoID", new ActiveField<int>() { Name = "VideoID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Video"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Video"  });

	fields.Add("VideoPostedDate", new ActiveField<System.DateTime?>() { Name = "VideoPostedDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Video"  });

	fields.Add("BikeModelID", new ActiveField<int?>() { Name = "BikeModelID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Video"  });

	fields.Add("VideoDescription", new ActiveField<string>() { Name = "VideoDescription", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Video"  });

	fields.Add("SourceWebsiteCode", new ActiveField<string>() { Name = "SourceWebsiteCode", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=10, TableName="Video"  });

	fields.Add("VideoCode", new ActiveField<string>() { Name = "VideoCode", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Video"  });

	fields.Add("ThumbnailUrl", new ActiveField<string>() { Name = "ThumbnailUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Video"  });

	fields.Add("Credit", new ActiveField<string>() { Name = "Credit", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Video"  });

	fields.Add("IsAuto", new ActiveField<bool>() { Name = "IsAuto", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Video"  });

	fields.Add("ViewCount", new ActiveField<int?>() { Name = "ViewCount", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Video"  });

	fields.Add("PublishOnSite", new ActiveField<bool>() { Name = "PublishOnSite", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Video"  });

	fields.Add("Status", new ActiveField<string>() { Name = "Status", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=10, TableName="Video"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Video"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Video"  });
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
				var rec = ActiveRecordLoader.LoadID<Video>(id, "Video", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Video with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Video or null if not in cache.</returns>
//		private static Video GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Video-" + id) as Video;
//			return Web.PageGlobals["ActiveRecord-Video-" + id] as Video;
//		}

		/// <summary>
		/// Caches this Video object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Video-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Video-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Video-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Video objects/records. This is the usual data structure for holding a number of Video records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class VideoList : ActiveRecordList<Video> {

		public VideoList() : base() {}
		public VideoList(List<Video> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Video to VideoList. 
		/// </summary>
		public static implicit operator VideoList(List<Video> list) {
			return new VideoList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from VideoList to List-of-Video. 
		/// </summary>
		public static implicit operator List<Video>(VideoList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Video objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Video records.</returns>
		public static VideoList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where VideoID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Video objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Video records.</returns>
		public static VideoList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static VideoList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where VideoID in (", ids.SqlizeNumberList(), ")");
			var result = new VideoList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Video objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Video records.</returns>
		public static VideoList Load(Sql sql) {
			var result = new VideoList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Video objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and VideoID desc.)
		/// </summary>
		public static VideoList LoadAll() {
			var result = new VideoList();
			result.LoadRecords(null);
			return result;
		}
		public static VideoList LoadAll(int itemsPerPage) {
			var result = new VideoList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static VideoList LoadAll(int itemsPerPage, int pageNum) {
			var result = new VideoList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Video objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static VideoList LoadActive() {
			var result = new VideoList();
			var sql = (new Video()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static VideoList LoadActive(int itemsPerPage) {
			var result = new VideoList();
			var sql = (new Video()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static VideoList LoadActive(int itemsPerPage, int pageNum) {
			var result = new VideoList();
			var sql = (new Video()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static VideoList LoadActivePlusExisting(object existingRecordID) {
			var result = new VideoList();
			var sql = (new Video()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Video");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Video");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Video()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Video.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: VideoID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public int VideoID {
			get { return Fields.VideoID.Value; }
			set { fields["VideoID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByVideoID(int videoIDValue) {
			return ActiveRecordLoader.LoadByField<Video>("VideoID", videoIDValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> VideoID {
				get { return (ActiveField<int>)fields["VideoID"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByVideoID(int videoIDValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where VideoID=", Sql.Sqlize(videoIDValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Video>("Title", titleValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: VideoPostedDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? VideoPostedDate {
			get { return Fields.VideoPostedDate.Value; }
			set { fields["VideoPostedDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByVideoPostedDate(System.DateTime? videoPostedDateValue) {
			return ActiveRecordLoader.LoadByField<Video>("VideoPostedDate", videoPostedDateValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> VideoPostedDate {
				get { return (ActiveField<System.DateTime?>)fields["VideoPostedDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByVideoPostedDate(System.DateTime? videoPostedDateValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where VideoPostedDate=", Sql.Sqlize(videoPostedDateValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BikeModelID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? BikeModelID {
			get { return Fields.BikeModelID.Value; }
			set { fields["BikeModelID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByBikeModelID(int? bikeModelIDValue) {
			return ActiveRecordLoader.LoadByField<Video>("BikeModelID", bikeModelIDValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> BikeModelID {
				get { return (ActiveField<int?>)fields["BikeModelID"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByBikeModelID(int? bikeModelIDValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where BikeModelID=", Sql.Sqlize(bikeModelIDValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: VideoDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string VideoDescription {
			get { return Fields.VideoDescription.Value; }
			set { fields["VideoDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByVideoDescription(string videoDescriptionValue) {
			return ActiveRecordLoader.LoadByField<Video>("VideoDescription", videoDescriptionValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> VideoDescription {
				get { return (ActiveField<string>)fields["VideoDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByVideoDescription(string videoDescriptionValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where VideoDescription=", Sql.Sqlize(videoDescriptionValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SourceWebsiteCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SourceWebsiteCode {
			get { return Fields.SourceWebsiteCode.Value; }
			set { fields["SourceWebsiteCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadBySourceWebsiteCode(string sourceWebsiteCodeValue) {
			return ActiveRecordLoader.LoadByField<Video>("SourceWebsiteCode", sourceWebsiteCodeValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SourceWebsiteCode {
				get { return (ActiveField<string>)fields["SourceWebsiteCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadBySourceWebsiteCode(string sourceWebsiteCodeValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where SourceWebsiteCode=", Sql.Sqlize(sourceWebsiteCodeValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: VideoCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string VideoCode {
			get { return Fields.VideoCode.Value; }
			set { fields["VideoCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByVideoCode(string videoCodeValue) {
			return ActiveRecordLoader.LoadByField<Video>("VideoCode", videoCodeValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> VideoCode {
				get { return (ActiveField<string>)fields["VideoCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByVideoCode(string videoCodeValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where VideoCode=", Sql.Sqlize(videoCodeValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ThumbnailUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ThumbnailUrl {
			get { return Fields.ThumbnailUrl.Value; }
			set { fields["ThumbnailUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByThumbnailUrl(string thumbnailUrlValue) {
			return ActiveRecordLoader.LoadByField<Video>("ThumbnailUrl", thumbnailUrlValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ThumbnailUrl {
				get { return (ActiveField<string>)fields["ThumbnailUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByThumbnailUrl(string thumbnailUrlValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where ThumbnailUrl=", Sql.Sqlize(thumbnailUrlValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Credit
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Credit {
			get { return Fields.Credit.Value; }
			set { fields["Credit"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByCredit(string creditValue) {
			return ActiveRecordLoader.LoadByField<Video>("Credit", creditValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Credit {
				get { return (ActiveField<string>)fields["Credit"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByCredit(string creditValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where Credit=", Sql.Sqlize(creditValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsAuto
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsAuto {
			get { return Fields.IsAuto.Value; }
			set { fields["IsAuto"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByIsAuto(bool isAutoValue) {
			return ActiveRecordLoader.LoadByField<Video>("IsAuto", isAutoValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsAuto {
				get { return (ActiveField<bool>)fields["IsAuto"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByIsAuto(bool isAutoValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where IsAuto=", Sql.Sqlize(isAutoValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ViewCount
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ViewCount {
			get { return Fields.ViewCount.Value; }
			set { fields["ViewCount"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByViewCount(int? viewCountValue) {
			return ActiveRecordLoader.LoadByField<Video>("ViewCount", viewCountValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ViewCount {
				get { return (ActiveField<int?>)fields["ViewCount"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByViewCount(int? viewCountValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where ViewCount=", Sql.Sqlize(viewCountValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishOnSite
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool PublishOnSite {
			get { return Fields.PublishOnSite.Value; }
			set { fields["PublishOnSite"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByPublishOnSite(bool publishOnSiteValue) {
			return ActiveRecordLoader.LoadByField<Video>("PublishOnSite", publishOnSiteValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> PublishOnSite {
				get { return (ActiveField<bool>)fields["PublishOnSite"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByPublishOnSite(bool publishOnSiteValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where PublishOnSite=", Sql.Sqlize(publishOnSiteValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Status
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Status {
			get { return Fields.Status.Value; }
			set { fields["Status"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByStatus(string statusValue) {
			return ActiveRecordLoader.LoadByField<Video>("Status", statusValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Status {
				get { return (ActiveField<string>)fields["Status"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByStatus(string statusValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where Status=", Sql.Sqlize(statusValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Video>("DateAdded", dateAddedValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return VideoList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Video {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Video LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Video>("DateModified", dateModifiedValue, "Video", Otherwise.Null);
		}

		public partial class VideoFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class VideoList {		
				
		[JetBrains.Annotations.NotNull]
		public static VideoList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Video".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return VideoList.Load(sql);
		}		
		
	}


}
#endregion