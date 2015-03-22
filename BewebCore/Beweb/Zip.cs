//#define UseCSharpZipLib

#if UseCSharpZipLib
using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Beweb
{
	// found this at http://www.eggheadcafe.com/tutorials/aspnet/9ce6c242-c14c-4969-9251-af95e4cf320f/zip--unzip-folders-and-f.aspx
	// and modified it:
	// added includeFolderInZip bool
	// added file close so it doesn't keep file open
	// changed to put resulting zip at same level as the folder it is zipping

	public static class Zip
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputFolderPath">full path to folder. Do not include final slash</param>
		/// <param name="outputPathAndFile">example: file.zip - it will be placed on the same file as the folder you zipped</param>
		/// <param name="password"></param>
		/// <param name="includeFolderInZip">should the folder be the single element in the root of the zip? If false the files are in the root</param>
		public static void ZipFiles(string inputFolderPath, string outputPathAndFile, string password, bool includeFolderInZip)
		{
			ArrayList ar = GenerateFileList(inputFolderPath); // generate file list

			string inputFolderParentPath = (Directory.GetParent(inputFolderPath)).ToString();
			
			// we trim the full path when adding to zip file - by how much depends on includeFolderInZip
			int trimLength = inputFolderPath.Length;
			if (includeFolderInZip) trimLength = inputFolderParentPath.Length;
			trimLength += 1; //remove '\'
			
			FileStream ostream;
			byte[] obuffer;
			string outPath = inputFolderParentPath + @"\" + outputPathAndFile;
			ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream
			if (!string.IsNullOrEmpty(password)) {
				oZipStream.Password = password;
			}
			oZipStream.SetLevel(9); // maximum compression
			ZipEntry oZipEntry;
			foreach (string fil in ar) // for each file, generate a zipentry
			{
				oZipEntry = new ZipEntry(fil.Remove(0, trimLength));
				oZipStream.PutNextEntry(oZipEntry);

				if (!fil.EndsWith(@"/")) // if a file ends with '/' its a directory
				{
					ostream = File.OpenRead(fil);
					obuffer = new byte[ostream.Length];
					ostream.Read(obuffer, 0, obuffer.Length);
					oZipStream.Write(obuffer, 0, obuffer.Length);
					ostream.Close();
				}
			}			
			oZipStream.Finish();
			oZipStream.Close();
		}

		private static ArrayList GenerateFileList(string Dir)
		{
			ArrayList fils = new ArrayList();
			bool Empty = true;
			foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
			{
				fils.Add(file);
				Empty = false;
			}

			if (Empty)
			{
				if (Directory.GetDirectories(Dir).Length == 0)
				// if directory is completely empty, add it
				{
					fils.Add(Dir + @"/");
				}
			}

			foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
			{
				foreach (object obj in GenerateFileList(dirs))
				{
					fils.Add(obj);
				}
			}
			return fils; // return file list
		}

		public static void UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
		{
			ZipInputStream s = new ZipInputStream(File.OpenRead(zipPathAndFile));
			if (password != null && password != String.Empty)
				s.Password = password;
			ZipEntry theEntry;
			string tmpEntry = String.Empty;
			while ((theEntry = s.GetNextEntry()) != null)
			{
				string directoryName = outputFolder;
				string fileName = Path.GetFileName(theEntry.Name);
				// create directory 
				if (directoryName != "")
				{
					Directory.CreateDirectory(directoryName);
				}
				if (fileName != String.Empty)
				{
					if (theEntry.Name.IndexOf(".ini") < 0)
					{
						string fullPath = directoryName + "\\" + theEntry.Name;
						fullPath = fullPath.Replace("\\ ", "\\");
						string fullDirPath = Path.GetDirectoryName(fullPath);
						if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
						FileStream streamWriter = File.Create(fullPath);
						int size = 2048;
						byte[] data = new byte[2048];
						while (true)
						{
							size = s.Read(data, 0, data.Length);
							if (size > 0)
							{
								streamWriter.Write(data, 0, size);
							}
							else
							{
								break;
							}
						}
						streamWriter.Close();
					}
				}
			}
			s.Close();
			if (deleteZipFile)
				File.Delete(zipPathAndFile);
		}
	}
}
#endif