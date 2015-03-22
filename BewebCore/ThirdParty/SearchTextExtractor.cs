#define RTFProcessingAvailable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beweb;
using BewebCore.ThirdParty.ReadWordDocText;

#if RTFProcessingAvailable
using Net.Sgoliver.NRtfTree.Core;
#endif
using PdfToText;

namespace BewebCore.ThirdParty {
	public class SearchTextExtractor {

		public static void CheckAttachmentsForDocOrPDFText(ActiveRecord record) {
			//walk the field list for this record looking for attachments
			foreach (var fieldName in record.GetFieldNames()) {
				if (fieldName.Contains("Attachment") && fieldName.DoesntContain("RawText")) {

					//if (record.Fields.Attachment.IsDirty) {
					if (ActiveFieldBase.IsDirtyObj(record[fieldName].ValueObject, record[fieldName].OriginalValueObject)) {
						if (record[fieldName].ToString().Contains(".doc") || record[fieldName].ToString().EndsWith(".pdf")|| record[fieldName].ToString().EndsWith(".rtf")) {
							if (!record.FieldExists(fieldName + "RawText")) {
								(new Sql("ALTER TABLE ", record.GetTableName().SqlizeName(), " ADD [" + fieldName + "RawText] nvarchar (MAX);")).Execute();
							}
							string output = "";
							if (record[fieldName].ToString().ToLower().EndsWith(".doc")) {
								OfficeFileReader.OfficeFileReader objOFR = new OfficeFileReader.OfficeFileReader();
								if (objOFR.GetText(Web.MapPath(Web.Attachments) + record[fieldName].ToString(), ref output) > 0) {
									//ok
								}
							} else if (record[fieldName].ToString().ToLower().EndsWith(".docx")) {
								BewebCore.ThirdParty.ReadWordDocText.DocxToText objOFR = new DocxToText(Web.MapPath(Web.Attachments) + record[fieldName].ToString());
								if ((output = objOFR.ExtractText()).Length > 0) {
									//ok
								}
							} else if (record[fieldName].ToString().Contains(".pdf")) {
								PdfToText.PDFParser pdf = new PDFParser();
								if (pdf.ExtractText(Web.MapPath(Web.Attachments) + record[fieldName].ToString(), ref output)) {
									//ok
								}
							} else if (record[fieldName].ToString().Contains(".rtf")) {
#if RTFProcessingAvailable
							//Create the RTF tree object
								RtfTree tree = new RtfTree();

								//Load and parse RTF document
								tree.LoadRtfFile(Web.MapPath(Web.Attachments) + record[fieldName].ToString());
								output = tree.Text;
#else
								throw new Exception("rtf library not included");
#endif
							}
							if (output.Trim() != "") {
								(new Sql("update ", record.GetTableName().SqlizeName(), "set " + fieldName + "RawText=", output.SqlizeText(), " where ",
											record.GetPrimaryKeyName().SqlizeName(), "=", record.ID_Field.Sqlize(), "")).Execute();
							}

						} else {
							//no doc any more
							if (record.FieldExists(fieldName + "RawText")) {
								(new Sql("update ", record.GetTableName().SqlizeName(), "set " + fieldName + "RawText=null where ",
									record.GetPrimaryKeyName().SqlizeName(), "=", record.ID_Field.Sqlize(), "")).Execute();
							}
						}
					}
				}
			}
		}
	}
}
