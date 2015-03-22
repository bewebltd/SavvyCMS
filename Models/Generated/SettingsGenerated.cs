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
// CLASS: Settings
// TABLE: Settings
//-----------------------------------------


	public partial class Settings : ActiveRecord {

		/// <summary>
		/// The list that this Settings is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Settings> GetContainingList() {
			return (ActiveRecordList<Settings>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Settings(): base("Settings", "SettingsID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Settings";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "SettingsID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property SettingsID.
		/// </summary>
		public int ID { get { return (int)fields["SettingsID"].ValueObject; } set { fields["SettingsID"].ValueObject = value; } }

		// field references
		public partial class SettingsFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public SettingsFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private SettingsFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public SettingsFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new SettingsFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Settings table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Settings</param>
		/// <returns>An instance of Settings containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Settings table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Settings.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Settings.LoadID(55);</example>
		/// <param name="id">Primary key of Settings</param>
		/// <returns>An instance of Settings containing the data in the record</returns>
		public static Settings LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Settings record = null;
//			record = Settings.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where SettingsID=", Sql.Sqlize(id));
//				record = new Settings();
//				if (!record.LoadData(sql)) return otherwise.Execute<Settings>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Settings>(id, "Settings", otherwise);
		}

		/// <summary>
		/// Loads a record from the Settings table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Settings containing the data in the record</returns>
		public static Settings Load(Sql sql) {
				return ActiveRecordLoader.Load<Settings>(sql, "Settings");
		}
		public static Settings Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Settings>(sql, "Settings", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Settings table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Settings containing the data in the record</returns>
		public static Settings Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Settings>(reader, "Settings");
		}
		public static Settings Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Settings>(reader, "Settings", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where SettingsID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("SettingsID", new ActiveField<int>() { Name = "SettingsID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Settings"  });

	fields.Add("WebmasterEmail", new ActiveField<string>() { Name = "WebmasterEmail", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="Settings"  });

	fields.Add("ScheduledTaskLastDailyRunTime", new ActiveField<System.DateTime?>() { Name = "ScheduledTaskLastDailyRunTime", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Settings"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Settings"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Settings"  });

	fields.Add("ShowAdminPanel", new ActiveField<bool>() { Name = "ShowAdminPanel", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Settings"  });

	fields.Add("AnalyticsTags", new ActiveField<string>() { Name = "AnalyticsTags", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="Settings"  });

	fields.Add("EnablePageRevisions", new ActiveField<bool>() { Name = "EnablePageRevisions", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Settings"  });

	fields.Add("EnableWorkflow", new ActiveField<bool>() { Name = "EnableWorkflow", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Settings"  });

	fields.Add("EnableRevisionEditing", new ActiveField<bool>() { Name = "EnableRevisionEditing", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Settings"  });

	fields.Add("PageTitleTagFormat", new ActiveField<string>() { Name = "PageTitleTagFormat", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Settings"  });
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
				var rec = ActiveRecordLoader.LoadID<Settings>(id, "Settings", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Settings with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Settings or null if not in cache.</returns>
//		private static Settings GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Settings-" + id) as Settings;
//			return Web.PageGlobals["ActiveRecord-Settings-" + id] as Settings;
//		}

		/// <summary>
		/// Caches this Settings object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Settings-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Settings-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Settings-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Settings objects/records. This is the usual data structure for holding a number of Settings records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class SettingsList : ActiveRecordList<Settings> {

		public SettingsList() : base() {}
		public SettingsList(List<Settings> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Settings to SettingsList. 
		/// </summary>
		public static implicit operator SettingsList(List<Settings> list) {
			return new SettingsList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from SettingsList to List-of-Settings. 
		/// </summary>
		public static implicit operator List<Settings>(SettingsList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Settings objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Settings records.</returns>
		public static SettingsList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where SettingsID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Settings objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Settings records.</returns>
		public static SettingsList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static SettingsList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where SettingsID in (", ids.SqlizeNumberList(), ")");
			var result = new SettingsList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Settings objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Settings records.</returns>
		public static SettingsList Load(Sql sql) {
			var result = new SettingsList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Settings objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and SettingsID desc.)
		/// </summary>
		public static SettingsList LoadAll() {
			var result = new SettingsList();
			result.LoadRecords(null);
			return result;
		}
		public static SettingsList LoadAll(int itemsPerPage) {
			var result = new SettingsList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static SettingsList LoadAll(int itemsPerPage, int pageNum) {
			var result = new SettingsList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Settings objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static SettingsList LoadActive() {
			var result = new SettingsList();
			var sql = (new Settings()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static SettingsList LoadActive(int itemsPerPage) {
			var result = new SettingsList();
			var sql = (new Settings()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static SettingsList LoadActive(int itemsPerPage, int pageNum) {
			var result = new SettingsList();
			var sql = (new Settings()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static SettingsList LoadActivePlusExisting(object existingRecordID) {
			var result = new SettingsList();
			var sql = (new Settings()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Settings");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Settings");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Settings()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Settings.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: SettingsID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public int SettingsID {
			get { return Fields.SettingsID.Value; }
			set { fields["SettingsID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadBySettingsID(int settingsIDValue) {
			return ActiveRecordLoader.LoadByField<Settings>("SettingsID", settingsIDValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> SettingsID {
				get { return (ActiveField<int>)fields["SettingsID"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadBySettingsID(int settingsIDValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where SettingsID=", Sql.Sqlize(settingsIDValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: WebmasterEmail
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public string WebmasterEmail {
			get { return Fields.WebmasterEmail.Value; }
			set { fields["WebmasterEmail"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByWebmasterEmail(string webmasterEmailValue) {
			return ActiveRecordLoader.LoadByField<Settings>("WebmasterEmail", webmasterEmailValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> WebmasterEmail {
				get { return (ActiveField<string>)fields["WebmasterEmail"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByWebmasterEmail(string webmasterEmailValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where WebmasterEmail=", Sql.Sqlize(webmasterEmailValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ScheduledTaskLastDailyRunTime
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ScheduledTaskLastDailyRunTime {
			get { return Fields.ScheduledTaskLastDailyRunTime.Value; }
			set { fields["ScheduledTaskLastDailyRunTime"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByScheduledTaskLastDailyRunTime(System.DateTime? scheduledTaskLastDailyRunTimeValue) {
			return ActiveRecordLoader.LoadByField<Settings>("ScheduledTaskLastDailyRunTime", scheduledTaskLastDailyRunTimeValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ScheduledTaskLastDailyRunTime {
				get { return (ActiveField<System.DateTime?>)fields["ScheduledTaskLastDailyRunTime"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByScheduledTaskLastDailyRunTime(System.DateTime? scheduledTaskLastDailyRunTimeValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where ScheduledTaskLastDailyRunTime=", Sql.Sqlize(scheduledTaskLastDailyRunTimeValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Settings>("DateAdded", dateAddedValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Settings>("DateModified", dateModifiedValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShowAdminPanel
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool ShowAdminPanel {
			get { return Fields.ShowAdminPanel.Value; }
			set { fields["ShowAdminPanel"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByShowAdminPanel(bool showAdminPanelValue) {
			return ActiveRecordLoader.LoadByField<Settings>("ShowAdminPanel", showAdminPanelValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> ShowAdminPanel {
				get { return (ActiveField<bool>)fields["ShowAdminPanel"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByShowAdminPanel(bool showAdminPanelValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where ShowAdminPanel=", Sql.Sqlize(showAdminPanelValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AnalyticsTags
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AnalyticsTags {
			get { return Fields.AnalyticsTags.Value; }
			set { fields["AnalyticsTags"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByAnalyticsTags(string analyticsTagsValue) {
			return ActiveRecordLoader.LoadByField<Settings>("AnalyticsTags", analyticsTagsValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AnalyticsTags {
				get { return (ActiveField<string>)fields["AnalyticsTags"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByAnalyticsTags(string analyticsTagsValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where AnalyticsTags=", Sql.Sqlize(analyticsTagsValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EnablePageRevisions
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool EnablePageRevisions {
			get { return Fields.EnablePageRevisions.Value; }
			set { fields["EnablePageRevisions"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByEnablePageRevisions(bool enablePageRevisionsValue) {
			return ActiveRecordLoader.LoadByField<Settings>("EnablePageRevisions", enablePageRevisionsValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> EnablePageRevisions {
				get { return (ActiveField<bool>)fields["EnablePageRevisions"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByEnablePageRevisions(bool enablePageRevisionsValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where EnablePageRevisions=", Sql.Sqlize(enablePageRevisionsValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EnableWorkflow
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool EnableWorkflow {
			get { return Fields.EnableWorkflow.Value; }
			set { fields["EnableWorkflow"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByEnableWorkflow(bool enableWorkflowValue) {
			return ActiveRecordLoader.LoadByField<Settings>("EnableWorkflow", enableWorkflowValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> EnableWorkflow {
				get { return (ActiveField<bool>)fields["EnableWorkflow"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByEnableWorkflow(bool enableWorkflowValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where EnableWorkflow=", Sql.Sqlize(enableWorkflowValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EnableRevisionEditing
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool EnableRevisionEditing {
			get { return Fields.EnableRevisionEditing.Value; }
			set { fields["EnableRevisionEditing"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByEnableRevisionEditing(bool enableRevisionEditingValue) {
			return ActiveRecordLoader.LoadByField<Settings>("EnableRevisionEditing", enableRevisionEditingValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> EnableRevisionEditing {
				get { return (ActiveField<bool>)fields["EnableRevisionEditing"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByEnableRevisionEditing(bool enableRevisionEditingValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where EnableRevisionEditing=", Sql.Sqlize(enableRevisionEditingValue));
			return SettingsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageTitleTagFormat
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Settings {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageTitleTagFormat {
			get { return Fields.PageTitleTagFormat.Value; }
			set { fields["PageTitleTagFormat"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Settings LoadByPageTitleTagFormat(string pageTitleTagFormatValue) {
			return ActiveRecordLoader.LoadByField<Settings>("PageTitleTagFormat", pageTitleTagFormatValue, "Settings", Otherwise.Null);
		}

		public partial class SettingsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageTitleTagFormat {
				get { return (ActiveField<string>)fields["PageTitleTagFormat"]; }
			}
		}

	}
	
	// define list class 
	public partial class SettingsList {		
				
		[JetBrains.Annotations.NotNull]
		public static SettingsList LoadByPageTitleTagFormat(string pageTitleTagFormatValue) {
			var sql = new Sql("select * from ","Settings".SqlizeName()," where PageTitleTagFormat=", Sql.Sqlize(pageTitleTagFormatValue));
			return SettingsList.Load(sql);
		}		
		
	}


}
#endregion