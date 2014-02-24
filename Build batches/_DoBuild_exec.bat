set buildMode=%1
set keepPDB=%2

::set internal vars
::form name of output folder
set outputFolderName=Output_%buildMode%
if %keepPDB% == 1 set outputFolderName=%outputFolderName%_WITHPDB

::DO BUILD
set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
MSBuild ..\Code\TrayGarden.sln /p:Configuration=%buildMode%

::copy result
if exist %outputFolderName% rd /s /q %outputFolderName%
md %outputFolderName%
copy ..\Code\TrayGarden\bin\%buildMode%\* %outputFolderName%\

::leave pdb if need
if NOT %keepPDB% == 1 del /q %outputFolderName%\*.pdb