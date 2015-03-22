<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.LiveChatController.LiveChatViewModel>" %>
<script type="text/javascript">
	var timer;
	var chatRefresh = <%=Model.ChatRefresh %>;
	var lastChatUpdate = "<%=Fmt.DateTime(DateTime.Now, Fmt.DateTimePrecision.Millisecond) %>";
	$(document).ready(function () {
		// scroll chat box to the bottom
		$("#chat-box .msg-box").prop({ scrollTop: $("#chat-box .msg-box").prop("scrollHeight") });
		<%if(Model.IsLoggedIn) {%>
		// set up a timer to check for new chats
		timer = setTimeout("UpdateLiveChat()", chatRefresh);
		$("#ChatPost").keypress(function(event) {
			if (event.which == 13) {
				event.preventDefault();
				PostToLiveChat();
			}
		});
		<%}else{%>
			$("#ChatPost").hide();
			$("#chat-box .msg-input .btn").attr("onclick", "");
			$("#chat-box .msg-input .btn").text("Login to participate");
			$("#chat-box .msg-input .btn").attr("href", "<%=Web.Root %>security/Login?ReturnUrl=<%=Html.Encode(Web.FullUrl) %>");
		<%}%>
	});

	function ScrollLiveChatToBottom() {
		$("#chat-box .msg-box").animate({ scrollTop: $("#chat-box .msg-box").prop("scrollHeight") }, 500);
	}

	function UpdateLiveChat() {
		clearTimeout(timer);
		var el = $("#chat-box .msg-box");
		var isAtBottom = (el[0].scrollHeight - el[0].scrollTop == el.innerHeight()) ? true : false;
		$.getJSON('<%=Web.BaseUrl +"LiveChat/UpdateLiveChat" %>',
			{
				"lastupdate": lastChatUpdate
			},
			function (data) {
				lastChatUpdate = data.LastUpdate;
				chatRefresh = data.ChatRefresh;
				var date = new Date();
				for (var i = 0; i < data.ChatList.length; i++) {
					var thisPost = data.ChatList[i];
					var timestamp = thisPost.Time;
					postDate = thisPost.Date.split("/");
					timestamp += (parseInt(postDate[0])!=date.getDate() && parseInt(postDate[1])!= date.getMonth()+1)? " on " + thisPost.Date:" today";
					$("#chat-box .msg-box").append("<div class='reply' id='reply" + thisPost.ID + "'><p class='name'>" + thisPost.FullName + "<br><small>at "+timestamp+"</small></p><p class='msg'>" + thisPost.Post + "</p></div>");
				}
				if (isAtBottom) {
					ScrollLiveChatToBottom();
				}
				timer = setTimeout("UpdateLiveChat()", chatRefresh);
			});
	}

	function PostToLiveChat() {
		clearTimeout(timer);
		var post = $("#ChatPost").val();
		if (post != $("#ChatPost").attr("data-placeholder") && post != "") {
			$.getJSON('<%=Web.BaseUrl +"/LiveChat/PostToLiveChat" %>',
				{
					'id': '<%=Model.EncryptedPersonID %>',
					'Post': post
				},
				function() {
					UpdateLiveChat(lastChatUpdate);
				});
		}
		ScrollLiveChatToBottom();
		$("#ChatPost").val("");
	}
</script>
<style>
		/* chat box */
	#chat-box {background-color: #CCC; border: 1px solid #999;width: 216px;padding: 10px;} 
	#chat-box h3 {color: #000;} 
	#chat-box .msg-response{}
	#chat-box .msg-response .msg-box{ background: #fff; border: 1px solid #999; width: 200px; height: 130px; padding: 8px; overflow-y: scroll; }
	#chat-box .reply{ margin-bottom: 8px; }
	#chat-box .reply p { margin-bottom: 0; }
	#chat-box .reply .name{ font-weight: bold;line-height: 12px;  }
	#chat-box .reply .name small{ font-weight: normal;font-size: 10px;letter-spacing:1px;color:#999;}
	#chat-box .msg-input{ width: 216px;}
	#chat-box .msg-input input{ width: 204px; margin-top: 11px; padding: 2px 6px 3px 6px;color: #000; }
	#chat-box .msg-input .btn{float: right;cursor: pointer;padding: 3px 6px;border: 1px solid #999;background-color: #FFF;margin-top: 6px; }
</style>
<div id="chat-box">
	<h3>LiveChat</h3>
	<div class="msg-response">
		<div class="msg-box">
			<%if(Model.ChatList != null && Model.ChatList.Count > 0) {
					var date = DateTime.Now;

					foreach (var post in Model.ChatList.OrderBy(cl => Convert.ToDateTime(cl.Date)).ThenBy(cl => cl.Time)) {
						var timestamp = post.Time;
						string[] postDate = post.Date.Split('/');
						timestamp += (postDate[0].ToInt() != date.Day || postDate[1].ToInt() != date.Month)?" on " + post.Date: " today";
						%>
					<div class="reply">
						<p class="name"><%= post.FullName %><br><small>at <%=timestamp %></small></p>
						<p class="msg"><%= post.Post %></p>
					</div>
				<% }
			 } else { %>
				<div class="reply">
					<p class="msg">- No current active chats -</p>
				</div>
			<%} %>
		</div>
	</div>
	<div class="msg-input">
		<input type="text" id="ChatPost" data-placeholder="Type your comment here" value="" />
		<div class="clear"></div>
		<a class="btn" onclick="PostToLiveChat();">Post<span></span></a>
		<div class="clear"></div>
	</div>
</div>