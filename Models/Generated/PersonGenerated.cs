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
// CLASS: Person
// TABLE: Person
//-----------------------------------------


	public partial class Person : ActiveRecord {

		/// <summary>
		/// The list that this Person is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Person> GetContainingList() {
			return (ActiveRecordList<Person>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Person(): base("Person", "PersonID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Person";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "PersonID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property PersonID.
		/// </summary>
		public int ID { get { return (int)fields["PersonID"].ValueObject; } set { fields["PersonID"].ValueObject = value; } }

		// field references
		public partial class PersonFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public PersonFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private PersonFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public PersonFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new PersonFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Person table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Person</param>
		/// <returns>An instance of Person containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Person LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Person table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Person.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Person.LoadID(55);</example>
		/// <param name="id">Primary key of Person</param>
		/// <returns>An instance of Person containing the data in the record</returns>
		public static Person LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Person record = null;
//			record = Person.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where PersonID=", Sql.Sqlize(id));
//				record = new Person();
//				if (!record.LoadData(sql)) return otherwise.Execute<Person>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Person>(id, "Person", otherwise);
		}

		/// <summary>
		/// Loads a record from the Person table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Person containing the data in the record</returns>
		public static Person Load(Sql sql) {
				return ActiveRecordLoader.Load<Person>(sql, "Person");
		}
		public static Person Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Person>(sql, "Person", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Person table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Person containing the data in the record</returns>
		public static Person Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Person>(reader, "Person");
		}
		public static Person Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Person>(reader, "Person", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where PersonID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("PersonID", new ActiveField<int>() { Name = "PersonID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Person"  });

	fields.Add("Email", new ActiveField<string>() { Name = "Email", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="Person"  });

	fields.Add("Password", new ActiveField<string>() { Name = "Password", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=500, TableName="Person"  });

	fields.Add("FirstName", new ActiveField<string>() { Name = "FirstName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Person"  });

	fields.Add("LastName", new ActiveField<string>() { Name = "LastName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Person"  });

	fields.Add("IsActive", new ActiveField<bool>() { Name = "IsActive", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Person"  });

	fields.Add("Role", new ActiveField<string>() { Name = "Role", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Person"  });

	fields.Add("IsDevAccess", new ActiveField<bool>() { Name = "IsDevAccess", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="Person"  });

	fields.Add("LoginCount", new ActiveField<int?>() { Name = "LoginCount", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Person"  });

	fields.Add("FailedLoginCount", new ActiveField<int?>() { Name = "FailedLoginCount", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Person"  });

	fields.Add("LastLoginDate", new ActiveField<System.DateTime?>() { Name = "LastLoginDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Person"  });

	fields.Add("LastIpAddress", new ActiveField<string>() { Name = "LastIpAddress", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Person"  });

	fields.Add("ResetCount", new ActiveField<int?>() { Name = "ResetCount", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Person"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Person"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Person"  });

	fields.Add("ResetId", new ActiveField<string>() { Name = "ResetId", ColumnType = "varchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Person"  });

	fields.Add("ResetDate", new ActiveField<System.DateTime?>() { Name = "ResetDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Person"  });
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
				var rec = ActiveRecordLoader.LoadID<Person>(id, "Person", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Person with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Person or null if not in cache.</returns>
//		private static Person GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Person-" + id) as Person;
//			return Web.PageGlobals["ActiveRecord-Person-" + id] as Person;
//		}

		/// <summary>
		/// Caches this Person object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Person-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Person-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Person-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Person objects/records. This is the usual data structure for holding a number of Person records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class PersonList : ActiveRecordList<Person> {

		public PersonList() : base() {}
		public PersonList(List<Person> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Person to PersonList. 
		/// </summary>
		public static implicit operator PersonList(List<Person> list) {
			return new PersonList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from PersonList to List-of-Person. 
		/// </summary>
		public static implicit operator List<Person>(PersonList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Person objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Person records.</returns>
		public static PersonList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where PersonID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Person objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Person records.</returns>
		public static PersonList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static PersonList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where PersonID in (", ids.SqlizeNumberList(), ")");
			var result = new PersonList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Person objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Person records.</returns>
		public static PersonList Load(Sql sql) {
			var result = new PersonList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Person objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and PersonID desc.)
		/// </summary>
		public static PersonList LoadAll() {
			var result = new PersonList();
			result.LoadRecords(null);
			return result;
		}
		public static PersonList LoadAll(int itemsPerPage) {
			var result = new PersonList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static PersonList LoadAll(int itemsPerPage, int pageNum) {
			var result = new PersonList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Person objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static PersonList LoadActive() {
			var result = new PersonList();
			var sql = (new Person()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static PersonList LoadActive(int itemsPerPage) {
			var result = new PersonList();
			var sql = (new Person()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static PersonList LoadActive(int itemsPerPage, int pageNum) {
			var result = new PersonList();
			var sql = (new Person()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static PersonList LoadActivePlusExisting(object existingRecordID) {
			var result = new PersonList();
			var sql = (new Person()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Person");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Person");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Person()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Person.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public int PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByPersonID(int personIDValue) {
			return ActiveRecordLoader.LoadByField<Person>("PersonID", personIDValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> PersonID {
				get { return (ActiveField<int>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByPersonID(int personIDValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Email
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Email {
			get { return Fields.Email.Value; }
			set { fields["Email"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByEmail(string emailValue) {
			return ActiveRecordLoader.LoadByField<Person>("Email", emailValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Email {
				get { return (ActiveField<string>)fields["Email"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByEmail(string emailValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where Email=", Sql.Sqlize(emailValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Password
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Password {
			get { return Fields.Password.Value; }
			set { fields["Password"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByPassword(string passwordValue) {
			return ActiveRecordLoader.LoadByField<Person>("Password", passwordValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Password {
				get { return (ActiveField<string>)fields["Password"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByPassword(string passwordValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where Password=", Sql.Sqlize(passwordValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FirstName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FirstName {
			get { return Fields.FirstName.Value; }
			set { fields["FirstName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByFirstName(string firstNameValue) {
			return ActiveRecordLoader.LoadByField<Person>("FirstName", firstNameValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FirstName {
				get { return (ActiveField<string>)fields["FirstName"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByFirstName(string firstNameValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where FirstName=", Sql.Sqlize(firstNameValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LastName {
			get { return Fields.LastName.Value; }
			set { fields["LastName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByLastName(string lastNameValue) {
			return ActiveRecordLoader.LoadByField<Person>("LastName", lastNameValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LastName {
				get { return (ActiveField<string>)fields["LastName"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByLastName(string lastNameValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where LastName=", Sql.Sqlize(lastNameValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsActive
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsActive {
			get { return Fields.IsActive.Value; }
			set { fields["IsActive"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByIsActive(bool isActiveValue) {
			return ActiveRecordLoader.LoadByField<Person>("IsActive", isActiveValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsActive {
				get { return (ActiveField<bool>)fields["IsActive"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByIsActive(bool isActiveValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where IsActive=", Sql.Sqlize(isActiveValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Role
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Role {
			get { return Fields.Role.Value; }
			set { fields["Role"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByRole(string roleValue) {
			return ActiveRecordLoader.LoadByField<Person>("Role", roleValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Role {
				get { return (ActiveField<string>)fields["Role"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByRole(string roleValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where Role=", Sql.Sqlize(roleValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsDevAccess
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsDevAccess {
			get { return Fields.IsDevAccess.Value; }
			set { fields["IsDevAccess"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByIsDevAccess(bool isDevAccessValue) {
			return ActiveRecordLoader.LoadByField<Person>("IsDevAccess", isDevAccessValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsDevAccess {
				get { return (ActiveField<bool>)fields["IsDevAccess"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByIsDevAccess(bool isDevAccessValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where IsDevAccess=", Sql.Sqlize(isDevAccessValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LoginCount
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? LoginCount {
			get { return Fields.LoginCount.Value; }
			set { fields["LoginCount"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByLoginCount(int? loginCountValue) {
			return ActiveRecordLoader.LoadByField<Person>("LoginCount", loginCountValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> LoginCount {
				get { return (ActiveField<int?>)fields["LoginCount"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByLoginCount(int? loginCountValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where LoginCount=", Sql.Sqlize(loginCountValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FailedLoginCount
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? FailedLoginCount {
			get { return Fields.FailedLoginCount.Value; }
			set { fields["FailedLoginCount"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByFailedLoginCount(int? failedLoginCountValue) {
			return ActiveRecordLoader.LoadByField<Person>("FailedLoginCount", failedLoginCountValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> FailedLoginCount {
				get { return (ActiveField<int?>)fields["FailedLoginCount"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByFailedLoginCount(int? failedLoginCountValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where FailedLoginCount=", Sql.Sqlize(failedLoginCountValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastLoginDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? LastLoginDate {
			get { return Fields.LastLoginDate.Value; }
			set { fields["LastLoginDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByLastLoginDate(System.DateTime? lastLoginDateValue) {
			return ActiveRecordLoader.LoadByField<Person>("LastLoginDate", lastLoginDateValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> LastLoginDate {
				get { return (ActiveField<System.DateTime?>)fields["LastLoginDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByLastLoginDate(System.DateTime? lastLoginDateValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where LastLoginDate=", Sql.Sqlize(lastLoginDateValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: LastIpAddress
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string LastIpAddress {
			get { return Fields.LastIpAddress.Value; }
			set { fields["LastIpAddress"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByLastIpAddress(string lastIpAddressValue) {
			return ActiveRecordLoader.LoadByField<Person>("LastIpAddress", lastIpAddressValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> LastIpAddress {
				get { return (ActiveField<string>)fields["LastIpAddress"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByLastIpAddress(string lastIpAddressValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where LastIpAddress=", Sql.Sqlize(lastIpAddressValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ResetCount
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ResetCount {
			get { return Fields.ResetCount.Value; }
			set { fields["ResetCount"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByResetCount(int? resetCountValue) {
			return ActiveRecordLoader.LoadByField<Person>("ResetCount", resetCountValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ResetCount {
				get { return (ActiveField<int?>)fields["ResetCount"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByResetCount(int? resetCountValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where ResetCount=", Sql.Sqlize(resetCountValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Person>("DateAdded", dateAddedValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Person>("DateModified", dateModifiedValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ResetID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public string ResetID {
			get { return Fields.ResetID.Value; }
			set { fields["ResetId"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByResetID(string resetIDValue) {
			return ActiveRecordLoader.LoadByField<Person>("ResetId", resetIDValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> ResetID {
				get { return (ActiveField<string>)fields["ResetId"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByResetID(string resetIDValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where ResetId=", Sql.Sqlize(resetIDValue));
			return PersonList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ResetDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Person {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ResetDate {
			get { return Fields.ResetDate.Value; }
			set { fields["ResetDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Person LoadByResetDate(System.DateTime? resetDateValue) {
			return ActiveRecordLoader.LoadByField<Person>("ResetDate", resetDateValue, "Person", Otherwise.Null);
		}

		public partial class PersonFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ResetDate {
				get { return (ActiveField<System.DateTime?>)fields["ResetDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class PersonList {		
				
		[JetBrains.Annotations.NotNull]
		public static PersonList LoadByResetDate(System.DateTime? resetDateValue) {
			var sql = new Sql("select * from ","Person".SqlizeName()," where ResetDate=", Sql.Sqlize(resetDateValue));
			return PersonList.Load(sql);
		}		
		
	}


}
#endregion