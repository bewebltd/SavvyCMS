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
// CLASS: ProductCategory
// TABLE: ProductCategory
//-----------------------------------------


	public partial class ProductCategory : ActiveRecord {

		/// <summary>
		/// The list that this ProductCategory is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ProductCategory> GetContainingList() {
			return (ActiveRecordList<ProductCategory>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ProductCategory(): base("ProductCategory", "ProductCategoryID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ProductCategory";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ProductCategoryID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ProductCategoryID.
		/// </summary>
		public int ID { get { return (int)fields["ProductCategoryID"].ValueObject; } set { fields["ProductCategoryID"].ValueObject = value; } }

		// field references
		public partial class ProductCategoryFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ProductCategoryFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ProductCategoryFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ProductCategoryFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ProductCategoryFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ProductCategory table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ProductCategory</param>
		/// <returns>An instance of ProductCategory containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ProductCategory table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ProductCategory.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ProductCategory.LoadID(55);</example>
		/// <param name="id">Primary key of ProductCategory</param>
		/// <returns>An instance of ProductCategory containing the data in the record</returns>
		public static ProductCategory LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ProductCategory record = null;
//			record = ProductCategory.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ProductCategoryID=", Sql.Sqlize(id));
//				record = new ProductCategory();
//				if (!record.LoadData(sql)) return otherwise.Execute<ProductCategory>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ProductCategory>(id, "ProductCategory", otherwise);
		}

		/// <summary>
		/// Loads a record from the ProductCategory table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ProductCategory containing the data in the record</returns>
		public static ProductCategory Load(Sql sql) {
				return ActiveRecordLoader.Load<ProductCategory>(sql, "ProductCategory");
		}
		public static ProductCategory Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ProductCategory>(sql, "ProductCategory", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ProductCategory table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ProductCategory containing the data in the record</returns>
		public static ProductCategory Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ProductCategory>(reader, "ProductCategory");
		}
		public static ProductCategory Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ProductCategory>(reader, "ProductCategory", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ProductCategoryID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ProductCategoryID", new ActiveField<int>() { Name = "ProductCategoryID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ProductCategory"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ProductCategory"  });

	fields.Add("Description", new ActiveField<string>() { Name = "Description", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=500, TableName="ProductCategory"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="ProductCategory"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ProductCategory"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ProductCategory"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ProductCategory"  });

	fields.Add("PageID", new ActiveField<int?>() { Name = "PageID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ProductCategory" , GetForeignRecord = () => this.Page, ForeignClassName = typeof(Models.Page), ForeignTableName = "Page", ForeignTableFieldName = "PageID" });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ProductCategory"  });
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
				var rec = ActiveRecordLoader.LoadID<ProductCategory>(id, "ProductCategory", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ProductCategory with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ProductCategory or null if not in cache.</returns>
//		private static ProductCategory GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ProductCategory-" + id) as ProductCategory;
//			return Web.PageGlobals["ActiveRecord-ProductCategory-" + id] as ProductCategory;
//		}

		/// <summary>
		/// Caches this ProductCategory object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ProductCategory-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ProductCategory-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ProductCategory-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ProductCategory objects/records. This is the usual data structure for holding a number of ProductCategory records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ProductCategoryList : ActiveRecordList<ProductCategory> {

		public ProductCategoryList() : base() {}
		public ProductCategoryList(List<ProductCategory> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ProductCategory to ProductCategoryList. 
		/// </summary>
		public static implicit operator ProductCategoryList(List<ProductCategory> list) {
			return new ProductCategoryList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ProductCategoryList to List-of-ProductCategory. 
		/// </summary>
		public static implicit operator List<ProductCategory>(ProductCategoryList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ProductCategory objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ProductCategory records.</returns>
		public static ProductCategoryList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ProductCategoryID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ProductCategory objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ProductCategory records.</returns>
		public static ProductCategoryList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ProductCategoryList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ProductCategoryID in (", ids.SqlizeNumberList(), ")");
			var result = new ProductCategoryList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ProductCategory objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ProductCategory records.</returns>
		public static ProductCategoryList Load(Sql sql) {
			var result = new ProductCategoryList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ProductCategory objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ProductCategoryID desc.)
		/// </summary>
		public static ProductCategoryList LoadAll() {
			var result = new ProductCategoryList();
			result.LoadRecords(null);
			return result;
		}
		public static ProductCategoryList LoadAll(int itemsPerPage) {
			var result = new ProductCategoryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ProductCategoryList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ProductCategoryList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ProductCategory objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ProductCategoryList LoadActive() {
			var result = new ProductCategoryList();
			var sql = (new ProductCategory()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ProductCategoryList LoadActive(int itemsPerPage) {
			var result = new ProductCategoryList();
			var sql = (new ProductCategory()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ProductCategoryList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ProductCategoryList();
			var sql = (new ProductCategory()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ProductCategoryList LoadActivePlusExisting(object existingRecordID) {
			var result = new ProductCategoryList();
			var sql = (new ProductCategory()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ProductCategory");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ProductCategory");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ProductCategory()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ProductCategory.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ProductCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ProductCategoryID {
			get { return Fields.ProductCategoryID.Value; }
			set { fields["ProductCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByProductCategoryID(int productCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("ProductCategoryID", productCategoryIDValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ProductCategoryID {
				get { return (ActiveField<int>)fields["ProductCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByProductCategoryID(int productCategoryIDValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where ProductCategoryID=", Sql.Sqlize(productCategoryIDValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("Title", titleValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Description
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Description {
			get { return Fields.Description.Value; }
			set { fields["Description"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByDescription(string descriptionValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("Description", descriptionValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Description {
				get { return (ActiveField<string>)fields["Description"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByDescription(string descriptionValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where Description=", Sql.Sqlize(descriptionValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("IsActive", isActiveValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("SortPosition", sortPositionValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("DateAdded", dateAddedValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("Picture", pictureValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PageID {
			get { return Fields.PageID.Value; }
			set { fields["PageID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByPageID(int? pageIDValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("PageID", pageIDValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PageID {
				get { return (ActiveField<int?>)fields["PageID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByPageID(int? pageIDValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where PageID=", Sql.Sqlize(pageIDValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Page
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ProductCategory {
		[NonSerialized]		
		private Page _Page;

		[JetBrains.Annotations.CanBeNull]
		public Page Page
		{
			get
			{
				 // lazy load
				if (this._Page == null && this.PageID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Page") && container.PrefetchCounter["Page"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Page>("PageID",container.Select(r=>r.PageID).ToList(),"Page",Otherwise.Null);
					}
					this._Page = Models.Page.LoadByPageID(PageID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Page")) {
							container.PrefetchCounter["Page"] = 0;
						}
						container.PrefetchCounter["Page"]++;
					}
				}
				return this._Page;
			}
			set
			{
				this._Page = value;
			}
		}
	}

	public partial class ProductCategoryList {
		internal int numFetchesOfPage = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Page {
		[NonSerialized]		
		private ProductCategoryList _ProductCategories;
		
		[JetBrains.Annotations.NotNull]
		public ProductCategoryList ProductCategories
		{
			get
			{
				// lazy load
				if (this._ProductCategories == null) {
					this._ProductCategories = Models.ProductCategoryList.LoadByPageID(this.ID);
					this._ProductCategories.SetParentBindField(this, "PageID");
				}
				return this._ProductCategories;
			}
			set
			{
				this._ProductCategories = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ProductCategory {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ProductCategory LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ProductCategory>("DateModified", dateModifiedValue, "ProductCategory", Otherwise.Null);
		}

		public partial class ProductCategoryFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ProductCategoryList {		
				
		[JetBrains.Annotations.NotNull]
		public static ProductCategoryList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ProductCategory".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ProductCategoryList.Load(sql);
		}		
		
	}


}
#endregion