<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dropboxjqueryjsontest-3boxes.aspx.cs" Inherits="tests_dropboxjqueryjsontest3boxes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script  type="text/javascript" src="../js/jQuery/jquery-1.3.2.min.js"></script>
</head>
<body>
    <div>
<script type="text/javascript">
/* <![CDATA[ */

(function($) {

$.fn.changeType = function(){

	var data = [
		{
		"DB1Main": [{"id":0,"title":"IT"}],
		"DB1Sub": [
		        { "id": 2, "title": "Programmer" },
		        { "id": 2, "title": "Solutions Architect" },
		        { "id": 2, "title": "Database Developer" }
		        ]
		},
		{"DB1Main": [{"id":1,"title":"Accounting"}],
		"DB1Sub": [
			    { "id": 2, "title": "Accountant" },
			    { "id": 2, "title": "Payroll Officer" },
			    { "id": 2, "title": "Accounts Clerk" },
			    { "id": 2, "title": "Analyst" },
			    { "id": 2, "title": "Financial Controller" }
			    ]
		},
		{
		"DB1Main": [{"id":2,"title":"HR"}],
		"DB1Sub": [
			    { "id": 2, "title": "Recruitment Consultant" },
			    { "id": 2, "title": "Change Management" },
			    { "id": 2, "title": "Industrial Relations" }
			    ]
		},
		{
		"DB1Main": [{"id":3,"title":"Marketing"}],
		"DB1Sub": [
			    { "id": 2, "title": "Market Researcher" },
			    { "id": 2, "title": "Marketing Manager" },
			    { "id": 2, "title": "Marketing Co-ordinator" }
			    ]
		}
		]

		var options_DB1Main = '<option>Select<\/option>';
		$.each(data, function(i, d)
		{
			options_DB1Main += '<option value="' + d.DB1Main.id + '">' + d.DB1Main.title + '<\/option>';
		});
		$("select#DB1Main", this).html(options_DB1Main);

		$("select#DB1Main", this).change(function()
		{
			var index = $(this).get(0).selectedIndex;
			var d = data[index-1];  // -1 because index 0 is for empty 'Select' option
			var options = '';
			if (index > 0) 
			{
				$.each(d.DB1Sub, function(i, j)
				{
		                    options += '<option value="' + j.id + '">' + j.title + '<\/option>';
				});
			} else 
			{
				options += '<option>Select<\/option>';
			}
			$("select#DB1Sub").html(options);
		})
};

})(jQuery);

$(document).ready(function() {
	$("form#search").changeType();
});

/* ]]> */
</script>
HTML
<form id="search" action="" name="search">
	<select name="DB1Main" id="DB1Main">
		<option>Select</option>
	</select>

	<select name="DB1Sub" id="DB1Sub">
		<option>Select</option>
	</select>
</form>
    </div>
</body>
</html>
