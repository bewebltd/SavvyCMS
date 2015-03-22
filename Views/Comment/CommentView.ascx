<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.CommentController.ViewModel>" %>

<script>
		function ReplyToComment(parentid, name) {
			goToByScroll("commentForm");
			$("#Comment_ParentID").val(parentid);
			$("#comment").text("Reply to "+name+"'s comment");
			
			// clear existing thankyou in case commenting more than once and show form
			$("#commentThankYou").html("");			
			$('#commentForm').show();
		}
		
		
		$(document).ready(function () {
		//	BewebInitForm("commentForm");
		});

		function AjaxSubmitComment(form) {
			var isValid =  CheckBasicFormValidation(form);
			if (isValid) {
				var url = "<%=Web.Root %>Comment/SendComment?auctionID=<%=Model.AuctionID%>&buyNowID=<%=Model.BuyNowID%>";
				<%if(!Security.IsLoggedIn) {%>
					url += "&name=" + escape($('#Comment_FirstName').val());
					url += "&email=" + escape($('#Comment_Email').val());
				<% }%>
				url += "&comment=" + escape($('#CommentText').val());
				url += "&parentID=" + escape($('#Comment_ParentID').val());
				url += "&auth=<%=Model.CommentPostAuthToken%>";
			
				$("#commentThankYou").load(url);
				$('#commentForm').hide();
				<%if(!Security.IsLoggedIn) {%>
					$('#Comment_FirstName').val('');
				<% }%>

				$('#CommentText').val('');
				$('#Comment_Email').val('');
				$('#Comment_ParentID').val('');
				$('#Comment_SignMeUp').val('');
				$('#comment').text("Leave a comment");
				$('#commentThankYou').show();		
			}
			return false;
		}

		function ExtraValidation() {
			alert("extra");
			return false;
		}
</script>

<div id="comments">
	<div class="commentBubble"></div>
	<h2><%=Fmt.Plural(Model.CommentList.Count, "Comment") %></h2>

	<%if (Model.CommentList.Count == 0) { %>
		<p class="noComments">There are no comments for this auction.</p>
	<% }else{ %>	
		<ul class="comment-list">
			<% foreach (var comment in Model.CommentList) {%>
				<li<%=comment.IndentCss%>>
					<%if(comment.PersonType == "Moderator") {%><img src="<%=Web.Root %>images/new-world-comment-badge.png" width="40" height="35" class="badge" title="New World Auction Moderator" /><%} %>
					<div class="left">
						<h3><%=comment.CommenterName.HtmlEncode() %></h3>
						<p class="text"><%=comment.CommentText.HtmlEncode() %></p>
						<p class="date">
							<%=Fmt.DateTime(comment.CommentDate).Replace("-"," ") %> 
							<%if (comment.IndentCss != " class=\"reply2\"") { %>
									<a href="#" onclick="ReplyToComment(<%=comment.ID%>, '<%=comment.CommenterName.HtmlEncode() %>');return false;">Reply</a>
							<% } %>
						</p>
					</div>
					<div class="clear"></div>
				</li>
			<%} %>
		</ul>
	<% } %>

	<form action="" method="post" id="commentForm" name="form" class="AutoValidate">
		<div class="validation-feedback" style="display: none;"><p style="font-size: 14px"><b>Drat!</b></p><br>
				<ul>
					<li id="validation_Comment_FirstName" style="display: none;">You don’t appear to have a name! Can you find one and try again?</li>
					<li id="validation_Comment_Email" style="display: none;">I can’t recognise that email address! Can you give it another go?</li>
					<li id="validation_CommentText" style="display: none;">What’s up? Enter a comment and tell us! </li>
				</ul>
			</div>
		<h2 style="width: 500px;">Got something to say? Leave a comment.</h2>

		<%if(!Security.IsLoggedIn) { %>
			<label>Name:*</label>
			<%= new Forms.TextField("Comment_FirstName", "", true){ShowValidation = true} %>
			<%--<span class="validation" id="validation_Comment_FirstName">test</span>--%>

			<label>Email:*</label>	
			<%= new Forms.EmailField("Comment_Email", "", true){ShowValidation = true} %>

		<%}else{ %>
			<div class="staticName">
				<label>Name:</label>
				<div><%=UserSession.Person.UserName %></div>
			</div>
			<div class="staticName">
				<label>Email:</label>	
				<div><%=UserSession.Person.Email %></div>
			</div>
		<%} %>

		<label class="left">Comment:* </label> 
		<%= new Forms.TextArea("CommentText", "", true){ShowValidation = true}  %>
		<%= new Forms.HiddenField("Comment_ParentID","") %>

		<input type="button" class="submitCommentBtn gfxBtn" value="Submit Comment" onclick="AjaxSubmitComment(this.form); return false;" />
	</form>
	<div id="commentThankYou"></div>
</div>
