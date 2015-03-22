set fs = createobject("scripting.filesystemobject") 
set shell = CreateObject("WScript.Shell")

solutionPath = lcase(wscript.arguments.item(0))
serverStage = ucase(wscript.arguments.item(1))

'msgbox "solutionPath=" & solutionPath

' find bc
beyondComparePath = "C:\Program Files (x86)\Beyond Compare 4\bcompare.exe"
if not fs.fileexists(beyondComparePath) then
	beyondComparePath = "C:\Program Files\Beyond Compare 4\bcompare.exe"
end if
if not fs.fileexists(beyondComparePath) then
	beyondComparePath = "C:\Program Files (x86)\Beyond Compare 3\bcompare.exe"
end if
if not fs.fileexists(beyondComparePath) then
	beyondComparePath = "C:\Program Files\Beyond Compare 3\bcompare.exe"
end if
if not fs.fileexists(beyondComparePath) then
	beyondComparePath = "d:\Program Files (x86)\Beyond Compare 3\bcompare.exe" 'jeremys machine, cdrive is ssd
end if
if not fs.fileexists(beyondComparePath) then
	beyondComparePath = "C:\Program Files\Beyond Compare 2\bc.exe"
end if
if not fs.fileexists(beyondComparePath) then
	msgbox "Could not find Beyond Compare executable. Do you have it installed and in the default location?"
end if

' find FTP details
if serverStage="STG" then
	rightfolder = GetConfigSetting("DeploySTG") 
else
	rightfolder = GetConfigSetting("DeployLVE")  
end if
extrafilters = GetConfigSetting("DeployExtraFilters")  

' launch BC
leftfolder = solutionPath
filters = "-.\WebWorkbench.mswwsettings;-.\*.docstates;-.\*.user;-.\*.suo;-.\*.tss;-.\admin\tools\comparefile2codelib.cmd;-.\admin\tools\comparefile2codelib.vbs;-.\admin\tools\CreateAdmin.aspx;-.\admin\tools\CreateAdmin.aspx.cs;-*.psd;-*.exclude;-.\attachments\;-.svn\;-.\_backup\;-.\_beweb\;-.\_ReSharper.*\;-.\admin\tools\dbcompare\;-.\admin\tools\template\;-.\Keystone\;-.\Visual Studio 2010\;-obj\;-.\CodeTemplates\;-.\SavvyMVC\bin\;-.\SavvyMVC\Properties\;-.\Properties\;-.\BewebCore\bin\;-.\BewebCore\Visual Studio 2010\;-.\BewebCore\Beweb\dll\;-.\BewebCore\Properties\;-obj\;-.\aspnet_client\" & ";" & extrafilters
rval = shell.run("""" & beyondComparePath & """ """ & leftfolder & """ """ & rightfolder & """ /filters=""" & filters & """", 1, false)


'---------------------

function GetConfigSetting(setting)
	dim configFile, xml, str
	configFile = solutionPath & "Web_AppSettings.config"
	if not(fs.fileexists(configFile)) then
		configFile = solutionPath & "MVC/Web_AppSettings.config"
	end if
	xml = GetFileContentsEx(configFile, false)
	str = ExtractTextBetween(xml, setting, "/>", true)
	str = ExtractTextBetween(str, "value=""", """", true)
	GetConfigSetting = str
end function

function GetFileContentsEx(path, isUnicode)
	' this function changed from GetFileContents due to possible name clashes
	dim f, str
	set f = fs.opentextfile(path,1,isUnicode)
	str = f.readall()
	f.close
	set f = nothing
	GetFileContentsEx = str
end function

function EndsWith(str, substr)
	EndsWith = right(str,len(substr)) = substr
end function


function StartsWith(str, substr)
	StartsWith = left(str,len(substr)) = substr
end function

function ExtractTextBetween(txt, textBefore, textAfter, throw)
	dim pos, str
	pos = instr(txt, textBefore)
	if pos > 0 then
		str = mid(txt, pos+len(textBefore))
		pos = instr(str, textAfter)
		if pos > 0 then
			str = left(str, pos-1)
		elseif throw then			
			eout "ExtractTextBetween could not find text after: " & textAfter
		else
			str = ""
		end if
	elseif throw then
		eout "ExtractTextBetween could not find text before: " & textBefore
	else
		str = ""
	end if
	ExtractTextBetween = str
end function


function eout(str)
	msgbox(str)
end function
