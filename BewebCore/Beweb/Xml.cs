using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Beweb {
	/// <summary>
	/// Summary description for Xml
	/// </summary>
	public class bwbXml {
		public static XmlTextWriter AddCDataNode(XmlTextWriter tw, string nodename, string value) {
			tw.WriteStartElement(nodename);
			if (!String.IsNullOrEmpty(value)) {
				tw.WriteCData(value);
			}
			tw.WriteEndElement();
			return tw;
		}

		/// <summary>
		/// Method to convert a custom Object to XML string. This uses Unicode (UTF-16) encoding.
		/// </summary>
		/// <param name="obj">Object that is to be serialized to XML. Note this must be a serializable object.</param>
		/// <returns>XML string</returns>
		public static String SerializeObject(Object obj) {
			var serializer = new XmlSerializer(obj.GetType());
			StringWriter sw = new StringWriter();
			serializer.Serialize(sw, obj);
			return sw.ToString();
		}

		/// <summary>
		/// Method to convert a custom Object to XML string. You can specify the encoding (eg UTF-8 or UTF-16).
		/// </summary>
		/// <param name="obj">Object that is to be serialized to XML. Note this must be a serializable object.</param>
		/// <param name="encoding">An encoding like System.Text.Encoding.UTF8</param>
		/// <returns>XML string</returns>
		public static String SerializeObject(Object obj, Encoding encoding) {
			var serializer = new XmlSerializer(obj.GetType());

			// create a MemoryStream here, we are just working
			// exclusively in memory
			System.IO.Stream stream = new System.IO.MemoryStream();

			// The XmlTextWriter takes a stream and encoding
			// as one of its constructors
			System.Xml.XmlTextWriter xtWriter = new System.Xml.XmlTextWriter(stream, encoding);

			serializer.Serialize(xtWriter, obj);

			xtWriter.Flush();

			// go back to the beginning of the Stream to read its contents
			stream.Seek(0, System.IO.SeekOrigin.Begin);

			// read back the contents of the stream and supply the encoding
			System.IO.StreamReader reader = new System.IO.StreamReader(stream, encoding);

			string result = reader.ReadToEnd();

			return result;
		}

		/// <summary>
		/// Method to create a object from an XML string. This uses Unicode (UTF-16) encoding.
		/// </summary>
		/// <returns>New object of given type T</returns>
		public static T DeserializeObject<T>(string xmlText) {
			var serializer = new XmlSerializer(typeof(T));
			StringReader sr = new StringReader(xmlText);
			object obj = serializer.Deserialize(sr);
			return (T)obj;
		}

		/// <summary>
		/// Validate an XML string against an XSD schema.
		/// </summary>
		/// <example>string errorMsg = ValidateXml(myxmlstring.ToStream(Encoding.Unicode), "schema.xsd", targetNamespace);</example>
		/// <param name="xml">XML contents as a Stream</param>
		/// <param name="xsdFile">File name including absolute path</param>
		/// <returns>Any validation error message or null if all OK</returns>
		public static string ValidateXml(Stream xml, string xsdFile, string targetNamespace) {
			string result = null;
			try {
				//string xmlFile = Web.Server.MapPath("Dvdlist.xml");
				//string xsdFile = Web.Server.MapPath("DVDList.xsd");

				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ValidationType = ValidationType.Schema;

				settings.Schemas.Add(targetNamespace, xsdFile);

				XmlReader objXmlReader = XmlReader.Create(xml, settings);

				while (objXmlReader.Read()) { }
			} catch (XmlSchemaValidationException e) {
				result = e.Message;
			}

			return result;
		}

		/// <summary>
		/// Validate an XML string against an XSD schema.
		/// </summary>
		/// <param name="xml">XML contents as a string (assumes UTF-16 which is standard string encoding)</param>
		/// <param name="xsdFile">File name including absolute path</param>
		/// <returns>Any validation error message or null if all OK</returns>		
		public static string ValidateXmlString(string xml, string xsdFile, string targetNamespace) {
			return ValidateXml(xml.ToStream(Encoding.Unicode), xsdFile, targetNamespace);
		}

		/// <summary>
		/// Given an xml string, returns an XDocument
		/// </summary>
		public static XDocument Parse(string responseXml) {
			return XDocument.Parse(responseXml);
		}

		/// <summary>
		/// Given a filename or URL, returns an XDocument
		/// </summary>
		public static XDocument Load(string filename) {
			return XDocument.Load(Web.MapPath(filename));
		}

		public static void KillNamespaces(XDocument doc) {
			doc.Descendants()
				.Attributes()
				.Where(x => x.IsNamespaceDeclaration)
				.Remove();

			foreach (var elem in doc.Descendants()) {
				elem.Name = elem.Name.LocalName;
			}
		}

		public static IEnumerable<XElement> StreamNodes(string inputUrl, string matchName) {
			HttpWebRequest httpRequest = WebRequest.Create(inputUrl) as HttpWebRequest;
			httpRequest.Timeout = 1000 * 60 * 10;//in millisecs														10 mins
			httpRequest.ReadWriteTimeout = 1000 * 60 * 10;//in millisecs									10 mins

			if (httpRequest != null) {
				httpRequest.Method = "GET";
				httpRequest.Accept = "*/*"; // any file type
				using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse) {
					if (httpResponse != null) {
						StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);



						using (var reader = XmlReader.Create(sr)) {

							reader.MoveToContent();
							while (reader.Read()) {
								switch (reader.NodeType) {
									case XmlNodeType.Element:
										if (reader.Name == matchName) {
											XElement el = XElement.ReadFrom(reader)
																						as XElement;
											if (el != null)
												yield return el;
										}
										break;
								}
							}
							reader.Close();
						}


						sr.Close();
						httpResponse.Close();
					}
				}
			}
		}

		public static string PrettyPrintXml(string xml) {
			try {
				XDocument doc = XDocument.Parse(xml);
				string str = doc.ToString().HtmlEncode();
				//str = str.ReplaceFirst("\n","<br>").Replace(" ", "&nbsp;");  // ideally replace only spaces before the tags
				 return "<pre>"+str+"</pre>";
			} catch (Exception) {
				return xml;
			}
		}

	}

	public static class XmlLinqExtensions {
		/// <summary>
		/// Returns the value (inner text) of an immediate child element with the supplied name, or null if the element is not found.
		/// </summary>
		public static string ElementValue(this XElement likeThis, string elementName) {
			string result = null;
			var element = likeThis.Element(elementName);
			if (element != null) {
				result = element.Value;
			}
			if (result == "") {
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Returns the value (inner text) of this element, or null if blank.
		/// </summary>
		public static string InnerText(this XElement likeThis) {
			string result = null;
			if (likeThis != null) {
				result = likeThis.Value;
			}
			if (result == "") {
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Returns the value (inner text) of an immediate child element with the supplied name, or null if the element is not found.
		/// </summary>
		public static string AttributeValue(this XElement likeThis, string attributeName) {
			string result = null;
			var attribute = likeThis.Attribute(attributeName);
			if (attribute != null) {
				result = attribute.Value;
			}
			if (result == "") {
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Returns the value (inner text) of the first descendent element with the supplied name, or null if the element is not found.
		/// </summary>
		public static string FindValue(this XElement likeThis, string elementName) {
			string result = null;
			var elements = likeThis.Descendants(elementName);
			var list = elements.ToList();
			//Web.Write("finding "+elementName+"... ");
			//Web.Write("count="+list.Count()+"... ");
			if (list.Count() > 0) {
				result = list.First().Value;
				//Web.Write("first="+result+"... ");
			}
			//Web.Write("result="+result+"... ");
			if (result == "") {
				result = null;
			}
			//			/Web.Write("result : " + result);
			return result;
		}

		/// <summary>
		/// Returns the raw value (inner HTML) of the first descendent element with the supplied name, or null if the element is not found.
		/// </summary>
		public static string FindValueRaw(this XElement likeThis, string elementName) {
			string result = null;
			var elements = likeThis.Descendants(elementName);
			var list = elements.ToList();
			//Web.Write("finding "+elementName+"... ");
			//Web.Write("count="+list.Count()+"... ");
			if (list.Count() > 0) {
				result = list.First().ToString();
				//Web.Write("first="+result+"... ");
			}
			//Web.Write("result="+result+"... ");
			if (result == "") {
				result = null;
			}
			//			/Web.Write("result : " + result);
			return result;
		}



	}


}