using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beweb;
namespace Beweb {
	/// <summary>
	/// This file should be the only difference in this folder when beyond comparing to codelib.
	/// These settings are the latest fashion - eg cool new date picker.
	/// All default settings inside other classes must be set to err on the side of backwards compability - eg same old buggy date picker.
	/// Note: to override on an app-specific basis, just set the properties you want to override in global.aspx.cs or just set them below
	/// </summary>
	public class BewebCoreSettings {
		public static BewebCoreSettings Settings;// = new BewebCoreSettings(); -- this is now called in gloabal.asax.cs app_start

		public BewebCoreSettings() {
			// these settings are the latest fashion - eg cool new date picker
			// all default settings inside other classes must be set to err on the side of backwards compability - eg same old buggy date picker
			Forms.DefaultDateSelector = Forms.DateSelectorOptions.JQueryUI;
			Forms.DefaultDateTimeSelector = Forms.DateTimeSelectorOptions.DateAndTimeInputs;
			//Security.PasswordMode = Security.PasswordModes.Level1Unencrypted;
			//Security.PasswordMode = Security.PasswordModes.Level2ReversibleEncryptionAndUnencrypted;
			Security.PasswordMode = Security.PasswordModes.Level3ReversibleEncryption;
			//Security.PasswordMode = Security.PasswordModes.Level4HashedOneWay;
			ActiveRecordGenerator.ReformatTableNames = false;
			ActiveRecordGenerator.ThrowExceptionOnFieldNameSameAsClass = true;         // for new systems set this to true, for old systems set to false for previous generating behaviour
			Forms.UseUniqueRadioIDs = true;        // for new systems set this to true
			Forms.DefaultShowInlineValidation = true;       // render a span after each field, to hold validation
			Forms.DefaultShowInlineAdminValidation = true;
			Fmt.DefaultDateFormatHasDashes = false;       // new format has no dashes
			Fmt.SqlDateIncludesTime = true;       // new format since 2011 SqlDate does not include time
			DelimitedString.ThrowExceptionOnStringContainsDelimiter = true;       // for new systems set this to true
			Forms.YesNoField.CurrentStyle = Forms.YesNoField.YesNoStyle.Style2;       // for new systems set this to 2
			Forms.PictureField.UseAcceptAttrib = true;
			Forms.NumberField.DefaultGroupDigits = true;							//usually true
			ActiveRecord.PossibleFieldNamesForPublishDate = "PublishDate";
			ActiveRecord.PossibleFieldNamesForExpiryDate = "ExpiryDate";
			ActiveFieldBase.IgnoreConversionErrors = false;			// default to false. when reading from request, ignore convrsion errors - e.g. user put 'lorem' in a date field
			//Web.Attachments = Web.Root + "Attachments/";
			//Web.ClassicAttachments = Web.Root + "Classic/Attachments/"; 
			//Web.Images = Web.BaseUrlNoProtocol + "/images/";  // todo - make it use a cookieless domain something along these lines
			Http.DefaultEncoding = Encoding.UTF8;		
		}
	}
}

