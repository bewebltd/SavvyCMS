#define FileSystem
#define PathAndFile

using System;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Text;
using Beweb;
using Microsoft.Win32;

namespace Beweb {
	/// <summary>
	/// implement things that the vb / asp filesystemobject does
	/// </summary>
	public class FileSystem {
		protected string _FileName = null;
		public string FileName {
			get { return _FileName; }
			set { _FileName = value; }
		}

		public static FileSystem OpenTextFile(string fileName) {
			FileSystem result = new FileSystem();
			result.FileName = fileName;

			//Pet my = new Pet(); //default to "cat"
			//my.PetTypeName = "Horse";
			//Console.Write("1pet[" + my.PetTypeName + "]");
			//Console.Write("2pet[" + my.ToString()+ "]");
			//Console.Write("3pet[" + my + "]");
			//Console.Write("4pet[" + my.ToString("hey") + "]");
			////Console.Write("5pet[" + my("hey") + "]");


			return result;
		}

		/// <summary>
		/// only for backward compat
		/// </summary>
		/// <returns></returns>
		public void close() {
		}

		/// <summary>
		/// Rename the current file
		/// </summary>
		/// <param name="newName">new file name</param>
		/// <returns>true if ok, if fail message in trace</returns>
		public bool Rename(string newName) {
			bool result = true;
			try {
				File.Move(FileName, newName);
			} catch (Exception ex) {
				HttpContext.Current.Trace.Warn(ex.Message);
				result = false;
			}
			return result;
		}
		/// <summary>
		/// Return true if current file exists
		/// </summary>
		/// <returns>true if exists</returns>
		public bool Exists() {
			return File.Exists(FileName);
		}

		/// <summary>
		/// Read the entire file into a string
		/// </summary>
		/// <returns>contents</returns>
		public string ReadAll() {
			// Open the stream and read it back.
			/*
			 * string fileSource = "";
			FileStream fs = null;
			try
			{

				// Open the stream and read it back.
				fs = File.Open(
					FileName,
					FileMode.Open,
					FileAccess.Read,
					FileShare.Read);
				
			}catch (Exception e)
			{
				//eout("Read File: failed to open file ["+FileName+"] ");
				Console.Write("Read File: failed to open file["+FileName+"]["+e.Message+"]");
			}


			int bytesRead = -1; 
			if (fs != null)
			{
				byte[] b = new byte[1024];
				UTF8Encoding temp = new UTF8Encoding(true);

				for (; bytesRead > 0 || bytesRead==-1; )
				{
					bytesRead = fs.Read(b, 0, b.Length);
					fileSource += temp.GetString(b);
				}
				fs.Close();
			} else
			{
				//eout("Read File: file closed["+FileName+"] ");
				Console.Write("Read File: file closed["+FileName+"]");
			}
			return fileSource.Substring(0,fileSource.Length-(1024-bytesRead));
			 */
			return File.ReadAllText(FileName);
		}

		/// <summary>
		/// Read the entire file into a string. Does not specify the encoding, therefore possibly it "auto detects" it?
		/// </summary>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public static string GetFileContents(string relativePath) {
			string path = Web.MapPath(relativePath);
			return File.ReadAllText(path);
		}

		/// <summary>
		/// Read the entire file into a string. Does not specify the encoding, therefore possibly it "auto detects" it?
		/// </summary>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public static string GetFileContentsAutoDetectEncoding(string relativePath) {
			string path = Web.MapPath(relativePath);
			var bytes = File.ReadAllBytes(path);
			var encoding = DetectEncoding(bytes);
			var str = encoding.GetString(bytes);
			return str;
		}

		public static DateTime GetFileDate(string relativePath) {
			string path = Web.MapPath(relativePath);
			DateTime creationTime = File.GetCreationTime(path);
			return creationTime;
		}

		/// <summary>
		/// Read the entire file into a string
		/// </summary>
		/// <param name="relativePath"></param>
		/// <param name="isUnicode"></param>
		/// <returns></returns>
		public static string GetFileContents(string relativePath, bool isUnicode) {
			string path = Web.MapPath(relativePath);
			return File.ReadAllText(path, isUnicode ? Encoding.Unicode : Encoding.ASCII);
		}

		/// <summary>
		/// Read the entire file into a string (same as GetFileContents) - assumes unicode file.
		/// See also ReadTextFileAutoDetectEncoding which may be better.
		/// </summary>
		public static string ReadTextFile(string filePath) {
			return GetFileContents(filePath, true);
		}

		/// <summary>
		/// Read the entire file into a string (same as GetFileContents) - attempts to detect the encoding, so generally better than ReadTextFile.
		/// </summary>
		public static string ReadTextFileAutoDetectEncoding(string filePath) {
			return GetFileContentsAutoDetectEncoding(filePath);
		}

		/// <summary>
		///  This works sometimes, but not always
		/// </summary>
		public static Encoding DetectEncoding(byte[] fileContent) {
			if (fileContent == null)
				throw new ArgumentNullException();

			if (fileContent.Length < 2)
				return Encoding.ASCII;      // Default fallback

			if (fileContent[0] == 0xff
					&& fileContent[1] == 0xfe
					&& (fileContent.Length < 4
							|| fileContent[2] != 0
							|| fileContent[3] != 0
							)
					)
				return Encoding.Unicode;

			if (fileContent[0] == 0xfe
					&& fileContent[1] == 0xff
					)
				return Encoding.BigEndianUnicode;

			if (fileContent.Length < 3)
				return null;

			if (fileContent[0] == 0xef && fileContent[1] == 0xbb && fileContent[2] == 0xbf)
				return Encoding.UTF8;

			if (fileContent[0] == 0x2b && fileContent[1] == 0x2f && fileContent[2] == 0x76)
				return Encoding.UTF7;

			if (fileContent.Length < 4)
				return null;

			if (fileContent[0] == 0xff && fileContent[1] == 0xfe && fileContent[2] == 0 && fileContent[3] == 0)
				return Encoding.UTF32;

			if (fileContent[0] == 0 && fileContent[1] == 0 && fileContent[2] == 0xfe && fileContent[3] == 0xff)
				return Encoding.GetEncoding(12001); 

			String probe;
			int len = fileContent.Length;

			if (fileContent.Length >= 128) len = 128;
			probe = Encoding.ASCII.GetString(fileContent, 0, len);

			MatchCollection mc = Regex.Matches(probe, "^<\\?xml[^<>]*encoding[ \\t\\n\\r]?=[\\t\\n\\r]?['\"]([A-Za-z]([A-Za-z0-9._]|-)*)", RegexOptions.Singleline);
			// Add '[0].Groups[1].Value' to the end to test regex

			if (mc.Count == 1 && mc[0].Groups.Count >= 2) {
				// Typically picks up 'UTF-8' string
				Encoding enc = null;

				try {
					enc = Encoding.GetEncoding(mc[0].Groups[1].Value);
				} catch (Exception) { }

				if (enc != null)
					return enc;
			}

			return Encoding.Default;      // Default fallback
		}


		/// <summary>
		/// insert a sufix into a filename (eg "_tn")
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static string InsertSuffix(string filename, string suffix) {
			// MN 20100908 replaced two lines below with better code moved from imageprocessing
			//int posn = file.LastIndexOf('.');
			//return (posn!=-1)?file.Substring(0,posn)+suffix+file.Substring(posn):file;
			string returnValue = "";
			if (!String.IsNullOrEmpty(filename)) {
				if (filename.LastIndexOf(".") > 0) {
					returnValue = filename.Insert(filename.LastIndexOf("."), suffix + "");
				} else {
					returnValue = filename + suffix;     // if no extension, still return the filename and just append suffix 
				}
			}
			return returnValue;

		}

		/// <summary>
		/// return true if the given path exists
		/// </summary>
		/// <param name="p">full path</param>
		/// <returns></returns>
		public static bool FolderExists(string p) {
			DirectoryInfo di = new DirectoryInfo(p);
			return (di.Exists);

		}
		/// <summary>
		/// return true if the given file exists
		/// </summary>
		/// <param name="p">full path from root of drive, or relative from root of site (mappath will be called in that case)</param>
		/// <returns></returns>
		public static bool FileExists(string p) {
			FileInfo fi = new FileInfo(Web.MapPath(p));
			return (fi.Exists);
		}

		/// <summary>
		/// returns the file name with the query removed eg file.png?x=123 becomes file.png
		/// </summary>
		/// <param name="filename">The name of the file</param>
		/// <returns>The trimmed name of the file</returns>
		public static string RemoveQueryFromFile(string filename) {
			if (filename.Contains("?")) {
				filename = filename.Substring(0, filename.IndexOf("?"));
			}
			return filename;
		}
		/// return true if an attachment exists 
		/// </summary>
		/// <param name="filename">only file name, no path</param>
		/// <returns>return true if an attachment exists</returns>
		public static bool FileAttachmentExists(string filename) {
			if (filename == null || filename.IsBlank()) return false;
			try {
				if (filename.Contains("?")) {
					filename = RemoveQueryFromFile(filename);
				}
				FileInfo fi = new FileInfo(Web.MapPath(Web.Attachments) + filename);
				return (fi.Exists);
			} catch (System.ArgumentException e) {
				throw new Beweb.ProgrammingErrorException("FileAttachmentExists Error: filename [" + filename + "] ", e);
			}
		}
		/// <summary>
		/// get file size in KB
		/// </summary>
		/// <param name="p">filename (relative to root)</param>
		/// <returns>size in KB</returns>
		public static int GetFileSize(string relativePath) {
			string path = Web.MapPath(relativePath);
			FileInfo fi = new FileInfo(path);
			return (int)Math.Round((fi.Length / 1024.0), 0);
		}

		/// <summary>
		/// get file size in bytes
		/// </summary>
		/// <param name="p">filename (relative to root)</param>
		public static int GetFileSizeBytes(string relativePath) {
			string path = Web.MapPath(relativePath);
			FileInfo fi = new FileInfo(path);
			return (int)fi.Length;
		}

		public static void Move(string sourcePath, string destinationPath, bool allowOverwrite) {
			if (!Path.HasExtension(destinationPath)) {
				destinationPath += "\\" + Path.GetFileName(sourcePath);
			}
			//todo add this CreateFolder(GetParentPath(destinationPath));
			if (FileExists(Web.MapPath(destinationPath))) {
				if (allowOverwrite) {
					File.Delete(Web.MapPath(destinationPath));
				} else {                                                                                              
					//destinationPath = GetUniqueFilename(destinationPath, false); //bad bad, removes path
				
					var newFileName = GetUniqueFilename(destinationPath, false);
					destinationPath = destinationPath.Substring(0,destinationPath.LastIndexOf("\\"))+"\\"+newFileName;       //20150130jn fixed this, was killing off the path before when getting a unique name
				}
			}
			try {
				File.Move(sourcePath, destinationPath);
			} catch (System.UnauthorizedAccessException ex) {

				throw new ProgrammingErrorException("Error moving file s[" + sourcePath + "] d[" + destinationPath + "] " + ex.Message, ex);
			}

		}

		public static void Delete(string filename) {
			if (!String.IsNullOrEmpty(filename)) {
				if (FileExists(Web.MapPath(filename))) {
					File.Delete(Web.MapPath(filename));
				}
			}
		}

		public static void DeleteDirectory(string target_dir) {
			string[] files = Directory.GetFiles(target_dir);
			string[] dirs = Directory.GetDirectories(target_dir);

			foreach (string file in files) {
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs) {
				DeleteDirectory(dir);
			}

			Directory.Delete(target_dir, false);
		}
		public static bool DeleteAttachment(string filename) {
			bool result = true;
			if (!String.IsNullOrEmpty(filename)) {
				if (FileExists(Web.MapPath(Web.Attachments + filename))) {
					try {
					File.Delete(Web.MapPath(Web.Attachments + filename));

					} catch (Exception) {
						result=false;
					}
				}
			}
			return result;
		}

		public static void DeletePictureAttachment(string picture) {
			//if (picture.IsBlank())  throw new BewebException("Beweb.FileSystem.DeletePictureAttachment(picture): picture file name is blank.");
			if (!String.IsNullOrEmpty(picture)) {
				DeleteAttachment(InsertSuffix(picture, "_tn"));
				DeleteAttachment(InsertSuffix(picture, "_pv"));
				DeleteAttachment(InsertSuffix(picture, "_med"));
				DeleteAttachment(InsertSuffix(picture, "_sml"));
				DeleteAttachment(picture);
			}
		}

		/// <summary>
		/// Creates a folder and parent folders if they do not already exist. 
		/// Path can be full physical (eg c:\blah) or app-relative (eg ~/blah).
		/// </summary>
		/// <param name="folderPath"></param>
		public static void CreateFolder(string folderPath) {
			string path = folderPath;
			if (path.StartsWith("~") || path.StartsWith("/")) path = Web.MapPath(path);
			if (!IsFolder(path)){
				path = Path.GetDirectoryName(path); //20141009 jn removed this thing mike added // 20141106 added back inside this IF statement
			}
			if (path!=null && !Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}

		/// <summary>
		/// Write a file with given text. Creates, writes text and closes file.
		/// If existing, it is overwritten.
		/// Creates folder and parent folders if they do not already exist. 
		/// Path can be full physical (eg c:\blah) or app-relative (eg ~/blah).
		/// </summary>
		/// <param name="folderPath"></param>
		public static void WriteTextFile(string filePath, string textContents) {
			string path = filePath;
			if (path.StartsWith("~")) path = Web.MapPath(path);
			CreateFolder(Path.GetDirectoryName(path));
			File.WriteAllText(path, textContents);
		}

#if	PathAndFile
		/// <summary>
		/// Turns a string such as a title into standard filename/URL without any special characters (ie lower case separated with dashes). This does not allow any dots.
		/// </summary>
		public static string CrunchFileName(string filename) {
			return PathAndFile.CrunchFileName(filename);
		}

		/// <summary>
		/// Turns a file name into santised standard filename/URL without any special characters (ie lower case separated with dashes). Allows for a dot and extension.
		/// </summary>
		public static string CleanFileName(string filename) {
			string fileBase = CrunchFileName(GetBaseName(filename));
			//string fileExt = Fmt.CleanString(GetExtension(filename).ToLowerInvariant(), "[^a-z0-9]");
			string fileExt = Fmt.CleanAlphaNumeric(GetExtension(filename).ToLowerInvariant());
			if (fileExt.IsNotBlank()) {
				return fileBase + "." + fileExt;
			}
			return fileBase;
		}
#endif
		/// <summary>
		/// return extn including dot
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string GetExtension(string filename) {
			//return Path.GetExtension(filename);  //MN 20130129 - this built-in method sucks - only allows valid Windows filenames, eg no good for attachment filenames
			//return filename.RightFrom(".");		 //JN 20130214 - put back in suck version as it returns extn including the dot
			var lastIndexOf = filename.LastIndexOf('.');
			if (lastIndexOf == -1) { return ""; }
			var extension = filename.Substring(lastIndexOf);
			return extension;
		}

		public static string GetBaseName(string filename) {
			string result = filename;
			try {
				//var extn = Path.GetExtension(filename);  MN 20130129 - this built-in method sucks - only allows valid Windows filenames, eg no good for attachment filenames
				var extn = GetExtension(filename);
				result = filename.RemoveSuffix(extn);
			} catch (Exception) {
				throw new Exception("GetBaseName: Error processing filename [" + filename + "] on " + DateTime.Now);
			}
			return result;
		}

#if	PathAndFile
		/// <summary>
		/// Given a filename, return a uniquely named filename that can go in the attachments folder.
		/// Returned filename will be relative to attachments path.
		/// Pass in a filename that may already exist, including relative path from attachments folder or full path.
		/// Cleans out any bad chars in filename and limits to 50 characters.
		/// Examples: 
		/// GetUniqueAttachmentFilename("subfolder/file's name.jpg")                    // assumes subfolder of attachments folder
		/// GetUniqueAttachmentFilename("file's name.jpg")                              // assumes in attachments folder
		/// GetUniqueAttachmentFilename("c:\websites\site\attachments\file's name.jpg") // full physical path including attachments
		/// GetUniqueAttachmentFilename(Web.Root+"attachments/file's name.jpg")         // from app root including attachments
		/// GetUniqueAttachmentFilename(Web.Attachments+"subfolder/file's name.jpg")    // from app root including attachments
		/// </summary>
		/// <param name="filename">filename that may already exist, including relative path from attachments folder or full path</param>
		/// <returns>filename-i where i is a unique number</returns>
		public static string GetUniqueAttachmentFilename(string filename) {
			// 20120927 MN: breaking change - slash used to be stripped out as invalid char, now it is treated as subdir
			return GetUniqueAttachmentFilename(filename, 50);
		}

		public static string GetUniqueAttachmentFilename(string filename, int maxLen) {
			//return GetUniqueFilename(Web.Attachments, filename);

			// be forgiving here - if passing in path from site root or drive root then take only from attachments
			string physicalAttachPath = Web.MapPath(Web.Attachments);
			string testPhysFile = Web.MapPath(filename);
			if (testPhysFile.StartsWith(physicalAttachPath)) {
				filename = testPhysFile.RemovePrefix(physicalAttachPath).RemovePrefix("/");
			}

			filename = filename.Replace("\\", "/");
			string subfolder = "";
			if (filename.Contains("/")) {
				subfolder = filename.LeftUntilLast("/");
				filename = filename.RightFrom("/");
			}
			return GetUniqueAttachmentFilename(subfolder, filename, maxLen);
		}

		/// <summary>
		/// Given a filename, return a uniquely named filename that can go in the attachments folder.
		/// Returned filename will be relative to attachments path (ie include the subfolder/ if supplied).
		/// Pass in a subfolder and filename that may already exist.
		/// Cleans out any bad chars in filename and limits to 50 characters.
		/// </summary>
		public static string GetUniqueAttachmentFilename(string subfolder, string filename) {
			return GetUniqueAttachmentFilename(subfolder, filename, 50);
		}

		/// <summary>
		/// Given a filename, return a uniquely named filename that can go in the attachments folder.
		/// Returned filename will be relative to attachments path (ie include the subfolder/ if supplied).
		/// Pass in a subfolder and filename that may already exist.
		/// Cleans out any bad chars in filename and limits to 50 characters.
		/// </summary>
		public static string GetUniqueAttachmentFilename(string subfolder, string filename, int maxLen) {
			maxLen = maxLen - subfolder.Length;
			CreateFolder(Web.Attachments + subfolder);
			filename = GetUniqueFilename(Web.Attachments + subfolder, filename, maxLen);
			if (subfolder != null && subfolder != "" && !subfolder.EndsWith("/")) {
				if (subfolder.EndsWith("\\")) {
					subfolder = subfolder.Replace("\\","/");
				} else {
					subfolder+="/";
				}
			}
			filename = subfolder + filename;
			return filename;
		}

		/// <summary>
		/// Given a path and a filename, return a uniquely named filename that can go in that path.
		/// Cleans out any bad chars in filename and limits to 50 characters.
		/// </summary>
		/// <param name="path">path from root of web site (not drive) (server.mappath will be called internally)</param>
		/// <param name="filename">filename that may already exist</param>
		/// <returns>filename-i where i is a unique number</returns>
		public static string GetUniqueFilename(string path, string filename) {
			return GetUniqueFilename(path, filename, 50);
		}

		public static string GetUniqueFilename(string filepath) {
			// split filepath
			filepath = filepath.Replace("\\", "/");
			string path = filepath.LeftUntilLast("/");
			string filename = filepath.RightFrom("/");
			return GetUniqueFilename(path, filename, 50);
		}

		public static string GetUniqueFilename(string path, bool cleanFilename) {
			return GetUniqueFilename(Path.GetDirectoryName(path), Path.GetFileName(path), 50, cleanFilename);
		}

		/// <summary>
		/// Given a path (virtual or physical) and a filename, return a uniquely named filename that can go in that path.
		/// If cleanFilename parameter is true, cleans out any bad chars in filename and limits to specified number of  characters.
		/// </summary>
		/// <param name="path">path from root of web site (not drive) (server.mappath will be called internally)</param>
		/// <param name="filename">filename that may already exist</param>
		/// <param name="maxLen">Max length of filename - will be trimmed if too long</param>
		/// <returns>filename-i where i is a unique number</returns>
		public static string GetUniqueFilename(string path, string filename, int maxLen) {
			return GetUniqueFilename(path, filename, maxLen, true);
		}
		public static string GetUniqueFilename(string path, string filename, int maxLen, bool cleanFilename) {
			if (filename == null) throw new Exception("GetUniqueFilename: filename cannot be null");

			if (cleanFilename) filename = CleanFileName(filename);

			string physicalPath = Web.MapPath(path);
			string fileBase = GetBaseName(filename);
			string fileExt = GetExtension(filename);
			int fileExtLength = fileExt.Length;

			// ensure it is not too long
			if (filename.Length > maxLen) {
				filename = fileBase.Left(maxLen - fileExtLength).TrimEnd('-') + fileExt;
			}

			// ensure filename is unique (does not already exist)
			// if not unique, add a number on the end
			int fileNum = 0;
			if (!physicalPath.EndsWith("\\")) physicalPath += "\\";						 //20150130 jn next line adds file name to path - check we have a \\ (slash)
			while (File.Exists(physicalPath + filename)) {
				fileNum++;
				string suffix = "-" + fileNum;
				filename = fileBase.Left(maxLen - suffix.Length - fileExtLength) + suffix + fileExt;
			}
			return filename;
		}
		public static string GuidFileName() {
			return CleanFileName(Guid.NewGuid().ToString());
		}
#endif

		/// <summary>
		/// Gets parent of this file or directory.
		/// Goes back to the last slash, or if the last char is a slash, the one before.
		/// Examples:
		/// HondaPublicServices\Autobase\BAL1002.txt => HondaPublicServices\Autobase
		/// HondaPublicServices\Autobase\ => HondaPublicServices
		/// HondaPublicServices\Autobase => HondaPublicServices
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetParentPath(string filePath) {
			// MN 20130418 - breaking change - beware - now removes any trailing slash, pretty sure the original intent was HondaPublicServices\Autobase\ => HondaPublicServices
			filePath.RemoveSuffix("\\").RemoveSuffix("/");
			return System.IO.Directory.GetParent(filePath).FullName;
		}

		/// <summary>
		/// Filename can be a full physical or relative app path.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string SaveStreamToFile(Stream stream, string filename) {
			filename = Web.MapPath(filename);
			CreateFolder(GetParentPath(filename));
			try {
				int length = 256;
				int bytesRead = 0;
				Byte[] buffer = new Byte[length];

				if (stream.CanSeek) {
					stream.Position = 0;
				}

				// write the required bytes
				using (FileStream fs = new FileStream(filename, FileMode.Create)) {
					do {
						bytesRead = stream.Read(buffer, 0, length);
						fs.Write(buffer, 0, bytesRead);
					} while (bytesRead == length);
				}

				stream.Dispose();
				return filename;
			} catch (Exception ex) {
				throw new Exception("SaveStreamToFile: An unexpected error occured uploading the file. " + ex.Message);
			}
		}

		/// <summary>
		/// Return a filestream.  
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static FileStream OpenStream(string filename) {
			filename = Web.MapPath(filename);
			return File.Open(filename, FileMode.Open);
		}

		/// <summary>
		/// Returns true if the filename represents an executable file that could be dangerous on a web server - should be used to prevent executable files being uploaded
		/// </summary>
		public static bool IsDangerous(string filename) {
			string badFileExtensions = "asp,aspx,com,php,pl,cgi,ADE,ADP,BAS,BAT,CHM,CMD,COM,CPL,CRT,DLL,EXE,HLP,HTA,INF,INS,ISP,JS,JSE,LNK,MDB,MDE,MSC,MSI,MSP,MST,OCX,PCD,PIF,REG,SCR,SCT,SHB,SHS,SYS,URL,VB,VBE,VBS,WSC,WSF,WSH";
			string ext = GetExtension(filename);
			return badFileExtensions.Split(',').ContainsInsensitive(ext.RemovePrefix("."));
		}

		public static string GetMimeType(string extension) {
			if (extension == null) {
				throw new ArgumentNullException("extension");
			}

			if (!extension.StartsWith(".")) {
				extension = "." + extension;
			}

			switch (extension) {
				#region Big freaking list of mime types

				// combination of values from Windows 7 Registry and 
				// from C:\Windows\System32\inetsrv\config\applicationHost.config
				// some added, including .7z and .dat

				case ".323":
					return "text/h323";
				case ".3g2":
					return "video/3gpp2";
				case ".3gp":
					return "video/3gpp";
				case ".3gp2":
					return "video/3gpp2";
				case ".3gpp":
					return "video/3gpp";
				case ".7z":
					return "application/x-7z-compressed";
				case ".aa":
					return "audio/audible";
				case ".AAC":
					return "audio/aac";
				case ".aaf":
					return "application/octet-stream";
				case ".aax":
					return "audio/vnd.audible.aax";
				case ".ac3":
					return "audio/ac3";
				case ".aca":
					return "application/octet-stream";
				case ".accda":
					return "application/msaccess.addin";
				case ".accdb":
					return "application/msaccess";
				case ".accdc":
					return "application/msaccess.cab";
				case ".accde":
					return "application/msaccess";
				case ".accdr":
					return "application/msaccess.runtime";
				case ".accdt":
					return "application/msaccess";
				case ".accdw":
					return "application/msaccess.webapplication";
				case ".accft":
					return "application/msaccess.ftemplate";
				case ".acx":
					return "application/internet-property-stream";
				case ".AddIn":
					return "text/xml";
				case ".ade":
					return "application/msaccess";
				case ".adobebridge":
					return "application/x-bridge-url";
				case ".adp":
					return "application/msaccess";
				case ".ADT":
					return "audio/vnd.dlna.adts";
				case ".ADTS":
					return "audio/aac";
				case ".afm":
					return "application/octet-stream";
				case ".ai":
					return "application/postscript";
				case ".aif":
					return "audio/x-aiff";
				case ".aifc":
					return "audio/aiff";
				case ".aiff":
					return "audio/aiff";
				case ".air":
					return "application/vnd.adobe.air-application-installer-package+zip";
				case ".amc":
					return "application/x-mpeg";
				case ".application":
					return "application/x-ms-application";
				case ".art":
					return "image/x-jg";
				case ".asa":
					return "application/xml";
				case ".asax":
					return "application/xml";
				case ".ascx":
					return "application/xml";
				case ".asd":
					return "application/octet-stream";
				case ".asf":
					return "video/x-ms-asf";
				case ".ashx":
					return "application/xml";
				case ".asi":
					return "application/octet-stream";
				case ".asm":
					return "text/plain";
				case ".asmx":
					return "application/xml";
				case ".aspx":
					return "application/xml";
				case ".asr":
					return "video/x-ms-asf";
				case ".asx":
					return "video/x-ms-asf";
				case ".atom":
					return "application/atom+xml";
				case ".au":
					return "audio/basic";
				case ".avi":
					return "video/x-msvideo";
				case ".axs":
					return "application/olescript";
				case ".bas":
					return "text/plain";
				case ".bcpio":
					return "application/x-bcpio";
				case ".bin":
					return "application/octet-stream";
				case ".bmp":
					return "image/bmp";
				case ".c":
					return "text/plain";
				case ".cab":
					return "application/octet-stream";
				case ".caf":
					return "audio/x-caf";
				case ".calx":
					return "application/vnd.ms-office.calx";
				case ".cat":
					return "application/vnd.ms-pki.seccat";
				case ".cc":
					return "text/plain";
				case ".cd":
					return "text/plain";
				case ".cdda":
					return "audio/aiff";
				case ".cdf":
					return "application/x-cdf";
				case ".cer":
					return "application/x-x509-ca-cert";
				case ".chm":
					return "application/octet-stream";
				case ".class":
					return "application/x-java-applet";
				case ".clp":
					return "application/x-msclip";
				case ".cmx":
					return "image/x-cmx";
				case ".cnf":
					return "text/plain";
				case ".cod":
					return "image/cis-cod";
				case ".config":
					return "application/xml";
				case ".contact":
					return "text/x-ms-contact";
				case ".coverage":
					return "application/xml";
				case ".cpio":
					return "application/x-cpio";
				case ".cpp":
					return "text/plain";
				case ".crd":
					return "application/x-mscardfile";
				case ".crl":
					return "application/pkix-crl";
				case ".crt":
					return "application/x-x509-ca-cert";
				case ".cs":
					return "text/plain";
				case ".csdproj":
					return "text/plain";
				case ".csh":
					return "application/x-csh";
				case ".csproj":
					return "text/plain";
				case ".css":
					return "text/css";
				case ".csv":
					return "application/octet-stream";
				case ".cur":
					return "application/octet-stream";
				case ".cxx":
					return "text/plain";
				case ".dat":
					return "application/octet-stream";
				case ".datasource":
					return "application/xml";
				case ".dbproj":
					return "text/plain";
				case ".dcr":
					return "application/x-director";
				case ".def":
					return "text/plain";
				case ".deploy":
					return "application/octet-stream";
				case ".der":
					return "application/x-x509-ca-cert";
				case ".dgml":
					return "application/xml";
				case ".dib":
					return "image/bmp";
				case ".dif":
					return "video/x-dv";
				case ".dir":
					return "application/x-director";
				case ".disco":
					return "text/xml";
				case ".dll":
					return "application/x-msdownload";
				case ".dll.config":
					return "text/xml";
				case ".dlm":
					return "text/dlm";
				case ".doc":
					return "application/msword";
				case ".docm":
					return "application/vnd.ms-word.document.macroEnabled.12";
				case ".docx":
					return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				case ".dot":
					return "application/msword";
				case ".dotm":
					return "application/vnd.ms-word.template.macroEnabled.12";
				case ".dotx":
					return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
				case ".dsp":
					return "application/octet-stream";
				case ".dsw":
					return "text/plain";
				case ".dtd":
					return "text/xml";
				case ".dtsConfig":
					return "text/xml";
				case ".dv":
					return "video/x-dv";
				case ".dvi":
					return "application/x-dvi";
				case ".dwf":
					return "drawing/x-dwf";
				case ".dwp":
					return "application/octet-stream";
				case ".dxr":
					return "application/x-director";
				case ".eml":
					return "message/rfc822";
				case ".emz":
					return "application/octet-stream";
				case ".eot":
					return "application/octet-stream";
				case ".eps":
					return "application/postscript";
				case ".etl":
					return "application/etl";
				case ".etx":
					return "text/x-setext";
				case ".evy":
					return "application/envoy";
				case ".exe":
					return "application/octet-stream";
				case ".exe.config":
					return "text/xml";
				case ".fdf":
					return "application/vnd.fdf";
				case ".fif":
					return "application/fractals";
				case ".filters":
					return "Application/xml";
				case ".fla":
					return "application/octet-stream";
				case ".flr":
					return "x-world/x-vrml";
				case ".flv":
					return "video/x-flv";
				case ".fsscript":
					return "application/fsharp-script";
				case ".fsx":
					return "application/fsharp-script";
				case ".generictest":
					return "application/xml";
				case ".gif":
					return "image/gif";
				case ".group":
					return "text/x-ms-group";
				case ".gsm":
					return "audio/x-gsm";
				case ".gtar":
					return "application/x-gtar";
				case ".gz":
					return "application/x-gzip";
				case ".h":
					return "text/plain";
				case ".hdf":
					return "application/x-hdf";
				case ".hdml":
					return "text/x-hdml";
				case ".hhc":
					return "application/x-oleobject";
				case ".hhk":
					return "application/octet-stream";
				case ".hhp":
					return "application/octet-stream";
				case ".hlp":
					return "application/winhlp";
				case ".hpp":
					return "text/plain";
				case ".hqx":
					return "application/mac-binhex40";
				case ".hta":
					return "application/hta";
				case ".htc":
					return "text/x-component";
				case ".htm":
					return "text/html";
				case ".html":
					return "text/html";
				case ".htt":
					return "text/webviewhtml";
				case ".hxa":
					return "application/xml";
				case ".hxc":
					return "application/xml";
				case ".hxd":
					return "application/octet-stream";
				case ".hxe":
					return "application/xml";
				case ".hxf":
					return "application/xml";
				case ".hxh":
					return "application/octet-stream";
				case ".hxi":
					return "application/octet-stream";
				case ".hxk":
					return "application/xml";
				case ".hxq":
					return "application/octet-stream";
				case ".hxr":
					return "application/octet-stream";
				case ".hxs":
					return "application/octet-stream";
				case ".hxt":
					return "text/html";
				case ".hxv":
					return "application/xml";
				case ".hxw":
					return "application/octet-stream";
				case ".hxx":
					return "text/plain";
				case ".i":
					return "text/plain";
				case ".ico":
					return "image/x-icon";
				case ".ics":
					return "application/octet-stream";
				case ".idl":
					return "text/plain";
				case ".ief":
					return "image/ief";
				case ".iii":
					return "application/x-iphone";
				case ".inc":
					return "text/plain";
				case ".inf":
					return "application/octet-stream";
				case ".inl":
					return "text/plain";
				case ".ins":
					return "application/x-internet-signup";
				case ".ipa":
					return "application/x-itunes-ipa";
				case ".ipg":
					return "application/x-itunes-ipg";
				case ".ipproj":
					return "text/plain";
				case ".ipsw":
					return "application/x-itunes-ipsw";
				case ".iqy":
					return "text/x-ms-iqy";
				case ".isp":
					return "application/x-internet-signup";
				case ".ite":
					return "application/x-itunes-ite";
				case ".itlp":
					return "application/x-itunes-itlp";
				case ".itms":
					return "application/x-itunes-itms";
				case ".itpc":
					return "application/x-itunes-itpc";
				case ".IVF":
					return "video/x-ivf";
				case ".jar":
					return "application/java-archive";
				case ".java":
					return "application/octet-stream";
				case ".jck":
					return "application/liquidmotion";
				case ".jcz":
					return "application/liquidmotion";
				case ".jfif":
					return "image/pjpeg";
				case ".jnlp":
					return "application/x-java-jnlp-file";
				case ".jpb":
					return "application/octet-stream";
				case ".jpe":
					return "image/jpeg";
				case ".jpeg":
					return "image/jpeg";
				case ".jpg":
					return "image/jpeg";
				case ".js":
					return "application/x-javascript";
				case ".jsx":
					return "text/jscript";
				case ".jsxbin":
					return "text/plain";
				case ".latex":
					return "application/x-latex";
				case ".library-ms":
					return "application/windows-library+xml";
				case ".lit":
					return "application/x-ms-reader";
				case ".loadtest":
					return "application/xml";
				case ".lpk":
					return "application/octet-stream";
				case ".lsf":
					return "video/x-la-asf";
				case ".lst":
					return "text/plain";
				case ".lsx":
					return "video/x-la-asf";
				case ".lzh":
					return "application/octet-stream";
				case ".m13":
					return "application/x-msmediaview";
				case ".m14":
					return "application/x-msmediaview";
				case ".m1v":
					return "video/mpeg";
				case ".m2t":
					return "video/vnd.dlna.mpeg-tts";
				case ".m2ts":
					return "video/vnd.dlna.mpeg-tts";
				case ".m2v":
					return "video/mpeg";
				case ".m3u":
					return "audio/x-mpegurl";
				case ".m3u8":
					return "audio/x-mpegurl";
				case ".m4a":
					return "audio/m4a";
				case ".m4b":
					return "audio/m4b";
				case ".m4p":
					return "audio/m4p";
				case ".m4r":
					return "audio/x-m4r";
				case ".m4v":
					return "video/x-m4v";
				case ".mac":
					return "image/x-macpaint";
				case ".mak":
					return "text/plain";
				case ".man":
					return "application/x-troff-man";
				case ".manifest":
					return "application/x-ms-manifest";
				case ".map":
					return "text/plain";
				case ".master":
					return "application/xml";
				case ".mda":
					return "application/msaccess";
				case ".mdb":
					return "application/x-msaccess";
				case ".mde":
					return "application/msaccess";
				case ".mdp":
					return "application/octet-stream";
				case ".me":
					return "application/x-troff-me";
				case ".mfp":
					return "application/x-shockwave-flash";
				case ".mht":
					return "message/rfc822";
				case ".mhtml":
					return "message/rfc822";
				case ".mid":
					return "audio/mid";
				case ".midi":
					return "audio/mid";
				case ".mix":
					return "application/octet-stream";
				case ".mk":
					return "text/plain";
				case ".mmf":
					return "application/x-smaf";
				case ".mno":
					return "text/xml";
				case ".mny":
					return "application/x-msmoney";
				case ".mod":
					return "video/mpeg";
				case ".mov":
					return "video/quicktime";
				case ".movie":
					return "video/x-sgi-movie";
				case ".mp2":
					return "video/mpeg";
				case ".mp2v":
					return "video/mpeg";
				case ".mp3":
					return "audio/mpeg";
				case ".mp4":
					return "video/mp4";
				case ".mp4v":
					return "video/mp4";
				case ".mpa":
					return "video/mpeg";
				case ".mpe":
					return "video/mpeg";
				case ".mpeg":
					return "video/mpeg";
				case ".mpf":
					return "application/vnd.ms-mediapackage";
				case ".mpg":
					return "video/mpeg";
				case ".mpp":
					return "application/vnd.ms-project";
				case ".mpv2":
					return "video/mpeg";
				case ".mqv":
					return "video/quicktime";
				case ".ms":
					return "application/x-troff-ms";
				case ".msi":
					return "application/octet-stream";
				case ".mso":
					return "application/octet-stream";
				case ".mts":
					return "video/vnd.dlna.mpeg-tts";
				case ".mtx":
					return "application/xml";
				case ".mvb":
					return "application/x-msmediaview";
				case ".mvc":
					return "application/x-miva-compiled";
				case ".mxp":
					return "application/x-mmxp";
				case ".nc":
					return "application/x-netcdf";
				case ".nsc":
					return "video/x-ms-asf";
				case ".nws":
					return "message/rfc822";
				case ".ocx":
					return "application/octet-stream";
				case ".oda":
					return "application/oda";
				case ".odc":
					return "text/x-ms-odc";
				case ".odh":
					return "text/plain";
				case ".odl":
					return "text/plain";
				case ".odp":
					return "application/vnd.oasis.opendocument.presentation";
				case ".ods":
					return "application/oleobject";
				case ".odt":
					return "application/vnd.oasis.opendocument.text";
				case ".one":
					return "application/onenote";
				case ".onea":
					return "application/onenote";
				case ".onepkg":
					return "application/onenote";
				case ".onetmp":
					return "application/onenote";
				case ".onetoc":
					return "application/onenote";
				case ".onetoc2":
					return "application/onenote";
				case ".orderedtest":
					return "application/xml";
				case ".osdx":
					return "application/opensearchdescription+xml";
				case ".p10":
					return "application/pkcs10";
				case ".p12":
					return "application/x-pkcs12";
				case ".p7b":
					return "application/x-pkcs7-certificates";
				case ".p7c":
					return "application/pkcs7-mime";
				case ".p7m":
					return "application/pkcs7-mime";
				case ".p7r":
					return "application/x-pkcs7-certreqresp";
				case ".p7s":
					return "application/pkcs7-signature";
				case ".pbm":
					return "image/x-portable-bitmap";
				case ".pcast":
					return "application/x-podcast";
				case ".pct":
					return "image/pict";
				case ".pcx":
					return "application/octet-stream";
				case ".pcz":
					return "application/octet-stream";
				case ".pdf":
					return "application/pdf";
				case ".pfb":
					return "application/octet-stream";
				case ".pfm":
					return "application/octet-stream";
				case ".pfx":
					return "application/x-pkcs12";
				case ".pgm":
					return "image/x-portable-graymap";
				case ".pic":
					return "image/pict";
				case ".pict":
					return "image/pict";
				case ".pkgdef":
					return "text/plain";
				case ".pkgundef":
					return "text/plain";
				case ".pko":
					return "application/vnd.ms-pki.pko";
				case ".pls":
					return "audio/scpls";
				case ".pma":
					return "application/x-perfmon";
				case ".pmc":
					return "application/x-perfmon";
				case ".pml":
					return "application/x-perfmon";
				case ".pmr":
					return "application/x-perfmon";
				case ".pmw":
					return "application/x-perfmon";
				case ".png":
					return "image/png";
				case ".pnm":
					return "image/x-portable-anymap";
				case ".pnt":
					return "image/x-macpaint";
				case ".pntg":
					return "image/x-macpaint";
				case ".pnz":
					return "image/png";
				case ".pot":
					return "application/vnd.ms-powerpoint";
				case ".potm":
					return "application/vnd.ms-powerpoint.template.macroEnabled.12";
				case ".potx":
					return "application/vnd.openxmlformats-officedocument.presentationml.template";
				case ".ppa":
					return "application/vnd.ms-powerpoint";
				case ".ppam":
					return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
				case ".ppm":
					return "image/x-portable-pixmap";
				case ".pps":
					return "application/vnd.ms-powerpoint";
				case ".ppsm":
					return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
				case ".ppsx":
					return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
				case ".ppt":
					return "application/vnd.ms-powerpoint";
				case ".pptm":
					return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
				case ".pptx":
					return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
				case ".prf":
					return "application/pics-rules";
				case ".prm":
					return "application/octet-stream";
				case ".prx":
					return "application/octet-stream";
				case ".ps":
					return "application/postscript";
				case ".psc1":
					return "application/PowerShell";
				case ".psd":
					return "application/octet-stream";
				case ".psess":
					return "application/xml";
				case ".psm":
					return "application/octet-stream";
				case ".psp":
					return "application/octet-stream";
				case ".pub":
					return "application/x-mspublisher";
				case ".pwz":
					return "application/vnd.ms-powerpoint";
				case ".qht":
					return "text/x-html-insertion";
				case ".qhtm":
					return "text/x-html-insertion";
				case ".qt":
					return "video/quicktime";
				case ".qti":
					return "image/x-quicktime";
				case ".qtif":
					return "image/x-quicktime";
				case ".qtl":
					return "application/x-quicktimeplayer";
				case ".qxd":
					return "application/octet-stream";
				case ".ra":
					return "audio/x-pn-realaudio";
				case ".ram":
					return "audio/x-pn-realaudio";
				case ".rar":
					return "application/octet-stream";
				case ".ras":
					return "image/x-cmu-raster";
				case ".rat":
					return "application/rat-file";
				case ".rc":
					return "text/plain";
				case ".rc2":
					return "text/plain";
				case ".rct":
					return "text/plain";
				case ".rdlc":
					return "application/xml";
				case ".resx":
					return "application/xml";
				case ".rf":
					return "image/vnd.rn-realflash";
				case ".rgb":
					return "image/x-rgb";
				case ".rgs":
					return "text/plain";
				case ".rm":
					return "application/vnd.rn-realmedia";
				case ".rmi":
					return "audio/mid";
				case ".rmp":
					return "application/vnd.rn-rn_music_package";
				case ".roff":
					return "application/x-troff";
				case ".rpm":
					return "audio/x-pn-realaudio-plugin";
				case ".rqy":
					return "text/x-ms-rqy";
				case ".rtf":
					return "application/rtf";
				case ".rtx":
					return "text/richtext";
				case ".ruleset":
					return "application/xml";
				case ".s":
					return "text/plain";
				case ".safariextz":
					return "application/x-safari-safariextz";
				case ".scd":
					return "application/x-msschedule";
				case ".sct":
					return "text/scriptlet";
				case ".sd2":
					return "audio/x-sd2";
				case ".sdp":
					return "application/sdp";
				case ".sea":
					return "application/octet-stream";
				case ".searchConnector-ms":
					return "application/windows-search-connector+xml";
				case ".setpay":
					return "application/set-payment-initiation";
				case ".setreg":
					return "application/set-registration-initiation";
				case ".settings":
					return "application/xml";
				case ".sgimb":
					return "application/x-sgimb";
				case ".sgml":
					return "text/sgml";
				case ".sh":
					return "application/x-sh";
				case ".shar":
					return "application/x-shar";
				case ".shtml":
					return "text/html";
				case ".sit":
					return "application/x-stuffit";
				case ".sitemap":
					return "application/xml";
				case ".skin":
					return "application/xml";
				case ".sldm":
					return "application/vnd.ms-powerpoint.slide.macroEnabled.12";
				case ".sldx":
					return "application/vnd.openxmlformats-officedocument.presentationml.slide";
				case ".slk":
					return "application/vnd.ms-excel";
				case ".sln":
					return "text/plain";
				case ".slupkg-ms":
					return "application/x-ms-license";
				case ".smd":
					return "audio/x-smd";
				case ".smi":
					return "application/octet-stream";
				case ".smx":
					return "audio/x-smd";
				case ".smz":
					return "audio/x-smd";
				case ".snd":
					return "audio/basic";
				case ".snippet":
					return "application/xml";
				case ".snp":
					return "application/octet-stream";
				case ".sol":
					return "text/plain";
				case ".sor":
					return "text/plain";
				case ".spc":
					return "application/x-pkcs7-certificates";
				case ".spl":
					return "application/futuresplash";
				case ".src":
					return "application/x-wais-source";
				case ".srf":
					return "text/plain";
				case ".SSISDeploymentManifest":
					return "text/xml";
				case ".ssm":
					return "application/streamingmedia";
				case ".sst":
					return "application/vnd.ms-pki.certstore";
				case ".stl":
					return "application/vnd.ms-pki.stl";
				case ".sv4cpio":
					return "application/x-sv4cpio";
				case ".sv4crc":
					return "application/x-sv4crc";
				case ".svc":
					return "application/xml";
				case ".swf":
					return "application/x-shockwave-flash";
				case ".t":
					return "application/x-troff";
				case ".tar":
					return "application/x-tar";
				case ".tcl":
					return "application/x-tcl";
				case ".testrunconfig":
					return "application/xml";
				case ".testsettings":
					return "application/xml";
				case ".tex":
					return "application/x-tex";
				case ".texi":
					return "application/x-texinfo";
				case ".texinfo":
					return "application/x-texinfo";
				case ".tgz":
					return "application/x-compressed";
				case ".thmx":
					return "application/vnd.ms-officetheme";
				case ".thn":
					return "application/octet-stream";
				case ".tif":
					return "image/tiff";
				case ".tiff":
					return "image/tiff";
				case ".tlh":
					return "text/plain";
				case ".tli":
					return "text/plain";
				case ".toc":
					return "application/octet-stream";
				case ".tr":
					return "application/x-troff";
				case ".trm":
					return "application/x-msterminal";
				case ".trx":
					return "application/xml";
				case ".ts":
					return "video/vnd.dlna.mpeg-tts";
				case ".tsv":
					return "text/tab-separated-values";
				case ".ttf":
					return "application/octet-stream";
				case ".tts":
					return "video/vnd.dlna.mpeg-tts";
				case ".txt":
					return "text/plain";
				case ".u32":
					return "application/octet-stream";
				case ".uls":
					return "text/iuls";
				case ".user":
					return "text/plain";
				case ".ustar":
					return "application/x-ustar";
				case ".vb":
					return "text/plain";
				case ".vbdproj":
					return "text/plain";
				case ".vbk":
					return "video/mpeg";
				case ".vbproj":
					return "text/plain";
				case ".vbs":
					return "text/vbscript";
				case ".vcf":
					return "text/x-vcard";
				case ".vcproj":
					return "Application/xml";
				case ".vcs":
					return "text/plain";
				case ".vcxproj":
					return "Application/xml";
				case ".vddproj":
					return "text/plain";
				case ".vdp":
					return "text/plain";
				case ".vdproj":
					return "text/plain";
				case ".vdx":
					return "application/vnd.ms-visio.viewer";
				case ".vml":
					return "text/xml";
				case ".vscontent":
					return "application/xml";
				case ".vsct":
					return "text/xml";
				case ".vsd":
					return "application/vnd.visio";
				case ".vsi":
					return "application/ms-vsi";
				case ".vsix":
					return "application/vsix";
				case ".vsixlangpack":
					return "text/xml";
				case ".vsixmanifest":
					return "text/xml";
				case ".vsmdi":
					return "application/xml";
				case ".vspscc":
					return "text/plain";
				case ".vss":
					return "application/vnd.visio";
				case ".vsscc":
					return "text/plain";
				case ".vssettings":
					return "text/xml";
				case ".vssscc":
					return "text/plain";
				case ".vst":
					return "application/vnd.visio";
				case ".vstemplate":
					return "text/xml";
				case ".vsto":
					return "application/x-ms-vsto";
				case ".vsw":
					return "application/vnd.visio";
				case ".vsx":
					return "application/vnd.visio";
				case ".vtx":
					return "application/vnd.visio";
				case ".wav":
					return "audio/wav";
				case ".wave":
					return "audio/wav";
				case ".wax":
					return "audio/x-ms-wax";
				case ".wbk":
					return "application/msword";
				case ".wbmp":
					return "image/vnd.wap.wbmp";
				case ".wcm":
					return "application/vnd.ms-works";
				case ".wdb":
					return "application/vnd.ms-works";
				case ".wdp":
					return "image/vnd.ms-photo";
				case ".webarchive":
					return "application/x-safari-webarchive";
				case ".webtest":
					return "application/xml";
				case ".wiq":
					return "application/xml";
				case ".wiz":
					return "application/msword";
				case ".wks":
					return "application/vnd.ms-works";
				case ".WLMP":
					return "application/wlmoviemaker";
				case ".wlpginstall":
					return "application/x-wlpg-detect";
				case ".wlpginstall3":
					return "application/x-wlpg3-detect";
				case ".wm":
					return "video/x-ms-wm";
				case ".wma":
					return "audio/x-ms-wma";
				case ".wmd":
					return "application/x-ms-wmd";
				case ".WMD":
					return "application/x-ms-wmd";
				case ".wmf":
					return "application/x-msmetafile";
				case ".wml":
					return "text/vnd.wap.wml";
				case ".wmlc":
					return "application/vnd.wap.wmlc";
				case ".wmls":
					return "text/vnd.wap.wmlscript";
				case ".wmlsc":
					return "application/vnd.wap.wmlscriptc";
				case ".wmp":
					return "video/x-ms-wmp";
				case ".wmv":
					return "video/x-ms-wmv";
				case ".wmx":
					return "video/x-ms-wmx";
				case ".wmz":
					return "application/x-ms-wmz";
				case ".wpl":
					return "application/vnd.ms-wpl";
				case ".wps":
					return "application/vnd.ms-works";
				case ".wri":
					return "application/x-mswrite";
				case ".wrl":
					return "x-world/x-vrml";
				case ".wrz":
					return "x-world/x-vrml";
				case ".wsc":
					return "text/scriptlet";
				case ".wsdl":
					return "text/xml";
				case ".wvx":
					return "video/x-ms-wvx";
				case ".x":
					return "application/directx";
				case ".xaf":
					return "x-world/x-vrml";
				case ".xaml":
					return "application/xaml+xml";
				case ".xap":
					return "application/x-silverlight-app";
				case ".xbap":
					return "application/x-ms-xbap";
				case ".xbm":
					return "image/x-xbitmap";
				case ".xdr":
					return "text/plain";
				case ".xht":
					return "application/xhtml+xml";
				case ".xhtml":
					return "application/xhtml+xml";
				case ".xla":
					return "application/vnd.ms-excel";
				case ".xlam":
					return "application/vnd.ms-excel.addin.macroEnabled.12";
				case ".xlc":
					return "application/vnd.ms-excel";
				case ".xld":
					return "application/vnd.ms-excel";
				case ".xlk":
					return "application/vnd.ms-excel";
				case ".xll":
					return "application/vnd.ms-excel";
				case ".xlm":
					return "application/vnd.ms-excel";
				case ".xls":
					return "application/vnd.ms-excel";
				case ".xlsb":
					return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
				case ".xlsm":
					return "application/vnd.ms-excel.sheet.macroEnabled.12";
				case ".xlsx":
					return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				case ".xlt":
					return "application/vnd.ms-excel";
				case ".xltm":
					return "application/vnd.ms-excel.template.macroEnabled.12";
				case ".xltx":
					return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
				case ".xlw":
					return "application/vnd.ms-excel";
				case ".xml":
					return "text/xml";
				case ".xmta":
					return "application/xml";
				case ".xof":
					return "x-world/x-vrml";
				case ".XOML":
					return "text/plain";
				case ".xpm":
					return "image/x-xpixmap";
				case ".xps":
					return "application/vnd.ms-xpsdocument";
				case ".xrm-ms":
					return "text/xml";
				case ".xsc":
					return "application/xml";
				case ".xsd":
					return "text/xml";
				case ".xsf":
					return "text/xml";
				case ".xsl":
					return "text/xml";
				case ".xslt":
					return "text/xml";
				case ".xsn":
					return "application/octet-stream";
				case ".xss":
					return "application/xml";
				case ".xtp":
					return "application/octet-stream";
				case ".xwd":
					return "image/x-xwindowdump";
				case ".z":
					return "application/x-compress";
				case ".zip":
					return "application/x-zip-compressed";

				#endregion

				default:
					// have a look in the registry on this server
					RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false);
					object value = key != null ? key.GetValue("Content Type", null) : null;
					string result = value != null ? value.ToString() : "application/octet-stream";

					return result;
			}
		}

		public static string GetExtensionFromMimeType(string mimeType) {
			if (mimeType == null) {
				throw new ArgumentNullException("mimeType");
			}
			var key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
			var value = key != null ? key.GetValue("Extension", null) : null;
			var result = value != null ? value.ToString() : string.Empty;
			return result;
		}

		/// <summary>
		/// Creates a file and returns the FileStream, used for saving a binary file.
		/// YOU MUST call .Dispose() on the returned filestream or use a "using" block.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static FileStream CreateFileStream(string filePath) {
			// convert to physical path if not already
			filePath = Web.MapPath(filePath);
			// create directories if not already
			CreateFolder(GetParentPath(filePath));
			// create file
			var file = new FileStream(filePath, FileMode.Create);
			return file;
		}

		public static double CalculateFolderSize(string folder) {
			double folderSize = 0.0;
			try {         //Checks if the path is valid or not         
				if (!Directory.Exists(folder)) {
					return folderSize;
				} else {
					try {
						foreach (string file in Directory.GetFiles(folder)) {
							if (File.Exists(file)) {
								FileInfo finfo = new FileInfo(file);
								folderSize += finfo.Length;
							}
						} foreach (string dir in Directory.GetDirectories(folder))
							folderSize += CalculateFolderSize(dir);
					} catch (NotSupportedException e) {
						throw new Beweb.ProgrammingErrorException("Unable to calculate folder size: " + folder + "\n" + e.Message);
					}
				}
			} catch (UnauthorizedAccessException e) {
				throw new Beweb.ProgrammingErrorException("Unable to calculate folder size: " + folder + "\n" + e.Message);
			}
			return folderSize;
		}

		public static void DeleteFolder(string path) {
			path = Web.MapPath(path);
			if (FolderExists(path)) {
				const bool recursive = true;
				System.IO.Directory.Delete(path, recursive);
			}
		}

		/// <summary>
		/// Copy a single file to another directory. Can be physical or virtual paths.
		/// Overwrites if file already exists.
		/// eg CopyFiles("c:\somepics\bike.jpg", Web.Attachments + "jpegs")
		/// </summary>
		public static void CopyFile(string sourceFilePath, string destDirectoryPath) {
			sourceFilePath = Web.MapPath(sourceFilePath);
			destDirectoryPath = Web.MapPath(destDirectoryPath);
			CreateFolder(destDirectoryPath);
			File.Copy(sourceFilePath, sourceFilePath.Replace(sourceFilePath, destDirectoryPath), true);
		}

		/// <summary>
		/// Copy a single file to another directory. Can be physical or virtual paths.
		/// Renames to a unique filename if file already exists.
		/// eg CopyFiles("c:\somepics\bike.jpg", Web.Attachments + "jpegs")
		/// </summary>
		public static void CopyFileUnique(string sourceFilePath, string destDirectoryPath) {
			sourceFilePath = Web.MapPath(sourceFilePath);
			destDirectoryPath = Web.MapPath(destDirectoryPath);
			CreateFolder(destDirectoryPath);
			string destFileName = GetFileNameWithoutPath(destDirectoryPath);
			string destDirectory = GetDirectoryName(destDirectoryPath);
			if (destFileName==null || IsFolder(destDirectoryPath)) {
				destFileName = GetFileNameWithoutPath(sourceFilePath);
			}
			destFileName = GetUniqueFilename(destDirectory, destFileName);
			File.Copy(sourceFilePath, sourceFilePath.Replace(sourceFilePath, Path.Combine(destDirectory, destFileName)), true);
		}

		/// <summary>
		/// Copy several files in a directory to another directory. Can be physical or virtual paths.
		/// eg CopyFiles("c:\somepics", Web.Attachments + "jpegs", "*.jpg")
		/// </summary>
		public static void CopyFiles(string sourceDirectoryPath, string destDirectoryPath, string searchPattern) {
			CopyFiles(sourceDirectoryPath, destDirectoryPath, searchPattern, false);
		}

		/// <summary>
		/// Copy several files in a directory to another directory. Can be physical or virtual paths.
		/// eg CopyFiles("c:\somepics", Web.Attachments + "jpegs", "*.jpg")
		/// </summary>
		public static void CopyFiles(string sourceDirectoryPath, string destDirectoryPath, string searchPattern, bool printOnScreen) {
			if (searchPattern == null) searchPattern = "*.*";
			sourceDirectoryPath = Web.MapPath(sourceDirectoryPath);
			destDirectoryPath = Web.MapPath(destDirectoryPath);
			foreach (var newPath in Directory.GetFiles(sourceDirectoryPath, searchPattern)) {
				var newFileName = newPath.Replace(sourceDirectoryPath, destDirectoryPath);
				File.Copy(newPath, newFileName, true);
				if (printOnScreen) Web.Write(newFileName + "<br>");
			}
		}


		/// <summary>
		/// needs testing
		/// </summary>
		public static bool IsFolder(string path) {
			if (path.EndsWith("\\") || path.EndsWith("/")) {
				return true;
			}
			if (Path.HasExtension(path)) {
				return false;
			}
			if (FolderExists(path)) {
				return true;
			}
			if (FileExists(path)) {
				return false;
			}
			// could be a file with no extension or a folder
			// eg c:\temp\mike      -- is this a file or a folder?
			return true;  // assume folder
		}

		public static string GetDirectoryName(string path) {
			return Path.GetDirectoryName(path);
		}

		public static string GetFileNameWithoutPath(string path) {
			return Path.GetFileName(path);
		}

		public static bool HasExtension(string path) {
			return Path.HasExtension(path);
		}


	}

}

namespace BewebTest {
	[TestClass]
	public class FileSystemTests {
		[TestMethod]
		public void TestDangerous() {
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.aspx"));
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.asp"));
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.ASPx"));
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.js"));
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.vbs"));
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.pif"));
			Assert.AreEqual(true, FileSystem.IsDangerous("mike.exe"));
			Assert.AreEqual(false, FileSystem.IsDangerous("mike.doc"));
			Assert.AreEqual(false, FileSystem.IsDangerous("mike.xls"));
		}

		[TestMethod]
		public void TestCrunchFileName() {
			Assert.AreEqual("qwerty-uiop-jpe-g", FileSystem.CrunchFileName("QWE!@#$%^*rty -___ -UI(op).jpE g + ---"));
		}

		[TestMethod]
		public void TestCleanFileName() {
			Assert.AreEqual("qwerty-uiop.jpeg", FileSystem.CleanFileName("QWE!@#$%^*rty -___ -UI(op).jpE g + "));
		}

		[TestMethod]
		public void TestGetUniqueFilename() {
			string actual = FileSystem.GetUniqueFilename(Web.Attachments + "test/", "a this is a really long image name with punctuation !@& stuff like that in it as well as bing very long.JPG.min.jpeg ");
			Assert.AreEqual(50, actual.Length);
			Assert.AreEqual("a-this-is-a-really-long-image-name-with-punct.jpeg", actual);
			actual = FileSystem.GetUniqueFilename(Web.Attachments + "test/", "a this is a really long image name with punc tuation !@& stuff like that in it as well as bing very long.JPG.min.jpeg ");
			Assert.AreEqual(49, actual.Length);
			Assert.AreEqual("a-this-is-a-really-long-image-name-with-punc.jpeg", actual);
			actual = FileSystem.GetUniqueFilename(Web.Attachments + "test/", "a this is a really long image name with punc tuation !@& stuff like that in it as well as bing very long.JPG.min.jpeg ", 30);
			Assert.AreEqual(30, actual.Length);
			actual = FileSystem.GetUniqueFilename(Web.Attachments + "test/", "a this is a really long image name with punc tuation !@& stuff like that in it as well as bing very long.JPG.min.jpeg ", 10);
			Assert.AreEqual(10, actual.Length);
		}


	}
}