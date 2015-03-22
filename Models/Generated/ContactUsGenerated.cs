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
// CLASS: ContactUs
// TABLE: ContactUs
//-----------------------------------------


	public partial class ContactUs : ActiveRecord {

		/// <summary>
		/// The list that this ContactUs is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<ContactUs> GetContainingList() {
			return (ActiveRecordList<ContactUs>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ContactUs(): base("ContactUs", "ContactUsID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "ContactUs";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "ContactUsID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property ContactUsID.
		/// </summary>
		public int ID { get { return (int)fields["ContactUsID"].ValueObject; } set { fields["ContactUsID"].ValueObject = value; } }

		// field references
		public partial class ContactUsFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public ContactUsFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private ContactUsFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public ContactUsFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new ContactUsFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the ContactUs table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of ContactUs</param>
		/// <returns>An instance of ContactUs containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the ContactUs table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg ContactUs.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = ContactUs.LoadID(55);</example>
		/// <param name="id">Primary key of ContactUs</param>
		/// <returns>An instance of ContactUs containing the data in the record</returns>
		public static ContactUs LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			ContactUs record = null;
//			record = ContactUs.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where ContactUsID=", Sql.Sqlize(id));
//				record = new ContactUs();
//				if (!record.LoadData(sql)) return otherwise.Execute<ContactUs>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<ContactUs>(id, "ContactUs", otherwise);
		}

		/// <summary>
		/// Loads a record from the ContactUs table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of ContactUs containing the data in the record</returns>
		public static ContactUs Load(Sql sql) {
				return ActiveRecordLoader.Load<ContactUs>(sql, "ContactUs");
		}
		public static ContactUs Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ContactUs>(sql, "ContactUs", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the ContactUs table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of ContactUs containing the data in the record</returns>
		public static ContactUs Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<ContactUs>(reader, "ContactUs");
		}
		public static ContactUs Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<ContactUs>(reader, "ContactUs", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where ContactUsID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("ContactUsID", new ActiveField<int>() { Name = "ContactUsID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="ContactUs"  });

	fields.Add("FirstName", new ActiveField<string>() { Name = "FirstName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ContactUs"  });

	fields.Add("LastName", new ActiveField<string>() { Name = "LastName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ContactUs"  });

	fields.Add("Email", new ActiveField<string>() { Name = "Email", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ContactUs"  });

	fields.Add("Company", new ActiveField<string>() { Name = "Company", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="ContactUs"  });

	fields.Add("Comments", new ActiveField<string>() { Name = "Comments", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="ContactUs"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ContactUs"  });

	fields.Add("AdminNotes", new ActiveField<string>() { Name = "AdminNotes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="ContactUs"  });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="ContactUs" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("Screen", new ActiveField<string>() { Name = "Screen", ColumnType = "nchar", Type = typeof(string), IsAuto = false, MaxLength=25, TableName="ContactUs"  });

	fields.Add("Phone", new ActiveField<string>() { Name = "Phone", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="ContactUs"  });

	fields.Add("Name", new ActiveField<string>() { Name = "Name", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ContactUs"  });

	fields.Add("Subject", new ActiveField<string>() { Name = "Subject", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="ContactUs"  });

	fields.Add("Message", new ActiveField<string>() { Name = "Message", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="ContactUs"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="ContactUs"  });
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
				var rec = ActiveRecordLoader.LoadID<ContactUs>(id, "ContactUs", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the ContactUs with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct ContactUs or null if not in cache.</returns>
//		private static ContactUs GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-ContactUs-" + id) as ContactUs;
//			return Web.PageGlobals["ActiveRecord-ContactUs-" + id] as ContactUs;
//		}

		/// <summary>
		/// Caches this ContactUs object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-ContactUs-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-ContactUs-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-ContactUs-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of ContactUs objects/records. This is the usual data structure for holding a number of ContactUs records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class ContactUsList : ActiveRecordList<ContactUs> {

		public ContactUsList() : base() {}
		public ContactUsList(List<ContactUs> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-ContactUs to ContactUsList. 
		/// </summary>
		public static implicit operator ContactUsList(List<ContactUs> list) {
			return new ContactUsList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from ContactUsList to List-of-ContactUs. 
		/// </summary>
		public static implicit operator List<ContactUs>(ContactUsList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of ContactUs objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of ContactUs records.</returns>
		public static ContactUsList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where ContactUsID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of ContactUs objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of ContactUs records.</returns>
		public static ContactUsList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static ContactUsList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where ContactUsID in (", ids.SqlizeNumberList(), ")");
			var result = new ContactUsList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of ContactUs objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of ContactUs records.</returns>
		public static ContactUsList Load(Sql sql) {
			var result = new ContactUsList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all ContactUs objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and ContactUsID desc.)
		/// </summary>
		public static ContactUsList LoadAll() {
			var result = new ContactUsList();
			result.LoadRecords(null);
			return result;
		}
		public static ContactUsList LoadAll(int itemsPerPage) {
			var result = new ContactUsList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ContactUsList LoadAll(int itemsPerPage, int pageNum) {
			var result = new ContactUsList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" ContactUs objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static ContactUsList LoadActive() {
			var result = new ContactUsList();
			var sql = (new ContactUs()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static ContactUsList LoadActive(int itemsPerPage) {
			var result = new ContactUsList();
			var sql = (new ContactUs()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static ContactUsList LoadActive(int itemsPerPage, int pageNum) {
			var result = new ContactUsList();
			var sql = (new ContactUs()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static ContactUsList LoadActivePlusExisting(object existingRecordID) {
			var result = new ContactUsList();
			var sql = (new ContactUs()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM ContactUs");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM ContactUs");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new ContactUs()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = ContactUs.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: ContactUsID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public int ContactUsID {
			get { return Fields.ContactUsID.Value; }
			set { fields["ContactUsID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByContactUsID(int contactUsIDValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("ContactUsID", contactUsIDValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> ContactUsID {
				get { return (ActiveField<int>)fields["ContactUsID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByContactUsID(int contactUsIDValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where ContactUsID=", Sql.Sqlize(contactUsIDValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FirstName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FirstName {
			get { return Fields.FirstName.Value; }
			set { fields["FirstName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByFirstName(string firstNameValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("FirstName", firstNameValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FirstName {
				get { return (ActiveField<string>)fields["FirstName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByFirstName(string firstNameValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where FirstName=", Sql.Sqlize(firstNameValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LastName {
			get { return Fields.LastName.Value; }
			set { fields["LastName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByLastName(string lastNameValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("LastName", lastNameValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LastName {
				get { return (ActiveField<string>)fields["LastName"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByLastName(string lastNameValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where LastName=", Sql.Sqlize(lastNameValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Email
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Email {
			get { return Fields.Email.Value; }
			set { fields["Email"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByEmail(string emailValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Email", emailValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Email {
				get { return (ActiveField<string>)fields["Email"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByEmail(string emailValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Email=", Sql.Sqlize(emailValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Company
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Company {
			get { return Fields.Company.Value; }
			set { fields["Company"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByCompany(string companyValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Company", companyValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Company {
				get { return (ActiveField<string>)fields["Company"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByCompany(string companyValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Company=", Sql.Sqlize(companyValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Comments
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Comments {
			get { return Fields.Comments.Value; }
			set { fields["Comments"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByComments(string commentsValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Comments", commentsValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Comments {
				get { return (ActiveField<string>)fields["Comments"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByComments(string commentsValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Comments=", Sql.Sqlize(commentsValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("DateAdded", dateAddedValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AdminNotes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AdminNotes {
			get { return Fields.AdminNotes.Value; }
			set { fields["AdminNotes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByAdminNotes(string adminNotesValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("AdminNotes", adminNotesValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AdminNotes {
				get { return (ActiveField<string>)fields["AdminNotes"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByAdminNotes(string adminNotesValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where AdminNotes=", Sql.Sqlize(adminNotesValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("PersonID", personIDValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return ContactUsList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class ContactUs {
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

	public partial class ContactUsList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private ContactUsList _ContactUs;
		
		[JetBrains.Annotations.NotNull]
		public ContactUsList ContactUs
		{
			get
			{
				// lazy load
				if (this._ContactUs == null) {
					this._ContactUs = Models.ContactUsList.LoadByPersonID(this.ID);
					this._ContactUs.SetParentBindField(this, "PersonID");
				}
				return this._ContactUs;
			}
			set
			{
				this._ContactUs = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: Screen
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Screen {
			get { return Fields.Screen.Value; }
			set { fields["Screen"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByScreen(string screenValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Screen", screenValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Screen {
				get { return (ActiveField<string>)fields["Screen"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByScreen(string screenValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Screen=", Sql.Sqlize(screenValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Phone
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Phone {
			get { return Fields.Phone.Value; }
			set { fields["Phone"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByPhone(string phoneValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Phone", phoneValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Phone {
				get { return (ActiveField<string>)fields["Phone"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByPhone(string phoneValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Phone=", Sql.Sqlize(phoneValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Name
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Name {
			get { return Fields.Name.Value; }
			set { fields["Name"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByName(string nameValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Name", nameValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Name {
				get { return (ActiveField<string>)fields["Name"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByName(string nameValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Name=", Sql.Sqlize(nameValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Subject
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Subject {
			get { return Fields.Subject.Value; }
			set { fields["Subject"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadBySubject(string subjectValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Subject", subjectValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Subject {
				get { return (ActiveField<string>)fields["Subject"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadBySubject(string subjectValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Subject=", Sql.Sqlize(subjectValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Message
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Message {
			get { return Fields.Message.Value; }
			set { fields["Message"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByMessage(string messageValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("Message", messageValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Message {
				get { return (ActiveField<string>)fields["Message"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByMessage(string messageValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where Message=", Sql.Sqlize(messageValue));
			return ContactUsList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class ContactUs {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static ContactUs LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<ContactUs>("DateModified", dateModifiedValue, "ContactUs", Otherwise.Null);
		}

		public partial class ContactUsFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class ContactUsList {		
				
		[JetBrains.Annotations.NotNull]
		public static ContactUsList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","ContactUs".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return ContactUsList.Load(sql);
		}		
		
	}


}
#endregion