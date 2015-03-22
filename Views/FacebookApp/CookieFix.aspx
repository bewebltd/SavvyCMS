<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Cookie Fix</title>
	</head>
	<body>
		
		<script>
			window.onunload = function () {
				var win = window.opener;
				win.cookieFixWindowClosed(getCookieSupport());
			};
			setTimeout(function () { window.close(); }, 500);

			function getCookieSupport() {
				var persist = true;
				do {
					var c = 'gCStest=' + Math.floor(Math.random() * 100000000);
					document.cookie = persist ? c + ';expires=Tue, 01-Jan-2030 00:00:00 GMT' : c;
					if (document.cookie.indexOf(c) !== -1) {
						document.cookie = c + ';expires=Sat, 01-Jan-2000 00:00:00 GMT';
						return persist;
					}
				} while (!(persist = !persist));
				return null;
			}

		</script>

	</body>
</html>
