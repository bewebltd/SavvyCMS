using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Site.SiteCustom;
using Beweb;

namespace Site.Controllers {
	public class DocumentCategoryController : Controller {
		//
		// GET: /DocumentCategory/

		//[SiteCustom.SiteOutputCache]
		public ActionResult ByPageID(int pageID) {
			DocumentCategory category = DocumentCategory.LoadByPageID(pageID);
			var data = new ViewModel(category);
			if (category == null) {
				throw new Beweb.BadUrlException("Category not found with Page ID of ["+pageID+"]");
			}
			return View("DocumentCategory", data);
			
		}

		// GET: /DocumentCategory/345
		public ActionResult ByCategoryID(string enccategoryid) {
			// get the category page and loat that page for the content page
			int categoryid = Crypto.DecryptID(enccategoryid);
			DocumentCategory category = DocumentCategory.LoadByDocumentCategoryID(categoryid);
			if (category == null) {
				throw new Beweb.BadUrlException("Category not found with ID of ["+ enccategoryid+" (" + categoryid + ")]");
			}
			var data = new ViewModel(category);
			return View("DocumentCategory", data);
		}

		public class ViewModel : PageTemplateViewModel {
			private DocumentCategory _documentCategory;
			private String _categoryBreadCrumb;
			private Page _categoryPage;

			public string CategoryBreadCrumb {
				get {
					return _categoryBreadCrumb;
				}
			}



			private void SetCategoryPage(int categoryID) {
				DocumentCategory thisCategory = DocumentCategory.LoadByDocumentCategoryID(categoryID);
				if(thisCategory.PageID.HasValue) {
					_categoryPage = Page.LoadByPageID(thisCategory.PageID.ToInt());
				}else if(thisCategory.ParentDocumentCategoryID.HasValue) {
					SetCategoryPage(thisCategory.ParentDocumentCategoryID.ToInt());
				}else {
					throw new Exception("Category has no page or parent page");
				}
			}

			private string CreateCategoryBreadCrumb(int categoryID, string categoryCrumb) {
				string separator = " &gt; ";
				string thisCrumb = "";
				DocumentCategory thisCategory = DocumentCategory.LoadByDocumentCategoryID(categoryID);
				if (thisCategory.PageID.HasValue) {
					thisCrumb = "<a href='" + _categoryPage.GetUrl() + "'>" + _categoryPage.Title + "</a>" + separator;
					thisCrumb += (categoryCrumb == "")?thisCategory.Title: "<a href='" + Web.Root + "DocumentCategory/" + Crypto.EncryptID(thisCategory.ID) + "'>" + thisCategory.Title + "</a>"+separator;
					categoryCrumb =  thisCrumb + categoryCrumb; 
				} else if (thisCategory.ParentDocumentCategoryID.HasValue) {
					thisCrumb = "<a href='" + Web.Root + "DocumentCategory/" + Crypto.EncryptID(thisCategory.ID) + "'>" + thisCategory.Title + "</a>";
					categoryCrumb = (categoryCrumb == "")?thisCategory.Title: thisCrumb	+ separator + categoryCrumb;
					categoryCrumb = CreateCategoryBreadCrumb(thisCategory.ParentDocumentCategoryID.ToInt(), categoryCrumb);
				} else {
					throw new Exception("Category has no page or parent page");
				}
				return categoryCrumb;
			}

			public ViewModel(Models.DocumentCategory documentCategory) {
				if (!documentCategory.GetIsActive()) {
					throw new Beweb.BadUrlException("Document Category not available with ID of [" + documentCategory.ID+ "]");
				}
				_documentCategory = documentCategory;
				SetCategoryPage(_documentCategory.ID);
				_categoryBreadCrumb = CreateCategoryBreadCrumb(_documentCategory.ID, "");
				// add the page 
				ContentPage = _categoryPage;
				TrackingBreadcrumb.Current.AddBreadcrumb(1, _documentCategory.Title);
			}

			public string CategoryTitle { get {return _documentCategory.Title;}}


			public DocumentCategory Category {
				get { return _documentCategory; }
			}

			private DocumentList downloadList ;

			public DocumentList DownloadList {
				get {
					if(downloadList == null){
						// documents do not have a active setting they are either visible to a group or not.
						Sql documentSql = new Sql("Select * from Document where DocumentCategoryID = ", Category.ID.SqlizeNumber());
						documentSql.Add(" order by SortPosition, DateAdded Desc");
						downloadList = DocumentList.Load(documentSql);
					}
					return downloadList;
				}
			}

			public DocumentCategoryList CategoryList {
				get {
					DocumentCategoryList categoryList = DocumentCategoryList.Load(new Sql("Select * from DocumentCategory where ParentDocumentCategoryID = ", Category.ID.SqlizeNumber(), " and IsActive=", true.SqlizeBool(), " order by SortPosition"));
					return categoryList;
				}

			}

		}
	}
}
