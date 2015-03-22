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
// CLASS: Event
// TABLE: Event
//-----------------------------------------


	public partial class Event : ActiveRecord {

		/// <summary>
		/// The list that this Event is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Event> GetContainingList() {
			return (ActiveRecordList<Event>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Event(): base("Event", "EventID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Event";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "EventID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property EventID.
		/// </summary>
		public int ID { get { return (int)fields["EventID"].ValueObject; } set { fields["EventID"].ValueObject = value; } }

		// field references
		public partial class EventFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public EventFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private EventFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public EventFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new EventFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Event table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Event</param>
		/// <returns>An instance of Event containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Event LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Event table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Event.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Event.LoadID(55);</example>
		/// <param name="id">Primary key of Event</param>
		/// <returns>An instance of Event containing the data in the record</returns>
		public static Event LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Event record = null;
//			record = Event.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where EventID=", Sql.Sqlize(id));
//				record = new Event();
//				if (!record.LoadData(sql)) return otherwise.Execute<Event>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Event>(id, "Event", otherwise);
		}

		/// <summary>
		/// Loads a record from the Event table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Event containing the data in the record</returns>
		public static Event Load(Sql sql) {
				return ActiveRecordLoader.Load<Event>(sql, "Event");
		}
		public static Event Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Event>(sql, "Event", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Event table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Event containing the data in the record</returns>
		public static Event Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Event>(reader, "Event");
		}
		public static Event Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Event>(reader, "Event", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where EventID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("EventID", new ActiveField<int>() { Name = "EventID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Event"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Event"  });

	fields.Add("Location", new ActiveField<string>() { Name = "Location", ColumnType = "nchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Event"  });

	fields.Add("StartDate", new ActiveField<System.DateTime?>() { Name = "StartDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Event"  });

	fields.Add("Description", new ActiveField<string>() { Name = "Description", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Event"  });

	fields.Add("LinkURL", new ActiveField<string>() { Name = "LinkURL", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Event"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Event"  });

	fields.Add("EndDate", new ActiveField<System.DateTime?>() { Name = "EndDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Event"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Event"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Event"  });
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
				var rec = ActiveRecordLoader.LoadID<Event>(id, "Event", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Event with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Event or null if not in cache.</returns>
//		private static Event GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Event-" + id) as Event;
//			return Web.PageGlobals["ActiveRecord-Event-" + id] as Event;
//		}

		/// <summary>
		/// Caches this Event object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Event-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Event-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Event-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Event objects/records. This is the usual data structure for holding a number of Event records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class EventList : ActiveRecordList<Event> {

		public EventList() : base() {}
		public EventList(List<Event> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Event to EventList. 
		/// </summary>
		public static implicit operator EventList(List<Event> list) {
			return new EventList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from EventList to List-of-Event. 
		/// </summary>
		public static implicit operator List<Event>(EventList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Event objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Event records.</returns>
		public static EventList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where EventID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Event objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Event records.</returns>
		public static EventList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static EventList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where EventID in (", ids.SqlizeNumberList(), ")");
			var result = new EventList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Event objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Event records.</returns>
		public static EventList Load(Sql sql) {
			var result = new EventList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Event objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and EventID desc.)
		/// </summary>
		public static EventList LoadAll() {
			var result = new EventList();
			result.LoadRecords(null);
			return result;
		}
		public static EventList LoadAll(int itemsPerPage) {
			var result = new EventList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static EventList LoadAll(int itemsPerPage, int pageNum) {
			var result = new EventList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Event objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static EventList LoadActive() {
			var result = new EventList();
			var sql = (new Event()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static EventList LoadActive(int itemsPerPage) {
			var result = new EventList();
			var sql = (new Event()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static EventList LoadActive(int itemsPerPage, int pageNum) {
			var result = new EventList();
			var sql = (new Event()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static EventList LoadActivePlusExisting(object existingRecordID) {
			var result = new EventList();
			var sql = (new Event()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Event");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Event");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Event()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Event.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: EventID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public int EventID {
			get { return Fields.EventID.Value; }
			set { fields["EventID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByEventID(int eventIDValue) {
			return ActiveRecordLoader.LoadByField<Event>("EventID", eventIDValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> EventID {
				get { return (ActiveField<int>)fields["EventID"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByEventID(int eventIDValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where EventID=", Sql.Sqlize(eventIDValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Event>("Title", titleValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Location
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Location {
			get { return Fields.Location.Value; }
			set { fields["Location"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByLocation(string locationValue) {
			return ActiveRecordLoader.LoadByField<Event>("Location", locationValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Location {
				get { return (ActiveField<string>)fields["Location"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByLocation(string locationValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where Location=", Sql.Sqlize(locationValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: StartDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? StartDate {
			get { return Fields.StartDate.Value; }
			set { fields["StartDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByStartDate(System.DateTime? startDateValue) {
			return ActiveRecordLoader.LoadByField<Event>("StartDate", startDateValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> StartDate {
				get { return (ActiveField<System.DateTime?>)fields["StartDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByStartDate(System.DateTime? startDateValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where StartDate=", Sql.Sqlize(startDateValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Description
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Description {
			get { return Fields.Description.Value; }
			set { fields["Description"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByDescription(string descriptionValue) {
			return ActiveRecordLoader.LoadByField<Event>("Description", descriptionValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Description {
				get { return (ActiveField<string>)fields["Description"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByDescription(string descriptionValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where Description=", Sql.Sqlize(descriptionValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LinkURL
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LinkURL {
			get { return Fields.LinkURL.Value; }
			set { fields["LinkURL"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByLinkURL(string linkURLValue) {
			return ActiveRecordLoader.LoadByField<Event>("LinkURL", linkURLValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LinkURL {
				get { return (ActiveField<string>)fields["LinkURL"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByLinkURL(string linkURLValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where LinkURL=", Sql.Sqlize(linkURLValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<Event>("IsPublished", isPublishedValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EndDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? EndDate {
			get { return Fields.EndDate.Value; }
			set { fields["EndDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByEndDate(System.DateTime? endDateValue) {
			return ActiveRecordLoader.LoadByField<Event>("EndDate", endDateValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> EndDate {
				get { return (ActiveField<System.DateTime?>)fields["EndDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByEndDate(System.DateTime? endDateValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where EndDate=", Sql.Sqlize(endDateValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Event>("DateAdded", dateAddedValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return EventList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Event {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Event LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Event>("DateModified", dateModifiedValue, "Event", Otherwise.Null);
		}

		public partial class EventFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class EventList {		
				
		[JetBrains.Annotations.NotNull]
		public static EventList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Event".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return EventList.Load(sql);
		}		
		
	}


}
#endregion