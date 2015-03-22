<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<BlogController.ViewModel>" MasterPageFile="~/site.master" Title="" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="server">
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td class="titleblock" bgcolor="#ffffff" width="219" valign="top">
				<table border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td valign="top">
							<div class="entries-list">
								<%if(Web.Request["d"]==null){%>
									<h1> Blog Entries</h1>
									<%} else{%>
									<h1> Blog Archive Entries for <%= Web.Request["d"].Split('-')[1] + ", " + Web.Request["d"].Split('-')[0]%></h1>
									<a href="<%=Web.Root %>blog?page=<%=Web.Request["page"] %>" title="Back to Full Blog" class="back-to-blog">&lt; Back to Blog</a>
									<%}%>
								<ul class="blog-menu">
								<%for(int sc = 0;sc<Model.blogList.Rows.Count;sc++)
								{
									var dr = Model.blogList.Rows[sc];
									string value = dr["Title"]+"";
			  
									%>
									<li>
										<a href="#blog<%=sc %>">&raquo;&nbsp; <%=value %></a>
									</li>
									<%
								}
								%>
								</ul>
							</div>
							<%if(Model.blogArchiveList.Rows.Count>0) {%>
								<div class="blog-archive">
									<h2>Blog Archive</h2>
									<ul class="blog-menu">
								
									<%for(int sc = 0; sc < Model.blogArchiveList.Rows.Count; sc++)
									{
										var dr = Model.blogArchiveList.Rows[sc];
									
										string value = dr["ArchiveDate"]+"";
										if(value.IsNotBlank())
										{
											var pairs = value.Split('-');
			  
											%>
											<li>
												<!-- in the href: needs a way to filter. Get all blogs by the selected ArchiveDate and show them -->
												<a href="<%=Web.Root %>blog?page=<% = Web.Request["page"]%>&d=<%=value%>">&raquo;&nbsp;<%=pairs[1] + pairs[0] + "(" + dr["NumBlogs"] + ")"%></a>
											</li>
											<%
										}
									}
									%>
									</ul>
								</div>
							<%} %>
						</td>
					</tr>
				</table>
			</td>
			<td width="23">
				&nbsp;
			</td>
			<td bgcolor="#ffffff" class="contentblock" valign="top">
				<table class="blog" border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td class="blog-entry" valign="top">
							<!-- 
								<br />
								<br />
							 -->
							<%for(int sc = 0;sc<Model.blogList.Rows.Count;sc++)
							{
								var dr = Model.blogList.Rows[sc];
								string value = dr["Title"]+"";
								string body = dr["BodyText"]+"";
								string Url = Server.UrlEncode(Web.Root+ "blog/blogdetail?id="+ Crypto.EncryptID(dr["BlogID"]+""));
								%>
								<h2>
                    <a name="blog<%=sc %>" href="<%=Web.Root %>blog/blogdetail?page=<%= Web.Request["page"] %>&id=<%=Crypto.EncryptID(dr["BlogID"]+"") %>" title="View Full Entry"><%=value.HtmlEncode() %></a>
                </h2>
								<div>
                  <div class="blog-date"style="float:right">
                      <%=Fmt.DateTime(dr["DateAdded"]+"") %>
                  </div>
									<div class="blog-text">
										<%=Fmt.Ellipsis(Fmt.StripTags(body),500)%>
										<div class="blog-actions">
											<p class="share">Share on :</p>
											
											<a class="fb" href="http://www.facebook.com/share.php?u= <%=Url%>" onclick="return fbs_click()" target="_blank"><img src="<%=Web.Root %>images/fb.jpg" alt="Share this post on Facebook"/></a>
											<a class="ld" href="http://www.linkedin.com/shareArticle?mini=true&url=<%=Url %>" target="_blank"><img src="<%=Web.Root %>images/ld.jpg" alt="Share this post on Linkedin"/></a>
											<a href="<%=Web.Root %>blog/blogdetail?page=<%= Web.Request["page"] %>&id=<%=Crypto.EncryptID(dr["BlogID"]+"") %>" class="comments-btn">See Comments</a>
										</div>
									</div>
									<div class="blog-rule"></div>
								</div>
								<%
							}
							 %>
							 <script>
								function fbs_click() {
								u=location.href;
								t=document.title;window.open('http://www.facebook.com/sharer.php?u='+encodeURIComponent(u)+'&t='+encodeURIComponent(t),'sharer','toolbar=0,status=0,width=626,height=436′);
								return false;}
							</script>
							<%--<site:FooterNav ID="FooterNav1" runat="server"></site:FooterNav>--%>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
