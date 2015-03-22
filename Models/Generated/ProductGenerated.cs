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
// CLASS: Product
// TABLE: Product
//-----------------------------------------


	public partial class Product : ActiveRecord {

		/// <summary>
		/// The list that this Product is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Product> GetContainingList() {
			return (ActiveRecordList<Product>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Product(): base("Product", "ProductID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Product";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ProductID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ProductID.
		/// </summary>
		public int ID { get { return (int)fields["ProductID"].ValueObject; } set { fields["ProductID"].ValueObject = value; } }

		// field references
		public partial class ProductFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ProductFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ProductFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ProductFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ProductFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Product table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Product</param>
		/// <returns>An instance of Product containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Product LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Product table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Product.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Product.LoadID(55);</example>
		/// <param name="id">Primary key of Product</param>
		/// <returns>An instance of Product containing the data in the record</returns>
		public static Product LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Product record = null;
//			record = Product.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ProductID=", Sql.Sqlize(id));
//				record = new Product();
//				if (!record.LoadData(sql)) return otherwise.Execute<Product>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Product>(id, "Product", otherwise);
		}

		/// <summary>
		/// Loads a record from the Product table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Product containing the data in the record</returns>
		public static Product Load(Sql sql) {
				return ActiveRecordLoader.Load<Product>(sql, "Product");
		}
		public static Product Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Product>(sql, "Product", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Product table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Product containing the data in the record</returns>
		public static Product Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Product>(reader, "Product");
		}
		public static Product Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Product>(reader, "Product", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ProductID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ProductID", new ActiveField<int>() { Name = "ProductID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Product"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Product"  });

	fields.Add("Price", new ActiveField<decimal?>() { Name = "Price", ColumnType = "money", Type = typeof(decimal?), IsAuto = false, MaxLength=8, TableName="Product"  });

	fields.Add("Reference", new ActiveField<string>() { Name = "Reference", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=20, TableName="Product"  });

	fields.Add("Description", new ActiveField<string>() { Name = "Description", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=500, TableName="Product"  });

	fields.Add("Gst", new ActiveField<decimal?>() { Name = "Gst", ColumnType = "money", Type = typeof(decimal?), IsAuto = false, MaxLength=8, TableName="Product"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Product"  });

	fields.Add("Picture1", new PictureActiveField() { Name = "Picture1", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Product"  });

	fields.Add("ModifiedDate", new ActiveField<System.DateTime?>() { Name = "ModifiedDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Product"  });

	fields.Add("Type", new ActiveField<string>() { Name = "Type", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=5, TableName="Product"  });

	fields.Add("ProductCategoryID", new ActiveField<int?>() { Name = "ProductCategoryID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Product" , GetForeignRecord = () => this.ProductCategory, ForeignClassName = typeof(Models.ProductCategory), ForeignTableName = "ProductCategory", ForeignTableFieldName = "ProductCategoryID" });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Product"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Product"  });

	fields.Add("Author", new ActiveField<string>() { Name = "Author", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Product"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Product"  });

	fields.Add("PageTitleTag", new ActiveField<string>() { Name = "PageTitleTag", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Product"  });

	fields.Add("MetaKeywords", new ActiveField<string>() { Name = "MetaKeywords", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="Product"  });

	fields.Add("MetaDescription", new ActiveField<string>() { Name = "MetaDescription", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="Product"  });

	fields.Add("FocusKeyword", new ActiveField<string>() { Name = "FocusKeyword", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="Product"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Product"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Product"  });
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
				var rec = ActiveRecordLoader.LoadID<Product>(id, "Product", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Product with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Product or null if not in cache.</returns>
//		private static Product GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Product-" + id) as Product;
//			return Web.PageGlobals["ActiveRecord-Product-" + id] as Product;
//		}

		/// <summary>
		/// Caches this Product object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Product-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Product-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Product-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Product objects/records. This is the usual data structure for holding a number of Product records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ProductList : ActiveRecordList<Product> {

		public ProductList() : base() {}
		public ProductList(List<Product> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Product to ProductList. 
		/// </summary>
		public static implicit operator ProductList(List<Product> list) {
			return new ProductList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ProductList to List-of-Product. 
		/// </summary>
		public static implicit operator List<Product>(ProductList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Product objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Product records.</returns>
		public static ProductList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ProductID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Product objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Product records.</returns>
		public static ProductList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ProductList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ProductID in (", ids.SqlizeNumberList(), ")");
			var result = new ProductList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Product objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Product records.</returns>
		public static ProductList Load(Sql sql) {
			var result = new ProductList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Product objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ProductID desc.)
		/// </summary>
		public static ProductList LoadAll() {
			var result = new ProductList();
			result.LoadRecords(null);
			return result;
		}
		public static ProductList LoadAll(int itemsPerPage) {
			var result = new ProductList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ProductList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ProductList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Product objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ProductList LoadActive() {
			var result = new ProductList();
			var sql = (new Product()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ProductList LoadActive(int itemsPerPage) {
			var result = new ProductList();
			var sql = (new Product()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ProductList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ProductList();
			var sql = (new Product()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ProductList LoadActivePlusExisting(object existingRecordID) {
			var result = new ProductList();
			var sql = (new Product()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Product");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Product");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Product()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Product.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ProductID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ProductID {
			get { return Fields.ProductID.Value; }
			set { fields["ProductID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByProductID(int productIDValue) {
			return ActiveRecordLoader.LoadByField<Product>("ProductID", productIDValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ProductID {
				get { return (ActiveField<int>)fields["ProductID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByProductID(int productIDValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where ProductID=", Sql.Sqlize(productIDValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Product>("Title", titleValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Price
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public decimal? Price {
			get { return Fields.Price.Value; }
			set { fields["Price"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByPrice(decimal? priceValue) {
			return ActiveRecordLoader.LoadByField<Product>("Price", priceValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<decimal?> Price {
				get { return (ActiveField<decimal?>)fields["Price"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByPrice(decimal? priceValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Price=", Sql.Sqlize(priceValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Reference
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Reference {
			get { return Fields.Reference.Value; }
			set { fields["Reference"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByReference(string referenceValue) {
			return ActiveRecordLoader.LoadByField<Product>("Reference", referenceValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Reference {
				get { return (ActiveField<string>)fields["Reference"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByReference(string referenceValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Reference=", Sql.Sqlize(referenceValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Description
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Description {
			get { return Fields.Description.Value; }
			set { fields["Description"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByDescription(string descriptionValue) {
			return ActiveRecordLoader.LoadByField<Product>("Description", descriptionValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Description {
				get { return (ActiveField<string>)fields["Description"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByDescription(string descriptionValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Description=", Sql.Sqlize(descriptionValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Gst
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public decimal? Gst {
			get { return Fields.Gst.Value; }
			set { fields["Gst"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByGst(decimal? gstValue) {
			return ActiveRecordLoader.LoadByField<Product>("Gst", gstValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<decimal?> Gst {
				get { return (ActiveField<decimal?>)fields["Gst"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByGst(decimal? gstValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Gst=", Sql.Sqlize(gstValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<Product>("SortPosition", sortPositionValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture1
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture1 {
			get { return Fields.Picture1.Value; }
			set { fields["Picture1"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByPicture1(string picture1Value) {
			return ActiveRecordLoader.LoadByField<Product>("Picture1", picture1Value, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture1 {
				get { return (PictureActiveField)fields["Picture1"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByPicture1(string picture1Value) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Picture1=", Sql.Sqlize(picture1Value));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ModifiedDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ModifiedDate {
			get { return Fields.ModifiedDate.Value; }
			set { fields["ModifiedDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByModifiedDate(System.DateTime? modifiedDateValue) {
			return ActiveRecordLoader.LoadByField<Product>("ModifiedDate", modifiedDateValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ModifiedDate {
				get { return (ActiveField<System.DateTime?>)fields["ModifiedDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByModifiedDate(System.DateTime? modifiedDateValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where ModifiedDate=", Sql.Sqlize(modifiedDateValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Type
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Type {
			get { return Fields.Type.Value; }
			set { fields["Type"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByType(string typeValue) {
			return ActiveRecordLoader.LoadByField<Product>("Type", typeValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Type {
				get { return (ActiveField<string>)fields["Type"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByType(string typeValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Type=", Sql.Sqlize(typeValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ProductCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ProductCategoryID {
			get { return Fields.ProductCategoryID.Value; }
			set { fields["ProductCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByProductCategoryID(int? productCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<Product>("ProductCategoryID", productCategoryIDValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ProductCategoryID {
				get { return (ActiveField<int?>)fields["ProductCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByProductCategoryID(int? productCategoryIDValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where ProductCategoryID=", Sql.Sqlize(productCategoryIDValue));
			return ProductList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ProductCategory
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Product {
		[NonSerialized]		
		private ProductCategory _ProductCategory;

		[JetBrains.Annotations.CanBeNull]
		public ProductCategory ProductCategory
		{
			get
			{
				 // lazy load
				if (this._ProductCategory == null && this.ProductCategoryID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ProductCategory") && container.PrefetchCounter["ProductCategory"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.ProductCategory>("ProductCategoryID",container.Select(r=>r.ProductCategoryID).ToList(),"ProductCategory",Otherwise.Null);
					}
					this._ProductCategory = Models.ProductCategory.LoadByProductCategoryID(ProductCategoryID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ProductCategory")) {
							container.PrefetchCounter["ProductCategory"] = 0;
						}
						container.PrefetchCounter["ProductCategory"]++;
					}
				}
				return this._ProductCategory;
			}
			set
			{
				this._ProductCategory = value;
			}
		}
	}

	public partial class ProductList {
		internal int numFetchesOfProductCategory = 0;
	}
	
	// define list in partial foreign table class 
	public partial class ProductCategory {
		[NonSerialized]		
		private ProductList _Products;
		
		[JetBrains.Annotations.NotNull]
		public ProductList Products
		{
			get
			{
				// lazy load
				if (this._Products == null) {
					this._Products = Models.ProductList.LoadByProductCategoryID(this.ID);
					this._Products.SetParentBindField(this, "ProductCategoryID");
				}
				return this._Products;
			}
			set
			{
				this._Products = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Product>("DateAdded", dateAddedValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<Product>("IsActive", isActiveValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Author
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Author {
			get { return Fields.Author.Value; }
			set { fields["Author"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByAuthor(string authorValue) {
			return ActiveRecordLoader.LoadByField<Product>("Author", authorValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Author {
				get { return (ActiveField<string>)fields["Author"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByAuthor(string authorValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where Author=", Sql.Sqlize(authorValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Product>("DateModified", dateModifiedValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageTitleTag
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageTitleTag {
			get { return Fields.PageTitleTag.Value; }
			set { fields["PageTitleTag"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByPageTitleTag(string pageTitleTagValue) {
			return ActiveRecordLoader.LoadByField<Product>("PageTitleTag", pageTitleTagValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageTitleTag {
				get { return (ActiveField<string>)fields["PageTitleTag"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByPageTitleTag(string pageTitleTagValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where PageTitleTag=", Sql.Sqlize(pageTitleTagValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaKeywords
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaKeywords {
			get { return Fields.MetaKeywords.Value; }
			set { fields["MetaKeywords"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByMetaKeywords(string metaKeywordsValue) {
			return ActiveRecordLoader.LoadByField<Product>("MetaKeywords", metaKeywordsValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaKeywords {
				get { return (ActiveField<string>)fields["MetaKeywords"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByMetaKeywords(string metaKeywordsValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where MetaKeywords=", Sql.Sqlize(metaKeywordsValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaDescription {
			get { return Fields.MetaDescription.Value; }
			set { fields["MetaDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByMetaDescription(string metaDescriptionValue) {
			return ActiveRecordLoader.LoadByField<Product>("MetaDescription", metaDescriptionValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaDescription {
				get { return (ActiveField<string>)fields["MetaDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByMetaDescription(string metaDescriptionValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where MetaDescription=", Sql.Sqlize(metaDescriptionValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FocusKeyword
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FocusKeyword {
			get { return Fields.FocusKeyword.Value; }
			set { fields["FocusKeyword"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByFocusKeyword(string focusKeywordValue) {
			return ActiveRecordLoader.LoadByField<Product>("FocusKeyword", focusKeywordValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FocusKeyword {
				get { return (ActiveField<string>)fields["FocusKeyword"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByFocusKeyword(string focusKeywordValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where FocusKeyword=", Sql.Sqlize(focusKeywordValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<Product>("PublishDate", publishDateValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return ProductList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Product {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Product LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<Product>("ExpiryDate", expiryDateValue, "Product", Otherwise.Null);
		}

		public partial class ProductFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","Product".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return ProductList.Load(sql);
		}		
		
	}


}
#endregion