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
// CLASS: ClientContactUsRegion
// TABLE: ClientContactUsRegion
//-----------------------------------------


	public partial class ClientContactUsRegion : ActiveRecord {

		/// <summary>
		/// The list that this ClientContactUsRegion is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ClientContactUsRegion> GetContainingList() {
			return (ActiveRecordList<ClientContactUsRegion>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ClientContactUsRegion(): base("ClientContactUsRegion", "ClientContactUsRegionID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ClientContactUsRegion";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ClientContactUsRegionID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ClientContactUsRegionID.
		/// </summary>
		public int ID { get { return (int)fields["ClientContactUsRegionID"].ValueObject; } set { fields["ClientContactUsRegionID"].ValueObject = value; } }

		// field references
		public partial class ClientContactUsRegionFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ClientContactUsRegionFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ClientContactUsRegionFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ClientContactUsRegionFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ClientContactUsRegionFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ClientContactUsRegion table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ClientContactUsRegion</param>
		/// <returns>An instance of ClientContactUsRegion containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ClientContactUsRegion table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ClientContactUsRegion.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ClientContactUsRegion.LoadID(55);</example>
		/// <param name="id">Primary key of ClientContactUsRegion</param>
		/// <returns>An instance of ClientContactUsRegion containing the data in the record</returns>
		public static ClientContactUsRegion LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ClientContactUsRegion record = null;
//			record = ClientContactUsRegion.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ClientContactUsRegionID=", Sql.Sqlize(id));
//				record = new ClientContactUsRegion();
//				if (!record.LoadData(sql)) return otherwise.Execute<ClientContactUsRegion>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ClientContactUsRegion>(id, "ClientContactUsRegion", otherwise);
		}

		/// <summary>
		/// Loads a record from the ClientContactUsRegion table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ClientContactUsRegion containing the data in the record</returns>
		public static ClientContactUsRegion Load(Sql sql) {
				return ActiveRecordLoader.Load<ClientContactUsRegion>(sql, "ClientContactUsRegion");
		}
		public static ClientContactUsRegion Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ClientContactUsRegion>(sql, "ClientContactUsRegion", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ClientContactUsRegion table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ClientContactUsRegion containing the data in the record</returns>
		public static ClientContactUsRegion Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ClientContactUsRegion>(reader, "ClientContactUsRegion");
		}
		public static ClientContactUsRegion Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ClientContactUsRegion>(reader, "ClientContactUsRegion", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ClientContactUsRegionID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ClientContactUsRegionID", new ActiveField<int>() { Name = "ClientContactUsRegionID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ClientContactUsRegion"  });

	fields.Add("RegionName", new ActiveField<string>() { Name = "RegionName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=300, TableName="ClientContactUsRegion"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ClientContactUsRegion"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="ClientContactUsRegion"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ClientContactUsRegion"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ClientContactUsRegion"  });
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
				var rec = ActiveRecordLoader.LoadID<ClientContactUsRegion>(id, "ClientContactUsRegion", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ClientContactUsRegion with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ClientContactUsRegion or null if not in cache.</returns>
//		private static ClientContactUsRegion GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ClientContactUsRegion-" + id) as ClientContactUsRegion;
//			return Web.PageGlobals["ActiveRecord-ClientContactUsRegion-" + id] as ClientContactUsRegion;
//		}

		/// <summary>
		/// Caches this ClientContactUsRegion object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ClientContactUsRegion-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ClientContactUsRegion-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ClientContactUsRegion-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ClientContactUsRegion objects/records. This is the usual data structure for holding a number of ClientContactUsRegion records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ClientContactUsRegionList : ActiveRecordList<ClientContactUsRegion> {

		public ClientContactUsRegionList() : base() {}
		public ClientContactUsRegionList(List<ClientContactUsRegion> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ClientContactUsRegion to ClientContactUsRegionList. 
		/// </summary>
		public static implicit operator ClientContactUsRegionList(List<ClientContactUsRegion> list) {
			return new ClientContactUsRegionList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ClientContactUsRegionList to List-of-ClientContactUsRegion. 
		/// </summary>
		public static implicit operator List<ClientContactUsRegion>(ClientContactUsRegionList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ClientContactUsRegion objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ClientContactUsRegion records.</returns>
		public static ClientContactUsRegionList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ClientContactUsRegionID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ClientContactUsRegion objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ClientContactUsRegion records.</returns>
		public static ClientContactUsRegionList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ClientContactUsRegionList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ClientContactUsRegionID in (", ids.SqlizeNumberList(), ")");
			var result = new ClientContactUsRegionList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ClientContactUsRegion objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ClientContactUsRegion records.</returns>
		public static ClientContactUsRegionList Load(Sql sql) {
			var result = new ClientContactUsRegionList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ClientContactUsRegion objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ClientContactUsRegionID desc.)
		/// </summary>
		public static ClientContactUsRegionList LoadAll() {
			var result = new ClientContactUsRegionList();
			result.LoadRecords(null);
			return result;
		}
		public static ClientContactUsRegionList LoadAll(int itemsPerPage) {
			var result = new ClientContactUsRegionList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ClientContactUsRegionList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ClientContactUsRegionList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ClientContactUsRegion objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ClientContactUsRegionList LoadActive() {
			var result = new ClientContactUsRegionList();
			var sql = (new ClientContactUsRegion()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ClientContactUsRegionList LoadActive(int itemsPerPage) {
			var result = new ClientContactUsRegionList();
			var sql = (new ClientContactUsRegion()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ClientContactUsRegionList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ClientContactUsRegionList();
			var sql = (new ClientContactUsRegion()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ClientContactUsRegionList LoadActivePlusExisting(object existingRecordID) {
			var result = new ClientContactUsRegionList();
			var sql = (new ClientContactUsRegion()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ClientContactUsRegion");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ClientContactUsRegion");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ClientContactUsRegion()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ClientContactUsRegion.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ClientContactUsRegionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsRegion {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ClientContactUsRegionID {
			get { return Fields.ClientContactUsRegionID.Value; }
			set { fields["ClientContactUsRegionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadByClientContactUsRegionID(int clientContactUsRegionIDValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsRegion>("ClientContactUsRegionID", clientContactUsRegionIDValue, "ClientContactUsRegion", Otherwise.Null);
		}

		public partial class ClientContactUsRegionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ClientContactUsRegionID {
				get { return (ActiveField<int>)fields["ClientContactUsRegionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsRegionList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsRegionList LoadByClientContactUsRegionID(int clientContactUsRegionIDValue) {
			var sql = new Sql("select * from ","ClientContactUsRegion".SqlizeName()," where ClientContactUsRegionID=", Sql.Sqlize(clientContactUsRegionIDValue));
			return ClientContactUsRegionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: RegionName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsRegion {		
				
		[JetBrains.Annotations.CanBeNull]
		public string RegionName {
			get { return Fields.RegionName.Value; }
			set { fields["RegionName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadByRegionName(string regionNameValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsRegion>("RegionName", regionNameValue, "ClientContactUsRegion", Otherwise.Null);
		}

		public partial class ClientContactUsRegionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> RegionName {
				get { return (ActiveField<string>)fields["RegionName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsRegionList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsRegionList LoadByRegionName(string regionNameValue) {
			var sql = new Sql("select * from ","ClientContactUsRegion".SqlizeName()," where RegionName=", Sql.Sqlize(regionNameValue));
			return ClientContactUsRegionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsRegion {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsRegion>("SortPosition", sortPositionValue, "ClientContactUsRegion", Otherwise.Null);
		}

		public partial class ClientContactUsRegionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsRegionList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsRegionList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","ClientContactUsRegion".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return ClientContactUsRegionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsRegion {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsRegion>("IsPublished", isPublishedValue, "ClientContactUsRegion", Otherwise.Null);
		}

		public partial class ClientContactUsRegionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsRegionList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsRegionList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","ClientContactUsRegion".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return ClientContactUsRegionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsRegion {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsRegion>("DateAdded", dateAddedValue, "ClientContactUsRegion", Otherwise.Null);
		}

		public partial class ClientContactUsRegionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsRegionList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsRegionList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ClientContactUsRegion".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ClientContactUsRegionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ClientContactUsRegion {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ClientContactUsRegion LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ClientContactUsRegion>("DateModified", dateModifiedValue, "ClientContactUsRegion", Otherwise.Null);
		}

		public partial class ClientContactUsRegionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ClientContactUsRegionList {		
				
		[JetBrains.Annotations.NotNull]
		public static ClientContactUsRegionList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ClientContactUsRegion".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ClientContactUsRegionList.Load(sql);
		}		
		
	}


}
#endregion