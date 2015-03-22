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
// CLASS: TextBlock
// TABLE: TextBlock
//-----------------------------------------


	public partial class TextBlock : ActiveRecord {

		/// <summary>
		/// The list that this TextBlock is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<TextBlock> GetContainingList() {
			return (ActiveRecordList<TextBlock>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public TextBlock(): base("TextBlock", "TextBlockID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "TextBlock";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "TextBlockID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property TextBlockID.
		/// </summary>
		public int ID { get { return (int)fields["TextBlockID"].ValueObject; } set { fields["TextBlockID"].ValueObject = value; } }

		// field references
		public partial class TextBlockFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public TextBlockFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private TextBlockFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public TextBlockFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new TextBlockFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the TextBlock table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of TextBlock</param>
		/// <returns>An instance of TextBlock containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the TextBlock table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg TextBlock.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = TextBlock.LoadID(55);</example>
		/// <param name="id">Primary key of TextBlock</param>
		/// <returns>An instance of TextBlock containing the data in the record</returns>
		public static TextBlock LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			TextBlock record = null;
//			record = TextBlock.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where TextBlockID=", Sql.Sqlize(id));
//				record = new TextBlock();
//				if (!record.LoadData(sql)) return otherwise.Execute<TextBlock>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<TextBlock>(id, "TextBlock", otherwise);
		}

		/// <summary>
		/// Loads a record from the TextBlock table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of TextBlock containing the data in the record</returns>
		public static TextBlock Load(Sql sql) {
				return ActiveRecordLoader.Load<TextBlock>(sql, "TextBlock");
		}
		public static TextBlock Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<TextBlock>(sql, "TextBlock", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the TextBlock table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of TextBlock containing the data in the record</returns>
		public static TextBlock Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<TextBlock>(reader, "TextBlock");
		}
		public static TextBlock Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<TextBlock>(reader, "TextBlock", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where TextBlockID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("TextBlockID", new ActiveField<int>() { Name = "TextBlockID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="TextBlock"  });

	fields.Add("SectionCode", new ActiveField<string>() { Name = "SectionCode", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="TextBlock"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="TextBlock"  });

	fields.Add("IsTitleAvailable", new ActiveField<bool>() { Name = "IsTitleAvailable", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="TextBlock"  });

	fields.Add("BodyTextHtml", new ActiveField<string>() { Name = "BodyTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="TextBlock"  });

	fields.Add("IsBodyPlainText", new ActiveField<bool>() { Name = "IsBodyPlainText", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="TextBlock"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="TextBlock"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="TextBlock"  });

	fields.Add("IsPictureAvailable", new ActiveField<bool>() { Name = "IsPictureAvailable", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="TextBlock"  });

	fields.Add("PictureWidth", new ActiveField<int?>() { Name = "PictureWidth", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="TextBlock"  });

	fields.Add("PictureHeight", new ActiveField<int?>() { Name = "PictureHeight", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="TextBlock"  });

	fields.Add("Url", new ActiveField<string>() { Name = "Url", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=255, TableName="TextBlock"  });

	fields.Add("IsUrlAvailable", new ActiveField<bool>() { Name = "IsUrlAvailable", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="TextBlock"  });

	fields.Add("UrlCaption", new ActiveField<string>() { Name = "UrlCaption", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="TextBlock"  });

	fields.Add("PictureCaption", new ActiveField<string>() { Name = "PictureCaption", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="TextBlock"  });

	fields.Add("TextBlockGroupID", new ActiveField<int?>() { Name = "TextBlockGroupID", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="TextBlock" , GetForeignRecord = () => this.TextBlockGroup, ForeignClassName = typeof(Models.TextBlockGroup), ForeignTableName = "TextBlockGroup", ForeignTableFieldName = "TextBlockGroupID" });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="TextBlock"  });

	fields.Add("MetaDescription", new ActiveField<string>() { Name = "MetaDescription", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="TextBlock"  });

	fields.Add("MetaKeywords", new ActiveField<string>() { Name = "MetaKeywords", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="TextBlock"  });

	fields.Add("PageTitleTag", new ActiveField<string>() { Name = "PageTitleTag", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=150, TableName="TextBlock"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="TextBlock"  });

	fields.Add("HasMailMergefields", new ActiveField<bool>() { Name = "HasMailMergefields", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="TextBlock"  });

	fields.Add("MergefieldHelp", new ActiveField<string>() { Name = "MergefieldHelp", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="TextBlock"  });

	fields.Add("AdminNotes", new ActiveField<string>() { Name = "AdminNotes", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="TextBlock"  });
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
				var rec = ActiveRecordLoader.LoadID<TextBlock>(id, "TextBlock", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the TextBlock with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct TextBlock or null if not in cache.</returns>
//		private static TextBlock GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-TextBlock-" + id) as TextBlock;
//			return Web.PageGlobals["ActiveRecord-TextBlock-" + id] as TextBlock;
//		}

		/// <summary>
		/// Caches this TextBlock object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-TextBlock-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-TextBlock-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-TextBlock-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of TextBlock objects/records. This is the usual data structure for holding a number of TextBlock records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class TextBlockList : ActiveRecordList<TextBlock> {

		public TextBlockList() : base() {}
		public TextBlockList(List<TextBlock> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-TextBlock to TextBlockList. 
		/// </summary>
		public static implicit operator TextBlockList(List<TextBlock> list) {
			return new TextBlockList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from TextBlockList to List-of-TextBlock. 
		/// </summary>
		public static implicit operator List<TextBlock>(TextBlockList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of TextBlock objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of TextBlock records.</returns>
		public static TextBlockList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where TextBlockID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of TextBlock objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of TextBlock records.</returns>
		public static TextBlockList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static TextBlockList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where TextBlockID in (", ids.SqlizeNumberList(), ")");
			var result = new TextBlockList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of TextBlock objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of TextBlock records.</returns>
		public static TextBlockList Load(Sql sql) {
			var result = new TextBlockList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all TextBlock objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and TextBlockID desc.)
		/// </summary>
		public static TextBlockList LoadAll() {
			var result = new TextBlockList();
			result.LoadRecords(null);
			return result;
		}
		public static TextBlockList LoadAll(int itemsPerPage) {
			var result = new TextBlockList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static TextBlockList LoadAll(int itemsPerPage, int pageNum) {
			var result = new TextBlockList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" TextBlock objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static TextBlockList LoadActive() {
			var result = new TextBlockList();
			var sql = (new TextBlock()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static TextBlockList LoadActive(int itemsPerPage) {
			var result = new TextBlockList();
			var sql = (new TextBlock()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static TextBlockList LoadActive(int itemsPerPage, int pageNum) {
			var result = new TextBlockList();
			var sql = (new TextBlock()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static TextBlockList LoadActivePlusExisting(object existingRecordID) {
			var result = new TextBlockList();
			var sql = (new TextBlock()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM TextBlock");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM TextBlock");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new TextBlock()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = TextBlock.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: TextBlockID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int TextBlockID {
			get { return Fields.TextBlockID.Value; }
			set { fields["TextBlockID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByTextBlockID(int textBlockIDValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("TextBlockID", textBlockIDValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> TextBlockID {
				get { return (ActiveField<int>)fields["TextBlockID"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByTextBlockID(int textBlockIDValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where TextBlockID=", Sql.Sqlize(textBlockIDValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SectionCode
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SectionCode {
			get { return Fields.SectionCode.Value; }
			set { fields["SectionCode"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadBySectionCode(string sectionCodeValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("SectionCode", sectionCodeValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SectionCode {
				get { return (ActiveField<string>)fields["SectionCode"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadBySectionCode(string sectionCodeValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where SectionCode=", Sql.Sqlize(sectionCodeValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("Title", titleValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsTitleAvailable
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsTitleAvailable {
			get { return Fields.IsTitleAvailable.Value; }
			set { fields["IsTitleAvailable"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByIsTitleAvailable(bool isTitleAvailableValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("IsTitleAvailable", isTitleAvailableValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsTitleAvailable {
				get { return (ActiveField<bool>)fields["IsTitleAvailable"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByIsTitleAvailable(bool isTitleAvailableValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where IsTitleAvailable=", Sql.Sqlize(isTitleAvailableValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHtml {
			get { return Fields.BodyTextHtml.Value; }
			set { fields["BodyTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByBodyTextHtml(string bodyTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("BodyTextHtml", bodyTextHtmlValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHtml {
				get { return (ActiveField<string>)fields["BodyTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByBodyTextHtml(string bodyTextHtmlValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where BodyTextHtml=", Sql.Sqlize(bodyTextHtmlValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsBodyPlainText
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsBodyPlainText {
			get { return Fields.IsBodyPlainText.Value; }
			set { fields["IsBodyPlainText"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByIsBodyPlainText(bool isBodyPlainTextValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("IsBodyPlainText", isBodyPlainTextValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsBodyPlainText {
				get { return (ActiveField<bool>)fields["IsBodyPlainText"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByIsBodyPlainText(bool isBodyPlainTextValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where IsBodyPlainText=", Sql.Sqlize(isBodyPlainTextValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("SortPosition", sortPositionValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("Picture", pictureValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPictureAvailable
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPictureAvailable {
			get { return Fields.IsPictureAvailable.Value; }
			set { fields["IsPictureAvailable"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByIsPictureAvailable(bool isPictureAvailableValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("IsPictureAvailable", isPictureAvailableValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPictureAvailable {
				get { return (ActiveField<bool>)fields["IsPictureAvailable"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByIsPictureAvailable(bool isPictureAvailableValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where IsPictureAvailable=", Sql.Sqlize(isPictureAvailableValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PictureWidth
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PictureWidth {
			get { return Fields.PictureWidth.Value; }
			set { fields["PictureWidth"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByPictureWidth(int? pictureWidthValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("PictureWidth", pictureWidthValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PictureWidth {
				get { return (ActiveField<int?>)fields["PictureWidth"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByPictureWidth(int? pictureWidthValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where PictureWidth=", Sql.Sqlize(pictureWidthValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PictureHeight
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? PictureHeight {
			get { return Fields.PictureHeight.Value; }
			set { fields["PictureHeight"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByPictureHeight(int? pictureHeightValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("PictureHeight", pictureHeightValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> PictureHeight {
				get { return (ActiveField<int?>)fields["PictureHeight"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByPictureHeight(int? pictureHeightValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where PictureHeight=", Sql.Sqlize(pictureHeightValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Url
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Url {
			get { return Fields.Url.Value; }
			set { fields["Url"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByUrl(string urlValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("Url", urlValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Url {
				get { return (ActiveField<string>)fields["Url"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByUrl(string urlValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where Url=", Sql.Sqlize(urlValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsUrlAvailable
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsUrlAvailable {
			get { return Fields.IsUrlAvailable.Value; }
			set { fields["IsUrlAvailable"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByIsUrlAvailable(bool isUrlAvailableValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("IsUrlAvailable", isUrlAvailableValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsUrlAvailable {
				get { return (ActiveField<bool>)fields["IsUrlAvailable"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByIsUrlAvailable(bool isUrlAvailableValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where IsUrlAvailable=", Sql.Sqlize(isUrlAvailableValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: UrlCaption
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string UrlCaption {
			get { return Fields.UrlCaption.Value; }
			set { fields["UrlCaption"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByUrlCaption(string urlCaptionValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("UrlCaption", urlCaptionValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> UrlCaption {
				get { return (ActiveField<string>)fields["UrlCaption"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByUrlCaption(string urlCaptionValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where UrlCaption=", Sql.Sqlize(urlCaptionValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PictureCaption
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PictureCaption {
			get { return Fields.PictureCaption.Value; }
			set { fields["PictureCaption"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByPictureCaption(string pictureCaptionValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("PictureCaption", pictureCaptionValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PictureCaption {
				get { return (ActiveField<string>)fields["PictureCaption"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByPictureCaption(string pictureCaptionValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where PictureCaption=", Sql.Sqlize(pictureCaptionValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: TextBlockGroupID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? TextBlockGroupID {
			get { return Fields.TextBlockGroupID.Value; }
			set { fields["TextBlockGroupID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByTextBlockGroupID(int? textBlockGroupIDValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("TextBlockGroupID", textBlockGroupIDValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> TextBlockGroupID {
				get { return (ActiveField<int?>)fields["TextBlockGroupID"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByTextBlockGroupID(int? textBlockGroupIDValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where TextBlockGroupID=", Sql.Sqlize(textBlockGroupIDValue));
			return TextBlockList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: TextBlockGroup
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class TextBlock {
		[NonSerialized]		
		private TextBlockGroup _TextBlockGroup;

		[JetBrains.Annotations.CanBeNull]
		public TextBlockGroup TextBlockGroup
		{
			get
			{
				 // lazy load
				if (this._TextBlockGroup == null && this.TextBlockGroupID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("TextBlockGroup") && container.PrefetchCounter["TextBlockGroup"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.TextBlockGroup>("TextBlockGroupID",container.Select(r=>r.TextBlockGroupID).ToList(),"TextBlockGroup",Otherwise.Null);
					}
					this._TextBlockGroup = Models.TextBlockGroup.LoadByTextBlockGroupID(TextBlockGroupID.Value);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("TextBlockGroup")) {
							container.PrefetchCounter["TextBlockGroup"] = 0;
						}
						container.PrefetchCounter["TextBlockGroup"]++;
					}
				}
				return this._TextBlockGroup;
			}
			set
			{
				this._TextBlockGroup = value;
			}
		}
	}

	public partial class TextBlockList {
		internal int numFetchesOfTextBlockGroup = 0;
	}
	
	// define list in partial foreign table class 
	public partial class TextBlockGroup {
		[NonSerialized]		
		private TextBlockList _TextBlocks;
		
		[JetBrains.Annotations.NotNull]
		public TextBlockList TextBlocks
		{
			get
			{
				// lazy load
				if (this._TextBlocks == null) {
					this._TextBlocks = Models.TextBlockList.LoadByTextBlockGroupID(this.ID);
					this._TextBlocks.SetParentBindField(this, "TextBlockGroupID");
				}
				return this._TextBlocks;
			}
			set
			{
				this._TextBlocks = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("DateModified", dateModifiedValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaDescription
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaDescription {
			get { return Fields.MetaDescription.Value; }
			set { fields["MetaDescription"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByMetaDescription(string metaDescriptionValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("MetaDescription", metaDescriptionValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaDescription {
				get { return (ActiveField<string>)fields["MetaDescription"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByMetaDescription(string metaDescriptionValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where MetaDescription=", Sql.Sqlize(metaDescriptionValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MetaKeywords
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MetaKeywords {
			get { return Fields.MetaKeywords.Value; }
			set { fields["MetaKeywords"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByMetaKeywords(string metaKeywordsValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("MetaKeywords", metaKeywordsValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MetaKeywords {
				get { return (ActiveField<string>)fields["MetaKeywords"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByMetaKeywords(string metaKeywordsValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where MetaKeywords=", Sql.Sqlize(metaKeywordsValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PageTitleTag
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string PageTitleTag {
			get { return Fields.PageTitleTag.Value; }
			set { fields["PageTitleTag"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByPageTitleTag(string pageTitleTagValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("PageTitleTag", pageTitleTagValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> PageTitleTag {
				get { return (ActiveField<string>)fields["PageTitleTag"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByPageTitleTag(string pageTitleTagValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where PageTitleTag=", Sql.Sqlize(pageTitleTagValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("DateAdded", dateAddedValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: HasMailMergefields
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool HasMailMergefields {
			get { return Fields.HasMailMergefields.Value; }
			set { fields["HasMailMergefields"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByHasMailMergefields(bool hasMailMergefieldsValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("HasMailMergefields", hasMailMergefieldsValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> HasMailMergefields {
				get { return (ActiveField<bool>)fields["HasMailMergefields"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByHasMailMergefields(bool hasMailMergefieldsValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where HasMailMergefields=", Sql.Sqlize(hasMailMergefieldsValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: MergefieldHelp
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string MergefieldHelp {
			get { return Fields.MergefieldHelp.Value; }
			set { fields["MergefieldHelp"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByMergefieldHelp(string mergefieldHelpValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("MergefieldHelp", mergefieldHelpValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> MergefieldHelp {
				get { return (ActiveField<string>)fields["MergefieldHelp"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByMergefieldHelp(string mergefieldHelpValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where MergefieldHelp=", Sql.Sqlize(mergefieldHelpValue));
			return TextBlockList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: AdminNotes
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlock {		
				
		[JetBrains.Annotations.CanBeNull]
		public string AdminNotes {
			get { return Fields.AdminNotes.Value; }
			set { fields["AdminNotes"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlock LoadByAdminNotes(string adminNotesValue) {
			return ActiveRecordLoader.LoadByField<TextBlock>("AdminNotes", adminNotesValue, "TextBlock", Otherwise.Null);
		}

		public partial class TextBlockFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> AdminNotes {
				get { return (ActiveField<string>)fields["AdminNotes"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockList LoadByAdminNotes(string adminNotesValue) {
			var sql = new Sql("select * from ","TextBlock".SqlizeName()," where AdminNotes=", Sql.Sqlize(adminNotesValue));
			return TextBlockList.Load(sql);
		}		
		
	}


}
#endregion