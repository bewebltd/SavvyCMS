using System;
using System.Linq;
using System.Web;
using Beweb;
using Site.SiteCustom;


namespace Models {
	public partial class PageList {}

	public partial class Page {
		public bool SwitchToNonSSLMode = false;
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Page object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			DateAdded = DateTime.Now;
			ShowInMainNav = true;
			//ShowInFooterNav = true;

			PublishDate = DateTime.Today;
			ShowInXMLSitemap = true;
			Fields.Picture.MetaData = GetPicMetaData();
			//Fields.SidebarPicture.MetaData = new DefaultPictureMetaData();
			//Fields.SidebarPicture.MetaData = new DefaultPictureMetaData();
			//Fields.Picture.MetaData.AllowPaste = true; //image paste example
		}
		public static PictureMetaDataAttribute GetPicMetaData() {
			var meta = new DefaultPictureMetaData();
			/*
			meta.IsExact = false;
			meta.IsCropped = false;
			meta.Width = 325;
			meta.Height = 244;
			meta.ThumbnailWidth = 100;
			meta.ThumbnailHeight = 100;
			*/
			return meta;
		}

		// You can put any business logic associated with this entity here


		public PictureMetaDataAttribute GetMetaData() {
			var meta = new PictureMetaDataAttribute();

			meta = new DefaultPictureMetaData();

			
			return meta;
		}

		public static Page LoadPageContent(string pageCodeValue) {
			return LoadOrCreatePageCode(pageCodeValue);
		}
		/// <summary>
		/// Custom URL scheme for this project
		/// </summary>
		/// <returns></returns>
		public override string GetUrl() {
			return GetUrl("page");
		}

		public string GetFullUrl() {
			return GetFullUrl("page");
		}

		public string GetFullUrl(string defaultControllerTemplate) {
			string url = GetUrl(defaultControllerTemplate);
			if (url.IsNotBlank()) { //20140821jn added as cant resolve blank
				if (!url.StartsWith("http") && !Util.ServerIsDev) {
					//url = Util.GetSetting("WebsiteBaseUrl").RemoveSuffix("/") + url;
					url = Web.ResolveUrlFull(url);
				}
			}
			return url;
		}

		/// <summary>
		/// Custom URL scheme for pages in this project
		/// </summary>
		/// <returns></returns>
		public override string GetUrl(string defaultControllerTemplate) {
			string url;
			if ((this.PageIsALink) || this.TemplateCode == "link") {
				Logging.trace("[" + this.Title + "]urllink");
				url = Fmt.ReplaceWebsiteBaseUrlPaths(this.LinkUrl);
			} else if (this.TemplateCode == "products") {
				url = base.GetUrl("Products");
			} else if (this.TemplateCode == "articlepage") {
				url = base.GetUrl("ArticlePage");
			} else if (this.TemplateCode == "resources") {
				url = base.GetUrl("Resources");
			} else if (this.TemplateCode == "section") {  //section place holder returns first child
				var children = this.ChildPages.Filter(page => page.GetIsActive());
				if (children != null && children.Count > 0) {
					url = children[0].GetUrl();
				} else {
					//Web.ErrorMessage = "Page Not Found";
					url = Web.Root;
				}
			} else if (this.URLRewriteTitle.IsNotBlank() && defaultControllerTemplate == "page" && PageCode.IsBlank()) {
				// user has set up this page with a custom url
				url = Web.Root + "page/" + this.URLRewriteTitle;
				Logging.trace("["+this.Title+"]urlrw");
			} else if (this.PageCode.IsNotBlank()) {
				if (PageCode.ToLower() == "home") {
					url = Web.Root;
				//} else	if (PageCode.ToLower() == "accessibility") {
				//	url = base.GetUrl(defaultControllerTemplate);						//this is how to use a page code, but not be a controller
				} else {
					// this is a built in page that corresponds to a controller
					url = Web.Root + this.PageCode;
				}
				Logging.trace("["+this.Title+"]urlpg");
			} else {
				// either a built-in template or a non-custom url (we don't allow custom urls for built-in templates)
				url = base.GetUrl(defaultControllerTemplate);
				Logging.trace("["+this.Title+"]urlbase");
			}

			if ((this.SwitchToNonSSLMode)) { //note this needs testing, maybe path wrong JC 20140625
				url = Web.BaseUrlSSL + url.RemovePrefix("/");;
			}

			return url;
		}

		public override string GetDefaultOrderBy() {
			return "order by sortposition,pageID";
		}

		public static Page LoadOrCreatePageCode(System.String pageCodeValue) {
			Page page = PageCache.GetByPageCode(pageCodeValue);
			if (page == null || page.IsNewRecord) {
				page = new Page() {
					PageCode = pageCodeValue,
					TemplateCode = "special",
					DateAdded = DateTime.Now,
					PublishDate = DateTime.Now,
					Title = Fmt.SplitTitleCase(pageCodeValue),
					BodyTextHtml = "Copy to be written for [" + pageCodeValue + "]<br>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto.",
					RevisionStatus = "Live",
					PageIsALink= false,
					ShowInMainNav = false,
					ShowInFooterNav = false,
					SortPosition = 999
				};
				page.Save();
				PageCache.Rebuild();
			}

			return page;
		}


		public string GetNavTitle() {
			var page = this;
			if (page.FieldExists("NavTitle") && !page["NavTitle"].IsBlank) {
				return page["NavTitle"].ToString();
			} else if (page.FieldExists("Title") && !page["Title"].IsBlank) {
				return page["Title"].ToString();
			} else {
				return "Title field not found";
			}
		}

		public string SectionTitle {
			get {
				var ancestor = this;
				while (ancestor.ParentPageID != 0 && ancestor.ParentPage != null) {
					ancestor = ancestor.ParentPage;
				}
				return ancestor.GetNavTitle();
			}
		}

		public string SearchResultsText {
			get {
				return (Introduction.IsNotBlank()) ? "<p>" + Introduction + "</p>" : BodyTextHtml;
			}
		}

		/// <summary>
		/// The PageID of the topmost page in the section that this page is in
		/// </summary>
		public int SectionPageID {
			get {
				var ancestor = this;
				while (ancestor.ParentPageID != 0 && ancestor.ParentPage != null) {
					ancestor = ancestor.ParentPage;
				}
				return ancestor.ID;
			}
		}

		public string SectionCode {
			get {
				var ancestor = this;
				while (ancestor.ParentPageID != 0 && ancestor.ParentPage != null) {
					ancestor = ancestor.ParentPage;
				}
				return ancestor.PageCode;
			}
		}

		/* example useful methods 
		public HtmlString GetIntroHtml(int maxChars) {
			string result = Fmt.TruncHTML(BodyTextHtml.StripTags(), maxChars);
			result = result.HtmlEncode();
			if (IsStory) {
				result = "<span class='dateline'>"+Fmt.ShortDate(PublishDate)+" &ndash;</span> " + result + "...";
			}
			return new HtmlString(result);
		}

		public bool IsStory {
			get { return TemplateCode=="Story"; }
		}
		*/
		//public int GetDepth(Page page, PageList pages) {
		//  int depth = 0;
		//  var checkPage = page;
		//  while(checkPage.ParentPageID != null && checkPage != null) {
		//    checkPage = pages.innerList.Find(p=>p.PageID == checkPage.ParentPageID);
		//    depth++;
		//  }
		//  return depth;
		//}

		/// <summary>
		/// Returns a list of all page, in order with children underneath their parents and with an extra virtual field called Depth
		/// </summary>
		/// <returns></returns>
		public static PageList GetPageHierarchy(){
			var pageHierarchy = new PageList();

#if PageRevisions
			var pages = PageList.Load(new Sql("select *, 0 as Depth from Page where HistoryPageID IS NULL and RevisionStatus = ", "Live".SqlizeText(), " order by SortPosition,PageID"));
#else
			var pages = PageList.Load(new Sql("select *, 0 as Depth from Page order by SortPosition,PageID"));
#endif
			foreach (var page in pages) {
				//page["Depth"].ValueObject = GetDepth(page, pages);
				
				if (page.ParentPage==null) {
				  // top level page
				  page["Depth"].ValueObject = 0;
				  pageHierarchy.Add(page);
				  AddChildren(pageHierarchy,page,1, pages);
				}
			}

			return pageHierarchy;
		}

		private static void AddChildren(PageList hierarchy, Page parentPage, int depth, PageList allPages) {
		  foreach (var checkPage in allPages) {
				if (checkPage.ParentPageID == parentPage.ID) {
					var childPage = checkPage;
					childPage["Depth"].ValueObject = depth;
					hierarchy.Add(childPage);
					AddChildren(hierarchy, childPage, depth + 1, allPages);
				}
		  }
		}
		
		public static string SearchableFieldNames {
			get { return new Page().GetTextFieldNames().Join(","); }
		}
		#if ArticlePage
		public ArticleList GetArticles() {
			var articles = new ArticleList();
			if (DisplayOrder == "Most Recent") {
				//articles = this.Articles.Active.OrderByDescending(a => a.PublishDate).ToList();
				articles = ArticleCache.Active.Where(a =>a.PageID == PageID).OrderByDescending(a => a.PublishDate).ToList();
			} else if (DisplayOrder == "Sort Position") {
				//articles = this.Articles.Active.OrderBy(a => a.SortPosition).ToList();
				articles = ArticleCache.Active.Where(a =>a.PageID == PageID).OrderBy(a => a.SortPosition).ToList();
			}
			return articles;
		}
		#endif
		
		
		public string GetPublishStatusHtml() {
			var result = GetPublishStatus();

			return "<span class=\"publish-status-" + result + "\">" + result + "</span>";
		}

		public string GetPublishStatus() {
			string result = "";
			if(FieldExists("RevisionStatus"))
			if (this["RevisionStatus"]+"" == "Live") {
				if (PublishDate == null) {
					result = "Unpublished";
				} else if (PublishDate > DateTime.Today) {
					result = "Scheduled";
				} else if (ExpiryDate != null && ExpiryDate < DateTime.Today) {
					result = "Expired";
				} else {
					result = "Live";
				}
			} else {
				result = "Unpublished";
			}
			return result;
		}
	}
	
	

}

// created: [ 13-May-2010 10:41:39pm ]