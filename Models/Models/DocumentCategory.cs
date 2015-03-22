using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class DocumentCategory {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new DocumentCategory object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			IsActive = true;
			PublishDate = DateTime.Now;
			DateAdded = DateTime.Now;
			DateModified = DateTime.Now;
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
		public override string GetDefaultOrderBy() {
			return base.GetDefaultOrderBy();
		}

		public override string GetUrl() {
			return base.GetUrl();
		}

		public override void Save() {
			//AutocompletePhrase.AddPhrase(this, Fields.Title);
			base.Save();
		}

		public override void Delete() {
			//AutocompletePhrase.DeletePhrase(this);
			base.Delete();
		}
		/// <summary>
		/// Returns a list of all page, in order with children underneath their parents and with an extra virtual field called Depth
		/// </summary>
		/// <returns></returns>
		public static DocumentCategoryList GetDocumentCategoryHierarchy(){
			var documentCategoryHierarchy = new DocumentCategoryList();
			var documentCategories = DocumentCategoryList.Load(new Sql("select *, 0 as Depth from DocumentCategory order by SortPosition,DocumentCategoryID"));
			foreach (var documentCategory in documentCategories) {
				//page["Depth"].ValueObject = GetDepth(page, pages);
				
				if (documentCategory.ParentDocumentCategory==null) {
				  // top level page
				  documentCategory["Depth"].ValueObject = 0;
				  documentCategoryHierarchy.Add(documentCategory);
				  AddChildren(documentCategoryHierarchy,documentCategory,1, documentCategories);
				}
			}

			return documentCategoryHierarchy;
		}

		private static void AddChildren(DocumentCategoryList hierarchy, DocumentCategory parentDocumentCategory, int depth, DocumentCategoryList allDocumentCategories) {
		  foreach (var checkDocumentCategory in allDocumentCategories) {
				if (checkDocumentCategory.ParentDocumentCategoryID == parentDocumentCategory.ID) {
					var childDocumentCategory = checkDocumentCategory;
					childDocumentCategory["Depth"].ValueObject = depth;
					hierarchy.Add(childDocumentCategory);
					AddChildren(hierarchy, childDocumentCategory, depth + 1, allDocumentCategories);
				}
		  }
		}

	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestDocumentCategory {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 13 May 2013 5:25:37pm ]