using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Management;
using System.Web.UI;
using Beweb;
using System.Data;

namespace Beweb {

	public partial class Forms {
		public static bool DefaultShowInlineValidation = false;       // to override this, set in global.asax.cs
		public static bool DefaultShowInlineAdminValidation = true;       // to override this, set in global.asax.cs
		public static bool FixIOs7SelectBoxByUsingAnOptgroup = false;       // to override this, set in global.asax.cs

		public static bool DefaultHtml5 = false;              // to override this, set in global.asax.cs
		public static bool DefaultHtml5Admin = false;         // to override this, set in global.asax.cs

		/// <summary>
		/// String of CSS classes to append to all form controls in the entire application.
		/// You should set this in global.asax.cs or BewebCoreSettings
		/// </summary>
		public static string DefaultAppendCssClass = "";

		/// <summary>
		/// String of CSS classes to append to all form controls in the admin section.
		/// You should set this in global.asax.cs or BewebCoreSettings
		/// </summary>
		public static string DefaultAppendCssClassAdmin = "";

		/// <summary>
		/// String of CSS classes to append to all form controls on the current page for the current user.
		/// You should set this in the controller or the view.
		/// (This overrides DefaultAppendCssClass and DefaultAppendCssClassAdmin which apply application-wide)
		/// </summary>
		public static string PageAppendCssClass {
			get {
				return (Web.PageGlobals["Forms.PageAppendCssClass"] as string) ?? (Web.IsAdminSection ? DefaultAppendCssClassAdmin : DefaultAppendCssClass);
			}
			set {
				Web.PageGlobals["Forms.PageAppendCssClass"] = value;
			}
		}

		public static bool PageDefaultShowInlineValidation {
			get {
				return (bool?)Web.PageGlobals["Forms.PageDefaultShowInlineValidation"] ?? (Web.IsAdminSection ? DefaultShowInlineAdminValidation : DefaultShowInlineValidation);
			}
			set {
				Web.PageGlobals["Forms.PageDefaultShowInlineValidation"] = value;
			}
		}

		public static bool PageDefaultHtml5 {
			get {
				return (bool?)Web.PageGlobals["Forms.PageDefaultHtml5"] ?? (Web.IsAdminSection ? DefaultHtml5Admin : DefaultHtml5);
			}
			set {
				Web.PageGlobals["Forms.PageDefaultHtml5"] = value;
			}
		}

		/// <summary>
		/// base class for savvy bindable field controls - not for use except for inheriting from.
		/// </summary>
		public abstract class SavvyBaseField {
			protected ActiveFieldBase boundField;
			private string baseName;
			protected string rowSuffix;
			//protected bool renderValue = false;    // TODO

			protected ActiveFieldBase BindTo {
				get { return boundField; }
				set {
					boundField = value;
					this.baseName = boundField.Name;
					this.rowSuffix = Forms.CurrentRowSuffix;
					this.name = baseName + rowSuffix;
					this.id = baseName + rowSuffix;
					if (!boundField.IsNull) {
						this.value = boundField.ToString();
					}
				}
			}

			protected bool isRequired;
			public bool IsRequired {
				get { return isRequired; }
				set { isRequired = value; }
			}

			public bool ShowValidation = PageDefaultShowInlineValidation;

			private bool _html5 = PageDefaultHtml5;
			public virtual bool Html5 {
				get { return _html5; }
				set { _html5 = value; }
			}

			/// <summary>
			/// Full name of input field eg "Title".
			/// (Including suffix if part of a subform eg "Title__storylist__0")
			/// </summary>
			protected string name { get; set; }
			protected string value { get; set; }

			/// <summary>
			/// Name of input field without suffix eg "Title".
			/// </summary>
			protected string BaseName { get { return baseName; } }
			/// <summary>
			/// Full name of input field including suffix if part of a subform eg "Title" or "Title__storylist__0"
			/// </summary>
			protected string FullName { get { return name; } }

			public string id { get; set; }
			public int? tabindex { get; set; }

			public string onclick { get; set; }
			public string onchange { get; set; }
			public string ondblclick { get; set; }
			public string onkeyup { get; set; }
			public string onkeydown { get; set; }
			public string onkeypress { get; set; }
			public string onfocus { get; set; }
			public string onblur { get; set; }
			public string dataplaceholder { get; set; }

			public string style { get; set; }

			/// <summary>
			/// example ExtraAttribs = new{mike=9}
			/// </summary>
			public object ExtraAttribs { get; set; }

			private string _cssClass;
			private string _appendedCssClasses = (" " + PageAppendCssClass).Trim();
			public string cssClass { get { return _cssClass; } set { _cssClass = value; } }

			// constructors
			protected SavvyBaseField() { }	// only for use by derived classed

			public SavvyBaseField(ActiveFieldBase fieldBinder, bool isRequired) {
				this.BindTo = fieldBinder;
				this.isRequired = isRequired;
			}

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public SavvyBaseField(string name, HttpRequest request, bool isRequired) : this(name, Web.RequestEx[name], isRequired) { }
			public SavvyBaseField(string name, Web.RequestExCollection request, bool isRequired) : this(name, Web.RequestEx[name], isRequired) { }

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public SavvyBaseField(string name, bool isRequired) {
				this.name = name;
				this.value = Web.RequestEx[name];
				this.isRequired = isRequired;
				this.id = name;
			}

			public SavvyBaseField(string name, string value, bool isRequired) {
				this.name = name;
				this.value = value;
				this.isRequired = isRequired;
				this.id = name;
			}

			public void ClearValueAttribute() {
				this.value = string.Empty;
			}

			// methods
			public virtual void AppendHtmlAttributes(HtmlTag html) {
				AppendHtmlAttributes(html, true);
			}
			public virtual void AppendHtmlAttributes(HtmlTag html, bool AddID) {
				//if (bindTo != null) {
				//  html.Add("name", bindTo.Name + Forms.CurrentRowSuffix);
				//  html.Add("id", bindTo.Name + Forms.CurrentRowSuffix);
				//} else {
				//	html.AddIfNotBlank("id", id + Forms.CurrentRowSuffix);
				//	html.AddIfNotBlank("name", name + Forms.CurrentRowSuffix);
				//}
				html.AddIfNotBlank("name", name);
				if (AddID) html.AddIfNotBlank("id", id);
				//MK 20110728 always add a value attribute to hiddens else can't get the value via javascript
				if (html.ToString().Contains("type=\"hidden\"") || html.ToString().Contains("type=\"text\"") || html.ToString().Contains("type=\"password\"")) {
					//if (renderValue) { 
					html.Add("value", value);
				} else {
					html.AddIfNotBlank("value", value);
				}

				// common attribs
				if (isRequired)
					AppendCssClass("required");

				if (tabindex.HasValue) html.AddIfNotBlank("tabindex", tabindex.Value + "");
				html.AddIfNotBlank("style", style);
				html.AddIfNotBlank("data-placeholder", dataplaceholder);

				// js events
				html.AddIfNotBlank("onclick", onclick, false);
				html.AddIfNotBlank("onchange", onchange, false);
				html.AddIfNotBlank("ondblclick", ondblclick, false);
				html.AddIfNotBlank("onkeyup", onkeyup, false);
				html.AddIfNotBlank("onkeydown", onkeydown, false);
				html.AddIfNotBlank("onkeypress", onkeypress, false);
				html.AddIfNotBlank("onfocus", onfocus, false);
				html.AddIfNotBlank("onblur", onblur, false);

				// all controls allow an anonymous object used as a property array of attributes and values
				if (this.ExtraAttribs != null) {
					if (this.ExtraAttribs.GetType() == typeof(string)) {
						html.Add(ExtraAttribs.ToString());
					} else {
						var props = ExtraAttribs.GetType().GetProperties();
						foreach (var prop in props) {
							string attribName = prop.Name;
							string attribValue = prop.GetValue(ExtraAttribs, null).ToString();
							html.Add(attribName, attribValue);
							if (attribName.ToLower() == "cssclass") {
								AppendCssClass(attribValue);
							} // class is reserved
						}
					}
				}

				html.AddIfNotBlank("class", cssClass + _appendedCssClasses);
			}
			/// <summary>
			/// add attribs to the object
			/// </summary>
			/// <param name="ExtraAttribs"></param>
			/// <returns>Returns the SavvyBaseField object, to allow chaining</returns>
			//public SavvyBaseField Add(object ExtraAttribs) {
			//	this.ExtraAttribs = ExtraAttribs;
			//	return this;
			//}


			/// <summary>
			/// returns its self to allow chaining:
			/// 
			/// e.g. : 	$ <%=(new Forms.MoneyField(individuals.Fields.Charity1DonationAmount,false)).AppendCssClass("charityamount").GetHtml() %>
			/// </summary>
			/// <param name="cls"></param>
			/// <returns></returns>
			public SavvyBaseField AppendCssClass(string cls) {
				if (!_appendedCssClasses.Contains(cls)) {
					this._appendedCssClasses += " " + cls;
				}
				return this;
			}

			public abstract string GetHtml();

			public override string ToString() {
				string html = GetHtml();
				if (ShowValidation) {
					html += "<span class=\"validation autoPosition\" id=\"validation_" + id + "\"></span>";
				}
				if (name.IsBlank()) {
					html += "input field has no name";
				}
				return html;
			}
		}

		/// <summary>
		/// Base class for any control which is based on an INPUT tag
		/// </summary>
		public class InputField : SavvyBaseField {
			public InputTypes type { get; set; }
			public enum InputTypes {
				text, button, checkbox, color, date, datetime, email, file, hidden, image, month, number, password, radio, range, reset, search, submit, tel, time, url, week //html 5 types
				//text, password, checkbox, radio, submit, reset, file, hidden, image, button  //html 4 input types
			}

			/// <summary>
			/// Set this to false to disable the standard browser password fill and remembered text dropdowns.
			/// This renders the attribute autocomplete="off"
			/// </summary>
			public bool BrowserAutocomplete = true;

			/// <summary>
			/// set the max length allowed in the field
			/// </summary>
			public string maxlength { get; set; }
			public string minlength { get; set; }

			private bool ignoreMaxLength { get; set; }
			public bool disabled { get; set; }
			public bool readOnly { get; set; }
			protected bool allowMaxlength = true;

			public void IgnoreMaxLength() {
				this.ignoreMaxLength = true;
			}

			/// <summary>
			/// Provides a text hint in the field until you click in there, then it disappears.
			/// This is built into HTML5 in modern browsers eg Chrome.
			/// For older browsers requires inclusion of jquery-placeholder-1.0.1.js
			/// </summary>
			public string placeholder;

			public InputField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public InputField(string name, bool isRequired) : base(name, isRequired) { }
			public InputField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			public override void AppendHtmlAttributes(HtmlTag html) {
				//				if(value==null)value=Web.Request[name]; -- 20100913 don't do this, use updatefromrequest instead
				html.AddIfNotBlank("placeholder", placeholder);
				if (placeholder.IsNotBlank()) AppendCssClass("placeholder");

				base.AppendHtmlAttributes(html);

				if (BindTo != null) {
					if (!ignoreMaxLength && string.IsNullOrEmpty(maxlength)) {
						maxlength = BindTo.MaxLength.ToString();
					}
				}

				if (disabled) {
					html.AddIfNotBlank("disabled", "disabled");
				}
				if (readOnly) {
					html.Add("readonly", "readonly");
				}
				if (allowMaxlength) {
					html.AddIfNotBlank("maxlength", maxlength);
					//html.AddIfNotBlank("width", maxlength);				//MN, JN 20120730 - should not have this
				}
				html.AddIfNotBlank("minlength", minlength);

				if (BrowserAutocomplete == false) {
					html.Add("autocomplete", "off");
				}
			}

			public override string GetHtml() {
				var html = new HtmlTag("input");
				html.Add("type", this.type.ToString());
				AppendHtmlAttributes(html);
				return html.ToString();
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=TEXT with skype lookup
		/// </summary>
		public class SkypeField : TextField {
			public SkypeField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { ShowTester = false; ShowLinkToSkypeCall = true; }

			public bool ShowTester { get; set; }
			public bool ShowLinkToSkypeCall { get; set; }

			// methods
			public override string GetHtml() {
				var html = new HtmlTag("input");
				if (cssClass.IsBlank())
					cssClass = "svyWideText";
				html.Add("type", "text");
				if (readOnly) {
					html.Add("readonly", "readonly");
				}

				string skTest = "";
				if (this.value.IsNotBlank() && ShowTester) {
					if (ShowLinkToSkypeCall) skTest += @"<a href=""skype:" + this.value + @"?call"">";
					skTest += @"<img src=""http://mystatus.skype.com/smallicon/" + this.value + @""" width=""16"" height=""16"" alt=""Skype status for " + this.value + @""" />";
					if (ShowLinkToSkypeCall) skTest += @"</a>";
				}

				AppendHtmlAttributes(html);
				return html.ToString() + skTest + "\n" + script;
			}
		}
		/// <summary>
		/// Outputs a INPUT TYPE=TEXT
		/// </summary>
		public class TextField : InputField {
			// constructors

			public string script { get; set; }

			/// <summary>
			/// If this is set to the current record id, the field will hide until clicked, then ajax save on blur - only for bound fields
			/// </summary>
			public int? AjaxSave { get; set; }

			public string Title { get; set; }

			private bool _devOnly = false;
			public bool DevOnly {
				get { return _devOnly; }
				set { _devOnly = value; }
			}
			private string _AutoCompleteValue;
			public string AutoCompleteValue {
				get { return _AutoCompleteValue; }
				set { _AutoCompleteValue = value; }
			}
			private bool _isAutoCompleteDropboxReplace = false;
			public bool IsAutoCompleteDropboxReplace {
				get { return _isAutoCompleteDropboxReplace; }
				//set { _isAutoComplete = value; }
			}

			/// <summary>
			/// Creates a text field and outputs it
			/// </summary>
			/// <example>test</example>
			/// <param name="bindToActiveField">Active Field to bind value and name to</param>
			/// <param name="isRequired">Required field validation</param>
			public TextField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public TextField(string name, HttpRequest request, bool isRequired) : this(name, Web.RequestEx[name], isRequired) { }
			public TextField(string name, Web.RequestExCollection request, bool isRequired) : this(name, Web.RequestEx[name], isRequired) { }

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public TextField(string name, bool isRequired) : base(name, isRequired) { }

			public TextField(string name, string value, bool isRequired) : base(name, value, isRequired) { }
			/// <summary>
			/// add attribs to the object
			/// </summary>
			/// <param name="ExtraAttribs"></param>
			/// <returns>Returns the TextField object, to allow chaining</returns>
			public TextField Add(object ExtraAttribs) {
				this.ExtraAttribs = ExtraAttribs;
				return this;
			}

			// methods
			public override string GetHtml() {
				var extraHtml = "";
				if (!_devOnly || Util.IsDevAccess()) {
					var html = new HtmlTag("input");
					if (cssClass.IsBlank()) {
						cssClass = "svyWideText";
					}
					if (Title.IsNotBlank()) {
						html.Add("title", Title.HtmlEncode());
					}
					html.Add("type", "text");
					if (readOnly) {
						html.Add("readonly", "readonly");
					}
					//if (isRequired) {			// 20100503 MN prep for html 5 -- 20100513 removed this as it caused validation to break
					//  html.Add("required", "true");
					//}

					if (_isAutoCompleteDropboxReplace) {
						extraHtml += "<input type=\"hidden\" name=\"" + this.name + "\" id=\"" + this.name + "_AutoCompleteHiddenID\">";
						name += "_TypeField";
						value = AutoCompleteValue;
					}

					AppendHtmlAttributes(html);
					//Web.Response.Flush();

					//if (BindTo != null && AjaxSave.HasValue) {
					//	var tb = new ActiveRecordGenerator.Table(BindTo.TableName, false);
					//	var saveData = Crypto.Encrypt("table:" + this.BindTo.TableName + "|col:" + tb.pkName + "|id:" + AjaxSave.Value + "");
					//	script+="<script>$(document).ready(function() {$('#"+this.id+"').hide().click(function(){$(this).show()}).blur(function(){ajaxSave('"+saveData+"',$(this).val())})});</script>";
					//}
					return ((_devOnly) ? "DEV: " : "") + html.ToString() + extraHtml + '\n' + script;

				} else {
					var html = new HtmlTag("span").Add("id", name);
					html.SetInnerText(value);
					return html.ToString();
				}
			}

			public TextField AutoComplete(string tableName, string displayCol) {
				return AutoComplete(tableName, displayCol, null);
			}

			public TextField AutoComplete(string tableName, string displayCol, string idColName) {
				return AutoComplete(tableName, displayCol, idColName, null);
			}

			/// <summary>
			/// Turn the text box into a basic autocomplete
			/// </summary>
			/// <param name="tableName">table to look for data in</param>
			/// <param name="displayCol">column to display</param>
			/// <param name="idColName">value to use in place of display</param>
			/// <param name="jqOptions">if null, default to matchContains:true,max:50 - options from http://docs.jquery.com/Plugins/Autocomplete/autocomplete#url_or_dataoptions. 
			/// if not null, you must specify all options, including defaults (ones used if null)</param>
			///<param name="textValue">value to show in the input box </param>
			///<param name="replaceDropbox">set true if you are replacing a dropbox </param>
			///example dropbox replacement:
			/// 		function handleSelect( event, ui ) {
			///				$('input[name=CompanyID]').val(ui.item.value);
			///				window.setTimeout(function (){ $('#CompanyID').val(ui.item.label);},250);
			///			}
			///	
			/// <%= new Forms.TextField(record.Fields.CompanyID, false).AutoComplete("Company","ShortName","CompanyID","select: handleSelect",replaceDropbox:true,textValue: ((record.Company!=null)?record.Company.ShortName:""))%>
			/// <returns></returns>
			public TextField AutoComplete(string tableName, string displayCol, string idColName, string jqOptions, int max = 20, string textValue = null, bool replaceDropbox = false) {
				if (replaceDropbox) {		 //if a text value is provided, 
					AutoCompleteValue = textValue;
					_isAutoCompleteDropboxReplace = true;
				}
				string data = "" + tableName + "|" + displayCol + "|" + idColName + "";
				if (jqOptions == null) {
					jqOptions = "minLength:2"; //JN 20121023 changed to : from =
				}
				//onfocus = "$(this).autocomplete({source:'" + Web.Root + "services/GetAutoComplete.aspx?max=" + max + "&data=" + Crypto.Encrypt(data) + "&rn='+Math.random()+''," + jqOptions + "}); "+
				//	"$(this).after(\'<input type=\\\"hidden\\\" name=\\\""+this.name+"\\\">\');"+
				//	"$(this).removeAttr(\\\"maxlength\\\");"+
				//	"$(this).attr(\\\"name\\\",\\\""+this.name+"display\\\");"+
				//	"this.focus = null;";
				onfocus = "$(this).autocomplete({source:'" + Web.Root + "services/GetAutoComplete.aspx?max=" + max + "&data=" + Crypto.Encrypt(data) + "&rn='+Math.random()+''," + jqOptions + "}); " +
					"$(this).removeAttr('maxlength');" +
					"this.focus = null;" +
					"";
				return this;//allow chaining
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=HIDDEN
		/// </summary>
		public class HiddenField : InputField {
			// constructors
			public HiddenField(ActiveFieldBase bindToActiveField) : base(bindToActiveField, false) { ShowValidation = false; }
			public HiddenField(string name) : base(name, false) { ShowValidation = false; }
			public HiddenField(string name, string value) : base(name, value, false) { ShowValidation = false; }

			// methods
			public override string GetHtml() {
				IgnoreMaxLength();
				var html = new HtmlTag("input");
				html.Add("type", "hidden");
				AppendHtmlAttributes(html);
				return html.ToString();
			}

		}


		/// <summary>
		/// Outputs a INPUT TYPE=CHECKBOX.
		/// A hidden field placeholder is also output for use with a boolean ActiveField. This is detected by UpdateFromRequest() so the ActiveField is set to false if the checkbox is not checked.
		/// If not using an ActiveField, you only get a value posted if checked (as this is a browser input type=checkbox).
		/// </summary>
		public class CheckboxField : InputField {

			public string label { get; set; }
			// constructors
			//public CheckboxField(ActiveFieldBase bindToActiveField,string label, bool isRequired) : base(bindToActiveField,isRequired) {this.label=label; }
			//public CheckboxField(string name, string label, bool isRequired) : base(name, isRequired) {this.label=label; }
			//public CheckboxField(string name, string label, bool value, bool isRequired) : base(name, isRequired) {this.label=label;this.value=value+""; }
			// MN 20100707 - breaking change to checkbox - label and isRequired are no longer core params - you can now set them in the curly braces instead

			/// <summary>
			/// Outputs a INPUT TYPE=CHECKBOX - great to use for terms where you must check the box.
			/// </summary>
			public CheckboxField(ActiveFieldBase bindToActiveField) : base(bindToActiveField, false) { this.label = label; }
			public CheckboxField(string name) : base(name, false) { this.label = label; }
			public CheckboxField(string name, bool value) : base(name, false) { this.label = label; this.value = value + ""; }

			// methods
			public override string GetHtml() {
				IgnoreMaxLength();
				var html = new HtmlTag("input");
				html.Add("type", "checkbox");

				if (value.ConvertToBool()) {
					html.Add("checked", "checked");
				}

				// checkboxes always should a value of true (since this is the value that is submitted if checked when posted)
				value = "true";

				AppendHtmlAttributes(html);

				string resultHTML = html.ToString();
				resultHTML += "<input type=\"hidden\" name=\"checkboxposted_" + this.FullName + "\" value=\"y\">";
				if (label.IsNotBlank()) {
                    resultHTML += " <label for=\"" + id + "\" " + ExtraAttribs+ ">" + label + " </label> ";
				}
				return resultHTML;
			}

		}

		/// <summary>
		/// Outputs the value of a decrypted bit of data - if allowed
		/// 
		/// example
		///	<%var expire = record.DateAdded.Value.AddDays(30);%>
		///	<%= new Forms.DecryptField(record.Fields.ApplicantName ,true){DecryptExpiry=expire} %>
		/// 
		/// </summary>
		public class DecryptField : TextField {
			public DateTime? DecryptExpiry { get; set; }
			// constructors
			public DecryptField(ActiveFieldBase bindToActiveField, bool ignoredField)
				: base(bindToActiveField, ignoredField) {
				this.name = name;
				//this.value = ""+(DateTime.Now.AddDays(-NumDays)<DateTime.Now?Crypto.Decrypt(value):"Data expired");;
				BindTo = new ActiveFieldBase();
				BindTo.Name = name;
				BindTo.ValueObject = value;
			}

			public override string GetHtml() {
				value = BindTo.ToString();      // this formats a date correctly
				string displayValueHTML = ("" + (DecryptExpiry.HasValue && DateTime.Now < DecryptExpiry.Value ? Crypto.Decrypt(value) : "Data expired")).HtmlEncode();
				if (displayValueHTML.IsBlank()) {
					displayValueHTML = "&nbsp;";
				}

				return displayValueHTML;
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=HIDDEN with a span prepended to it that displays the value
		/// </summary>
		public class DisplayField : HiddenField {
			string DefaultValue = null;
			// constructors
			public DisplayField(ActiveFieldBase bindToActiveField) : base(bindToActiveField) { }
			public DisplayField(ActiveFieldBase bindToActiveField, string defaultValue, bool ignoredField) : base(bindToActiveField) { this.DefaultValue = defaultValue; } //this is here to make it easy to change a checkboxfield or yesnofield into a display field
			public DisplayField(ActiveFieldBase bindToActiveField, bool ignoredField) : base(bindToActiveField) { } //this is here to make it easy to change a checkboxfield or yesnofield into a display field

			public DisplayField(string name, string value)
				: base(name, value) {
				this.name = name;
				this.value = value;
				BindTo = new ActiveFieldBase();
				BindTo.Name = name;
				BindTo.ValueObject = value;
			}

			public override string GetHtml() {
				// get a standard hidden field and also add a span containing the value
				value = BindTo.ToString();      // this formats a date correctly
				name = BindTo.Name + rowSuffix;  // MN 20141218 added suffix, wasnt correct in subform
				string html = base.GetHtml();
				string displayValueHTML = value.HtmlEncode();     // MN updated
				if (displayValueHTML.IsBlank()) displayValueHTML = DefaultValue;
				if (displayValueHTML.IsBlank()) {
					displayValueHTML = "&nbsp;";
				}

				if (this.BindTo.Type == typeof(System.Boolean)) {
					displayValueHTML = Fmt.YesNo(displayValueHTML);
				} else if (this.BindTo.Type == typeof(System.DateTime)) {
					displayValueHTML = Fmt.DateTime(displayValueHTML).Replace("12:00:00 a.m.", "");
				}
				var span = new HtmlTag("span")
					.Add("id", BindTo.Name + Forms.CurrentRowSuffix + "_span")
					.SetInnerHtml(displayValueHTML);
				if (cssClass.IsNotBlank()) {
					span.Add("class", cssClass + " displayfield");
				} else {
					span.Add("class", "displayfield");
				}

				return string.Concat(span.ToString(), html);
			}
		}

		/// <summary>
		/// Outputs a span that displays the value, without any field at all - not a hidden!
		/// </summary>
		public class DisplayOnly : HiddenField {
			private string DefaultValue;

			// constructors
			public DisplayOnly(ActiveFieldBase bindToActiveField) : base(bindToActiveField) { }
			public DisplayOnly(ActiveFieldBase bindToActiveField, string defaultValue, bool ignoredField) : base(bindToActiveField) { this.DefaultValue = defaultValue; } //this is here to make it easy to change a checkboxfield or yesnofield into a display field
			public DisplayOnly(ActiveFieldBase bindToActiveField, bool ignoredField) : base(bindToActiveField) { } //this is here to make it easy to change a checkboxfield or yesnofield into a display field

			public DisplayOnly(string name, string value)
				: base(name, value) {
				this.name = name;
				this.value = value;
				BindTo = new ActiveFieldBase();
				BindTo.Name = name;
				BindTo.ValueObject = value;
			}


			public override string GetHtml() {
				// get a span containing the value
				value = BindTo.ToString();      // this formats a date correctly
				string displayValueHTML = value.HtmlEncode();
				if (displayValueHTML.IsBlank()) displayValueHTML = DefaultValue;
				if (displayValueHTML.IsBlank()) {
					displayValueHTML = "&nbsp;";
				}

				if (this.BindTo.Type == typeof(System.Boolean)) {
					displayValueHTML = Fmt.YesNo(displayValueHTML);
				} else if (this.BindTo.Type == typeof(System.DateTime)) {
					//displayValueHTML = Fmt.DateTime( displayValueHTML).Replace("12:00:00 a.m.","");
				}
				var span = new HtmlTag("span")
					.Add("id", BindTo.Name + Forms.CurrentRowSuffix + "_span")
					.SetInnerHtml(displayValueHTML);
				if (cssClass.IsNotBlank()) {
					span.Add("class", cssClass + " displayfield");
				} else {
					span.Add("class", "displayfield");
				}

				return span.ToString();
			}
		}


		/// <summary>
		/// Outputs a INPUT TYPE=PASSWORD
		/// Defaults BrowserAutocomplete=false so it will not try and remember your password.
		/// If using this for a Login form, you should set it to true.
		/// </summary>
		public class PasswordField : InputField {
			// constructors
			public PasswordField(ActiveFieldBase bindToActiveField, bool isRequired)
				: base(bindToActiveField, isRequired) {
				BrowserAutocomplete = false;
			}
			public PasswordField(string name, bool isRequired)
				: base(name, isRequired) {
				BrowserAutocomplete = false;
			}
			public PasswordField(string name, string value, bool isRequired)
				: base(name, value, isRequired) {
				BrowserAutocomplete = false;
			}

			//public void Write()
			//{
			//	HttpContext.Current.Response.Write(this.GetHtml());
			//}

			public override string GetHtml() {
				this.type = InputTypes.password;
				return base.GetHtml();
			}

			// we dont override ToString on this

		}

		/// <summary>
		/// Outputs a INPUT TYPE=NUMBER which allows only integer input
		/// </summary>
		public class NumberField : InputField {
			public int? min { get; set; }
			public int? max { get; set; }

			protected int decimalPlaces = -1;					//-1 means auto
			public int DecimalPlaces {
				get { return decimalPlaces; }
				set { decimalPlaces = value; }
			}

			public static bool DefaultGroupDigits = true;         // default global setting, set this in BewebCoreSettings 
			public bool GroupDigits = DefaultGroupDigits;        // individual setting takes default from static 

			private bool _allowNegative = false;
			public bool AllowNegative {
				get { return _allowNegative; }
				set { _allowNegative = value; }
			}

			// constructors
			public NumberField(ActiveFieldBase bindToActiveField, bool isRequired)
				: base(bindToActiveField, isRequired) {
				if (BindTo.DecimalPlaces != null) decimalPlaces = BindTo.DecimalPlaces.Value;
			}

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public NumberField(string name, HttpRequest request, bool isRequired) : this(name, Web.RequestEx[name], isRequired) { }
			public NumberField(string name, Web.RequestExCollection request, bool isRequired) : this(name, Web.RequestEx[name], isRequired) { }

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public NumberField(string name, bool isRequired) : base(name, isRequired) { }
			public NumberField(string name, double? value, bool isRequired) : base(name, value + "", isRequired) { }
			public NumberField(string name, decimal? value, bool isRequired) : base(name, value + "", isRequired) { }
			public NumberField(string name, int? value, bool isRequired) : base(name, value + "", isRequired) { }
			public NumberField(string name, string value, bool isRequired) : base(name, value + "", isRequired) { }

			// methods
			public override void AppendHtmlAttributes(HtmlTag html) {
				base.AppendHtmlAttributes(html);
				html.AddIfNotBlank("min", min + "");
				html.AddIfNotBlank("max", max + "");
				html.AddIfNotBlank("df_groupDigits", GroupDigits.ToString().ToLower());
				html.AddIfNotBlank("df_decimalPlaces", decimalPlaces.ToString());  // this is correct, don't add that crap about -1
				html.AddIfNotBlank("df_allowNegative", AllowNegative.ToString().ToLower());
			}

			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.number; //-- possibly buggy in chrome
				} else {
					this.type = InputTypes.text;
				}
				//this.IgnoreMaxLength();
				if (this.maxlength.IsBlank()) this.maxlength = "9";
				if (this.value.IsNotBlank()) {
					try {
						if (this.value + "" == "0") this.value = "0.0";
						Decimal amount = this.value.ToDecimal(); // dont round here 20110706JN
						if (decimalPlaces != -1) Math.Round(amount, decimalPlaces);
						this.value = Fmt.Number(amount, decimalPlaces, GroupDigits);
						//}catch (System.ArgumentOutOfRangeException exception) {
						//  // 20100913 - should this exist or comment it out?
						//  throw new ActiveRecordException("Error in data value for Beweb.Forms.NumberField(). Field ["+this.name+"] value ["+this.value+"] ["+exception.Message+"]");
					} catch (System.ArgumentException exception) {
						throw new ActiveRecordException("ArgumentException Error in data value for Beweb.Forms.NumberField(). Field [" + this.name + "] value [" + this.value + "] [" + exception.Message + "]");
					} catch (System.FormatException exception) {
						throw new ActiveRecordException("FormatException Error in data value for Beweb.Forms.NumberField(). Field [" + this.name + "] value [" + this.value + "] [" + exception.Message + "]");
					} catch (System.Exception exception) {
						throw new ActiveRecordException("Error in data value for Beweb.Forms.NumberField(). Field [" + this.name + "] value [" + this.value + "] [" + exception.Message + "]");
					}
				}

				AppendCssClass("number");
				return base.GetHtml();
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=NUMBER which allows only integer input
		/// </summary>
		public class IntegerField : NumberField {
			public IntegerField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { Init(); }
			public IntegerField(string name, bool isRequired) : base(name, isRequired) { Init(); }
			public IntegerField(string name, int? value, bool isRequired) : base(name, value, isRequired) { Init(); }
			public IntegerField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			private void Init() {
				//allowMaxlength = true;
				maxlength = "9";
				decimalPlaces = 0;
				GroupDigits = false;
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=TEXT which allows only float input
		/// </summary>
		public class FloatField : NumberField {
			//public int DecimalPlaces { get { return decimalPlaces; } set { decimalPlaces = value; } } //in number field

			// constructors
			public FloatField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { Init(); }
			public FloatField(string name, bool isRequired) : base(name, isRequired) { Init(); }
			public FloatField(string name, float value, bool isRequired) : base(name, value, isRequired) { Init(); }
			public FloatField(string name, Decimal? value, bool isRequired) : base(name, value, isRequired) { Init(); }
			private void Init() {
				//allowMaxlength = false;
				maxlength = "9";
				decimalPlaces = -1;
			}

			// methods
		}

		/// <summary>
		/// Outputs a INPUT TYPE=NUMBER which allows only money input
		/// </summary>
		public class MoneyField : NumberField {
			public int DecimalPlaces { get { return decimalPlaces; } set { decimalPlaces = value; } }

			// constructors
			public MoneyField(ActiveField<decimal> bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { Init(); }
			public MoneyField(ActiveField<decimal?> bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { Init(); }
			public MoneyField(string name, bool isRequired) : base(name, isRequired) { Init(); }
			public MoneyField(string name, decimal? value, bool isRequired) : base(name, value, isRequired) { Init(); }
			private void Init() {
				//allowMaxlength = true;
				decimalPlaces = 2;
				//AllowNegative = false;
			}

			// methods
			public override string GetHtml() {
				if (this.value.IsNotBlank()) {
					this.value = Math.Round(this.value.ToDecimal(), DecimalPlaces).ToString();
				}
				AppendCssClass("money");
				// no - not necessary - this is applied in BewebInitForm()		onblur+="CheckNumberField(this, "+DecimalPlaces+","+((AllowNegative)?"true":"false")+","+((GroupDigits)?"true":"false")+");";		 //CheckNumberField(fieldObj, decimalPlaces, allowNegative, groupDigits) 
				return base.GetHtml();
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=TEXT for phone number input
		/// </summary>
		public class PhoneField : InputField {
			public PhoneField(ActiveField<string> bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public PhoneField(string name, bool isRequired) : base(name, isRequired) { }
			public PhoneField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.tel;
				} else {
					this.type = InputTypes.text;
				}
				AppendCssClass("phonenumber");
				return base.GetHtml();
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=TEXT for digits only input similar to number input except allowing leading zeros
		/// This should be used on a NVARCHAR field
		/// </summary>
		public class DigitsField : InputField {
			public DigitsField(ActiveField<string> bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public DigitsField(string name, bool isRequired) : base(name, isRequired) { }
			public DigitsField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.number;
				} else {
					this.type = InputTypes.text;
				}
				AppendCssClass("digitsonly");
				return base.GetHtml();
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=TEXT which allows only email input
		/// </summary>
		public class EmailField : InputField {
			// constructors
			public EmailField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public EmailField(string name, bool isRequired) : base(name, isRequired) { }
			public EmailField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			// methods
			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.email;
				} else {
					this.type = InputTypes.text;
				}
				AppendCssClass("email");
				AppendCssClass("svyWideText");
				return base.GetHtml();
			}
		}

		/// <summary>
		/// Outputs a INPUT TYPE=TEXT which allows only email input
		/// </summary>
		public class UrlField : InputField {
			/// <summary>
			/// set dont validate to true if you dont want to check for complete url starting with http://
			/// </summary>
			public bool DontValidate { get; set; }
			// constructors
			public UrlField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public UrlField(string name, bool isRequired) : base(name, isRequired) { }
			public UrlField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			// methods
			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.url;
				} else {
					this.type = InputTypes.text;
				}

				var input = new HtmlTag("input");
				input.Add("type", this.type.ToString());
				if (cssClass.IsBlank())
					cssClass = "svyWideText";
				if (!DontValidate) AppendCssClass("url");
				AppendHtmlAttributes(input);

				var button = new HtmlTag("input");
				button.Add("type", "button");
				button.Add("value", "Test URL");
				button.Add("class", "btn btn-mini");
				//button.Add("onclick", "window.open(jQuery(this).prev().val())"); // doesn't work in child forms in IE
				button.Add("onclick", "window.open($(this).prevAll('input[type=text]').val())");

				var div = new HtmlTag("div");
				div.AddTag(input);
				if (!DontValidate) div.AddTag(button);
				return div.ToString();
			}
		}

		public class SortPositionField : IntegerField {

			public SortPositionField(ActiveFieldBase activeField) : base(activeField, true) { }

			public SortPositionField(ActiveField<int?> bindToActiveField)
				: base(bindToActiveField, true) {
				//if (!bindToActiveField.IsNumericField || !bindToActiveField.Name.EndsWith("SortPosition"))
				//	throw new Exception(string.Format("Cannot bind field {0} to a SortPositionField because it is not of the correct type.	The field name must end with 'SortPosition' and the field type must be numeric.", bindToActiveField.Name));
			}
			public SortPositionField(ActiveField<int> bindToActiveField) : base(bindToActiveField, true) { }
			public SortPositionField(ActiveField<double?> bindToActiveField) : base(bindToActiveField, true) { }
			public SortPositionField(ActiveField<double> bindToActiveField) : base(bindToActiveField, true) { }
			public bool AutoIncrement = false;

			public override string GetHtml() {
				if (string.IsNullOrEmpty(value)) {
					if (!AutoIncrement) {
						this.value = "50";
					} else {
						var position = BewebData.GetInt("Select top 1 " + BindTo.Name + " from " + BindTo.TableName + " order by " + BindTo.Name + " desc");
						position = (position == 0) ? 50 : position + 10;
						this.value = position.ToString();
					}
				}

				return base.GetHtml();
			}
		}


		//public class RelatedCheckboxes : SavvyBaseField
		//{

		//}

		public class Dropbox : SavvyBaseField {
			public SelectOptions options = new SelectOptions();
			protected string _SelectText = "-- please select --";
			public string SelectText { get { return _SelectText; } set { _SelectText = value; } }
			/// <summary>
			/// show this when there are no options to show
			/// </summary>
			public string noitems { get; set; }
			private bool _showPleaseSelect = false;
			private string fieldName;
			private System.Data.DataSet dataSet;
			// constructors
			public Dropbox(ActiveFieldBase bindToActiveField, bool isRequired) : this(bindToActiveField, isRequired, isRequired) { }

			public Dropbox(ActiveFieldBase bindToActiveField, bool isRequired, bool showPleaseSelect)
				: base(bindToActiveField, isRequired) {
				if (showPleaseSelect) options.Add("", SelectText);
				_showPleaseSelect = showPleaseSelect;
			}

			public Dropbox(string name, bool isRequired) : this(name, isRequired, isRequired) { }

			public Dropbox(string name, bool isRequired, bool showPleaseSelect)
				: base(name, isRequired) {
				if (showPleaseSelect) options.Add("", SelectText);
				_showPleaseSelect = showPleaseSelect;
			}

			/// <summary>
			/// Defaults value from the request using Web.RequestEx[fieldname]
			/// </summary>
			public Dropbox(string name, HttpRequest request, bool isRequired) : this(name, Web.Request[name], isRequired, isRequired) { }
			public Dropbox(string name, Web.RequestExCollection request, bool isRequired) : this(name, Web.Request[name], isRequired, isRequired) { }

			public Dropbox(string name, string value, bool isRequired) : this(name, value, isRequired, isRequired) { }

			public Dropbox(string name, string value, bool isRequired, bool showPleaseSelect)
				: base(name, value, isRequired) {
				if (showPleaseSelect) options.Add("", SelectText);
				_showPleaseSelect = showPleaseSelect;
			}

			public Dropbox(string name, int? value, bool isRequired) : this(name, value, isRequired, isRequired) { }

			public Dropbox(string name, int? value, bool isRequired, bool showPleaseSelect)
				: base(name, value + "", isRequired) {
				if (showPleaseSelect) options.Add("", SelectText);
				_showPleaseSelect = showPleaseSelect;
			}
			//public Dropbox(IActiveRecordList recordlist, string name, string value, bool isRequired, bool showPleaseSelect) : base(name, value, isRequired) 
			//{
			//  if(showPleaseSelect)options.Add("", SelectText);
			//  foreach(ActiveRecord li in recordlist)
			//  {
			//    options.Add(li[li.GetPrimaryKeyName().],li.GetFieldByIndex(1).ValueObject.ToString());
			//  }
			//  _showPleaseSelect=showPleaseSelect;
			//}

			public Dropbox(Sql sql, string name, string value, bool isRequired, bool showPleaseSelect)
				: base(name, value, isRequired) {
				if (showPleaseSelect) options.Add("", SelectText);
				options.Add(sql);
				_showPleaseSelect = showPleaseSelect;
			}
			//public Dropbox(string name, string value, bool isRequired, bool showPleaseSelect) : base(name, value, isRequired) 
			//{
			//  if(showPleaseSelect)options.Add("", SelectText);
			//  options.Add(sql);

			//  _showPleaseSelect=showPleaseSelect;
			//}

			//public Dropbox(string name, string value, bool isRequired, bool showPleaseSelect) : base(name, value, isRequired) 
			//{
			//  if(showPleaseSelect)options.Add("", SelectText);
			//  //options.Add(sql);

			//  _showPleaseSelect=showPleaseSelect;
			//}
			//private bool _AddExistingNameValueToOptionsIfNotInList=false;
			//public bool AddExistingNameValueToOptionsIfNotInList
			//{
			//  get { return _AddExistingNameValueToOptionsIfNotInList; }
			//  set { _AddExistingNameValueToOptionsIfNotInList=value; }
			//}

			public Dropbox AddExistingNameValueToOptionsIfNotInList(string value, string textValue) {
				bool found = false;
				foreach (var item in options) {
					if (item.Value == value && item.Text == textValue) {
						found = true;
						break;
					}
				}
				if (!found && textValue.IsNotBlank()) options.Add(value, textValue);
				return this;
			}
			public Dropbox(Sql sql, string name, System.Web.UI.WebControls.TextBox webformsValueTextboxControl, bool isRequired, bool showPleaseSelect)
				: base(name, webformsValueTextboxControl.Text, isRequired) {
				if (showPleaseSelect) options.Add("", SelectText);
				options.Add(sql);
				//this.onchange = "$('"+webformsValueTextboxControl.ClientID+"').text($('"+name+"').text())";
				this.onchange = "document.getElementById('" + webformsValueTextboxControl.ClientID + "').value=document.getElementById('" + name + "').options[document.getElementById('" + name + "').selectedIndex].value";
				_showPleaseSelect = showPleaseSelect;
				//this.onchange = "alert('hey')";
			}
			/// <summary>
			/// draw a dropbox using table[0] from a dataset as the data source
			/// </summary>
			/// <param name="FieldName"></param>
			/// <param name="dataSet"></param>
			/// <param name="value"></param>
			/// <param name="isRequired"></param>
			public Dropbox(string fieldName, System.Data.DataSet dataSet, string value, bool isRequired)
				: base(fieldName, value, isRequired) {
				//this.fieldName = fieldName; 
				this.Add(dataSet);
			}

			// methods

			/// <summary>
			/// Adds an Option with the given value and same text. 
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <param name="value"></param>
			/// <returns>Returns the Dropbox object, to allow chaining</returns>
			public Dropbox Add(string value) {
				options.Add(value);
				return this;
			}

			public Dropbox Add(SelectOptions.Option newOption) {
				options.Add(newOption);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <param name="value"></param>
			/// <returns>Returns the Dropbox object, to allow chaining</returns>
			public Dropbox Add(string value, string text) {
				options.Add(value, text);
				return this;
			}

			/// <summary>
			/// Adds Options from a SQL query, where the first field is the value and second field is the text.
			/// The second field is optional.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <returns>Returns the Dropbox object, to allow chaining</returns>
			public Dropbox Add(Sql sql) {
				options.Add(sql);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// Adds options in the provided collection and uses the properties defined in valueProperty and textProperty as the value and text.
			/// </summary>
			/// <returns>Returns the Dropbox object, to allow chaining</returns>
			public Dropbox Add(IEnumerable collection, string valueProperty, string textProperty) {
				options.Add(collection, valueProperty, textProperty);
				return this;
			}

			public Dropbox AddHierarchy(List<ActiveRecord> records) {
				return AddHierarchy(records, null, null);
			}

			public Dropbox AddHierarchy(List<ActiveRecord> records, string parentFieldName) {
				return AddHierarchy(records, parentFieldName, null);
			}

			public Dropbox AddHierarchy(List<ActiveRecord> records, string parentFieldName, int? maxDepth) {
				return AddHierarchy(records, parentFieldName, maxDepth, null);
			}

			public Dropbox AddHierarchy(List<ActiveRecord> records, string parentFieldName, int? maxDepth, Predicate<ActiveRecord> lambda) {
				if (records != null && records.Count > 0) {
					if (parentFieldName + "" == "") {
						// guess parent field name
						parentFieldName = "Parent" + records[0].GetTableName() + "ID";
						if (!records[0].FieldExists(parentFieldName)) {
							parentFieldName = "ParentID";
						}
						if (!records[0].FieldExists(parentFieldName)) {
							throw new ProgrammingErrorException("AddHierarchy: cannot find parent field");
						}
					}
					var pageHierarchy = new List<ActiveRecord>();
					foreach (var page in records) {
						if (page[parentFieldName] == null || page[parentFieldName].ToInt() == 0) {
							// top level page
							AddItemAndChildren(pageHierarchy, page, 0, records, parentFieldName, maxDepth);
						}
					}
					//options.Add(pageHierarchy);

				}
				return this;
			}

			private void AddItemAndChildren(List<ActiveRecord> hierarchy, ActiveRecord parentPage, int depth, List<ActiveRecord> allPages, string parentFieldName, int? maxDepth) {
				if (maxDepth != null && depth > maxDepth) {
					return;
				}
				if (!BindTo.Record.IsNewRecord) {
					if (BindTo.Record.ID_Field.ToString() == parentPage.ID_Field.ToString() && BindTo.Record.GetTableName() == parentPage.GetTableName()) {
						// cannot select own self or a child of self (this only applies if the hierarchy is self referential, ie the same table)
						return;
					}
				}
				//hierarchy.Add(parentPage);
				options.Add(parentPage.ID_Field.ToString(), new string('-', depth) + new string('-', depth) + " " + parentPage.GetName());
				foreach (var checkPage in allPages) {
					if (checkPage[parentFieldName].ToString() == parentPage.ID_Field.ToString()) {
						var childPage = checkPage;
						AddItemAndChildren(hierarchy, childPage, depth + 1, allPages, parentFieldName, maxDepth);
					}
				}
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <returns>Returns the Dropbox object, to allow chaining</returns>
			public Dropbox Add(IEnumerable collection) {
				options.Add(collection);
				return this;
			}
			public override string GetHtml() {
				//IgnoreMaxLength();
				var html = new HtmlTag("select");
				AppendHtmlAttributes(html);
				HtmlTag currentOptGroup = null;
				//MN 20131015 - removed this so it just outputs the dropbox with name etc as normal (without any options in it if there are none)
				//if (options.Count == 0 || (options.Count == 1 && this._showPleaseSelect)) return (noitems != "") ? "<select><option>" + noitems + "</option></select>" : "dropbox - no options supplied";
				foreach (SelectOptions.Option entry in options) {
					if (entry.IsOptGroup) {
						//opt group
						currentOptGroup = new HtmlTag("optgroup");
						currentOptGroup.Add("label", entry.Text);
						html.AddTag(currentOptGroup);
					} else {
						//option
						var optionTag = new HtmlTag("option");
						optionTag.Add("value", entry.Value);
						// MN 20140513 - moved this first as exact value should alwasys work
						if (entry.Value != null && entry.Value.Trim() == this.value) {
							optionTag.Add("selected", "selected");
						} else if (this.value != null && this.value.Contains(",")) {
							var multiOptions = this.value.Split(",");
							foreach (var multiOption in multiOptions) {
								if (entry.Value != null && entry.Value.Trim() == multiOption) {
									optionTag.Add("selected", "selected");
								}
							}
						}
						if (entry.Disabled) {
							optionTag.Add("disabled", "disabled");
						}
						optionTag.SetInnerText(entry.Text);
						if (currentOptGroup != null) {
							currentOptGroup.AddTag(optionTag);
						} else {
							html.AddTag(optionTag);
						}
					}
				}
				if (FixIOs7SelectBoxByUsingAnOptgroup) {//fix for ios when dropbox has really wide content: http://stackoverflow.com/questions/19398154/how-to-fix-truncated-text-on-select-element-on-ios7
					html.AddTag(new HtmlTag("optgroup").Add("label", ""));
				}
				return html.ToString();
			}

			/*
						public Dropbox AddRelevantList() {
							// determine type of list from parameter
							Type listType;
							if (this.BindTo==null) throw new Exception("AddRelevantList only works when the Dropbox parameter is an ActiveField. It can be a list field or an ID field that matches a list field.");
							if (this.BindTo.IsNumericField && this.BindTo.Name.EndsWith("ID")) {
								var record = BindTo.Record;
								.Fields[this.BindTo.Name.RemoveSuffix("ID")];
								listType = this.BindTo.Name.RemoveSuffix("ID");
							}
							IActiveRecordList list = ;
							options.Add(list);
						}
			*/

			/// <summary>
			/// create items from option [0] of the default view of the dataset passed in
			/// </summary>
			/// <param name="dataSet"></param>
			public void Add(DataSet dataSet) {
				this.dataSet = dataSet;
				//foreach (DataRow dr in dataSet.Tables[0].Rows)
				if (dataSet.Tables[0].DefaultView == null) throw new Exception();
				foreach (DataRowView dr in dataSet.Tables[0].DefaultView) {
					options.Add(dr[0].ToString(), dr[0].ToString());
				}
			}

			/// <summary>
			/// Add numbers to a dropbox
			/// </summary>
			public Dropbox AddRange(int start, int count) {
				options.AddRange(start, count);
				return this;
			}

		}

		//public class RadioGroup : SavvyBaseField
		//{

		//}

		public class TextArea : SavvyBaseField {
			public int rows { get; set; }
			public int cols { get; set; }
			/// <summary>
			/// set the max length allowed in the field
			/// </summary>
			public int? maxlength { get; set; }
			public bool ShowMaxLengthIndicator = true;

			/// <summary>
			/// Provides a text hint in the field until you click in there, then it disappears.
			/// This is built into HTML5 in modern browsers eg Chrome.
			/// For older browsers requires inclusion of jquery-placeholder-1.0.1.js
			/// </summary>
			public string placeholder;

			public TextArea(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { }
			public TextArea(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { }

			// methods
			public override string GetHtml() {
				var html = new HtmlTag("textarea");
				cssClass = (cssClass.IsBlank()) ? "svyWideText" : cssClass;
				if (rows == 0) {
					rows = 3;
				}
				if (cols == 0) {
					cols = 40;
				}
				string textAreaContents = value;
				base.value = "";
				ExtraAttribs = new { rows = rows, cols = cols };
				//allowMaxlength = false;
				html.AddIfNotBlank("placeholder", placeholder);
				if (placeholder.IsNotBlank()) AppendCssClass("placeholder");

				AppendHtmlAttributes(html);
				html.SetInnerHtml(textAreaContents);
				html.AddEndTag = true;
				string script = "";
				if (BindTo != null && maxlength == null && BindTo.MaxLength < 4000) {
					maxlength = BindTo.MaxLength;
				}
				if (this.maxlength.HasValue) {    //  && ShowMaxLengthIndicator 20110729 MN - if not ShowMaxLengthIndicator we still should check length
					script = "$('#" + this.name + "').bind('keyup change',function(obj){return textboxMultilineMaxNumber($('#" + this.name + "')[0]," + this.maxlength + "," + ShowMaxLengthIndicator.ToString().ToLower() + ")});";
				}
				if (script != "") { script = "<span id='maxlen_" + this.name + "' class='maxlencounter'></span><script language='javascript'>" + script + "</script>"; }

				return html.ToString() + "\n" + script;
			}
		}

		public enum RichTextEditorOptions {TinyMCE, Redactor };
		public static RichTextEditorOptions DefaultRichTextEditor = RichTextEditorOptions.TinyMCE;   // this is the default for backwards compatablity - override in BewebCoreSettings

		public class RichTextEditor : SavvyBaseField {
			protected string _buttonList = Util.GetSetting("tinyMCEButtons", "formatselect,bold,italic,hr,blockquote,bullist,numlist,indent,outdent,|,undo,redo,|,anchor,link,image,|,paste,pastetext,pasteword");
			protected string _buttonList2 = Util.GetSetting("tinyMCEButtons2", "");
			protected string _buttonList3 = Util.GetSetting("tinyMCEButtons3", "");
			
			public bool allowValidate { get; set; }
			public string mceClassName { get; set; }
			public bool disableRichText { get; set; }
			public int rows { get; set; }
			System.Web.UI.Page curPage;

			protected int _contentWidth = Util.GetNamedSetting("MCEUploadedImageWidth", "200").ConvertToInt();
			protected int _uploadedImageHeight = Util.GetNamedSetting("MCEUploadedImageHeight", "1000").ConvertToInt();

			/// <summary>
			/// Specify content width (ie max image width). This defaults to web config setting MCEUploadedImageWidth.
			/// </summary>
			public int ContentWidth {
				get { return _contentWidth; }
				set { _contentWidth = value; }
			}

			/// <summary>
			/// Specify complete list of buttons (comma separated button MCE codes).
			/// Reference button list is at: http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
			/// </summary>
			public string ButtonList { get; set; }

			/// <summary>
			/// Specify just any extra buttons in the button list (comma separated button MCE codes). Will use the standard button list plus this lot.
			/// Reference button list is at: http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
			/// </summary>
			public string ExtraButtons { get; set; }
			public string DisplayMode { get; set; }



			public RichTextEditor(System.Web.UI.Page page, ActiveFieldBase activeField, bool isRequired)
				: base(activeField, isRequired) {
				this.curPage = page;
			}
			public RichTextEditor(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { }
			public RichTextEditor(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { }
			public RichTextEditor() {
			}
			// methods
			public override string GetHtml() {
				allowValidate = true;
				disableRichText = false;
				mceClassName = "mceEditor";
				//InitMCE(curPage);

				if (rows == 0)
					rows = 5;
				string textAreaContents = value;
				//ExtraAttribs = new { rows = rows };
				// allowMaxlength = false;
				cssClass = (cssClass.IsBlank()) ? "" : cssClass;
				string htmlText = string.Format(@"
<span class=""mceLoader"" id=""mceLoader_{1}"" style=""width:550px;height:120px;text-align:center;""><img src=""{0}admin/images/spinner.gif"" alt=""Loading Rich Text Editor""/><br>Loading Rich Text Editor</span>
<span class=""mceWrapper"" id=""mceWrapper_{1}"" style=""display:none;""><textarea name=""{1}"" rows=""{2}"" cols=""60"" id=""{1}"" {4}>{3}</textarea></span>", Web.Root, this.name, rows, textAreaContents, ExtraAttribs);
				//string htmlText = string.Format(@"<textarea name=""{1}"" rows=""{2}"" cols=""60"" id=""{1}"" class=""mceEditor"">{3}</textarea>",Web.ResolveUrl("~"),this.name,rows,textAreaContents);

				base.value = "";
				return htmlText;
			}

			/// <summary>
			/// pages that use mce should call this (once)
			/// </summary>
			/// <param name="p"></param>
			public void InitMCE(System.Web.UI.Page p) {
				InitMCE(p, Util.IncludeRenderMode.Auto);
			}

			/// <summary>
			/// pages that use mce should call this (once)
			/// </summary>
			/// <param name="p"></param>
			public void InitMCE(System.Web.UI.Page p, Util.IncludeRenderMode includeRenderMode) {
				string tinyMcePath = Util.GetSetting("tinyMCEPath", Web.Root + "js/tiny_mce_3_2_6");
				Util.IncludeJavascriptFile(p, tinyMcePath + "/plugins/paste/js/beweb_smartpaste.js", includeRenderMode);
				// MN 20120426 moved mce loading show/hide
				// prepare the button list, complete button list is at: http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
				if (ButtonList.IsNotBlank()) _buttonList = ButtonList;		// override all buttons
				//string buttonList = "formatselect,bold,italic,underline,hr,bullist,numlist,|,undo,redo,|,link,image,|,paste,pastetext,pasteword";
				//string buttonList = "paste,pastetext,pasteword,separator,formatselect,bold,italic,underline,hr,bullist,numlist,|,undo,redo,|,link,image";
				//buttonList += ",|,forecolor,backcolor,fontselect,fontsizeselect";
				//buttonList += ",tablecontrols";
				if (ExtraButtons.IsNotBlank()) {
					ExtraButtons = ExtraButtons.TrimStart(',').TrimEnd(',');
					_buttonList += ",|," + ExtraButtons;
				}
				if (Util.IsDevAccess() && _buttonList.Split(',').DoesntContain("code")) _buttonList += ",|,code";
				if (Security.IsSuperAdminAccess && _buttonList.Split(',').DoesntContain("code")) _buttonList += ",|,code";

				// the @ sign allows string over multiple lines
				/*
				advhr
				advimage
				advlink
				autosave
				bbcode
				compat2x
				contextmenu
				directionality
				emotions
				example
				fullpage
				fullscreen
				iespell
				inlinepopups
				insertdatetime
				layer
				media
				nonbreaking
				noneditable
				pagebreak
				paste
				preview
				print
				safari
				save
				searchreplace
				spellchecker
				style
				table
				template
				visualchars
				xhtmlxtras
			*/

				/*plugins : "table,save,advhr,advimage,advlink,emotions,iespell,insertdatetime,preview,zoom,media,searchreplace,print,contextmenu,paste,directionality,fullscreen",*/
				/*plugins : "table,save,advhr,advimage,advlink,iespell,insertdatetime,searchreplace,print,contextmenu,paste",*/
				/*theme_advanced_buttons1_add_before : "save,newdocument,separator",*/
				/*theme_advanced_buttons1_add : "fontselect,fontsizeselect",*/
				/*theme_advanced_buttons2_add : "separator,insertdate,inserttime,preview,zoom,separator,forecolor,backcolor",*/
				/*theme_advanced_buttons2_add_before: "cut,copy,paste,pastetext,pasteword,separator,search,replace,separator",	*/
				/*theme_advanced_buttons3_add_before : "tablecontrols,separator",																								*/
				/*theme_advanced_buttons3_add : "emotions,iespell,media,advhr,separator,print,separator,ltr,rtl,separator,fullscreen",*/

				// Change between the minified and non-minified version depending on enviroment
				string mceIncludeFile = !Util.ServerIsLive ? Web.ResolveUrl(tinyMcePath + "/tiny_mce_src.js") : Web.ResolveUrl(tinyMcePath + "/tiny_mce.js");
				string mceStylesheet = Util.GetSetting("tinyMCEStylesheets", "~/site.css");
				mceStylesheet = mceStylesheet.Replace(",", "%2C");   // commas are valid in URLs but tinyMCE will split on them so we need to urlencode them
				var splitStylesheets = mceStylesheet.Split('|');
				string stylesheets = "";
				foreach (var stylesheet in splitStylesheets) {
					if (stylesheet.Trim().IsBlank()) continue; //skip blank sheets
					if (stylesheets != "") stylesheets += ",";
					stylesheets += Util.FileUrlWithFingerprint(stylesheet);
				}
				string plugins =Util.GetSetting("tinyMCEPlugins", "(default)");
				plugins = plugins.Replace("(default)", "table,save,advhr,advimage,advlink,insertdatetime,searchreplace,print,contextmenu,safari,paste,attachment");
				 //"AtD,table,save,advhr,advimage,advlink,insertdatetime,searchreplace,print,safari,paste,attachment,AtD,autolink,lists,pagebreak,style,layer,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave";
				string pluginPath = Web.MapPath(tinyMcePath+"/plugins");
				var subdirs = Directory.GetDirectories(pluginPath);
				string pluginsAvailable = subdirs.Join(",").Remove(pluginPath).Remove("\\");
				if (plugins == "(auto)") {
					plugins = pluginsAvailable;
				} else {
					// check plugins are all there
					
				}

				string height = Util.GetSetting("tinyMCEHeight","");//500
				string extraOptions = Util.GetSetting("tinyMCEExtraOptions", "");
				string fontSize = Util.GetSetting("tinyMCEFontSize", ""); //"13";
				string fontSizes = Util.GetSetting("tinyMCEFontSizes", "");//"8=8pt,10=10pt,12=12pt,13=13pt,14=14pt,16=16pt,18=18pt,20=20pt,22=22pt,24=24pt,26=26pt,28=28pt,30=30pt,32=32pt,34=34pt,36=36pt";
				string fonts = Util.GetSetting("tinyMCEFonts", "");// "";
				string spellCheck = "AtD";
				
				if (spellCheck=="AtD") {
					spellCheck = @"		atd_button_url: '"+Web.Root+@"js/tiny_mce_3_4_9/plugins/atd/atdbuttontr.gif',
						atd_rpc_url : '"+Web.Root+@"SpellCheck?url=',
						atd_rpc_id : '20fa1249-ac79-4007-887d-8fe5e5a04aeb',
						atd_css_url: '"+Web.Root+@"js/tiny_mce_3_4_9/plugins/atd/css/content.css',
						atd_show_types : 'Bias Language,Cliches,Complex Expression,Diacritical Marks,Double Negatives,Hidden Verbs,Jargon Language,Passive voice,Phrases to Avoid,Redundant Expression',
						atd_ignore_strings : 'AtD,Pengellys',
						atd_ignore_enable: false,
						gecko_spellcheck: false,";
				}
				
				string script = @"<script language='javascript' type='text/javascript'>
	tinyMCE.init({
		mode : 'textareas' ,		 /*'none' or 'textareas'*/
		theme : 'advanced',
		plugins :'"+plugins+@"',
		height: '"+height+@"',
		theme_advanced_runtime_fontsize : '"+fontSize+@"',
		theme_advanced_fonts : '"+fonts+@"',
		theme_advanced_font_sizes : '"+fontSizes+@"',
		"+extraOptions+@"
		"+spellCheck+@"
		theme_advanced_buttons1 : '"+_buttonList+@"',			// button rows 1, 2 and 3
		theme_advanced_buttons2 : '"+_buttonList2+@"',					// clear other rows to make them not appear
		theme_advanced_buttons3 : '"+_buttonList3+@"',
		theme_advanced_blockformats : 'p,h2,h3,h4,h5,h6',
		theme_advanced_toolbar_location : 'top',
		theme_advanced_toolbar_align : 'left',
		theme_advanced_statusbar_location : 'bottom',
		tab_focus : ':prev,:next',						// make sure the tab order works as you'd expect
		editor_selector : '"+mceClassName+@"',
		editor_deselector : 'svyWideText',
		content_css : '"+stylesheets+@"',
		theme_advanced_resizing: true,
		theme_advanced_resizing_use_cookie : false,  // dont keep remembering size forever
		theme_advanced_resize_horizontal : true,
		table_styles : 'Show Borders=table-gridlines;Invisible Gridlines=table-no-gridlines;Small Text=table-small',
		table_cell_styles : 'Header 1=header1;Header 2=header2;Header 3=header3;Table Cell=tableCel1',
		table_row_styles : 'Header 1=header1;Header 2=header2;Header 3=header3;Table Row=tableRow1',
		remove_script_host : false,
		extended_valid_elements : 'iframe[align|class|frameborder|height|id|longdesc|marginheight|marginwidth|name|scrolling|src|style|title|width]',   
		relative_urls : false,/*store full urls rather than relative to the location of mce. These paths must be replaced on live*/
		paste_use_dialog : false,
		paste_auto_cleanup_on_paste : true,
		paste_convert_headers_to_strong : false,
		paste_convert_middot_lists: true,
		paste_strip_class_attributes : 'all',
		paste_use_dialog : false,
		paste_remove_spans : true,
		paste_remove_styles : true,
		init_instance_callback : function(inst) {
			$('.mceLoader').hide();
			$('.mceWrapper').css('display','inline-block');   // we need this wrapper because display:none on the textarea caused MCE to hide the iframe too
			document.getElementById(inst.editorId+'_tbl').style.width='100%';  // make it expand to the width of the wrapper - to set a width, use css on the wrapper
		},
		setupcontent_callback: function(editor_id, body, doc) {
			body.className = 'mceContentBody normal';      // add normal here, to make it easier for standard styles to automatically reflect in rich text editor
			if(typeof cf_tinyMCEinit == 'function')cf_tinyMCEinit(editor_id);
		},
		setup: function (ed) {									//20130730jn added to get virt scroll bar when html:overflow hidden is set in included stylesheet
			ed.onInit.add(function (ed, e) {
				$(ed.getDoc()).children().find('head').append('<style type=\'text/css\'>html { overflow-x:hidden;overflow-y:scroll; }</style>');
			});

			ed.onPaste.add(function(ed, e) {
				if (window.SavvySmartPasteWatcher) SavvySmartPasteWatcher(ed,e);
			});
   
		},

		/*paste_block_drop : true,*/
		paste_retain_style_properties:'',
		paste_preprocess : function(pl, o) {
			// Content string containing the HTML from the clipboard
			o.content = PastePreProcess(o.content);
		},
		paste_postprocess : function(pl, o) {
			// Content DOM node containing the DOM structure of the clipboard
			//alert(o.node.innerHTML);
			o.node.innerHTML = PastePostProcess(o.node.innerHTML);
		},
		force_br_newlines : false,
		force_p_newlines : true,
		forced_root_block : '', // Needed for 3.x
		save_callback :  function(element_id, html, body){
			html = html.replace(/<p><\/p>/g,'<p><br /></p>');
			html = html.replace(/<p>&nbsp;<\/p>/g,'<p><br /></p>');
			return html;
		} 
		"+validHTML()+@"
	});
</script>";
																						 
				if (!disableRichText) {
					Util.IncludeJavascriptFile(p, mceIncludeFile, includeRenderMode);   // do usual cache bust - ie include ?v=xxx after which tinyMCE passes thru to all other js files
					Util.IncludeJavascript(p, script, false, includeRenderMode);
				}
			}

			protected string validHTML() {
				var setting = Util.GetSetting("tinyMCEHtmlValidation","default");
				if (setting=="none") {
					return @",verify_html : false,
									schema: 'html5'";
				} else if (setting=="html5") {
					return @",verify_html : true,
									schema: 'html5'";
        } 
				return @",verify_html : true,	
                  valid_elements : ''
									+'a[accesskey|charset|class|coords|dir<ltr?rtl|href|hreflang|id|lang|name'
										+'|onblur|onclick|ondblclick|onfocus|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|rel|rev'
										+'|shape<circle?default?poly?rect|style|tabindex|title|target|type],'
									+'abbr[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'acronym[class|dir<ltr?rtl|id|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'address[class|align|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'applet[align<bottom?left?middle?right?top|alt|archive|class|code|codebase'
										+'|height|hspace|id|name|object|style|title|vspace|width],'
									+'area[accesskey|alt|class|coords|dir<ltr?rtl|href|id|lang|nohref<nohref'
										+'|onblur|onclick|ondblclick|onfocus|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup'
										+'|shape<circle?default?poly?rect|style|tabindex|title|target],'
									+'article[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'aside[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'audio[autoplay|class|controls|dir<ltr?rtl|id|lang|loop|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|preload|src|style'
										+'|title],'
									+'base[href|target],'
									+'basefont[color|face|id|size],'
									+'bdo[class|dir<ltr?rtl|id|lang|style|title],'
									+'big[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'blockquote[dir|style|cite|class|dir<ltr?rtl|id|lang|onclick|ondblclick'
										+'|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout'
										+'|onmouseover|onmouseup|style|title],'
									+'body[alink|background|bgcolor|class|dir<ltr?rtl|id|lang|link|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onload|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|onunload|style|title|text|vlink],'
									+'br[class|clear<all?left?none?right|id|style|title],'
									+'button[accesskey|class|dir<ltr?rtl|disabled<disabled|id|lang|name|onblur'
										+'|onclick|ondblclick|onfocus|onkeydown|onkeypress|onkeyup|onmousedown'
										+'|onmousemove|onmouseout|onmouseover|onmouseup|style|tabindex|title|type'
										+'|value],'
									+'canvas[class|dir<ltr?rtl|height|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title|width],'
									+'caption[align<bottom?left?right?top|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'center[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'cite[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'code[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'col[align<center?char?justify?left?right|char|charoff|class|dir<ltr?rtl|id'
										+'|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown'
										+'|onmousemove|onmouseout|onmouseover|onmouseup|span|style|title'
										+'|valign<baseline?bottom?middle?top|width],'
									+'colgroup[align<center?char?justify?left?right|char|charoff|class|dir<ltr?rtl'
										+'|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown'
										+'|onmousemove|onmouseout|onmouseover|onmouseup|span|style|title'
										+'|valign<baseline?bottom?middle?top|width],'
									+'command[class|dir<ltr?rtl|disabled|icon|id|label|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title|type],'
									+'datalist[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'dd[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title],'
									+'del[cite|class|datetime|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'details[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|open|style'
										+'|title],'
									+'dfn[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'dir[class|compact<compact|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'div[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'dl[class|compact<compact|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'dt[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title],'
									+'em/i[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'embed[class|dir<ltr?rtl|height|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|src|style'
										+'|title|type|width|height|flashvars|allowfullscreen],'
									+'fieldset[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'figcaption[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'figure[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'footer[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'font[class|color|dir<ltr?rtl|face|id|lang|size|style|title],'
									+'form[accept|accept-charset|action|class|dir<ltr?rtl|enctype|id|lang'
										+'|method<get?post|name|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|onreset|onsubmit'
										+'|style|title|target],'
									+'frame[class|frameborder|id|longdesc|marginheight|marginwidth|name'
										+'|noresize<noresize|scrolling<auto?no?yes|src|style|title],'
									+'frameset[class|cols|id|onload|onunload|rows|style|title],'
									+'h1[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'h2[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'h3[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'h4[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'h5[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'h6[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'head[dir<ltr?rtl|lang|profile],'
									+'header[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'hgroup[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'hr[align<center?left?right|class|dir<ltr?rtl|id|lang|noshade<noshade|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|size|style|title|width],'
									+'html[dir<ltr?rtl|lang|version],'
									+'iframe[align|class|frameborder|height|id'
										+'|longdesc|marginheight|marginwidth|name|scrolling<auto?no?yes|src|style'
										+'|title|width],'
									+'img[align<bottom?left?middle?right?top|alt|border|class|dir<ltr?rtl|height'
										+'|hspace|id|ismap<ismap|lang|longdesc|name|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|src|style|title|usemap|vspace|width],'
									+'input[accept|accesskey|align<bottom?left?middle?right?top|alt|autocomplete|autofocus'
										+'|checked<checked|class|dir<ltr?rtl|disabled<disabled|form|id|ismap<ismap|lang|list'
										+'|max|maxlength|min|name|onblur|onclick|ondblclick|onfocus|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|onselect'
										+'|pattern|placeholder|readonly<readonly|required<required|size|src|style|tabindex|title'
										+'|type<button?checkbox?file?hidden?image?password?radio?reset?submit?text'
										+'?datetime?datetime-local?date?month?time?week?number?range?email?url?search?tel?color'
										+'|usemap|value],'
									+'ins[cite|class|datetime|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'isindex[class|dir<ltr?rtl|id|lang|prompt|style|title],'
									+'kbd[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'keygen[autofocus|challenge|class|dir<ltr?rtl|disabled<disabled|form|id|keytype|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'label[accesskey|class|dir<ltr?rtl|for|id|lang|onblur|onclick|ondblclick'
										+'|onfocus|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout'
										+'|onmouseover|onmouseup|style|title],'
									+'legend[align<bottom?left?right?top|accesskey|class|dir<ltr?rtl|id|lang'
										+'|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'li[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title|type'
										+'|value],'
									+'link[charset|class|dir<ltr?rtl|href|hreflang|id|lang|media|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|rel|rev|style|title|target|type],'
									+'map[class|dir<ltr?rtl|id|lang|name|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'mark[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'menu[class|compact<compact|dir<ltr?rtl|id|label|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title|type],'
									+'meta[content|dir<ltr?rtl|http-equiv|lang|name|scheme],'
									+'meter[class|dir<ltr?rtl|high|id|lang|low|max|min|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|optimum|style'
										+'|title|value],'
									+'nav[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'noframes[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'noscript[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'object[align<bottom?left?middle?right?top|archive|border|class|classid'
										+'|codebase|codetype|data|declare|dir<ltr?rtl|height|hspace|id|lang|name'
										+'|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|standby|style|tabindex|title|type|usemap'
										+'|vspace|width],'
									+'ol[class|compact<compact|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|start|style|title|type],'
									+'optgroup[class|dir<ltr?rtl|disabled<disabled|id|label|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'option[class|dir<ltr?rtl|disabled<disabled|id|label|lang|onclick|ondblclick'
										+'|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout'
										+'|onmouseover|onmouseup|selected<selected|style|title|value],'
									+'output[class|dir<ltr?rtl|for|form|id|lang|name|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'p[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|style|title],'
									+'param[id|name|type|value|valuetype<DATA?OBJECT?REF],'
									+'pre/listing/plaintext/xmp[align|class|dir<ltr?rtl|id|lang|onclick|ondblclick'
										+'|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout'
										+'|onmouseover|onmouseup|style|title|width],'
									+'progress[class|dir<ltr?rtl|id|lang|max|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title|value],'
									+'q[cite|class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'rp[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'rt[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'ruby[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'s[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title],'
									+'samp[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'script[charset|defer|language|src|type],'
									+'section[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'select[class|dir<ltr?rtl|disabled<disabled|id|lang|multiple<multiple|name'
										+'|onblur|onclick|ondblclick|onfocus|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|size|style'
										+'|tabindex|title],'
									+'small[class|dir<ltr?rtl|id|lang|media|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'source[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|src|style'
										+'|title|type],'
									+'span[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'strike[class|class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title],'
									+'strong/b[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'style[dir<ltr?rtl|lang|media|title|type],'
									+'sub[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'summary[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|open|style'
										+'|title],'
									+'sup[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title],'
									+'table[align<center?left?right|bgcolor|border|cellpadding|cellspacing|class'
										+'|dir<ltr?rtl|frame|height|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|rules'
										+'|style|summary|title|width],'
									+'tbody[align<center?char?justify?left?right|char|class|charoff|dir<ltr?rtl|id'
										+'|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown'
										+'|onmousemove|onmouseout|onmouseover|onmouseup|style|title'
										+'|valign<baseline?bottom?middle?top],'
									+'td[abbr|align<center?char?justify?left?right|axis|bgcolor|char|charoff|class'
										+'|colspan|dir<ltr?rtl|headers|height|id|lang|nowrap<nowrap|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|rowspan|scope<col?colgroup?row?rowgroup'
										+'|style|title|valign<baseline?bottom?middle?top|width],'
									+'textarea[accesskey|class|cols|dir<ltr?rtl|disabled<disabled|id|lang|name'
										+'|onblur|onclick|ondblclick|onfocus|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|onselect'
										+'|readonly<readonly|rows|style|tabindex|title],'
									+'tfoot[align<center?char?justify?left?right|char|charoff|class|dir<ltr?rtl|id'
										+'|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown'
										+'|onmousemove|onmouseout|onmouseover|onmouseup|style|title'
										+'|valign<baseline?bottom?middle?top],'
									+'th[abbr|align<center?char?justify?left?right|axis|bgcolor|char|charoff|class'
										+'|colspan|dir<ltr?rtl|headers|height|id|lang|nowrap<nowrap|onclick'
										+'|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove'
										+'|onmouseout|onmouseover|onmouseup|rowspan|scope<col?colgroup?row?rowgroup'
										+'|style|title|valign<baseline?bottom?middle?top|width],'
									+'thead[align<center?char?justify?left?right|char|charoff|class|dir<ltr?rtl|id'
										+'|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown'
										+'|onmousemove|onmouseout|onmouseover|onmouseup|style|title'
										+'|valign<baseline?bottom?middle?top],'
									+'time[class|datetime|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|pubdate|style'
										+'|title],'
									+'title[dir<ltr?rtl|lang],'
									+'tr[abbr|align<center?char?justify?left?right|bgcolor|char|charoff|class'
										+'|rowspan|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title|valign<baseline?bottom?middle?top],'
									+'tt[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title],'
									+'u[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup'
										+'|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title],'
									+'ul[class|compact<compact|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown'
										+'|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover'
										+'|onmouseup|style|title|type],'
									+'var[class|dir<ltr?rtl|height|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style],'
									+'video[autoplay|class|controls|dir<ltr?rtl|id|lang|loop|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|preload|poster|src|style'
										+'|title|width|height],'
									+'wbr[class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress'
										+'|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style'
										+'|title]'";
			}
		}

		public class TimeField : InputField {
			public TimeField(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { init(); }

			private void init() {
				ShowPicker = true;
				EarliestHour = 0;
				LatestHour = 23;
			}
			public TimeField(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { init(); }
			public TimeField(string fieldName, DateTime? value, bool isRequired) : base(fieldName, Fmt.Date(value), isRequired) { init(); }

			public bool ShowPicker { get; set; }
			public int? EarliestHour { get; set; }
			public int? LatestHour { get; set; }
			public string DefaultTime { get; set; }
			internal DateTimeField DateTimeField { get; set; }

			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.time;
				} else {
					this.type = InputTypes.text;
				}
				if (value.IsNotBlank()) {
					DateTime d = DateTime.Parse(value);
					if (Html5) {
						this.value = d.ToString("HH:mm");
					} else {
						this.value = Fmt.Time(d, Fmt.DateTimePrecision.Minute);
					}
				}
				var html = new HtmlTag("input");
				html.Add("type", this.type.ToString());

				IgnoreMaxLength();
				AppendCssClass("time");
				AppendCssClass("svyTimeInput");
				if (Html5) {
					if (EarliestHour != null) {
						html.Add("min", EarliestHour + ":00");
					}
					if (LatestHour != null) {
						html.Add("min", LatestHour + ":00");
					}
					if (DefaultTime != null && this.value == null) {
						this.value = DateTime.Parse(DefaultTime).ToString("HH:mm");
					}
				} else {
					html.AddIfNotBlank("data-earliest-hour", EarliestHour + "");
					html.AddIfNotBlank("data-latest-hour", LatestHour + "");
					html.AddIfNotBlank("data-default-time", DefaultTime);
				}
				AppendHtmlAttributes(html);
				if (DateTimeField != null) {
					DateTimeField.AppendHtmlAttributes(html);
				}
				if (ShowPicker) {
					html.AddTag(new HtmlTag("img").Add("src", Web.Root + "images/clock.png").Add("class", "ClockPick_button").Add("onmouseover", "InitClockPick(this)"));
					Util.IncludeJavascriptFile("~/js/jquery.clockpick/jquery.clockpick.1.2.9BW.js");
					Util.IncludeStylesheetFile("~/js/jquery.clockpick/jquery.clockpick.1.2.9BW.css");
				}
				return html.ToString();
			}
		}

		public enum DateSelectorOptions { JQueryUI, MonthYear, MonthYearCombined, DayMonthYear, Html5 };
		public static DateSelectorOptions DefaultDateSelector = DateSelectorOptions.JQueryUI;   // this is the default for backwards compatablity - override in BewebCoreSettings

		public static bool UseUniqueRadioIDs = false;    // this is the default for backwards compatablity - override in BewebCoreSettings

		/// <summary>
		/// usage: 
		/// =new Forms.DateField("fieldname","value",false)
		/// =new Forms.DateField("fieldname","value",false){DisplayMode=Forms.DateSelectorOptions.MonthYear} 
		/// </summary>
		public class DateField : InputField {
			public DateField(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { init(); }

			private void init() {
				daySelectText = "Day";
				monthSelectText = "Month";
				yearSelectText = "Year";
				DisplayMode = DefaultDateSelector;
				ReverseYears = false;
			}

			public DateField(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { init(); }
			public DateField(string fieldName, DateTime? value, bool isRequired) : base(fieldName, Fmt.Date(value), isRequired) { init(); }

			public DateSelectorOptions DisplayMode { get; set; }
			public bool ReverseYears { get; set; }
			private int? _earliestYear;
			public int? EarliestYear {
				get { return !_earliestYear.HasValue ? DateTime.Now.Year - 50 : _earliestYear.Value; }		 //default to 50 years ago, unless earliest year specified specifically
				set { _earliestYear = value; }
			}

			private int? _latestYear = DateTime.Now.Year + 1;
			public int? LatestYear {
				get { return _latestYear; }
				set { _latestYear = value; }
			}

			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.date; //new input type="date" - wait for html 5 - using this causes problems for chrome with jquery ui datepicker and validate with the beweb date format
				} else {
					this.type = InputTypes.text;
				}
				if (value.IsNotBlank()) {
					try {
						DateTime d = DateTime.Parse(value);
						if (Html5) {
							this.value = d.ToString("yyyy-MM-dd");
						} else {
							this.value = d.ToString("d MMM yyyy");
						}
					} catch (FormatException) {
						//failed
					}
				}
				if (DisplayMode == DateSelectorOptions.MonthYear || DisplayMode == DateSelectorOptions.DayMonthYear) {
					if (!EarliestYear.HasValue) EarliestYear = DateTime.Now.Year - 50;
					if (!LatestYear.HasValue) LatestYear = DateTime.Now.Year + 1;
					string years = "";
					DateTime? dateValObj = null;
					if (value.IsNotBlank()) {
						dateValObj = DateTime.Parse(value);

					}
					if (dateValObj.HasValue && (dateValObj.Value.Year < EarliestYear.Value || dateValObj.Value.Year > LatestYear.Value)) {
						// add currently selected value since it is not in the year range but it is presumably still a valid option since it was previously selected
						years += "<option value=\"" + dateValObj.Value.Year + "\" selected>" + dateValObj.Value.Year + "</option>";
					}
					if (!ReverseYears) {
						for (int yr = EarliestYear.Value; yr <= LatestYear.Value; yr++) {
							string sel = (dateValObj.HasValue && dateValObj.Value.Year == yr) ? " selected" : "";
							years += "<option" + sel + " value=\"" + yr + "\" >" + yr + "</option>";
						}
					} else {
						for (int yr = LatestYear.Value; yr >= EarliestYear.Value; yr--) {
							string sel = (dateValObj.HasValue && dateValObj.Value.Year == yr) ? " selected" : "";
							years += "<option" + sel + " value=\"" + yr + "\" >" + yr + "</option>";
						}
					}
					var months = GetMonthOptions((dateValObj.HasValue) ? dateValObj.Value.Month : -1);
					string htmlDateSelector = "";
					string isFullDateJSBool = "false";
					string extraClasses = (isRequired) ? " required" : "";
					if (UseShortDate) extraClasses += " shortdateselector";
					extraClasses += " " + cssClass;  // add any custom classes
					if (DisplayMode == DateSelectorOptions.DayMonthYear) {
						isFullDateJSBool = "true";
						string days = "";
						//for(int day=dayArray.Length-1;day>=0;day--)//reverse days
						for (int day = 1; day <= 31; day++) {
							string sel = (dateValObj.HasValue && dateValObj.Value.Day == day) ? " selected" : "";
							days += "<option value=\"" + day + "\"" + sel + ">" + day + "</option>";
						}

						htmlDateSelector = string.Format(@"
	<select name=""{0}_day"" id=""{8}_day"" onchange=""handleDaySelector('{8}',{7});{6}""{9} class=""svyDate drop{4} day"">
		<option value="""">{5}</option>
		{3}
	</select>",
						this.name,
						value,
						years,
						days,
						extraClasses,
						daySelectText,
						onchange,
						isFullDateJSBool,
						this.id
						, this.readOnly ? " disabled=\"disabled\"" : ""
						);
					}

					string html = string.Format(@"
	{8}<select name=""{0}_month"" id=""{10}_month"" onchange=""handleMonthSelector('{10}',{9});{7}""{11} class=""svyDate drop{4} month"">
		<option value="""">{5}</option>
		{3}
	</select>
	<select name=""{0}_year"" id=""{10}_year"" onchange=""handleYearSelector('{10}',{9});{7}""{11} class=""svyDate drop{4} year"">
		<option value="""">{6}</option>
		{2}
	</select>
	<input type=""hidden"" name=""{0}"" id=""{10}"" value=""{1}""/>
				",
						this.name,						 //	0
						value,								 //1
						years,								 //2
						months,								 //3
						extraClasses,					 //	4
						monthSelectText,			 //	 5
						yearSelectText,				 //	6
						onchange,							 //7
						htmlDateSelector,			 //8
						isFullDateJSBool,			 //	9
						this.id,								 //	 10		
						this.readOnly ? " disabled=\"disabled\"" : ""
						);
					string script = "";
					html = "<span style=\"white-space:nowrap\">" + html + "</span>";
					//if(isRequired)script+="$(document).ready(function() {$('#"+this.name+"_year').validate({ messages: {'"+this.name+"_year': '*'}});";
					//if(script!=""){script="<script language='javascript'>"+script+"</script>";}
					if (onclick.IsNotBlank()) throw new Exception("onclick not handled, use onchange for date selector");
					return html + script;
				} else if (DisplayMode == DateSelectorOptions.MonthYearCombined) {
					if (!EarliestYear.HasValue) EarliestYear = DateTime.Now.Year - 50;
					if (!LatestYear.HasValue) LatestYear = DateTime.Now.Year + 1;
					string monthyears = "";
					DateTime? dateValObj = null;
					if (value.IsNotBlank()) {
						dateValObj = DateTime.Parse(value);
					}
					var startdate = DateTime.Parse("1 jan " + EarliestYear.Value);
					var endDate = DateTime.Parse("31 dec " + LatestYear.Value);
					if (!ReverseYears) {
						for (DateTime scanDate = startdate; scanDate.MonthDifference(endDate) > 0; ) {
							string sel = (dateValObj.HasValue && dateValObj.Value.GetFollowingMonthBegin() == scanDate) ? " selected" : "";
							monthyears += "<option" + sel + " value=\"1 " + scanDate.FmtMonthYear() + "\" >" + scanDate.FmtMonthYear() + "</option>";
							scanDate = scanDate.AddMonths(1);
						}
					} else {
						//todo reverse
						//for (int yr = LatestYear.Value; yr >= EarliestYear.Value; yr--) {
						//	string sel = (dateValObj.HasValue && dateValObj.Value.Year == yr) ? " selected" : "";
						//	monthyears += "<option" + sel + " value=\"" + yr + "\" >" + yr + "</option>";
						//}
					}

					string htmlDateSelector = "";
					string isFullDateJSBool = "false";
					string extraClasses = (isRequired) ? " required" : "";
					extraClasses += " " + cssClass;  // add any custom classes

					string html = string.Format(@"
	{8}<select name=""{0}_monthyear"" id=""{10}_monthyear"" onchange=""handleMonthYearSelector('{10}',{9});{7}""{11} class=""svyDate drop{4} monthyear"">
		<option value="""">{6}</option>
		{2}
	</select>
	<input type=""hidden"" name=""{0}"" id=""{10}"" value=""{1}""/>
				",
						this.name,						 //	0
						value,								 //1
						monthyears,								 //2
						null,								 //3
						extraClasses,					 //	4
						monthSelectText,			 //	 5
						yearSelectText,				 //	6
						onchange,							 //7
						htmlDateSelector,			 //8
						isFullDateJSBool,			 //	9
						this.id								 //	 10		
						, this.readOnly ? " disabled=\"disabled\"" : ""
						);
					string script = "";
					html = "<span style=\"white-space:nowrap\">" + html + "</span>";
					//if(isRequired)script+="$(document).ready(function() {$('#"+this.name+"_year').validate({ messages: {'"+this.name+"_year': '*'}});";
					//if(script!=""){script="<script language='javascript'>"+script+"</script>";}
					if (onclick.IsNotBlank()) throw new Exception("onclick not handled, use onchange for date selector");
					return html + script;


				} else if (DisplayMode == DateSelectorOptions.JQueryUI) {
					var html = new HtmlTag("input");
					html.Add("type", this.type.ToString());

					// normal date field - text entry with calendar onfocus
					IgnoreMaxLength();
					AppendCssClass("date");
					AppendCssClass("svyDateInput");
					var script = "";
					string extraOpts = "";
					/*
					if(EarliestYear.HasValue){extraOpts+=",minDate:new Date("+EarliestYear+",1-1,1)";}
					if(LatestYear.HasValue){extraOpts+=",maxDate:new Date("+LatestYear+",12-1,31)";}
					script	+="\n$('input.date').datepicker({ showAnim: 'fold',yearRange: '-100:+100',changeYear:'true',changeMonth:'true',dateFormat: 'd M yy'"+extraOpts+"});";;
					if(EarliestYear.HasValue)script	+="\n$('input.date').datepicker({minDate: new Date("+EarliestYear+", 1 - 1, 1)  });";
					if(LatestYear.HasValue)script	+="\n$('input.date').datepicker({maxDate: new Date("+LatestYear+", 1 - 1, 1)  });";
					if(EarliestYear.HasValue&&LatestYear.HasValue)script	+="\n$('input.date').datepicker({yearRange: '"+EarliestYear+":"+LatestYear+"' });";
					if(script!=""){script="<script language='javascript'>"+script+"</script>";}
					
					return base.GetHtml()+script;
					*/
					script += "\n$('input.date').datepicker({ showAnim: 'fold',changeYear:'true',changeMonth:'true',dateFormat: 'd M yy'" + extraOpts + "});"; ;
					if (EarliestYear.HasValue) {
						script += "\n$('input.date').datepicker({minDate: new Date(" + EarliestYear + ", 1 - 1, 1)  });";
						html.AddIfNotBlank("earliestyear", EarliestYear.Value + "");
					}
					if (LatestYear.HasValue) {
						script += "\n$('input.date').datepicker({maxDate: new Date(" + LatestYear + ", 1 - 1, 1)  });";
						html.AddIfNotBlank("latestyear", LatestYear.Value + "");
					}
					if (EarliestYear.HasValue && LatestYear.HasValue) script += "\n$('input.date').datepicker({yearRange: '" + EarliestYear + ":" + LatestYear + "' });";
					if (script != "") { script = "$(document).ready(function () { " + script + "});"; }
					if (script != "") { script = "<script language='javascript'>" + script + "</script>"; }

					AppendHtmlAttributes(html);
					string result = "<span class=\"datetimewrap\">" + html.ToString() + "<span class=\"dateerror\"></span></span>";
					if (BindTo != null && BindTo.IsDateField && ((BindTo.Record.Advanced.ExpiryDateField != null || BindTo.Record.Advanced.PublishDateField != null) && (BindTo.Name == BindTo.Record.Advanced.ExpiryDateField.Name || BindTo.Name == BindTo.Record.Advanced.PublishDateField.Name)) && BindTo.Record.Advanced.ExpiryDatesHaveTimes && !(this is DateTimeField)) {
						var time = new TimeField("timefield_" + BindTo.Name, BindTo.ToString(), isRequired);
						result += time.GetHtml();
					}
					return result;
				} else if (DisplayMode == DateSelectorOptions.Html5) {
					var html = new HtmlTag("input");
					html.Add("type", this.type.ToString());
					// todo - not sure what attribs to set by default - subject to change
					IgnoreMaxLength();
					//AppendCssClass("date");
					//AppendCssClass("svyDateInput");
					AppendHtmlAttributes(html);
					return html.ToString();
				} else {
					throw new Exception("you need to specify the datepicker type");
				}
			}

			public static string GetMonthOptions(int selectedMonth) {
				string months = "";
				var monthArray = GetMonthArray();
				for (int mon = 0; mon < monthArray.Length; mon++) {
					string sel = (selectedMonth != 0 && selectedMonth - 1 == mon) ? " selected" : ""; //20130816jn set to zero
					months += "<option value=\"" + monthArray[mon] + "\"" + sel + ">" + monthArray[mon] + "</option>";
				}
				return months;
			}

			public static string GetMonthOptions(string selectedMonth) {
				string months = "";
				var monthArray = GetMonthArray();

				//for(int mon=monthArray.Length-1;mon>=0;mon--)//reverse months
				for (int mon = 0; mon < monthArray.Length; mon++) {
					string sel = (selectedMonth == monthArray[mon]) ? " selected" : "";
					months += "<option value=\"" + monthArray[mon] + "\"" + sel + ">" + monthArray[mon] + "</option>";
				}
				return months;
			}

			private static string[] GetMonthArray() {
				//string[] monthArray = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
				if (System.Globalization.DateTimeFormatInfo.CurrentInfo == null) {
					throw new Exception("System.Globalization.DateTimeFormatInfo.CurrentInfo missing");
				}
				//string[] monthArray = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
				string[] monthArray = System.Globalization.DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
				return monthArray;
			}

			public string daySelectText { get; set; }
			public string monthSelectText { get; set; }

			public string yearSelectText { get; set; }

			/// <summary>
			/// when using day  mon yyyy, show date as dd mmm yy
			/// </summary>
			public bool useShortDate = false;
			public bool UseShortDate {
				get { return useShortDate; }
				set {
					useShortDate = value;
					if (value) {
						daySelectText = "dd";
						monthSelectText = "mm";
						yearSelectText = "yyyy";
					} else {
						init();
					}
				}
			}
		}


		public enum DateTimeSelectorOptions { DateAndTimeInputs, SingleInput };
		public static DateTimeSelectorOptions DefaultDateTimeSelector = DateTimeSelectorOptions.SingleInput;   // this is the default for backwards compatablity - override in BewebCoreSettings

		/// <summary>
		/// Render a date field with a time field
		/// </summary>
		public class DateTimeField : DateField {
			public DateTimeField(ActiveFieldBase activeField, bool isRequired)
				: base(activeField, isRequired) {
				Time = new TimeField("timefield_" + activeField.Name, activeField.ToString(), isRequired);
				Time.DateTimeField = this;
			}
			public DateTimeField(string fieldName, string value, bool isRequired)
				: base(fieldName, value, isRequired) {
				Time = new TimeField("timefield_" + fieldName, value, isRequired);
				Time.DateTimeField = this;
			}

			public TimeField Time;
			public DateTimeSelectorOptions DisplayMode = DefaultDateTimeSelector;

			public bool IsTimeRequired {
				get { return Time.IsRequired; }
				set { Time.IsRequired = value; }
			}
			public string DefaultTime {
				get { return Time.DefaultTime; }
				set { Time.DefaultTime = value; }
			}
			public string TimeCssClass {
				get { return Time.cssClass; }
				set { Time.cssClass = value; }
			}
			public bool ShowTimePicker {
				get { return Time.ShowPicker; }
				set { Time.ShowPicker = value; }
			}
			public override bool Html5 {
				get { return base.Html5; }
				set {
					DisplayMode = DateTimeSelectorOptions.DateAndTimeInputs;
					if (value) {
						base.DisplayMode = DateSelectorOptions.Html5;
						Time.ShowPicker = false;
					}
					//ShowValidation = !value;
					Time.Html5 = value;
					base.Html5 = value;
				}
			}
			public override string GetHtml() {
				if (Html5) {
					this.type = InputTypes.date; //new input type="date" - html 5 - using this causes problems for chrome with jquery ui datepicker and validate with the beweb date format
				} else {
					this.type = InputTypes.text;
				}
				string html = null;
				if (DisplayMode == DateTimeSelectorOptions.DateAndTimeInputs) {
					// time goes in separate field - use jQuery UI for the date and ClockPick for the time
					this.value = Fmt.Date(value);
					IgnoreMaxLength();
					//AppendCssClass("date");
					//AppendCssClass("svyDateInput");  -- these are set in the base, which is now DateField
					html = base.GetHtml();
					html += Time.GetHtml();
				} else {
					// combined date and time field - uses jQuery UI date and related timepicker
					if (IsTimeRequired) {
						// always show time, even if 0:00
						this.value = Fmt.Date(value) + " " + Fmt.Time(value);
					} else {
						// only shows time if there is one
						this.value = Fmt.DateTime(value);
					}
					ExtraAttribs = new DateTimeExtraAttribs() { showTime = "true" };
					IgnoreMaxLength();
					AppendCssClass("datetime");
					html = base.GetHtml();
				}

				return html;
			}

			public class DateTimeExtraAttribs {
				public string showTime { get; set; }
			}
		}

		/*
				public class DateTimeField : InputField {
					public DateTimeField(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { }
					public DateTimeField(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { }
					public SingleInputField = false;

					public override string GetHtml() {
						//this.type = InputTypes.datetime;
						this.type = InputTypes.text; //see datefield comment
						if (value.IsNotBlank()) {
							DateTime d = DateTime.Parse(value);
							this.value = d.ToString("d MMM yyyy hh:mm") + d.ToString(" t").ToLower() + "m";
						}

						ExtraAttribs = new DateTimeExtraAttribs() { showTime = "true" };
						IgnoreMaxLength();
						AppendCssClass("datetime");
						return base.GetHtml();


					}

					public class DateTimeExtraAttribs {
						public string showTime { get; set; }
					}
				}
				*/

		public class YesNoField : SavvyBaseField {

			public enum YesNoStyle { Style1, Style2 }
			public static YesNoStyle CurrentStyle = YesNoStyle.Style2;
			public string label { get; set; }
			/// <summary>
			/// true if you want to store the value in a hidden, not in the 'checked' attr in the radios. good to use when radios are in a colorbox with ie7
			/// </summary>
			public bool UseHiddenValue { get; set; }
			/// <summary>
			/// if usehiddenvalue is true, when true, put the onclick code against each radio to change the hidden. If false, use the jquery bind method in a script block under the radios
			/// </summary>
			protected bool UseInlineScript { get; set; }

			/// <summary>
			/// Allows you to set the value to something else eg passing in an active field, but overwrite it's value
			/// </summary>
			public bool? ValueOverride { set { this.value = value + ""; } }

			public YesNoField(ActiveFieldBase activeField) : base(activeField, true) { }
			public YesNoField(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { }
			public YesNoField(string fieldName, bool isRequired) : base(fieldName, isRequired) { }
			public YesNoField(string fieldName, bool? value, bool isRequired) {
				this.isRequired = isRequired;
				this.name = fieldName;
				this.id = fieldName;
				this.value = (value == null) ? null : value.Value.ToString();
			}

			// methods
			public override string GetHtml() {
				return (CurrentStyle == YesNoStyle.Style1) ? GetHtmlStyle1() : GetHtmlStyle2();
			}

			// style1 - may be better as it assigns separate IDs to Yes and No radios, which is strictly correct and jquery needs it
			private string GetHtmlStyle1() {
				AppendCssClass("yesno");
				bool? isChecked = (value.IsBlank()) ? null : (bool?)Convert.ToBoolean(value);

				// don't render value attribute
				ClearValueAttribute();

				AppendCssClass("YNRadio");
				var radio0 = new HtmlTag("input");
				string attributeValue0 = "yesno_" + name + "_False";
				radio0.Add("id", attributeValue0);
				radio0.Add("type", "radio");
				radio0.Add("value", "0");

				if (isChecked != null && !isChecked.Value)
					radio0.Add("checked", "checked");
				if (UseHiddenValue) {
					radio0.Add("name", "yesno_" + name);
					if (UseInlineScript) radio0.Add("onclick", "$(\'#" + name + "\').val(\'0\');", false);
				}
				AppendHtmlAttributes(radio0, false);

				// true - index 1
				var radio1 = new HtmlTag("input");
				string attributeValue1 = "yesno_" + name + "_True";
				radio1.Add("id", attributeValue1);
				radio1.Add("type", "radio");
				radio1.Add("value", "1");

				if (isChecked != null && isChecked.Value)
					radio1.Add("checked", "checked");
				if (UseHiddenValue) {
					radio1.Add("name", "yesno_" + name);
					if (UseInlineScript) radio1.Add("onclick", "$(\'#" + name + "\').val(\'1\');", false);
				}

				AppendHtmlAttributes(radio1, false);
				string html = "";
				if (label != "") {
					html = "" + radio1.ToString() + "<label id=\"" + name + "_True" + "\" for=\"" + "yesno_" + name + "_True" + "\"> Yes </label> &nbsp;&nbsp; ";
					html += "" + radio0.ToString() + "<label id=\"" + name + "_False" + "\" for=\"" + "yesno_" + name + "_False" + "\"> No</label> ";
				} else {
					html = "" + radio0.ToString() + "\n" + radio1.ToString() + "  ";
				}

				if (label.IsNotBlank()) { html += "- " + label.HtmlEncode(); }

				if (UseHiddenValue) {
					html += new Forms.HiddenField(name, (isChecked.HasValue) ? isChecked.Value ? "1" : "0" : "").GetHtml();
					if (!UseInlineScript) {
						html += "\n<script type=\"text/javascript\">\n$('#" + attributeValue0 + "').bind('click',function(){$('#" + name + "').val('0');});\n";
						html += "$('#" + attributeValue1 + "').bind('click',function(){$('#" + name + "').val('1');});\n</script>";
					}
				}

				return html;
			}
			private string GetHtmlStyle2() {
				AppendCssClass("yesno");
				bool? isChecked = (value.IsBlank()) ? null : (bool?)Convert.ToBoolean(value);

				// don't render value attribute
				ClearValueAttribute();

				var radio1 = new HtmlTag("input");
				if (UseUniqueRadioIDs) radio1.Add("id", name + "_True");
				radio1.Add("type", "radio");
				radio1.Add("value", "True");

				if (isChecked != null && isChecked.Value)
					radio1.Add("checked", "checked");
				AppendHtmlAttributes(radio1);

				var radio0 = new HtmlTag("input");
				if (UseUniqueRadioIDs) radio0.Add("id", name + "_False");
				radio0.Add("type", "radio");
				radio0.Add("value", "False");
				if (isChecked != null && !isChecked.Value)
					radio0.Add("checked", "checked");
				AppendHtmlAttributes(radio0);

				string html;
				if (UseUniqueRadioIDs) {
					html = "<label id=" + name + "_TrueLabel" + " for=" + name + "_True>" + radio1.ToString() + " Yes </label> &nbsp;&nbsp; ";
					html += "<label id=" + name + "_FalseLabel" + " for=" + name + "_False>" + radio0.ToString() + " No</label> ";
				} else {
					html = "<label id=" + name + "_True" + ">" + radio1.ToString() + " Yes </label> &nbsp;&nbsp; ";
					html += "<label id=" + name + "_False" + ">" + radio0.ToString() + " No</label> ";
				}

				if (label.IsNotBlank()) { html += "- " + label.HtmlEncode(); }

				return html;
			}

		}

		public class Radio : InputField {
			public string label { get; set; }
			protected string selectedValue { get; set; }
			protected string radioValue { get; set; }
			public Radio(ActiveFieldBase activeField, string radioValue, bool isRequired) : base(activeField, isRequired) { this.selectedValue = activeField.ToString(); this.radioValue = radioValue; }
			public Radio(string fieldName, string radioValue, bool isRequired) : base(fieldName, radioValue.ToString(), isRequired) { this.selectedValue = null; this.radioValue = radioValue; }
			public Radio(string fieldName, string radioValue, string selectedValue, bool isRequired) : base(fieldName, radioValue.ToString(), isRequired) { this.selectedValue = selectedValue; this.radioValue = radioValue; }

			public override string GetHtml() {
				IgnoreMaxLength();
				var html = new HtmlTag("input");
				html.Add("type", "radio");
				//if(this.value!=null)
				{
					if ((selectedValue + "").ToLower() == (this.radioValue + "").ToLower()) {
						html.Add("checked", "checked"); // check this
					}
				}
				// bug fixed. Previously confusing, now clearer.. (jb + MN)
				this.id += "_" + Fmt.PascalCase(Fmt.Crunch(radioValue));
				this.value = radioValue;
				AppendHtmlAttributes(html);
				string htmlString = html.ToString();
				if (label.IsNotBlank()) {
					htmlString = "<label>" + htmlString + label + "</label> ";
				}
				return htmlString;
			}
		}

		public class Radios : SavvyBaseField {
			private SelectOptions options = new SelectOptions();
			private bool _showLabels = true;
			/// <summary>
			/// Defaults to true = wrap each radio in a SPAN tag to provide flexibility in CSS. 
			/// Set this to false if you don't want the span tags.
			/// </summary>
			private bool WrapInSpans = true;
			public string DescriptionFieldName { get; set; }
			public enum RadioFormat { Spans, Divs, Naked }
			public RadioFormat Format { get; set; }

			public Radios(ActiveFieldBase activeField, bool isRequired) : base(activeField, isRequired) { }
			public Radios(string fieldName, bool isRequired) : base(fieldName, isRequired) { }
			public Radios(string fieldName, string value, bool isRequired) : base(fieldName, value == null ? "" : value.ToString(), isRequired) { }

			/// <summary>
			/// Adds an Option with the given value and same text. 
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <param name="value"></param>
			/// <returns>Returns the Radios object, to allow chaining</returns>
			public Radios Add(string value) {
				options.Add(value);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <returns>Returns the Radios object, to allow chaining</returns>
			public Radios Add(string value, string text) {
				options.Add(value, text);
				return this;
			}

			/// <summary>
			/// Adds Options from a SQL query, where the first field is the value and second field is the text.
			/// The second field is optional.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <returns>Returns the Radios object, to allow chaining</returns>
			public Radios Add(Sql sql) {
				options.Add(sql);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <param name="collection"></param>
			/// <returns>Returns the Radios object, to allow chaining</returns>
			public Radios Add(IEnumerable collection) {
				options.Add(collection);
				return this;
			}

			// methods
			public override string GetHtml() {
				//IgnoreMaxLength();
				AppendCssClass("radio-list");
				string html = "";
				if (BindTo != null) {
					id = BindTo.Name;
					name = BindTo.Name;
				}
				//BindTo = null;
				int counter = 1;
				foreach (SelectOptions.Option entry in options) {
					id = (name + "__" + Fmt.PascalCase(Fmt.Crunch(entry.Value).Replace("-", " ")));

					var radio = new HtmlTag("input")
						.Add("type", "radio")
						.Add("value", entry.Value);

					if (this.value == entry.Value)
						radio.Add("checked", "checked");

					AppendHtmlAttributes(radio);

					var label = new HtmlTag("label")
						.Add("for", id)
						.SetInnerText(entry.Text);

					var innerHtml = "";
					if (this.ShowLabels) {
						innerHtml = radio.ToString() + " " + label.ToString()/* +" "+ label.ToString()*/;
					} else {
						innerHtml = radio.ToString();
					}

					//if (WrapInSpans) {
					if (WrapInSpans || Format == RadioFormat.Spans || Format == RadioFormat.Divs) {
						var span = new HtmlTag(WrapInSpans || Format == RadioFormat.Spans ? "span" : "div").Add("class", "savvyRadioList");
						//.SetInnerHtml(label.ToString()+" "+radio.ToString()/* +" "+ label.ToString()*/);
						if (counter == options.Count)
							span.Add("id", name + "_Last");
						span.SetInnerHtml(innerHtml);
						html += span.ToString();
					} else {
						html += innerHtml;
					}
					counter++;
				}

				return html;
			}

			public bool ShowLabels {
				get {
					return _showLabels;
				}
				set {
					_showLabels = value;
				}
			}
		}

		/// <summary>
		/// Render a set of related checkboxes
		/// </summary>
		public class Checkboxes : SavvyBaseField {

			//private IActiveRecordList values;
			private IEnumerable values;
			private SelectOptions options = new SelectOptions();
			//private IActiveRecordList options;

			public string DescriptionFieldName { get; set; }

			public string ForeignKey { get; set; }


			private int? _scrollHeight = null;

			/// <summary>
			/// Height in pixels of the wrapper around the checkboxes. If there are more checkboxes than will fit in this height, it will scroll vertically.
			/// Set this to null or 0 to disable the fixed height and scrolling. Default is null.
			/// </summary>
			public int? ScrollHeight {
				get { return _scrollHeight; }
				set { _scrollHeight = value; }
			}

			/// <summary>
			/// Outputs a list of related checkboxes. 
			/// For use when you don't have ActiveRecords for values and options.
			/// You can add the options using the .Add() method.
			/// </summary>
			/// <param name="inputFieldName"></param>
			/// <param name="selectedValues"></param>
			/// <param name="foreignKey"></param>
			public Checkboxes(string inputFieldName, IEnumerable<int> selectedValues, string foreignKey) {
				this.name = inputFieldName;
				Init(selectedValues);
				if (foreignKey == null) throw new Exception("Form.Checkboxes: foreignKey is null.");
				this.ForeignKey = foreignKey;
			}

			/// <summary>
			/// Outputs a list of related checkboxes. 
			/// For use when you don't have ActiveRecords for values and options.
			/// You can add the options using the .Add() method.
			/// </summary>
			/// <param name="inputFieldName"></param>
			/// <param name="selectedValues"></param>
			/// <param name="foreignKey"></param>
			public Checkboxes(string inputFieldName, IEnumerable<string> selectedValues, string foreignKey) {
				this.name = inputFieldName;
				Init(selectedValues);
				if (foreignKey == null) throw new Exception("Form.Checkboxes: foreignKey is null.");
				this.ForeignKey = foreignKey;
			}

			/// <summary>
			/// Outputs a list of related checkboxes. No values preselected.
			/// You can add the options using the .Add() method.
			/// </summary>
			public Checkboxes(string inputFieldName) {
				this.name = inputFieldName;
			}

			/// <summary>
			/// Outputs a list of related checkboxes. 
			/// selectedValues is an ActiveRecordList that contains the selected IDs that shows which checkboxes should be checked (assumes foreign key field is named according to convention).
			/// You can add the options using the .Add() method.
			/// </summary>
			/// <param name="selectedValues"></param>
			/// <param name="foreignKey"></param>
			public Checkboxes(IActiveRecordList selectedValues, string foreignKey) {
				this.name = selectedValues.GetTableName();
				Init(selectedValues);
				if (foreignKey == null) throw new Exception("Form.Checkboxes: foreignKey is null.");
				this.ForeignKey = foreignKey;
			}

			/// <summary>
			/// Outputs a list of related checkboxes. 
			/// selectedValues is an ActiveRecordList that contains the selected IDs that shows which checkboxes should be checked (assumes foreign key field is named according to convention).
			/// availableOptions is the list of all checkboxes (uses ID and name)
			/// </summary>
			/// <param name="selectedValues"></param>
			/// <param name="availableOptions"></param>
			public Checkboxes(IActiveRecordList selectedValues, IActiveRecordList availableOptions)
				: this(selectedValues, availableOptions, null) {
			}

			/// <summary>
			/// Outputs a list of related checkboxes. 
			/// selectedValues is an ActiveRecordList that contains the selected IDs that shows which checkboxes should be checked (assumes foreign key field is named according to convention).
			/// availableOptions is the list of all checkboxes (uses ID and name)
			/// </summary>
			/// <param name="selectedValues"></param>
			/// <param name="availableOptions"></param>
			/// <param name="dataColName"> if not null, add a data-meta="val" attrib to each checkbox where val is the value from the col named (must be in available options)</param>
			public Checkboxes(IActiveRecordList selectedValues, IActiveRecordList availableOptions, string dataColName) {
				this.name = selectedValues.GetTableName();
				Init(selectedValues);
				DetermineForeignKey(selectedValues, availableOptions);
				this.options.Add(availableOptions, dataColName);
			}

			private void DetermineForeignKey(IActiveRecordList selectedValues, IActiveRecordList availableOptions) {
				ForeignKey = availableOptions.GetPrimaryKeyName();
				if (!selectedValues.FieldExists(ForeignKey)) {
					ForeignKey = availableOptions.GetTableName() + "ID";
				}
				if (!selectedValues.FieldExists(ForeignKey)) {
					throw new Exception("Form.Checkboxes: foreign key field not found [" + ForeignKey + "]");
				}
			}


			private void Init(IEnumerable selectedValues) {
				this.values = selectedValues;
				this.isRequired = false;
				this.cssClass = "";
				this.options = new SelectOptions();
			}

			/// <summary>
			/// Adds an Option with the given value and same text. 
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <param name="value"></param>
			/// <returns>Returns the Checkboxes object, to allow chaining</returns>
			public Checkboxes Add(string value) {
				options.Add(value);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <returns>Returns the Checkboxes object, to allow chaining</returns>
			public Checkboxes Add(string value, string text) {
				options.Add(value, text);
				return this;
			}

			/// <summary>
			/// Adds Options from a SQL query, where the first field is the value and second field is the text.
			/// The second field is optional.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <returns>Returns the Checkboxes object, to allow chaining</returns>
			public Checkboxes Add(Sql sql) {
				options.Add(sql);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// If you want to add an option with no text such as "please select", then use Add("", "Please select")
			/// </summary>
			/// <param name="value"></param>
			/// <returns>Returns the Checkboxes object, to allow chaining</returns>
			public Checkboxes Add(IEnumerable collection) {
				options.Add(collection);
				return this;
			}

			public override string GetHtml() {
				HtmlTag wrapper = new HtmlTag("div").Add("class", "svyCheckboxes");
				if (ScrollHeight != null && ScrollHeight.Value != 0) {
					wrapper.Add("style", "height:" + ScrollHeight.Value + "px; overflow-y:scroll;", false);
				}

				HtmlTag refInp = new HtmlTag("input")
					.Add("type", "hidden")
					.Add("name", name + "__reference")
					.Add("value", ForeignKey);
				//.Add("value", options.GetTableName());
				wrapper.AddTag(refInp);

				int i = 0;

				foreach (SelectOptions.Option entry in options) {
					//				foreach (ActiveRecord rec in options) {
					id = name + "__" + i;
					// reset "value" property of base, which is used in AppendHtmlAttributes to set value of current checkbox
					//					value = rec.ID_Field.ValueObject.ToString(); MN 16-Jun-2010
					value = entry.Value;
					string checkboxLabel = entry.Text;

					HtmlTag container = new HtmlTag((!UseSpan) ? "div" : "span")
						.Add("class", "checkboxes");
					if (FloatBoxesLeft) { container.Add("style", "float:left"); }
					HtmlTag inp = new HtmlTag("input")
						.Add("type", "checkbox");
					AppendHtmlAttributes(inp);
					if (entry.Data != null) inp.Add("data-meta", entry.Data); //if has data attr, add to checkbox input as data-meta="val"

					bool isChecked = false;
					if (values is IActiveRecordList) {
						isChecked = IsSelected(value);
					} else if (values is IEnumerable<int>) {
						isChecked = ((IEnumerable<int>)values).Contains(value.ToInt());
					} else {
						if (values != null) {
							isChecked = ((IEnumerable<string>)values).ContainsInsensitive(value);
						}
					}
					if (isChecked) {
						inp.Add("checked", "checked");
					}

					HtmlTag label = new HtmlTag("label")
						.Add("for", id)
						//.SetInnerText(DescriptionFieldName.IsNotBlank() ? rec.GetFieldByName(DescriptionFieldName).ValueObject.ToString() : rec.GetName()); MN 16-Jun-2010
						.SetInnerText(checkboxLabel);

					container.SetInnerHtml(inp.ToString() + label.ToString());
					wrapper.AddTag(container);
					i++;
				}

				return wrapper.ToString();
			}

			/// <summary>
			/// when drawing, checkboxes are in a dov by default. Set this to true to draw in a span instead (inline)
			/// </summary>
			public bool UseSpan { get; set; }						//default to false
			public bool FloatBoxesLeft { get; set; }						//default to false

			private bool IsSelected(string currentOptionID) {
				foreach (ActiveRecord selectedValue in values) {
					if (selectedValue.GetFieldByName(ForeignKey) != null
							&& selectedValue.GetFieldByName(ForeignKey).ToString() == currentOptionID) {
						return true;
					}
				}
				return false;
			}

			private bool IsSelected(ActiveRecord currentOptionRecord) {
				// loop through all selected values - ie records in the join table - looking for this value
				string currentOptionID = currentOptionRecord.ID_Field.ToString();
				string foreignKeyName = currentOptionRecord.ID_Field.Name;

				foreach (ActiveRecord selectedValue in values) {
					if (!selectedValue.FieldExists(foreignKeyName)) {
						foreignKeyName = currentOptionRecord.GetTableName() + "ID";
					}
					if (!selectedValue.FieldExists(foreignKeyName)) {
						throw new ActiveRecordException("Form.Checkboxes Error - Cannot locate foreign key field in selectedValue record list. Beweb convention is there should be a field [" + foreignKeyName + "] in table [" + currentOptionRecord.GetTableName() + "].");
					}

					if (selectedValue.GetFieldByName(foreignKeyName) != null
						&& selectedValue.GetFieldByName(foreignKeyName).ToString() == currentOptionID) {
						return true;
					}
				}

				return false;
			}
		}

		public class ParentPageDropbox : Dropbox {
			private ActiveField<int?> bindToActiveField;
			public ParentPageDropbox(ActiveField<int?> bindToActiveField, int maxParentDepth)
				: this(bindToActiveField, maxParentDepth, true, null) {

			}

			public ParentPageDropbox(ActiveField<int?> bindToActiveField, int maxParentDepth, bool addNoneOption, string templateCodes)
				: base(bindToActiveField, false, false) {
				if (addNoneOption) {
					Add("", "(None)");
				}
				AddChildList(null, 0, maxParentDepth, "", templateCodes);
				this.AppendCssClass("parentpage");
			}

			void AddChildList(int? parentid, int depth, int maxParentDepth, string prefix, string templateCodes) {
				Sql sql = null;
				if (depth >= maxParentDepth) return;
				ActiveRecord activeRecord = new ActiveRecord("Page", "PageID");
				activeRecord.CheckFieldsCollectionIsPopulated();
				sql = new Sql("select pageid, isnull(navtitle,title) as Title,parentpageid ");
				bool checkTemplates = false;
				if (templateCodes != null) {
					checkTemplates = BewebData.FieldExists("Page", "TemplateCode");
					if (checkTemplates) {
						sql.Add(", TemplateCode");
					}
				}
				sql.Add("from page ");

				if (parentid.HasValue) {
					sql.Add((activeRecord.GetSqlWhereActivePlusExisting(parentid)));					//includes where stmt
					sql.Add("and parentpageid=", parentid.Value.SqlizeNumber());
				} else {
					sql = new Sql("select pageid, isnull(navtitle,title) as Title,parentpageid from page ");
					sql.Add((activeRecord.GetSqlWhereActive()));					//includes where stmt
					sql.Add("and parentpageid is null");
				}
#if PageRevisions

				if (BewebData.FieldExists("Page", "HistoryPageID")) { // 20140610 mn - added!
					sql.Add("and HistoryPageID is null"); //20140519jn added!
				}
#endif

				if (!BindTo.Record.IsNewRecord && BindTo.Record.GetTableName() == "Page") {
					int ownSelfPageID = BindTo.Record.ID_Field.ToString().ToInt(0);
					sql.Add("and pageid<>", ownSelfPageID);
				}
				sql.Add("order by sortposition");
				//walk the results
				DbDataReader dbDataReader = sql.GetReader();
				foreach (DbDataRecord row in dbDataReader) {
					int pageid = (int)row["pageID"];

					bool isDisabled = false;
					if (checkTemplates) {
						isDisabled = !row["TemplateCode"].ToString().ContainsCommaSeparated(templateCodes);
					}

					var opt = new SelectOptions.Option(pageid + "", prefix + "" + row["title"] + "", null, isDisabled);
					Add(opt);

					//var children = new Sql("select count(*) from page where parentpageid=", pageid.SqlizeNumber(), " ");
					var children = new Sql("select count(*) from page ");
					children.Add(activeRecord.GetSqlWhereActive());
					children.Add("and  parentpageid=", pageid.SqlizeNumber(), " ");
#if PageRevisions
					if (BewebData.FieldExists("Page", "HistoryPageID")) { // 20140610 mn - added!
						children.Add("and HistoryPageID is null"); //20140519jn added!
					}
#endif

					if (children.FetchIntOrZero() > 0) {
						//recurse
						AddChildList(pageid, depth + 1, maxParentDepth, prefix + "----", templateCodes);
					}
				}
				dbDataReader.Close();
				dbDataReader.Dispose();

			}
		}

		/*
				[Incomplete]
				public class ParentPickerField: SavvyBaseField {
					// properties
			
					public override string GetHtml() {
						//IgnoreMaxLength();
						string v,hid,ig,bas,all;
						v = "";
						hid = "";
						ig = "";
						bas = "";
						all= "";
						var basehtml = new HtmlTag("div");
						string html = string.Format(@"
							var v = '{0}';
							var hiddenFieldClientID = '{1}';
							var ignoreID = '{2}';
							var basePath = '{3}';
							var allowedDepth = {4};	",v,hid,ig,bas,all);
						basehtml.SetInnerHtml(html);
				
						return basehtml.ToString();
					}
				}
		*/

		public class ColorPickerDisplayField : SavvyBaseField {
			// note: folder /js/jscolor/ is required
			// properties
			public ColorPickerDisplayField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }

			public override string GetHtml() {

				var basehtml = new HtmlTag("div");
				var html = new HtmlTag("div");
				if (value.IsNotBlank()) {

					html.Add("style", "width:50px;height:20px;background-color:" + value);
					html.Add("title", "" + value);
				} else {
					html.SetInnerText("no colour supplied");
				}
				base.AppendHtmlAttributes(html);
				basehtml.SetInnerHtml(html.ToString());
				return basehtml.ToString();
			}
		}

		public class ColorPickerField : SavvyBaseField {
			// note: folder /js/jscolor/ is required
			// properties
			public ColorPickerField(ActiveFieldBase bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public ColorPickerField(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { }


			public override string GetHtml() {

				var basehtml = new HtmlTag("div");
				var html = new HtmlTag("input");
				//if (cssClass.IsBlank())cssClass = "svyWideText";
				html.Add("type", "text");
				html.Add("class", "color {hash:true}");
				html.Add("value", value);
				//if (isRequired) {			// 20100503 MN prep for html 5 -- 20100513 removed this as it caused validation to break
				//  html.Add("required", "true");
				//}
				base.AppendHtmlAttributes(html);
				string html2 = string.Format(@"<script type=""text/javascript"" src=" + Web.ResolveUrl("~/js/jscolor/jscolor.js") + "></script>");
				basehtml.SetInnerHtml(html2 + html.ToString());
				return basehtml.ToString();
			}
		}
		public class PictureField : SavvyBaseField {
			public static bool UseAcceptAttrib;
			private PictureMetaDataAttribute _metaData = new PictureMetaDataAttribute() { AllowSelectFromServer = false, AllowPasteAndDrag = false, ShowDimensionMessage = false };

			// properties
			public PictureActiveField Picture { get { return (PictureActiveField)base.BindTo; } }
			public PictureMetaDataAttribute MetaData {
				get { return (Picture != null) ? Picture.MetaData : _metaData; }
				set { _metaData = value; }
			}

			public bool ShowDimensionMessage { set { MetaData.ShowDimensionMessage = value; } }
			//public int DimensionMessageWidth { set { MetaData.DimensionMessageWidth = value; } }
			//public int DimensionMessageHeight { set { MetaData.DimensionMessageHeight = value; } }
			public bool ShowCropResizeChoice { set { MetaData.ShowCropResizeChoice = value; } }
			public bool ShowFreeImageSearchLinks { set { MetaData.ShowFreeImageSearchLinks = value; } }
			public bool ShowPreviewImage { set { MetaData.ShowPreviewImage = value; } }
			public bool AllowSelectFromServer { set { MetaData.AllowSelectFromServer = value; } }
			public bool ShowRemoveCheckbox { set { MetaData.ShowRemoveCheckbox = value; } }
			public bool AllowPasteAndDrag { set { MetaData.AllowPasteAndDrag = value; } }
			public bool ShowDragButton { set { MetaData.ShowDragButton = value; } get { return MetaData.ShowDragButton; } }
			public bool ShowPasteButton { set { MetaData.ShowPasteButton = value; } get { return MetaData.ShowPasteButton; }}
			public bool AllowDownload { get; set; } //default to false
			/// <summary>
			/// Content types to allow as the 'accept' attribute in the input type=file tag (eg "image/*").
			/// Synonym for AllowedMimeTypes
			/// </summary>
			public string accept { get { return MetaData.AllowedMimeTypes; } set { MetaData.AllowedMimeTypes = value; } }
			/// <summary>
			/// Content types to allow as the 'accept' attribute in the input type=file tag (eg "image/*").
			/// Synonym for accept
			/// </summary>
			public string AllowedMimeTypes { get { return MetaData.AllowedMimeTypes; } set { MetaData.AllowedMimeTypes = value; } }

			// constructors
			public PictureField(PictureActiveField bindToActiveField, bool isRequired) : base(bindToActiveField, isRequired) { }
			public PictureField(string name, string value, bool isRequired) : base(name, value, isRequired) { }

			private HtmlTag GetUploadedPictureHtml() {
				var span = new HtmlTag("div");
				span.Add("id", "shp_" + this.name); //show picture	container

				if (Picture.FileName.StartsWith("http://") || FileSystem.FileAttachmentExists(Picture.FileName)) {

					// create file name and thumb display
					if (MetaData != null) {
						string spancontents = "";
						string fileName = Picture.FileName.RemovePrefix(MetaData.Subfolder);
						if (MetaData.ShowPreviewImage) {
							if (Web.IsAbsoluteUrl(Picture.FileName)) {
								spancontents += "<img width=\""+MetaData.ThumbnailWidth+"\" class=\"SavvyUploadPreview\" src=\"" + Picture.FileName + "\"></a>";
							} else if (FileSystem.FileExists(Picture.ImagePreviewPath)) { // MN 20130214 fixed bug - was FileAttachmentExists
								spancontents += Html.PicturePreview(Picture, fileName, "SavvyUploadPreview prv");
							} else if (FileSystem.FileExists(Picture.ImageThumbPath)) {
								spancontents += Html.PictureThumb(Picture, fileName, "SavvyUploadPreview thm");
							} else {
								spancontents += Html.Picture(Picture, fileName, "SavvyUploadPreview pic");
							}
							if (MetaData.ShowPreviewImageEnlargement) {
								spancontents = "<a href=\"" + Picture.ImagePath + "\" title=\"Click to view full size image\" class=\"colorbox\">" + spancontents + "</a>";
							}
						} else {
							spancontents += fileName;
						}
						var svyChangePictureLinkContainer = "svyChangePictureLink_" + this.FullName;
						spancontents += "<div  id = \"" + svyChangePictureLinkContainer + "\" class=\"svyPicOptions svyLinkContainer svyNotMobileScreen\"><a class='svyPasteLink btn btn-mini' href=\"\" onclick='svyChangePicture(\"" + this.FullName + "\");return false;'><i class='icon icon-picture'></i> Change picture</a></div>";
						if (MetaData.ShowRemoveCheckbox && !this.IsRequired) {
							spancontents += "<div style='display:none'><input type=\"checkbox\" id=\"" + this.name + "_remove\" name=\"" + this.name + "_remove\" title=\"Check this box to delete the picture when you click Save below.\" value=\"1\"/> Remove picture</label></div>";

							var svyRemovePictureLinkContainer = "svyRemovePictureLink_" + this.FullName;
							spancontents += "<div id = \"" + svyRemovePictureLinkContainer + "\" class='svyLinkContainer svyRemovePicture svyNotMobileScreen'>";
							spancontents += "<a class='svyPasteLink btn btn-mini' href='#' onclick='svyRemovePicture(\"" + this.FullName + "\");return false;'><i class='icon icon-trash'></i> Remove picture</a>";
							spancontents += "</div>";
						}
						if (AllowDownload && Picture.FileName.IsNotBlank() && !Picture.FileName.StartsWith("http://")) spancontents += "<br style=\"clear:both\"><a href=\"" + Web.ResolveUrl("~/attachments/") + Picture.FileName + "\" target=\"_blank\">download picture / open in new window / image link</a>";
						span.SetInnerHtml(spancontents);
					}
					//basehtml.AddTag(span);
				} else {
					//picture missing
					span.SetInnerHtml("<div class='svyError'>Picture file missing: " + Picture.FileName + "</div>");
				}
				return span;
			}

			private HtmlTag GetPictureLinksHtml(bool isExisting) {
				; //isExisting = if theres a picture
				var html = new HtmlTag("div");
				html.Add("id", "ulc_" + this.name); //uploadcontainer  - ulc picture
				html.Add("class", "svyPicContainer"); //uploadcontainer

				if (isExisting) {
					html.Add("style", "display:none;"); //if theres a picture already uploaded hide this
				}
				// create input

				html.AddRawHtml("<div class='svyLinksWrapper'>");		//open svyLinksWrapper
				GetFilePictureSelect(html);

				if (!Web.IsMobile) {
					var cancelCustomBrowseContainer = "svyCancelCustomeBrowseLink_" + this.FullName;
					html.AddRawHtml("<div  id = \"" + cancelCustomBrowseContainer + "\" class='svyHtml5 svyCancelPaste'>");
					html.AddRawHtml("<a class='svyPasteLink btn btn-mini' href='#' onclick='svyCancel(\"" + this.FullName + "\");return false;'><i class='icon icon-remove'></i> Cancel Upload</a>");
					html.AddRawHtml("</div>");
					//End common elements for upload
				}
				//Image Paste
				if (MetaData != null && (MetaData.AllowPasteAndDrag || MetaData.ForceAjax)) {
					//Until all browsers support copy and paste and drag and drop we render all the applet and html5 links and containers then let js hide or show them
					//depending on what features are supports or what broswers the client is using.

					GetMoreControlsHtml(html); // JC Refactor 20140902 changed the name because its not just on paste that this function is now called
				}

				if (MetaData != null && MetaData.AllowSelectFromServer) {
					GetSelectFileFromServer(html);
				}

				if (!Web.IsMobile) {
					if (MetaData != null) {
						ShowHelp(html);
					}
				}
				html.AddRawHtml("</div>"); //close links wrapper

				html.AddRawHtml("<div style='clear:both'></div>");

				if (!Web.IsMobile) {
					if (MetaData != null && MetaData.ShowFreeImageSearchLinks) {
						GetFreeImageLinks(html);
					}
				}

				html.AddRawHtml("<div style='clear:both;'></div>");
				if (MetaData != null) {
					ShowCropresizeChoice(html);
				}
				if (!Web.IsMobile) {
					GetCancelPicture(html);
				}
				return html;
			}

			private void GetCancelPicture(HtmlTag html) {
				var cancelPicContainer = "cancelPicture_" + this.FullName;
				html.AddRawHtml("<div  id = \"" + cancelPicContainer + "\" class='svyLinkContainer svyCancelPaste' style='display:none'></i>");
				html.AddTag(new HtmlTag("a")
				.SetInnerHtml("<i class='icon icon-remove'></i> Keep Existing Picture")
				.Add("onclick", "cancelChangePicture('" + this.name + "');return false;")
				.Add("class", "svyPicCancelChange svyPasteLink btn btn-mini")
				.Add("href", ""));
				html.AddRawHtml("</div>");
			}

			private void GetFilePictureSelect(HtmlTag html) {

				var isMobileSite = Web.IsMobile;
				const string mobileClass = "svyMobile";
				if (isMobileSite) {
					html.AddRawHtml("<button id='file_" + this.name + "Button' class='btn-primary btn btn-mobile-takepicture ' onclick='handleFileBrowse(\"" + this.FullName + "\")' type='button'>Take Picture or Choose Photo</button>");
				}

				var input = new HtmlTag("input");
				input.Add("type", "file");
				//	input.Add("class", "svyPicBrowseFile");
				AppendCssClass("svyPicBrowseFile");
				AppendCssClass("svyFileUpload");

				if (!MetaData.AllowPasteAndDrag) {
					AppendCssClass("svyFileUploadSmall");
				}

				if (Web.IsMobile) {
					AppendCssClass("svyMobileFileUpload");
				}

				if (Picture != null && FileSystem.FileAttachmentExists(Picture.FileName)) {
					AppendCssClass("svyPicUploaded");
				}
				if (MetaData.AllowPasteAndDrag || MetaData.AllowSelectFromServer) {
					AppendCssClass("svyAllowPasteAndDrag");

				} else {
					AppendCssClass("svyNoPasteAndDrag");
				}
				input.Add("id", "file_" + this.name);
				if (MetaData != null) {
					if (UseAcceptAttrib) input.AddIfNotBlank("accept", MetaData.AllowedMimeTypes);
				}
				if (!isMobileSite) {
					if (onclick.IsNotBlank()) {
						onclick += ";";
					}
					onclick += "handleFileBrowse('" + this.name + "')";
				}

				AppendHtmlAttributes(input);
				if (!isMobileSite) {
					if (onchange.IsNotBlank()) {
						onchange += ";";
					}
					onchange += "browseSelectPicture('" + this.name + "');return false;";
				}

				if (isMobileSite) {

					if (onchange.IsNotBlank()) {
						onchange += ";";
					}
					onchange += "browseMobileSelectPicture('" + this.name + "');return false;";
				}

				AppendHtmlAttributes(input);
				html.AddTag(input);

				if (!isMobileSite) {
					//if (MetaData.AllowPasteAndDrag || MetaData.AllowSelectFromServer || MetaData.ForceAjax) {
					if (true) { //always do this 
						var filesvyLinkContainer = "fileBrowseLink_" + this.FullName;
						html.AddRawHtml("<div class='svyFileNameText_" + this.FullName + " svyFileNameText'></div>");
						html.AddRawHtml("<div  id=\"" + filesvyLinkContainer + "\" class='svyLinkContainer " + mobileClass + "'>");
						//html.AddRawHtml("<a class='svyPasteLink btn btn-mini' href='#' onclick='openFileBroswer(\"" + this.FullName + "\");return false;'><i class='icon icon-file'></i> Upload an Image</a>");
						html.AddRawHtml("<a class='svyPasteLink btn btn-mini' href='#'><i class='icon icon-file'></i> Choose file</a>"); // AF20150304 - Trigger the click via javascript doesn't work in IE. Now the file input sits on top of the "Choose file" button
						html.AddRawHtml("</div>");
					}
				}

			}

			private void GetSelectFileFromServer(HtmlTag html) {
				var selectspan = new HtmlTag("span")
			.Add("class", "svyPicSelectContainer")
			.Add("id", "sel_" + this.name)
			.Add("style", "display:none;");
				var filterLabel = new HtmlTag("label").Add("class", "svyPicSelectFilterLabel");
				var filter = new HtmlTag("input")
					.Add("type", "text")
					//.Add("style","display:none")
					.Add("class", "svyPicSelectFilter")
					.Add("onkeyup", "filterServerImageList(this,'" + this.name + "');return false;");
				var preview = new HtmlTag("img")
					.Add("id", "pv_" + this.name)
					.Add("class", "svyPicSelectPreview")
					.Add("src", Web.ResolveUrl("~/admin/images/") + "testpattern.gif");
				var svrcancel = new HtmlTag("a")
					.Add("href", "#")
					.Add("class", "svyPicCancelFile svyPasteLink btn btn-mini")
					.Add("style", "display:block")
					.Add("id", "cnlsv_" + name)
					.Add("onclick", "svyCancelSelectPicture('" + this.name + "');return false;")
					.SetInnerHtml("<i class='icon icon-remove'></i> Cancel uploaded files select");

				var srvcancelwrap = new HtmlTag("span").Add("class", "svyPicCancelFileWrap");
				srvcancelwrap.AddTag(svrcancel);

				var select = new HtmlTag("select")
					.Add("name", "svyPicInactive_" + this.name + "")
					.Add("id", "select_" + this.name + "")
					//.Add("multiple","multiple") -- doesn't make sense, you can't select multiple files
					.Add("class", "svyPicSelectFile")
					.Add("size", "5")
					.Add("onchange", "handleImagePreviewSelect('" + Web.ResolveUrl("~/attachments/") + "',this,'" + MetaData.Subfolder + "');return false;", false);
				// per below - cannot use JSEnquote for the subfolder as it out puts it in double quotes.. this breaks the page.
				//.Add("onchange","handleImagePreviewSelect('"+Web.ResolveUrl("~/attachments/")+"',this,"+ MetaData.Subfolder.JsEnquote() +" "+");return false;", false);
				//.Add("onclick","handleImagePreviewSelect('"+Web.ResolveUrl("~/attachments/"+MetaData.Subfolder)+"',this);return false;");

				filterLabel.SetInnerText("Filter ");
				filterLabel.AddTag(filter);

				select.AddTag(new HtmlTag("option").SetInnerText("sample"));
				selectspan.AddTag(select);
				selectspan.AddTag(preview);
				selectspan.AddTag(filterLabel);
				selectspan.AddTag(srvcancelwrap);

				html.AddTag(selectspan);
				if (!Web.IsMobile) {
					var selectFromServerContainer = "selectFromServer_" + this.FullName;
					html.AddRawHtml("<div  id = \"" + selectFromServerContainer + "\" class='svyLinkContainer'>");
					html.AddTag(new HtmlTag("a")
						.Add("href", "")
						.Add("id", "selsv_" + name)
						.Add("class", "svyPasteLink  btn btn-mini")
						.Add("onclick", "svyHandleSelectPicture('" + Web.ResolveUrl("~/services/") + "', '" + Web.ResolveUrl("~/attachments/") + "','" + this.name + "', '" + MetaData.Subfolder + "');return false;")
						.SetInnerHtml("<i class='icon icon-picture'></i> Select uploaded image"));
					html.AddRawHtml("</div>");
				}
			}

			private void ShowCropresizeChoice(HtmlTag html) {
				var cropResizeId = "cropResize_" + this.FullName;
				html.AddRawHtml("<input type='hidden'  id = \"" + cropResizeId + "\"  id = \"" + cropResizeId + "\" value= \"" + MetaData.ShowCropResizeChoice + "\">");

				if (MetaData.ShowCropResizeChoice) {
					var scalespan = new HtmlTag("div")
						.Add("id", "scale_" + this.name)
						.Add("class", "svyLinkContainer scaleContainer");

					string str = "Resize mode: <label><input type=radio name='scale_" + this.name + "' value='Crop' checked>Photos (resize & crop)</label>";
					str += "<label><input type=radio name='scale_" + this.name + "' value='Scale'>Logos (resize only)</label>";
					scalespan.SetInnerHtml(str);
					html.AddTag(scalespan);
				}
			}

			private void ShowHelp(HtmlTag html) {
				if (MetaData.ShowDimensionMessage && !MetaData.DontResizeOriginal) {

					var svyFreeLinkContainer = "filedim_" + this.FullName;
					var dimensionsLabel = new HtmlTag("div").Add("class", "svyPicBrowseDimensions").Add("style", "width:20px;").Add("id", svyFreeLinkContainer);
					//dimensionsLabel.SetInnerText("Dimensions: " +MetaData.DimensionMessageWidth +"px by "+ MetaData.DimensionMessageHeight+"px ");
					string picInfo = "";
					if (MetaData.ShowFullDimensionMessage) {
						dimensionsLabel.SetInnerText("Dimensions: " + MetaData.DimensionMessageWidth + "px by " + MetaData.DimensionMessageHeight + "px ");
					} else {
						picInfo += "Dimensions: " + MetaData.DimensionMessageWidth + "px wide by " + MetaData.DimensionMessageHeight + "px high";
					}
					if (MetaData.IsCropped) {
						picInfo += " (or bigger)";
					}
					picInfo += "\\n\\n";
					if (MetaData.IsCropped) {
						picInfo += "Larger images will be automatically scaled down and edges cropped to fit.\\n";
					} else {
						picInfo += "Larger images will be automatically scaled down without losing any of the image.\\n";
					}
					picInfo += "\\n";
					if (MetaData.IsExact) {
						picInfo += "Smaller images will be be padded to fit.\\n";
					} else {
						picInfo += "Smaller images are OK.\\n";
					}
					picInfo += "\\n";
					if (false && MetaData.ThumbnailWidth != 0 && MetaData.ThumbnailHeight != 0) {   // MN 20140625 - never a need to show thumb size is there?
						picInfo += "Thumbnail dimensions: " + MetaData.ThumbnailWidth + "x" + MetaData.ThumbnailHeight + "\\n";
						picInfo += "Crop thumbnail: " + Fmt.YesNo(MetaData.IsThumbnailCropped) + "\\n";
						picInfo += "Force thumbnail to exact size: " + Fmt.YesNo(MetaData.IsThumbnailExact) + "\\n";
					}
					var filesAllowed = MetaData.AllowedMimeTypesForErrorMessage;
					if (filesAllowed.IsBlank()) filesAllowed = MetaData.AllowedMimeTypes.Replace("image/", "").Replace(",", ", ").ToUpper();
					picInfo += "File types allowed: " + filesAllowed + "\\n";
					var infohref = (new HtmlTag("a").Add("href", "").Add("onclick", "if(window.OpenInline){OpenInline('" + picInfo.Replace("\\n", "<br>") + "','Image Help',400,170,window.event)}else{alert('" + picInfo + "')};return false;"));
					infohref.AddTag(new HtmlTag("img").Add("src", Web.ResolveUrl("~/admin/images/help.gif")));
					dimensionsLabel.AddTag(infohref);
					html.AddTag(dimensionsLabel);
				}
			}

			private void GetFreeImageLinks(HtmlTag html) {
				var svyFreeLinkContainer = "freeImageLink_" + this.FullName;
				var freeLinkId = "freeImageLink_" + this.FullName;
				html.AddRawHtml("<div class='svyPicSectionBreak' style='margin-top:10px;' id = \"" + svyFreeLinkContainer + "\" class='svyLinkContainer'>");
				html.AddRawHtml("<input type='hidden'  id = \"" + freeLinkId + "\"  id = \"" + freeLinkId + "\" value= \"" + MetaData.ShowFreeImageSearchLinks + "\">");
				html.AddRawHtml("<span class='freeImageText'>Find free images: </span>");

				html.AddTag(new HtmlTag("a")
													.Add("href", "http://commons.wikimedia.org/wiki/Commons:Featured_pictures")
													.Add("target", "_blank")
													.Add("class", "svyPasteLink btn btn-mini")
													.Add("title", "Search for free images on Wikimedia Commons")
													.Add("style", "margin-right:10px;")
													.SetInnerHtml("<i class='icon icon-search'></i> Wikimedia Commons"));
				html.AddTag(new HtmlTag("a")
											.Add("href", "http://flickr.com/creativecommons/by-2.0/")
											.Add("target", "_blank")
											.Add("class", "svyPasteLink btn btn-mini")
											.Add("title", "Search for free images on Flickr Creative Commons search")
											.SetInnerHtml("<i class='icon icon-search'></i> Flickr"));
				html.AddRawHtml("</div>");
			}

			private void GetMoreControlsHtml(HtmlTag html) {
				//Common elements for upload
				html.AddRawHtml("<div style = 'display:none;'  class='svyFiledragProgess' id='svyImagePasteProgress_" + this.FullName + "'>");

				var spanID = "svyImagePasteSpan_" + this.FullName;
				html.AddRawHtml("<p></p>");
				html.AddRawHtml("<span id = '" + spanID + "'></span>");
				html.AddRawHtml("<input value = '' class='svyPasteHidden' id=\"paste_" + this.FullName + "\" type=hidden name=\"paste_" + this.FullName + "\">");
				html.AddRawHtml("<input value = '' class='svyPasteHidden' id=\"RealFileName_" + this.FullName + "\" type=hidden name=\"RealFileName_" + this.FullName + "\">");

				html.AddRawHtml("</div>"); //end progress div

				html.AddRawHtml("<div class='svyImagePasteContainer'></div>");
				html.AddRawHtml("<div style = 'display:none !important;' class='svyFiledragLive svyHtml5' id=\"svyFiledragLive_" + this.FullName + "\" ></div>");

				if (!Web.IsMobile && !MetaData.ForceAjax) {
					//svyHtml5 Links
					//the links to show hide the drag drop and svyHtml5 paste container
					var pasteImagesvyLinkContainer = "svyPasteImageLink_" + this.FullName;
					//var pasteAgainImagesvyLinkContainer = "svyPasteAgainImageLink_" + this.FullName;
					if (ShowPasteButton) {
						html.AddRawHtml("<div  id = \"" + pasteImagesvyLinkContainer + "\" class='svyHtml5 svyLinkContainer svyNotMobileScreen svyPaste'>");
						html.AddRawHtml("<a class='svyPasteLink btn btn-mini' href='#' onclick='svyShowPaste(\"" + this.FullName + "\");return false;'><i class='icon icon-camera'></i> Paste an image</a>");
						html.AddRawHtml("</div>");
					}
					if (ShowDragButton) {
						var fileDragsvyLinkContainer = "svyDragImageLink_" + this.FullName;
						html.AddRawHtml("<div  id = \"" + fileDragsvyLinkContainer + "\" class='svyHtml5 svyLinkContainer svyNotMobileScreen'>");
						html.AddRawHtml("<a class='svyPasteLink btn btn-mini' href='#' onclick='svyHandleDragDrop(\"" + this.FullName + "\");return false;'><i class='icon icon-move'></i> Drag an image</a>");
						html.AddRawHtml("</div>");

						var cancelDragImagesvyLinkContainer = "svyCancelDragImageLink_" + this.FullName;
						html.AddRawHtml("<div  id = \"" + cancelDragImagesvyLinkContainer + "\" class='svyHtml5 svyCancelPaste'>");
						html.AddRawHtml("<a class='svyPasteLink btn btn-mini' href='#' onclick='svyCancelHtml5Drag(\"" + this.FullName + "\");return false;'><i class='icon icon-remove'></i> Cancel Drag</a>");
						html.AddRawHtml("</div>");
					}
					var cancelFileSelectContainer = "cancelFileSelectLink_" + this.FullName;
					html.AddRawHtml("<div  id = \"" + cancelFileSelectContainer + "\" class='svyCancelPaste svyPasteLink '>");
					html.AddRawHtml("<a class='svyPasteLink btn btn-mini ' href='#' onclick='svyCancelFileSelect(\"" + this.FullName + "\");return false;'><i class='icon icon-remove'></i> Cancel file select</a>");
					html.AddRawHtml("</div>");
					//End Html5 Links
				}
			}

			public override void AppendHtmlAttributes(HtmlTag html) {
				html.AddIfNotBlank("data-meta-width", MetaData.Width + "", false);
				html.AddIfNotBlank("data-meta-height", MetaData.Height + "", false);
				html.AddIfNotBlank("data-meta-cropresizechoice", MetaData.ShowCropResizeChoice.ToStringLower(), false);
				html.AddIfNotBlank("data-meta-showcropwindow", MetaData.ShowCropWindow.ToStringLower(), false);
				html.AddIfNotBlank("data-meta-allowpasteanddrag", MetaData.AllowPasteAndDrag.ToStringLower(), false);
				html.AddIfNotBlank("data-meta-allowselectfromserver", MetaData.AllowSelectFromServer.ToStringLower(), false);
				html.AddIfNotBlank("data-meta-backgroundcolor", MetaData.BackgroundColor, false);
				html.AddIfNotBlank("data-meta-forceajax", MetaData.ForceAjax.ToStringLower(), false);

				base.AppendHtmlAttributes(html);
			}

			// methods
			public override string GetHtml() {
				//IgnoreMaxLength();
				var isMobile = Web.IsMobile;
				string mobileClassContainerName = "";
				if (isMobile) mobileClassContainerName = "svyMobilePictureContainer";
				var basehtml = new HtmlTag("div");
				basehtml.Add("class", "svyPictureContainer " + mobileClassContainerName + "");
				if (cssClass.IsNotBlank()) basehtml.Add("class", cssClass);

				var isExisting = Picture != null && Picture.Exists;
				//var isExisting = Picture != null && FileSystem.FileAttachmentExists(Picture.FileName);
				if (isExisting) {
					HtmlTag uploadedPicHtml = GetUploadedPictureHtml();
					basehtml.AddTag(uploadedPicHtml);
				}
				if (Picture != null) {
					isExisting = FileSystem.FileAttachmentExists(Picture.FileName);
				}
				HtmlTag linksHtml = GetPictureLinksHtml(isExisting);
				basehtml.AddTag(linksHtml);
				//if (isExisting && MetaData.ShowCropWindow) {  MN 20141220 - nope, crop doesnt work on existing files
				//	basehtml.AddRawHtml("<a href=\"\" onclick=\"return svyCropPicture(this,'" + this.FullName + "','" + this.value + "')\">crop</a>");
				//}

				basehtml.AddTag(new HtmlTag("div").Add("style", "clear:both"));

				return basehtml.ToString();
			}
		}

		public class AttachmentField : SavvyBaseField {
			// properties
			public AttachmentActiveField Attachment { get { return (AttachmentActiveField)base.BindTo; } }
			public bool AllowRemove { get; set; }
			public bool AllowAjax { get; set; }
			public bool AllowChange { get; set; }
			public string AllowedMimetypes { get; set; }
			public bool AllowDownload { get; set; }
			/// <summary>
			/// This requires allow ajax to be set to true also.
			/// </summary>
			public bool AllowDragArea { get; set; }
			public string Subfolder { get; set; }

			private AttachmentMetaDataAttribute _metaData = new AttachmentMetaDataAttribute() { };
			public AttachmentMetaDataAttribute MetaData {
				get { return (Attachment != null) ? Attachment.MetaData : _metaData; }
				set { _metaData = value; }
			}


			//public bool AllowPaste { set { MetaData.AllowPasteAndDrag = value; } }

			// constructors
			public AttachmentField(AttachmentActiveField bindToActiveField, bool isRequired)
				: base(bindToActiveField, isRequired) {
				AllowRemove = true; AllowChange = true; AllowDownload = true; AllowAjax = false; AllowDragArea = false; AllowedMimetypes = "";
				MetaData = new AttachmentMetaDataAttribute();
			}
			public AttachmentField(string name, string value, bool isRequired)
				: base(name, value, isRequired) {
				AllowRemove = true; AllowChange = true; AllowDownload = true;
				AllowAjax = false; AllowedMimetypes = "";
				AllowDragArea = false;
			}


			// methods
			public override string GetHtml() {
				var basehtml = new HtmlTag("div");
				basehtml.Add("class", isRequired ? "svyAttachmentCntr required" : "svyAttachmentCntr");

				var html = new HtmlTag("div");
				html.Add("id", "aulc_" + this.name);
				// create input

				html.Add("class", "svyAttachment"); //Attachment upload container

				var input = new HtmlTag("input");
				input.Add("type", "file");


				input.Add("id", "file_" + this.name);

				if (AllowAjax) {
					input.Add("class", "svyAttachmentAjax"); //Attachment upload container
				}

				if (AllowedMimetypes.IsNotBlank()) {
					//CheckAllowedTypes
					onchange += "CheckAllowedTypes('" + this.name + "','" + AllowedMimetypes + "')";
					html.AddRawHtml("<input type=hidden id=\"attachmentMimeTypes_" + this.name + "\" name=\"attachmentMimeTypes_" + this.name + "\" value='" + AllowedMimetypes + "'>");
				}

				if (Attachment != null && Attachment.Exists) {
					isRequired = false;
				}
				AppendHtmlAttributes(input);
				html.AddTag(input);

				var span = new HtmlTag("span");
				span.Add("id", "sha_" + this.name); //show container
				if (Attachment != null && Attachment.Exists) {
					if (FileSystem.FileAttachmentExists(Attachment.FileName)) {
						html.Add("style", "display:none;");
						html.AddTag(new HtmlTag("a").SetInnerText("Cancel change attachment").Add("onclick", "$('#aulc_" + this.name + "').hide(200,function(){$('#sha_" + this.name + "').show(200)});$('#svyFiledragLive_" + this.name + "').hide();return false;").Add("href", "#"));
						string spancontents = "";

						string filename = Attachment.FileName;
						if (filename.StartsWith("secure/secure")) filename = filename.Replace("secure/secure", "secure/"); //twitch fix

						filename = filename.Substring(filename.LastIndexOf("\\") + 1);
						string fileType = filename.Substring(filename.LastIndexOf(".") + 1).ToUpper();
						if (!"DOC,XLS,PDF,ZIP,PPT,JPG,GIF,PNG,BMP".Contains(fileType)) {
							fileType = "generic";
						}
						if (fileType == "JPG" || fileType == "GIF" || fileType == "PNG" || fileType == "BMP") {
							fileType = "picture";
						}
						spancontents += "<img src='" + Web.ResolveUrl("~/images/filetypes") + "/" + fileType + "_small.gif' align=absmiddle style='margin-right:3px;border:0;'> <span class=\"svyAttachmentFile\">" + filename;
						if (AllowDownload) {
							var displayFilename = Attachment.FileName;
							if (displayFilename.StartsWith("secure/secure")) displayFilename = displayFilename.Replace("secure/secure", "secure/");

							spancontents += " - <a href=\"" + Web.ResolveUrl(Web.Attachments) + displayFilename + "\" class=\"svyAttachDownload\" target=\"_blank\">download attachment</a>";
						}

						//todo: zipdownload
						//spancontents += " - <a href=\""+Web.ResolveUrl("~/")+"zipdownload.aspx?filename="+ Attachment.FileName+"\" target=\"_blank\"\">Download Zipped Version of this Attachment (new window)</a><br>";
						if (AllowChange) spancontents += " | <a href=\"\" class=\"svyAttachmentChange\" onclick=\"$('#sha_" + this.name + "').hide(200,function(){$('#aulc_" + this.name + "').show(200);$('#svyFiledragLive_" + this.name + "').show();});return false;\">change attachment</a><br />";
						if (AllowRemove) spancontents += "<input type=\"checkbox\" id=\"" + this.name + "_remove\" name=\"" + this.name + "_remove\" title=\"Check this box to delete the attached file when you click Save below.\" value=\"1\"/><label for=\"" + this.name + "_remove\" class=\"svyAttachmentRemove\">Remove attachment</label>	";
						span.SetInnerHtml(spancontents);

						//basehtml.AddTag(span);
					} else {
						//attachment missing
						span.SetInnerText("attachment file missing [" + Attachment.FileName + "]");
					}
				}

				if (AllowAjax) {

					html.AddRawHtml("<input value = '' class='svyPasteHidden' id=\"paste_" + this.FullName + "\" type=hidden name=\"paste_" + this.FullName + "\">");

					html.AddRawHtml("</div>"); //end progress div

					html.AddRawHtml("<div class='svyImagePasteContainer'></div>");

					if (this.MetaData.AllowPasteAndDrag) {
						var attachmentExists = FileSystem.FileAttachmentExists(Attachment.FileName) ? "display:none;" : ""; //if file exists hide the drag container

						html.AddRawHtml("<div style = '" + attachmentExists + " ' class='svyFiledragLive " + " svyHtml5' id=\"svyFiledragLive_" + this.FullName + "\" ></div>");

						html.AddRawHtml("<input class='svyAttachmentHidden' id=\"attachmentFileName_" + this.FullName + "\" type=hidden name=\"attachmentFileName_" + this.FullName + "\">");
					}

					if (Subfolder.IsNotBlank()) {
						html.AddRawHtml("<input value = '" + this.Subfolder + "' class='svyAttachmentSubFolder' id=\"attachmentSubFolder_" + this.FullName + "\" type=hidden name=\"attachmentSubFolder_" + this.FullName + "\">");
					}

					//Common elements for upload
					html.AddRawHtml("<div style = 'display:none;'  class='svyFiledragProgess' id='svyImagePasteProgress_" + this.FullName + "'>");
					var spanID = "svyImagePasteSpan_" + this.FullName;
					html.AddRawHtml("<p></p>");
					html.AddRawHtml("<span id = '" + spanID + "'></span>");


					if (onclick.IsNotBlank()) {
						onclick += ";";
					}
					onclick += "handleFileBrowse('" + this.name + "')";

					AppendHtmlAttributes(input);

					if (onchange.IsNotBlank()) {
						onchange += ";";
					}
					//onchange += "IsAcceptableMimeType('" + this.name + "','msword,doc,docx,zip,txt,pdf');";
					AppendHtmlAttributes(input);

				}
				basehtml.AddTag(html);
				basehtml.AddTag(span);
				return basehtml.ToString();
			}

			public override void AppendHtmlAttributes(HtmlTag html) {
				if (MetaData != null) {
					html.AddIfNotBlank("data-meta-allowpasteanddrag", MetaData.AllowPasteAndDrag.ToStringLower(), false);
					// 20141220fixed bug again! bad merge? base.AppendHtmlAttributes(html);
				}
				base.AppendHtmlAttributes(html); // MN 20141218 fixed bug
			}

		}



		//function cf_OnChangestr_original_name(field) {
		//	var filename
		//	if (field.value!="") {
		//		message = "File is attached"
		//		filename = field.value
		//		filename = filename.substr(filename.lastIndexOf("\\")+1)
		//		fileType = filename.substr(filename.lastIndexOf(".")+1).toUpperCase()
		//		if (fileType!="DOC" && fileType!="XLS" && fileType!="PDF" && fileType!="ZIP" && fileType!="PPT") {
		//			fileType = "generic"
		//		}
		//		message = "<img src='../images/filetypes/"+fileType+"_small.gif' align=absmiddle style='margin-right:3px;border:0px;'> <b>"+filename+"</b>"
		//		jQuery("#AttachMessage"+df_currentRowSuffix).html(message)
		//		jQuery("#AttachFile"+df_currentRowSuffix).hide()
		//	} else {
		////		message = "Click Browse to attach a file"
		//	}
		//}


		/// <summary>
		/// Displays an address search box and map with draggable marker for entering a valid address, using Google Maps.
		/// This requires an Address table in the database with specific fields to hold the geodata returned from Google geocoder.
		/// Pass in the AddressID field which should be a foreign key in your main table (eg record.Fields.AddressID).
		/// To save the data, call "Forms.GeoAddress.Save()".
		/// This server side code is fully compatible with Matt's original Address table data structure and geoaddress.js file.
		/// </summary>
		public class GeoAddress : SavvyBaseField {
			private string MapDiv = "MapDiv";
			private string AddressField = "Address__Specified";
			private string LatitudeField = "Address__Latitude";
			private string LongitudeField = "Address__Longitude";
			private string AltitudeField = "Address__Altitude";
			private string ZoomField = "Address__Zoom";
			private string ProperField = "Address__Proper";
			private string AccuracyField = "Address__Accuracy";
			private string ThoroughfareField = "Address__Thoroughfare";
			private string LocalityField = "Address__Locality";
			private string SubAdministrativeAreaField = "Address__SubAdministrativeArea";
			private string AdministrativeAreaField = "Address__AdministrativeArea";
			private string PostalCodeField = "Address__PostalCode";
			private string CountryCodeField = "Address__CountryCode";
			private string CountryNameField = "Address__CountryName";
			private int RequiredAccuracy = 7;
			private string ChangedStatusField = "Address__ChangedStatus";

			public GeoAddress(ActiveField<int?> addressIdField, bool isRequired) : base(addressIdField, isRequired) { }
			public GeoAddress(string fieldName, string value, bool isRequired) : base(fieldName, value, isRequired) { }
			public override string GetHtml() {
				Util.IncludejQuery();
				Util.IncludeGoogleMaps(null);
				// this server side code is fully compatible with Matt's original data structure and geoaddress.js file
				Util.IncludeJavascriptFile(null, "~/js/geoaddress.js");

				string geoMapDetails =
					String.Format(
						"'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}'"
						, MapDiv
						, AddressField
						, LatitudeField
						, LongitudeField
						, AltitudeField
						, ZoomField
						, ProperField
						, AccuracyField
						, ThoroughfareField
						, LocalityField
						, SubAdministrativeAreaField
						, AdministrativeAreaField
						, PostalCodeField
						, CountryCodeField
						, CountryNameField
						, RequiredAccuracy
						, ChangedStatusField
						);

				if (this.value.IsNotBlank()) {
					//string script = "$(document).ready(function(){ window.geoMap = new GeoMap(" + geoMapDetails + ", document.getElementById('')); ShowMap(window.geoMap) });";
					// show the map
					Beweb.Util.IncludeJavascript(null, String.Format(@"
$(document).ready(function() {{
	ShowMap(new GeoMap({0}));
}});
", geoMapDetails));
				}

				var addressRecord = new ActiveRecord<int>("Address", "AddressID");
				if (!BindTo.IsBlank) {
					var sql = new Sql("select * from address");
					sql.Add("where addressID=", BindTo.Sqlize());
					addressRecord.LoadData(sql);
				} else {
					addressRecord.AddNew();
				}

				HtmlTag html = new HtmlTag("div");
				var searchInput = new HtmlTag("input").Add("id", "Address__Specified").Add("name", "Address__Specified").Add("maxlength", "255").Add("style", "width:400px");
				//searchInput.Add("onchange", "MapChanged(window.geoMap, this)", false);
				if (addressRecord.FieldExists("Specified") && addressRecord["Specified"].IsNotNull) {
					searchInput.Add("value", addressRecord["Specified"].ToString());
				}
				searchInput.Add("onchange", String.Format("MapChanged(new GeoMap({0}, this))", geoMapDetails), false);
				html.AddTag(searchInput);
				var go = new HtmlTag("input").Add("type", "button").Add("value", "go");
				html.AddTag(go);

				string[] fieldNames = {
																//"Specified",
																"Latitude",
																"Longitude",
																"Altitude",
																"Zoom",
																"Proper",
																"Accuracy",
																"Thoroughfare",
																"Locality",
																"SubAdministrativeArea",
																"AdministrativeArea",
																"PostalCode",
																"CountryCode",
																"CountryName",
																"AddressID"
															};

				foreach (var field in addressRecord.GetFields()) {
					if (fieldNames.Contains(field.Name, StringComparer.InvariantCultureIgnoreCase)) {
						var hidden = new HtmlTag("input").Add("type", "hidden").Add("name", "Address__" + field.Name).Add("id", "Address__" + field.Name).Add("value", field.ToString());
						html.AddTag(hidden);
					}
				}
				var hiddenStatus = new HtmlTag("input").Add("type", "hidden").Add("name", "Address__ChangedStatus").Add("id", "Address__ChangedStatus").Add("value", "0");
				html.AddTag(hiddenStatus);

				string result = html.ToString();
				result += "<div class=help_text>Is this marker in the right place?<br />If not, you can drag it to its right location</div>";
				result += "<div id=MapDiv class=geoaddress-map></div>";

				return result;
			}

			/// <summary>
			/// Save a selected address to the Address table in the database. Returns the AddressID of the address record, which you should save in your main table (eg you should have an AddressID field in your Customer table and save this value in there).
			/// </summary>
			/// <returns></returns>
			public static int Save() {
				//Logging.DumpFormHTML();
				var address = new ActiveRecord("Address", "AddressID");
				string prefix = "Address__";
				int? addressID = Web.Request[prefix + "AddressID"].ToInt(null);
				if (addressID != null) {
					address.LoadData(new Sql("select * from [Address] where addressID=", addressID.Value));
					//address.LoadID(addressID.Value);
				}
				address.UpdateFromRequest(prefix, "");
				address.Save();
				return (int)address.ID_Field.ValueObject;
			}
		}

		//public class ParentPicker : SavvyBaseField
		//{

		//}

		/// <summary>
		/// this is part of the internals
		/// </summary>
		public class SavvyFieldBinderOLD {
			private object _dataObject;

			public enum DataTypes				 // TODO: dont think we need this really?
			{
				String,
				DateTime,
				Int,
				Money,
				Float,
				Bit
			}
			public string FieldName { get; set; }
			public object Value {
				get {
					return GetPropertyValue(_dataObject, FieldName);
				}
			}

			public DataTypes DataType { get; set; }
			public int MaxLength { get; set; }

			// constructors
			public SavvyFieldBinderOLD(string name) {
				this.FieldName = name;
			}

			public SavvyFieldBinderOLD(object dataObject, string name, DataTypes dataType) {
				_dataObject = dataObject;
				FieldName = name;
				DataType = dataType;
				MaxLength = 0;
			}

			public SavvyFieldBinderOLD(object dataObject, string name, DataTypes dataType, int maxLength) {
				_dataObject = dataObject;
				FieldName = name;
				DataType = dataType;
				MaxLength = maxLength;
			}

			// methods
			public void Init(object dataObject) {
				this._dataObject = dataObject;
			}

			public static object GetPropertyValue(object obj, string propertyName) {
				return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
			}


		}

		/// <summary>
		/// An ordered list of name, value pairs to use as select options, checkbox lists and radio groups
		/// </summary>
		public class SelectOptions : IEnumerable<SelectOptions.Option> {
			//:  OrderedDictionary {
			//protected List<KeyValuePair<string, string>> elements = new List<KeyValuePair<string, string>>();
			protected List<Option> elements = new List<Option>();

			public class Option {
				public Option(string value, string text)
					: this(value, text, null) {
					Value = value;
					Text = text;
				}

				public Option(string value, string text, string data) {
					Value = value;
					Text = text;
					Data = data;
				}

				public Option(string value, string text, string data, bool isDisabled) {
					Value = value;
					Text = text;
					Data = data;
					Disabled = isDisabled;
				}

				public Option(string value) {
					Value = value;
					Text = value;
				}

				public string Value { get; set; }
				public string Text { get; set; }
				public string Data { get; set; }
				public bool Disabled;
				public bool IsOptGroup;
			}

			public int Count {
				get { return elements.Count; }
			}

			/// <summary>
			/// Adds an Option with the given value and same text. 
			/// </summary>
			public SelectOptions Add(string value) {
				this.Add(value, value);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and same text. 
			/// </summary>
			public SelectOptions Add(StringConst value) {
				this.Add(value.Code, value.DisplayName);
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// </summary>
			public SelectOptions Add(string value, string text) {
				elements.Add(new Option(value, text));
				//elements.Add(value, text);
				return this;
			}

			public SelectOptions AddWithData(string value, string text, string dataval) {
				elements.Add(new Option(value, text, dataval));
				//elements.Add(value, text);
				return this;
			}

			/// <summary>
			/// Checks if an Option already exists with the given Value and if not, adds an Option with the given Value and given Text.
			/// </summary>
			public SelectOptions AddUnique(string value, string text) {
				if (!ContainsValue(value)) Add(value, text);
				return this;
			}

			/// <summary>
			/// Adds a dividing line. This is useful for dropdown options only (not radios).
			/// </summary>
			public SelectOptions AddDivider() {
				Add("", "----------");
				return this;
			}

			/// <summary>
			/// Adds Options from a SQL query, where the first field is the value and second field is the text.
			/// The second field is optional.
			/// </summary>
			public SelectOptions Add(Sql sql) {
				if (sql == null) throw new Exception("Form.SelectOptions.Add: parameter sql is null");
				using (var reader = sql.GetReader()) {
					foreach (DbDataRecord record in reader) {
						if (record.FieldCount == 1 || record[1] == null) {
							this.Add(record[0].ToString());
						} else {
							this.Add(record[0].ToString(), record[1].ToString());
						}
					}
					reader.Close();
					reader.Dispose();
				}
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// </summary>
			public SelectOptions Add(IActiveRecordList collection) {
				return Add(collection, null);
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// </summary>
			public SelectOptions Add(IActiveRecordList collection, string datacolumn) {
				if (collection == null) return this;
				foreach (var item in collection) {
					ActiveRecord record = (ActiveRecord)item;
					if (datacolumn == null) {
						this.Add(record.ID_Field.ToString(), record.GetName());
					} else {
						this.AddWithData(record.ID_Field.ToString(), record.GetName(), record[datacolumn] + "");
					}
				}
				return this;
			}

			/// <summary>
			/// Adds an Option with the given value and given text.
			/// </summary>
			public SelectOptions Add(IEnumerable collection) {
				if (collection == null) return this;
				foreach (var item in collection) {
					if (item is ActiveRecord) {
						ActiveRecord record = (ActiveRecord)item;
						this.Add(record.ID_Field.ToString(), record.GetName());
					} else if (item is Option) {
						this.Add((Option)item);
					} else if (item is DbDataRecord) {
						DbDataRecord dataRecord = (DbDataRecord)item;
						this.Add(dataRecord[0] + "", dataRecord[1] + "");
					} else if (item is StringConst) {
						this.Add((StringConst)item);
					} else {
						// generic enumerable - add as string
						this.Add(item.ToString());
					}
				}
				return this;
			}

			public SelectOptions Add(Option item) {
				elements.Add(item);
				return this;
			}

			/// <summary>
			/// Adds options in the provided collection and uses the properties defined in valueProperty and textProperty as the value and text.
			/// </summary>
			/// <param name="collection"></param>
			/// <param name="valueProperty"></param>
			/// <param name="textProperty"></param>
			public SelectOptions Add(IEnumerable collection, string valueProperty, string textProperty) {
				foreach (object item in collection) {
					this.Add(item.GetPropertyValue(valueProperty) + "", item.GetPropertyValue(textProperty) + "");
				}
				return this;
			}

			/// <summary>
			/// You should not normally need this, but it you want to sort or remove or otherwise muck round with the elements, this method returns them.
			/// </summary>
			public IEnumerable<Option> InternalElements {
				get { return elements; }
			}

			/// <summary>
			/// Returns the final element list once all elements are there.
			/// </summary>
			public IEnumerable<Option> FinalElements {
				get {
					int dividerWidth = 2;
					foreach (var option in elements) {
						if (option.Text.Length > dividerWidth) {
							dividerWidth = option.Text.Length;
						}
					}
					string divider = new string('-', dividerWidth);
					foreach (var option in elements) {
						if (option.Value == "----------") {
							option.Value = divider;
						}
					}
					return elements;
				}
			}

			public IEnumerator<SelectOptions.Option> GetEnumerator() {
				return elements.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			/// <summary>
			/// Returns true if the options list already contains an option with the given value. 
			/// (Options consist of a Value and Text - this checks the Value, not the Text.)
			/// </summary>
			/// <returns></returns>
			public bool ContainsValue(string value) {
				foreach (var option in elements) {
					if (option.Value == value) {
						return true;
					}
				}
				return false;
			}

			public SelectOptions AddRange(int start, int count) {
				return this.Add(Enumerable.Range(start, count));
			}

			public SelectOptions AddOptGroup(string label) {
				var opt = new Option("", label);
				opt.IsOptGroup = true;
				this.Add(opt);
				return this;
			}

		}

		/// <summary>
		/// Get or set the row prefix to be applied to all field names.
		/// </summary>
		public static string CurrentRowSuffix {
			get {
				return Web.PageGlobals["CurrentRowSuffix"] + "";
			}
			set {
				Web.PageGlobals["CurrentRowSuffix"] = value;
			}
		}

		public static int CurrentRowIndex {
			get { return (Web.PageGlobals["CurrentRowSuffix"] + "").RightFrom("__").ToInt(0); }
		}

		public static string EditableDropbox(ActiveRecord record, ActiveField<int?> dropboxField, string displayvalue, Forms.Dropbox dropContent) {
			//return "<span onclick=\"alert('not available')\" style=\"cursor:pointer\">"+numberField.Value+"</span>";
			var dropboxIDValue = dropboxField.Value;
			//var value = ((dropboxIDValue.HasValue) ? "" + dropboxIDValue.Value : "");
			//var displayvalue = ((dropboxIDValue.HasValue) ? "" + dropboxIDValue.Value : "");	 
			var value = displayvalue;
			string result = value;
			var tableName = dropboxField.TableName;
			var tn = Crypto.Encrypt(tableName);
			int? id = record.ID_Field.ValueObject.ToInt(-1);
			if (id > 0) {
				var rid = Crypto.EncryptID(id);
				var colName = dropboxField.Name;

				var cn = Crypto.Encrypt(colName); //col name
				result = "<div data-tn=\"" + tn + "\" data-cn=\"" + cn + "\" data-rid=\"" + rid + "\" data-ov=\"" + Fmt.JsEncode(value) + "\" class=\"edfield\"  onclick=\"return dv(this,'dropbox')\">" + value + "</div>";
				//var dropboxList = new Forms.Dropbox(dropboxField, true).Add(new Sql("select "+tableName+"id,"+colName+" from "+tableName+""));
				result += "<div class=\"dyn drop edfield\" data-rid=\"" + rid + "\" style=\"display:none\">" + dropContent + "</div>";
			}
			return result;
		}
		public static string EditableNumberField(ActiveRecord record, ActiveField<int?> numberField) {
			//return "<span onclick=\"alert('not available')\" style=\"cursor:pointer\">"+numberField.Value+"</span>";
			var displayValue = numberField.Value;
			var value = ((displayValue.HasValue) ? "" + displayValue.Value : "");
			string result = value;
			var tn = Crypto.Encrypt(numberField.TableName);
			int? id = record.ID_Field.ValueObject.ToInt(-1);
			if (id > 0) {
				var rid = Crypto.EncryptID(id);
				var cn = Crypto.Encrypt(numberField.Name); //col name
				result = "<div data-tn=\"" + tn + "\" data-cn=\"" + cn + "\" data-rid=\"" + rid + "\" data-ov=\"" + Fmt.JsEncode(value) + "\" class=\"edfield\"  onclick=\"return dv(this,'number')\">" + value + "</div>";
			}
			return result;
		}
		public static string EditableMoneyField(ActiveRecord record, ActiveField<decimal?> decimalField) { //JC ADDED 20141111 Dont forget to update forms.js
			//return "<span onclick=\"alert('not available')\" style=\"cursor:pointer\">"+numberField.Value+"</span>";
			var displayValue = decimalField.Value;
			var value = ((displayValue.HasValue) ? "" + displayValue.Value : "");
			value = Numbers.Round(value.ToDecimal(), 2).ToString();
			string result = value;
			var tn = Crypto.Encrypt(decimalField.TableName);
			int? id = record.ID_Field.ValueObject.ToInt(-1);
			if (id > 0) {
				var rid = Crypto.EncryptID(id);
				var cn = Crypto.Encrypt(decimalField.Name); //col name
				result = "$<span data-tn=\"" + tn + "\" data-cn=\"" + cn + "\" data-rid=\"" + rid + "\" data-ov=\"" + Fmt.JsEncode(value) + "\" class=\"edfield\"  onclick=\"return dv(this,'money')\">" + value + "</span>";
			}
			return result;
		}
		public static string EditableTextField(ActiveRecord record, ActiveField<string> stringField) {
			//return "<span onclick=\"alert('not available')\" style=\"cursor:pointer\">"+numberField.Value+"</span>";
			var displayValue = stringField.Value;
			var value = displayValue + "";
			string result = value;
			var tn = Crypto.Encrypt(stringField.TableName);
			int? id = record.ID_Field.ValueObject.ToInt(-1);
			if (id > 0) {
				var rid = Crypto.EncryptID(id);
				var cn = Crypto.Encrypt(stringField.Name); //col name
				result = "<div data-tn=\"" + tn + "\" data-cn=\"" + cn + "\" data-rid=\"" + rid + "\" data-ov=\"" + Fmt.JsEncode(value) + "\" class=\"edfield\" onclick=\"return dv(this,'text')\">" + value + "</div>";
			}
			return result;
		}
	}

	#region OldCode
	//public partial class Forms {

	//	public class Processor<TModel> {
	//		public Action OnSaving { get; set; }
	//		public Action OnSaved { get; set; }
	//		public Action OnFinished { get; set; }
	//		public Action OnSubformSaving { get; set; }
	//		public Action OnSubformSaved { get; set; }
	//		public Action OnInvalid { get; set; }
	//		public void Process() { }

	//		public string Suffix { get; set; }
	//		public string TableName { get; set; }
	//		public string KeyName { get; set; }
	//		public string KeyValue { get; set; }
	//		public TModel Model { get; set; }

	//		public void Cancel() { }
	//		public void Error(string message) { }

	//		public void BackToCallingPage() { }
	//		public void GoToDuplicate() { }
	//		public void GoToEdit() { }
	//		public void GoToAdd() { }

	//	}
	//}
	#endregion
}
