set fs = createobject("scripting.filesystemobject") 
set shell = CreateObject("WScript.Shell")

path = "C:\data\jeremy\dev\web\bpy" 'no slash here
set baseFolder = fs.getfolder(path)

'fileList = "smark_park.css|stylesheet.css"
fileList = ""
for each file in baseFolder.files
	ext = lcase(fs.getextensionname(file.name))
	if ext="asp"	then
		newname = fs.getbasename(file.name) & ".aspx"
		' add to list
		if fileList<>"" then fileList = fileList & ","
		fileList = fileList & file.name & "|" & newname
		' rename
		'msgbox newname
		On Error Resume Next
			file.name = newname
			if (err.number<>0) then
				msgbox "There was a problem ["&err.number&"]["&err.description&"] newname["&newname&"]"
				Err.Clear			' Clear the error.
			end if
		On Error goto 0
	end if
next

fileList = split(fileList,",")

' go thru files opening each one and do replacements
for each record1 in fileList
	filename = split(record1,"|")(1)

	' read file
	'msgbox path & "\" & filename
	set s = fs.opentextfile(path & "\" & filename,1,false,false)	' 1=forreading
	txt = s.readall()
	s.close

	' replace text
	for each record in fileList
		rec = split(record,"|")
		txt = replace(txt, rec(0), rec(1))
	next

	' write file
	set s = fs.opentextfile(path & "\" & filename,2,false,false)	' 2=forwriting
	s.write(txt)
	s.close
next

msgbox "Done"

