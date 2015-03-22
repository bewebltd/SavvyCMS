set fs = createobject("scripting.filesystemobject") 
set shell = CreateObject("WScript.Shell")

filename = lcase(wscript.arguments.item(0))
solutionPath = lcase(wscript.arguments.item(1))
if right(solutionPath,2)="\\" then
	solutionPath = left(solutionPath, len(solutionPath)-1)
end if
libFilename = ""

' find library path
libPath = "c:\dev\codelibMVC\"
if not fs.folderexists(libPath) then
	libPath = "c:\data\projects\codelibMVC\"
	if not fs.folderexists(libPath) then
		libPath = "c:\data\dev\projects\codelibMVC\"
		if not fs.folderexists(libPath) then
			libPath = "c:\data\dev\web\codelibMVC\"  'jeremys machine
			if not fs.folderexists(libPath) then
				libPath = "f:\dev\codelibMVC\"  ' mikes
				if not fs.folderexists(libPath) then
					msgbox "Could not find codelibMVC on C drive"
				end if
			end if
		end if
	end if
end if
if right(libPath,1) = "\" then
	libPath = left(libPath,len(libPath)-1)
end if

if right(filename,4)=".asp" then
	' find classic ASP codelib instead yeah
	libPath = "c:\dev\codelibClassic\"
	if not fs.folderexists(libPath) then
		libPath = "c:\data\projects\codelibClassic\"
		if not fs.folderexists(libPath) then
			libPath = "c:\data\dev\projects\codelibClassic\"
			if not fs.folderexists(libPath) then
				libPath = "c:\data\dev\web\codelibClassic\"  'jeremys machine
				if not fs.folderexists(libPath) then
					libPath = "f:\dev\codelibClassic\"  ' mikes
					if not fs.folderexists(libPath) then
						msgbox "Could not find codelibClassic on C drive"
					end if
				end if
			end if
		end if
	end if
	if right(libPath,1) = "\" then
		libPath = left(libPath,len(libPath)-1)
	end if

	if not fs.folderexists(libPath) then
		msgbox "Could not find Codelib Classic. Note that it is now in SVN."
	end if
	'libPath = "w:\codelib\latest"
	'modulesPath = "w:\codelib\modules"
	'if not fs.folderexists(libPath) then
	'	libPath = "\\sam4\_proj\codelib\latest"
	'	modulesPath = "\\sam4\_proj\codelib\modules"
	'	if not fs.folderexists(libPath) then
	'		msgbox "Could not find codelib"
	'	end if
	'end if

	' find equivalent codelib file
	if EndsWith(filename,"\admin\newsletter") then
		libFilename = libPath & "\admin\email"
	elseif EndsWith(filename,"\newsletter") then
		libFilename = libPath & "\newsletter"
	elseif EndsWith(filename,"\admin") then
		libFilename = libPath & "\admin"
	elseif EndsWith(filename,"\js") then
		libFilename = libPath & "\js"
	elseif EndsWith(filename,"apps\beweb\common\includes") then
		libFilename = libPath & "\admin\includes"

	elseif instr(filename,"\admin\includes\")>0 then
		pos = instr(filename,"\admin\includes\")
		libFilename = libPath & mid(filename, pos)
	elseif instr(filename,"\js\")>0 then
		pos = instr(filename,"\js\")
		libFilename = libPath & mid(filename, pos)
	elseif instr(filename,"apps\beweb\common\includes\")>0 then
		pos = instr(filename,"\common") + 7
		libFilename = libPath & "\admin" & mid(filename, pos)
	elseif instr(filename,"\includes\")>0 then
		pos = instr(filename,"\includes\")
		libFilename = libPath & "\admin" & mid(filename, pos)
	elseif instr(filename,"\admin\")>0 then
		pos = instr(filename,"\admin\")
		libFilename = libPath & mid(filename, pos)
	elseif instr(filename,"united_travel\pod")>0 then
		pos = instr(filename,"\pod") + 4
		libFilename = libPath & "\admin" & mid(filename, pos)
	elseif instr(filename,"apps\beweb\common\template")>0 then
		pos = instr(filename,"\common") + 7
		libFilename = libPath & "\admin" & mid(filename, pos)
	elseif instr(filename,"apps\beweb\common")>0 then
		pos = instr(filename,"\common") + 7
		libFilename = libPath & "\admin\includes" & mid(filename, pos)
	elseif instr(filename,"\newsletter\")>0 then
		pos = instr(filename,"\newsletter\")
		libFilename = libPath & mid(filename, pos)
	elseif instr(filename,"\chat")>0 then
		pos = instrrev(filename,"\chat")
		libFilename = modulesPath & "\chat"  & mid(filename, pos)
	else
		pos = instrRev(filename,"\")
		libFilename = libPath & "\" & mid(filename, pos)
	end if
else
	' assume codelibMVC

	'msgbox "filename=" & filename
	'msgbox "solutionPath=" & solutionPath
	'msgbox "libPath=" & libPath

	' find equivalent codelib file
	relativeFilename = replace(filename&"", solutionPath, "\")
	if(instr(relativeFilename,"\mvc\"))then
		' allow whole project to be in a subfolder called MVC
		relativeFilename = replace(relativeFilename, "\mvc\", "\")
	end if
	if(instr(relativeFilename,"app_code\beweb"))then
		relativeFilename = replace(relativeFilename, "app_code\beweb", "bewebcore\beweb")
	end if
	if(instr(relativeFilename,"beweb-cma.js"))then
		relativeFilename = "\js\bewebcore\beweb-cma.js"
	end if
	if(instr(relativeFilename,"Beweb.jquery.validate.js"))then
		relativeFilename = "\js\bewebcore\Beweb.jquery.validate.js"
	end if
	if(instr(relativeFilename,"\forms.js"))then
		relativeFilename = "\js\bewebcore\forms.js"
	end if
	libFilename = libPath & relativeFilename
end if

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

' see if it exists
if fs.fileexists(libFilename) then
	rval = shell.run("""" & beyondComparePath & """ """ & filename & """  """	& libFilename & """", 1, false)
elseif fs.folderexists(libFilename) then
	rval = shell.run("""" & beyondComparePath & """ """ & filename & """  """	& libFilename & """", 1, false)
else
	' could not find file or folder

	' try matching to a template
	if EndsWith(filename,"edit.asp") then
		libFilename = libPath & "\admin\template\dbedit_template.asp"
	elseif EndsWith(filename,"editchildren.asp") then
		libFilename = libPath & "\admin\template\dbeditchildren_template.asp"
	elseif EndsWith(filename,"editchildren-inc.asp") then
		libFilename = libPath & "\admin\template\dbeditchildren-inc_template.asp"
	elseif EndsWith(filename,"list.asp") then
		libFilename = libPath & "\admin\template\dblist_template.asp"
	end if

	if fs.fileexists(libFilename) then
		msgbox "Could not find matching file. Found template [" & libFilename & "]"	
	else
		msgbox "Could not find file in codelibMVC [" & filename & "][" & libFilename & "]"
	end if
	rval = shell.run("""" & beyondComparePath & """ """ & filename & """  """	& libFilename & """", 1, false)
end if


function EndsWith(str, substr)
	EndsWith = right(str,len(substr)) = substr
end function


function StartsWith(str, substr)
	StartsWith = left(str,len(substr)) = substr
end function
