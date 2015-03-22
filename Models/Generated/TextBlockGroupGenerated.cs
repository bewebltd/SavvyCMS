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
// CLASS: TextBlockGroup
// TABLE: TextBlockGroup
//-----------------------------------------


	public partial class TextBlockGroup : ActiveRecord {

		/// <summary>
		/// The list that this TextBlockGroup is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<TextBlockGroup> GetContainingList() {
			return (ActiveRecordList<TextBlockGroup>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public TextBlockGroup(): base("TextBlockGroup", "TextBlockGroupID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "TextBlockGroup";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "TextBlockGroupID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property TextBlockGroupID.
		/// </summary>
		public int ID { get { return (int)fields["TextBlockGroupID"].ValueObject; } set { fields["TextBlockGroupID"].ValueObject = value; } }

		// field references
		public partial class TextBlockGroupFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public TextBlockGroupFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private TextBlockGroupFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public TextBlockGroupFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new TextBlockGroupFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the TextBlockGroup table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of TextBlockGroup</param>
		/// <returns>An instance of TextBlockGroup containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static TextBlockGroup LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the TextBlockGroup table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg TextBlockGroup.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = TextBlockGroup.LoadID(55);</example>
		/// <param name="id">Primary key of TextBlockGroup</param>
		/// <returns>An instance of TextBlockGroup containing the data in the record</returns>
		public static TextBlockGroup LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			TextBlockGroup record = null;
//			record = TextBlockGroup.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where TextBlockGroupID=", Sql.Sqlize(id));
//				record = new TextBlockGroup();
//				if (!record.LoadData(sql)) return otherwise.Execute<TextBlockGroup>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<TextBlockGroup>(id, "TextBlockGroup", otherwise);
		}

		/// <summary>
		/// Loads a record from the TextBlockGroup table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of TextBlockGroup containing the data in the record</returns>
		public static TextBlockGroup Load(Sql sql) {
				return ActiveRecordLoader.Load<TextBlockGroup>(sql, "TextBlockGroup");
		}
		public static TextBlockGroup Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<TextBlockGroup>(sql, "TextBlockGroup", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the TextBlockGroup table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of TextBlockGroup containing the data in the record</returns>
		public static TextBlockGroup Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<TextBlockGroup>(reader, "TextBlockGroup");
		}
		public static TextBlockGroup Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<TextBlockGroup>(reader, "TextBlockGroup", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where TextBlockGroupID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("TextBlockGroupID", new ActiveField<int>() { Name = "TextBlockGroupID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="TextBlockGroup"  });

	fields.Add("GroupName", new ActiveField<string>() { Name = "GroupName", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="TextBlockGroup"  });

	fields.Add("SortPosition", new ActiveField<int?>() { Name = "SortPosition", ColumnType = "int", Type = typeof(int?), IsAuto = false, MaxLength=4, TableName="TextBlockGroup"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="TextBlockGroup"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="TextBlockGroup"  });
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
				var rec = ActiveRecordLoader.LoadID<TextBlockGroup>(id, "TextBlockGroup", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the TextBlockGroup with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct TextBlockGroup or null if not in cache.</returns>
//		private static TextBlockGroup GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-TextBlockGroup-" + id) as TextBlockGroup;
//			return Web.PageGlobals["ActiveRecord-TextBlockGroup-" + id] as TextBlockGroup;
//		}

		/// <summary>
		/// Caches this TextBlockGroup object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-TextBlockGroup-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-TextBlockGroup-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-TextBlockGroup-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of TextBlockGroup objects/records. This is the usual data structure for holding a number of TextBlockGroup records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class TextBlockGroupList : ActiveRecordList<TextBlockGroup> {

		public TextBlockGroupList() : base() {}
		public TextBlockGroupList(List<TextBlockGroup> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-TextBlockGroup to TextBlockGroupList. 
		/// </summary>
		public static implicit operator TextBlockGroupList(List<TextBlockGroup> list) {
			return new TextBlockGroupList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from TextBlockGroupList to List-of-TextBlockGroup. 
		/// </summary>
		public static implicit operator List<TextBlockGroup>(TextBlockGroupList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of TextBlockGroup objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of TextBlockGroup records.</returns>
		public static TextBlockGroupList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where TextBlockGroupID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of TextBlockGroup objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of TextBlockGroup records.</returns>
		public static TextBlockGroupList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static TextBlockGroupList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where TextBlockGroupID in (", ids.SqlizeNumberList(), ")");
			var result = new TextBlockGroupList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of TextBlockGroup objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of TextBlockGroup records.</returns>
		public static TextBlockGroupList Load(Sql sql) {
			var result = new TextBlockGroupList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all TextBlockGroup objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and TextBlockGroupID desc.)
		/// </summary>
		public static TextBlockGroupList LoadAll() {
			var result = new TextBlockGroupList();
			result.LoadRecords(null);
			return result;
		}
		public static TextBlockGroupList LoadAll(int itemsPerPage) {
			var result = new TextBlockGroupList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static TextBlockGroupList LoadAll(int itemsPerPage, int pageNum) {
			var result = new TextBlockGroupList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" TextBlockGroup objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static TextBlockGroupList LoadActive() {
			var result = new TextBlockGroupList();
			var sql = (new TextBlockGroup()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static TextBlockGroupList LoadActive(int itemsPerPage) {
			var result = new TextBlockGroupList();
			var sql = (new TextBlockGroup()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static TextBlockGroupList LoadActive(int itemsPerPage, int pageNum) {
			var result = new TextBlockGroupList();
			var sql = (new TextBlockGroup()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static TextBlockGroupList LoadActivePlusExisting(object existingRecordID) {
			var result = new TextBlockGroupList();
			var sql = (new TextBlockGroup()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM TextBlockGroup");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM TextBlockGroup");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new TextBlockGroup()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = TextBlockGroup.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: TextBlockGroupID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlockGroup {		
				
		[JetBrains.Annotations.CanBeNull]
		public int TextBlockGroupID {
			get { return Fields.TextBlockGroupID.Value; }
			set { fields["TextBlockGroupID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlockGroup LoadByTextBlockGroupID(int textBlockGroupIDValue) {
			return ActiveRecordLoader.LoadByField<TextBlockGroup>("TextBlockGroupID", textBlockGroupIDValue, "TextBlockGroup", Otherwise.Null);
		}

		public partial class TextBlockGroupFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> TextBlockGroupID {
				get { return (ActiveField<int>)fields["TextBlockGroupID"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockGroupList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockGroupList LoadByTextBlockGroupID(int textBlockGroupIDValue) {
			var sql = new Sql("select * from ","TextBlockGroup".SqlizeName()," where TextBlockGroupID=", Sql.Sqlize(textBlockGroupIDValue));
			return TextBlockGroupList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: GroupName
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlockGroup {		
				
		[JetBrains.Annotations.CanBeNull]
		public string GroupName {
			get { return Fields.GroupName.Value; }
			set { fields["GroupName"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlockGroup LoadByGroupName(string groupNameValue) {
			return ActiveRecordLoader.LoadByField<TextBlockGroup>("GroupName", groupNameValue, "TextBlockGroup", Otherwise.Null);
		}

		public partial class TextBlockGroupFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> GroupName {
				get { return (ActiveField<string>)fields["GroupName"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockGroupList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockGroupList LoadByGroupName(string groupNameValue) {
			var sql = new Sql("select * from ","TextBlockGroup".SqlizeName()," where GroupName=", Sql.Sqlize(groupNameValue));
			return TextBlockGroupList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: SortPosition
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlockGroup {		
				
		[JetBrains.Annotations.CanBeNull]
		public int? SortPosition {
			get { return Fields.SortPosition.Value; }
			set { fields["SortPosition"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlockGroup LoadBySortPosition(int? sortPositionValue) {
			return ActiveRecordLoader.LoadByField<TextBlockGroup>("SortPosition", sortPositionValue, "TextBlockGroup", Otherwise.Null);
		}

		public partial class TextBlockGroupFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int?> SortPosition {
				get { return (ActiveField<int?>)fields["SortPosition"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockGroupList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockGroupList LoadBySortPosition(int? sortPositionValue) {
			var sql = new Sql("select * from ","TextBlockGroup".SqlizeName()," where SortPosition=", Sql.Sqlize(sortPositionValue));
			return TextBlockGroupList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlockGroup {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlockGroup LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<TextBlockGroup>("DateAdded", dateAddedValue, "TextBlockGroup", Otherwise.Null);
		}

		public partial class TextBlockGroupFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockGroupList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockGroupList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","TextBlockGroup".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return TextBlockGroupList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class TextBlockGroup {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static TextBlockGroup LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<TextBlockGroup>("DateModified", dateModifiedValue, "TextBlockGroup", Otherwise.Null);
		}

		public partial class TextBlockGroupFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class TextBlockGroupList {		
				
		[JetBrains.Annotations.NotNull]
		public static TextBlockGroupList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","TextBlockGroup".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return TextBlockGroupList.Load(sql);
		}		
		
	}


}
#endregion