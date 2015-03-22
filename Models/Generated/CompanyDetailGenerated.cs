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
// CLASS: CompanyDetail
// TABLE: CompanyDetail
//-----------------------------------------


	public partial class CompanyDetail : ActiveRecord {

		/// <summary>
		/// The list that this CompanyDetail is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<CompanyDetail> GetContainingList() {
			return (ActiveRecordList<CompanyDetail>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public CompanyDetail(): base("CompanyDetail", "CompanyDetailID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "CompanyDetail";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "CompanyDetailID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property CompanyDetailID.
		/// </summary>
		public int ID { get { return (int)fields["CompanyDetailID"].ValueObject; } set { fields["CompanyDetailID"].ValueObject = value; } }

		// field references
		public partial class CompanyDetailFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public CompanyDetailFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private CompanyDetailFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public CompanyDetailFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new CompanyDetailFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the CompanyDetail table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of CompanyDetail</param>
		/// <returns>An instance of CompanyDetail containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the CompanyDetail table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg CompanyDetail.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = CompanyDetail.LoadID(55);</example>
		/// <param name="id">Primary key of CompanyDetail</param>
		/// <returns>An instance of CompanyDetail containing the data in the record</returns>
		public static CompanyDetail LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			CompanyDetail record = null;
//			record = CompanyDetail.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where CompanyDetailID=", Sql.Sqlize(id));
//				record = new CompanyDetail();
//				if (!record.LoadData(sql)) return otherwise.Execute<CompanyDetail>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<CompanyDetail>(id, "CompanyDetail", otherwise);
		}

		/// <summary>
		/// Loads a record from the CompanyDetail table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of CompanyDetail containing the data in the record</returns>
		public static CompanyDetail Load(Sql sql) {
				return ActiveRecordLoader.Load<CompanyDetail>(sql, "CompanyDetail");
		}
		public static CompanyDetail Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<CompanyDetail>(sql, "CompanyDetail", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the CompanyDetail table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of CompanyDetail containing the data in the record</returns>
		public static CompanyDetail Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<CompanyDetail>(reader, "CompanyDetail");
		}
		public static CompanyDetail Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<CompanyDetail>(reader, "CompanyDetail", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where CompanyDetailID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("CompanyDetailID", new ActiveField<int>() { Name = "CompanyDetailID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="CompanyDetail"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="CompanyDetail"  });

	fields.Add("Address", new ActiveField<string>() { Name = "Address", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="CompanyDetail"  });

	fields.Add("Phone", new ActiveField<string>() { Name = "Phone", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="CompanyDetail"  });

	fields.Add("Email", new ActiveField<string>() { Name = "Email", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="CompanyDetail"  });

	fields.Add("Latitude", new ActiveField<double?>() { Name = "Latitude", ColumnType = "float", Type = typeof(double?), IsAuto = false, MaxLength=8, TableName="CompanyDetail"  });

	fields.Add("Longitude", new ActiveField<double?>() { Name = "Longitude", ColumnType = "float", Type = typeof(double?), IsAuto = false, MaxLength=8, TableName="CompanyDetail"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="CompanyDetail"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="CompanyDetail"  });

	fields.Add("CompanyPicture", new PictureActiveField() { Name = "CompanyPicture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="CompanyDetail"  });

	fields.Add("CompanyPicture1", new PictureActiveField() { Name = "CompanyPicture1", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="CompanyDetail"  });
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
				var rec = ActiveRecordLoader.LoadID<CompanyDetail>(id, "CompanyDetail", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the CompanyDetail with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct CompanyDetail or null if not in cache.</returns>
//		private static CompanyDetail GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-CompanyDetail-" + id) as CompanyDetail;
//			return Web.PageGlobals["ActiveRecord-CompanyDetail-" + id] as CompanyDetail;
//		}

		/// <summary>
		/// Caches this CompanyDetail object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-CompanyDetail-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-CompanyDetail-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-CompanyDetail-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of CompanyDetail objects/records. This is the usual data structure for holding a number of CompanyDetail records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class CompanyDetailList : ActiveRecordList<CompanyDetail> {

		public CompanyDetailList() : base() {}
		public CompanyDetailList(List<CompanyDetail> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-CompanyDetail to CompanyDetailList. 
		/// </summary>
		public static implicit operator CompanyDetailList(List<CompanyDetail> list) {
			return new CompanyDetailList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from CompanyDetailList to List-of-CompanyDetail. 
		/// </summary>
		public static implicit operator List<CompanyDetail>(CompanyDetailList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of CompanyDetail objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of CompanyDetail records.</returns>
		public static CompanyDetailList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where CompanyDetailID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of CompanyDetail objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of CompanyDetail records.</returns>
		public static CompanyDetailList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static CompanyDetailList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where CompanyDetailID in (", ids.SqlizeNumberList(), ")");
			var result = new CompanyDetailList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of CompanyDetail objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of CompanyDetail records.</returns>
		public static CompanyDetailList Load(Sql sql) {
			var result = new CompanyDetailList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all CompanyDetail objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and CompanyDetailID desc.)
		/// </summary>
		public static CompanyDetailList LoadAll() {
			var result = new CompanyDetailList();
			result.LoadRecords(null);
			return result;
		}
		public static CompanyDetailList LoadAll(int itemsPerPage) {
			var result = new CompanyDetailList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CompanyDetailList LoadAll(int itemsPerPage, int pageNum) {
			var result = new CompanyDetailList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" CompanyDetail objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static CompanyDetailList LoadActive() {
			var result = new CompanyDetailList();
			var sql = (new CompanyDetail()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static CompanyDetailList LoadActive(int itemsPerPage) {
			var result = new CompanyDetailList();
			var sql = (new CompanyDetail()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CompanyDetailList LoadActive(int itemsPerPage, int pageNum) {
			var result = new CompanyDetailList();
			var sql = (new CompanyDetail()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static CompanyDetailList LoadActivePlusExisting(object existingRecordID) {
			var result = new CompanyDetailList();
			var sql = (new CompanyDetail()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM CompanyDetail");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM CompanyDetail");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new CompanyDetail()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = CompanyDetail.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: CompanyDetailID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public int CompanyDetailID {
			get { return Fields.CompanyDetailID.Value; }
			set { fields["CompanyDetailID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByCompanyDetailID(int companyDetailIDValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("CompanyDetailID", companyDetailIDValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> CompanyDetailID {
				get { return (ActiveField<int>)fields["CompanyDetailID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByCompanyDetailID(int companyDetailIDValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where CompanyDetailID=", Sql.Sqlize(companyDetailIDValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("Title", titleValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Address
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Address {
			get { return Fields.Address.Value; }
			set { fields["Address"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByAddress(string addressValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("Address", addressValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Address {
				get { return (ActiveField<string>)fields["Address"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByAddress(string addressValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where Address=", Sql.Sqlize(addressValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Phone
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Phone {
			get { return Fields.Phone.Value; }
			set { fields["Phone"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByPhone(string phoneValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("Phone", phoneValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Phone {
				get { return (ActiveField<string>)fields["Phone"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByPhone(string phoneValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where Phone=", Sql.Sqlize(phoneValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Email
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Email {
			get { return Fields.Email.Value; }
			set { fields["Email"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByEmail(string emailValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("Email", emailValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Email {
				get { return (ActiveField<string>)fields["Email"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByEmail(string emailValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where Email=", Sql.Sqlize(emailValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Latitude
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public double? Latitude {
			get { return Fields.Latitude.Value; }
			set { fields["Latitude"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByLatitude(double? latitudeValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("Latitude", latitudeValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<double?> Latitude {
				get { return (ActiveField<double?>)fields["Latitude"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByLatitude(double? latitudeValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where Latitude=", Sql.Sqlize(latitudeValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Longitude
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public double? Longitude {
			get { return Fields.Longitude.Value; }
			set { fields["Longitude"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByLongitude(double? longitudeValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("Longitude", longitudeValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<double?> Longitude {
				get { return (ActiveField<double?>)fields["Longitude"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByLongitude(double? longitudeValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where Longitude=", Sql.Sqlize(longitudeValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("DateAdded", dateAddedValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("DateModified", dateModifiedValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CompanyPicture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CompanyPicture {
			get { return Fields.CompanyPicture.Value; }
			set { fields["CompanyPicture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByCompanyPicture(string companyPictureValue) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("CompanyPicture", companyPictureValue, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField CompanyPicture {
				get { return (PictureActiveField)fields["CompanyPicture"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByCompanyPicture(string companyPictureValue) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where CompanyPicture=", Sql.Sqlize(companyPictureValue));
			return CompanyDetailList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CompanyPicture1
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class CompanyDetail {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CompanyPicture1 {
			get { return Fields.CompanyPicture1.Value; }
			set { fields["CompanyPicture1"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static CompanyDetail LoadByCompanyPicture1(string companyPicture1Value) {
			return ActiveRecordLoader.LoadByField<CompanyDetail>("CompanyPicture1", companyPicture1Value, "CompanyDetail", Otherwise.Null);
		}

		public partial class CompanyDetailFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField CompanyPicture1 {
				get { return (PictureActiveField)fields["CompanyPicture1"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompanyDetailList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompanyDetailList LoadByCompanyPicture1(string companyPicture1Value) {
			var sql = new Sql("select * from ","CompanyDetail".SqlizeName()," where CompanyPicture1=", Sql.Sqlize(companyPicture1Value));
			return CompanyDetailList.Load(sql);
		}		
		
	}


}
#endregion