using System;
using Beweb;

namespace Savvy
{

	/// <summary>
	/// Allow record Locking in admin while a user is editing a record
	/// </summary>
	public class Locking
	{
		public string LockMessage="";
		public int AdministratorID;
		private DataBlock db;
		public int LockNumMinutes=20;
		public bool IsLocked;
		public bool LockingEnabled = false;
	
		~Locking()
		{
			if(db!=null)db.CloseDB();
		}
		//public void InitLocking(DataBlock dbParam, int adminID)
		public void InitLocking(int adminID)
		{
			//db = dbParam;
			db = new DataBlock();
			db.OpenDB();
			AdministratorID = adminID;
			LockingEnabled = Util.GetSettingBool("UseLocking", false);
		}

		/// <summary>
		/// mark a record as locked for editing
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="recordID"></param>
		/// <returns>true if locked, else set message</returns>
		public bool LockTable(string tableName, int recordID, string recordDescription)
		{
			bool result = false;
			if(!LockingEnabled)return true;
			if(tableName.DoesntContain("]"))tableName = "[" + tableName + "]";
			if (AdministratorID > 0)// invalid admin id if 0 or less
			{
				if(!db.TableExists("lock"))	db.execute(@"
CREATE TABLE [dbo].[Lock](
	[LockID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](50) NULL,
	[RecordID] [int] NULL,
	[AdministratorID] [int] NULL,
	[DateLocked] [datetime] NULL,
 CONSTRAINT [pk_Lock] PRIMARY KEY CLUSTERED 
(
	[LockID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]");

				DataBlock rslock = db.execute("select * from lock where tablename=" +Fmt.SqlText(tableName) + " and recordid=" + recordID + " ");
				if (rslock.eof())
				{
					db.execute("insert into lock(tablename,recordid,administratorid,datelocked)values(" +Fmt.SqlText(tableName) + "," + recordID + ", " + AdministratorID + "," + Fmt.SqlDateTime( DateTime.Now) + ")");
					result = true;
				}else {
					var lockTime = rslock.GetValueDate("DateLocked");
					if (lockTime.AddMinutes(LockNumMinutes) < DateTime.Now)// has lock timed out
					{
						//auto unlock / relock
						IsLocked = true;
						string adminUser = db.FetchValue("select email from Person where PersonID=" + AdministratorID + "");
						UnLockTable(tableName, recordID, true); //any person unlock record - timed out
						Web.InfoMessage = "Record was locked by "+adminUser+", but its been "+LockNumMinutes+" minutes without a save, so you may now edit it. You may want to contact them to check you are not overwriting their work.";
						result = LockTable(tableName, recordID, recordDescription);
					}else if (AdministratorID != rslock.GetValueInt("AdministratorID"))// make sure not same user
					{
						//int lockid = rslock.GetValueInt("LockID");
						int adminid = rslock.GetValueInt("AdministratorID");

						string adminUser = db.FetchValue("select email from Person where PersonID=" + adminid + "");
						LockMessage = "This edit page is in use (" + Fmt.TimeDiffText( rslock["DateLocked"].ConvertToDate(null)) + " ago) by " + adminUser + ": " + recordDescription + " (id:" + recordID + "). You may save, but your changes may be overwritten by them. Check with this person, or wait "+LockNumMinutes+" mins.";
					}else
					{
						result = true;
					}
				}
				rslock.close();
			}else
			{
				LockMessage = "Admin id missing";
			}
			IsLocked = result;
			return result;
		}


		/// <summary>
		/// unlock the table being edited by the current admin
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="recordID">record to unlock , or zero for new record</param>
		public void UnLockTable(string tableName, int recordID) {
			UnLockTable(tableName, recordID, false);
		}

		/// <summary>
		/// unlock the table being edited by the current admin
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="recordID">record to unlock , or zero for new record</param>
		public void UnLockTable(string tableName, int recordID, bool anyPerson)
		{
			if (Security.LoggedInUserID > 0)// invalid admin id if 0 or less
			{
				AdministratorID = Security.LoggedInUserID;
				if (recordID != 0 && IsLocked) //zero = new record
				{
					if(tableName.DoesntContain("]"))tableName = "[" + tableName + "]";
					var sql = "select * from lock where tablename=" +Fmt.SqlText(tableName) + " and recordid=" + recordID + " ";
					if (!anyPerson) {
						sql += "and  AdministratorID=" + AdministratorID + " ";
					}
					DataBlock rslock = db.execute(sql);
					if (!rslock.eof())
					{
						sql = "delete from lock where tablename=" +Fmt.SqlText(tableName) + " and recordid=" + recordID + "";
						if (!anyPerson) {
							sql += "and  AdministratorID=" + AdministratorID + " ";
						}
					
						db.execute(sql);

						IsLocked = false;
					}
					rslock.close();
				}
			}else
			{
				LockMessage = "Admin id missing";
			}
		}
	}
}