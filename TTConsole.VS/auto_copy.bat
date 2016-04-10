::自动拷贝VS项目编译dll到指定目录
::by chiuanwei

::protected check running need params
if {%1} == {} (
	echo ### dont running without params.
	pause
	exit
)

if {%2} == {} (
	echo ### dont running without params.
	pause
	exit
)

if {%3} == {} (
	echo ### dont running without params.
	pause
	exit
)

if {%4} == {} (
	echo ### dont running without params.
	pause
	exit
)

:: 首先传 文件名、目标导出目录、方案名称Release|Debug

set TARGET_NAME=%1
set TARGET_FOLDER=%2
set SOLUTION_NAME=%3
set MOD_FOLDER=%4
set MOD_FILE=%MOD_FOLDER%%TARGET_NAME%.%SOLUTION_NAME%.UnityFolder.txt

echo mod file= %MOD_FILE%

if exist %MOD_FILE% (echo mod file exist) else (
	echo ### mod file dont exist...
	pause
	exit
)

echo target name = %1
echo target folder = %2
echo solution name = %3
echo mods folder = %4

echo found the project.solution.mods copyto target folders.

::get params
::set DLL_PATH=%1
::if {%2} == {} (
::	echo ### Not Input The Target Unity Folder Will Put Into \Plugins
::	set DLL_FOLDER=Plugins
::) ^
::else (
::	set DLL_FOLDER=%2
::)

::Copy the %DLL_PATH% dll into Unity Project Folder ###

::into directory
cd /d %~dp0
echo ### Folder Root= %cd%

::Get UNITY_FOLDER in config.txt
::perline per project root
::and do auto copy.
setlocal enabledelayedexpansion
for /f "delims==" %%i in (%MOD_FILE%) do (
	if exist %%i (
		echo ### Copy to %%i\
		if %SOLUTION_NAME% == Debug (
			copy %TARGET_FOLDER%\%TARGET_NAME%.dll %%i\%TARGET_NAME%.dll
			copy %TARGET_FOLDER%\%TARGET_NAME%.pdb %%i\%TARGET_NAME%.pdb
		) ^
		else (
			copy %TARGET_FOLDER%\%TARGET_NAME%.dll %%i\%TARGET_NAME%.dll
		) ^
	) ^
	else (
        if exist %MOD_FOLDER%%%i (
            echo ### Copy to %MOD_FOLDER%%%i\
            if %SOLUTION_NAME% == Debug (
                copy %TARGET_FOLDER%\%TARGET_NAME%.dll %MOD_FOLDER%%%i\%TARGET_NAME%.dll
                copy %TARGET_FOLDER%\%TARGET_NAME%.pdb %MOD_FOLDER%%%i\%TARGET_NAME%.pdb
            ) ^
            else (
                copy %TARGET_FOLDER%\%TARGET_NAME%.dll %MOD_FOLDER%%%i\%TARGET_NAME%.dll
            ) ^
        ) ^
        else (
		    echo ### Cant Copy to Not Exist Folder: %%i\
        )
	)
)
