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
// CLASS: Document
// TABLE: Document
//-----------------------------------------


	public partial class Document : ActiveRecord {

		/// <summary>
		/// The list that this Document is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Document> GetContainingList() {
			return (ActiveRecordList<Document>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Document(): base("Document", "DocumentID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Document";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "DocumentID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property DocumentID.
		/// </summary>
		public int ID { get { return (int)fields["DocumentID"].ValueObject; } set { fields["DocumentID"].ValueObject = value; } }

		// field references
		public partial class DocumentFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public DocumentFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private DocumentFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public DocumentFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new DocumentFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Document table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Document</param>
		/// <returns>An instance of Document containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Document LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Document table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Document.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Document.LoadID(55);</example>
		/// <param name="id">Primary key of Document</param>
		/// <returns>An instance of Document containing the data in the record</returns>
		public static Document LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Document record = null;
//			record = Document.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where DocumentID=", Sql.Sqlize(id));
//				record = new Document();
//				if (!record.LoadData(sql)) return otherwise.Execute<Document>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Document>(id, "Document", otherwise);
		}

		/// <summary>
		/// Loads a record from the Document table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Document containing the data in the record</returns>
		public static Document Load(Sql sql) {
				return ActiveRecordLoader.Load<Document>(sql, "Document");
		}
		public static Document Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Document>(sql, "Document", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Document table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Document containing the data in the record</returns>
		public static Document Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Document>(reader, "Document");
		}
		public static Document Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Document>(reader, "Document", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where DocumentID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("DocumentID", new ActiveField<int>() { Name = "DocumentID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Document"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="Document"  });

	fields.Add("Description", new ActiveField<string>() { Name = "Description", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=500, TableName="Document"  });

	fields.Add("FileAttachment", new AttachmentActiveField() { Name = "FileAttachment", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Document"  });

	fields.Add("DocumentCategoryID", new ActiveField<int?>() { Name = "DocumentCategoryID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Document" , GetForeignRecord = () => this.DocumentCategory, ForeignClassName = typeof(Models.DocumentCategory), ForeignTableName = "DocumentCategory", ForeignTableFieldName = "DocumentCategoryID" });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Document"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Document"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Document"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Document"  });

	fields.Add("AddedByPersonID", new ActiveField<int?>() { Name = "AddedByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Document" , GetForeignRecord = () => this.AddedByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Document"  });

	fields.Add("ModifiedByPersonID", new ActiveField<int?>() { Name = "ModifiedByPersonID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="Document" , GetForeignRecord = () => this.ModifiedByPerson, ForeignClassName = typeof(Models.Person), ForeignTableName = "Person", ForeignTableFieldName = "PersonID" });
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
				var rec = ActiveRecordLoader.LoadID<Document>(id, "Document", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Document with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Document or null if not in cache.</returns>
//		private static Document GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Document-" + id) as Document;
//			return Web.PageGlobals["ActiveRecord-Document-" + id] as Document;
//		}

		/// <summary>
		/// Caches this Document object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Document-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Document-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Document-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Document objects/records. This is the usual data structure for holding a number of Document records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class DocumentList : ActiveRecordList<Document> {

		public DocumentList() : base() {}
		public DocumentList(List<Document> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Document to DocumentList. 
		/// </summary>
		public static implicit operator DocumentList(List<Document> list) {
			return new DocumentList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from DocumentList to List-of-Document. 
		/// </summary>
		public static implicit operator List<Document>(DocumentList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Document objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Document records.</returns>
		public static DocumentList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where DocumentID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Document objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Document records.</returns>
		public static DocumentList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static DocumentList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where DocumentID in (", ids.SqlizeNumberList(), ")");
			var result = new DocumentList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Document objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Document records.</returns>
		public static DocumentList Load(Sql sql) {
			var result = new DocumentList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Document objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and DocumentID desc.)
		/// </summary>
		public static DocumentList LoadAll() {
			var result = new DocumentList();
			result.LoadRecords(null);
			return result;
		}
		public static DocumentList LoadAll(int itemsPerPage) {
			var result = new DocumentList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentList LoadAll(int itemsPerPage, int pageNum) {
			var result = new DocumentList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Document objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static DocumentList LoadActive() {
			var result = new DocumentList();
			var sql = (new Document()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentList LoadActive(int itemsPerPage) {
			var result = new DocumentList();
			var sql = (new Document()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static DocumentList LoadActive(int itemsPerPage, int pageNum) {
			var result = new DocumentList();
			var sql = (new Document()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static DocumentList LoadActivePlusExisting(object existingRecordID) {
			var result = new DocumentList();
			var sql = (new Document()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Document");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Document");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Document()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Document.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: DocumentID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public int DocumentID {
			get { return Fields.DocumentID.Value; }
			set { fields["DocumentID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByDocumentID(int documentIDValue) {
			return ActiveRecordLoader.LoadByField<Document>("DocumentID", documentIDValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> DocumentID {
				get { return (ActiveField<int>)fields["DocumentID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByDocumentID(int documentIDValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where DocumentID=", Sql.Sqlize(documentIDValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Document>("Title", titleValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Description
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Description {
			get { return Fields.Description.Value; }
			set { fields["Description"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByDescription(string descriptionValue) {
			return ActiveRecordLoader.LoadByField<Document>("Description", descriptionValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Description {
				get { return (ActiveField<string>)fields["Description"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByDescription(string descriptionValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where Description=", Sql.Sqlize(descriptionValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FileAttachment
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FileAttachment {
			get { return Fields.FileAttachment.Value; }
			set { fields["FileAttachment"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByFileAttachment(string fileAttachmentValue) {
			return ActiveRecordLoader.LoadByField<Document>("FileAttachment", fileAttachmentValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public AttachmentActiveField FileAttachment {
				get { return (AttachmentActiveField)fields["FileAttachment"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByFileAttachment(string fileAttachmentValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where FileAttachment=", Sql.Sqlize(fileAttachmentValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DocumentCategoryID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? DocumentCategoryID {
			get { return Fields.DocumentCategoryID.Value; }
			set { fields["DocumentCategoryID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByDocumentCategoryID(int? documentCategoryIDValue) {
			return ActiveRecordLoader.LoadByField<Document>("DocumentCategoryID", documentCategoryIDValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> DocumentCategoryID {
				get { return (ActiveField<int?>)fields["DocumentCategoryID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByDocumentCategoryID(int? documentCategoryIDValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where DocumentCategoryID=", Sql.Sqlize(documentCategoryIDValue));
			return DocumentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: DocumentCategory
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Document {
		[NonSerialized]		
		private DocumentCategory _DocumentCategory;

		[JetBrains.Annotations.CanBeNull]
		public DocumentCategory DocumentCategory
		{
			get
			{
				 // lazy load
				if (this._DocumentCategory == null && this.DocumentCategoryID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("DocumentCategory") && container.PrefetchCounter["DocumentCategory"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.DocumentCategory>("DocumentCategoryID",container.Select(r=>r.DocumentCategoryID).ToList(),"DocumentCategory",Otherwise.Null);
					}
					this._DocumentCategory = Models.DocumentCategory.LoadByDocumentCategoryID(DocumentCategoryID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("DocumentCategory")) {
							container.PrefetchCounter["DocumentCategory"] = 0;
						}
						container.PrefetchCounter["DocumentCategory"]++;
					}
				}
				return this._DocumentCategory;
			}
			set
			{
				this._DocumentCategory = value;
			}
		}
	}

	public partial class DocumentList {
		internal int numFetchesOfDocumentCategory = 0;
	}
	
	// define list in partial foreign table class 
	public partial class DocumentCategory {
		[NonSerialized]		
		private DocumentList _Documents;
		
		[JetBrains.Annotations.NotNull]
		public DocumentList Documents
		{
			get
			{
				// lazy load
				if (this._Documents == null) {
					this._Documents = Models.DocumentList.LoadByDocumentCategoryID(this.ID);
					this._Documents.SetParentBindField(this, "DocumentCategoryID");
				}
				return this._Documents;
			}
			set
			{
				this._Documents = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<Document>("SortPosition", sortPositionValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<Document>("PublishDate", publishDateValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<Document>("ExpiryDate", expiryDateValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Document>("DateAdded", dateAddedValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AddedByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? AddedByPersonID {
			get { return Fields.AddedByPersonID.Value; }
			set { fields["AddedByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByAddedByPersonID(int? addedByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<Document>("AddedByPersonID", addedByPersonIDValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> AddedByPersonID {
				get { return (ActiveField<int?>)fields["AddedByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByAddedByPersonID(int? addedByPersonIDValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where AddedByPersonID=", Sql.Sqlize(addedByPersonIDValue));
			return DocumentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: AddedByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Document {
		[NonSerialized]		
		private Person _AddedByPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person AddedByPerson
		{
			get
			{
				 // lazy load
				if (this._AddedByPerson == null && this.AddedByPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("AddedByPerson") && container.PrefetchCounter["AddedByPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.AddedByPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._AddedByPerson = Models.Person.LoadByPersonID(AddedByPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("AddedByPerson")) {
							container.PrefetchCounter["AddedByPerson"] = 0;
						}
						container.PrefetchCounter["AddedByPerson"]++;
					}
				}
				return this._AddedByPerson;
			}
			set
			{
				this._AddedByPerson = value;
			}
		}
	}

	public partial class DocumentList {
		internal int numFetchesOfAddedByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private DocumentList _DocumentsAddedBy;
		
		[JetBrains.Annotations.NotNull]
		public DocumentList DocumentsAddedBy
		{
			get
			{
				// lazy load
				if (this._DocumentsAddedBy == null) {
					this._DocumentsAddedBy = Models.DocumentList.LoadByAddedByPersonID(this.ID);
					this._DocumentsAddedBy.SetParentBindField(this, "AddedByPersonID");
				}
				return this._DocumentsAddedBy;
			}
			set
			{
				this._DocumentsAddedBy = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Document>("DateModified", dateModifiedValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return DocumentList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ModifiedByPersonID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Document {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? ModifiedByPersonID {
			get { return Fields.ModifiedByPersonID.Value; }
			set { fields["ModifiedByPersonID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Document LoadByModifiedByPersonID(int? modifiedByPersonIDValue) {
			return ActiveRecordLoader.LoadByField<Document>("ModifiedByPersonID", modifiedByPersonIDValue, "Document", Otherwise.Null);
		}

		public partial class DocumentFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> ModifiedByPersonID {
				get { return (ActiveField<int?>)fields["ModifiedByPersonID"]; }
			}
		}

	}
	
	// define list class 
	public partial class DocumentList {		
				
		[JetBrains.Annotations.NotNull]
		public static DocumentList LoadByModifiedByPersonID(int? modifiedByPersonIDValue) {
			var sql = new Sql("select * from ","Document".SqlizeName()," where ModifiedByPersonID=", Sql.Sqlize(modifiedByPersonIDValue));
			return DocumentList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: ModifiedByPerson
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class Document {
		[NonSerialized]		
		private Person _ModifiedByPerson;

		[JetBrains.Annotations.CanBeNull]
		public Person ModifiedByPerson
		{
			get
			{
				 // lazy load
				if (this._ModifiedByPerson == null && this.ModifiedByPersonID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("ModifiedByPerson") && container.PrefetchCounter["ModifiedByPerson"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.Person>("PersonID",container.Select(r=>r.ModifiedByPersonID).ToList(),"Person",Otherwise.Null);
					}
					this._ModifiedByPerson = Models.Person.LoadByPersonID(ModifiedByPersonID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("ModifiedByPerson")) {
							container.PrefetchCounter["ModifiedByPerson"] = 0;
						}
						container.PrefetchCounter["ModifiedByPerson"]++;
					}
				}
				return this._ModifiedByPerson;
			}
			set
			{
				this._ModifiedByPerson = value;
			}
		}
	}

	public partial class DocumentList {
		internal int numFetchesOfModifiedByPerson = 0;
	}
	
	// define list in partial foreign table class 
	public partial class Person {
		[NonSerialized]		
		private DocumentList _DocumentsModifiedBy;
		
		[JetBrains.Annotations.NotNull]
		public DocumentList DocumentsModifiedBy
		{
			get
			{
				// lazy load
				if (this._DocumentsModifiedBy == null) {
					this._DocumentsModifiedBy = Models.DocumentList.LoadByModifiedByPersonID(this.ID);
					this._DocumentsModifiedBy.SetParentBindField(this, "ModifiedByPersonID");
				}
				return this._DocumentsModifiedBy;
			}
			set
			{
				this._DocumentsModifiedBy = value;
			}
		}
	}
	
}
#endregion