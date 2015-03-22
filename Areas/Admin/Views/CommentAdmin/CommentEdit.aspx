<%@ Page Title="Edit Comment" Inherits="System.Web.Mvc.ViewPage<Models.Comment>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.8.1/jquery.validate-vsdoc.js"></script><%}   // provides intellisense %>
	<script type="text/javascript">
		$(document).ready(function () {
			BewebInitForm("form");
			$('input[name=NewStatus]').click(function () { ShowModeratorMessage(); });
		});

		function ShowModeratorMessage() {
			var action = $('input[name=NewStatus]:checked').val();
			var quickMessageSelect = $('#QuickMessage');
			if (action == '') {
				$('#QuickMessage').val(''); // clear the email
				$('.moderatorMessage').slideUp();
			}
			else {
				$('#Status').val(action);
				$('option', quickMessageSelect).removeAttr('disabled');
				$('.moderatorMessage').slideDown();
				if (action == 'Approved') {
					$('option', quickMessageSelect).slice(2, 4).attr('disabled', 'disabled');
				}
				if (action == 'Declined') {
					$('option', quickMessageSelect).eq(1).attr('disabled', 'disabled');
				}
			}
		}

		function BuildQuickMessage() {
			<%

				
				var data = Model;
				var url = "";

//				if(data.Auction != null) {
//    			url = data.Auction.GetUrl();
//				}
//				else {
    				url = "NA";
				//}
						
				var personName = data.CommenterName;
				if(data.PersonID != null) {
					var p = Models.Person.LoadByPersonID(Model.PersonID.Value);
					if(p != null) {
						personName = p.FirstName;
					}
				}
			%>		
			var personName = '<%= personName.DefaultValue("N/A").Replace("'", "\\'") %>';
			
			
			<%//if(data.Auction != null) { %>
				//var baseUrl = '<%=Web.Host%><%//=data.Auction.GetUrl().ToLower()%>';
			<%// } %>


        var message = 'Dear ' + personName + '\n\n';
			switch ($('#QuickMessage').val()) {
				case 'Approve':
					message += 'Looks like a great comment! Thanks for sharing it with us.';
					message += '\n\nView your comment at: \n' + baseUrl;
					break;
				case 'DeclineRelevance':
					message += 'Sorry, your comment isn’t relevant to the promotion, we won’t be posting it on the website.';
					break;
				case 'DeclineRude':
					message += 'The comment you submitted is not fit for public viewing, we won’t be posting it on the website.';
					break;
				default:
					break;
			}
			$('#DeclineReason').val(message);
		}
	</script>
	<style type="text/css">
		.moderatorMessage { display: none; }
	</style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Comment </th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
		<%--	<%if (record.AuctionID!=null){ %>
				<tr>
					<td class="label">Auction:</td>
					<td class="field"><a href="<%=record.Auction.GetUrl()%>" target="_blank"><%=record.Auction.Title%> closing <%=record.Auction.ClosingDate.FmtShortDate()%></a></td>
				</tr>
			<%}else{ %>
				<tr>
					<td class="label">Buy Now Item:</td>
					<td class="field"><a href="<%=record.BuyNowItem.GetUrl()%>" target="_blank"><%= record.BuyNowItem.Title%> closing <%=record.BuyNowItem.ClosingDate.FmtShortDate()%></a></td>
				</tr>
			<%} %>
			<tr>
				<td class="label">Auction Closes:</td>
				<td class="field"><%= new Forms.DisplayField(record.Auction.Fields.ClosingDate) %></td>
			</tr>--%>
			<tr>
				<td class="label">Comment Status:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.Status, false) %></td>
			</tr>
			<tr>
				<td class="label">Moderator Action:</td>
				<td class="field">
					<label><input type="radio" name="NewStatus" value="Approved"/> Approve this comment</label>
					<label><input type="radio" name="NewStatus" value="Declined"/> Decline this comment</label>
					<label><input type="radio" name="NewStatus" value=""/> leave it as it is</label>
					
					<div class="moderatorMessage">
						<br/>
						choose a quick message: <select id="QuickMessage" onchange="BuildQuickMessage()">
							<option value=""> -- please select quick message -- </option>
							<option value="Approve">Approve</option>
							<option value="DeclineRelevance">Decline - Irrelevant</option>
							<option value="DeclineRude">Decline - Rude</option>
						</select><br/>
						(any message sent already will show in the box below)<br/>
						<%= new Forms.TextArea(record.Fields.DeclineReason, false){ rows = 7} %><br/>
						<%=Html.SaveButton("Save & Send Email", "SaveButton") %>
					</div>

				</td>
			</tr>

			<% if(record.ParentCommentID.ToString().IsNotBlank()) { %>
			<tr>
				<td class="label">In reply to:</td>
				<td class="field">
					<%
      				var parentComment = Models.Comment.LoadByCommentID(record.ParentCommentID.Value);
					
					 %>
					
					<a href="<%=Web.Root %>Admin/CommentAdmin/Edit/<%= record.ParentCommentID %>">"<%=parentComment.CommentTextShort %>" by <%= parentComment.CommenterFullName %></a>
				</td>
			</tr>
			<% } %>
			<tr>
				<td class="label">Comment Text:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.CommentText ,true){rows = 7} %></td>
			</tr>
			<tr>
				<td class="label">Commenter Icon:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.PersonType, false)
								.Add(Models.Comment.CommentPersonType.Member.ToString()) 
								.Add(Models.Comment.CommentPersonType.Moderator.ToString()) 
								%>
				</td>
			</tr>
			
			<% if(record.PersonID.ToString().IsNotBlank()) { %>
			<tr>
				<td class="label">Person (logged in):</td>
				<td class="field">
					<%
						var fullName = "";
						if(record.PersonID != null) {
							var p = Models.Person.LoadByPersonID(record.PersonID.Value);
							if(p != null) {
								fullName = p.FullName;
							}
						}
					%>
					<%= fullName %>
				</td>
			</tr>
			<% } else { %>
			<tr>
				<td class="label">Commenter Name (not logged in):</td>
				<td class="field"><%=new Forms.TextField(record.Fields.CommenterName, true) %></td>
			</tr>
			<tr>
				<td class="label">Commenter Email:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.CommenterEmail, true) %></td>
			</tr>
			<% } %>
			<tr>
				<td class="label">Commenter IP:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.CommenterIP) %></td>
			</tr>
			<tr>
				<td class="label">Comment Date:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.CommentDate) %></td>
			</tr>
			<tr>
				<td class="label section">Approved By:</td>
				<td class="field section">
					<%=(record.ApprovedByPersonID == null) ? "" : Models.Person.LoadByPersonID(record.ApprovedByPersonID.Value).FullName %>
				</td>
			</tr>
			<tr>
				<td class="label">Approved Date:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.ApprovedDate, false) %></td>
			</tr>
			<tr>
				<td colspan="2" class="footer">
					<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Comment Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

