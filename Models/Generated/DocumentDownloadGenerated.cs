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
// CLASS: DocumentDownload
// TABLE: DocumentDownload
//-----------------------------------------


	public partial class DocumentDownload : ActiveRecord {

		/// <summary>
		/// The list that this DocumentDownload is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<DocumentDownload> GetContainingList() {
			return (ActiveRecordList<DocumentDownload>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public DocumentDownload(): base("DocumentDownload", "DocumentDownloadID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "DocumentDownload";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "DocumentDownloadID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property DocumentDownloadID.
		/// </summary>
		public int ID { get { return (int)fields["DocumentDownloadID"].ValueObject; } set { fields["DocumentDownloadID"].ValueObject = value; } }

		// field references
		public partial class DocumentDownloadFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public DocumentDownloadFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private DocumentDownloadFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public DocumentDownloadFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new DocumentDownloadFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the DocumentDownload table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of DocumentDownload</param>
		/// <returns>An instance of DocumentDownload containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static DocumentDownload LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the DocumentDownload table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg DocumentDownload.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = DocumentDownload.LoadID(55);</example>
		/// <param name="id">Primary key of DocumentDownload</param>
		/// <returns>An instance of DocumentDownload containing the data in the record</returns>
		public static DocumentDownload LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			DocumentDownload record = null;
//			record = DocumentDownload.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where DocumentDownloadID=", Sql.Sqlize(id));
//				record = new DocumentDownload();
//				if (!record.LoadData(sql)) return otherwise.Execute<DocumentDownload>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<DocumentDownload>(id, "DocumentDownload", otherwise);
		}

		/// <summary>
		/// Loads a record from the DocumentDownload table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of DocumentDownload containing the data in the record</returns>
		public static DocumentDownload Load(Sql sql) {
				return ActiveRecordLoader.Load<DocumentDownload>(sql, "DocumentDownload");
		}
		public static DocumentDownload Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<DocumentDownload>(sql, "DocumentDownload", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the DocumentDownload table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of DocumentDownload containing the data in the record</returns>
		public static DocumentDownload Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<DocumentDownload>(reader, "DocumentDownload");
		}
		public static DocumentDownload Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<DocumentDownload>(reader, "DocumentDownload", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where DocumentDownloadID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("DocumentDownloadID", new ActiveField<int>() { Name = "DocumentDownloadID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="DocumentDownload"  });

	fields.Add("DocumentID", new ActiveField<int?>() { Name = "DocumentID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentDownload" , GetForeignRecord = () => this.Document, ForeignClassName = typeof(Models.Document), ForeignTableName = "Document", ForeignTableFieldName = "DocumentID" });

	fields.Add("PersonID", new ActiveField<int?>() { Name = "PersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="DocumentDownload" , GetForeignRecord = () => this.Person, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DocumentDownload"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="DocumentDownload"  });
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
				var rec = ActiveRecordLoader.LoadID<DocumentDownload>(id, "DocumentDownload", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the DocumentDownload with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct DocumentDownload or null if not in cache.</returns>
//		private static DocumentDownload GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-DocumentDownload-" + id) as DocumentDownload;
//			return Web.PageGlobals["ActiveRecord-DocumentDownload-" + id] as DocumentDownload;
//		}

		/// <summary>
		/// Caches this DocumentDownload object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-DocumentDownload-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-DocumentDownload-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-DocumentDownload-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of DocumentDownload objects/records. This is the usual data structure for holding a number of DocumentDownload records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class DocumentDownloadList : ActiveRecordList<DocumentDownload> {

		public DocumentDownloadList() : base() {}
		public DocumentDownloadList(List<DocumentDownload> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-DocumentDownload to DocumentDownloadList. 
		/// </summary>
		public static implicit operator DocumentDownloadList(List<DocumentDownload> list) {
			return new DocumentDownloadList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from DocumentDownloadList to List-of-DocumentDownload. 
		/// </summary>
		public static implicit operator List<DocumentDownload>(DocumentDownloadList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of DocumentDownload objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of DocumentDownload records.</returns>
		public static DocumentDownloadList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where DocumentDownloadID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of DocumentDownload objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of DocumentDownload records.</returns>
		public static DocumentDownloadList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static DocumentDownloadList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where DocumentDownloadID in (", ids.SqlizeNumberList(), ")");
			var result = new DocumentDownloadList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of DocumentDownload objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of DocumentDownload records.</returns>
		public static DocumentDownloadList Load(Sql sql) {
			var result = new DocumentDownloadList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all DocumentDownload objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and DocumentDownloadID desc.)
		/// </summary>
		public static DocumentDownloadList LoadAll() {
			var result = new DocumentDownloadList();
			result.LoadRecords(null);
			return result;
		}
		public static DocumentDownloadList LoadAll(int itemsPerPage) {
			var result = new DocumentDownloadList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentDownloadList LoadAll(int itemsPerPage, int pageNum) {
			var result = new DocumentDownloadList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" DocumentDownload objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static DocumentDownloadList LoadActive() {
			var result = new DocumentDownloadList();
			var sql = (new DocumentDownload()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentDownloadList LoadActive(int itemsPerPage) {
			var result = new DocumentDownloadList();
			var sql = (new DocumentDownload()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentDownloadList LoadActive(int itemsPerPage, int pageNum) {
			var result = new DocumentDownloadList();
			var sql = (new DocumentDownload()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static DocumentDownloadList LoadActivePlusExisting(object existingRecordID) {
			var result = new DocumentDownloadList();
			var sql = (new DocumentDownload()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM DocumentDownload");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM DocumentDownload");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new DocumentDownload()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = DocumentDownload.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: DocumentDownloadID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentDownload {		
				
		[JetBrains.Annotations.CanBeNull]
		public int DocumentDownloadID {
			get { return Fields.DocumentDownloadID.Value; }
			set { fields["DocumentDownloadID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentDownload LoadByDocumentDownloadID(int documentDownloadIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentDownload>("DocumentDownloadID", documentDownloadIDValue, "DocumentDownload", Otherwise.Null);
		}

		public partial class DocumentDownloadFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> DocumentDownloadID {
				get { return (ActiveField<int>)fields["DocumentDownloadID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentDownloadList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentDownloadList LoadByDocumentDownloadID(int documentDownloadIDValue) {
			var sql = new Sql("select * from ","DocumentDownload".SqlizeName()," where DocumentDownloadID=", Sql.Sqlize(documentDownloadIDValue));
			return DocumentDownloadList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DocumentID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentDownload {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? DocumentID {
			get { return Fields.DocumentID.Value; }
			set { fields["DocumentID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentDownload LoadByDocumentID(int? documentIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentDownload>("DocumentID", documentIDValue, "DocumentDownload", Otherwise.Null);
		}

		public partial class DocumentDownloadFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> DocumentID {
				get { return (ActiveField<int?>)fields["DocumentID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentDownloadList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentDownloadList LoadByDocumentID(int? documentIDValue) {
			var sql = new Sql("select * from ","DocumentDownload".SqlizeName()," where DocumentID=", Sql.Sqlize(documentIDValue));
			return DocumentDownloadList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Document
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class DocumentDownload {
		[NonSerialized]		
		private Document _Document;

		[JetBrains.Annotations.CanBeNull]
		public Document Document
		{
			get
			{
				 // lazy load
				if (this._Document == null && this.DocumentID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("Document") && container.PrefetchCounter["Document"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Document>("DocumentID",container.Select(r=>r.DocumentID).ToList(),"Document",Otherwise.Null);
					}
					this._Document = Models.Document.LoadByDocumentID(DocumentID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("Document")) {
							container.PrefetchCounter["Document"] = 0;
						}
						container.PrefetchCounter["Document"]++;
					}
				}
				return this._Document;
			}
			set
			{
				this._Document = value;
			}
		}
	}

	public partial class DocumentDownloadList {
		internal int numFetchesOfDocument = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Document {
		[NonSerialized]		
		private DocumentDownloadList _DocumentDownloads;
		
		[JetBrains.Annotations.NotNull]
		public DocumentDownloadList DocumentDownloads
		{
			get
			{
				// lazy load
				if (this._DocumentDownloads == null) {
					this._DocumentDownloads = Models.DocumentDownloadList.LoadByDocumentID(this.ID);
					this._DocumentDownloads.SetParentBindField(this, "DocumentID");
				}
				return this._DocumentDownloads;
			}
			set
			{
				this._DocumentDownloads = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: PersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentDownload {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PersonID {
			get { return Fields.PersonID.Value; }
			set { fields["PersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentDownload LoadByPersonID(int? personIDValue) {
			return ActiveRecordLoader.LoadByField<DocumentDownload>("PersonID", personIDValue, "DocumentDownload", Otherwise.Null);
		}

		public partial class DocumentDownloadFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PersonID {
				get { return (ActiveField<int?>)fields["PersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentDownloadList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentDownloadList LoadByPersonID(int? personIDValue) {
			var sql = new Sql("select * from ","DocumentDownload".SqlizeName()," where PersonID=", Sql.Sqlize(personIDValue));
			return DocumentDownloadList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: Person
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class DocumentDownload {
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

	public partial class DocumentDownloadList {
		internal int numFetchesOfPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private DocumentDownloadList _DocumentDownloads;
		
		[JetBrains.Annotations.NotNull]
		public DocumentDownloadList DocumentDownloads
		{
			get
			{
				// lazy load
				if (this._DocumentDownloads == null) {
					this._DocumentDownloads = Models.DocumentDownloadList.LoadByPersonID(this.ID);
					this._DocumentDownloads.SetParentBindField(this, "PersonID");
				}
				return this._DocumentDownloads;
			}
			set
			{
				this._DocumentDownloads = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentDownload {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentDownload LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<DocumentDownload>("DateAdded", dateAddedValue, "DocumentDownload", Otherwise.Null);
		}

		public partial class DocumentDownloadFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentDownloadList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentDownloadList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","DocumentDownload".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return DocumentDownloadList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class DocumentDownload {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static DocumentDownload LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<DocumentDownload>("DateModified", dateModifiedValue, "DocumentDownload", Otherwise.Null);
		}

		public partial class DocumentDownloadFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentDownloadList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentDownloadList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","DocumentDownload".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return DocumentDownloadList.Load(sql);
		}		
		
	}


}
#endregion