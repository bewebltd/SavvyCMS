<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<div class="frontend questionnaire">
		<div class="inner">
			<h1>Oops, there was a problem...</h1>

			<p>The most common cause of what just happened is that you have cookies turned off, or didn't allow this site to save one.</p>
			<p>This site needs to use a cookie to work, it's what provides security between you and us, so we know it's really you answering the questionnaire.</p>
			
			<h3 style="margin-top: 30px;">What are cookies? I heard they were bad?</h3>
			<p>A 'cookie' is a weird name for a little text file that websites can save on your computer. In our case that file contains an encrypted string, that can be checked on our server to make sure it's not a robot trying to send us potentially harmful data.</p>
			<p>Cookie files themselves are harmless, they are just some text, but the reason you may have heard that cookies are bad is because once you have a cookie, the website that put it there (and ONLY that website) can see if it's there next time you visit the website. This means you aren't as anonymous as you think because the website knows you visited it before.</p>
			<p>The first thing we do is ask for your name, we need to know who you are. This is not the sort of site where anonymity is beneficial.</p>
			
			<h3 style="margin-top: 30px;">What can I do to fix it?</h3>
			<p>Here is how you can turn cookies on: <a href="http://support.google.com/accounts/bin/answer.py?hl=en&answer=61416" target="_blank">http://support.google.com/accounts/bin/answer.py?hl=en&answer=61416</a></p>
			<p>Then just either click "back" on your browser, or you can re-click the link in your email to start again.</p>
		</div>
	</div>
	<div class="poweredby">
		
	</div>
</asp:Content>


