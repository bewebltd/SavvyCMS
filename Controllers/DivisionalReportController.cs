using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class DivisionalReportController : ApplicationController {

		public ActionResult Index(string selectedMonth, string showBreakdown) {
			var data = new ViewModel();
			var month = DateTime.Today.GetPreviousMonthBegin();
			if (selectedMonth != null) {
				month = DateTime.Parse(selectedMonth);
			}
			data.Month = month;
			data.ShowBreakdown = showBreakdown ?? "Summary";

			data.RunReport(month);

			return View("DivisionalReportView", data);
		}


		public class ReportLine {
			public string Format = "norm";
			public int?[] Values = new int?[4];
			public int Level;
			public ReportLine Parent;  // optional parent
			public string Title { get; set; }
			public List<ReportLine> SubLines = new List<ReportLine>();
			public string SortKey {
				get {
					var str = "";
					str += (Values[1] + "").PadLeft(10, '0');
					str += (Values[0] + "").PadLeft(10, '0');
					str += (Values[3] + "").PadLeft(10, '0');
					str += (Values[2] + "").PadLeft(10, '0');
					return str;
				}
			}

			public string FmtValue(int colIndex) {
				var perValue = Values[colIndex];
				return perValue == null || perValue == 0 ? "-" : perValue.Value.FmtNumber();
			}
		}

		public class PeriodReport {
			public PeriodReport(string title, DateTime startDate, DateTime endDate, ViewModel report, int colNum) {
				Title = title;
				StartDate = startDate;
				EndDate = endDate;
				AfterEndDate = endDate.AddDays(1);
				Report = report;
				ColNum = colNum;
			}

			public void Run(ReportLine root) {
				ReportLine h2, h3, norm;
				Sql sql, where;

				var branches = new[] { "AF", "MD", "AK", "PA", "NH", "SF" }.SqlizeTextList();

				h2 = AddMainHeader(root, "Auckland Cartage");
				
				h3 = AddHeader(h2, "FCL - Pengelly's Trucks");

				// FCL -pnegellys
				var modes = new[] { "FCL", "Empty" }.SqlizeTextList();
				where = new Sql("DeliveryAssignment_view WHERE FreightMode = ", "FCL".SqlizeText(), " and fromaddress_despatchzoneid=15 AND Carrier IS NULL AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Pengellys 20' FCL", where, "Num20FtContainers", "FreightMode");
				AddBreakdown(h3, "Pengellys 40' FCL", where, "Num40FtContainers", "FreightMode");

				where = new Sql("despatchjob dj inner join DespatchTruck t on t.despatchtruckid=dj.despatchtruckid WHERE FreightMode in (", modes, ") and fromaddress_despatchzoneid=15 AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Pengellys FCL by Truck", new Sql(where).AddRawSqlString("and FreightMode ='FCL'"), "COUNT", "TruckName");
				AddBreakdown(h3, "Pengellys Empties by Truck", new Sql(where).AddRawSqlString("and FreightMode ='Empty'"), "COUNT", "TruckName");

				// fcl by carrier breakdown 
				h3 = AddHeader(h2, "FCL - Subcontracted or Carrier Assigned");
				where = new Sql("DeliveryAssignment_view WHERE FreightMode = ", "FCL".SqlizeText(), " and fromaddress_despatchzoneid=15 AND Carrier IS NOT NULL AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Contractor 20' FCL", where, "Num20FtContainers", "CarrierName");
				AddBreakdown(h3, "Contractor 40' FCL", where, "Num40FtContainers", "CarrierName");

				// LCL
				h3 = AddHeader(h2, "LCL, Air, Cartage - Pengelly's Trucks");

				modes = new[] { "Air", "LCL", "Cartage" }.SqlizeTextList();
				where = new Sql("DeliveryAssignment_view WHERE FreightMode in (", modes, ") and fromaddress_despatchzoneid=15 AND Carrier IS NULL AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Pengellys Deliveries", where, "NumDeliveries", null);
				AddBreakdown(h3, "Pengellys Kgs", where, "Kilos", null);
				where = new Sql("despatchjob dj inner join despatchrun r on r.despatchrunid=dj.despatchrunid inner join DespatchTruck t on t.despatchtruckid=r.despatchtruckid WHERE FreightMode in (", modes, ") and fromaddress_despatchzoneid=15 AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Pengellys Deliveries by Truck", where, "COUNT", "TruckName");
				//AddBreakdown(h3, "Pengellys Cubic", where, "Cubic", null);

				h3 = AddHeader(h2, "LCL, Air, Cartage - Subcontracted or Carrier Assigned");

				// lcl by carrier breakdown 
				where = new Sql("DeliveryAssignment_view WHERE FreightMode in (", modes, ") and fromaddress_despatchzoneid=15 AND Carrier IS not NULL AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Deliveries by Contractor", where, "NumDeliveries", "CarrierName");
				AddBreakdown(h3, "Kilos by Contractor", where, "Kilos", "CarrierName");
				//AddBreakdown(h3, "Cubic by Contractor", where, "Cubic", "CarrierName");

				// cartage zones
				where = new Sql("DeliveryAssignment_view d inner join DespatchZone z on z.DespatchZoneID=d.ToAddress_DespatchZoneID WHERE FreightMode in (", modes, ") and fromaddress_despatchzoneid=15 AND Carrier IS NOT NULL AND DateDelivered >= ", StartDate, "and DateDelivered < ", AfterEndDate);
				AddBreakdown(h3, "Contractor Deliveries from Auckland to...", where, "NumDeliveries", "ZoneName");
				//CartageBreakdown(h3, "Contractor Kilos", where, "Kilos", "toaddress_despatchzoneid");
				//CartageBreakdown(h3, "Contractor Cubic", where, "Cubic", "toaddress_despatchzoneid");

				// logistrics
				h2 = AddMainHeader(root, "Auckland Logistics");
				h3 = AddHeader(h2, "Revenue");

				//select sum(amount) from logisticsinvoice where InvoiceDate > and 

				var logisticsStockCodes = new[] { "STM", "STN", "STX", "STA", "HLM", "HLN", "HLX", "HLA" }.SqlizeTextList(); ;
				where = new Sql("peng.dbo.DR_invlines line inner join peng.dbo.DR_TRANS inv on inv.SEQNO=line.HDR_SEQNO where line.stockcode in (", logisticsStockCodes, ") and inv.transdate >= ", StartDate, "and inv.transdate < ", AfterEndDate);
				//where = new Sql("logistics.dbo.logisticsinvoice where branch in (",branches,") and InvoiceDate >= ", StartDate, "and InvoiceDate < ", AfterEndDate);
				AddBreakdown(h3, "By Code ($)", where, "linetotal", "left(stockcode,2)");
				AddBreakdown(h3, "By Branch ($)", where, "linetotal", "right(stockcode,1)");

				//AddBreakdown(h3, "Storage ($)", where, "Amount1", "Branch");
				//AddBreakdown(h3, "Handling ($)", where, "Amount2", "Branch");

				h3 = AddHeader(h2, "Orders Processed");
				where = new Sql("logistics.dbo.TransBatch b inner join logistics.dbo.Warehouse w on b.WarehouseID=w.warehouseid where billingbranch in (", branches, ") and IsProcessed=1 and BatchDate >= ", StartDate, "and BatchDate < ", AfterEndDate);
				AddBreakdown(h3, "Inwards (#)", new Sql().AddSql(where).Add("and transtype=1"), "COUNT", "w.ShortName");
				AddBreakdown(h3, "Outwards (#)", new Sql().AddSql(where).Add("and transtype=0"), "COUNT", "w.ShortName");

				// customs
				var customsChargeCode = "AD,AT,CU,CM,CS,EX,FC,OD,PE,PF,RF,SE,TF".SqlizeTextList();
				h2 = AddMainHeader(root, "Auckland Customs");
				h3 = AddHeader(h2, "Jobs");
				where = new Sql("trackpeng.dbo.job job inner join trackpeng.dbo.staff staff on job.staff=staff.initials inner join trackpeng.dbo.JobAccountsSummary s on s.job=job.number where job.type=", "I".SqlizeText(), " and staff.user_name in (", branches, ") and left(code,2) in (", customsChargeCode, ")").Add("and ETA >= ", StartDate, "and ETA <= ", EndDate);
				AddBreakdown(h3, "# Entries", where, "COUNT(distinct Number)", "staff.name");
				AddBreakdown(h3, "Invoiced ($)", where, "Sales", "Staff.Name");

				where = new Sql("trackpeng.dbo.job job inner join trackpeng.dbo.staff staff on job.staff=staff.initials where job.type=", "I".SqlizeText(), " and staff.user_name in (", branches, ")").Add("and ETA >= ", StartDate, "and ETA <= ", EndDate);
				AddBreakdown(h3, "# Lines", where, "Total_Lines", "Staff.Name");

				//h3 = AddHeader(h2, "Revenue");
				//and POD like 'NZ%'
				//where = new Sql("trackpeng.dbo.job j inner join trackpeng.dbo.JobAccountsSummary s on s.job=j.number where FreightJob=0 and branch in (",branches,") and left(code,2) in (",customsChargeCode,") and transdate >= ", StartDate, "and transdate < ", AfterEndDate);

				// freight
				h2 = AddMainHeader(root, "Auckland Freight");
				h3 = AddHeader(h2, "Imports");
				//where = new Sql("trackpeng.dbo.job where branch in (", branches, ") and ETA >= ", StartDate, "and ETA < ", AfterEndDate);
				//where.AddRawSqlString("and freightjob=0 and mode='A' and POD like 'NZ%'");
				//where = new Sql("trackpeng.dbo.Report_ImpExpSummary_View where branch in (", branches, ") and ETA >= ", StartDate, "and ETA < ", AfterEndDate);
				where = new Sql("trackpeng.dbo.JOB where ETA >= ", StartDate, "and ETA < ", AfterEndDate).AddRawSqlString("and (Freight_Provider='PENINT' or Freight_Provider='PENAIR') and TYPe='I'");
				//where.AddRawSqlString("and freightjob=0 and mode='A' and POD like 'NZ%'");
				AddBreakdown(h3, "# Import Jobs", where, "COUNT", "Mode");
				AddBreakdown(h3, "Import Air Kilos", new Sql(where).AddRawSqlString("and mode='A'"), "Total_Airfreight", null);
				AddBreakdown(h3, "Import LCL Cubic", new Sql(where).AddRawSqlString("and mode='S' and total_LCL is not null and total_LCL <> 0"), "Pengellys_CubicMetres", null);
				AddBreakdown(h3, "Import 20' Containers", new Sql(where).AddRawSqlString("and mode='S'"), "total_20F", null);
				AddBreakdown(h3, "Import 40' Containers", new Sql(where).AddRawSqlString("and mode='S'"), "total_40F", null);

				h3 = AddHeader(h2, "Exports");
				where = new Sql("trackpeng.dbo.JOB where ETD >= ", StartDate, "and ETD < ", AfterEndDate).AddRawSqlString("and (POL='NZAKL' or POL='NZTRG') and type='E'");
				//where.AddRawSqlString("and freightjob=0 and mode='A' and POD like 'NZ%'");
				AddBreakdown(h3, "# Export Jobs", where, "COUNT", "Mode");
				AddBreakdown(h3, "Export Air Kilos", new Sql(where).AddRawSqlString("and mode='A'"), "Total_Airfreight", null);
				AddBreakdown(h3, "Export LCL Cubic", new Sql(where).AddRawSqlString("and mode='S' and total_LCL is not null and total_LCL <> 0"), "Pengellys_CubicMetres", null);
				AddBreakdown(h3, "Export 20' Containers", new Sql(where).AddRawSqlString("and mode='S'"), "total_20F", null);
				AddBreakdown(h3, "Export 40' Containers", new Sql(where).AddRawSqlString("and mode='S'"), "total_40F", null);

				h3 = AddHeader(h2, "Revenue");
				var freightChargeCode = "FI,FO,ID,IT,PS,IO,EI,AD,FF,FE,EO,SK,AS,BA".SqlizeTextList();
				where = new Sql("trackpeng.dbo.job j inner join trackpeng.dbo.JobAccountsSummary s on s.job=j.number where branch in (", branches, ") and left(code,2) in (", freightChargeCode, ") and transdate >= ", StartDate, "and transdate < ", AfterEndDate);
				AddBreakdown(h3, "Invoiced ($)", where, "Sales", "isnull(Pengellys_Direction,'Other')+' '+Mode");

			}



			private void AddBreakdown(ReportLine h3, string title, Sql where, string fieldName, string breakdownBy) {
				string fieldAggregate = "sum(" + fieldName + ")";
				if (fieldName == "COUNT") {
					fieldAggregate = "count(*)";
				} else if (fieldName.StartsWith("COUNT")) {
					fieldAggregate = fieldName;
				}
				Sql sql = new Sql("select " + fieldAggregate + " as Amt from ").AddSql(where);
				var norm = AddLine(h3, title, sql.FetchNumber().ToInt());
				if (breakdownBy != null) {
					sql = new Sql().AddRawSqlString("select " + breakdownBy + " as Title, " + fieldAggregate + " as Amt from ").AddSql(where);
					if (fieldName.DoesntContain("COUNT")) {
						sql.Add("and " + fieldName + " is not null and " + fieldName + " <> 0");
					}
					sql.AddRawSqlString("group by " + breakdownBy + " order by " + fieldAggregate + " desc");
					AddSubLines(norm, "By Contractor", sql);
				}
			}

			private ReportLine AddMainHeader(ReportLine parent, string title) {
				return Report.AddLine(title, null, "h2", this.ColNum, 1, parent);
			}

			private ReportLine AddHeader(ReportLine parent, string title) {
				return Report.AddLine(title, null, "h3", this.ColNum, 2, parent);
			}

			private ReportLine AddLine(ReportLine parent, string title, int? value) {
				return Report.AddLine(title, value, "norm", this.ColNum, 3, parent);
			}

			private ReportLine AddSubLine(ReportLine parent, string title, int? value) {
				return Report.AddLine(title, value, "sub", this.ColNum, 4, parent);
			}

			private void AddSubLines(ReportLine parent, string subtitle, Sql sql) {
				//foreach (var perdi in periods) {
				var recs = sql.GetActiveRecordList();
				foreach (var rec in recs) {
					AddSubLine(parent, rec[0].ToString(), rec[1].ToInt(null));
				}

				//}
			}


			public string Title;
			public DateTime StartDate;
			public DateTime EndDate;
			public DateTime AfterEndDate;
			private ViewModel Report;

			public int ColNum = 1;

			//public string GetLineValue(ReportLine line) {
			//  var foundLine = Report.Lines.Find(l=>l.Title==line.Title && l.Period==this);
			//  if (foundLine != null) {
			//    return foundLine.Value.ToString();
			//  }
			//  return "-";
			//}
		}
		public class ViewModel : PageTemplateViewModel {
			public List<PeriodReport> Periods = new List<PeriodReport>();
			public ReportLine RootLine = new ReportLine();
			private int colCount = 0;
			public DateTime? Month;
			public string ShowBreakdown;

			//public List<ReportLine> Lines {
			//  get {
			//    if (lines == null) {
			//      lines = new List<ReportLine>();
			//      foreach (var report in Periods) {
			//        foreach (var reportLine in report.Lines) {
			//          if (!lines.Exists(l => l.Title == reportLine.Title)) {
			//            lines.Add(reportLine);
			//          }
			//        }
			//      }
			//    }
			//    return lines;
			//  }
			//}

			public void AddPeriods(DateTime month) {
				Periods.Add(new PeriodReport(month.GetMonthName().Left(3) + " " + month.Year, month, month.GetMonthEnd(), this, colCount++));
				var fyBegin = Dates.GetFinancialYearBegin(month, "Apr");
				Periods.Add(new PeriodReport("YTD", fyBegin, month.GetMonthEnd(), this, colCount++));
			}

			public void RunReport(DateTime month) {
				AddPeriods(month);
				AddPeriods(month.AddYears(-1));

				foreach (var periodReport in Periods) {
					periodReport.Run(this.RootLine);
				}


				foreach (var h1 in RootLine.SubLines) {
					foreach (var h2 in h1.SubLines) {
						foreach (var h3 in h2.SubLines) {
							foreach (var h4 in h3.SubLines) {
								// this level needs sorting
								h3.SubLines.Sort((a, b) => b.SortKey.CompareTo(a.SortKey));
							}
						}
					}
				}
			}

			public ReportLine AddLine(string title, int? value, string format, int colIndex, int level, ReportLine parent) {
				//var line = Lines.Find(l=>l.Title==title && l.Format==format && l.Level==level && l.Parent==parent);
				if (level == 4 && ShowBreakdown != "Detail") {
					return null;
				}

				var line = parent.SubLines.Find(l => l.Title == title && l.Format == format);
				if (line == null) {
					line = new ReportLine();
					parent.SubLines.Add(line);
				}
				line.Parent = parent;
				line.Title = title;
				line.Format = format;
				line.Level = level;
				line.Values[colIndex] = value;

				return line;
			}

		}

	}
}