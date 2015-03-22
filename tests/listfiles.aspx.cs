using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;

namespace Site.tests {
	public partial class listfiles : System.Web.UI.Page {
		const int HowDeepToScan = 10;
		public string excludeList = @"-*.suo;-*.testsettings;-*.pdb;-.svn\;-_beweb\;-.\_backup\;-.\_ReSharper.*\;-.\BewebCore\Beweb\dll\;-.\attachments\;-.\theme\;-obj\Debug\;-js\tiny_mce_3_2_6\;-Visual Studio 2010\Visualizers;-*.ReSharper.user;-*.DotSettings.user;-*.manifest.html;-bin-pub\";				   //from BC exclude list

		protected void Page_Load(object sender, EventArgs e) {
			if (Request["show"]=="orphans") {
				excludeList += GetFilesInManifests();
			}
		}

		protected string GetFilesInManifests() {
			string filesInManifests = "";
			string[] fileEntries = Directory.GetFiles(Server.MapPath("./CreateProject"), "*.manifest.*");
			foreach (string fileName in fileEntries) {
				string txt = FileSystem.GetFileContents(fileName);
				foreach (var line in txt.Split('\n')) {
					if (line.Trim().IsNotBlank() && !line.StartsWith("#")) {
						filesInManifests += ";-" + line.Trim();
					}
				}
			}
			return filesInManifests;
		}

		public void ProcessDir(string sourceDir, int recursionLvl) {
			// note this is a recursive function which is called for each sub-folder
			if (recursionLvl <= HowDeepToScan) {
				// Process the list of files found in the directory. 
				string[] fileEntries = Directory.GetFiles(sourceDir);
				foreach (string fileName in fileEntries) {
					// do something with fileName
					//Console.WriteLine(fileName);

					//remove path
					var fileNameNoPath = fileName.Replace(Web.Server.MapPath("~/"),"");

					bool processFile = true;
					foreach(var filter in excludeList.Split(';'))
					{
						if(!filter.EndsWith("\\"))//file
						{
							if((fileNameNoPath).Contains(filter.Substring(2)))processFile=false;
						}
					}
					if(processFile)
					{

						Web.Write(fileNameNoPath + "\n");
					}
				}


				// Recurse into subdirectories of this directory.
				string[] subdirEntries = Directory.GetDirectories(sourceDir);
				foreach (string subdir in subdirEntries) {
					// Do not iterate through reparse points
					if ((File.GetAttributes(subdir) &
							 FileAttributes.ReparsePoint) !=
									 FileAttributes.ReparsePoint) {

						bool processSubfolder = true;
						var subdirTxt = subdir;
						foreach(var filter in excludeList.Split(';'))
						{
							
							if(filter.EndsWith("\\"))//folder
							{
								//subdirTxt = subdirTxt.Replace("*","");
								//subdirTxt = subdirTxt.RemoveCharsFromEnd(1);
								var filterTxt = filter.RemoveCharsFromEnd(1); //remove the \
								filterTxt = filterTxt.RemoveCharsFromStart(2); //remove the -.
								filterTxt = filterTxt.Replace("*","");
								//Web.Write("["+filterTxt+"]");
								if((subdirTxt).Contains(filterTxt))processSubfolder=false;
							}
						}
						if(processSubfolder)
						{
							ProcessDir(subdirTxt, recursionLvl + 1);
						}
					}
				}
			}
		}
	}
}