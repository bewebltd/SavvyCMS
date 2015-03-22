using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;

namespace Site.tests {
	public partial class TestFTP : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e)
		{
			foreach(var line in GetFileList("attachments"))
			{
				Logging.dout(line);
			}
			Upload(Server.MapPath("~/attachments/lighthouse.jpg"),"attachments/ftpsend");
			Download(Server.MapPath("~/attachments/ftpget/"),"lighthouse.jpg");
		}

		//ftp://beweb:XViE3kiUux@edna.beweb.co.nz
		string ftpServerIP = "edna.beweb.co.nz";
		private string ftpUserID = "beweb";
		private string ftpPassword = "XViE3kiUux";
		//string ftpServerIP = "theindex.co.nz";
		//private string ftpUserID = "testsupplier@supplier.co.nz";
		//private string ftpPassword = "blah";

		private void Upload(string filename, string serverFolderPath) {
			FileInfo fileInf = new FileInfo(filename);
			string uri = "ftp://" + ftpServerIP + "/" + serverFolderPath + "/" + fileInf.Name;
			FtpWebRequest reqFTP;
			// Create FtpWebRequest object from the Uri provided
			reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));
			// Provide the WebPermission Credintials
			reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
			// By default KeepAlive is true, where the control connection is not closed
			// after a command is executed.
			reqFTP.KeepAlive = false;
			// Specify the command to be executed.
			reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
			// Specify the data transfer type.
			reqFTP.UseBinary = true;
			// Notify the server about the size of the uploaded file
			reqFTP.ContentLength = fileInf.Length;
			// The buffer size is set to 2kb
			int buffLength = 2048;
			byte[] buff = new byte[buffLength];
			int contentLen;
			// Opens a file stream (System.IO.FileStream) to read the file to be uploaded
			FileStream fs = fileInf.OpenRead();
			try {
				// Stream to which the file to be upload is written
				Stream strm = reqFTP.GetRequestStream();
				// Read from the file stream 2kb at a time
				contentLen = fs.Read(buff, 0, buffLength);
				// Till Stream content ends
				while (contentLen != 0) {
					// Write Content from the file stream to the FTP Upload Stream
					strm.Write(buff, 0, contentLen);
					contentLen = fs.Read(buff, 0, buffLength);
				}
				// Close the file stream and the Request Stream
				strm.Close();
				fs.Close();
			} catch (Exception ex) {
				//MessageBox.Show(ex.Message, "Upload Error");
				Logging.dout("Upload Error ["+ex.Message+"]");
			}
		}
		//Above is a sample code for FTP Upload (PUT). The underlying sub command used is STOR. Here an FtpWebRequest object is made for the specified file on the ftp server. Different properties are set for the request namely Credentials, KeepAlive, Method, UseBinary, ContentLength.

		//The file in our local machine is opened and the contents are written to the FTP request stream. Here a buffer of size 2kb is used as an appropriate size suited for upload of larger or smaler files.

		private void Download(string filePath, string fileName) {
			FtpWebRequest reqFTP;
			try {
				//filePath = <<The full path where the file is to be created. the>>,
				//fileName = <<Name of the file to be createdNeed not name on FTP server. name name()>>
				FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
				reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
				reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
				reqFTP.UseBinary = true;
				reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
				Stream ftpStream = response.GetResponseStream();
				long cl = response.ContentLength;
				int bufferSize = 2048;
				int readCount;
				byte[] buffer = new byte[bufferSize];
				readCount = ftpStream.Read(buffer, 0, bufferSize);
				while (readCount > 0) {
					outputStream.Write(buffer, 0, readCount);
					readCount = ftpStream.Read(buffer, 0, bufferSize);
				}
				ftpStream.Close();
				outputStream.Close();
				response.Close();
			} catch (Exception ex) {
				//MessageBox.Show(ex.Message);
				Logging.dout("Download Error ["+ex.Message+"]");

			}
		}
		//Above is a sample code for Download of file from the FTP server. Unlike the Upload functionality described above, Download would require the response stream, which will contain the content of the file requested. Here the file to download is specified as part of the Uri which inturn is used for the creation of the FtpWebRequest object. To 'GET' the file requested, get the response of the FtpWebRequest object using GetResponse() method. This new response object built provides the response stream which contain the file content as stream, which you can easily convert to a file stream to get the file in place.

		//Note: We have the flexibility to set the location and name of the file under which it is to be saved on our local machine.

		/// <summary>
		/// get contents of a folder eg. attachments\ftpget - no trailing slash
		/// </summary>
		/// <param name="serverFolderPath"></param>
		/// <returns></returns>
		public string[] GetFileList(string serverFolderPath) {
			string[] downloadFiles;
			StringBuilder result = new StringBuilder();
			FtpWebRequest reqFTP;
			try {
				reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/"));
				//reqFTP.RenameTo //cool, try this
				reqFTP.UseBinary = true;
				reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
				reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
				WebResponse response = reqFTP.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream());
				string line = reader.ReadLine();
				while (line != null) {
					result.Append(line);
					result.Append("\n");
					line = reader.ReadLine();
				}
				// to remove the trailing '\n'
				result.Remove(result.ToString().LastIndexOf('\n'), 1);
				reader.Close();
				response.Close();
				return result.ToString().Split('\n');
			} catch (Exception ex) {
				//System.Windows.Forms.MessageBox.Show(ex.Message);
				Logging.dout("List Error ["+ex.Message+"]");

				downloadFiles = null;
				return downloadFiles;
			}
		}
	}
}