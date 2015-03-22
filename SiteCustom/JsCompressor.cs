using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Site.SiteCustom {
	public class JsCompressor {

		/// <summary>
		/// remove C-style comments and multi-line comments.
		/// </summary>
		private bool removeComments = true;
		
		/// <summary>
		/// trim lines and remove multiple blank lines.
		/// </summary>
		private bool removeAndTrimBlankLines = true;
		
		/// <summary>
		/// remove all CRLF characters.
		/// </summary>
		private bool removeCarriageReturns = true;
		
		/// <summary>
		/// skim the rest of the code.
		/// </summary>
		private bool removeEverthingElse = true;
		
		/// <summary>
		/// Matches /* c-style comments. 
		/// */
		/// </summary>
		private Regex regCStyleComment;

		/// <summary>
		/// Matches //line comments.
		/// </summary>
		private Regex regLineComment;

		/// <summary>
		/// Matches any white space including CRLF at the end of line.
		/// </summary>
		private Regex regSpaceLeft;
		
		/// <summary>
		/// Matches any whitespace at the beginning of the line.
		/// </summary>
		private Regex regSpaceRight;

		/// <summary>
		/// Matches any space-tab combination.
		/// </summary>
		private Regex regWhiteSpaceExceptCRLF;

		/// <summary>
		/// Quotes and regular expressions.
		/// </summary>
		private Regex regSpecialElement;

		/// <summary>
		/// Matches opening curly brace "{".
		/// </summary>
		private Regex regLeftCurlyBrace;

		/// <summary>
		/// Matches closing curly brace "}".
		/// </summary>
		private Regex regRightCurlyBrace;
	
		/// <summary>
		/// Matches a comma surrounded by whitespace characters.
		/// </summary>
		private Regex regComma;	

		/// <summary>
		/// Matches a semi-column surrounded by whitespace characters.
		/// </summary>
		private Regex regSemiColumn;

		/// <summary>
		/// Matches CRLF characters.
		/// </summary>
		private Regex regNewLine;

		private Regex regCarriageAfterKeyword;
		
		/// <summary>
		/// Hashtable to store the captured special elements.
		/// </summary>
		private Hashtable htCaptureFields;

		/// <summary>
		/// Hashtable to store pre-compiled regular expressions for special elements.
		/// </summary>
		private Hashtable htRegSpecialElement;
	
		/// <summary>
		/// The total number of special elements captured.
		/// </summary>
		private int specialItemCount;

		/// <summary>
		/// If <code>true</code> comments will be removed.
		/// Default value is <code>true</code>.
		/// </summary>
		public bool RemoveComments {
			set {
				removeComments = value;
			}
		}

		/// <summary>
		/// If <code>true</code> lines will be trimmed and multiple blank
		/// new lines will be removed.
		/// Default value is <code>true</code>.
		/// </summary>
		public bool TrimLines {
			set {
				removeAndTrimBlankLines = value;
			}
		}

		/// <summary>
		/// If <code>true</code> all CRLF characters will be removed.
		/// Default value is <code>true</code>.
		/// </summary>
		public bool RemoveCRLF {
			set {
				removeCarriageReturns = value;
			}
		}

		/// <summary>
		/// If <code>true</code> some additional compression will be done.
		/// Default value is <code>true</code>.
		/// </summary>
		public bool RemoveEverthingElse {
			set {
				removeEverthingElse = value;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public JsCompressor() {
			/* initialize members */

			regCStyleComment = new Regex("/\\*.*?\\*/",RegexOptions.Compiled|RegexOptions.Singleline);
			regLineComment = new Regex("//.*\r\n",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regSpaceLeft = new Regex("^\\s*",RegexOptions.Compiled|RegexOptions.Multiline);
			regSpaceRight = new Regex("\\s*\\r\\n",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regWhiteSpaceExceptCRLF = new Regex("[ \\t]+",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regSpecialElement = new Regex(
				"\"[^\"\\r\\n]*\"|'[^'\\r\\n]*'|/[^/\\*](?<![/\\S]/.)([^/\\\\\\r\\n]|\\\\.)*/(?=[ig]{0,2}[^\\S])",
				RegexOptions.Compiled|RegexOptions.Multiline);
			regLeftCurlyBrace = new Regex("\\s*{\\s*",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regRightCurlyBrace = new Regex("\\s*}\\s*",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regComma = new Regex("\\s*,\\s*",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regSemiColumn = new Regex("\\s*;\\s*",RegexOptions.Compiled|RegexOptions.ECMAScript);
			regNewLine = new Regex("\\r\\n",RegexOptions.Compiled|RegexOptions.ECMAScript);

			regCarriageAfterKeyword = new Regex(
				"\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)",
				RegexOptions.Compiled|RegexOptions.ECMAScript);

			htCaptureFields = new Hashtable();
			htRegSpecialElement = new Hashtable();
			
			specialItemCount = 0;
		}

		/// <summary>
		/// Compresses the given String.
		/// </summary>
		/// <param name="toBeCompressed">The String to be compressed.</param>
		public string Compress(String toBeCompressed) {
			/*clean the hasthable*/
			htCaptureFields.Clear();
			htRegSpecialElement.Clear();
			specialItemCount = 0;


			
			/* mark special elements */
			MarkQuotesAndRegExps(ref toBeCompressed);
			
			if(removeComments) {
				/* remove line comments */
				RemoveLineComments(ref toBeCompressed);
				/* remove C Style comments */
				RemoveCStyleComments(ref toBeCompressed);
			}
			
			if(removeAndTrimBlankLines) {
				/* trim left */
				TrimLinesLeft(ref toBeCompressed);
				/* trim right */
				TrimLinesRight(ref toBeCompressed);
			}

			if(removeEverthingElse) {
				/* { */
				ReplaceLeftCurlyBrace(ref toBeCompressed);
				/* } */
				ReplaceRightCurlyBrace(ref toBeCompressed);
				/* , */
				ReplaceComma(ref toBeCompressed);
				/* ; */
				ReplaceSemiColumn(ref toBeCompressed);		
			}
			
			if(removeCarriageReturns) {			
				/* 
				 * else[CRLF]
				 * return
				 */
				ReplaceCarriageAfterKeyword(ref toBeCompressed);
				/* clear all CRLF's */
				ReplaceNewLine(ref toBeCompressed);
			}

			/* restore the formerly stored elements. */
			RestoreQuotesAndRegExps(ref toBeCompressed);
		
			return toBeCompressed;
		}

		/// <summary>
		/// Replaces the stored special elements back to their places.
		/// </summary>
		/// <param name="input">The input String to process.</param>
		private void RestoreQuotesAndRegExps(ref String input) {
			int captureCount = htCaptureFields.Count;
			for(int i=0;i<captureCount;i++) {
				input = ((Regex) htRegSpecialElement[i]).Replace(input,(String)htCaptureFields[i]);
			}
		}

		/// <summary>
		/// Quotes and regular expressions should be untouched and unprocessed at all times.
		/// So we mark and store them beforehand in a private Hashtable for later use.
		/// </summary>
		/// <param name="input">The input String to process. It should be a single line.</param>
		private void MarkQuotesAndRegExps(ref String input) {
			MatchCollection matches = regSpecialElement.Matches(input);

			int count=matches.Count;
			Match currentMatch;
			
			/* store strings and regular expressions */
			for(int i=0;i<count;i++) {
				currentMatch = matches[i];
				htCaptureFields.Add(specialItemCount,currentMatch.Value);
				/* we added one more special item to our Hashtable */
				specialItemCount++;
			}

			/* replace strings and regular expressions */
			for(int i=0;i<count;i++) {
				/* 
				 * compile and add the Regex to the hashtable
				 * so that it executes faster at the Restore phase.
				 *
				 * A trade off between Regex compilation speed and 
				 * memory. 
				 */
				htRegSpecialElement.Add (i,new Regex("____SPECIAL_ELEMENT____"+(i)+"____",
					RegexOptions.ECMAScript|RegexOptions.Compiled));
				
				input = regSpecialElement.Replace(input,"____SPECIAL_ELEMENT____"+(i)+"____",1);
			}
		}

		/// <summary>
		/// Removes any multi-line single line /* c style comments */
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void RemoveCStyleComments(ref String input) {
			input = regCStyleComment.Replace(input,"");
		}

		/// <summary>
		/// Removes all \\line comments.
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void RemoveLineComments(ref String input) {
			input = regLineComment.Replace(input,"");
		}
		
		/// <summary>
		/// Replaces any duplicate space-tab combinations with a single space.
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void ReplaceDuplicateWhiteSpace(ref String input) {
			input = regWhiteSpaceExceptCRLF.Replace(input," ");
		}

		/// <summary>
		/// Trims all the trailing whitespace characters in a line with "".
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void TrimLinesLeft(ref String input) {
			input = regSpaceLeft.Replace(input,"");
		}

		/// <summary>
		/// Trims all whitespace after the end of the line, and the proceeding CRLF characters
		/// with a single CRLF.
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void TrimLinesRight(ref String input) {
			input = regSpaceRight.Replace(input,"\r\n");
		}

		/// <summary>
		/// Replaces any whitespace before and after "{" characters with "".
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void ReplaceLeftCurlyBrace(ref String input) {
			input = regLeftCurlyBrace.Replace(input,"{");
		}

		/// <summary>
		/// Replaces any whitespace before and after "}" characters with "".
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void ReplaceRightCurlyBrace(ref String input) {
			input = regRightCurlyBrace.Replace(input,"}");
		}

		private void ReplaceCarriageAfterKeyword(ref String input) {
			input = regCarriageAfterKeyword.Replace(input," ");
		}
		
		/// <summary>
		/// Replaces any whitespace before and after "," characters with "".
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void ReplaceComma(ref String input) {
			input = regComma.Replace(input,",");
		}

		/// <summary>
		/// Replaces any whitespace before and after ";" characters with "".
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void ReplaceSemiColumn(ref String input) {
			input = regSemiColumn.Replace(input,";");
		}

		/// <summary>
		/// Replaces all CRLF characters in the input to "".
		/// </summary>
		/// <param name="input">The input String to replace.</param>
		private void ReplaceNewLine(ref String input) {
			input = regNewLine.Replace(input,"");
		}

	}
}