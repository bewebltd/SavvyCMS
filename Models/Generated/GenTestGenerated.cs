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
// CLASS: GenTest
// TABLE: GenTest
//-----------------------------------------


	public partial class GenTest : ActiveRecord {

		/// <summary>
		/// The list that this GenTest is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<GenTest> GetContainingList() {
			return (ActiveRecordList<GenTest>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public GenTest(): base("GenTest", "GenTestID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "GenTest";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "GenTestID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property GenTestID.
		/// </summary>
		public int ID { get { return (int)fields["GenTestID"].ValueObject; } set { fields["GenTestID"].ValueObject = value; } }

		// field references
		public partial class GenTestFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public GenTestFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private GenTestFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public GenTestFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new GenTestFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the GenTest table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of GenTest</param>
		/// <returns>An instance of GenTest containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the GenTest table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg GenTest.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of GenTest</param>
		/// <returns>An instance of GenTest containing the data in the record</returns>
		public static GenTest LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			GenTest record = null;
//			record = GenTest.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where GenTestID=", Sql.Sqlize(id));
//				record = new GenTest();
//				if (!record.LoadData(sql)) return otherwise.Execute<GenTest>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<GenTest>(id, "GenTest", otherwise);
		}

		/// <summary>
		/// Loads a record from the GenTest table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of GenTest containing the data in the record</returns>
		public static GenTest Load(Sql sql) {
				return ActiveRecordLoader.Load<GenTest>(sql, "GenTest");
		}
		public static GenTest Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GenTest>(sql, "GenTest", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the GenTest table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of GenTest containing the data in the record</returns>
		public static GenTest Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<GenTest>(reader, "GenTest");
		}
		public static GenTest Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<GenTest>(reader, "GenTest", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where GenTestID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("GenTestID", new ActiveField<int>() { Name = "GenTestID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="GenTest"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GenTest"  });

	fields.Add("IntroCopyHtml", new ActiveField<string>() { Name = "IntroCopyHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="GenTest"  });

	fields.Add("BodyCopy", new ActiveField<string>() { Name = "BodyCopy", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="GenTest"  });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="GenTest"  });

	fields.Add("Cost", new ActiveField<decimal?>() { Name = "Cost", ColumnType = "money", Type = typeof(decimal?), IsAuto = false, MaxLength=8, TableName="GenTest"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="GenTest"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTest"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTest"  });

	fields.Add("Ratio", new ActiveField<double?>() { Name = "Ratio", ColumnType = "float", Type = typeof(double?), IsAuto = false, MaxLength=8, TableName="GenTest"  });

	fields.Add("GUI", new ActiveField<System.Guid?>() { Name = "GUI", ColumnType = "uniqueidentifier", Type = typeof(System.Guid?), IsAuto = false, MaxLength=16, TableName="GenTest"  });

	fields.Add("NumberOfStaff", new ActiveField<int?>() { Name = "NumberOfStaff", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GenTest"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GenTest"  });

	fields.Add("GenTestCatID", new ActiveField<int?>() { Name = "GenTestCatID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="GenTest" , GetForeignRecord = () => this.GenTestCat, ForeignClassName = typeof(Models.GenTestCat), ForeignTableName = "GenTestCat", ForeignTableFieldName = "GenTestCatID" });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GenTest"  });

	fields.Add("Attachment", new AttachmentActiveField() { Name = "Attachment", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GenTest"  });

	fields.Add("Picture1", new PictureActiveField() { Name = "Picture1", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GenTest"  });

	fields.Add("Attachment1", new AttachmentActiveField() { Name = "Attachment1", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="GenTest"  });

	fields.Add("InvoiceAmount", new ActiveField<decimal?>() { Name = "InvoiceAmount", ColumnType = "money", Type = typeof(decimal?), IsAuto = false, MaxLength=8, TableName="GenTest"  });

	fields.Add("Latitude", new ActiveField<decimal?>() { Name = "Latitude", ColumnType = "decimal", Type = typeof(decimal?), IsAuto = false, MaxLength=17, DecimalPlaces=8, TableName="GenTest"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="GenTest"  });

	fields.Add("AttachmentPDFText", new AttachmentActiveField() { Name = "AttachmentPDFText", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="GenTest"  });

	fields.Add("AttachmentRAWText", new AttachmentActiveField() { Name = "AttachmentRAWText", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="GenTest"  });

	fields.Add("Attachment1RAWText", new AttachmentActiveField() { Name = "Attachment1RAWText", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=2147483647, TableName="GenTest"  });
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
				var rec = ActiveRecordLoader.LoadID<GenTest>(id, "GenTest", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the GenTest with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct GenTest or null if not in cache.</returns>
//		private static GenTest GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-GenTest-" + id) as GenTest;
//			return Web.PageGlobals["ActiveRecord-GenTest-" + id] as GenTest;
//		}

		/// <summary>
		/// Caches this GenTest object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-GenTest-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-GenTest-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-GenTest-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of GenTest objects/records. This is the usual data structure for holding a number of GenTest records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class GenTestList : ActiveRecordList<GenTest> {

		public GenTestList() : base() {}
		public GenTestList(List<GenTest> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-GenTest to GenTestList. 
		/// </summary>
		public static implicit operator GenTestList(List<GenTest> list) {
			return new GenTestList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from GenTestList to List-of-GenTest. 
		/// </summary>
		public static implicit operator List<GenTest>(GenTestList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of GenTest objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of GenTest records.</returns>
		public static GenTestList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where GenTestID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of GenTest objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of GenTest records.</returns>
		public static GenTestList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static GenTestList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where GenTestID in (", ids.SqlizeNumberList(), ")");
			var result = new GenTestList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of GenTest objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of GenTest records.</returns>
		public static GenTestList Load(Sql sql) {
			var result = new GenTestList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all GenTest objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and GenTestID desc.)
		/// </summary>
		public static GenTestList LoadAll() {
			var result = new GenTestList();
			result.LoadRecords(null);
			return result;
		}
		public static GenTestList LoadAll(int itemsPerPage) {
			var result = new GenTestList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestList LoadAll(int itemsPerPage, int pageNum) {
			var result = new GenTestList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" GenTest objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static GenTestList LoadActive() {
			var result = new GenTestList();
			var sql = (new GenTest()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestList LoadActive(int itemsPerPage) {
			var result = new GenTestList();
			var sql = (new GenTest()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static GenTestList LoadActive(int itemsPerPage, int pageNum) {
			var result = new GenTestList();
			var sql = (new GenTest()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static GenTestList LoadActivePlusExisting(object existingRecordID) {
			var result = new GenTestList();
			var sql = (new GenTest()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM GenTest");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM GenTest");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new GenTest()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = GenTest.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: GenTestID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public int GenTestID {
			get { return Fields.GenTestID.Value; }
			set { fields["GenTestID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByGenTestID(int genTestIDValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("GenTestID", genTestIDValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> GenTestID {
				get { return (ActiveField<int>)fields["GenTestID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByGenTestID(int genTestIDValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where GenTestID=", Sql.Sqlize(genTestIDValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Title", titleValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IntroCopyHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string IntroCopyHtml {
			get { return Fields.IntroCopyHtml.Value; }
			set { fields["IntroCopyHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByIntroCopyHtml(string introCopyHtmlValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("IntroCopyHtml", introCopyHtmlValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> IntroCopyHtml {
				get { return (ActiveField<string>)fields["IntroCopyHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByIntroCopyHtml(string introCopyHtmlValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where IntroCopyHtml=", Sql.Sqlize(introCopyHtmlValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyCopy
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyCopy {
			get { return Fields.BodyCopy.Value; }
			set { fields["BodyCopy"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByBodyCopy(string bodyCopyValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("BodyCopy", bodyCopyValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyCopy {
				get { return (ActiveField<string>)fields["BodyCopy"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByBodyCopy(string bodyCopyValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where BodyCopy=", Sql.Sqlize(bodyCopyValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("BodyTextHtml", bodyTextHtmlValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Cost
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public decimal? Cost {
			get { return Fields.Cost.Value; }
			set { fields["Cost"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByCost(decimal? costValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Cost", costValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<decimal?> Cost {
				get { return (ActiveField<decimal?>)fields["Cost"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByCost(decimal? costValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Cost=", Sql.Sqlize(costValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("IsActive", isActiveValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("DateAdded", dateAddedValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("PublishDate", publishDateValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Ratio
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public double? Ratio {
			get { return Fields.Ratio.Value; }
			set { fields["Ratio"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByRatio(double? ratioValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Ratio", ratioValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<double?> Ratio {
				get { return (ActiveField<double?>)fields["Ratio"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByRatio(double? ratioValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Ratio=", Sql.Sqlize(ratioValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: GUI
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.Guid? GUI {
			get { return Fields.GUI.Value; }
			set { fields["GUI"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByGUI(System.Guid? gUIValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("GUI", gUIValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.Guid?> GUI {
				get { return (ActiveField<System.Guid?>)fields["GUI"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByGUI(System.Guid? gUIValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where GUI=", Sql.Sqlize(gUIValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: NumberOfStaff
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? NumberOfStaff {
			get { return Fields.NumberOfStaff.Value; }
			set { fields["NumberOfStaff"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByNumberOfStaff(int? numberOfStaffValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("NumberOfStaff", numberOfStaffValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> NumberOfStaff {
				get { return (ActiveField<int?>)fields["NumberOfStaff"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByNumberOfStaff(int? numberOfStaffValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where NumberOfStaff=", Sql.Sqlize(numberOfStaffValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("SortPosition", sortPositionValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: GenTestCatID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? GenTestCatID {
			get { return Fields.GenTestCatID.Value; }
			set { fields["GenTestCatID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByGenTestCatID(int? genTestCatIDValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("GenTestCatID", genTestCatIDValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> GenTestCatID {
				get { return (ActiveField<int?>)fields["GenTestCatID"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByGenTestCatID(int? genTestCatIDValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where GenTestCatID=", Sql.Sqlize(genTestCatIDValue));
			return GenTestList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: GenTestCat
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class GenTest {
		[NonSerialized]		
		private GenTestCat _GenTestCat;

		[JetBrains.Annotations.CanBeNull]
		public GenTestCat GenTestCat
		{
			get
			{
				 // lazy load
				if (this._GenTestCat == null && this.GenTestCatID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("GenTestCat") && container.PrefetchCounter["GenTestCat"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.GenTestCat>("GenTestCatID",container.Select(r=>r.GenTestCatID).ToList(),"GenTestCat",Otherwise.Null);
					}
					this._GenTestCat = Models.GenTestCat.LoadByGenTestCatID(GenTestCatID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("GenTestCat")) {
							container.PrefetchCounter["GenTestCat"] = 0;
						}
						container.PrefetchCounter["GenTestCat"]++;
					}
				}
				return this._GenTestCat;
			}
			set
			{
				this._GenTestCat = value;
			}
		}
	}

	public partial class GenTestList {
		internal int numFetchesOfGenTestCat = 0;
	}
	
	// define list in partial foreign table class 
	public partial class GenTestCat {
		[NonSerialized]		
		private GenTestList _GenTests;
		
		[JetBrains.Annotations.NotNull]
		public GenTestList GenTests
		{
			get
			{
				// lazy load
				if (this._GenTests == null) {
					this._GenTests = Models.GenTestList.LoadByGenTestCatID(this.ID);
					this._GenTests.SetParentBindField(this, "GenTestCatID");
				}
				return this._GenTests;
			}
			set
			{
				this._GenTests = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Picture", pictureValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Attachment
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Attachment {
			get { return Fields.Attachment.Value; }
			set { fields["Attachment"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByAttachment(string attachmentValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Attachment", attachmentValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField Attachment {
				get { return (AttachmentActiveField)fields["Attachment"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByAttachment(string attachmentValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Attachment=", Sql.Sqlize(attachmentValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture1
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture1 {
			get { return Fields.Picture1.Value; }
			set { fields["Picture1"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByPicture1(string picture1Value) {
			return ActiveRecordLoader.LoadByField<GenTest>("Picture1", picture1Value, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture1 {
				get { return (PictureActiveField)fields["Picture1"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByPicture1(string picture1Value) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Picture1=", Sql.Sqlize(picture1Value));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Attachment1
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Attachment1 {
			get { return Fields.Attachment1.Value; }
			set { fields["Attachment1"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByAttachment1(string attachment1Value) {
			return ActiveRecordLoader.LoadByField<GenTest>("Attachment1", attachment1Value, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField Attachment1 {
				get { return (AttachmentActiveField)fields["Attachment1"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByAttachment1(string attachment1Value) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Attachment1=", Sql.Sqlize(attachment1Value));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: InvoiceAmount
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public decimal? InvoiceAmount {
			get { return Fields.InvoiceAmount.Value; }
			set { fields["InvoiceAmount"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByInvoiceAmount(decimal? invoiceAmountValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("InvoiceAmount", invoiceAmountValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<decimal?> InvoiceAmount {
				get { return (ActiveField<decimal?>)fields["InvoiceAmount"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByInvoiceAmount(decimal? invoiceAmountValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where InvoiceAmount=", Sql.Sqlize(invoiceAmountValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Latitude
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public decimal? Latitude {
			get { return Fields.Latitude.Value; }
			set { fields["Latitude"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByLatitude(decimal? latitudeValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Latitude", latitudeValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<decimal?> Latitude {
				get { return (ActiveField<decimal?>)fields["Latitude"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByLatitude(decimal? latitudeValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Latitude=", Sql.Sqlize(latitudeValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("DateModified", dateModifiedValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AttachmentPDFText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AttachmentPDFText {
			get { return Fields.AttachmentPDFText.Value; }
			set { fields["AttachmentPDFText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByAttachmentPDFText(string attachmentPDFTextValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("AttachmentPDFText", attachmentPDFTextValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField AttachmentPDFText {
				get { return (AttachmentActiveField)fields["AttachmentPDFText"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByAttachmentPDFText(string attachmentPDFTextValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where AttachmentPDFText=", Sql.Sqlize(attachmentPDFTextValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AttachmentRAWText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AttachmentRAWText {
			get { return Fields.AttachmentRAWText.Value; }
			set { fields["AttachmentRAWText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByAttachmentRAWText(string attachmentRAWTextValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("AttachmentRAWText", attachmentRAWTextValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField AttachmentRAWText {
				get { return (AttachmentActiveField)fields["AttachmentRAWText"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByAttachmentRAWText(string attachmentRAWTextValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where AttachmentRAWText=", Sql.Sqlize(attachmentRAWTextValue));
			return GenTestList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Attachment1RAWText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class GenTest {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Attachment1RAWText {
			get { return Fields.Attachment1RAWText.Value; }
			set { fields["Attachment1RAWText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static GenTest LoadByAttachment1RAWText(string attachment1RAWTextValue) {
			return ActiveRecordLoader.LoadByField<GenTest>("Attachment1RAWText", attachment1RAWTextValue, "GenTest", Otherwise.Null);
		}

		public partial class GenTestFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField Attachment1RAWText {
				get { return (AttachmentActiveField)fields["Attachment1RAWText"]; }
			}
		}

	}
	
	// define list class 
	public partial class GenTestList {		
				
		[JetBrains.Annotations.NotNull]
		public static GenTestList LoadByAttachment1RAWText(string attachment1RAWTextValue) {
			var sql = new Sql("select * from ","GenTest".SqlizeName()," where Attachment1RAWText=", Sql.Sqlize(attachment1RAWTextValue));
			return GenTestList.Load(sql);
		}		
		
	}


}
#endregion