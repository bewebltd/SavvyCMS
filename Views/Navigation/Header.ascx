<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.NavViewModel>" %>
<%@ Import Namespace="Models" %>

			<div class="dontprint">
				<a href="<%=Model.baseUrl%>" target="_top"><img src="<%=Web.Root%>images/logo.gif" width="206" height="69" alt="Logo" class="logo" /></a>
				<img src="<%=Web.Root%>images/find_your_nearest_store.gif" width="156" height="24" alt="" class="find_store" />
				<a href="<%=Model.baseUrl%>/Contact" target="_top" class="find_store_search"><img src="<%=Web.Root%>images/blank.gif" /></a>
				<%--<input name="findstoresearch" type="image" src="<%=Web.Root%>images/blank.gif" class="find_store_search" onclick="window.location='<%=baseUrl%>Contact'" />--%>
				<img src="<%=Web.Root%>images/sign_up_for_travel_deals.gif" width="157" height="24" alt="" class="sign_up" />
				<a href="<%=Model.baseUrl%>/Register" target="_top" class="sign_up_btn_wide"><img src="<%=Web.Root%>images/blank.gif" /></a>
				<%--<input name="signupbtn" type="image" src="<%=Web.Root%>images/blank.gif" class="sign_up_btn_wide" onclick="window.location='<%=baseUrl%>Register'" />--%>
				<img src="<%=Web.Root%>images/call_the_professionals.gif" width="205" height="51" alt="" class="call_pros" />
				<ul class="nav">
					<li class="home">
						<a title="Home" href="<%=Model.baseUrl%>" target="_top"><span></span></a>
						<ul>
							<%foreach (var subpage in Model.SubPages){ %>
								<li><a href="<%=subpage.GetFullUrl()%>" class="<%=subpage.ID==Model.CurrentPageID?"active":""%>" title="<%:subpage.Title%>" target="<%if(subpage.LinkUrlIsExternal){ %>_blank<%} else{%>_top<%} %>"><%:subpage.GetNavTitle()%></a></li>
							<%} %>
						</ul>
					</li>
					<%
					// top level nav
					foreach (var page in Model.Pages) {
						string css = "";
						if (Model.SectionCode==page.PageCode) {
							css = "active";
						}
						%>
						<li class="<%=page.PageCode.ToLowerInvariant()%>"><a href="<%=page.GetFullUrl()%>" class="<%=css%>" title="<%=page.Title.HtmlEncode()%>" target="_top"><span></span></a>
						</li>
						<%
					}	
					%>
				</ul>				
			</div>