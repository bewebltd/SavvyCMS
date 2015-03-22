using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class Comment {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Comment object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data

		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here

		public enum CommentStatus { Submitted, Approved, Declined }
		public enum CommentPersonType { Member, Moderator }

		public int Depth = 0;

		public string CommenterFullName {
			get {
				if (PersonID.ToString().IsBlank() || PersonID == null || PersonID.Value.ToString().IsBlank()) {
					return CommenterName;
				}

				var p = Person.LoadByPersonID(PersonID.Value);
				if (p != null) {
					return p.FullName;
				}

				return "";
			}
		}

		public string CommentTextShort {
			get {
				int shortTextMaxLength = 30;
				string commentText = CommentText+"";
				if (commentText.Length > shortTextMaxLength) {
					return commentText.Substring(0, shortTextMaxLength);
				} else {
					return commentText.Substring(0, commentText.Length);
				}
			}
		}

		public string ApprovedByName {
			get {
				if (ApprovedByPersonID.ToString().IsBlank()) {
					return "<span style='color:#c00;'>Approval required</span>";
				}

				var p = Person.LoadByPersonID(ApprovedByPersonID.Value);
				return (p == null) ? "" : p.FullName;
			}
		}

		

		private string indentCss = null;

		public string IndentCss {
			get {
				if (indentCss == null) {
					if (ParentComment == null) {
						indentCss = "";
					} else if (ParentComment.ParentComment == null) {
						indentCss = " class=\"reply1\"";
					} else {
						indentCss = " class=\"reply2\"";
					}
				}
				return indentCss;
			}
		}

		//public string AuctionCommentIndent(int cid) {
		//  // considering this is all cached, this should be highly efficient
		//  var str = "";
		//  var parentCommentID = Comment.LoadID(cid).ParentCommentID;
		//  if (parentCommentID == null) {
		//    str = "";
		//  } else {
		//    var parentparentCommentID = Comment.LoadID(parentCommentID.Value).ParentCommentID;
		//    str = parentparentCommentID == null ? " class=\"reply1\"" : " class=\"reply2\"";
		//  }
		//  return str;
		//}

		//public static CommentList GetCommentHierarchy(Auction auction=null, BuyNowItem buyNow=null) {
		//	var fkName = auction!=null? "auctionID": "buyNowItemID";
		//	var fkValue = auction!=null? auction.ID: buyNow.ID;
		//  var hierarchy = new CommentList();
		//  var comments = CommentList.Load(new Sql("select * from Comment where "+fkName+"=", fkValue, "and status=", Comment.CommentStatus.Approved.ToString().SqlizeText(), " order by CommentDate Desc"));
		//  foreach (var comment in comments) {
		//    if (comment.ParentCommentID == null) {
		//      // top level comment
		//      comment.Depth = 0;
		//      hierarchy.Add(comment);
		//      AddChildren(hierarchy, comment, 1, comments);
		//    }
		//  }
		//  return hierarchy;
		//}

		private static void AddChildren(CommentList hierarchy, Comment parentComment, int depth, CommentList allComments) {
		  foreach (var checkComment in allComments) {
		    if (checkComment.ParentCommentID == parentComment.ID) {
		      var childComment = checkComment;
		      childComment.Depth = depth;
		      hierarchy.Add(childComment);
		      AddChildren(hierarchy, childComment, depth + 1, allComments);
		    }
		  }
		}



	}
}

namespace BewebTest {
	[TestClass]
	public class TestComment {
		[TestMethod]
		public static void TestSomething() {
			var expectedValue = 5;
			var actualValue = 5;
			Assert.AreEqual(expectedValue, actualValue);
		}
	}
}

// created: [ 04-Nov-2011 1:59:43am ]