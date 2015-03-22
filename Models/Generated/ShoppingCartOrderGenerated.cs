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
// CLASS: ShoppingCartOrder
// TABLE: ShoppingCartOrder
//-----------------------------------------


	public partial class ShoppingCartOrder : ActiveRecord {

		/// <summary>
		/// The list that this ShoppingCartOrder is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ShoppingCartOrder> GetContainingList() {
			return (ActiveRecordList<ShoppingCartOrder>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ShoppingCartOrder(): base("ShoppingCartOrder", "ShoppingCartOrderID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ShoppingCartOrder";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ShoppingCartOrderID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ShoppingCartOrderID.
		/// </summary>
		public int ID { get { return (int)fields["ShoppingCartOrderID"].ValueObject; } set { fields["ShoppingCartOrderID"].ValueObject = value; } }

		// field references
		public partial class ShoppingCartOrderFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ShoppingCartOrderFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ShoppingCartOrderFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ShoppingCartOrderFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ShoppingCartOrderFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ShoppingCartOrder table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ShoppingCartOrder</param>
		/// <returns>An instance of ShoppingCartOrder containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ShoppingCartOrder table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ShoppingCartOrder.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ShoppingCartOrder.LoadID(55);</example>
		/// <param name="id">Primary key of ShoppingCartOrder</param>
		/// <returns>An instance of ShoppingCartOrder containing the data in the record</returns>
		public static ShoppingCartOrder LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ShoppingCartOrder record = null;
//			record = ShoppingCartOrder.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ShoppingCartOrderID=", Sql.Sqlize(id));
//				record = new ShoppingCartOrder();
//				if (!record.LoadData(sql)) return otherwise.Execute<ShoppingCartOrder>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ShoppingCartOrder>(id, "ShoppingCartOrder", otherwise);
		}

		/// <summary>
		/// Loads a record from the ShoppingCartOrder table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ShoppingCartOrder containing the data in the record</returns>
		public static ShoppingCartOrder Load(Sql sql) {
				return ActiveRecordLoader.Load<ShoppingCartOrder>(sql, "ShoppingCartOrder");
		}
		public static ShoppingCartOrder Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ShoppingCartOrder>(sql, "ShoppingCartOrder", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ShoppingCartOrder table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ShoppingCartOrder containing the data in the record</returns>
		public static ShoppingCartOrder Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ShoppingCartOrder>(reader, "ShoppingCartOrder");
		}
		public static ShoppingCartOrder Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ShoppingCartOrder>(reader, "ShoppingCartOrder", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ShoppingCartOrderID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ShoppingCartOrderID", new ActiveField<int>() { Name = "ShoppingCartOrderID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ShoppingCartOrder"  });

	fields.Add("OrderRef", new ActiveField<string>() { Name = "OrderRef", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ShoppingCartOrder"  });

	fields.Add("DateOrdered", new ActiveField<System.DateTime?>() { Name = "DateOrdered", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ShoppingCartOrder"  });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ShoppingCartOrder" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("Email", new ActiveField<string>() { Name = "Email", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ShoppingCartOrder"  });

	fields.Add("FirstName", new ActiveField<string>() { Name = "FirstName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ShoppingCartOrder"  });

	fields.Add("LastName", new ActiveField<string>() { Name = "LastName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ShoppingCartOrder"  });

	fields.Add("IsCostEnquiry", new ActiveField<bool>() { Name = "IsCostEnquiry", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="ShoppingCartOrder"  });

	fields.Add("Notes", new ActiveField<string>() { Name = "Notes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="ShoppingCartOrder"  });

	fields.Add("CustomerOrderReference", new ActiveField<string>() { Name = "CustomerOrderReference", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=60, TableName="ShoppingCartOrder"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ShoppingCartOrder"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ShoppingCartOrder"  });
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
				var rec = ActiveRecordLoader.LoadID<ShoppingCartOrder>(id, "ShoppingCartOrder", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ShoppingCartOrder with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ShoppingCartOrder or null if not in cache.</returns>
//		private static ShoppingCartOrder GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ShoppingCartOrder-" + id) as ShoppingCartOrder;
//			return Web.PageGlobals["ActiveRecord-ShoppingCartOrder-" + id] as ShoppingCartOrder;
//		}

		/// <summary>
		/// Caches this ShoppingCartOrder object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ShoppingCartOrder-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ShoppingCartOrder-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ShoppingCartOrder-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ShoppingCartOrder objects/records. This is the usual data structure for holding a number of ShoppingCartOrder records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ShoppingCartOrderList : ActiveRecordList<ShoppingCartOrder> {

		public ShoppingCartOrderList() : base() {}
		public ShoppingCartOrderList(List<ShoppingCartOrder> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ShoppingCartOrder to ShoppingCartOrderList. 
		/// </summary>
		public static implicit operator ShoppingCartOrderList(List<ShoppingCartOrder> list) {
			return new ShoppingCartOrderList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ShoppingCartOrderList to List-of-ShoppingCartOrder. 
		/// </summary>
		public static implicit operator List<ShoppingCartOrder>(ShoppingCartOrderList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ShoppingCartOrder objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ShoppingCartOrder records.</returns>
		public static ShoppingCartOrderList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ShoppingCartOrderID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ShoppingCartOrder objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ShoppingCartOrder records.</returns>
		public static ShoppingCartOrderList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ShoppingCartOrderList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ShoppingCartOrderID in (", ids.SqlizeNumberList(), ")");
			var result = new ShoppingCartOrderList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ShoppingCartOrder objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ShoppingCartOrder records.</returns>
		public static ShoppingCartOrderList Load(Sql sql) {
			var result = new ShoppingCartOrderList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ShoppingCartOrder objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ShoppingCartOrderID desc.)
		/// </summary>
		public static ShoppingCartOrderList LoadAll() {
			var result = new ShoppingCartOrderList();
			result.LoadRecords(null);
			return result;
		}
		public static ShoppingCartOrderList LoadAll(int itemsPerPage) {
			var result = new ShoppingCartOrderList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ShoppingCartOrderList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ShoppingCartOrderList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ShoppingCartOrder objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ShoppingCartOrderList LoadActive() {
			var result = new ShoppingCartOrderList();
			var sql = (new ShoppingCartOrder()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ShoppingCartOrderList LoadActive(int itemsPerPage) {
			var result = new ShoppingCartOrderList();
			var sql = (new ShoppingCartOrder()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ShoppingCartOrderList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ShoppingCartOrderList();
			var sql = (new ShoppingCartOrder()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ShoppingCartOrderList LoadActivePlusExisting(object existingRecordID) {
			var result = new ShoppingCartOrderList();
			var sql = (new ShoppingCartOrder()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ShoppingCartOrder");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ShoppingCartOrder");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ShoppingCartOrder()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ShoppingCartOrder.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ShoppingCartOrderID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ShoppingCartOrderID {
			get { return Fields.ShoppingCartOrderID.Value; }
			set { fields["ShoppingCartOrderID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByShoppingCartOrderID(int shoppingCartOrderIDValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("ShoppingCartOrderID", shoppingCartOrderIDValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ShoppingCartOrderID {
				get { return (ActiveField<int>)fields["ShoppingCartOrderID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByShoppingCartOrderID(int shoppingCartOrderIDValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where ShoppingCartOrderID=", Sql.Sqlize(shoppingCartOrderIDValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: OrderRef
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public string OrderRef {
			get { return Fields.OrderRef.Value; }
			set { fields["OrderRef"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByOrderRef(string orderRefValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("OrderRef", orderRefValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> OrderRef {
				get { return (ActiveField<string>)fields["OrderRef"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByOrderRef(string orderRefValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where OrderRef=", Sql.Sqlize(orderRefValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateOrdered
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateOrdered {
			get { return Fields.DateOrdered.Value; }
			set { fields["DateOrdered"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByDateOrdered(System.DateTime? dateOrderedValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("DateOrdered", dateOrderedValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateOrdered {
				get { return (ActiveField<System.DateTime?>)fields["DateOrdered"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByDateOrdered(System.DateTime? dateOrderedValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where DateOrdered=", Sql.Sqlize(dateOrderedValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("PersonID", personIDValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ShoppingCartOrder {
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

	public partial class ShoppingCartOrderList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private ShoppingCartOrderList _ShoppingCartOrders;
		
		[JetBrains.Annotations.NotNull]
		public ShoppingCartOrderList ShoppingCartOrders
		{
			get
			{
				// lazy load
				if (this._ShoppingCartOrders == null) {
					this._ShoppingCartOrders = Models.ShoppingCartOrderList.LoadByPersonID(this.ID);
					this._ShoppingCartOrders.SetParentBindField(this, "PersonID");
				}
				return this._ShoppingCartOrders;
			}
			set
			{
				this._ShoppingCartOrders = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Email
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Email {
			get { return Fields.Email.Value; }
			set { fields["Email"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByEmail(string emailValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("Email", emailValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Email {
				get { return (ActiveField<string>)fields["Email"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByEmail(string emailValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where Email=", Sql.Sqlize(emailValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FirstName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FirstName {
			get { return Fields.FirstName.Value; }
			set { fields["FirstName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByFirstName(string firstNameValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("FirstName", firstNameValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FirstName {
				get { return (ActiveField<string>)fields["FirstName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByFirstName(string firstNameValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where FirstName=", Sql.Sqlize(firstNameValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LastName {
			get { return Fields.LastName.Value; }
			set { fields["LastName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByLastName(string lastNameValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("LastName", lastNameValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LastName {
				get { return (ActiveField<string>)fields["LastName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByLastName(string lastNameValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where LastName=", Sql.Sqlize(lastNameValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsCostEnquiry
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsCostEnquiry {
			get { return Fields.IsCostEnquiry.Value; }
			set { fields["IsCostEnquiry"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByIsCostEnquiry(bool isCostEnquiryValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("IsCostEnquiry", isCostEnquiryValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsCostEnquiry {
				get { return (ActiveField<bool>)fields["IsCostEnquiry"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByIsCostEnquiry(bool isCostEnquiryValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where IsCostEnquiry=", Sql.Sqlize(isCostEnquiryValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Notes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Notes {
			get { return Fields.Notes.Value; }
			set { fields["Notes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByNotes(string notesValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("Notes", notesValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Notes {
				get { return (ActiveField<string>)fields["Notes"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByNotes(string notesValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where Notes=", Sql.Sqlize(notesValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CustomerOrderReference
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CustomerOrderReference {
			get { return Fields.CustomerOrderReference.Value; }
			set { fields["CustomerOrderReference"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByCustomerOrderReference(string customerOrderReferenceValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("CustomerOrderReference", customerOrderReferenceValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CustomerOrderReference {
				get { return (ActiveField<string>)fields["CustomerOrderReference"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByCustomerOrderReference(string customerOrderReferenceValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where CustomerOrderReference=", Sql.Sqlize(customerOrderReferenceValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("DateAdded", dateAddedValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCartOrder {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCartOrder LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCartOrder>("DateModified", dateModifiedValue, "ShoppingCartOrder", Otherwise.Null);
		}

		public partial class ShoppingCartOrderFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartOrderList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartOrderList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ShoppingCartOrder".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ShoppingCartOrderList.Load(sql);
		}		
		
	}


}
#endregion