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
// CLASS: MapLocation
// TABLE: MapLocation
//-----------------------------------------


	public partial class MapLocation : ActiveRecord {

		/// <summary>
		/// The list that this MapLocation is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<MapLocation> GetContainingList() {
			return (ActiveRecordList<MapLocation>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public MapLocation(): base("MapLocation", "MapLocationID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "MapLocation";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "MapLocationID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property MapLocationID.
		/// </summary>
		public int ID { get { return (int)fields["MapLocationID"].ValueObject; } set { fields["MapLocationID"].ValueObject = value; } }

		// field references
		public partial class MapLocationFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public MapLocationFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private MapLocationFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public MapLocationFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new MapLocationFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the MapLocation table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of MapLocation</param>
		/// <returns>An instance of MapLocation containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the MapLocation table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg MapLocation.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = MapLocation.LoadID(55);</example>
		/// <param name="id">Primary key of MapLocation</param>
		/// <returns>An instance of MapLocation containing the data in the record</returns>
		public static MapLocation LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			MapLocation record = null;
//			record = MapLocation.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where MapLocationID=", Sql.Sqlize(id));
//				record = new MapLocation();
//				if (!record.LoadData(sql)) return otherwise.Execute<MapLocation>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<MapLocation>(id, "MapLocation", otherwise);
		}

		/// <summary>
		/// Loads a record from the MapLocation table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of MapLocation containing the data in the record</returns>
		public static MapLocation Load(Sql sql) {
				return ActiveRecordLoader.Load<MapLocation>(sql, "MapLocation");
		}
		public static MapLocation Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<MapLocation>(sql, "MapLocation", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the MapLocation table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of MapLocation containing the data in the record</returns>
		public static MapLocation Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<MapLocation>(reader, "MapLocation");
		}
		public static MapLocation Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<MapLocation>(reader, "MapLocation", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where MapLocationID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("MapLocationID", new ActiveField<int>() { Name = "MapLocationID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="MapLocation"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MapLocation"  });

	fields.Add("EventType", new ActiveField<string>() { Name = "EventType", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="MapLocation"  });

	fields.Add("Latitude", new ActiveField<double?>() { Name = "Latitude", ColumnType = "float", Type = typeof(double?), IsAuto = false, MaxLength=8, TableName="MapLocation"  });

	fields.Add("Longitude", new ActiveField<double?>() { Name = "Longitude", ColumnType = "float", Type = typeof(double?), IsAuto = false, MaxLength=8, TableName="MapLocation"  });

	fields.Add("LocationName", new ActiveField<string>() { Name = "LocationName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MapLocation"  });

	fields.Add("LocationAddress", new ActiveField<string>() { Name = "LocationAddress", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="MapLocation"  });

	fields.Add("Dates", new ActiveField<string>() { Name = "Dates", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="MapLocation"  });

	fields.Add("StartTime", new ActiveField<System.DateTime?>() { Name = "StartTime", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MapLocation"  });

	fields.Add("MapRegionID", new ActiveField<int?>() { Name = "MapRegionID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="MapLocation" , GetForeignRecord = () => this.MapRegion, ForeignClassName = typeof(Models.MapRegion), ForeignTableName = "MapRegion", ForeignTableFieldName = "MapRegionID" });

	fields.Add("MoreInfoTextHtml", new ActiveField<string>() { Name = "MoreInfoTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="MapLocation"  });

	fields.Add("LinkUrl", new ActiveField<string>() { Name = "LinkUrl", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="MapLocation"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="MapLocation"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MapLocation"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="MapLocation"  });
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
				var rec = ActiveRecordLoader.LoadID<MapLocation>(id, "MapLocation", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the MapLocation with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct MapLocation or null if not in cache.</returns>
//		private static MapLocation GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-MapLocation-" + id) as MapLocation;
//			return Web.PageGlobals["ActiveRecord-MapLocation-" + id] as MapLocation;
//		}

		/// <summary>
		/// Caches this MapLocation object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-MapLocation-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-MapLocation-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-MapLocation-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of MapLocation objects/records. This is the usual data structure for holding a number of MapLocation records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class MapLocationList : ActiveRecordList<MapLocation> {

		public MapLocationList() : base() {}
		public MapLocationList(List<MapLocation> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-MapLocation to MapLocationList. 
		/// </summary>
		public static implicit operator MapLocationList(List<MapLocation> list) {
			return new MapLocationList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from MapLocationList to List-of-MapLocation. 
		/// </summary>
		public static implicit operator List<MapLocation>(MapLocationList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of MapLocation objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of MapLocation records.</returns>
		public static MapLocationList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where MapLocationID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of MapLocation objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of MapLocation records.</returns>
		public static MapLocationList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static MapLocationList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where MapLocationID in (", ids.SqlizeNumberList(), ")");
			var result = new MapLocationList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of MapLocation objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of MapLocation records.</returns>
		public static MapLocationList Load(Sql sql) {
			var result = new MapLocationList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all MapLocation objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and MapLocationID desc.)
		/// </summary>
		public static MapLocationList LoadAll() {
			var result = new MapLocationList();
			result.LoadRecords(null);
			return result;
		}
		public static MapLocationList LoadAll(int itemsPerPage) {
			var result = new MapLocationList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static MapLocationList LoadAll(int itemsPerPage, int pageNum) {
			var result = new MapLocationList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" MapLocation objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static MapLocationList LoadActive() {
			var result = new MapLocationList();
			var sql = (new MapLocation()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static MapLocationList LoadActive(int itemsPerPage) {
			var result = new MapLocationList();
			var sql = (new MapLocation()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static MapLocationList LoadActive(int itemsPerPage, int pageNum) {
			var result = new MapLocationList();
			var sql = (new MapLocation()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static MapLocationList LoadActivePlusExisting(object existingRecordID) {
			var result = new MapLocationList();
			var sql = (new MapLocation()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM MapLocation");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM MapLocation");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new MapLocation()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = MapLocation.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: MapLocationID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public int MapLocationID {
			get { return Fields.MapLocationID.Value; }
			set { fields["MapLocationID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByMapLocationID(int mapLocationIDValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("MapLocationID", mapLocationIDValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> MapLocationID {
				get { return (ActiveField<int>)fields["MapLocationID"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByMapLocationID(int mapLocationIDValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where MapLocationID=", Sql.Sqlize(mapLocationIDValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("Title", titleValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EventType
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EventType {
			get { return Fields.EventType.Value; }
			set { fields["EventType"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByEventType(string eventTypeValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("EventType", eventTypeValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EventType {
				get { return (ActiveField<string>)fields["EventType"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByEventType(string eventTypeValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where EventType=", Sql.Sqlize(eventTypeValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Latitude
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public double? Latitude {
			get { return Fields.Latitude.Value; }
			set { fields["Latitude"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByLatitude(double? latitudeValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("Latitude", latitudeValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<double?> Latitude {
				get { return (ActiveField<double?>)fields["Latitude"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByLatitude(double? latitudeValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where Latitude=", Sql.Sqlize(latitudeValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Longitude
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public double? Longitude {
			get { return Fields.Longitude.Value; }
			set { fields["Longitude"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByLongitude(double? longitudeValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("Longitude", longitudeValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<double?> Longitude {
				get { return (ActiveField<double?>)fields["Longitude"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByLongitude(double? longitudeValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where Longitude=", Sql.Sqlize(longitudeValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LocationName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LocationName {
			get { return Fields.LocationName.Value; }
			set { fields["LocationName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByLocationName(string locationNameValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("LocationName", locationNameValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LocationName {
				get { return (ActiveField<string>)fields["LocationName"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByLocationName(string locationNameValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where LocationName=", Sql.Sqlize(locationNameValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LocationAddress
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LocationAddress {
			get { return Fields.LocationAddress.Value; }
			set { fields["LocationAddress"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByLocationAddress(string locationAddressValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("LocationAddress", locationAddressValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LocationAddress {
				get { return (ActiveField<string>)fields["LocationAddress"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByLocationAddress(string locationAddressValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where LocationAddress=", Sql.Sqlize(locationAddressValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Dates
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Dates {
			get { return Fields.Dates.Value; }
			set { fields["Dates"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByDates(string datesValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("Dates", datesValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Dates {
				get { return (ActiveField<string>)fields["Dates"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByDates(string datesValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where Dates=", Sql.Sqlize(datesValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: StartTime
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? StartTime {
			get { return Fields.StartTime.Value; }
			set { fields["StartTime"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByStartTime(System.DateTime? startTimeValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("StartTime", startTimeValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> StartTime {
				get { return (ActiveField<System.DateTime?>)fields["StartTime"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByStartTime(System.DateTime? startTimeValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where StartTime=", Sql.Sqlize(startTimeValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MapRegionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? MapRegionID {
			get { return Fields.MapRegionID.Value; }
			set { fields["MapRegionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByMapRegionID(int? mapRegionIDValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("MapRegionID", mapRegionIDValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> MapRegionID {
				get { return (ActiveField<int?>)fields["MapRegionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByMapRegionID(int? mapRegionIDValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where MapRegionID=", Sql.Sqlize(mapRegionIDValue));
			return MapLocationList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: MapRegion
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class MapLocation {
		[NonSerialized]		
		private MapRegion _MapRegion;

		[JetBrains.Annotations.CanBeNull]
		public MapRegion MapRegion
		{
			get
			{
				 // lazy load
				if (this._MapRegion == null && this.MapRegionID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("MapRegion") && container.PrefetchCounter["MapRegion"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.MapRegion>("MapRegionID",container.Select(r=>r.MapRegionID).ToList(),"MapRegion",Otherwise.Null);
					}
					this._MapRegion = Models.MapRegion.LoadByMapRegionID(MapRegionID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("MapRegion")) {
							container.PrefetchCounter["MapRegion"] = 0;
						}
						container.PrefetchCounter["MapRegion"]++;
					}
				}
				return this._MapRegion;
			}
			set
			{
				this._MapRegion = value;
			}
		}
	}

	public partial class MapLocationList {
		internal int numFetchesOfMapRegion = 0;
	}
	
	// define list in partial foreign table class 
	public partial class MapRegion {
		[NonSerialized]		
		private MapLocationList _MapLocations;
		
		[JetBrains.Annotations.NotNull]
		public MapLocationList MapLocations
		{
			get
			{
				// lazy load
				if (this._MapLocations == null) {
					this._MapLocations = Models.MapLocationList.LoadByMapRegionID(this.ID);
					this._MapLocations.SetParentBindField(this, "MapRegionID");
				}
				return this._MapLocations;
			}
			set
			{
				this._MapLocations = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: MoreInfoTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MoreInfoTextHtml {
			get { return Fields.MoreInfoTextHtml.Value; }
			set { fields["MoreInfoTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByMoreInfoTextHtml(string moreInfoTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("MoreInfoTextHtml", moreInfoTextHtmlValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MoreInfoTextHtml {
				get { return (ActiveField<string>)fields["MoreInfoTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByMoreInfoTextHtml(string moreInfoTextHtmlValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where MoreInfoTextHtml=", Sql.Sqlize(moreInfoTextHtmlValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkUrl
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkUrl {
			get { return Fields.LinkUrl.Value; }
			set { fields["LinkUrl"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByLinkUrl(string linkUrlValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("LinkUrl", linkUrlValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkUrl {
				get { return (ActiveField<string>)fields["LinkUrl"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByLinkUrl(string linkUrlValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where LinkUrl=", Sql.Sqlize(linkUrlValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("IsActive", isActiveValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("DateAdded", dateAddedValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return MapLocationList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class MapLocation {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static MapLocation LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<MapLocation>("DateModified", dateModifiedValue, "MapLocation", Otherwise.Null);
		}

		public partial class MapLocationFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class MapLocationList {		
				
		[JetBrains.Annotations.NotNull]
		public static MapLocationList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","MapLocation".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return MapLocationList.Load(sql);
		}		
		
	}


}
#endregion