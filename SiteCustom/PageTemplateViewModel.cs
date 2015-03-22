//#define pages
using System;
using Beweb;
using Models;
using SavvyMVC;

namespace Site.SiteCustom {
	public class PageTemplateViewModel : SavvyBaseViewData {
		
#if pages
		private Page contentPage = new Page();        // default to blank page, makes it easier to create non-page related views
		private string _sectionCode = null;

		public Models.Page ContentPage {
			get {
				if (contentPage == null) throw new Exception("ContentPage must be set in ViewData");
				return contentPage;
			}
			set {
				contentPage = value;
				if (contentPage == null) throw new Exception("ContentPage must be set in ViewData");
				PageTitleTag = String.IsNullOrWhiteSpace(contentPage.PageTitleTag) ? contentPage.Title : contentPage.PageTitleTag;
				MetaDescription = contentPage.MetaDescription;
				MetaKeywords = contentPage.MetaKeywords;
				_sectionCode = GetPageSectionCode();
			}
		}

		public string SectionCode {
			get {
				return _sectionCode;
			}
			set {
				_sectionCode = value;
			}
		}


		public string PageCode {
			get {
				return Web.LocalUrl.Split("/")[0].ToLower();
			}
		}

		private string GetPageSectionCode() {
			var ancestor = ContentPage;
			while (ancestor.ParentPageID != 0 && ancestor.ParentPage != null) {
				ancestor = ancestor.ParentPage;
			}
			return (ancestor.PageID == 0) ? "" : ancestor.PageCode;
		}

		public PageTemplateViewModel(Models.Page page) {
			ContentPage = page;
		}
#endif
		
		public string BodyCssClass { get; set; }
		public string WebsiteImagesBaseUrl {
			get {
				return Util.GetSetting("WebsiteImagesBaseUrl", "(local)");
			}
		}
		public string PageTitleTagOutput {
			get {
				string format = "[title] - [sitename]";
				var formatField = Settings.All["PageTitleTagFormat"];
				if (formatField != null && formatField.IsNotBlank) {
					format = formatField.ValueObject.ToString();
				}
				
				string output = format.Replace("[title]", PageTitleTag).Replace("[sitename]", Util.GetSiteName()).Trim();
				if (output.StartsWith("- ")) {
					output = output.Substring(2);
				}

				return output.HtmlEncode();
			}
		}
		public PageTemplateViewModel() { }
	}
}