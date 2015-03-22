

namespace Wikey.Domain.Components
{
	/*
* HtmlToPdfBuilder.cs
* ---------------------------------
* Hugo Bonacci (webdev_hb@yahoo.com)
* www.hugoware.net
	 * 
	 * 
If you’ve ever had to generate PDFs on the fly then you may have run into iTextSharp, which is a port of iText for C#. It isn’t as straight forward as some people might like, but it is certainly a powerful tool once you figure it out.

Normally, if you wanted to take some HTML and turn it into a PDF you would write a lot of extra code to recreate the document in PDF format, but lucky for all of us, iTextSharp also supports HTML. Again, it isn’t exactly straight forward, but to help I wrote a quick wrapper today to help with the process.

HtmlToPdfBuilder allows you to build a PDF using HTML and hides the complexities of working with iTextSharp. You still need iTextSharp to get this project to run, so make sure to include it.

To start, create a new HtmlToPdfBuilder object. As part of the constructor you’ll need to set the document size. It’s actually a Rectangle, but iTextSharp has predefined sizes already available in the PageSize class (constants)

1
//Page sizes are found in iTextSharp.text.PageSize
2
HtmlToPdfBuilder builder = new HtmlToPdfBuilder(PageSize.LETTER);
After you have the builder you can add as many pages as you would like using the .AddPage() method. You can also access each of the pages by their index on the builder you create.

1
HtmlPdfPage first = builder.AddPage();
2
//also found at builder[0]
3
 
4
HtmlPdfPage second = builder.AddPage();
Once you’ve added your pages you can start adding your HTML with .AppendHtml().

1
first.AppendHtml("<h1>Hello World</h1>");
2
 
3
//you can also use params for formatting
4
second.AppendHtml("<h1>{0}</h1><span>{0}</span>", "Hello Second Page", "Another Param");
Next, you’re going to want to apply some styles to your PDF document, you can use a couple methods. First, the .AddStyle() let’s you add a single style to your page. The first parameter is the selector, such as "H1" or ".totals", the second parameter is a single line of CSS such as "color:#F00;font-weight:bold;".

There is also a method called .ImportStylesheet() that accepts an absolute path (not relative) to a stylesheet and adds all of the styles it finds. I was pretty pleased with the method because I was able to do the entire thing with a single Regular Expression.

1
//add individual styles
2
builder.AddStyle("H1", "color:#F00");
3
builder.AddStyle("p", "font-weight:bold;text-decoration:underline;");
4
 
5
//import an entire sheet
6
builder.ImportStylesheet("c:\\stylesheets\\pdf.css");
It’s worth mentioning that all of my efforts to set heights, widths, paddings and margins didn’t go so well. I’m not sure what the rules are when it comes to that part so be warned.

Finally, you’re ready to save your document. Use the .RenderPdf() method to get the bytes of your PDF.

1
byte[] file = builder.RenderPdf();
2
File.WriteAllBytes("c:\\output\\final.pdf", file);
If you’ve worked with iTextSharp before, or you want a little more control over the rendering process, the builder has two events named BeforeRender and AfterRender that give you access to the iTextSharp classes PdfWriter and Document.

	 
	
	 * 
	*/

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using iTextSharp.text;
	using System.IO;
	using iTextSharp.text.pdf;
	using iTextSharp.text.html.simpleparser;
	using System.Collections;
	using iTextSharp.text.html;
	using System.Text.RegularExpressions;



	#region HtmlToPdfBuilder Class

	/// <summary>
	/// Simplifies generating HTML into a PDF file
	/// </summary>
	public class HtmlToPdfBuilder
	{

		#region Constants

		private const string STYLE_DEFAULT_TYPE = "style";
		private const string DOCUMENT_HTML_START = "<html><head></head><body>";
		private const string DOCUMENT_HTML_END = "</body></html>";
		private const string REGEX_GROUP_SELECTOR = "selector";
		private const string REGEX_GROUP_STYLE = "style";

		//amazing regular expression magic
		private const string REGEX_GET_STYLES = @"(?<selector>[^\{\s]+\w+(\s\[^\{\s]+)?)\s?\{(?<style>[^\}]*)\}";

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new PDF document template. Use PageSizes.{DocumentSize}
		/// </summary>
		public HtmlToPdfBuilder(Rectangle size)
		{
			this.PageSize = size;
			this._Pages = new List<HtmlPdfPage>();
			this._Styles = new StyleSheet();
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Method to override to have additional control over the document
		/// </summary>
		public event RenderEvent BeforeRender = (writer, document) => { };

		/// <summary>
		/// Method to override to have additional control over the document
		/// </summary>
		public event RenderEvent AfterRender = (writer, document) => { };

		#endregion

		#region Properties

		/// <summary>
		/// The page size to make this document
		/// </summary>
		public Rectangle PageSize { get; set; }

		/// <summary>
		/// Returns the page at the specified index
		/// </summary>
		public HtmlPdfPage this[int index]
		{
			get
			{
				return this._Pages[index];
			}
		}

		/// <summary>
		/// Returns a list of the pages available
		/// </summary>
		public IEnumerable<HtmlPdfPage> Pages
		{
			get
			{
				return this._Pages.AsEnumerable();
			}
		}

		#endregion

		#region Members

		private List<HtmlPdfPage> _Pages;
		private StyleSheet _Styles;

		#endregion

		#region Working With The Document

		/// <summary>
		/// Appends and returns a new page for this document
		/// </summary>
		public HtmlPdfPage AddPage()
		{
			HtmlPdfPage page = new HtmlPdfPage();
			this._Pages.Add(page);
			return page;
		}

		/// <summary>
		/// Removes the page from the document
		/// </summary>
		public void RemovePage(HtmlPdfPage page)
		{
			this._Pages.Remove(page);
		}

		/// <summary>
		/// Appends a style for this sheet
		/// </summary>
		public void AddStyle(string selector, string styles)
		{
			this._Styles.LoadTagStyle(selector, HtmlToPdfBuilder.STYLE_DEFAULT_TYPE, styles);
		}

		/// <summary>
		/// Imports a stylesheet into the document
		/// </summary>
		public void ImportStylesheet(string path)
		{

			//load the file
			string content = File.ReadAllText(path);

			//use a little regular expression magic
			foreach (Match match in Regex.Matches(content, HtmlToPdfBuilder.REGEX_GET_STYLES))
			{
				string selector = match.Groups[HtmlToPdfBuilder.REGEX_GROUP_SELECTOR].Value;
				string style = match.Groups[HtmlToPdfBuilder.REGEX_GROUP_STYLE].Value;
				this.AddStyle(selector, style);
			}

		}


		#endregion

		#region Document Navigation

		/// <summary>
		/// Moves a page before another
		/// </summary>
		public void InsertBefore(HtmlPdfPage page, HtmlPdfPage before)
		{
			this._Pages.Remove(page);
			this._Pages.Insert(
					Math.Max(this._Pages.IndexOf(before), 0),
					page);
		}

		/// <summary>
		/// Moves a page after another
		/// </summary>
		public void InsertAfter(HtmlPdfPage page, HtmlPdfPage after)
		{
			this._Pages.Remove(page);
			this._Pages.Insert(
					Math.Min(this._Pages.IndexOf(after) + 1, this._Pages.Count),
					page);
		}


		#endregion

		#region Rendering The Document

		/// <summary>
		/// Renders the PDF to an array of bytes
		/// </summary>
		public byte[] RenderPdf()
		{

			//Document is inbuilt class, available in iTextSharp
			MemoryStream file = new MemoryStream();
			Document document = new Document(this.PageSize);
			PdfWriter writer = PdfWriter.GetInstance(document, file);

			//allow modifications of the document
			if (this.BeforeRender is RenderEvent)
			{
				this.BeforeRender(writer, document);
			}

			//header
			document.Add(new Header(Markup.HTML_ATTR_STYLESHEET, string.Empty));
			document.Open();

			//render each page that has been added
			foreach (HtmlPdfPage page in this._Pages)
			{
				document.NewPage();

				//generate this page of text
				MemoryStream output = new MemoryStream();
				StreamWriter html = new StreamWriter(output, Encoding.UTF8);

				//get the page output
				html.Write(string.Concat(HtmlToPdfBuilder.DOCUMENT_HTML_START, page._Html.ToString(), HtmlToPdfBuilder.DOCUMENT_HTML_END));
				html.Close();
				html.Dispose();

				//read the created stream
				MemoryStream generate = new MemoryStream(output.ToArray());
				StreamReader reader = new StreamReader(generate);
				foreach (var item in (IEnumerable)HTMLWorker.ParseToList(reader, this._Styles))
				{
					document.Add((IElement)item);
				}

				//cleanup these streams
				html.Dispose();
				reader.Dispose();
				output.Dispose();
				generate.Dispose();

			}

			//after rendering
			{
				this.AfterRender(writer, document);
			}

			//return the rendered PDF
			document.Close();
			return file.ToArray();

		}

		#endregion

	}

	#endregion


	#region HtmlPdfPage Class

	/// <summary>
	/// A page to insert into a HtmlToPdfBuilder Class
	/// </summary>
	public class HtmlPdfPage
	{

		#region Constructors

		/// <summary>
		/// The default information for this page
		/// </summary>
		public HtmlPdfPage()
		{
			this._Html = new StringBuilder();
		}

		#endregion

		#region Fields

		//parts for generating the page
		internal StringBuilder _Html;

		#endregion

		#region Working With The Html

		/// <summary>
		/// Appends the formatted HTML onto a page
		/// </summary>
		public virtual void AppendHtml(string content, params object[] values)
		{
			this._Html.AppendFormat(content, values);
		}

		#endregion

	}

	#endregion


	#region Rendering Delegate

	/// <summary>
	/// Delegate for rendering events
	/// </summary>
	public delegate void RenderEvent(PdfWriter writer, Document document);

	#endregion

}
