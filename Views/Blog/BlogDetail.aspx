<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.BlogController.DetailViewModel>" MasterPageFile="~/site.master" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent"><script type="text/javascript">
		$(document).ready(function () {
			$('input[type="text"],textarea').addClass('required')
			$('#company').removeClass('required')
			<%if(Request["post"]!=null) {%>
			$('#thanksMsg').show();
			window.setTimeout(function(){$('#thanksMsg').hide('slow');},5000)
			<%} %>
		});
		function ExtraValidation(form, invalidFields) {
			//extra validation here
			return true;
		}
	</script></asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">



	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td class="titleblock" bgcolor="#ffffff" width="219" valign="top">
				<table border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td valign="top">
							<h1> Blog</h1>
							<a href="<%=Web.Root %>blog">&laquo;&nbsp;Back to blog list</a>
						</td>
					</tr>
				</table>
			</td>
			<td width="23">
				&nbsp;
			</td>
			<td bgcolor="#ffffff" class="contentblock" valign="top">
				<table border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td class="blog-entry" valign="top">
							<!-- 
								<br />
								<br />
							-->
							
							<h2><%=Model.BlogTitle.HtmlEncode() %></h2>
							<span class="date"><% =(Model.BlogDate!=null)?Fmt.DateTime(Model.BlogDate):"no date"%></span>
							<div class="blog-text">
								
								<% =Fmt.HTMLText(Model.BlogBody)%>

								<div class="blog-actions">
									<a class="ld f-right" href="http://www.linkedin.com/shareArticle?mini=true&url=<%=Url %>"><img src="<%=Web.Root %>images/ld.jpg" alt="Share this post on Linkedin"/></a>
									<a class="fb f-right" href="http://www.facebook.com/share.php?u=<%=Url%>" onclick="return fbs_click()" target="_blank"><img src="<%=Web.Root %>images/fb.jpg" alt="Share this post on Facebook"/></a>
									<p class="share f-right">Share on :</p>
								</div>
							</div>
							<%--shown by server, see css above--%>
							<div id="thanksMsg">Thank you for your comment. If it has not appeared immediately, it is awaiting moderation and will be up shortly.</div>
							<%
							if(Model.blogCommentList.Rows.Count>0)
							{
								%>
								<h3 class="comm-title">Comments</h3>
								<%
								for(int sc = 0;sc<Model.blogCommentList.Rows.Count;sc++)
								{
									var dr = Model.blogCommentList.Rows[sc];
									string value = dr["Title"]+"";
									string body = dr["BodyText"]+"";
									bool bodyIsTooLong = false;
									
									if (body.Length > 190) bodyIsTooLong = true;
									%>
									<h2><%=value.HtmlEncode() %></h2>
									<div>
										<div class="comm-date">
											<%=Fmt.DateTime(dr["DateAdded"]+"") %>
										</div>
										<div class="comm-text">
											<span id="teaserbody<%=sc %>">
													<%if (bodyIsTooLong){%>
													<%=Fmt.Ellipsis(Fmt.StripTags(body),190) %>
													
													<%} %>
											</span>
											<%if (bodyIsTooLong){%>
											<a href="" class="comm-show-full" onclick="$(this).hide('slow');$('#teaserbody<%=sc %>').hide('slow');$('#body<%=sc %>').show('slow');return false;">Show Complete Comment &raquo;</a>
											<%} %>
											<div <%if(bodyIsTooLong){ %>style="display:none"<%} %> id="body<%=sc %>">
												
												<% =Fmt.StripTags(body)%>
											</div>
										</div>
										<div class="blog-rule"></div>
									</div>
								<%	
								}
							}else{
								%>
								<div class="blog-rule">
									<p class="blog-msg">This post has no comments.</p>
								</div>
							<%
							}
							 %>
							 <!-- xx only if logged in -->
							 <%if(true){ %>
								<a href="" onclick="$('#commentform').show('slow');$(this).hide('slow');return false;" class="comments-btn" id="add-comm" style="clear:both;">Add a comment</a>
								<div class="comm-form" id="commentform" style="display: none;">
									<form action="" method="post">
										<%if(Web.Session["etcglobalCurrentUserID"]+""==""){ %>

											<label for="company">Company Name:</label><input type="text" id="company" 	name="company" maxlength="50"/><br />
											<label for="firstname">First Name: *</label><input type="text" id="firstname"	name="firstname" maxlength="50"/><br />
											<label for="lastname">Last Name: *</label><input type="text"	 id="lastname" 	name="lastname" maxlength="50"/><br />
											<label for="email">Email: *</label><input type="text"					 id="email"			name="email" maxlength="80" class="email"/><br />
										<%} %>
										<label for="title">Comment Title: *</label><input type="text" name="title" id="title" maxlength="50"/><br />
										<label for="body">Comment: *</label><textarea name="body" id="body"></textarea>
										<div style="clear:both"><small>* = required field</small></div>
										<input type="submit" name="go" value="Post Comment" class="comments-btn"/><br />
									</form>
								</div>
								<script type="text/javascript">
									$(document).ready(function () {
										$("#add-comm").click(function () {
											$("#commentform").show();
										});
									});
								</script>

							<%} else{%>
								<p class="comment-login">You need to login to post a comment.
								<a href="<%=Web.Root %>security/login?ReturnUrl=<%=("blogdetail.aspx?page="+Request["page"]+"&id="+Request["id"]).UrlEncode() %>" class="comments-btn comment-login">login</a>
								</p>
								<div class="clear"></div>
							<%} %>
							<%--<site:FooterNav ID="FooterNav1" runat="server"></site:FooterNav>--%>
						</td>
					</tr>
				</table>
				<script type="text/javascript">
					function fbs_click() {
						u=location.href;
						t=document.title;window.open('http://www.facebook.com/sharer.php?u='+encodeURIComponent(u)+'&t='+encodeURIComponent(t),'sharer','toolbar=0,status=0,width=626,height=436′);
						return false;
					}
				</script>
			</td>
		</tr>
	</table>
</asp:Content>
