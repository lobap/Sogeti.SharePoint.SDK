Function WriteHeading ($HeadingText)
{
	Write-Host
	Write-host "[" -foregroundcolor "DarkGray" -NoNewLine
	Write-host "[" -foregroundcolor "Gray" -NoNewLine
	Write-host "[" -foregroundcolor "White" -NoNewLine
	Write-host $HeadingText -foregroundcolor "green" -NoNewLine
	Write-host "]" -foregroundcolor "White" -NoNewLine
	Write-host "]" -foregroundcolor "Gray" -NoNewLine
	Write-host "]" -foregroundcolor "DarkGray"
	Write-Host
}
Function WriteError ($ErrText)
{
	Write-Host
	Write-host "[" -foregroundcolor "DarkGray" -NoNewLine
	Write-host "[" -foregroundcolor "Gray" -NoNewLine
	Write-host "[" -foregroundcolor "White" -NoNewLine
	Write-host $ErrText -foregroundcolor "red" -NoNewLine
	Write-host "]" -foregroundcolor "White" -NoNewLine
	Write-host "]" -foregroundcolor "Gray" -NoNewLine
	Write-host "]" -foregroundcolor "DarkGray"
	Write-Host
}

Function WriteSuccess
{
	Write-Host ": " -foregroundcolor "Gray" -NoNewLine
	Write-Host Success -foregroundcolor "Green"
}
