#define UsePictureActiveField
#define UseAttachmentActiveField
#define Fmt
//#define EnableAutoTypeConversion
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web;
using Beweb;
namespace Beweb {

#if UsePictureActiveField
	public class PictureActiveField : AttachmentActiveField {
		public static PictureMetaDataAttribute DefaultMetaData;
		private new PictureMetaDataAttribute _metaData = DefaultMetaData;

		public PictureMetaDataAttribute MetaData {
			get {
				if (_metaData == null) throw new ActiveRecordException("Picture MetaData is not defined for field [" + TableName + "].[" + Name + "]. Please define in model partial class (or delete and regenerate model partial file in Models subfolder).");
				return _metaData;
			}
			set { _metaData = value; }
		}

		public string GetSubFolder() {
			return Subfolder;
		}

		protected override string Subfolder {
			get {
				return MetaData.Subfolder;
			}
		}
		//public string ImageName { get; set; }  20100427 MN - is this needed? confusing
		//public string ClientFilePath { get; set; }  20100427 MN - is this needed? confusing

		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage.png)
		/// </summary>
		public string ImagePath {
			get { return Beweb.ImageProcessing.ImagePath(FileName); }
		}

		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage???.png)
		/// fileSuffix eg "_tn"
		/// </summary>
		public string ImageCustomSizePath(string fileSuffix) {
			return ImageProcessing.ImagePath(FileName, fileSuffix ?? "_tn");
		}

		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage_tn.png)
		/// </summary>
		public string ImageThumbPath {
			get { return Beweb.ImageProcessing.ImageThumbPath(FileName); }
		}

		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage_pv.png)
		/// </summary>
		public string ImagePreviewPath {
			get { return Beweb.ImageProcessing.ImagePreviewPath(FileName); }
		}

		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage_med.png)
		/// </summary>
		public string ImageMediumPath {
			get { return Beweb.ImageProcessing.ImageMediumPath(FileName); }
		}

		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage_med.png)
		/// </summary>
		public string ImageBigPath {
			get { return Beweb.ImageProcessing.ImageBigPath(FileName); }
		}
		
		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage_zm.png)
		/// </summary>
		public string ImageZoomPath {
			get { return Beweb.ImageProcessing.ImageZoomPath(FileName); }
		}
		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Returns URL path resolved to the site root (eg /attachments/folder/myimage_sml.png)
		/// </summary>
		public string ImageSmallPath {
			get { return Beweb.ImageProcessing.ImageSmallPath(FileName); }
		}

		public Size Dimensions {
			get { return ImageProcessing.GetImageDimensions(Web.MapPath(ImagePath)); }
		}

		public Size ThumbDimensions {
			get { return ImageProcessing.GetImageDimensions(Web.MapPath(ImageThumbPath)); }
		}

		public Size PreviewDimensions {
			get { return ImageProcessing.GetImageDimensions(Web.MapPath(ImagePreviewPath)); }
		}

		// MN 20100819 - this function is useless, it just writes out the path!
		//public string Html() {
		//   return Beweb.ImageProcessing.ImagePreviewPath(this.Value); 
		//} 
		public string ToHtml(string altText, string cssClass) {
			return Beweb.Html.Picture(this, altText, cssClass);
		}
		public string ToHtml(string altText) {
			return ToHtml(altText, "");
		}

		/// <summary>
		/// If this image was uploaded, grab a reference to the posted file from the request.
		/// </summary>
		public override bool UpdateFromRequest(string prefix, string suffix) {
			// also check for field option for user to select cropping/resize
			string scale = Web.Request[prefix + "scale_" + this.Name + suffix] + "";
			if (scale.IsNotBlank()) {
				if (scale == "Scale") {
					MetaData.IsCropped = false;
					MetaData.IsThumbnailCropped = false;
					MetaData.IsPreviewCropped = false;
				} else if (scale == "Crop") {
					MetaData.IsThumbnailCropped = true;
					MetaData.IsPreviewCropped = true;
				}
			}

			string pasteFileName = Web.Request[prefix + "paste_" + this.Name + suffix] + "";
			if (pasteFileName.IsNotBlank()) {
				// move file into correct subfolder
				string guidFileName = pasteFileName.RightFrom("\\").RightFrom("/");
				string newLocation = this.MetaData.Subfolder + guidFileName;
				string pasteFileLocation = Web.MapPath(Web.Attachments + pasteFileName);

				//override with real file name if passed from hidden JC 20140902
				string realFileName = Web.Request[prefix + "RealFileName_" + this.Name + suffix] + "";
				if (realFileName.IsNotBlank()) {
					realFileName= FileSystem.GetUniqueFilename(Web.Attachments + this.Subfolder,realFileName, this.MaxLength - this.Subfolder.Length);
					newLocation = this.MetaData.Subfolder + realFileName;
				}

				ImageProcessing.ResizeImageUsingMetaData(newLocation, this.MetaData, pasteFileLocation);
				File.Delete(pasteFileLocation);
				this.Value = newLocation;
				if (realFileName.IsNotBlank()) {
					return true;
				}
			}

			// do base from file upload
			return base.UpdateFromRequest(prefix, suffix);
		}

		/// <summary>
		/// If an image file was uploaded, this is where it is validated, resized, thumbnails created, and saved to the disk in the correct subfolder of the attachments folder.
		/// </summary>
		public override void PrepareForSave(ActiveRecord record) {
			this.wasPosted = false; // assume we are not going to re-save this
			if (toDelete || (fileData != null && this.OriginalValue.IsNotBlank())) {
				//string pkname = record.GetPrimaryKeyName();
				//string filename = (new Sql("select ", this.Name.SqlizeName(), " from ", this.TableName.SqlizeName(), " where ",pkname.SqlizeName(),"=",record[pkname].Sqlize())).FetchString();
				PrepareForDelete(true);
				this.wasPosted = true;

			}

			if (fileData != null && this.FileName != null) {

				if (this.MetaData.UseGuidFileNaming) {
					// don't make up a filename or location - not needed as it is generated automatically
				} else {
					// create subfolder if not existing
					FileSystem.CreateFolder(Web.Attachments + Subfolder);
					MakeUniqueFilename();
				}
				// get the filename back out (may not be the same as sent in, if UseGuidFileNaming=true)
				this.FileName = ImageProcessing.ResizeImageUsingMetaData(this.FileName, this.MetaData, this.fileData);

				// dont need this fileuploadstream in memory anymore
				fileData.InputStream.Dispose();
				fileData = null;

				this.wasPosted = true;

				// 20100809 dont call this as it appends the file to the folder badly without a slash (eg 300x300-113x150lion-2.gif)
				//base.PrepareForSave(record);
			}
			if (this.FileName != null) //set by the server files selector
			{
				this.wasPosted = true;
			}
		}

		public override void PrepareForDelete(bool deleteAttachedFiles) {
			var fileName = this.OriginalValue;
			// we assume image might be used elsewhere if AllowSelectFromServer, so don't delete it
			if (MetaData != null && MetaData.AllowSelectFromServer) {
				deleteAttachedFiles = false;
			}
			if (fileName.IsNotBlank() && deleteAttachedFiles) {
				FileSystem.DeletePictureAttachment(fileName);
			}
			base.PrepareForDelete(deleteAttachedFiles);
		}

		/// <summary>
		/// Given a filename, resize and create all the required thumbnails, and update the value to be the resulting output filename.
		/// sourceAttachmentFileName is relative path from attachments folder (eg "chair.jpg" or "1024x768/chair.jpg")
		/// </summary>
		/// <param name="sourceAttachmentFileName"></param>
		public void ResizeAndSaveImage(string sourceAttachmentFileName) {
			this.Value = sourceAttachmentFileName;
			MakeUniqueFilename();
			this.Value = ImageProcessing.ResizeImageUsingMetaData(sourceAttachmentFileName, this.MetaData);
		}

	}
#endif
#if UseAttachmentActiveField
	public class AttachmentMetaDataAttribute : Attribute {
		private string _subfolder;

		public AttachmentMetaDataAttribute() {
			Subfolder = "";
		}

		/// <summary>
		/// Specify a subfolder path to save attachments in a subfolder underneath ~/attachments/ (which is the default). 
		/// </summary>
		public string Subfolder {
			get {
				if (_subfolder.IsBlank()) return "";
				return _subfolder.TrimEnd("/\\") + "/";
			}
			set {
				_subfolder = value;
			}
		}

		/// <summary>
		/// Shows a Paste and Drag Drop area to allow the user to paste or drag the image directly from the clipboard, if their browser supports it.
		/// </summary>
		public bool AllowPasteAndDrag { get; set; }

		/// <summary>
		/// NOT IMPLEMENTED
		/// </summary>
		public bool UseGuidFileNaming { get; set; }

	}

	public class AttachmentActiveField : ActiveField<string> {
		public AttachmentMetaDataAttribute MetaData;
		protected virtual string Subfolder {
			get {
				if (MetaData == null) return "";
				return MetaData.Subfolder;
			}
		}
		protected HttpPostedFile fileData;
		protected string fileContentType;
		protected int fileLength;
		protected bool toDelete;
		private bool savedAlready;

		public string ErrorMessage { get; set; }

		public bool Exists {
			get { return this.Value.IsNotBlank(); }
		}

		public string FileName { get { return Value; } set { Value = value; } }

		public override bool UpdateFromRequest(string prefix, string suffix) {
			// file attachment/image removing functionality

			bool removeItem = (Web.RequestEx[prefix + this.Name + "_remove" + suffix] == "1");
			this.toDelete = (removeItem) ? true : false;

			string attachmentFileName = Web.Request["attachmentFileName_" + this.Name];

			if (attachmentFileName.IsNotBlank()) {
				this.FileName = attachmentFileName;
				return true;
			}

			var file = Web.Request.Files[prefix + this.Name + suffix];
			if (file == null) {
				wasPosted = false;
				// try new filename from server file selector
				var filename = Web.Request[prefix + this.Name + suffix];
				if (filename != null) {
					wasPosted = true;
					this.FileName = filename;
					if (Web.Request.ServerVariables["CONTENT_TYPE"] == "application/x-www-form-urlencoded") {
						// MK 20110728 breaks on recipe edit/update with ajax-style image upload / 20110806 MN Does it, or was this due to incorrect mime type?
						throw new ActiveRecordException("UpdateFromRequest: Picture or Attachment field attempting to be posted in a form with the wrong enctype. enctype attribute needs to be set to multipart/form-data");
					}
				}
			} else{
				wasPosted = true;
				this.fileLength = file.ContentLength;
				if (fileLength == 0) {
				} else {
					// 20110627 MN - internal change - don't pass the stream across - because streams need disposing
					this.fileData = file;
					this.fileContentType = file.ContentType;
					this.FileName = file.FileName.RightFrom("\\");
					// check mime type
					// maybe generate a guid filename at this point
				}
			}
			return wasPosted;
		}

		/// <summary>
		/// If a file was uploaded, this is where it is saved to the disk in the attachments folder.
		/// </summary>
		public override void PrepareForSave(ActiveRecord record) {
			this.wasPosted = false;			// set to false, but back to true if file supplied
			if (toDelete || (fileData != null && this.OriginalValue.IsNotBlank())) {
				PrepareForDelete(true);
				this.wasPosted = true;
			}

			if (fileData != null && this.FileName != null && !this.savedAlready) {
				//save the file if just been uploaded
				using (var stream = fileData.InputStream) {
					if (stream != null && (stream.Position == 0 || (stream.Length > 0 && stream.Position != stream.Length))) //JN only save if not already saved (eg my picture control)
					//if (true) // do we need the line above?
					{
						// correct filename
						MakeUniqueFilename();
						//this.FileName = FileSystem.GetUniqueFilename(Web.Attachments, this.FileName, this.MaxLength);
						string fullPath = Web.MapPath(Web.Attachments + this.FileName);
						stream.SaveStreamToFile(fullPath);
						this.wasPosted = true;
						stream.Close();
						stream.Dispose();
						this.savedAlready = true;
					}
				}
			}
			base.PrepareForSave(record);
		}

		protected void MakeUniqueFilename() {
			// create subfolder if not existing
			FileSystem.CreateFolder(Web.Attachments + Subfolder);
			int maxSuffixLen = "_med".Length;
			var maxLen = this.MaxLength - Subfolder.Length - maxSuffixLen;
			this.FileName = FileSystem.GetUniqueFilename(Web.Attachments + Subfolder, this.FileName, maxLen);
			this.FileName = Subfolder + this.FileName;
		}

		/// <summary>
		/// delete the files
		/// </summary>
		public override void PrepareForDelete(bool deleteAttachedFiles) {
			if (deleteAttachedFiles) {
				if (!FileSystem.DeleteAttachment(this.OriginalValue)) {
					Web.ErrorMessage = "Failed to delete "+this.OriginalValue;
				}
			}
			// MK & MN: was deleting the newly uploaded file if the box was ticked
			//if (this.toDelete && !this.IsDirty) {
			if (this.toDelete && fileData == null) {
			//if (this.toDelete && fileData == null) {  -- TODO Victor to fix!
				this.Value = null;
			}
		}
	}

#endif

	/// <summary>
	/// Represents a column in an Active Record.
	/// Contains data value and meta data.
	/// This is a generic class which uses a type parameter to represent the type of the field.
	/// </summary>
	public class ActiveField<T> : ActiveFieldBase {
		public ActiveField() {
			// set to default value for the given type (eg null if a nullable type or false if bool)
			this._valueObject = default(T);
			this.OriginalValueObject = default(T);
			this.SavedValueObject = default(T);
		}

		public T Value {

			get {
				T result;
				try {
					if (this.ValueObject == null || this.ValueObject == System.DBNull.Value) {
						result = default(T);
					} else {
						result = (T)this.ValueObject;
					}
				} catch (Exception ex) {
					throw new ProgrammingErrorException("Get value failed. value[" + this.ValueObject + "], name[" + this.Name + "], col type[" + this.ColumnType + "], table[" + this.TableName + "]", ex);
				}

				return result;
			}
			set { this.ValueObject = value; }
		}

		public T OriginalValue {
			get { return (T)((this.OriginalValueObject != System.DBNull.Value) ? this.OriginalValueObject : default(T)); }
			set { this.OriginalValueObject = value; }
		}

		[Obsolete("No longer in use. Now use ActiveFieldBase.UpdateFromRequest(prefix, suffix)")]
		public void UpdateFromRequest() {
			this.ValueObject = Convert.ChangeType(Web.RequestEx[this.Name], typeof(T));
		}

		public override SqlizedValue Sqlize() {
			object val = Value;
			if (IsTextField && CurrentAutoFixBaseUrl && val != null) {
				string stringValue = val.ToString();
				val = Fmt.InsertWebsiteBaseUrlPathMarkers(stringValue);
			}
			return SqlStringBuilder.Sqlize(val);
		}

		public override void FromString(string stringValue) {
			// MN 20131111 - moved this code into utility method, now just call base
			base.FromString(stringValue);
		}

	}

	/// <summary>
	/// Represents a column in an Active Record.
	/// Contains data value and meta data.
	/// This is the base class. You would normally use the typed generic version.
	/// </summary>
	public class ActiveFieldBase {
		/// <summary>
		/// Note this should not be set back and forth by logic directly as it is a static (shared between threads) and must be the same for the whole application
		/// Therefore I dont want to make it public (should be internal static readonly)
		/// </summary>
		internal static readonly bool AutoFixBaseUrl = Util.GetSettingBool("SavvyActiveRecord_AutoFixBaseUrl", false);

		protected bool CurrentAutoFixBaseUrl {
			get {
				if (!AutoFixBaseUrl) return false;
				var exclusions = Util.GetSetting("SavvyActiveRecord_AutoFixBaseUrlExclusions", "").Split("|");
				return exclusions.DoesntContain(TableName);
			}
		}

		public static bool IgnoreConversionErrors { get; set; }

		public string Name;
		protected object _valueObject;
		public object ValueObject {
			get { return _valueObject; }
			set {
				_valueObject = value;

				//now check that the type is correct, so that subsequent calls to get value return items of the correct type (type is used in sql string builder for example)
				if (value != null && this.Type != null && value.GetType() != this.Type) {
					if (this.Type.IsNullableType() && !value.GetType().IsNullableType() && value.GetType().MakeNullableType() == this.Type) {
						//ok - it is a nullable version of the same type
					} else if (value.GetType() == typeof(DBNull)) {
						if (CurrentAutoFixBaseUrl) {
							_valueObject = null;
						}
						//else for old projects, leave it as DBNull because this is more backwards compatible
					} else if (value.GetType() == typeof(string)) {
						FromString(value + "");
					} else {
						throw new ActiveRecordException("Failed to convert valueobject[" + value + "] is the incorrect type [" + value.GetType() + "], should be [" + this.Type.FullName + "]");
					}
				}
			}
		}

		/// <summary>
		/// The original value of the field when it was loaded from the database. In the case of a new record, it will be the default value for that field (usually null or false).
		/// (This value is NOT reset when the record is saved, so you can continue to check against original value even in code after the Save() is called.)
		/// </summary>
		public object OriginalValueObject;

		/// <summary>
		/// The value of the field that is currently stored in the database. In the case of a new record, it will be the default value for that field (usually null or false).
		/// (This value is reset when the record is saved - used by save function for sql update and modlog)
		/// </summary>
		public object SavedValueObject;

		public bool AllowNulls = true;
		private Type _type;

		/// <summary>
		/// Returns the c# type which corresponds to the database column type (eg string, bool, int, int?, decimal?, System.DateTime? etc)
		/// </summary>
		public Type Type {
			get { return _type; }
			set {
				_type = value;
				AllowNulls = (_type.IsNullableTypeOrString());
			}
		}

		/// <summary>
		/// The database column type (eg INT, BIT, NTEXT, NVARCHAR etc)
		/// </summary>
		public string ColumnType;

		/// <summary>
		/// Maximum length in characters for string fields (which is useful for maxlen on input boxes) or number of bytes for other fields (which is typically not very useful).
		/// </summary>
		public int MaxLength;

		public int? DecimalPlaces = null;

		/// <summary>
		/// Returns true if the field is an autonumber (identity) column. Returns false for normal fields or any other kind of calculated columns (eg total columns).
		/// </summary>
		public bool IsAuto = false;

		//public bool IsDirty { get { return Equals(ValueObject, OriginalValueObject); } }

		/// <summary>
		/// Returns true if the field value has changed since it was loaded from the database. In the case of a new record, it returns true if the value has been set and is not the default value for that field (eg null or false).
		/// (Dirty flag is NOT reset when the record is saved, so you can continue to check if dirty even in code after the Save() is called. Dirty does not mean different from database, it means changed since you loaded. If you really want to reset it, you can call record.ReloadFromDatabase() or ClearDirtyFlag().)
		/// </summary>
		public bool IsDirty {
			get {
				if (!IsDirtyObj(ValueObject, OriginalValueObject)) return false;

				// they are different, therefore dirty
				return true;
			}
		}

		protected internal bool IsDirtySinceSaved {
			get {
				if (!IsDirtyObj(ValueObject, SavedValueObject)) return false;

				// they are different, therefore dirty
				return true;
			}
		}

		/// <summary>
		/// Returns true if the field value has changed since it was loaded from the database. In the case of a new record, it returns true if the value has been set and is not the default value for that field (eg null or false).
		/// (Dirty flag is NOT reset when the record is saved, so you can continue to check if dirty even in code after the Save() is called. Dirty does not mean different from database, it means changed since you loaded. If you really want to reset it, you can call record.ReloadFromDatabase() or ClearDirtyFlag().)
		/// </summary>
		public static bool IsDirtyObj(object ValueObject, object OriginalValueObject) {
			if (Equals(ValueObject, OriginalValueObject)) return false; // they are the same

			if (OriginalValueObject + "" == ValueObject + "") return false; // cast to strings, they are the same

			if (OriginalValueObject + "" != "" && ValueObject + "" != "") {
				// new records can have one blank and one decimal, which is always dirty - MUST do string casting
				if (OriginalValueObject is decimal || ValueObject is decimal) {
					// we have a decimal, so because money fields in SQL only store 4 decimal places, see if our decimals are the same (to four decimal places) which is close enough
					if (Math.Round((Decimal) OriginalValueObject, 4) == Math.Round((Decimal) ValueObject, 4)) return false;
				}
			}
			return true;
		}


		public bool IsNull { get { return ValueObject == null || ValueObject == DBNull.Value; } }

		public bool IsNotNull { get { return !IsNull; } }

		public bool IsBlank { get { return ValueObject == null || ValueObject == DBNull.Value || ValueObject + "" == ""; } }

		public bool IsNotBlank { get { return !IsBlank; } }

		public bool IsNumericField {
			get { return Type == typeof(int) || Type == typeof(double) || Type == typeof(decimal) || Type == typeof(int?) || Type == typeof(double?) || Type == typeof(decimal?); }
		}
		public bool IsTextField {
			get { return Type == typeof(string); }
		}
		public bool IsDateField {
			get { return Type == typeof(DateTime) || Type == typeof(DateTime?); }
		}
		public bool IsBooleanField {
			get { return Type == typeof(bool) || Type == typeof(bool?); }
		}

		public virtual SqlizedValue Sqlize() {
			object val = ValueObject;
			if (IsTextField && CurrentAutoFixBaseUrl && ValueObject != null) {
				string stringValue = val.ToString();
				val = Fmt.InsertWebsiteBaseUrlPathMarkers(stringValue);
			}
			return Sql.Sqlize(val, Type);
		}

		public string TableName;
		public ActiveRecord Record { get; internal set; }

		/// <summary>
		/// If wasPosted is true this means that the form contained this field, an exception is thrown if no fields posted match the active record
		/// </summary>
		protected bool wasPosted = true;

		private string _friendlyName;

		/// <summary>
		/// Descriptive name for the field (eg 'First Name' instead of 'FirstName').
		/// Used in any messages to the user.
		/// You can override FriendlyName for a field simply by setting it in the model partial InitDefaults method.
		/// </summary>
		public string FriendlyName {
			get {
				if (_friendlyName == null) {
					_friendlyName = Fmt.SplitTitleCase(Name);
				}
				return _friendlyName;
			}
			set { _friendlyName = value; }
		}

		/// <summary>
		/// Get data type definition SQL for use in add or alter column statements. Does not include a semicolon at the end; the caller should add this if required.
		/// </summary>
		/// <returns></returns>
		public string GetSqlDataTypeDeclaration() {
			if (IsAuto && Type == typeof(int)) {
				return "INTEGER IDENTITY(1,1)";
			} else if (Type == typeof(bool) || (Type == typeof(bool?) && !ActiveRecordGenerator.AllowNullableBooleans)) {
				return "BIT NOT NULL CONSTRAINT [DF_" + TableName + "_" + Name + "] DEFAULT (0)";      // MN 20110615 - should not have a semicolon in the sql as this is only a data type and not the end of a statement
			} else if (ColumnType == "decimal") {
				if (DecimalPlaces == null) DecimalPlaces = 2;
				return "DECIMAL(" + (18 - DecimalPlaces) + "," + DecimalPlaces + ")";      // JN 20121023 decimal default is DECIMAL(18,0) - no decimal places, also should not have a semicolon in the sql as this is only a data type and not the end of a statement
			} else {
				if (ColumnType == "date") { ColumnType = "datetime"; }
				string result = ColumnType;
				if (Type == typeof(string) && ColumnType != "ntext" && ColumnType != "text") {
					if (MaxLength == 2147483647) {
						result += "(MAX)";
					} else {
						result += "(" + MaxLength + ")";
					}
				}
				if (!AllowNulls) {
					result += " NOT NULL";      // MN 20110615 - should not have a semicolon in the sql as this is only a data type and not the end of a statement
				}
				return result;
			}
		}

		/// <summary>
		/// Return a string value for general display
		/// Changed to 'override' so it can simply be appended to a string or output
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (ValueObject == null || DBNull.Value.Equals(ValueObject)) {
				return "";
			} else if (IsDateField) {
				return Fmt.DateTime((DateTime)ValueObject);
			}
			return ValueObject.ToString();
		}


#if EnableAutoTypeConversion
		public static implicit operator bool(ActiveFieldBase activeField) {
			if (activeField.IsBooleanField) {
				return activeField.ToBool();
			} else {
				throw new ActiveRecordException("ActiveField type mismatch. " + activeField.TableName + "." + activeField.Name + " is being used as a Boolean but it is a " + activeField.Type.Name + " field.");
			}
		}

		public static implicit operator bool?(ActiveFieldBase activeField) {
			if (activeField.IsNull) {
				return null;
			} else if (activeField.IsBooleanField) {
				return activeField.ToBool();
			} else {
				throw new ActiveRecordException("ActiveField type mismatch. " + activeField.TableName + "." + activeField.Name + " is being used as a Boolean but it is a " + activeField.Type.Name + " field.");
			}
		}

		public static implicit operator decimal?(ActiveFieldBase activeField) {
			if (activeField.IsNull) {
				return null;
			} else if (activeField.IsNumericField) {
				return activeField.ToDecimal();
			} else {
				throw new ActiveRecordException("ActiveField type mismatch. " + activeField.TableName + "." + activeField.Name + " is being used as a Decimal but it is a " + activeField.Type.Name + " field.");
			}
		}

		public static implicit operator double?(ActiveFieldBase activeField) {
			if (activeField.IsNull) {
				return null;
			} else if (activeField.IsNumericField) {
				return activeField.ToDouble();
			} else {
				throw new ActiveRecordException("ActiveField type mismatch. " + activeField.TableName + "." + activeField.Name + " is being used as a Double but it is a " + activeField.Type.Name + " field.");
			}
		}

		public static implicit operator int?(ActiveFieldBase activeField) {
			if (activeField.IsNull) {
				return null;
			} else if (activeField.IsNumericField) {
				return activeField.ToInt();
			} else {
				throw new ActiveRecordException("ActiveField type mismatch. " + activeField.TableName + "." + activeField.Name + " is being used as a Double but it is a " + activeField.Type.Name + " field.");
			}
		}

		public static implicit operator DateTime?(ActiveFieldBase activeField) {
			if (activeField.IsNull) {
				return null;
			} else if (activeField.IsDateField) {
				return (DateTime)activeField.ValueObject;
			} else {
				throw new ActiveRecordException("ActiveField type mismatch. " + activeField.TableName + "." + activeField.Name + " is being used as a Double but it is a " + activeField.Type.Name + " field.");
			}
		}

		public static implicit operator string(ActiveFieldBase activeField) {
			return activeField.ToString();
		}
#endif

		/// <summary>
		/// Return a string value for general display
		/// </summary>
		/// <returns></returns>
		public string OriginalValueToString() {
			if (OriginalValueObject == null || DBNull.Value.Equals(OriginalValueObject)) {
				return "";
			} else if (IsDateField) {
				return Fmt.DateTime((DateTime)OriginalValueObject);
			}
			return OriginalValueObject.ToString();
		}

		/// <summary>
		/// Return a string value for output on an HTML page
		/// </summary>
		/// <returns></returns>
		public string HtmlEncode() {
			return this.ToString().HtmlEncode();
		}

		public bool ToBool() {
			if (IsNull) return false;
			if (IsBooleanField && ValueObject is bool) return (bool)ValueObject;
			return ToString().ConvertToBool();
		}
		public int ToInt(int defaultValue) {
			if (IsNull) return 0;
			if (IsNumericField && ValueObject is int) return (int)ValueObject;
			return ToString().ToInt(defaultValue);
		}
		public int ToInt() {
			if (IsNull) return 0;
			if (IsNumericField && ValueObject is int) return (int)ValueObject;
			return ToString().ToInt(0);
		}
		public int? ToInt(BaseTypeExtensions.Null defaultValue) {
			if (IsNull) return null;
			if (IsNumericField && ValueObject is int) return (int)ValueObject;
			return ToString().ToInt(null);
		}
		public decimal ToDecimal(decimal defaultValue) {
			if (IsNull) return 0;
			if (IsNumericField && ValueObject is decimal) return (decimal)ValueObject;
			return ToString().ToDecimal(defaultValue);
		}
		public decimal ToDecimal() {
			if (IsNull) return 0;
			if (IsNumericField && ValueObject is decimal) return (decimal)ValueObject;
			return ToString().ToDecimal(0);
		}
		public decimal? ToDecimal(BaseTypeExtensions.Null defaultValue) {
			if (IsNull) return null;
			if (IsNumericField && ValueObject is decimal) return (decimal)ValueObject;
			return ToString().ToDecimal(null);
		}
		public double ToDouble(double defaultValue) {
			if (IsNull) return defaultValue;
			if (IsNumericField && ValueObject is double) return (double)ValueObject;
			return ToString().ToDouble(defaultValue);
		}
		public double ToDouble() {
			if (IsNull) return 0;
			if (IsNumericField && ValueObject is double) return (double)ValueObject;
			return ToString().ToDouble(0);
		}
		public double? ToDouble(BaseTypeExtensions.Null defaultValue) {
			if (IsNull) return null;
			if (IsNumericField && ValueObject is double) return (double)ValueObject;
			return ToString().ToDouble(null);
		}
		public DateTime ConvertToDate(DateTime defaultValue) {
			if (IsNull) return defaultValue;
			if (IsDateField && ValueObject is DateTime) return (DateTime)ValueObject;
			return ToString().ConvertToDate(defaultValue);
		}
		public DateTime? ConvertToDate() {
			if (IsNull) return null;
			if (IsDateField && ValueObject is DateTime) return (DateTime)ValueObject;
			return ToString().ConvertToDate(null);
		}
		public DateTime? ConvertToDate(BaseTypeExtensions.Null defaultValue) {
			if (IsNull) return null;
			if (IsDateField && ValueObject is DateTime) return (DateTime)ValueObject;
			return ToString().ConvertToDate(null);
		}

		public virtual bool UpdateFromRequest(string prefix, string suffix) {
			return UpdateFromRequest(prefix, suffix, false);
		}

		public virtual bool UpdateFromRequest(string prefix, string suffix, bool tryNameAlone) {
			//bool wasPosted;
			string postedValue = Web.RequestEx[prefix + this.Name + suffix];

			if (postedValue == null && tryNameAlone) { //not found,...
				//try without the prefixes
				postedValue = Web.RequestEx[this.Name];
			}
			if (IsDateField) {
				string postedTimeValue = Web.RequestEx[prefix + "timefield_" + this.Name + suffix];
				if (postedValue == null && tryNameAlone) {
					postedTimeValue = Web.RequestEx["timefield_" + this.Name];
				}
				if (postedTimeValue != null) postedValue += " " + postedTimeValue; //20120111JN fixed this was converting null to " " when no date or time, clearing the dateadded autofield
			}
			if (postedValue == null) {
				//wasPosted = false; //dont do this as the default is true
				// catch checkbox fields that an unchecked and set them to false - by looking for presence of a specially named hidden field
				if (IsBooleanField && Web.Request[prefix + "checkboxposted_" + this.Name + "" + suffix] == "y") {
					this.ValueObject = false;
					wasPosted = true;
				}
			} else {
				//wasPosted = true;			  //dont do this as the default is true
				FromString(postedValue);
			}
			return wasPosted;
		}

		public virtual void FromString(string stringValue) {
			// this is overridden by different field types
			//this.ValueObject = stringValue.Trim();
			ValueObject = Conv.FromString(stringValue, this.Type);
		}

		public void FromDatabase(object dbValue) {
			//MK 20101130 - when we are loading data from db, original value is always database val, not init defaults value. This makes sure isDirty works properly
			if (dbValue is DBNull) {
				dbValue = null;
			}
			if (CurrentAutoFixBaseUrl && IsTextField && dbValue != null) {
				dbValue = dbValue.ToString().Replace("~WebRoot/", Web.BaseUrl);
			}
			ValueObject = dbValue;
			OriginalValueObject = dbValue;
			SavedValueObject = dbValue;
		}

		/// <summary>
		/// prepare each field for save
		/// </summary>
		/// <param name="record"></param>
		public virtual void PrepareForSave(ActiveRecord record) {
			if (IsDateField && Name == "DateModified") {
				ValueObject = DateTime.Now;
			} else if (IsDateField && Name == "DateAdded" && record.IsNewRecord) {
				ValueObject = DateTime.Now;
			}
		}

		public virtual void PrepareForDelete(bool deleteAttachedFiles) {
		}

		/// <summary>
		/// Add a field to a string
		/// </summary>
		/// <param name="likeThis"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string operator +(ActiveFieldBase likeThis, string str) {
			return likeThis.ToString() + str;
		}
		public static string operator +(string str, ActiveFieldBase likeThis) {
			return str + likeThis.ToString();
		}

		/// <summary>
		/// If this field is a foreign key, this method will return the record it is linked to.
		/// </summary>
		public Func<ActiveRecord> GetForeignRecord;
		public Type ForeignClassName;
		public string ForeignTableName;
		public string ForeignTableFieldName;
		public bool IsForeignKey {
			get { return ForeignTableName + "" != ""; }
		}

		/// <summary>
		/// Return a display version of the value.
		/// String = as is
		/// Bool = yes/no
		/// Date = formatted
		/// Foreign key = lookup field and get name using GetName()
		/// </summary>
		/// <returns></returns>
		public string ToStringNice() {
			string displayText = "";
			if (IsForeignKey) {
				var lookup = GetForeignRecord();
				if (lookup != null) {
					displayText = lookup.GetName();
				}
			} else if (IsBooleanField) {
				displayText = Fmt.YesNo(ValueObject.ToBool());
			} else if (IsNumericField) {
				displayText = Fmt.Number(ValueObject, DecimalPlaces ?? -1, true);
			} else {
				displayText = ToString();
			}
			return displayText;
		}

		public string OriginalValueToStringNice() {
			string displayText = "";
			if (IsForeignKey) {
				displayText = null;
			} else if (IsBooleanField) {
				displayText = Fmt.YesNo(OriginalValueObject.ToBool());
			} else if (IsNumericField) {
				displayText = Fmt.Number(OriginalValueObject, DecimalPlaces??-1, true);
			} else {
				displayText = OriginalValueToString();
			}
			return displayText;
		}

		public string JsonStringify() {
			return ValueObject.JsonStringify();
		}

	}

	//todo: enable this code - need to alter template to descend from this class
	//public class ActiveRecordFieldReferences {
	//  public ActiveFieldBase this[string fieldName] {
	//    get { return fields[fieldName]; }
	//    set { fields[fieldName] = value; }
	//  }
	//}


	///// <summary>
	///// Not Used Yet - PROTOTYPE ONLY
	///// </summary>
	//  public class ActiveField2 {

	//    public class BaseRoot {}

	//    public class Base<T>: BaseRoot {
	//      public string Name;
	//      public T Value;
	//      public T OriginalValue;

	//      public bool AllowNulls = true;
	//      public Type Type;
	//      public string ColumnType;
	//      public int MaxLength;
	//      public bool IsAuto = false;

	//      public bool IsDirty { get { return Equals(this.Value, this.OriginalValue); } }
	//      public SqlizedValue Sqlize() {
	//        return Sql.Sqlize(this.Value);
	//      }

	//      public virtual string ToHtml() {
	//        return Value.ToString();
	//      }
	//    }

	//    public class String : Base<string> {
	//      public override string ToHtml() {
	//        return Value.HtmlEncode();
	//      }
	//    }

	//    public class Html:Base<string> {
	//      public override string ToHtml() {
	//        return Value;
	//      }
	//    }

	//    public class Int:Base<int> {}

	//    public class Currency:Base<decimal> {
	//      public override string ToHtml() {
	//        return ToString();
	//      }		
	//      public string ToHtml(int decimalPlaces, string currencySymbol) {
	//        return Value.ToString("");
	//      }		
	//      public override string ToString() {
	//        return Fmt.Currency(Value+"");
	//      }							
	//    }

	//    public class Date:Base<System.DateTime> {
	//      public override string ToHtml() {
	//        return ToString();
	//      }
	//      public override string ToString() {
	//        return Fmt.Date(Value);
	//      }
	//    }

	//    public class Time:Base<System.DateTime> {
	//      public override string ToHtml() {
	//        return ToString();
	//      }
	//      public override string ToString() {
	//        return Fmt.Time(Value);
	//      }
	//    }

	//    public class DateTime:Base<System.DateTime> {
	//      public override string ToHtml() {
	//        return ToString();
	//      }
	//      public override string ToString() {
	//        return Fmt.DateTime(Value);
	//      }
	//    }

	//}
}
