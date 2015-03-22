using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Beweb;
using System.Xml;

public class Manifest {
	public string Name;
	public string Description;
	public string FileName;
	public List<string> Defines = new List<string>();
	public string Action;
	public List<string> Tables = new List<string>();
	public List<string> Requires = new List<string>();
	public string Maintainer;
	public string SourceProject;
	public string ErrorMsg;
	public bool IsEnabled = true;

	public Manifest(string fileName) {
		var fileNameTxt = fileName.Substring(fileName.LastIndexOf('\\') + 1);
		fileNameTxt = fileNameTxt.Substring(0, fileNameTxt.LastIndexOf("manifest.html") - 1);
		string manifestContents = FileSystem.GetFileContents(fileName, false);
		foreach (var line in manifestContents.Split('\n')) {
			if (line.StartsWith("#")) {
				if (line.StartsWith("#description ")) {
					Description = line.RemovePrefix("#description");
				} else if (line.StartsWith("#define ")) {
					string defines = line.RemovePrefix("#define") + "";
					if (defines.IsNotBlank()) {
						Defines.AddRange(defines.Trim().Replace(" ", "").Replace(",", ";").Split(';'));
					}
				} else if (line.StartsWith("#maintainer ")) {
					Maintainer = line.RemovePrefix("#maintainer ") + ""; ;

				} else if (line.StartsWith("#source ")) {
					SourceProject = line.RemovePrefix("#source ") + ""; ;

				} else if (line.StartsWith("#disabled")) {
					IsEnabled = false;
				} else if (line.StartsWith("#requires ")) {
					string requires = line.RemovePrefix("#requires") + "";
					if (requires.IsNotBlank()) {
						Requires.AddRange(requires.Trim().Replace(" ", "").Replace(",", ";").Split(';'));
					}
				}
			} else if (line.StartsWith("Models\\Generated\\")) {
				string tableName = line.Trim().RemovePrefix("Models\\Generated\\").RemoveSuffix("Generated.cs");
				if (tableName != "ActiveRecordTests.cs") {
					Tables.Add(tableName);
				}
			}

			FileName = fileName;
			Name = fileNameTxt;

			// see what action was submitted
			Action = Web.Request["Manifest_" + Name] ?? "Leave";
		}
		if (Maintainer.IsBlank()) ErrorMsg += "No maintainer specified";

	}
}

public partial class tests_create : System.Web.UI.Page {
	public string pwd;
	public List<Manifest> availableManifests;
	public string basePath = null;

	protected IEnumerable<string> ClientFolders {
		get {
			List<string> clients = new List<string>();
			var folders = System.IO.Directory.GetDirectories(DefPath);
			foreach (string folder in folders) {
				if (!FileSystem.FileExists(folder + "\\site.master")) { // && Directory.GetDirectories(folder).Length > 1
					clients.Add(folder.RightFrom("\\"));
				}
			}
			return clients;
		}
	}

	protected IEnumerable<string> ThemeFolders {
		get {
			List<string> themes = new List<string>();
			var folders = System.IO.Directory.GetDirectories(Web.MapPath("~/theme"));
			foreach (string folder in folders) {
				if (folder.Contains(".svn")) continue;
				themes.Add(folder.RightFrom("\\"));
			}
			return themes;
		}
	}

	protected string ThemeName = Web.Request["theme"].DefaultValue("blanktheme");

	protected string DefPath;

	protected void Page_Load(object sender, EventArgs e) {
		Response.Buffer = true;
		DefPath = Server.MapPath("~").RemoveSuffix("codelibmvc");
		SetupManifestFiles();
	}

	protected void WriteResults() {
		basePath = Request["path"];
		if (basePath != null && !basePath.EndsWith("\\")) basePath += "\\";
		//HandleAllCSFiles();
		CreateProj();
		ModifyCSFiles();
		//if (Request["CreateDB"]=="1") {
		//  Beweb.BewebData.ExecuteSQL(GetDBCreateScript("thatcher"));
		//}
		Response.Write("<br>");
		if (Request["create"] == null) {
			Response.Write("There are more options after you hit 'go'.<br>");
		} else {
			Response.Write("1. If you have no database, click here for a <a href=\"\" onclick=\"$('#dbscript').show(); return false;\">database create script</a> - go into SQL Server and run it.<br>");
			Response.Write("<div id='dbscript' style='display:none'><pre>" + GetDBCreateScript("") + "</pre></div>");
			Response.Write("2. Next open the new solution file in Visual Studio and Build it<br>");
			Response.Write("3. Next run \"<a href=\"http://localhost/" + Request["projname"] + "/tests/default.aspx?mode=AddDbFields\">Create tables</a>\" in the new project script.<br>");
			Response.Write("4. Then \"<a href=\"http://localhost/" + Request["projname"] + "/\">Open new home</a>\".<br>");
			Response.Write("<br>");
			//Response.Write("You can also \"<a href=\"http://localhost/"+Request["projname"]+"/tests/default.aspx\">Open tests</a>\".<br>");
			//Response.Write("You can also \"<a href=\"http://localhost/"+Request["projname"]+"/admin/\">Open new admin system</a>\".<br>");
			//Response.Write("You can also \"<a href=\"http://localhost/"+Request["projname"]+"/\">Open new home</a>\".<br>");
			Response.Write("You can also compare \"<a href=\"" + Web.Root + "tests/listfiles.aspx\">listfiles.aspx</a>\" in this project to \"<a href=\"http://localhost/" + Request["projname"] + "/tests/listfiles.aspx\">listfiles.aspx</a>\" in the new project .<br>");

		}
	}

	private string GetDBCreateScript(string serverName) {
		string result = @"
CREATE DATABASE [thatcher].[NewProj] 
GO
USE [thatcher].[NewProj]
GO
IF NOT EXISTS (SELECT name FROM [thatcher].sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [thatcher].[NewProj] MODIFY FILEGROUP [PRIMARY] DEFAULT
GO


--user create
USE [thatcher].[master]
GO
CREATE LOGIN [NewProj_user] WITH PASSWORD=N'**PWD**', DEFAULT_DATABASE=[NewProj], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [thatcher].[NewProj]
GO
CREATE USER [NewProj_user] FOR LOGIN [NewProj_user]
GO
EXEC sp_addrolemember N'db_datareader', N'NewProj_user'
GO
EXEC sp_addrolemember N'db_datawriter', N'NewProj_user'
GO
EXEC sp_addrolemember N'db_owner', N'NewProj_user'
GO
		";

		result = result.Replace("NewProj", Request["projname"]);
		result = result.Replace("**PWD**", pwd);
		if (serverName + "" != "") {
			result = result.Replace("[thatcher]", "[" + serverName + "]");
		} else {
			// no server, remove references to thatcher
			result = result.Replace("[thatcher].", "");
		}
		//FileSystem.CreateFolder("~/_beweb");
		FileSystem.WriteTextFile("~/_beweb/create_db_script_"+Fmt.DateTimeCompressed(DateTime.Now)+".sql", result);
		return result;
	}

	public void Log(string msg) {
		if (Request["showlog"] != null) Web.Write(msg);
	}

	//protected void ParseRootCSFile()
	//{
	//  string newName = "site.csproj";
	//  ParseCSproj(basePath, newName);
	//}

	/// <summary>
	/// step 1: add any missing things to the target proj file that exist in the file system (this handles case where new files are added to an existing project)
	/// step 2: search the target project, looking for csproj files. For each of these found, remove any xml nodes in there that dont exist on the file system
	/// </summary>
	protected void ModifyCSFiles() {
		if (basePath != null) {
			SearchForCSProjFiles(basePath, 0);
			AddAnyMissingTargetFilesToTheCSProjFiles(basePath);
			ParseAllCSProjFilesRemovingDeadNodes();
		}
	}

	protected List<string> targetASPXandCSFiles = new List<string>();

	private void AddAnyMissingTargetFilesToTheCSProjFiles(string basePath) {
		//get all the cs and aspx files in the system (not source, but the target system) into a list (member var)
		FindAllASPXandCSFiles(basePath, 0);
		//foreach csproj file
		foreach (var projFile in CSProjFiles) //walk the cs proj files looking for site.csproj
		{
			if (projFile.DoesntContain("Site.csproj")) continue;																		 //step to next file if not site.csproj
			var projName = projFile.Substring(projFile.LastIndexOf("\\") + 1);
			int filesProcessed = 0;
			XmlDocument doc = new XmlDocument();
			bool docModified = false;
			doc.Load(projFile);  //load the proj file

			//walk each file, check in csproj to make sure the file exists
			var itemsToAddToProjDom = new List<string>();
			//foreach (var file in targetASPXandCSFiles) {
			XmlNode nodeToAddTo = null;
			for (int scanFiles = 0; scanFiles < targetASPXandCSFiles.Count; scanFiles++) {
				bool itemFound = false;
				var fileToSearchDomFor = targetASPXandCSFiles[scanFiles];
				//if path in another csproj, step to next file
				string vpath = null;
				bool skipThisFileAsInAnotherCSProj = false;
				foreach (var projFileCheck in CSProjFiles) {
					//if(projFile==@"C:\data\dev\web\newproj\Site.csproj" &&fileToSearchDomFor==@"BewebCore\Beweb\ActiveField.cs")
					//{
					//  int i=1;
					//}
					var pathProj = projFile.Substring(0, projFile.LastIndexOf("\\") + 1);
					var path = projFileCheck.Substring(0, projFileCheck.LastIndexOf("\\") + 1);
					vpath = path.Replace(pathProj, "");//remove abs path
					if (projFileCheck != projFile && vpath.IsNotBlank() && fileToSearchDomFor.StartsWith(vpath)) {
						skipThisFileAsInAnotherCSProj = true;
						break;		//gt out of csproj list
					}
				}

				if (skipThisFileAsInAnotherCSProj) {
					Log("skip file[" + fileToSearchDomFor + "] as its in proj name [" + vpath + "]<br />");
					continue;	 //get out of file list	 as we dont need to find this file
				}

				//walk the xml dom checking if each file in the list exists, if not, add it.

				//Log("<br><br>load file[" + basePath + newName + "]<br />");
				var root = doc.ChildNodes[1];
				for (int scan = 0; scan < root.ChildNodes.Count; scan++) {
					var nodeName = root.ChildNodes[scan].Name;
					//Log("1[" + nodeName + "]<br />");
					if (nodeName == "ItemGroup") {
						if (nodeToAddTo == null) nodeToAddTo = root.ChildNodes[scan];
						if (FindItemInContentOrCompile(basePath, root.ChildNodes[scan], fileToSearchDomFor)) {
							itemFound = true;
							//targetASPXandCSFiles[scanFiles] += "**FOUND"; //found - dont re-check this
							break;
						}
					}
				}
				if (!itemFound) {
					Log("*********not found[" + projFile + "][" + fileToSearchDomFor + "]<br />");

					itemsToAddToProjDom.Add(fileToSearchDomFor);
				}

				filesProcessed++;//if(filesProcessed >20)break;		//debug only
			}

			XmlNode newNode = null;
			foreach (var item in itemsToAddToProjDom) {
				string nodeType = null;
				if (item.Contains("--added--") || item.Contains(" ")) continue;  //skip spaces
				if (item.EndsWith("aspx.cs")) {
					nodeType = "Content";
				} else if (item.EndsWith(".cs")) {
					nodeType = "Compile";
				} else if (item.EndsWith(".aspx")) {
					nodeType = "Content";
				} else if (item.EndsWith(".js")) {
					nodeType = "Content";
				} else if (item.EndsWith(".css")) {
					nodeType = "Content";
				} else {
					Log("--- dont know what to add[" + item + "]<br />");

				}
				if (nodeType != null) {
					newNode = doc.CreateNode(XmlNodeType.Element, nodeType, null);
					//XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
					//namespaces.Add(string.Empty, string.Empty);

					XmlAttribute xa = doc.CreateAttribute("Include");
					xa.Value = item;
					newNode.Attributes.Append(xa);
					nodeToAddTo.AppendChild(newNode);
					Log("append this to dom[" + item + "]<br />");
					docModified = true;
				}
			}

			if (docModified) {
				doc.Save(basePath + "\\" + projName);

				RemoveEmptyNamespaceAttribs(basePath + "\\" + projName);
				//XmlTextWriter writer = new XmlTextWriter(basePath +"\\"+ projName, Encoding.UTF8);
				//writer.Namespaces = false;
				//writer.WriteStartDocument();
				//writer.WriteRaw(doc.InnerXml);
				//writer.WriteEndDocument();

				Log("updated proj[" + basePath + projName + "] with missing items<br />");
			}
			string subpath = Request["path"].RemovePrefix(DefPath);
			ReplaceSampleSitenameInConfig(basePath + "\\Web_AppSettings.config", Request["projname"], subpath, ThemeName);
			ReplaceSampleSitenameInConnStrings(basePath + "\\Web_ConnectionStrings.config", Request["projname"]);

		}

	}

	private void ReplaceSampleSitenameInConfig(string filename, string sitename, string subpath, string theme) {
		if (File.Exists(filename)) {
			string origText = FileSystem.GetFileContents(filename, false);
			string appsettingsFile = origText;
			//Web.Write(appsettingsFile.FmtPlainTextAsHtml());
			//sitename.beweb.co.nz
			//sitename.co.nz
			//<add key="SiteName" value="sitename"/>
			//website@sitename.co.nz
			//staging.beweb.co.nz
			//www.sitename.co.nz
			appsettingsFile = appsettingsFile.Replace("clientname/sitename", subpath);
			appsettingsFile = appsettingsFile.Replace("sitename", sitename.ToLower());  // this should correctly do the same as the below (given Replace is case sensitive)
			//appsettingsFile = appsettingsFile.Replace("<add key=\"SiteName\" value=\"sitename\"/>", "<add key=\"SiteName\" value=\"" + sitename.SplitTitleCase() + "\"/>");
			//appsettingsFile = appsettingsFile.Replace("sitename.beweb.co.nz", "" + sitename.ToLower() + ".beweb.co.nz");
			//appsettingsFile = appsettingsFile.Replace("sitename.co.nz", "" + sitename.ToLower() + ".co.nz");
			//appsettingsFile = appsettingsFile.Replace("website@sitename.co.nz", "website@" + sitename.ToLower() + ".co.nz");
			//appsettingsFile = appsettingsFile.Replace("sitename.beweb.co.nz", "" + sitename.ToLower() + ".beweb.co.nz");
			//appsettingsFile = appsettingsFile.Replace("www.sitename.co.nz", "www." + sitename.ToLower() + ".co.nz");
			appsettingsFile = appsettingsFile.Replace(@"key=""Theme"" value=""focus""", @"key=""Theme"" value=""blanktheme""");
			appsettingsFile = appsettingsFile.Replace("blanktheme", theme);
			appsettingsFile = appsettingsFile.Replace("--CryptKey--", RandomPassword.Generate(48, 48,
				RandomPassword.PASSWORD_CHARS_LCASE,
				RandomPassword.PASSWORD_CHARS_UCASE,
				RandomPassword.PASSWORD_CHARS_NUMERIC,
				RandomPassword.PASSWORD_CHARS_SPECIAL
				)); //jdsjnkjksdioa89493920sdjkasn-1239mflmkkckd$%^*(
			appsettingsFile = appsettingsFile.Replace("BuildStartDate\" value=\"tbd", "BuildStartDate\" value=\"" + Fmt.DateTimeCompressed(DateTime.Now));
			if (appsettingsFile != origText) {
				File.WriteAllText(filename, appsettingsFile);
				Log("updated appsetting[" + filename + "] <br />");
			} else {
				Log("didnt update the appsettings [" + filename + "]<br />");

			}
		}
	}
	private void ReplaceSampleSitenameInConnStrings(string filename, string sitename) {
		if (File.Exists(filename)) {
			string origText = FileSystem.GetFileContents(filename);
			string appsettingsFile = origText;
			//Initial Catalog=codelibmvc;
			// note: this generated password is not used if the password has already been replaced out
			pwd = RandomPassword.Generate(12, 14);
			// dev
			appsettingsFile = appsettingsFile.Replace("Data Source=[datasource];Initial Catalog=codelibmvc;User Id=sa;Password=[password]", "Data Source=thatcher;Initial Catalog=sitename;User Id=sitename;Password=" + pwd);
			// stg and lve
			appsettingsFile = appsettingsFile.Replace("Initial Catalog=sitename;User Id=sitename;", "Initial Catalog=" + sitename + ";User Id=" + sitename + "_user;");
			appsettingsFile = appsettingsFile.Replace(";Password=pw", ";Password=" + pwd);

			if (appsettingsFile != origText) {
				File.WriteAllText(filename, appsettingsFile);
				Log("updated conn strings setting[" + filename + "], <span style=\"color:red;\">you should create a user called " + sitename + "user, with a Password=" + pwd + "</span><br />");
			} else {
				Log("didnt update the conn strings[" + filename + "]<br />");

			}
		}
	}

	private void RemoveEmptyNamespaceAttribs(string filename) {
		string projFileContents = FileSystem.ReadTextFile(filename);
		projFileContents = projFileContents.Replace("xmlns=\"\"", "");
		File.WriteAllText(filename, projFileContents);
	}

	private bool FindItemInContentOrCompile(string basePath, XmlNode xmlNode, string searchingForFilename) {
		for (int scan = 0; scan < xmlNode.ChildNodes.Count; scan++) {
			var nodeName = xmlNode.ChildNodes[scan].Name;
			//Log("2[" + nodeName + "]<br />");
			if (nodeName == "Compile" || nodeName == "Content") {
				var childNode = xmlNode.ChildNodes[scan];
				var filename = childNode.Attributes["Include"].Value.ToString();
				//Log("Parsing[" + xmlNode.Name + "] attr[" + filename + "]<br />");

				if (filename == searchingForFilename) {
					//Log("-----found[" + xmlNode.Name + "] attr[" + filename + "]<br />");
					return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// search for aspx and cs files in the ter
	/// </summary>
	/// <param name="sourceDir">phys path to start at</param>
	/// <param name="recursionLvl">start at 0, recurseive call</param>
	public void FindAllASPXandCSFiles(string sourceDir, int recursionLvl) {
		if (recursionLvl <= HowDeepToScan) {
			// Process the list of files found in the directory. 
			string[] fileEntries = Directory.GetFiles(sourceDir);
			foreach (string fileName in fileEntries) {
				//remove path
				var fileNameNoPath = fileName.Replace(basePath, "");
				if ((fileNameNoPath.ToLower().EndsWith(".aspx")) || (fileNameNoPath.ToLower().EndsWith(".cs"))) {
					targetASPXandCSFiles.Add(fileNameNoPath.Substring(1)); //add to list - remove \\ at start
				}
			}

			// Recurse into subdirectories of this directory.
			string[] subdirEntries = Directory.GetDirectories(sourceDir);
			foreach (string subdir in subdirEntries) {
				// Do not iterate through reparse points
				if ((File.GetAttributes(subdir) &
						 FileAttributes.ReparsePoint) !=
						FileAttributes.ReparsePoint) {
					if (!subdir.Contains("_backup") && !subdir.Contains("template") && !subdir.Contains(".exclude")) {
						FindAllASPXandCSFiles(subdir, recursionLvl + 1);
					}
				}
			}
		}
	}


	protected void SetupManifestFiles() {
		//basePath = Web.Request["path"];
		string[] fileEntries = Directory.GetFiles(Server.MapPath("./CreateProject"), "*.manifest.*");
		availableManifests = new List<Manifest>();
		foreach (string fileName in fileEntries) {
			//Log("f[" + fileName + "]<br>");
			availableManifests.Add(new Manifest(fileName));
		}
	}

	private bool docModified = false;

	protected void ParseCSproj(string basePath, string newName) {
		//Log(Logging.DumpForm());	
		List<string> definesToAdd = new List<string>();
		List<string> definesToRemove = new List<string>();
		foreach (var manifest in availableManifests) {
			if (manifest.Action == "Create" && manifest.Defines != null) {
				foreach (var define in manifest.Defines) {
					if (!definesToAdd.Contains(define)) {
						definesToAdd.Add(define);
					}
				}
			} else if (manifest.Action == "Remove" && manifest.Defines != null) {
				foreach (var define in manifest.Defines) {
					if (!definesToRemove.Contains(define)) {
						definesToRemove.Add(define);
					}
				}
			}
		}
		//Log("<br><br>definesToAdd[" + definesToAdd.Join(";") + "] definesToRemove[" + definesToRemove.Join(";") + "]<br />");

		XmlDocument doc = new XmlDocument();
		docModified = false;
		doc.Load(basePath + newName);
		//Log("<br><br>load file[" + basePath + newName + "]<br />");
		var root = doc.ChildNodes[1];

		for (int scan = 0; scan < root.ChildNodes.Count; scan++) {
			var nodeName = root.ChildNodes[scan].Name;
			//Log("1[" + nodeName + "]<br />");
			if (nodeName == "ItemGroup") {
				ProcessItemGroup(basePath, root.ChildNodes[scan]);
			} else if (nodeName == "PropertyGroup") {
				ProcessPropertyGroup(basePath, root.ChildNodes[scan], definesToAdd, definesToRemove);
			}
		}
		if (docModified) {
			doc.Save(basePath + newName);
			Log("updated proj[" + newName + "]<br />");
		}
	}

	private void ProcessPropertyGroup(string basePath, XmlNode xmlNode, List<string> definesToAdd, List<string> definesToRemove) {
		if (definesToAdd.Count > 0 || definesToRemove.Count > 0) {
			for (int scan = 0; scan < xmlNode.ChildNodes.Count; scan++) {
				XmlNode childNode = xmlNode.ChildNodes[scan];
				var nodeName = childNode.Name;
				//Log("2[" + nodeName + "]<br />");
				if (nodeName == "DefineConstants") {
					List<string> defines = new List<string>(childNode.InnerText.Split(';'));
					foreach (string def in definesToRemove) {
						defines.Remove(def);
					}
					foreach (string def in definesToAdd) {
						if (!defines.Contains(def)) {
							defines.Add(def);
						}
					}
					string newDefines = defines.Join(";");
					if (childNode.InnerText != newDefines) {
						childNode.InnerText = newDefines;
						docModified = true;
					}
				}
			}
		}
	}

	private void ProcessItemGroup(string basePath, XmlNode xmlNode) {
		var removeList = new List<XmlNode>();
		for (int scan = 0; scan < xmlNode.ChildNodes.Count; scan++) {
			var nodeName = xmlNode.ChildNodes[scan].Name;
			//Log("2[" + nodeName + "]<br />");
			if (nodeName == "Compile" || nodeName == "Content") {
				var childNode = xmlNode.ChildNodes[scan];
				if (!ProcessCompile(basePath, childNode)) {
					Log("Removed[" + xmlNode.Name + "] attr[" + childNode.Attributes["Include"].Value.ToString() + "]<br />");

					//xmlNode.RemoveChild(childNode); //dont remove now, as it will stuff up the count
					//docModified = true;
					removeList.Add(childNode);

				}
			}
			//if (nodeName == "Content") ProcessContent(xmlNode.ChildNodes[scan]);
		}

		// remove the things mark to remove
		foreach (var rmItem in removeList) {
			xmlNode.RemoveChild(rmItem);
			docModified = true;
		}
	}

	private bool ProcessCompile(string basePath, XmlNode xmlNode) {
		//Log("Compile?[" + xmlNode.Name + "] attr[" + xmlNode.Attributes["Include"].Value.ToString() + "]<br />");
		var checkPath = basePath + xmlNode.Attributes["Include"].Value;
		//Log("checkPath[" + checkPath + "]<br />");
		if (!File.Exists(checkPath)) {
			Log("Missing[" + xmlNode.Name + "] attr[" + xmlNode.Attributes["Include"].Value.ToString() + "]<br />");
			return false;
		}
		return true;
	}

	protected void CreateProj() {
		//Log(Logging.DumpForm());

		if (Request["create"] != null) {
			Log("<b>Create Project</b><br /><br />");

			basePath = Request["path"];

			if (basePath.IsNotBlank()) FileSystem.CreateFolder(basePath);
			for (int scan = 0; scan < HttpContext.Current.Request.Form.Count; scan++) {
				string formItem = HttpContext.Current.Request.Form[scan];
				string formItemName = HttpContext.Current.Request.Form.AllKeys[scan];

				//if(formItem=="")break;

				Log("[" + scan + "]: [" + formItemName + "] = [" + formItem + "]<br>");
				var searchText = "Manifest_";
				if (formItemName.StartsWith(searchText)) {
					var manifestName = formItemName.Substring(searchText.Length);
					//Log("mname["+manifestName+"]");
					//Log("formItem["+formItem+"]");

					if (formItem == "Create") {
						ProcessFile(basePath, "CreateProject\\" + manifestName + ".manifest.html");
					} else if (formItem == "Remove") {
						RemoveFilesFromManifest(basePath, "CreateProject\\" + manifestName + ".manifest.html");
					} else {
						Log("formItem[" + formItem + "] - do nothing<br>");
					}

				}
			}

			CopyThemeFolder(basePath);

			//CopyFile(basePath,"iepngfix.htc");

			FixSLNFile(basePath);

			//CreateSubversionProj();

			Log("<br>Done.");
		}
	}

	private void CreateSubversionProj() {
		//		return;

		/*
		 * from jeremy:
		string _systemRoot = Environment.GetEnvironmentVariable("SYSTEMROOT");

		ProcessStartInfo _processStartInfo = new ProcessStartInfo();
		_processStartInfo.WorkingDirectory = @"%ProgramFiles%";
		_processStartInfo.FileName = @"Notepad.exe";
		_processStartInfo.Arguments = "test.txt";
		_processStartInfo.CreateNoWindow = true;
		Process myProcess = Process.Start(_processStartInfo);
		*/


		// Get the full file path
		string strFilePath = Server.MapPath("fine.bat");

		// Create the ProcessInfo object
		System.Diagnostics.ProcessStartInfo psi =
						new System.Diagnostics.ProcessStartInfo("cmd.exe");
		psi.UseShellExecute = false;
		psi.RedirectStandardOutput = true;
		psi.RedirectStandardInput = true;
		psi.RedirectStandardError = true;

		// Start the process
		System.Diagnostics.Process proc =
							 System.Diagnostics.Process.Start(psi);

		// Open the batch file for reading
		//System.IO.StreamReader strm = 
		//           System.IO.File.OpenText(strFilePath);
		System.IO.StreamReader strm = proc.StandardError;
		// Attach the output for reading
		System.IO.StreamReader sOut = proc.StandardOutput;

		// Attach the in for writing
		//    System.IO.StreamWriter sIn = proc.StandardInput;

		// Write each line of the batch file to standard input
		/*while(strm.Peek() != -1)
		{
				sIn.WriteLine(strm.ReadLine());
		}*/
		// sIn.WriteLine(exec);
		string errors = strm.ReadToEnd().Trim();
		strm.Close();

		Web.WriteLine("Errors: " + errors);

		// Exit CMD.EXE
		//  string stEchoFmt = "# {0} run successfully. Exiting";

		//  sIn.WriteLine(String.Format(stEchoFmt, strFilePath));
		//  sIn.WriteLine("EXIT");

		// Close the process
		proc.Close();

		// Read the sOut to a string.
		string results = sOut.ReadToEnd().Trim();

		// Close the io Streams;
		//     sIn.Close();
		sOut.Close();

		// Write out the results.
		string fmtStdOut = "<font face=courier size=0>{0}</font>";
		this.Response.Write("<br>");
		this.Response.Write("<br>");
		this.Response.Write("<br>");
		this.Response.Write(String.Format(fmtStdOut,
			 results.Replace(System.Environment.NewLine, "<br>")));
	}

	private void CopyThemeFolder(string basePath) {
		string themePath = Web.MapPath("~/theme/" + ThemeName);
		CopyFolder(basePath, themePath);
	}

	private void CopyFolder(string basePath, string folderFullPath) {
		// filter out svns
		if (folderFullPath.Contains(".svn")) return;
		// copy all files in this folder
		var files = System.IO.Directory.GetFiles(folderFullPath);
		foreach (string file in files) {
			string filename = file.RemovePrefix(Web.MapPath(Web.Root));
			CopyFile(basePath, filename);
		}
		// recursively copy aany subfolders
		var folders = System.IO.Directory.GetDirectories(folderFullPath);
		foreach (string folder in folders) {
			CopyFolder(basePath, folder);
		}
	}

	public void FixSLNFile(String basePath) {
		basePath = basePath + "\\";
		string[] fileEntries = Directory.GetFiles(basePath, "*.sln");
		string newName = Web.Request["projname"] + ".sln";
		foreach (string fileName in fileEntries) {
			//Log("f[" + fileName + "]<br>");
			//Log("n[" + basePath + newName + "]<br>");
			Web.Flush();
			if (fileName != basePath + newName) {
				if (!File.Exists(basePath + newName)) {
					File.Move(fileName, basePath + newName);
				} else {
					File.Delete(fileName); //delete old sln file - already done - sln with wrong name was in manifest
				}

				FixProjFileContents(basePath, fileName);
				break;
			} else {
				Log("sln already renamed<br>");
				break;
			}
		}
	}

	public void FixProjFileContents(String basePath, String fileName) {
		var projName = "site.csproj";
		string projContents = FileSystem.GetFileContents(basePath + projName, false);
		//Log("<br>projContents["+projContents+"].");

		var projectName = fileName.Replace(".sln", "");
		projectName = projectName.Substring(projectName.LastIndexOf("\\") + 1);

		Log("<br>projectName[" + projectName + "].");
		var searchText = "http://localhost/" + projectName;
		var newHTTPPath = "http://localhost/" + Web.Request["projname"];
		if (projContents.ContainsInsensitive(searchText)) {
			projContents = projContents.ReplaceInsensitive(searchText, newHTTPPath);
			File.WriteAllText(basePath + projName, projContents);
			Log("site.prj rewritten with proj name[" + Web.Request["projname"] + "]<br>");

		}
	}

	public void ProcessFile(String basePath, String fileName) {
		string fileList = FileSystem.GetFileContents(Server.MapPath(Web.LocalPagePath) + fileName, false);
		//Log(fileList+"<br>.");
		foreach (var file in fileList.Split('\n')) {
			if (!file.StartsWith("#")) {
				CopyFile(basePath, file);
			}
		}
	}

	public void RemoveFilesFromManifest(String basePath, String fileName) {
		//read the manifest file
		string fileList = FileSystem.GetFileContents(Server.MapPath(Web.LocalPagePath) + fileName, false);
		//Log(fileList+"<br>.");
		foreach (var file in fileList.Split('\n')) {
			if (!file.StartsWith("#")) {
				RemoveFile(basePath, file);
			}
		}
	}

	public void RemoveFile(String basePath, String fileName) {
		fileName = fileName.Replace("\r", "");
		if (fileName.IsBlank()) return;
		if (File.Exists(basePath + "\\" + fileName)) {
			File.Delete(basePath + "\\" + fileName);
			Log("file removed[" + basePath + "\\" + fileName + "]<br>");
		} else {
			Log("attempt to remove a file in manifest, but it is not in the file system[" + basePath + "\\" + fileName + "]<br>");

		}

	}

	public void CopyFile(String basePath, String fileName) {
		fileName = fileName.Replace("\r", "");
		if (fileName.IsBlank()) return;
		if (fileName.LastIndexOf('\\') != -1) {
			//not in root
			//Log("create folder[" + basePath + "\\" + file.Substring(0, file.LastIndexOf('\\')) + "]<br>");
			//Web.Flush();
			FileSystem.CreateFolder(basePath + "\\" + fileName.Substring(0, fileName.LastIndexOf('\\'))+'\\');
		}
		if (!File.Exists(basePath + "\\" + fileName)) {
			if (File.Exists(Server.MapPath("~/") + fileName)) {
				File.Copy(Server.MapPath("~/") + fileName, basePath + "\\" + fileName, false);
				//Log("file copied[" + fileName + "]<br>");
			} else {
				Log("file listed in manifest is not in this project[" + fileName + "]<br>");
			}
		} else {
			//Log("file exists[" + fileName + "]<br>");
		}
	}

	private const int HowDeepToScan = 10;
	private List<string> CSProjFiles = new List<string>();

	public void ParseAllCSProjFilesRemovingDeadNodes() {
		foreach (var fileNameNoPath in CSProjFiles) {
			var path = fileNameNoPath.Substring(0, fileNameNoPath.LastIndexOf("\\") + 1);
			var projName = fileNameNoPath.Substring(fileNameNoPath.LastIndexOf("\\") + 1);
			Log("csproj file[" + fileNameNoPath + "]<br />");
			ParseCSproj(path, projName);
		}
	}


	/// <summary>
	/// search for csproj files
	/// </summary>
	/// <param name="sourceDir">phys path to start at</param>
	/// <param name="recursionLvl">start at 0, recurseive call</param>
	public void SearchForCSProjFiles(string sourceDir, int recursionLvl) {
		if (recursionLvl <= HowDeepToScan) {
			// Process the list of files found in the directory. 
			string[] fileEntries = Directory.GetFiles(sourceDir);
			foreach (string fileName in fileEntries) {
				//remove path
				var fileNameNoPath = fileName.Replace(Web.Server.MapPath("~/"), "");
				if (fileNameNoPath.ToLower().EndsWith(".csproj")) {
					//Log("cs file[" + fileNameNoPath + "]<br />");
					//var path = fileNameNoPath.Substring(0, fileNameNoPath.LastIndexOf("\\") + 1);
					//var projName = fileNameNoPath.Substring(fileNameNoPath.LastIndexOf("\\") + 1);
					//Log("path[" + path + "]<br />");
					//Log("projName[" + projName + "]<br />");

					//ParseCSproj(path, projName);
					CSProjFiles.Add(fileNameNoPath);
				}
			}

			// Recurse into subdirectories of this directory.
			string[] subdirEntries = Directory.GetDirectories(sourceDir);
			foreach (string subdir in subdirEntries) {
				// Do not iterate through reparse points
				if ((File.GetAttributes(subdir) &
						 FileAttributes.ReparsePoint) !=
						FileAttributes.ReparsePoint) {
					SearchForCSProjFiles(subdir, recursionLvl + 1);
				}
			}
		}
	}

}


