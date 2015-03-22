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
// CLASS: ClientContactUsPerson
// TABLE: ClientContactUsPerson
//-----------------------------------------


	public partial class ClientContactUsPerson : ActiveRecord {

		/// <summary>
		/// The list that this ClientContactUsPerson is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ClientContactUsPerson> GetContainingList() {
			return (ActiveRecordList<ClientContactUsPerson>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ClientContactUsPerson(): base("ClientContactUsPerson", "ClientContactUsPersonID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ClientContactUsPerson";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ClientContactUsPersonID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ClientContactUsPersonID.
		/// </summary>
		public int ID { get { return (int)fields["ClientContactUsPersonID"].ValueObject; } set { fields["ClientContactUsPersonID"].ValueObject = value; } }

		// field references
		public partial class ClientContactUsPersonFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ClientContactUsPersonFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ClientContactUsPersonFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ClientContactUsPersonFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ClientContactUsPersonFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ClientContactUsPerson table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ClientContactUsPerson</param>
		/// <returns>An instance of ClientContactUsPerson containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ClientContactUsPerson table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ClientContactUsPerson.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ClientContactUsPerson.LoadID(55);</example>
		/// <param name="id">Primary key of ClientContactUsPerson</param>
		/// <returns>An instance of ClientContactUsPerson containing the data in the record</returns>
		public static ClientContactUsPerson LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ClientContactUsPerson record = null;
//			record = ClientContactUsPerson.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ClientContactUsPersonID=", Sql.Sqlize(id));
//				record = new ClientContactUsPerson();
//				if (!record.LoadData(sql)) return otherwise.Execute<ClientContactUsPerson>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ClientContactUsPerson>(id, "ClientContactUsPerson", otherwise);
		}

		/// <summary>
		/// Loads a record from the ClientContactUsPerson table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ClientContactUsPerson containing the data in the record</returns>
		public static ClientContactUsPerson Load(Sql sql) {
				return ActiveRecordLoader.Load<ClientContactUsPerson>(sql, "ClientContactUsPerson");
		}
		public static ClientContactUsPerson Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ClientContactUsPerson>(sql, "ClientContactUsPerson", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ClientContactUsPerson table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ClientContactUsPerson containing the data in the record</returns>
		public static ClientContactUsPerson Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ClientContactUsPerson>(reader, "ClientContactUsPerson");
		}
		public static ClientContactUsPerson Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ClientContactUsPerson>(reader, "ClientContactUsPerson", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ClientContactUsPersonID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ClientContactUsPersonID", new ActiveField<int>() { Name = "ClientContactUsPersonID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ClientContactUsPerson"  });

	fields.Add("ClientContactUsRegionID", new ActiveField<int?>() { Name = "ClientContactUsRegionID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ClientContactUsPerson" , GetForeignRecord = () => this.ClientContactUsRegion, ForeignClassName = typeof(Models.ClientContactUsRegion), ForeignTableName = "ClientContactUsRegion", ForeignTableFieldName = "ClientContactUsRegionID" });

	fields.Add("PersonName", new ActiveField<string>() { Name = "PersonName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsPerson"  });

	fields.Add("PhotoPicture", new PictureActiveField() { Name = "PhotoPicture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsPerson"  });

	fields.Add("TelephoneNumber", new ActiveField<string>() { Name = "TelephoneNumber", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsPerson"  });

	fields.Add("EmailAddress", new ActiveField<string>() { Name = "EmailAddress", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ClientContactUsPerson"  });

	fields.Add("SkypeAddress", new ActiveField<string>() { Name = "SkypeAddress", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsPerson"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ClientContactUsPerson"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="ClientContactUsPerson"  });

	fields.Add("JobDescription", new ActiveField<string>() { Name = "JobDescription", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsPerson"  });

	fields.Add("Introduction", new ActiveField<string>() { Name = "Introduction", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsPerson"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ClientContactUsPerson"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ClientContactUsPerson"  });
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
				var rec = ActiveRecordLoader.LoadID<ClientContactUsPerson>(id, "ClientContactUsPerson", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ClientContactUsPerson with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ClientContactUsPerson or null if not in cache.</returns>
//		private static ClientContactUsPerson GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ClientContactUsPerson-" + id) as ClientContactUsPerson;
//			return Web.PageGlobals["ActiveRecord-ClientContactUsPerson-" + id] as ClientContactUsPerson;
//		}

		/// <summary>
		/// Caches this ClientContactUsPerson object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ClientContactUsPerson-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ClientContactUsPerson-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ClientContactUsPerson-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ClientContactUsPerson objects/records. This is the usual data structure for holding a number of ClientContactUsPerson records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ClientContactUsPersonList : ActiveRecordList<ClientContactUsPerson> {

		public ClientContactUsPersonList() : base() {}
		public ClientContactUsPersonList(List<ClientContactUsPerson> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ClientContactUsPerson to ClientContactUsPersonList. 
		/// </summary>
		public static implicit operator ClientContactUsPersonList(List<ClientContactUsPerson> list) {
			return new ClientContactUsPersonList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ClientContactUsPersonList to List-of-ClientContactUsPerson. 
		/// </summary>
		public static implicit operator List<ClientContactUsPerson>(ClientContactUsPersonList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ClientContactUsPerson objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ClientContactUsPerson records.</returns>
		public static ClientContactUsPersonList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ClientContactUsPersonID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ClientContactUsPerson objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ClientContactUsPerson records.</returns>
		public static ClientContactUsPersonList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ClientContactUsPersonList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ClientContactUsPersonID in (", ids.SqlizeNumberList(), ")");
			var result = new ClientContactUsPersonList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ClientContactUsPerson objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ClientContactUsPerson records.</returns>
		public static ClientContactUsPersonList Load(Sql sql) {
			var result = new ClientContactUsPersonList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ClientContactUsPerson objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ClientContactUsPersonID desc.)
		/// </summary>
		public static ClientContactUsPersonList LoadAll() {
			var result = new ClientContactUsPersonList();
			result.LoadRecords(null);
			return result;
		}
		public static ClientContactUsPersonList LoadAll(int itemsPerPage) {
			var result = new ClientContactUsPersonList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ClientContactUsPersonList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ClientContactUsPersonList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ClientContactUsPerson objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ClientContactUsPersonList LoadActive() {
			var result = new ClientContactUsPersonList();
			var sql = (new ClientContactUsPerson()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ClientContactUsPersonList LoadActive(int itemsPerPage) {
			var result = new ClientContactUsPersonList();
			var sql = (new ClientContactUsPerson()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ClientContactUsPersonList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ClientContactUsPersonList();
			var sql = (new ClientContactUsPerson()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ClientContactUsPersonList LoadActivePlusExisting(object existingRecordID) {
			var result = new ClientContactUsPersonList();
			var sql = (new ClientContactUsPerson()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ClientContactUsPerson");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ClientContactUsPerson");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ClientContactUsPerson()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ClientContactUsPerson.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ClientContactUsPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ClientContactUsPersonID {
			get { return Fields.ClientContactUsPersonID.Value; }
			set { fields["ClientContactUsPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByClientContactUsPersonID(int clientContactUsPersonIDValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("ClientContactUsPersonID", clientContactUsPersonIDValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ClientContactUsPersonID {
				get { return (ActiveField<int>)fields["ClientContactUsPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByClientContactUsPersonID(int clientContactUsPersonIDValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where ClientContactUsPersonID=", Sql.Sqlize(clientContactUsPersonIDValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ClientContactUsRegionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ClientContactUsRegionID {
			get { return Fields.ClientContactUsRegionID.Value; }
			set { fields["ClientContactUsRegionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByClientContactUsRegionID(int? clientContactUsRegionIDValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("ClientContactUsRegionID", clientContactUsRegionIDValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ClientContactUsRegionID {
				get { return (ActiveField<int?>)fields["ClientContactUsRegionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByClientContactUsRegionID(int? clientContactUsRegionIDValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where ClientContactUsRegionID=", Sql.Sqlize(clientContactUsRegionIDValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ClientContactUsRegion
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ClientContactUsPerson {
		[NonSerialized]		
		private ClientContactUsRegion _ClientContactUsRegion;

		[JetBrains.Annotations.CanBeNull]
		public ClientContactUsRegion ClientContactUsRegion
		{
			get
			{
				 // lazy load
				if (this._ClientContactUsRegion == null && this.ClientContactUsRegionID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ClientContactUsRegion") && container.PrefetchCounter["ClientContactUsRegion"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.ClientContactUsRegion>("ClientContactUsRegionID",container.Select(r=>r.ClientContactUsRegionID).ToList(),"ClientContactUsRegion",Otherwise.Null);
					}
					this._ClientContactUsRegion = Models.ClientContactUsRegion.LoadByClientContactUsRegionID(ClientContactUsRegionID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ClientContactUsRegion")) {
							container.PrefetchCounter["ClientContactUsRegion"] = 0;
						}
						container.PrefetchCounter["ClientContactUsRegion"]++;
					}
				}
				return this._ClientContactUsRegion;
			}
			set
			{
				this._ClientContactUsRegion = value;
			}
		}
	}

	public partial class ClientContactUsPersonList {
		internal int numFetchesOfClientContactUsRegion = 0;
	}
	
	// define list in partial foreign table class 
	public partial class ClientContactUsRegion {
		[NonSerialized]		
		private ClientContactUsPersonList _ClientContactUsPeople;
		
		[JetBrains.Annotations.NotNull]
		public ClientContactUsPersonList ClientContactUsPeople
		{
			get
			{
				// lazy load
				if (this._ClientContactUsPeople == null) {
					this._ClientContactUsPeople = Models.ClientContactUsPersonList.LoadByClientContactUsRegionID(this.ID);
					this._ClientContactUsPeople.SetParentBindField(this, "ClientContactUsRegionID");
				}
				return this._ClientContactUsPeople;
			}
			set
			{
				this._ClientContactUsPeople = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: PersonName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PersonName {
			get { return Fields.PersonName.Value; }
			set { fields["PersonName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByPersonName(string personNameValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("PersonName", personNameValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PersonName {
				get { return (ActiveField<string>)fields["PersonName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByPersonName(string personNameValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where PersonName=", Sql.Sqlize(personNameValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PhotoPicture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PhotoPicture {
			get { return Fields.PhotoPicture.Value; }
			set { fields["PhotoPicture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByPhotoPicture(string photoPictureValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("PhotoPicture", photoPictureValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField PhotoPicture {
				get { return (PictureActiveField)fields["PhotoPicture"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByPhotoPicture(string photoPictureValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where PhotoPicture=", Sql.Sqlize(photoPictureValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TelephoneNumber
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string TelephoneNumber {
			get { return Fields.TelephoneNumber.Value; }
			set { fields["TelephoneNumber"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByTelephoneNumber(string telephoneNumberValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("TelephoneNumber", telephoneNumberValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> TelephoneNumber {
				get { return (ActiveField<string>)fields["TelephoneNumber"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByTelephoneNumber(string telephoneNumberValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where TelephoneNumber=", Sql.Sqlize(telephoneNumberValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: EmailAddress
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string EmailAddress {
			get { return Fields.EmailAddress.Value; }
			set { fields["EmailAddress"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByEmailAddress(string emailAddressValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("EmailAddress", emailAddressValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> EmailAddress {
				get { return (ActiveField<string>)fields["EmailAddress"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByEmailAddress(string emailAddressValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where EmailAddress=", Sql.Sqlize(emailAddressValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SkypeAddress
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SkypeAddress {
			get { return Fields.SkypeAddress.Value; }
			set { fields["SkypeAddress"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadBySkypeAddress(string skypeAddressValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("SkypeAddress", skypeAddressValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SkypeAddress {
				get { return (ActiveField<string>)fields["SkypeAddress"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadBySkypeAddress(string skypeAddressValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where SkypeAddress=", Sql.Sqlize(skypeAddressValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("SortPosition", sortPositionValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("IsPublished", isPublishedValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: JobDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string JobDescription {
			get { return Fields.JobDescription.Value; }
			set { fields["JobDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByJobDescription(string jobDescriptionValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("JobDescription", jobDescriptionValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> JobDescription {
				get { return (ActiveField<string>)fields["JobDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByJobDescription(string jobDescriptionValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where JobDescription=", Sql.Sqlize(jobDescriptionValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Introduction
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Introduction {
			get { return Fields.Introduction.Value; }
			set { fields["Introduction"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByIntroduction(string introductionValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("Introduction", introductionValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Introduction {
				get { return (ActiveField<string>)fields["Introduction"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByIntroduction(string introductionValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where Introduction=", Sql.Sqlize(introductionValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("DateAdded", dateAddedValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsPerson {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsPerson LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsPerson>("DateModified", dateModifiedValue, "ClientContactUsPerson", Otherwise.Null);
		}

		public partial class ClientContactUsPersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsPersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsPersonList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ClientContactUsPerson".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ClientContactUsPersonList.Load(sql);
		}		
		
	}


}
#endregion