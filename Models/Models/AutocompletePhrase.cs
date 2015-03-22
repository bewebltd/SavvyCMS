using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.MobileControls;
using Beweb;
using Site.SiteCustom;

/*
 * To use, copy this code into the partial model class for the records you want to add to the autocomplete list:
  
  	public override void Save() {
			AutocompletePhrase.AddPhrase(this, DealTitle);
			AutocompletePhrase.AddPhrase(this, MetaKeywords, true);
			base.Save();
		}

		public override void Delete() {
			AutocompletePhrase.DeletePhrase(this);
			base.Delete();
		}

 */

namespace Models {
	public partial class AutocompletePhrase {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new AutocompletePhrase object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		//public static void DeletePhrase(string tableName, int recordID) {
		//}
		public static void AutocompletePhraseCleanup() {
			// STAGE 1: Remove any Expired Keywords
			Sql sql = new Sql("delete from AutoCompletePhrase");
			sql.Execute();
			
			// Add keywords for pages that are now Active and are not currently in the AutocompletePhrase table
			// this covers and content that has a publish date in the future
			foreach (var page in PageList.LoadActive()) {
				AddPagePhrase(page);
			}

		//	foreach (var rec in SomeOtherTable.LoadActive()) {
		//		rec.Save();
		//	}
		//	foreach (var rec in SomeOtherTable.LoadActive()) {
		//		rec.Save();
		//	}


		}

		public static void AddPagePhrase(Page page) {
			// delete any existing phrases
			DeletePhrase("Page", page.ID);
			if (page.GetIsActive()) {
			// save the meta keywords to the autocomplete table
				AddPhrase("Page", page.ID, page.Title);
				AddPhrase("Page", page.ID, page.MetaKeywords, true);
				/* Add and other fields you would like to here */
			}else {
				// clean up any child pages as these are no longer navigatable
				foreach (var childPage in page.ChildPages) {
					DeletePhrase("Page", childPage.ID);
				}
			}
		}
		

		public static void AddPhrase(ActiveRecord record, string phrase = null, bool splitPhrase = false, string delimiter = ",") {
			if (phrase==null) {
				phrase = record.GetName();
			}
			if (record.GetIsActive()) {
				AddPhrase(record.GetTableName(), record.ID_Field.ToInt(), phrase, splitPhrase, delimiter);
			} else {
				DeletePhrase(record);
			}
		}

		public static void AddPhrase(string tableName, int recordID, string phrase, bool splitPhrase = false, string delimiter = ",") {
			if(phrase.IsNotBlank()){
				// creat a list from the phrase param if it has a vaule
				List<string> phraseList = new List<string>();
				if (splitPhrase) {
					string[] phrases = phrase.Split(',');
					foreach (var keyword in phrases) {
						phraseList.Add(keyword);
					}
				} else {
					phraseList.Add(phrase);
				}
				// loop through the resulting phraselist
				foreach (var keyword in phraseList) {
					if (keyword.IsNotBlank()) {
						// save the new phrase if it is not null or blank
						var newPhrase = new AutocompletePhrase();
						newPhrase.TableName = tableName;
						newPhrase.RecordID = recordID;
						newPhrase.Phrase = keyword.Trim();
						newPhrase.Save();
					}
				}
			}
		}

		public static void DeletePhrase(ActiveRecord record) {
			DeletePhrase(record.GetTableName(), record.ID_Field.ToInt());
		}

		public static void DeletePhrase(string tableName, int recordID) {
			new Sql("delete from AutocompletePhrase  where tablename=", tableName.SqlizeText(), "and recordid=", recordID).Execute();
		}
		
	}
	
}

namespace BewebTest {
	[TestClass]
	public class TestAutocompletePhrase {
		[TestMethod]
		public static void TestSomething() {
			var expectedValue = 5;
			var actualValue = 5;
			Assert.AreEqual(expectedValue,actualValue);
		}
	}
}

// created: [ 28 Mar 2012 2:53:23pm ]