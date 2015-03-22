using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Beweb;

namespace Savvy
{
	/// <summary>
	/// Summary description for NavTree
	/// </summary>
	public class NavTree
	{
		public NavTree()
		{
		}
	
		public static void Refresh(string tableName)
		{
			string refreshSql =
				@"
DELETE FROM NavTree;

INSERT INTO NavTree (TableName, NodeID, ParentID, Title, URL, SortPosition, IsVisible, IsJustALink, ItemCode, RoleAllowed) SELECT 'Page', PageID, ParentPageID, CASE WHEN NavTitle='' or NavTitle is null THEN Title ELSE NavTitle END, CASE WHEN LinkUrl='' or IsLinkUrl is null THEN URLRewriteTitle ELSE IsLinkUrl END, SortPosition, ShowInNav, CASE WHEN IsLinkUrl='' or IsLinkUrl is null THEN 0 ELSE 1 END, PageCode, RoleAllowed FROM Page ORDER BY SortPosition ASC;

UPDATE NavTree SET 
Lineage='/' + CONVERT(nvarchar, NodeID) + '/', 
Sorting='/' + REPLACE(STR(SortPosition*NavTreeID,12,0),' ','0') + '/', 
Depth=0 
WHERE ParentID = 0;

WHILE EXISTS (SELECT * FROM NavTree WHERE Depth Is Null)
UPDATE T SET T.Depth = P.Depth + 1,
T.Lineage = P.Lineage + CONVERT(nvarchar, T.NodeID) + '/',
T.Sorting = P.Sorting + REPLACE(STR(T.SortPosition*T.NavTreeID,12,0),' ','0') + '/'
FROM NavTree AS T
INNER JOIN NavTree AS P ON T.ParentID = P.NodeID
WHERE P.Depth >=0 AND P.Lineage IS NOT NULL AND T.Depth IS NULL;";

			BewebData.ExecuteSQL(refreshSql, true);

		}

		public static DataSet GetData()
		{
			return GetData(0, false, 0);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="depth">any integer >= 1</param>
		/// <param name="showInvisible">show invisible tree items</param>
		/// <returns></returns>
		public static DataSet GetData(int depth, bool showInvisible)
		{
			return GetData(depth, showInvisible, 0);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="depth">any integer >= 1</param>
		/// <param name="showInvisible">show invisible tree items</param>
		/// <param name="parentID">the parentID at the top of the returned tree - the parent is not returned. 0 = all nodes</param>
		/// <returns></returns>
		public static DataSet GetData(int depth, bool showInvisible, int parentID)
		{
			string sql = "SELECT * FROM NavTree";
			Parameter parentIdParam = new Parameter("ParentID", TypeCode.String, parentID.ToString());
			ParameterCollection pc = new ParameterCollection();
			if (!showInvisible)
			{
				sql = BewebData.AppendWhereClause(sql, "AND", "IsVisible=1");
			}
			int lowerDepth = 0;
			if (parentID > 0)
			{
				sql = BewebData.AppendWhereClause(sql, "AND", "Lineage LIKE '%/' + @ParentID + '/%'");
				pc.Add(parentIdParam);

				// get the parent node's depth
				lowerDepth = Convert.ToInt32(BewebData.GetValue("SELECT Depth FROM NavTree WHERE NodeID=@ParentID", parentIdParam));

				sql = BewebData.AppendWhereClause(sql, "AND", "Depth > @LowerDepth");
				pc.Add("LowerDepth", TypeCode.Int32, lowerDepth.ToString());
			}
			if (depth > 0)
			{
				int higherDepth = lowerDepth + depth;
				sql = BewebData.AppendWhereClause(sql, "AND", "Depth < @HigherDepth");
				pc.Add("HigherDepth", TypeCode.Int32, higherDepth.ToString());
			}
			sql += " ORDER BY Sorting ASC";

			return BewebData.GetDataSet(sql, pc);
		}
	}
}