set fs = createobject("scripting.filesystemobject") 
set shell = CreateObject("WScript.Shell")

' eg params should be: $(SolutionDir) DEV LVE

solutionPath = lcase(wscript.arguments.item(0))      ' $(SolutionDir)
serverStageLeft = ucase(wscript.arguments.item(1))   ' eg DEV
serverStageRight = ucase(wscript.arguments.item(2))  ' eg LVE

'msgbox "solutionPath=" & solutionPath

' find bc
beyondComparePath = "C:\Program Files (x86)\Beyond Compare 4\bcompare.exe"
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
leftfolder = GetConfigSetting("WebsiteBaseUrl" & serverStageLeft) & "admin/tools/structure.aspx"
rightfolder = GetConfigSetting("WebsiteBaseUrl" & serverStageRight) & "admin/tools/structure.aspx"

' launch BC
filters = ""
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
