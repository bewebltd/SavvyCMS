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
// CLASS: FAQItem
// TABLE: FAQItem
//-----------------------------------------


	public partial class FAQItem : ActiveRecord {

		/// <summary>
		/// The list that this FAQItem is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<FAQItem> GetContainingList() {
			return (ActiveRecordList<FAQItem>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public FAQItem(): base("FAQItem", "FAQItemID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "FAQItem";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "FAQItemID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property FAQItemID.
		/// </summary>
		public int ID { get { return (int)fields["FAQItemID"].ValueObject; } set { fields["FAQItemID"].ValueObject = value; } }

		// field references
		public partial class FAQItemFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public FAQItemFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private FAQItemFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public FAQItemFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new FAQItemFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the FAQItem table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of FAQItem</param>
		/// <returns>An instance of FAQItem containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the FAQItem table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg FAQItem.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = FAQItem.LoadID(55);</example>
		/// <param name="id">Primary key of FAQItem</param>
		/// <returns>An instance of FAQItem containing the data in the record</returns>
		public static FAQItem LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			FAQItem record = null;
//			record = FAQItem.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where FAQItemID=", Sql.Sqlize(id));
//				record = new FAQItem();
//				if (!record.LoadData(sql)) return otherwise.Execute<FAQItem>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<FAQItem>(id, "FAQItem", otherwise);
		}

		/// <summary>
		/// Loads a record from the FAQItem table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of FAQItem containing the data in the record</returns>
		public static FAQItem Load(Sql sql) {
				return ActiveRecordLoader.Load<FAQItem>(sql, "FAQItem");
		}
		public static FAQItem Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<FAQItem>(sql, "FAQItem", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the FAQItem table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of FAQItem containing the data in the record</returns>
		public static FAQItem Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<FAQItem>(reader, "FAQItem");
		}
		public static FAQItem Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<FAQItem>(reader, "FAQItem", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where FAQItemID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("FAQItemID", new ActiveField<int>() { Name = "FAQItemID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="FAQItem"  });

	fields.Add("FAQSectionID", new ActiveField<int>() { Name = "FAQSectionID", ColumnType = "int", Type = typeof(int), IsAuto = false, MaxLength=4, TableName="FAQItem" , GetForeignRecord = () => this.FAQSection, ForeignClassName = typeof(Models.FAQSection), ForeignTableName = "FAQSection", ForeignTableFieldName = "FAQSectionID" });

	fields.Add("FAQTitle", new ActiveField<string>() { Name = "FAQTitle", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=250, TableName="FAQItem"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="FAQItem"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="FAQItem"  });

	fields.Add("BodyTextHTML", new ActiveField<string>() { Name = "BodyTextHTML", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="FAQItem"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="FAQItem"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="FAQItem"  });
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
				var rec = ActiveRecordLoader.LoadID<FAQItem>(id, "FAQItem", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the FAQItem with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct FAQItem or null if not in cache.</returns>
//		private static FAQItem GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-FAQItem-" + id) as FAQItem;
//			return Web.PageGlobals["ActiveRecord-FAQItem-" + id] as FAQItem;
//		}

		/// <summary>
		/// Caches this FAQItem object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-FAQItem-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-FAQItem-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-FAQItem-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of FAQItem objects/records. This is the usual data structure for holding a number of FAQItem records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class FAQItemList : ActiveRecordList<FAQItem> {

		public FAQItemList() : base() {}
		public FAQItemList(List<FAQItem> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-FAQItem to FAQItemList. 
		/// </summary>
		public static implicit operator FAQItemList(List<FAQItem> list) {
			return new FAQItemList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from FAQItemList to List-of-FAQItem. 
		/// </summary>
		public static implicit operator List<FAQItem>(FAQItemList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of FAQItem objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of FAQItem records.</returns>
		public static FAQItemList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where FAQItemID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of FAQItem objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of FAQItem records.</returns>
		public static FAQItemList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static FAQItemList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where FAQItemID in (", ids.SqlizeNumberList(), ")");
			var result = new FAQItemList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of FAQItem objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of FAQItem records.</returns>
		public static FAQItemList Load(Sql sql) {
			var result = new FAQItemList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all FAQItem objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and FAQItemID desc.)
		/// </summary>
		public static FAQItemList LoadAll() {
			var result = new FAQItemList();
			result.LoadRecords(null);
			return result;
		}
		public static FAQItemList LoadAll(int itemsPerPage) {
			var result = new FAQItemList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static FAQItemList LoadAll(int itemsPerPage, int pageNum) {
			var result = new FAQItemList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" FAQItem objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static FAQItemList LoadActive() {
			var result = new FAQItemList();
			var sql = (new FAQItem()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static FAQItemList LoadActive(int itemsPerPage) {
			var result = new FAQItemList();
			var sql = (new FAQItem()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static FAQItemList LoadActive(int itemsPerPage, int pageNum) {
			var result = new FAQItemList();
			var sql = (new FAQItem()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static FAQItemList LoadActivePlusExisting(object existingRecordID) {
			var result = new FAQItemList();
			var sql = (new FAQItem()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM FAQItem");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM FAQItem");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new FAQItem()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = FAQItem.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: FAQItemID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public int FAQItemID {
			get { return Fields.FAQItemID.Value; }
			set { fields["FAQItemID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByFAQItemID(int fAQItemIDValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("FAQItemID", fAQItemIDValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> FAQItemID {
				get { return (ActiveField<int>)fields["FAQItemID"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByFAQItemID(int fAQItemIDValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where FAQItemID=", Sql.Sqlize(fAQItemIDValue));
			return FAQItemList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: FAQSectionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public int FAQSectionID {
			get { return Fields.FAQSectionID.Value; }
			set { fields["FAQSectionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByFAQSectionID(int fAQSectionIDValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("FAQSectionID", fAQSectionIDValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> FAQSectionID {
				get { return (ActiveField<int>)fields["FAQSectionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByFAQSectionID(int fAQSectionIDValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where FAQSectionID=", Sql.Sqlize(fAQSectionIDValue));
			return FAQItemList.Load(sql);
		}		
		
	}


//-----------------------------------------
// FOREIGN KEY: FAQSection
//-----------------------------------------

	// define foreign key as object in partial table class 
	public partial class FAQItem {
		[NonSerialized]		
		private FAQSection _FAQSection;

		[JetBrains.Annotations.CanBeNull]
		public FAQSection FAQSection
		{
			get
			{
				 // lazy load
				if (this._FAQSection == null && this.FAQSectionID != null) {
					var container = GetContainingList();
					if (container != null && container.PrefetchCounter.ContainsKey("FAQSection") && container.PrefetchCounter["FAQSection"] == 2) {  // MN 20110726 - fixed bug - eager fetch should happen only once
						ActiveRecordLoader.LoadRecordsByKeyValues<Models.FAQSection>("FAQSectionID",container.Select(r=>r.FAQSectionID).ToList(),"FAQSection",Otherwise.Null);
					}
					this._FAQSection = Models.FAQSection.LoadByFAQSectionID(FAQSectionID);
					if (container != null) {
						if (!container.PrefetchCounter.ContainsKey("FAQSection")) {
							container.PrefetchCounter["FAQSection"] = 0;
						}
						container.PrefetchCounter["FAQSection"]++;
					}
				}
				return this._FAQSection;
			}
			set
			{
				this._FAQSection = value;
			}
		}
	}

	public partial class FAQItemList {
		internal int numFetchesOfFAQSection = 0;
	}
	
	// define list in partial foreign table class 
	public partial class FAQSection {
		[NonSerialized]		
		private FAQItemList _FAQItems;
		
		[JetBrains.Annotations.NotNull]
		public FAQItemList FAQItems
		{
			get
			{
				// lazy load
				if (this._FAQItems == null) {
					this._FAQItems = Models.FAQItemList.LoadByFAQSectionID(this.ID);
					this._FAQItems.SetParentBindField(this, "FAQSectionID");
				}
				return this._FAQItems;
			}
			set
			{
				this._FAQItems = value;
			}
		}
	}
	
	//-------------------------------------------------------------------
	// PROPERTY: FAQTitle
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public string FAQTitle {
			get { return Fields.FAQTitle.Value; }
			set { fields["FAQTitle"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByFAQTitle(string fAQTitleValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("FAQTitle", fAQTitleValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> FAQTitle {
				get { return (ActiveField<string>)fields["FAQTitle"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByFAQTitle(string fAQTitleValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where FAQTitle=", Sql.Sqlize(fAQTitleValue));
			return FAQItemList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("SortPosition", sortPositionValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return FAQItemList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("IsPublished", isPublishedValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return FAQItemList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: BodyTextHTML
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public string BodyTextHTML {
			get { return Fields.BodyTextHTML.Value; }
			set { fields["BodyTextHTML"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByBodyTextHTML(string bodyTextHTMLValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("BodyTextHTML", bodyTextHTMLValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> BodyTextHTML {
				get { return (ActiveField<string>)fields["BodyTextHTML"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByBodyTextHTML(string bodyTextHTMLValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where BodyTextHTML=", Sql.Sqlize(bodyTextHTMLValue));
			return FAQItemList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("DateAdded", dateAddedValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return FAQItemList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQItem {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQItem LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<FAQItem>("DateModified", dateModifiedValue, "FAQItem", Otherwise.Null);
		}

		public partial class FAQItemFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQItemList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQItemList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","FAQItem".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return FAQItemList.Load(sql);
		}		
		
	}


}
#endregion