using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class NavigationController : Controller {
		//
		// GET: /Navigation/

		/// <summary>
		/// Render primary navigation, including top level items and optionally also with first level of children underneath.
		/// This can be used for top navigation with dropdown menus.
		/// </summary>
		public ActionResult MainNav(string sectionCode, Page currentPage, bool includeChildren) {
			var data = new NavViewModel();
			sectionCode = sectionCode+"";
			data.SectionCode = sectionCode;

			var sql = new Sql("where ShowInMainNav=1 and ParentPageID is null");
			sql.AndIsActive<Page>();
			//var pages = PageList.Load(sql);
			var pages = PageCache.MainNav;
			foreach (var page in pages) {
				var item = new NavItem(page);
				if (page.ID == currentPage.SectionPageID) {
					item.IsSelected = true;
				//} else if (page.PageCode.IsNotBlank() && page.PageCode.ToLower()==sectionCode.ToLower()) {
				//	item.IsSelected = true;
				}
				if(page.LinkUrlIsExternal) {
					item.IsExternalUrl = true;
				}
				if (includeChildren) {
					item.SubPages = GetChildren(page, sectionCode); 
				}
				data.NavItems.Add(item);
			}

			// set css classes
			int selectedItemIndex = data.NavItems.FindIndex(item=>item.IsSelected);
			if (selectedItemIndex>-1) {
				data.NavItems[selectedItemIndex].CssClass = "active";
			}
			data.CurrentPageID = currentPage.ID;
			return View(data);
		}

		public ActionResult SubNav(string sectionCode, Models.Page page) {
			var data = new NavViewModel();
			//var sql = new Sql("where ParentPageID=", page.ID);

			var sql = new Sql();
			if(page.ParentPageID != null) {
				sql.Add("where ParentPageID=", page.ParentPageID.Value);
			} else {
				sql.Add("where ParentPageID=", page.ID);
			}

			sql.AndIsActive<Page>();
			var pages = PageList.Load(sql);

			foreach (var childPage in pages) {
				var item = new NavItem(childPage);
				if (childPage.ID == page.ID) {
					item.IsSelected = true;
				}

				data.NavItems.Add(item);
			}
			
			int selectedItemIndex = data.NavItems.FindIndex(item=>item.IsSelected);
			if (selectedItemIndex>-1) {
				data.NavItems[selectedItemIndex].CssClass = "active";
			}

			return View(data);
		}

		private List<NavItem> GetChildren(Models.Page page, string sectionCode) {
			List<NavItem> children = new List<NavItem>();
			// example code: how to add records from other tables as children in the nav
			//if(page.PageCode.ToLower() == "flights"){
			//  foreach (var region in Region.LoadRegionsWithCurrentDeals("flights")){
			//    item.SubPages.Add(new NavItem{Title=region.Name, Url = region.GetUrl()});
			//  }
			//}
			//if(page.PageCode.ToLower() == "holidays"){
			//  foreach (var destination in Destination.LoadDestinationsWithCurrentDeals("holidays")){
			//    item.SubPages.Add(new NavItem{Title=destination.Title, Url = destination.GetUrl()});
			//  }
			//}
			foreach (var childPage in page.ChildPages.Active) {
				var navItem = new NavItem(childPage);
				children.Add(navItem);
				navItem.SubPages = GetChildren(childPage, sectionCode);
			}
			return children;
		}

		/// <summary>
		/// Shows a sidenav with subpages expanded. 
		/// This code assumes top level pages are those with a NULL ParentPageID (ie 'Home' would not normally be the root unless you want it displayed in the sidenav).
		/// </summary>
		public ActionResult SideNavPages(string sectionCode, Page contentPage) {
			var data = new SideNavViewModel();
			// get selected pageID
			int selectedPageID = 0;
			if (contentPage!=null) {
				selectedPageID = contentPage.ID;
			}
			// get root (ie a top level section page)
			var root = contentPage;
			if (root==null && sectionCode!=null) {
				root = Page.LoadByPageCode(sectionCode);
			}
			while (root.ParentPage != null) {
				root = root.ParentPage;
			}
			var item = new NavItem { Title = root.GetNavTitle(), Url = root.GetFullUrl(), PageCode = root.PageCode, IsExternalUrl = root.LinkUrlIsExternal };
			item.IsSelected = (root.ID == contentPage.ID);
			data.RootNavItem = item;
			data.PanelTitle = root.GetNavTitle();
			// get pages down from root
			foreach (var page in root.ChildPages.Active){
				item = new NavItem { Title = page.GetNavTitle(), Url = page.GetFullUrl(), PageCode = page.PageCode, IsExternalUrl = page.LinkUrlIsExternal };
				item.IsSelected = (page.ID == contentPage.ID);
				var isExpanded = (item.IsSelected || page.ID == contentPage.ParentPageID);
				if (isExpanded) {
					// get subpages of selected page
					foreach (var childPage in page.ChildPages.Active) {
						if (childPage.ShowInMainNav) {
							item.SubPages.Add(new NavItem { PageID = childPage.ID, Title = childPage.GetNavTitle(), Url = childPage.GetFullUrl(), PageCode = childPage.PageCode, IsSelected = (childPage.ID == contentPage.ID) });
						}
					}
				}
				data.NavItems.Add(item);
			}
			return View("SideNav", data);
		}

		public class SideNavViewModel {
			public string PanelTitle;
			public List<NavItem> NavItems = new List<NavItem>();
			public NavItem RootNavItem;
		}


/*
		/// <summary>
		/// Render the primary naviation along the top
		/// </summary>
		/// <param name="sectionCode"></param>
		/// <param name="currentPageID"></param>
		/// <returns></returns>
		public ActionResult MainNav(string sectionCode, int currentPageID) {
			var data = new NavViewModel();
			data.SectionCode = sectionCode;
			var sql = new Sql("where ShowInMainNav=1 and ParentPageID is null");
			sql.AndIsActive<Page>();
			sql.Paging(6);  // get first 6 only
			data.NavItems = PageList.Load(sql);
			data.CurrentPageID = currentPageID;
			return View(data);
		}

		/// <summary>
		/// Render the small nav in the corner at the very top
		/// </summary>
		/// <param name="sectionCode"></param>
		/// <param name="currentPageID"></param>
		/// <returns></returns>
		public ActionResult SecondaryNav(string sectionCode, int currentPageID) {
			var data = new NavViewModel();
			data.SectionCode = sectionCode;
			data.CurrentPageID = currentPageID;
			var sql = new Sql("where ShowInSecondaryNav=1");
			sql.AndIsActive<Page>();
			data.Pages = PageList.Load(sql);
			return View(data);
		}

		/* not used
		/// <summary>
		/// Render subnav (single level only)
		/// </summary>
		/// <param name="sectionCode"></param>
		/// <param name="currentPageID"></param>
		/// <returns></returns>
		public ActionResult SubNavSingleLevel(string sectionCode, int currentPageID) {
			var data = new NavViewModel();
			data.SectionCode = sectionCode;
			data.CurrentPageID = currentPageID;
			var currentTopPage = Models.Page.LoadByPageCode(sectionCode);
			if (currentTopPage != null) {
				data.Pages = currentTopPage.ChildPages.Active;
			}
			return View(data);
		}
		*/

//		public ActionResult FooterNav(string sectionCode, int currentPageID) {
		public ActionResult FooterNav() {
			var data = new NavViewModel();
			data.SectionCode = ""; // irrelevant as not used
			var sql = new Sql("where ShowInFooterNav=1");
			sql.AndIsActive<Page>();
			var pages = PageList.Load(sql);
			foreach (var page in pages){
				if(true){
					var item = new NavItem (page);
					data.NavItems.Add(item);
				}else{
					//all footers
					
					var item = new NavItem { PageID = page.ID, Title = page.GetNavTitle(), Url = page.GetFullUrl(), PageCode = page.PageCode, IsExternalUrl = page.LinkUrlIsExternal };
					//item.SubPages = GetChildren(page, null).FindAll(p => p.); 
					foreach (var childPage in page.ChildPages.Active) {
						if (childPage.ShowInFooterNav) {
							item.SubPages.Add(new NavItem(childPage));

							if(childPage.ChildPages != null) {
								foreach (var subChildPage in childPage.ChildPages.Active) {
									if (subChildPage.ShowInFooterNav) {
										item.SubPages.Add(new NavItem(subChildPage));
									}
								}
							}

						}
					}
					data.NavItems.Add(item);
									
				}
			}
			return View("FooterNav", data);
		}
		
		public ActionResult SiteMapFooterNav() {
			var data = new NavViewModel();
			data.SectionCode = ""; // irrelevant as not used
			var sql = new Sql("where ShowInMainNav=1 and parentpageid is null");
			sql.AndIsActive<Page>();
			var pages = PageList.Load(sql);
			foreach (var page in pages) {
				var item = new NavItem { PageID = page.ID, Title = page.GetNavTitle(), Url = page.GetFullUrl(), PageCode = page.PageCode, IsExternalUrl = page.LinkUrlIsExternal };
				data.NavItems.Add(item);
				//item.SubPages = GetChildren(page, null).FindAll(p => p.); 
				foreach (var childPage in page.ChildPages.Active) {
					if (childPage.ShowInMainNav) {
						item.SubPages.Add(new NavItem(childPage));
					}
				}

			}
			return View(data);
		}

		public ActionResult SecondaryNav() {
			var data = new NavViewModel();
			data.SectionCode = ""; // irrelevant as not used
			var sql = new Sql("where ShowInSecondaryNav=1");
			sql.AndIsActive<Page>();
			var pages = PageList.Load(sql);
			foreach (var page in pages){
				var item = new NavItem(page);
				data.NavItems.Add(item);
			}
			if (data.NavItems.Count > 0) {
				data.NavItems[0].CssClass = "first";
			}
			return View("SecondaryNav", data);
		}

		/// <summary>
		/// NavItem is a class for any view that renders navigation. It represents a single page/link in the navigation.
		/// </summary>
		public class NavItem {
			public int PageID;
			public string Url;
			public string Title;
			public string PageCode;
			public string CssClass;
			public bool IsSelected;
			public bool IsExternalUrl;
			public bool IsLast;
			public int SortPosition;
			public Models.Page Page;
			public List<NavItem> SubPages = new List<NavItem>();

			public NavItem() {}

			/// <summary>
			/// Create a nav item from a Page.
			/// </summary>
			public NavItem(Page page) {
				PageID = page.ID;
				Title = page.GetNavTitle();
				Url = page.GetFullUrl();
				PageCode = page.PageCode;
				Page = page;
				IsExternalUrl = page.LinkUrlIsExternal;
			}

			///// <summary>
			///// Create a nav item from a Product.
			///// </summary>
			//public NavItem(Product product) {
			//}

			public string GetUrl() {
				return this.Page.GetUrl();
			}
		}

		/// <summary>
		/// NavViewModel is a view model which can be used by views that render navigation. It represents all the data needed for the navigation view.
		/// </summary>
		public class NavViewModel {
			public List<NavItem> NavItems = new List<NavItem>();
			public string SectionCode = "";
			public int CurrentPageID;
			public string baseUrl;
			//public ActiveRecordList<Page> Pages;

			public NavViewModel() {
				baseUrl =  Util.ServerIsDev? Web.BaseUrl: Util.GetSetting("WebsiteBaseUrl");
				if (Web.Protocol=="https") {
					baseUrl = baseUrl.ReplaceFirst("http://", "https://");
				}
				baseUrl = baseUrl.RemoveSuffix("/");
			}

			public bool DrawDropdown(NavItem navItem) {
				// Don't show the dropdown if we are aleady on the page that has children or on the children pages
				if(navItem.PageID == CurrentPageID) {
					return false;
				}
				foreach (var subPage in navItem.SubPages) {
					if(subPage.PageID == CurrentPageID) {
						return false;
					}
				}

				return true;
			}

		}

		/// <summary>
		/// Render a breadcrumb navigation based on where the page is in the site structure.
		/// The other kind of breadcrumb is called TrackingBreadcrumb
		/// </summary>
		public ActionResult StructuralBreadcrumb(string sectionCode, int currentPageID) {
			return StructuralBreadcrumbWithAltTitle(sectionCode, currentPageID, "", null);
		}

		/// <summary>
		/// Render a breadcrumb navigation based on where the page is in the site structure with extra items at the end.
		/// The other kind of breadcrumb is called TrackingBreadcrumb
		/// </summary>
		public ActionResult StructuralBreadcrumbWithExtra(string sectionCode, int currentPageID, List<NavItem> extraBreadcrumbs) {
			return StructuralBreadcrumbWithAltTitle(sectionCode, currentPageID, "", extraBreadcrumbs);
		}

		/// <summary>
		/// Render a breadcrumb navigation based on where the page is in the site structure.
		/// The other kind of breadcrumb is called TrackingBreadcrumb
		/// </summary>
		public ActionResult StructuralBreadcrumbWithAltTitle(string sectionCode, int currentPageID, string altTitle, List<NavItem> extraBreadcrumbs) {
			var data = new NavViewModel();
			data.SectionCode = sectionCode;
			data.CurrentPageID = currentPageID;
			var breadPage = Page.LoadID(currentPageID);
			if (breadPage == null || breadPage.PageCode == "Home") {
				return Content("");
			}

			if (extraBreadcrumbs != null && extraBreadcrumbs.Count > 0) {
				// Make the last item from the page hierarchy clickable
				data.NavItems.Add(new NavItem { Title = breadPage.GetNavTitle(), Url = breadPage.GetFullUrl(), PageCode = breadPage.PageCode });

				// Add the last extra bradcrumb as the non clickable item anb remove it from the list of clickable items
				ViewData["BreadcrumbCurrentPageTitle"] = extraBreadcrumbs.Last().Title;
				extraBreadcrumbs.Remove(extraBreadcrumbs.Last());
			} else {
				ViewData["BreadcrumbCurrentPageTitle"] = altTitle.IsNotBlank() ? altTitle : breadPage.GetNavTitle();
			}
			
			while (breadPage.ParentPageID != null) {
				breadPage = breadPage.ParentPage;
				data.NavItems.Add(new NavItem { Title = breadPage.GetNavTitle(), Url = breadPage.GetFullUrl(), PageCode = breadPage.PageCode });
			}

			// Add Home
			data.NavItems.Add(new NavItem { Title = "Home", Url = Web.BaseUrl });
			
			data.NavItems.Reverse();//todo make original loop do the reverse
			
			// Add extra breadcrumbs
			if (extraBreadcrumbs != null) {
				data.NavItems.AddRange(extraBreadcrumbs);
			}
			return View("Breadcrumb", data);
		}
		
		/// <summary>
		/// Render a breadcrumb navigation based on the path the user has taken through the site.
		/// The other kind of breadcrumb is called StructuralBreadcrumb
		/// </summary>
		public ActionResult TrackingBreadcrumb(string sectionCode, int currentPageID) {
			return Content(SiteCustom.TrackingBreadcrumb.Current.GetBreadcrumbLinks());
		}

	}
}
