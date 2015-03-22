/*
 *
 * -----------------------------------------------------------------------------------------------------------------------------------------------------
 *                                                     Copyright (C) 2009 - 2012 By Michael Roberts
 * -----------------------------------------------------------------------------------------------------------------------------------------------------
 *  This is a parser for a complex CSV (Comma Seperated Value Document) that is part of the "Dragon Fire Web Browser" Parsing Engine.
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using Beweb;

namespace ThirdParty {
	public static class CommaSeparatedValueFile {

		public static List<List<string>> Parse(string path) {
			List<List<string>> parsedData = new List<List<string>>();
			uint total_comments = 0;
			uint total_blankLines = 0;
			bool tokenInQuotes = false;
			bool tokenContinued = true;
			uint total_tokens = 0;
			string temp_println = "";
			List<string> resultLine = new List<string>();

			try {
				StreamReader readFile = new StreamReader(path);
				string readLine = null;
				string printLine = null;

				while ((readLine = readFile.ReadLine()) != null) {
					// Ignore Any Lines Starting With ';'
					if (readLine.StartsWith(";")) {
						printLine = null;
						total_comments = total_comments + 1;
					} else if (readLine.Trim() == null || readLine.Length == 0) {
						// If line is not comment line check if its blank
						printLine = null;
						total_blankLines = total_blankLines + 1;
					} else if ((readLine.Trim() != null) && (!readLine.StartsWith(";"))) { 
						// Check For Any Other Characters (Default Action)
						// Cycle Each Character
						foreach (char character in readLine) {
							if (tokenContinued == true) {
								temp_println = printLine;
								printLine = temp_println;
							}
							// Split Tokens At The Commas
							if (character == ',') {
								if (!tokenInQuotes) {
									total_tokens = total_tokens + 1;
									//Web.Write("  [*] [" + printLine + "]");
									resultLine.Add(UnescapeToken(printLine));
									//Web.Write("done");
									printLine = null;
									tokenContinued = false;
									temp_println = null;
								} else if (tokenInQuotes) {
									total_tokens = total_tokens - 0;
									printLine += character;
									tokenContinued = true;
								}
								continue;
							}

							if (character == '\"') {
								// Check For Start Of Quotation
								if (character == '\"' && tokenInQuotes == false) {
									tokenInQuotes = true;
									printLine += character;
									continue;
								}

								// Check for end of Quotations
								else if (tokenInQuotes == true && character == '\"') {
									tokenInQuotes = false;
									printLine += character;
									continue;
								}
							}

							//// Check For Internal Comments -- MN removed this bullshit
							//if (character == ';') {
							//  total_comments = total_comments + 1;
							//  temp_println = printLine;
							//  printLine = null;
							//  printLine = temp_println;
							//  break;
							//}

							// Handle all other characters
							//if (character != ';' && character != '\"' && character != ',') {
							if (character != '\"' && character != ',') {
								printLine += character;
								continue;
							}
						}
						// Print tokens at the end of the line
						if (tokenContinued == false) {
							total_tokens = total_tokens + 1;
							//Web.Write("  {*} " + printLine + "<br/>");
							resultLine.Add(UnescapeToken(printLine));
							parsedData.Add(resultLine);
							resultLine = new List<string>();
							printLine = null;
							temp_println = null;
						}
					}
				}

				readFile.Close();
				readFile.Dispose();

				//Web.Write("File Stats: ");
				//Web.Write("  (*) File Contains " + total_comments + " Comments");
				//Web.Write("  (*) File Contains " + total_blankLines + " Blank Lines");
				//Web.Write("  (*) File Contains " + total_tokens + " tokens");

			} catch (DirectoryNotFoundException dnfe) {

				throw new Exception("CSV Import Error: Directory For The Comma Seperated Value (CSV) File Could Not Be Found." + dnfe);
				/*
								string error = dnfe.ToString();
				*/

			} catch (EndOfStreamException eose) {

				throw new Exception("CSV Import Error: There Was An Unexpected End To The Comma Seperated Value (CSV) File" + eose);
				/*
								string error = eose.ToString();
				*/

			} catch (FileNotFoundException fnfe) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Could Not Be Found." + fnfe);
				/*
								string error = fnfe.ToString();
				*/

			} catch (FileLoadException fle) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Could Not Be Loaded." + fle);
				/*
								string error = fle.ToString();
				*/

			} catch (PathTooLongException ptle) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Path Is Too Long." + ptle);
				/*
								string error = ptle.ToString();
				*/

			} catch (IOException ioe) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Encountered an I/O Error.\nDetails: " + ioe.Message);
				/*
								string error = ioe.ToString();
				*/

			} catch (ArgumentOutOfRangeException aoore) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) Parser Has Encountered Too Many Arguments." + aoore);
				/*
								string error = aoore.ToString();
				*/

			} catch (IndexOutOfRangeException ioore) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Was Not Indexed Correctly.Try Again." + ioore);
				/*
								string error = ioore.ToString();
				*/

			} catch (NullReferenceException nfe) {

				//throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Encountered An Empty Array Listing.");
				string error = nfe.ToString();
				Web.Write(error);

			} catch (EntryPointNotFoundException epnfe) {

				throw new Exception("CSV Import Error: The Comma Seperated Value (CSV) File Could Not Find The Entry Point." + epnfe);
				/*
								string error = epnfe.ToString();
				*/

			} catch (OutOfMemoryException oome) {

				throw new Exception("CSV Import Error: The Parser Has Run Out Of Memory" + oome);
				/*
								string error = oome.ToString();
				*/


			}
			return parsedData;
		}

		private static string UnescapeToken(string str) {
			if (str != null) {
				if (str.StartsWith("\"") && str.EndsWith("\"")) {
					str = str.RemoveCharsFromStart(1).RemoveCharsFromEnd(1);
				}
				if (str.Contains("\"\"")) {
					str = str.Replace("\"\"", "\"");
				}
			}
			return str;
		}
	}
}
