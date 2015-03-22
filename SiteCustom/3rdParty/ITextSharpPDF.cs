using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.rtf.graphic;
namespace PDF
{
	public class PDFReports
	{
		public PDFReports()
		{
		
		}
	
		public string currencyFormat(string amount, Boolean withDollarSign)
		{
			Decimal amountDec = Convert.ToDecimal(amount);
			amountDec = Math.Round(amountDec, 2);

			amount = amountDec.ToString("###,###,###,###.##");
			//Place appropriate number of 0s to the number when there is no decimal place.
			if (!amount.Contains("."))
			{
				amount += ".00";
			}
			//Place one 0 if there is only one decimal place.
			if (amount.Length - amount.LastIndexOf(".") == 2)
			{
				amount += "0";
			}
			if (Convert.ToDecimal(amount) < 1)
			{
				amount = "0" + amount;
			}
			if (withDollarSign)
			{
				return "$" + amount;
			}


			else { return amount; }
		}
	}

}