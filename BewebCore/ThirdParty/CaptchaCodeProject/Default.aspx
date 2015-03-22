<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="CaptchaImage._Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CaptchaImage Test</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">
			BODY { FONT-SIZE: 10pt; FONT-FAMILY: sans-serif }
			TD { FONT-SIZE: 10pt; FONT-FAMILY: sans-serif }
			TH { FONT-SIZE: 10pt; FONT-FAMILY: sans-serif }
			.notice { font-size: 90%; }
			.info { FONT-WEIGHT: bold; COLOR: #008000 }
			.error { FONT-WEIGHT: bold; COLOR: #800000 }
		</style>
	</HEAD>
	<body>
		<h2>CaptchaImage Test</h2>
		<p>A demonstration using the <code>CaptchaImage</code> object to prevent automated 
			form submission.</p>
		<form id="Default" method="post" runat="server">
			<img src="JpegImage.aspx"><br>
			<p>
				<strong>Enter the code shown above:</strong><br>
				<asp:TextBox id="CodeNumberTextBox" runat="server"></asp:TextBox>
				<asp:Button id="SubmitButton" runat="server" Text="Submit"></asp:Button><br>
			</p>
			<p>
				<em class="notice">(Note: If you cannot read the numbers in the above<br>
					image, reload the page to generate a new one.)</em>
			</p>
			<p><asp:Label id="MessageLabel" runat="server"></asp:Label></p>
		</form>
	</body>
</HTML>
