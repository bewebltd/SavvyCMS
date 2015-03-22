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
// CLASS: Competition
// TABLE: Competition
//-----------------------------------------


	public partial class Competition : ActiveRecord {

		/// <summary>
		/// The list that this Competition is a member of (ie was loaded with) - or null if it is not a member of a list.
		/// </summary>
		protected ActiveRecordList<Competition> GetContainingList() {
			return (ActiveRecordList<Competition>)this.containingList;
		}

		/// <summary>
		/// Constructor - create new - initialise defaults
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Competition(): base("Competition", "CompetitionID") {
			SetupFieldMetaData();
			InitDefaults();
		}

		/// <summary>
		/// Return the database table name for this Active Record.
		/// </summary>
		/// <returns></returns>
		public override string GetTableName() {
			return "Competition";
		}

		/// <summary>
		/// Return the database primary key name for this Active Record. This is also referred to as the ID.
		/// </summary>
		/// <returns></returns>
		public override string GetPrimaryKeyName() {
			return "CompetitionID";
		}

		/// <summary>
		/// Return true if this is actually a database VIEW not a TABLE.
		/// </summary>
		/// <returns></returns>
		public override bool GetIsView() {
			return false;
		}

		/// <summary>
		/// The primary key of the record. This is simply a shortcut for the property CompetitionID.
		/// </summary>
		public int ID { get { return (int)fields["CompetitionID"].ValueObject; } set { fields["CompetitionID"].ValueObject = value; } }

		// field references
		public partial class CompetitionFieldReferences {
			Dictionary<string, ActiveFieldBase> fields;
			public CompetitionFieldReferences(Dictionary<string, ActiveFieldBase> fields) {
				this.fields = fields;
			}
		}
		private CompetitionFieldReferences _Fields = null;

		/// <summary>
		/// Method for accessing a reference to a field (eg for passing to Beweb.Form.TextField) or meta-data about the field (eg max length).
		/// </summary>
		public CompetitionFieldReferences Fields {
			get {
				if (_Fields==null) {
					_Fields = new CompetitionFieldReferences(fields);
					CheckFieldsCollectionIsPopulated();
				}
				return _Fields;
			}
		}


		/// <summary>
		/// Loads a record from the Competition table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, null is returned, or you can specify the behaviour with the second parameter 'otherwise'.
		/// </summary>
		/// <example>var g = GenTest.LoadID(55);</example>
		/// <param name="id">Primary key of Competition</param>
		/// <returns>An instance of Competition containing the data in the record</returns>
		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadID(int id) {
			return LoadID(id, Otherwise.Null);
		}

		/// <summary>
		/// Loads a record from the Competition table in the database given the supplied primary key value.
		/// If found in the cache, record will be loaded from the cache.
		/// If NOT FOUND, you can specify the behaviour with Beweb.Otherwise - eg Competition.LoadID(55, Otherwise.Null) will return null if not found.
		/// </summary>
		/// <example>var g = Competition.LoadID(55);</example>
		/// <param name="id">Primary key of Competition</param>
		/// <returns>An instance of Competition containing the data in the record</returns>
		public static Competition LoadID(int id, Otherwise otherwise) {
			// see if already in cache, else create sql and load
//			Competition record = null;
//			record = Competition.GetFromCache(id); //comment out if not int pk type
//			if (record == null)
//			{
//				var sql = new Sql("where CompetitionID=", Sql.Sqlize(id));
//				record = new Competition();
//				if (!record.LoadData(sql)) return otherwise.Execute<Competition>();
//				record.StoreInCache();
//			}
//			return record;
				return ActiveRecordLoader.LoadID<Competition>(id, "Competition", otherwise);
		}

		/// <summary>
		/// Loads a record from the Competition table in the database given the supplied Sql command object.
		/// You can use just a WHERE clause as your Sql - see example.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>
		/// var sql = new Sql("where name=", Web.Request["name"].SqlizeText());
		/// var g = GenTest.Load(sql);
		/// </example>
		/// <param name="sql"></param>
		/// <returns>An instance of Competition containing the data in the record</returns>
		public static Competition Load(Sql sql) {
				return ActiveRecordLoader.Load<Competition>(sql, "Competition");
		}
		public static Competition Load(Sql sql, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Competition>(sql, "Competition", otherwise);
		}

		/// <summary>
		/// Loads a record given a DataReader containing data from the Competition table in the database.
		/// You should find that Load(Sql) or LoadID(id) cover most scenarios but you can use this method if you need to.
		/// When using a DataReader be careful to ensure you close it using reader.Close() to avoid connection leaks.
		/// If found in the cache, record will be loaded from the cache.
		/// </summary>
		/// <example>var g = GenTest.Load(reader);</example>
		/// <param name="sql"></param>
		/// <returns>An instance of Competition containing the data in the record</returns>
		public static Competition Load(DbDataReader reader) {
				return ActiveRecordLoader.Load<Competition>(reader, "Competition");
		}
		public static Competition Load(DbDataReader reader, Otherwise otherwise) {
				return ActiveRecordLoader.Load<Competition>(reader, "Competition", otherwise);
		}

		/// <summary>
		/// Forces a reload of the data in this object from the database. This can be used to revert any changes.
		/// It can also be use to get fresh data when the underlying database may have been updated (but in most cases the cache should handle this automatically).
		/// Unlike the Load() methods this skips the cache lookup and goes straight to the database.
		/// </summary>
		public void ReloadFromDatabase() {
			this.RemoveFromCache();
			var sql = new Sql("where CompetitionID=", Sql.Sqlize(this.ID));
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
			
	fields.Add("CompetitionID", new ActiveField<int>() { Name = "CompetitionID", ColumnType = "int", Type = typeof(int), IsAuto = true, MaxLength=4, TableName="Competition"  });

	fields.Add("Title", new ActiveField<string>() { Name = "Title", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=50, TableName="Competition"  });

	fields.Add("IntroTextHtml", new ActiveField<string>() { Name = "IntroTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Competition"  });

	fields.Add("Picture", new PictureActiveField() { Name = "Picture", ColumnType = "nvarchar", Type = typeof(string), IsAuto = false, MaxLength=100, TableName="Competition"  });

	fields.Add("PublishDate", new ActiveField<System.DateTime?>() { Name = "PublishDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Competition"  });

	fields.Add("ExpiryDate", new ActiveField<System.DateTime?>() { Name = "ExpiryDate", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Competition"  });

	fields.Add("CompetitionClosedTextHtml", new ActiveField<string>() { Name = "CompetitionClosedTextHtml", ColumnType = "ntext", Type = typeof(string), IsAuto = false, MaxLength=1073741823, TableName="Competition"  });

	fields.Add("DateAdded", new ActiveField<System.DateTime?>() { Name = "DateAdded", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Competition"  });

	fields.Add("DateModified", new ActiveField<System.DateTime?>() { Name = "DateModified", ColumnType = "datetime", Type = typeof(System.DateTime?), IsAuto = false, MaxLength=8, TableName="Competition"  });
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
				var rec = ActiveRecordLoader.LoadID<Competition>(id, "Competition", Otherwise.Null);
				if (rec!=null) {
					return rec.GetName();
				}
			}
			return null;
		}

		/// <summary>
		/// Looks in the cache to see if we have already loaded the Competition with the specified primary key value.
		/// Returns null if not found.
		/// </summary>
		/// <param name="id">The ID or primary key value</param>
		/// <returns>Returns the correct Competition or null if not in cache.</returns>
//		private static Competition GetFromCache(int id) {
//			//return Web.Cache.Get("ActiveRecord-Competition-" + id) as Competition;
//			return Web.PageGlobals["ActiveRecord-Competition-" + id] as Competition;
//		}

		/// <summary>
		/// Caches this Competition object so it can be retrieved next time it is needed without going to the database.
		/// This must be done after loading, saving or setting the primary key value.
		/// </summary>
//		protected override void StoreInCache() {
//			//Web.Cache.Insert("ActiveRecord-Competition-" + this.ID, this);
//			if (GetFromCache(this.ID) == null) {Web.PageGlobals.Add("ActiveRecord-Competition-" + this.ID, this);}		 // this line commented out by the generator if there is a non-int pk
//		}

		/// <summary>
		/// Unloads this object from the cache.
		/// This must be done before deleting or changing the primary key value.
		/// </summary>
//		public override void RemoveFromCache() {
//			Web.PageGlobals.Remove("ActiveRecord-Competition-" + this.ID);
//		}
	}


	/// <summary>
	/// A recordset-style list of Competition objects/records. This is the usual data structure for holding a number of Competition records.
	/// It allows loading, lazy loading with prefetching of frequently accessed related records and saving.
	/// Support for paging will be added soon.
	/// You can sort, filter, loop, add, remove.
	/// </summary>
	public partial class CompetitionList : ActiveRecordList<Competition> {

		public CompetitionList() : base() {}
		public CompetitionList(List<Competition> list) : base(list) {}

		/// <summary>
		/// Implicit conversion (ie automatic) from List-of-Competition to CompetitionList. 
		/// </summary>
		public static implicit operator CompetitionList(List<Competition> list) {
			return new CompetitionList(list);
		}

		/// <summary>
		/// Implicit conversion (ie automatic) from CompetitionList to List-of-Competition. 
		/// </summary>
		public static implicit operator List<Competition>(CompetitionList list) {
			return list.innerList;
		}

		/// <summary>
		/// Returns a record list of Competition objects, given a List<int> of IDs (primary key values of records to load).
		/// </summary>
		/// <param name="ids">List of IDs</param>
		/// <returns>A list of Competition records.</returns>
		public static CompetitionList LoadIDs(IEnumerable<int> ids) {
			var sql = new Sql("where CompetitionID in (", ids.ToArray(), ")");
			return Load(sql);
		}
		// Note: LoadIDs merged with above, extra method removed.
		/// <summary>
		// TODO: mike, remove string version if string primary key (e.g. not a number)
		/// Returns a record list of Competition objects, given an array of IDs as strings (primary key values of records to load).
		/// </summary>
		/// <param name="ids">Array of IDs</param>
		/// <returns>A list of Competition records.</returns>
		public static CompetitionList LoadIDs(string[] ids) {		
			return LoadIDs(ids,true);
		}

		public static CompetitionList LoadIDs(string[] ids, bool useDefaultOrderBy) {
			var sql = new Sql("where CompetitionID in (", ids.SqlizeNumberList(), ")");
			var result = new CompetitionList();
			result.LoadRecords(sql,useDefaultOrderBy);
			return result;
		}

		/// <summary>
		/// Returns a record list of Competition objects, given a SQL statement.
		/// </summary>
		/// <param name="sql">A SQL statement constructed using the Beweb.Sql class</param>
		/// <returns>A list of Competition records.</returns>
		public static CompetitionList Load(Sql sql) {
			var result = new CompetitionList();
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all Competition objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all records; LoadActive(10) = returns top 10 records by default sort order; LoadActive(10, 2) = returns second page of 10 records by default sort order
		/// ("Default sort order" can be defined for each record type by overriding in the partial model file. By default it looks for fields: SortPosition,SortOrder,Position, DateAdded,CreateDate,DateCreated and CompetitionID desc.)
		/// </summary>
		public static CompetitionList LoadAll() {
			var result = new CompetitionList();
			result.LoadRecords(null);
			return result;
		}
		public static CompetitionList LoadAll(int itemsPerPage) {
			var result = new CompetitionList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CompetitionList LoadAll(int itemsPerPage, int pageNum) {
			var result = new CompetitionList();
			var sql = new Sql("where 1=1");
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads and returns a record list of all the "active" Competition objects.
		/// Parameters: itemsPerPage = max records to fetch; pageNum = one based page number if multiple pages (otherwise itemsPerPage is effectively TOP)
		/// eg LoadActive() = returns all active records; LoadActive(10) = returns top 10 active records by default sort order; LoadActive(10, 2) = returns second page of 10 active records by default sort order
		/// ("Active" can be defined for each record type by overriding in the partial model file. By default it looks for fields: PublishDate/ExpiryDate, IsActive,IsEnabled,IsPublished,Active,Enabled,Published,IsVisible,Visible.)
		/// </summary>
		public static CompetitionList LoadActive() {
			var result = new CompetitionList();
			var sql = (new Competition()).GetSqlWhereActive();
			result.LoadRecords(sql);
			return result;
		}
		public static CompetitionList LoadActive(int itemsPerPage) {
			var result = new CompetitionList();
			var sql = (new Competition()).GetSqlWhereActive();
			sql.Paging(itemsPerPage);
			result.LoadRecords(sql);
			return result;
		}
		public static CompetitionList LoadActive(int itemsPerPage, int pageNum) {
			var result = new CompetitionList();
			var sql = (new Competition()).GetSqlWhereActive();
			sql.Paging(itemsPerPage, pageNum);
			result.LoadRecords(sql);
			return result;
		}
		/// <summary>
		/// Loads all records which are active plus the one with the given ID. For situations where you want to only show active records except you also want to include the record that is already current.
		/// </summary>
		public static CompetitionList LoadActivePlusExisting(object existingRecordID) {
			var result = new CompetitionList();
			var sql = (new Competition()).GetSqlWhereActivePlusExisting(existingRecordID);
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
				sql = new Sql("SELECT * FROM Competition");
			}
			if (sql.Value.StartsWith("where ")) {
				sql.Prepend("SELECT * FROM Competition");
			}
			if (!sql.Value.ToLower().Contains("order by")) {
				sql.Add((new Competition()).GetDefaultOrderBy());
			}
			var reader = sql.GetReader();
			int rec = 1;
			while (reader.Read()) {
				var record = Competition.Load(reader);
				this.Add(record);
				rec++;
				if (sql.EnablePaging && rec > sql.ItemsPerPage) break;
			}
			reader.Close();
		}
		*/


	}


	//-------------------------------------------------------------------
	// PROPERTY: CompetitionID
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public int CompetitionID {
			get { return Fields.CompetitionID.Value; }
			set { fields["CompetitionID"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByCompetitionID(int competitionIDValue) {
			return ActiveRecordLoader.LoadByField<Competition>("CompetitionID", competitionIDValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<int> CompetitionID {
				get { return (ActiveField<int>)fields["CompetitionID"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByCompetitionID(int competitionIDValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where CompetitionID=", Sql.Sqlize(competitionIDValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Title
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Title {
			get { return Fields.Title.Value; }
			set { fields["Title"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByTitle(string titleValue) {
			return ActiveRecordLoader.LoadByField<Competition>("Title", titleValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> Title {
				get { return (ActiveField<string>)fields["Title"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByTitle(string titleValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where Title=", Sql.Sqlize(titleValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: IntroTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public string IntroTextHtml {
			get { return Fields.IntroTextHtml.Value; }
			set { fields["IntroTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByIntroTextHtml(string introTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<Competition>("IntroTextHtml", introTextHtmlValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> IntroTextHtml {
				get { return (ActiveField<string>)fields["IntroTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByIntroTextHtml(string introTextHtmlValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where IntroTextHtml=", Sql.Sqlize(introTextHtmlValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: Picture
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public string Picture {
			get { return Fields.Picture.Value; }
			set { fields["Picture"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByPicture(string pictureValue) {
			return ActiveRecordLoader.LoadByField<Competition>("Picture", pictureValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public PictureActiveField Picture {
				get { return (PictureActiveField)fields["Picture"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByPicture(string pictureValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where Picture=", Sql.Sqlize(pictureValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: PublishDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? PublishDate {
			get { return Fields.PublishDate.Value; }
			set { fields["PublishDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByPublishDate(System.DateTime? publishDateValue) {
			return ActiveRecordLoader.LoadByField<Competition>("PublishDate", publishDateValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> PublishDate {
				get { return (ActiveField<System.DateTime?>)fields["PublishDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByPublishDate(System.DateTime? publishDateValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where PublishDate=", Sql.Sqlize(publishDateValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: ExpiryDate
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? ExpiryDate {
			get { return Fields.ExpiryDate.Value; }
			set { fields["ExpiryDate"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByExpiryDate(System.DateTime? expiryDateValue) {
			return ActiveRecordLoader.LoadByField<Competition>("ExpiryDate", expiryDateValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> ExpiryDate {
				get { return (ActiveField<System.DateTime?>)fields["ExpiryDate"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByExpiryDate(System.DateTime? expiryDateValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where ExpiryDate=", Sql.Sqlize(expiryDateValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: CompetitionClosedTextHtml
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public string CompetitionClosedTextHtml {
			get { return Fields.CompetitionClosedTextHtml.Value; }
			set { fields["CompetitionClosedTextHtml"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByCompetitionClosedTextHtml(string competitionClosedTextHtmlValue) {
			return ActiveRecordLoader.LoadByField<Competition>("CompetitionClosedTextHtml", competitionClosedTextHtmlValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<string> CompetitionClosedTextHtml {
				get { return (ActiveField<string>)fields["CompetitionClosedTextHtml"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByCompetitionClosedTextHtml(string competitionClosedTextHtmlValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where CompetitionClosedTextHtml=", Sql.Sqlize(competitionClosedTextHtmlValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateAdded
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateAdded {
			get { return Fields.DateAdded.Value; }
			set { fields["DateAdded"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByDateAdded(System.DateTime? dateAddedValue) {
			return ActiveRecordLoader.LoadByField<Competition>("DateAdded", dateAddedValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateAdded {
				get { return (ActiveField<System.DateTime?>)fields["DateAdded"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByDateAdded(System.DateTime? dateAddedValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where DateAdded=", Sql.Sqlize(dateAddedValue));
			return CompetitionList.Load(sql);
		}		
		
	}


	//-------------------------------------------------------------------
	// PROPERTY: DateModified
	//-------------------------------------------------------------------
	
	// define column related members in partial table class 
	public partial class Competition {		
				
		[JetBrains.Annotations.CanBeNull]
		public System.DateTime? DateModified {
			get { return Fields.DateModified.Value; }
			set { fields["DateModified"].ValueObject = value; } 
		}

		[JetBrains.Annotations.CanBeNull]
		public static Competition LoadByDateModified(System.DateTime? dateModifiedValue) {
			return ActiveRecordLoader.LoadByField<Competition>("DateModified", dateModifiedValue, "Competition", Otherwise.Null);
		}

		public partial class CompetitionFieldReferences {
			[JetBrains.Annotations.NotNull]
			public ActiveField<System.DateTime?> DateModified {
				get { return (ActiveField<System.DateTime?>)fields["DateModified"]; }
			}
		}

	}
	
	// define list class 
	public partial class CompetitionList {		
				
		[JetBrains.Annotations.NotNull]
		public static CompetitionList LoadByDateModified(System.DateTime? dateModifiedValue) {
			var sql = new Sql("select * from ","Competition".SqlizeName()," where DateModified=", Sql.Sqlize(dateModifiedValue));
			return CompetitionList.Load(sql);
		}		
		
	}


}
#endregion