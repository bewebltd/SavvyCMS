<%@ Page Title="" Language="C#" %>

<h2>
	Testimonials</h2>
<div class="box">
	<%foreach (var te in Models.TestimonialList.LoadActive()) { %>
	<p>
		<strong>
			<%=te.Comments %>
		</strong>
	</p>
	<p>
		<em>
			<%=te.Author %>
			<br />
			<%=te.AuthorRole %>
		</em>
	</p>
	<%} %>
</div>
