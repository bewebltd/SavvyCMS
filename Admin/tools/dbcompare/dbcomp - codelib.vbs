option explicit

dim SendEmailOnError
SendEmailOnError = false

dim url1, url2 
'url1 = "http://localhost/projects/futureproofbuilding/admin/tools/dbedit-sql.aspx?showcreate=sqlserver"
'url1 = "http://localhost/codelib/admin/tools/dbedit-sql.aspx?showcreate=sqlserver"
'
url1 = "http://localhost/projects/futureproofbuilding/admin/tools/structure.aspx"
url2 = "http://localhost/codelib/admin/tools/structure.aspx"
'url1 = "http://localhost/codelib/admin/tools/dbstructure.aspx?server=sam4&pas=&us="
'url2 = "http://localhost/codelib/admin/tools/dbstructure.aspx?server=versa.beweb.co.nz&pas=&us="


dim runCommand
'runCommand = """C:\Program Files\Beyond Compare 2\BC2.exe"" file1.html file2.html"
runCommand = """C:\Program Files\Beyond Compare 4\BCompare.exe"" file1.html file2.html"



'-----------------------------------------------------------------------------
'
' Setup constants / globals
'
'-----------------------------------------------------------------------------
const logName = "dbcomp.log"
const openLogInEditor = false' true' 

const editorPath = "c:\program files\cw32\cw32.exe"

' Constants for HttpStatus property.
Const HTTPSTATUS_OK = 200
Const HTTPSTATUS_CREATED = 201
Const HTTPSTATUS_ACCEPTED = 202
Const HTTPSTATUS_MULTISTATUS = 207
Const HTTPSTATUS_BADREQUEST = 400
Const HTTPSTATUS_UNAUTHORIZED = 401
Const HTTPSTATUS_FORBIDDEN = 403
Const HTTPSTATUS_NOTFOUND = 404
Const HTTPSTATUS_INTERNALSERVERERROR = 500




'-----------------------------------------------------------------------------
'
' call main function
'
'-----------------------------------------------------------------------------
call Main()


'-----------------------------------------------------------------------------
'
' Run the main task
'
'-----------------------------------------------------------------------------
function Main()
	startlog "start ["&now()&"]"
	'log "start ["&now()&"]"
	
	Dim ArgObj, var1, var2 
	Set ArgObj = WScript.Arguments 

	'First parameter
	on error resume next
	var1 = ArgObj(0) 
	on error goto 0
	'''Second parameter
	''var2 = ArgObj(1) 
	''msgbox "Variable 1=" & var1 & " Variable 2=" & var2 set ArgObj = Nothing 

	
	'Clear object out of memory
	set ArgObj = Nothing

	call RunTask()
	log "-----END ["&now()&"]"

	'-----------------------------------------------------------------------------
	'
	' open the log in an editor
	'
	'-----------------------------------------------------------------------------
	if openLogInEditor then
		Dim WshShell, oExec
		Set WshShell = CreateObject("WScript.Shell")
		dim str
		str = """"&editorPath&""" "&logName&""
		Set oExec = WshShell.Exec(str)
		Set oExec = nothing
	end if
	'-----------------------------------------------------------------------------

end function


'-----------------------------------------------------------------------------
'
' Run the tasks
'
'-----------------------------------------------------------------------------
function RunTask()
	dim htmlResults1, htmlResults2
	
	dim emailTarget
	emailTarget = "jeremy@beweb.co.nz"
	htmlResults1 = GetPageHTML(url1, emailTarget)

	call writeToFile("file1.html", replace(htmlResults1,"<br>",vbcrlf))
	
	htmlResults2 = GetPageHTML(url2, emailTarget)

	call writeToFile("file2.html", replace(htmlResults2,"<br>",vbcrlf))
	
	log "runCommand["&runCommand&"]"
	
	Dim WshShell, oExec
	Set WshShell = CreateObject("WScript.Shell")
	Set oExec = WshShell.Exec(runCommand)
	Set oExec = nothing

	'log "html ["&left(htmlResults, 1000)&"]"
end function

'-------------------------------------------------------------------------------
Function FmtDate(byval d)
	' date in unambiguous 18-Feb-2000 format
	if IsNull(d) then
		FmtDate = "n/a"
	elseif d = "" then
		FmtDate = "n/a"
	else
		FmtDate = right("0"&Day(d),2) & "-" & MonthName(Month(d),true) & "-" & Year(d)
	end if
end function

'-------------------------------------------------------------------------------
'
' get the text for a page
'
'-----------------------------------------------------------------------------
function GetPageHTML(url, emailTarget)
	log "-----------------------------------------------------------"
	dim ErrorFound
	ErrorFound = false
	
	dim xmlhttp, html
	log "create obj"
	set xmlhttp = createobject("Microsoft.XMLHTTP")
	log "open url ["&url&"]"
	xmlhttp.Open "GET", url, false
	log "send get command"
	on error resume next
	xmlhttp.Send
	if(err.number<>0)then
		LogError "Failed to send GET to URL", url, "err["&err.description&"] num["&err.number&"]", emailTarget
		ErrorFound = true
	end if
	on error goto 0
	log "send done"
	
	if(not ErrorFound)then
		html = ""
		if(xmlhttp.Status = HTTPSTATUS_OK) or (xmlhttp.Status = HTTPSTATUS_MULTISTATUS)then
			html = xmlhttp.responseText
		else
			LogError "HTTP return status not valid", url, xmlhttp.Status,emailTarget
		end if
		log "done"
	else
		log "stopping due to error"
		LogError "Serious but not fatal Error", url, 0, emailTarget
	end if
	set xmlhttp = nothing	 
	GetPageHTML = html
end function



'-----------------------------------------------------------------------------
'
' Test send a mail
'
'-----------------------------------------------------------------------------
function TestEmailSend()
	dim msg
	msg = ""+_
		"This is a test mail."+vbcrlf+_
		""
	dim email, toName, senderEmail, senderName
	email					 = "jeremy@beweb.co.nz"
	toName				 = "Jeremy to"
	senderEmail		 = "jeremy@beweb.co.nz"
	senderName		 = "Jeremy from"
		
	dim mailResult
	mailResult	= SendEmail("Test Mail", msg, "log", email, toName, senderEmail, senderName, "")
	log "email sent : result["&mailResult&"]"
				
end function

'-----------------------------------------------------------------------------
function LogError(msg,url,httpStatus,emailTarget)
	on error goto 0
	log "ERROR: "&msg&": url["&url&"], httpStatus["&httpStatus&"]"

	dim statusText
	
	if(httpStatus = HTTPSTATUS_OK)then
		statusText = "HTTPSTATUS_OK"
	end if
	
	if(httpStatus = HTTPSTATUS_CREATED)then
		statusText = "HTTPSTATUS_CREATED"
	end if
	if(httpStatus = HTTPSTATUS_ACCEPTED)then
		statusText = "HTTPSTATUS_ACCEPTED"
	end if
	if(httpStatus = HTTPSTATUS_MULTISTATUS)then
		statusText = "HTTPSTATUS_MULTISTATUS"
	end if
	if(httpStatus = HTTPSTATUS_BADREQUEST)then
		statusText = "HTTPSTATUS_BADREQUEST"
	end if
	if(httpStatus = HTTPSTATUS_UNAUTHORIZED)then
		statusText = "HTTPSTATUS_UNAUTHORIZED"
	end if
	if(httpStatus = HTTPSTATUS_FORBIDDEN)then
		statusText = "HTTPSTATUS_FORBIDDEN"
	end if
	if(httpStatus = HTTPSTATUS_NOTFOUND)then
		statusText = "HTTPSTATUS_NOTFOUND"
	end if
	if(httpStatus = HTTPSTATUS_INTERNALSERVERERROR)then
		statusText = "HTTPSTATUS_INTERNALSERVERERROR"
	end if
	if(statusText<>"")then
		log "ERROR: statusText["&statusText&"]"
	end if
	if(SendEmailOnError)then
		dim mailmsg
		mailmsg = ""+_
			"ERROR:"+vbcrlf+_
			"	Msg["&msg&"]"+vbcrlf+_
			"	URL["&url&"]"+vbcrlf+_
			"	StatusText["&statusText&"]"+vbcrlf+_
			"	HttpStatus["&httpStatus&"]"+vbcrlf+_
			""
		if(emailTarget<>"popup" and ""&emailTarget<>"")then
			'dim msg
			dim email, toName, senderEmail, senderName
			'email					 = "021489084@vodafone.net.nz" ' jeremys phone
			email 				= emailTarget
			toName				= "Monitor Admin"
			senderEmail		= "monitor@beweb.co.nz"
			senderName		= "Beweb Monitoring System"
				
			dim mailResult
			mailResult	= SendEmail("Site Error:["&url&"] ["&msg&"]", mailmsg, "log", email, toName, senderEmail, senderName, "")
			log "email sent : result["&mailResult&"]"
		else
			if(""&emailTarget<>"")then
				msgbox "SITE MONITOR "&mailmsg&""
			end if
		end if
	end if
	
end function
'-----------------------------------------------------------------------------
'
' start log and write to log functions
'
'-----------------------------------------------------------------------------
function startlog(msg)
	Const ForReading = 1, ForWriting = 2, ForAppending = 8
	Dim fso, f
	Set fso = CreateObject("Scripting.FileSystemObject")
	Set f = fso.OpenTextFile(logName, ForWriting, True)
	f.Write msg & vbcrlf
	f.Close
end function

function log(msg)
	Const ForReading = 1, ForWriting = 2, ForAppending = 8
	Dim fso, f
	Set fso = CreateObject("Scripting.FileSystemObject")
	Set f = fso.OpenTextFile(logName, ForAppending, True)
	f.Write msg & vbcrlf
	f.Close
end function


'-----------------------------------------------------------------------------
'
' generic write to file
'
'-----------------------------------------------------------------------------
function writeToFile(fileName, msg)
	Const ForReading = 1, ForWriting = 2, ForAppending = 8
	Dim fso, f
	Set fso = CreateObject("Scripting.FileSystemObject")
	Set f = fso.OpenTextFile(fileName, ForWriting, True)
	f.Write msg '& vbcrlf
	f.Close
end function
'-----------------------------------------------------------------------------
'
' generic send mail function
'
'-----------------------------------------------------------------------------
function SendEmail(subject, msg, options, toEmail, toName, fromEmail, fromName, attachments)
	dim mailComponent, mailServer, isHTML, isQuiet, isLog, obj
	dim result
	result = ""
	
	if(true)then
		' options is a string of options separated by commas
		' options can be: "html,quiet,log,print"
		' if fromName or toName is blank then the email address only will be used
		' fromEmail & fromName default to globals websiteSenderEmail & websiteSenderName
		' toEmail, toName, attachments can arrays or strings
		' if strings they can be single values or crlf separated lists
		' toEmail can be a recordset containing "Email", "Name" and any other merge fields
		
		'if not isarray(toEmail) then
		'	toEmail = split(toEmail, vbcrlf)
		'end if
		'if not isarray(toName) then
		'	toName = split(toName, vbcrlf)
		'end if
		if isObject(toEmail) then
			do while not toEmail.eof
				' todo: log where up to each time
				SendEmail subject, msg, options, rs("Email"), rs("Name"), fromEmail, fromName, attachments
				toEmail.movenext
			loop
		end if
		
		' default from address
		if fromEmail="" then
			fromEmail = websiteSenderEmail
			if fromName="" then
				fromName = websiteSenderName
			end if
		end if
		if fromName="" then
			fromName = fromEmail
		end if
		
		' determine options
		if instr(options, "html")>0 then
			isHTML = true
		end if
		if instr(options, "quiet")>0 then
			isQuiet = true
		end if
		if instr(options, "log")>0 then
			isLog = true
		end if
		

		' send mail

		set obj = CreateObject("JMail.SMTPMail")
		if isQuiet then
			obj.silent = true
		end if
		if isHTML then
			obj.ContentType = "text/html"
		end if

		obj.ServerAddress = "smtp.xtra.co.nz"

		obj.Sender = fromEmail
		if fromName<>fromEmail then
			obj.SenderName = fromName
		end if
		obj.AddRecipient toEmail
		
		obj.Subject = subject
		obj.Body = msg
		''''obj.AddHeader "Originating-IP", Request.ServerVariables("REMOTE_ADDR")
		
		if true then
			log "Mail NOW"
			if not obj.Execute then
				result = obj.errormessage
			end if
		else
			log "REALLY skipped sending email"
			
		end if
		set obj = nothing

	else
		log "skipped sending email"
	end if
	SendEmail = result
end function



'-------------------------------------------------------------------------------
'-------------------------------------------------------------------------------
function EncryptID(id)
	randomize timer
	if id<>"" then
		EncryptID = (clng(id) * 8 + 909434) / 2
		EncryptID = int(rnd * 9) & int(rnd * 9) & EncryptID & int(rnd * 9) & int(rnd * 9) & int(rnd * 9) & int(rnd * 9) & chr(65 + rnd * 25) & chr(65 + rnd * 25) & chr(65 + rnd * 25)
	else
		EncryptID = ""
	end if
end function
'-------------------------------------------------------------------------------

'-------------------------------------------------------------------------------
'-------------------------------------------------------------------------------
function DecryptID(id)
	if id<>"" then
		DecryptID = mid(id, 3, len(id)-9)
		DecryptID = (clng(DecryptID) * 2 - 909434) / 8
	else
		DecryptID = ""
	end if
end function

'-------------------------------------------------------------------------------
Function FetchValue(sql)
	dim rs

	Dim messageText
	messageText = ""
	log "fetchvalue["&sql&"]"
	'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
	On Error Resume Next

		'---------------------------------------------------------------------------
		'
		' the enclosed block is inclined to be able to cause an exception
		'
		'---------------------------------------------------------------------------

		set rs = db.execute(sql)
		'---------------------------------------------------------------------------
		'
		' the above block is inclined to be able to cause an exception
		'
		'---------------------------------------------------------------------------
		if (err.number<>0) then
			messageText = "There was a problem ["&err.number&"]["&err.description&"]"

			log "ERROR: fetch error["& messageText &"]"
			log "ERROR: sql["& sql &"]"

			Err.Clear			' Clear the error.
		end if
	On Error goto 0
	'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
	if(messageText="" and isObject(rs))then
		if rs.eof then
			FetchValue = null
			log "fetchvalue no result"
		else
			FetchValue = rs(0)
			log "fetchvalue result["&rs(0)&"]"
		end if
		rs.close
	end if
	log "	end fetch ["&FetchValue&"]"
	set rs = nothing
End Function
'-------------------------------------------------------------------------------


