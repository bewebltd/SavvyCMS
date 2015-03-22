using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Beweb;

namespace Pop3 {
	/// <summary>
	/// Summary description for Pop3Attachment.
	/// </summary>
	public class Pop3Component {
		private string m_contentType;
		private string m_name;
		private string m_filename;
		private string m_contentTransferEncoding;
		private string m_contentDescription;
		private string m_contentDisposition;
		private string m_data;
		private string m_filePath;

		public byte[] m_binaryData;

		public string FileExtension {
			get {
				string extension = null;

				// if file has a filename and the filename
				// has an extension ...

				if ((m_filename != null) &&
					Regex.Match(m_filename, @"^.*\..*$").Success) {
					// get extension ...
					extension =
					Regex.Replace(m_name, @"^[^\.]*\.([^\.]+)$", "$1");
				}

				// NOTE: return null if extension
				// not found ...
				return extension;
			}
		}

		public string FileNoExtension {
			get {
				string extension = null;

				// if file has a filename and the filename
				// has an extension ...

				if ((m_filename != null) &&
					Regex.Match(m_filename, @"^.*\..*$").Success) {
					// get extension ...
					extension =
						Regex.Replace(m_name, @"^([^\.]*)\.[^\.]+$", "$1");
				}

				// NOTE: return null if extension
				// not found ...
				return extension;
			}
		}

		public string FilePath {
			get { return m_filePath; }
		}

		public string Filename {
			get { return m_filename; }
		}

		public string ContentType {
			get { return m_contentType; }
		}

		public string Name {
			get { return m_name; }
		}

		public string ContentTransferEncoding {
			get { return m_contentTransferEncoding; }
		}

		public string ContentDescription {
			get { return m_contentDescription; }
		}

		public string ContentDisposition {
			get { return m_contentDisposition; }
		}

		public string Data {
			get { return m_data; }
		}

		public override string ToString() {
			return
				"Content-Type: " + m_contentType + "\r\n" +
				"Name: " + m_name + "\r\n" +
				"Filename: " + m_filename + "\r\n" +
				"Content-Transfer-Encoding: " + m_contentTransferEncoding + "\r\n" +
				"Content-Description: " + m_contentDescription + "\r\n" +
				"Content-Disposition: " + m_contentDisposition + "\r\n" +
				"Data :" + m_data;
		}


		public bool IsBody {
			get {
				return
				(m_contentDisposition == null) ? true : false;
			}
		}

		public bool IsAttachment {
			get {
				bool ret = false;

				if (m_contentDisposition != null) {
					ret =
						Regex
						.Match(m_contentDisposition,
						"^attachment.*$")
						.Success;
				}

				return ret;
			}
		}

		private void DecodeData() {
			// if this data is an attachment ...
			if (m_contentDisposition != null) {

				// create data folder if it doesn't exist ...
				if (!Directory.Exists(Pop3Statics.DataFolder)) {
					Directory.CreateDirectory(Pop3Statics.DataFolder);
				}
				var searchText = "; filename=";
				if (m_filename == null && m_contentDisposition.Contains(searchText)) //look in  filename=\"import example file.csv\"
				{
					//below three lines could be easier using regular expression 
					int startIndex = m_contentDisposition.IndexOf(searchText) + searchText.Length;
					string startOfFileName = m_contentDisposition.Remove(0, startIndex + 1); //remove start
					m_filename = startOfFileName.Remove(startOfFileName.Length - 2); //remove end
					if(m_filename.Contains("\"")) {
						m_filename = m_filename.Substring(0,m_filename.IndexOf('"'));
					}

					m_filePath = Pop3Statics.DataFolder + @"\" + Beweb.FileSystem.GetUniqueFilename(Pop3Statics.DataFolder + @"\", m_filename);
					//m_filePath = Pop3Statics.DataFolder + @"\" + m_filename;
				}


				// if BASE-64 data ...
				if ((m_contentDisposition.Contains("attachment;")) &&
					(m_contentTransferEncoding.ToUpper()
					.Equals("BASE64"))) {
					// convert attachment from BASE64 ...
					m_binaryData =
						Convert.FromBase64String(m_data.Replace("\n", ""));

					BinaryWriter bw = new BinaryWriter(
						new FileStream(m_filePath, FileMode.Create));

					bw.Write(m_binaryData);
					bw.Flush();
					bw.Close();
				} else if ((m_contentDisposition.Contains("attachment;")) &&// if PRINTABLE ...
					(m_contentTransferEncoding.ToUpper()
					.Equals("QUOTED-PRINTABLE"))) {
					using (StreamWriter sw = File.CreateText(m_filePath)) {
						sw.Write(Pop3Statics.FromQuotedPrintable(m_data));
						sw.Flush();
						sw.Close();
					}
				}else {
					//Unknown type

				}
			}
		}


		public Pop3Component(string contentType, string data) {
			m_contentType = contentType;
			m_data = data;
		}

		public Pop3Component(string contentType, string name, string filename,
			string contentTransferEncoding, string contentDescription,
			string contentDisposition, string data) {
			m_contentType = contentType;
			m_name = name;
			m_filename = filename;
			m_contentTransferEncoding = contentTransferEncoding;
			m_contentDescription = contentDescription;
			m_contentDisposition = contentDisposition;
			m_data = data;
			try {
				DecodeData();
			}
			catch(Exception e) {
				//throw new BewebDataException("Error processing file, filename = [" + filename + "], exception = [" + e + "]");
			}
		}
	}
}
