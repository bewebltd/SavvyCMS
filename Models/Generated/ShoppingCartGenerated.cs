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
// CLASS: ShoppingCart
// TABLE: ShoppingCart
//-----------------------------------------


	public partial class ShoppingCart : ActiveRecord {

		/// <summary>
		/// The list that this ShoppingCart is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ShoppingCart> GetContainingList() {
			return (ActiveRecordList<ShoppingCart>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ShoppingCart(): base("ShoppingCart", "ShoppingCartID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ShoppingCart";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ShoppingCartID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ShoppingCartID.
		/// </summary>
		public int ID { get { return (int)fields["ShoppingCartID"].ValueObject; } set { fields["ShoppingCartID"].ValueObject = value; } }

		// field references
		public partial class ShoppingCartFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ShoppingCartFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ShoppingCartFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ShoppingCartFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ShoppingCartFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ShoppingCart table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ShoppingCart</param>
		/// <returns>An instance of ShoppingCart containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ShoppingCart table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ShoppingCart.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ShoppingCart.LoadID(55);</example>
		/// <param name="id">Primary key of ShoppingCart</param>
		/// <returns>An instance of ShoppingCart containing the data in the record</returns>
		public static ShoppingCart LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ShoppingCart record = null;
//			record = ShoppingCart.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ShoppingCartID=", Sql.Sqlize(id));
//				record = new ShoppingCart();
//				if (!record.LoadData(sql)) return otherwise.Execute<ShoppingCart>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ShoppingCart>(id, "ShoppingCart", otherwise);
		}

		/// <summary>
		/// Loads a record from the ShoppingCart table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ShoppingCart containing the data in the record</returns>
		public static ShoppingCart Load(Sql sql) {
				return ActiveRecordLoader.Load<ShoppingCart>(sql, "ShoppingCart");
		}
		public static ShoppingCart Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ShoppingCart>(sql, "ShoppingCart", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ShoppingCart table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ShoppingCart containing the data in the record</returns>
		public static ShoppingCart Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ShoppingCart>(reader, "ShoppingCart");
		}
		public static ShoppingCart Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ShoppingCart>(reader, "ShoppingCart", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ShoppingCartID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ShoppingCartID", new ActiveField<int>() { Name = "ShoppingCartID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ShoppingCart"  });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ShoppingCart" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("PartNumber", new ActiveField<int?>() { Name = "PartNumber", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ShoppingCart"  });

	fields.Add("PartDescription", new ActiveField<string>() { Name = "PartDescription", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="ShoppingCart"  });

	fields.Add("Quantity", new ActiveField<int?>() { Name = "Quantity", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ShoppingCart"  });

	fields.Add("Status", new ActiveField<string>() { Name = "Status", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ShoppingCart"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ShoppingCart"  });

	fields.Add("IsDeleted", new ActiveField<bool>() { Name = "IsDeleted", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="ShoppingCart"  });

	fields.Add("ShoppingCartOrderID", new ActiveField<int?>() { Name = "ShoppingCartOrderID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ShoppingCart" , GetForeignRecord = () => this.ShoppingCartOrder, ForeignClassName = typeof(Models.ShoppingCartOrder), ForeignTableName = "ShoppingCartOrder", ForeignTableFieldName = "ShoppingCartOrderID" });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ShoppingCart"  });
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
				var rec = ActiveRecordLoader.LoadID<ShoppingCart>(id, "ShoppingCart", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ShoppingCart with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ShoppingCart or null if not in cache.</returns>
//		private static ShoppingCart GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ShoppingCart-" + id) as ShoppingCart;
//			return Web.PageGlobals["ActiveRecord-ShoppingCart-" + id] as ShoppingCart;
//		}

		/// <summary>
		/// Caches this ShoppingCart object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ShoppingCart-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ShoppingCart-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ShoppingCart-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ShoppingCart objects/records. This is the usual data structure for holding a number of ShoppingCart records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ShoppingCartList : ActiveRecordList<ShoppingCart> {

		public ShoppingCartList() : base() {}
		public ShoppingCartList(List<ShoppingCart> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ShoppingCart to ShoppingCartList. 
		/// </summary>
		public static implicit operator ShoppingCartList(List<ShoppingCart> list) {
			return new ShoppingCartList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ShoppingCartList to List-of-ShoppingCart. 
		/// </summary>
		public static implicit operator List<ShoppingCart>(ShoppingCartList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ShoppingCart objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ShoppingCart records.</returns>
		public static ShoppingCartList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ShoppingCartID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ShoppingCart objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ShoppingCart records.</returns>
		public static ShoppingCartList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ShoppingCartList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ShoppingCartID in (", ids.SqlizeNumberList(), ")");
			var result = new ShoppingCartList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ShoppingCart objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ShoppingCart records.</returns>
		public static ShoppingCartList Load(Sql sql) {
			var result = new ShoppingCartList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ShoppingCart objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ShoppingCartID desc.)
		/// </summary>
		public static ShoppingCartList LoadAll() {
			var result = new ShoppingCartList();
			result.LoadRecords(null);
			return result;
		}
		public static ShoppingCartList LoadAll(int itemsPerPage) {
			var result = new ShoppingCartList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ShoppingCartList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ShoppingCartList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ShoppingCart objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ShoppingCartList LoadActive() {
			var result = new ShoppingCartList();
			var sql = (new ShoppingCart()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ShoppingCartList LoadActive(int itemsPerPage) {
			var result = new ShoppingCartList();
			var sql = (new ShoppingCart()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ShoppingCartList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ShoppingCartList();
			var sql = (new ShoppingCart()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ShoppingCartList LoadActivePlusExisting(object existingRecordID) {
			var result = new ShoppingCartList();
			var sql = (new ShoppingCart()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ShoppingCart");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ShoppingCart");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ShoppingCart()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ShoppingCart.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ShoppingCartID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ShoppingCartID {
			get { return Fields.ShoppingCartID.Value; }
			set { fields["ShoppingCartID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByShoppingCartID(int shoppingCartIDValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("ShoppingCartID", shoppingCartIDValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ShoppingCartID {
				get { return (ActiveField<int>)fields["ShoppingCartID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByShoppingCartID(int shoppingCartIDValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where ShoppingCartID=", Sql.Sqlize(shoppingCartIDValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("PersonID", personIDValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ShoppingCart {
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

	public partial class ShoppingCartList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private ShoppingCartList _ShoppingCarts;
		
		[JetBrains.Annotations.NotNull]
		public ShoppingCartList ShoppingCarts
		{
			get
			{
				// lazy load
				if (this._ShoppingCarts == null) {
					this._ShoppingCarts = Models.ShoppingCartList.LoadByPersonID(this.ID);
					this._ShoppingCarts.SetParentBindField(this, "PersonID");
				}
				return this._ShoppingCarts;
			}
			set
			{
				this._ShoppingCarts = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: PartNumber
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PartNumber {
			get { return Fields.PartNumber.Value; }
			set { fields["PartNumber"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByPartNumber(int? partNumberValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("PartNumber", partNumberValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PartNumber {
				get { return (ActiveField<int?>)fields["PartNumber"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByPartNumber(int? partNumberValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where PartNumber=", Sql.Sqlize(partNumberValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PartDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PartDescription {
			get { return Fields.PartDescription.Value; }
			set { fields["PartDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByPartDescription(string partDescriptionValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("PartDescription", partDescriptionValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PartDescription {
				get { return (ActiveField<string>)fields["PartDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByPartDescription(string partDescriptionValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where PartDescription=", Sql.Sqlize(partDescriptionValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Quantity
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? Quantity {
			get { return Fields.Quantity.Value; }
			set { fields["Quantity"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByQuantity(int? quantityValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("Quantity", quantityValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> Quantity {
				get { return (ActiveField<int?>)fields["Quantity"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByQuantity(int? quantityValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where Quantity=", Sql.Sqlize(quantityValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Status
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Status {
			get { return Fields.Status.Value; }
			set { fields["Status"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByStatus(string statusValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("Status", statusValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Status {
				get { return (ActiveField<string>)fields["Status"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByStatus(string statusValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where Status=", Sql.Sqlize(statusValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("DateAdded", dateAddedValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsDeleted
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsDeleted {
			get { return Fields.IsDeleted.Value; }
			set { fields["IsDeleted"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByIsDeleted(bool isDeletedValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("IsDeleted", isDeletedValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsDeleted {
				get { return (ActiveField<bool>)fields["IsDeleted"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByIsDeleted(bool isDeletedValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where IsDeleted=", Sql.Sqlize(isDeletedValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ShoppingCartOrderID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ShoppingCartOrderID {
			get { return Fields.ShoppingCartOrderID.Value; }
			set { fields["ShoppingCartOrderID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByShoppingCartOrderID(int? shoppingCartOrderIDValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("ShoppingCartOrderID", shoppingCartOrderIDValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ShoppingCartOrderID {
				get { return (ActiveField<int?>)fields["ShoppingCartOrderID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByShoppingCartOrderID(int? shoppingCartOrderIDValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where ShoppingCartOrderID=", Sql.Sqlize(shoppingCartOrderIDValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ShoppingCartOrder
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ShoppingCart {
		[NonSerialized]		
		private ShoppingCartOrder _ShoppingCartOrder;

		[JetBrains.Annotations.CanBeNull]
		public ShoppingCartOrder ShoppingCartOrder
		{
			get
			{
				 // lazy load
				if (this._ShoppingCartOrder == null && this.ShoppingCartOrderID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ShoppingCartOrder") && container.PrefetchCounter["ShoppingCartOrder"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.ShoppingCartOrder>("ShoppingCartOrderID",container.Select(r=>r.ShoppingCartOrderID).ToList(),"ShoppingCartOrder",Otherwise.Null);
					}
					this._ShoppingCartOrder = Models.ShoppingCartOrder.LoadByShoppingCartOrderID(ShoppingCartOrderID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ShoppingCartOrder")) {
							container.PrefetchCounter["ShoppingCartOrder"] = 0;
						}
						container.PrefetchCounter["ShoppingCartOrder"]++;
					}
				}
				return this._ShoppingCartOrder;
			}
			set
			{
				this._ShoppingCartOrder = value;
			}
		}
	}

	public partial class ShoppingCartList {
		internal int numFetchesOfShoppingCartOrder = 0;
	}
	
	// define list in partial foreign table class 
	public partial class ShoppingCartOrder {
		[NonSerialized]		
		private ShoppingCartList _ShoppingCarts;
		
		[JetBrains.Annotations.NotNull]
		public ShoppingCartList ShoppingCarts
		{
			get
			{
				// lazy load
				if (this._ShoppingCarts == null) {
					this._ShoppingCarts = Models.ShoppingCartList.LoadByShoppingCartOrderID(this.ID);
					this._ShoppingCarts.SetParentBindField(this, "ShoppingCartOrderID");
				}
				return this._ShoppingCarts;
			}
			set
			{
				this._ShoppingCarts = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ShoppingCart {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ShoppingCart LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ShoppingCart>("DateModified", dateModifiedValue, "ShoppingCart", Otherwise.Null);
		}

		public partial class ShoppingCartFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ShoppingCartList {		
				
		[JetBrains.Annotations.NotNull]
		public static ShoppingCartList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ShoppingCart".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ShoppingCartList.Load(sql);
		}		
		
	}


}
#endregion