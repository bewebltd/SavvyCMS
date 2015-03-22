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
// CLASS: LiveChat
// TABLE: LiveChat
//-----------------------------------------


	public partial class LiveChat : ActiveRecord {

		/// <summary>
		/// The list that this LiveChat is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<LiveChat> GetContainingList() {
			return (ActiveRecordList<LiveChat>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public LiveChat(): base("LiveChat", "LiveChatID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "LiveChat";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "LiveChatID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property LiveChatID.
		/// </summary>
		public int ID { get { return (int)fields["LiveChatID"].ValueObject; } set { fields["LiveChatID"].ValueObject = value; } }

		// field references
		public partial class LiveChatFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public LiveChatFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private LiveChatFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public LiveChatFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new LiveChatFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the LiveChat table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of LiveChat</param>
		/// <returns>An instance of LiveChat containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static LiveChat LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the LiveChat table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg LiveChat.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = LiveChat.LoadID(55);</example>
		/// <param name="id">Primary key of LiveChat</param>
		/// <returns>An instance of LiveChat containing the data in the record</returns>
		public static LiveChat LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			LiveChat record = null;
//			record = LiveChat.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where LiveChatID=", Sql.Sqlize(id));
//				record = new LiveChat();
//				if (!record.LoadData(sql)) return otherwise.Execute<LiveChat>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<LiveChat>(id, "LiveChat", otherwise);
		}

		/// <summary>
		/// Loads a record from the LiveChat table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of LiveChat containing the data in the record</returns>
		public static LiveChat Load(Sql sql) {
				return ActiveRecordLoader.Load<LiveChat>(sql, "LiveChat");
		}
		public static LiveChat Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<LiveChat>(sql, "LiveChat", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the LiveChat table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of LiveChat containing the data in the record</returns>
		public static LiveChat Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<LiveChat>(reader, "LiveChat");
		}
		public static LiveChat Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<LiveChat>(reader, "LiveChat", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where LiveChatID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("LiveChatID", new ActiveField<int>() { Name = "LiveChatID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="LiveChat"  });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="LiveChat" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("Post", new ActiveField<string>() { Name = "Post", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=500, TableName="LiveChat"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="LiveChat"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="LiveChat"  });
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
				var rec = ActiveRecordLoader.LoadID<LiveChat>(id, "LiveChat", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the LiveChat with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct LiveChat or null if not in cache.</returns>
//		private static LiveChat GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-LiveChat-" + id) as LiveChat;
//			return Web.PageGlobals["ActiveRecord-LiveChat-" + id] as LiveChat;
//		}

		/// <summary>
		/// Caches this LiveChat object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-LiveChat-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-LiveChat-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-LiveChat-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of LiveChat objects/records. This is the usual data structure for holding a number of LiveChat records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class LiveChatList : ActiveRecordList<LiveChat> {

		public LiveChatList() : base() {}
		public LiveChatList(List<LiveChat> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-LiveChat to LiveChatList. 
		/// </summary>
		public static implicit operator LiveChatList(List<LiveChat> list) {
			return new LiveChatList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from LiveChatList to List-of-LiveChat. 
		/// </summary>
		public static implicit operator List<LiveChat>(LiveChatList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of LiveChat objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of LiveChat records.</returns>
		public static LiveChatList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where LiveChatID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of LiveChat objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of LiveChat records.</returns>
		public static LiveChatList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static LiveChatList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where LiveChatID in (", ids.SqlizeNumberList(), ")");
			var result = new LiveChatList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of LiveChat objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of LiveChat records.</returns>
		public static LiveChatList Load(Sql sql) {
			var result = new LiveChatList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all LiveChat objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and LiveChatID desc.)
		/// </summary>
		public static LiveChatList LoadAll() {
			var result = new LiveChatList();
			result.LoadRecords(null);
			return result;
		}
		public static LiveChatList LoadAll(int itemsPerPage) {
			var result = new LiveChatList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static LiveChatList LoadAll(int itemsPerPage, int pageNum) {
			var result = new LiveChatList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" LiveChat objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static LiveChatList LoadActive() {
			var result = new LiveChatList();
			var sql = (new LiveChat()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static LiveChatList LoadActive(int itemsPerPage) {
			var result = new LiveChatList();
			var sql = (new LiveChat()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static LiveChatList LoadActive(int itemsPerPage, int pageNum) {
			var result = new LiveChatList();
			var sql = (new LiveChat()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static LiveChatList LoadActivePlusExisting(object existingRecordID) {
			var result = new LiveChatList();
			var sql = (new LiveChat()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM LiveChat");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM LiveChat");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new LiveChat()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = LiveChat.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: LiveChatID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class LiveChat {		
				
		[JetBrains.Annotations.CanBeNull]
		public int LiveChatID {
			get { return Fields.LiveChatID.Value; }
			set { fields["LiveChatID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static LiveChat LoadByLiveChatID(int liveChatIDValue) {
			return ActiveRecordLoader.LoadByField<LiveChat>("LiveChatID", liveChatIDValue, "LiveChat", Otherwise.Null);
		}

		public partial class LiveChatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> LiveChatID {
				get { return (ActiveField<int>)fields["LiveChatID"]; }
			}
		}

	}
	
	// define list class 
	public partial class LiveChatList {		
				
		[JetBrains.Annotations.NotNull]
		public static LiveChatList LoadByLiveChatID(int liveChatIDValue) {
			var sql = new Sql("select * from ","LiveChat".SqlizeName()," where LiveChatID=", Sql.Sqlize(liveChatIDValue));
			return LiveChatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class LiveChat {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static LiveChat LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<LiveChat>("PersonID", personIDValue, "LiveChat", Otherwise.Null);
		}

		public partial class LiveChatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class LiveChatList {		
				
		[JetBrains.Annotations.NotNull]
		public static LiveChatList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","LiveChat".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return LiveChatList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class LiveChat {
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

	public partial class LiveChatList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private LiveChatList _LiveChats;
		
		[JetBrains.Annotations.NotNull]
		public LiveChatList LiveChats
		{
			get
			{
				// lazy load
				if (this._LiveChats == null) {
					this._LiveChats = Models.LiveChatList.LoadByPersonID(this.ID);
					this._LiveChats.SetParentBindField(this, "PersonID");
				}
				return this._LiveChats;
			}
			set
			{
				this._LiveChats = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Post
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class LiveChat {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Post {
			get { return Fields.Post.Value; }
			set { fields["Post"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static LiveChat LoadByPost(string postValue) {
			return ActiveRecordLoader.LoadByField<LiveChat>("Post", postValue, "LiveChat", Otherwise.Null);
		}

		public partial class LiveChatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Post {
				get { return (ActiveField<string>)fields["Post"]; }
			}
		}

	}
	
	// define list class 
	public partial class LiveChatList {		
				
		[JetBrains.Annotations.NotNull]
		public static LiveChatList LoadByPost(string postValue) {
			var sql = new Sql("select * from ","LiveChat".SqlizeName()," where Post=", Sql.Sqlize(postValue));
			return LiveChatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class LiveChat {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static LiveChat LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<LiveChat>("DateAdded", dateAddedValue, "LiveChat", Otherwise.Null);
		}

		public partial class LiveChatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class LiveChatList {		
				
		[JetBrains.Annotations.NotNull]
		public static LiveChatList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","LiveChat".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return LiveChatList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class LiveChat {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static LiveChat LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<LiveChat>("DateModified", dateModifiedValue, "LiveChat", Otherwise.Null);
		}

		public partial class LiveChatFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class LiveChatList {		
				
		[JetBrains.Annotations.NotNull]
		public static LiveChatList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","LiveChat".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return LiveChatList.Load(sql);
		}		
		
	}


}
#endregion