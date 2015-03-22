<%@ Page Title="Events" Language="C#" MasterPageFile="~/site.master" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.EventsController.ViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
		    $("#events_list li.dimmed:last").addClass("dimmedborder");
		    $('ul#events_list li .view').css("cursor", "pointer")
		    $('ul#events_list li .month-icon').css("cursor", "pointer")
		    $('ul#events_list li h3').css("cursor", "pointer")
		    $('ul#events_list li .detail').hide();
		    $('ul#events_list li .overview').toggle(
			    function () {
			        $(this).parent().children('.detail').slideDown(500);
			        $(this).children('.view').html("Close")
			    }, function () {
			        $(this).parent().children('.detail').slideUp(500);
			        $(this).children('.view').html("View Item")
			    }
		    );
		    // close button in sliding div
		    $('ul#events_list li .detail .close_detail').click(
		        function () {
		            $(this).parent().parent().find(".overview").click();
		        }
	        );		
		});
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">

<%--
dim sitecode, imageBaseUrl, showEventFilter, eventCategoryID
sitecode = request("sc")&""
if (sitecode="") then response.End()
showEventFilter = FetchValue("Select ShowEventFilter from SiteEventCalender where SiteCode = '"& FmtSqlString(sitecode) &"'")
eventCategoryID = request("EventCategoryID")&""
imageBaseUrl =  "http://honda-motorcycles.co.nz/" 'Replace(websiteBaseUrl,"www","legacy") & "/"
if (request("dev")&"" = "1" ) then
--%>
	<style>
	
	#newsevents_events_wrapper{
		width:285px;
		float:left;	
		margin:0;
		/*position:relative;*/
	}
	
	#newsevents_events_wrapper p{
		color:#293752;
		line-height:140%;
		font-family:Arial, Helvetica, sans-serif;
		font-size:12px;
		font-style:normal;
		font-variant:normal;
		font-weight:normal;
		}
	
	#newsevents_events_wrapper h2{
		color:#000;
		line-height:normal;
		padding-bottom: 2px;
		font-family:Arial, Helvetica, sans-serif;
		font-size:18px;
		font-style:normal;
		font-variant:normal;
		font-weight:bold;
		margin-bottom:7px;
		border-bottom: 3px solid #d9d9d9;
		}
	#newsevents_events_wrapper h3{
		color: #262626;
		line-height:normal;
		font-family:Arial, Helvetica, sans-serif;
		font-size:12px;
		font-style:normal;
		font-variant:normal;
		font-weight:bold;
		text-decoration:none;
		margin-bottom:0px;
		}
	#newsevents_events_wrapper .date{
		color: #676767;
		line-height:normal;
		font-family:Arial, Helvetica, sans-serif;
		font-size:10px;
		font-style:italic;
		font-variant:normal;
		font-weight:normal;
		text-decoration: none !important;
		margin-bottom:0px;
		margin-top:0px;
		}
	#newsevents_events_wrapper .view{
		color:#a00002;
		line-height:normal;
		padding-left:9px;
		font-family:Arial, Helvetica, sans-serif;
		font-size:11px;
		font-style:normal;
		font-variant:normal;
		font-weight:bold;
		text-decoration:none;
		margin-left:3px;
		background-image:url(<%=Web.Root%>images/grey_arrow.gif);
		background-attachment:scroll;
		background-repeat:no-repeat;
		background-position: 0px 4px;
		background-color:transparent;
		}
		
	#newsevents_events_wrapper .box_outer_border{
		/*position:relative;*/
		background-color: #fff;
		border-left: 1px solid #ececec;
		border-right: 1px solid #ececec;
		width:155px;
		margin-left:5px;
	}
	#newsevents_events_wrapper .box_inner_border{
		padding: 7px;
		padding-top: 10px;
		background-color: #fff;
		/*
		border-top: 1px solid #ececec;
		border-bottom: 1px solid #ececec;
		border-left: 1px solid #dfdfdf;
		border-right: 1px solid #dfdfdf;
		*/
		min-height: 135px;
	}
	#newsevents_events_wrapper .margin3{
		margin-top:0px;
		margin-bottom:7px;
		}
	#events_list h3, #events_list h3 a{
		margin-bottom:0;
		color:#262626;
		font-size:12px;
		text-decoration:none;	
	}
	#events_list h3:hover, #events_list h3 a:hover{
		text-decoration:underline;		
	}
	#events_list{   
		margin-top: -10px;
		margin-left:0px;
		list-style-type:none;
		list-style-image:none;
	}
	#events_list li{
		border-bottom:1px dotted #d0d0d0;
		padding:10px 0 11px 0;		
	}
	#events_list li .date{
		font-size:10px;
		color:#676767;
		margin-bottom:0;
	}
	#events_list li.first{
		padding-top:0;
	}
	#events_list li.last{
		border-bottom:0;	
	}
	#events_list .dimmedborder{
		border-bottom:3px solid #9D102C;
	}
	
	#events_list li.dimmed h3, #events_list li.dimmed .date, #events_list li.dimmed a.view{
		color:#8F8F8F;
	}
	#events_list li a.month-icon{
		display:block;
		width:48px;
		height:29px;
		padding:18px 7px 0 0;
		background-image:url(<%=Web.Root%>images/month-sprite.jpg);
		background-repeat:none;
		float:left;
		margin-right:4px;
		color:#fff;
		font-size:26px;
		line-height:26px;
		text-align:center;
		text-shadow:1px 1px 3px rgba(0, 0, 0, 0.3);
		text-decoration:none;
	}
	#events_list li a.jan{background-position:0 -48px;}
	#events_list li a.feb{background-position:-55px -48px;}
	#events_list li a.mar{background-position:-110px -48px;}
	#events_list li a.apr{background-position:-165px -48px;}
	#events_list li a.may{background-position:-220px -48px;}
	#events_list li a.jun{background-position:-275px -48px;}
	#events_list li a.jul{background-position:-330px -48px;}
	#events_list li a.aug{background-position:-385px -48px;}
	#events_list li a.sep{background-position:-440px -48px;}
	#events_list li a.oct{background-position:-495px -48px;}
	#events_list li a.nov{background-position:-550px -48px;}
	#events_list li a.dec{background-position:-605px -48px;}
	
	#events_list li.dimmed a.jan{background-position:0 0;}
	#events_list li.dimmed a.feb{background-position:-55px 0;}
	#events_list li.dimmed a.mar{background-position:-110px 0;}
	#events_list li.dimmed a.apr{background-position:-165px 0;}
	#events_list li.dimmed a.may{background-position:-220px 0;}
	#events_list li.dimmed a.jun{background-position:-275px 0;}
	#events_list li.dimmed a.jul{background-position:-330px 0;}
	#events_list li.dimmed a.aug{background-position:-385px 0;}
	#events_list li.dimmed a.sep{background-position:-440px 0;}
	#events_list li.dimmed a.oct{background-position:-495px 0;}
	#events_list li.dimmed a.nov{background-position:-550px 0;}
	#events_list li.dimmed a.dec{background-position:-605px 0;}
	
	#events_list li div.detail{
		clear:both;
		margin-top:7px;
		padding:10px;
		color:#293752;
	}
	#events_list li div.detail a.close_detail{
		display:block;
		width:16px;
		height:16px;
		float:right;
		background:url(<%=Web.Root%>images/btn_close_detail.png) 0 0 no-repeat;
		cursor:pointer;
	}
	#events_list li div.detail a{
		color:#5794C8;
		font-family:Arial, Helvetica, sans-serif;
		font-size:12px;
		text-decoration:underline;
	}
	
	#events_list li div.jan{background-color:#F8F1D4;}
	#events_list li div.feb{background-color:#EFDEEB;}
	#events_list li div.mar{background-color:#E1EEF7;}
	#events_list li div.apr{background-color:#DEF1EF;}
	#events_list li div.may{background-color:#F3E7F9;}
	#events_list li div.jun{background-color:#F2DFD8;}
	#events_list li div.jul{background-color:#E4F4F5;}
	#events_list li div.aug{background-color:#DAF1DB;}
	#events_list li div.sep{background-color:#E9E6F6;}
	#events_list li div.oct{background-color:#F3E5D7;}
	#events_list li div.nov{background-color:#EEE1F1;}
	#events_list li div.dec{background-color:#E8F2D8;}
	
	#events_list li.dimmed div.dec,
	#events_list li.dimmed div.jan,
	#events_list li.dimmed div.feb,
	#events_list li.dimmed div.mar,
	#events_list li.dimmed div.apr,
	#events_list li.dimmed div.may,
	#events_list li.dimmed div.jun,
	#events_list li.dimmed div.jul,
	#events_list li.dimmed div.aug,
	#events_list li.dimmed div.sep,
	#events_list li.dimmed div.oct,
	#events_list li.dimmed div.nov,
	#events_list li.dimmed div.dec{
			background-color:#f0f0f0;
	}
	
	</style>

<script type="text/javascript">
	$(document).ready(function () {
		$("#events_list li.dimmed:last").addClass("dimmedborder");
		$('ul#events_list li .view').css("cursor", "pointer")
		$('ul#events_list li .month-icon').css("cursor", "pointer")
		$('ul#events_list li h3').css("cursor", "pointer")
		$('ul#events_list li .detail').hide();
		$('ul#events_list li .overview').toggle(
				function () {
					$(this).parent().children('.detail').slideDown(500);
					$(this).children('.view').html("Close")
				}, function () {
					$(this).parent().children('.detail').slideUp(500);
					$(this).children('.view').html("View Item")
				}
			);
		// close button in sliding div
		$('ul#events_list li .detail .close_detail').click(
					function () {
						$(this).parent().parent().find(".overview").click();
					}
				);
	});
</script>
<%
//Models.EventList.Load(new Sql("Select * from Event where IsPublished=1 and StartDate >=", DateTime.Now.AddDays(-7).SqlizeDate(), " order by StartDate"));
%>
<div id="newsevents_events_wrapper" class="box_outer_border">
    <div class="box_inner_border">
		<%if (Model.showEventFilter) {%>
			<h2 style="margin-top:-5px;">Events
				<div style="float:right;">
					<form name="form" id="form" method="get" style="margin:0px;margin-top:-3px;">
							<select name="EventCategoryID" onchange="form.submit();">
								<option value="0">All Events</option>
								<%
								//sql= "Select EventCategoryID, Title from EventCategory where ShowOnExtranetOnly=0 order by Sortposition, Title desc"
								//WriteDropDownOptions sql, eventCategoryID
								%>
						</select>
						<% if(Request["dev"]+""=="1"){%><input type="hidden" name="dev" value="<%=Request["dev"]%>"><%}%>
						<input type="hidden" name="filter" value="<%=Request["filter"]%>">
					</form>
				</div>
			</h2>
		<%}else{%>
			<h2 style="margin-top:-5px;">Events</h2>
		<%}%>
		<ul id="events_list">
			<%
			foreach(var rs in Model.eventList) 
			{
				var eventDate = rs.StartDate.FmtDate();//Model.FormatNewsDate(["StartDate"])
				string eventEndDate;
				if (!rs.EndDate.HasValue) {
					eventEndDate = "";
				}else{
					eventEndDate = " - " + rs.EndDate.Value.FmtDate();
				} 
				string style = "";//LCase(Split(FmtDate(rs("EventDate")),"-")(1));
				string eventDay = "";//Split(FmtDate(rs("EventDate")),"-")(0);
				%>
				<li <%if(rs.StartDate < DateTime.Now){ %> class="dimmed"<% }  %>>
					<div class="overview">
							<a class="month-icon <%=style %>"><%=eventDay %></a>				
							<h3><%=rs["Title"].ValueObject %></h3>
							<p class="date"><%=eventDate %> <%=eventEndDate %></p>
							<a class="view">View Item</a>
					</div>
					<div class="detail <%=style %>">
					<p class="margin3"><%=Fmt.StripTags(rs.Description) %></p>
					<a class="close_detail"></a>
					<%if(rs["LinkURL"]+"" != ""){ %>
							<a href="<%=rs["LinkURL"]%>" target="_blank" class="">Click here to view more information</a>
					<%} %>
				<div style="clear:both;margin-bottom:-5px;"></div>
				</div>				
			</li>
				 <%
				}
     	%>
		</ul>
		<div class="clear"></div>
    </div>
</div>

</asp:Content>
