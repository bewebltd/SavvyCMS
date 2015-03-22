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
// CLASS: FAQSection
// TABLE: FAQSection
//-----------------------------------------


	public partial class FAQSection : ActiveRecord {

		/// <summary>
		/// The list that this FAQSection is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<FAQSection> GetContainingList() {
			return (ActiveRecordList<FAQSection>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public FAQSection(): base("FAQSection", "FAQSectionID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "FAQSection";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "FAQSectionID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property FAQSectionID.
		/// </summary>
		public int ID { get { return (int)fields["FAQSectionID"].ValueObject; } set { fields["FAQSectionID"].ValueObject = value; } }

		// field references
		public partial class FAQSectionFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public FAQSectionFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private FAQSectionFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public FAQSectionFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new FAQSectionFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the FAQSection table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of FAQSection</param>
		/// <returns>An instance of FAQSection containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the FAQSection table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg FAQSection.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = FAQSection.LoadID(55);</example>
		/// <param name="id">Primary key of FAQSection</param>
		/// <returns>An instance of FAQSection containing the data in the record</returns>
		public static FAQSection LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			FAQSection record = null;
//			record = FAQSection.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where FAQSectionID=", Sql.Sqlize(id));
//				record = new FAQSection();
//				if (!record.LoadData(sql)) return otherwise.Execute<FAQSection>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<FAQSection>(id, "FAQSection", otherwise);
		}

		/// <summary>
		/// Loads a record from the FAQSection table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of FAQSection containing the data in the record</returns>
		public static FAQSection Load(Sql sql) {
				return ActiveRecordLoader.Load<FAQSection>(sql, "FAQSection");
		}
		public static FAQSection Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<FAQSection>(sql, "FAQSection", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the FAQSection table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of FAQSection containing the data in the record</returns>
		public static FAQSection Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<FAQSection>(reader, "FAQSection");
		}
		public static FAQSection Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<FAQSection>(reader, "FAQSection", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where FAQSectionID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("FAQSectionID", new ActiveField<int>() { Name = "FAQSectionID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="FAQSection"  });

	fields.Add("SectionName", new ActiveField<string>() { Name = "SectionName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="FAQSection"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="FAQSection"  });

	fields.Add("IsPublished", new ActiveField<bool>() { Name = "IsPublished", ColumnType = "bit", Type = typeof(bool), IsAuto = false, MaxLength=1, TableName="FAQSection"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="FAQSection"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="FAQSection"  });
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
				var rec = ActiveRecordLoader.LoadID<FAQSection>(id, "FAQSection", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the FAQSection with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct FAQSection or null if not in cache.</returns>
//		private static FAQSection GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-FAQSection-" + id) as FAQSection;
//			return Web.PageGlobals["ActiveRecord-FAQSection-" + id] as FAQSection;
//		}

		/// <summary>
		/// Caches this FAQSection object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-FAQSection-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-FAQSection-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-FAQSection-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of FAQSection objects/records. This is the usual data structure for holding a number of FAQSection records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class FAQSectionList : ActiveRecordList<FAQSection> {

		public FAQSectionList() : base() {}
		public FAQSectionList(List<FAQSection> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-FAQSection to FAQSectionList. 
		/// </summary>
		public static implicit operator FAQSectionList(List<FAQSection> list) {
			return new FAQSectionList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from FAQSectionList to List-of-FAQSection. 
		/// </summary>
		public static implicit operator List<FAQSection>(FAQSectionList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of FAQSection objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of FAQSection records.</returns>
		public static FAQSectionList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where FAQSectionID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of FAQSection objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of FAQSection records.</returns>
		public static FAQSectionList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static FAQSectionList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where FAQSectionID in (", ids.SqlizeNumberList(), ")");
			var result = new FAQSectionList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of FAQSection objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of FAQSection records.</returns>
		public static FAQSectionList Load(Sql sql) {
			var result = new FAQSectionList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all FAQSection objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and FAQSectionID desc.)
		/// </summary>
		public static FAQSectionList LoadAll() {
			var result = new FAQSectionList();
			result.LoadRecords(null);
			return result;
		}
		public static FAQSectionList LoadAll(int itemsPerPage) {
			var result = new FAQSectionList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static FAQSectionList LoadAll(int itemsPerPage, int pageNum) {
			var result = new FAQSectionList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" FAQSection objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static FAQSectionList LoadActive() {
			var result = new FAQSectionList();
			var sql = (new FAQSection()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static FAQSectionList LoadActive(int itemsPerPage) {
			var result = new FAQSectionList();
			var sql = (new FAQSection()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static FAQSectionList LoadActive(int itemsPerPage, int pageNum) {
			var result = new FAQSectionList();
			var sql = (new FAQSection()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static FAQSectionList LoadActivePlusExisting(object existingRecordID) {
			var result = new FAQSectionList();
			var sql = (new FAQSection()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM FAQSection");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM FAQSection");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new FAQSection()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = FAQSection.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: FAQSectionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQSection {		
				
		[JetBrains.Annotations.CanBeNull]
		public int FAQSectionID {
			get { return Fields.FAQSectionID.Value; }
			set { fields["FAQSectionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadByFAQSectionID(int fAQSectionIDValue) {
			return ActiveRecordLoader.LoadByField<FAQSection>("FAQSectionID", fAQSectionIDValue, "FAQSection", Otherwise.Null);
		}

		public partial class FAQSectionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> FAQSectionID {
				get { return (ActiveField<int>)fields["FAQSectionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQSectionList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQSectionList LoadByFAQSectionID(int fAQSectionIDValue) {
			var sql = new Sql("select * from ","FAQSection".SqlizeName()," where FAQSectionID=", Sql.Sqlize(fAQSectionIDValue));
			return FAQSectionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SectionName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQSection {		
				
		[JetBrains.Annotations.CanBeNull]
		public string SectionName {
			get { return Fields.SectionName.Value; }
			set { fields["SectionName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadBySectionName(string sectionNameValue) {
			return ActiveRecordLoader.LoadByField<FAQSection>("SectionName", sectionNameValue, "FAQSection", Otherwise.Null);
		}

		public partial class FAQSectionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> SectionName {
				get { return (ActiveField<string>)fields["SectionName"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQSectionList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQSectionList LoadBySectionName(string sectionNameValue) {
			var sql = new Sql("select * from ","FAQSection".SqlizeName()," where SectionName=", Sql.Sqlize(sectionNameValue));
			return FAQSectionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQSection {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<FAQSection>("SortPosition", sortPositionValue, "FAQSection", Otherwise.Null);
		}

		public partial class FAQSectionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQSectionList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQSectionList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","FAQSection".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return FAQSectionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IsPublished
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQSection {		
				
		[JetBrains.Annotations.CanBeNull]
		public bool IsPublished {
			get { return Fields.IsPublished.Value; }
			set { fields["IsPublished"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadByIsPublished(bool isPublishedValue) {
			return ActiveRecordLoader.LoadByField<FAQSection>("IsPublished", isPublishedValue, "FAQSection", Otherwise.Null);
		}

		public partial class FAQSectionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<bool> IsPublished {
				get { return (ActiveField<bool>)fields["IsPublished"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQSectionList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQSectionList LoadByIsPublished(bool isPublishedValue) {
			var sql = new Sql("select * from ","FAQSection".SqlizeName()," where IsPublished=", Sql.Sqlize(isPublishedValue));
			return FAQSectionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQSection {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<FAQSection>("DateAdded", dateAddedValue, "FAQSection", Otherwise.Null);
		}

		public partial class FAQSectionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQSectionList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQSectionList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","FAQSection".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return FAQSectionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class FAQSection {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static FAQSection LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<FAQSection>("DateModified", dateModifiedValue, "FAQSection", Otherwise.Null);
		}

		public partial class FAQSectionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class FAQSectionList {		
				
		[JetBrains.Annotations.NotNull]
		public static FAQSectionList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","FAQSection".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return FAQSectionList.Load(sql);
		}		
		
	}


}
#endregion