:: Imposta la variabile d'ambiente per il percorso del progetto
set "PROJECT_PATH=C:\Users\Alessandro\source\repos\CartaAlta\CartaAlta"

:: Rimuove la directory publish-win64
rmdir /s /q "%PROJECT_PATH%\publish-win64"

:: Esegue il publish del progetto con dotnet
dotnet publish -c Release -r win-x64 -o "%PROJECT_PATH%\publish-win64"

:: Crea una directory per i nodi
mkdir "%PROJECT_PATH%\NODES"
mkdir "%PROJECT_PATH%\NODES\node1"
mkdir "%PROJECT_PATH%\NODES\node2"
mkdir "%PROJECT_PATH%\NODES\node3"
mkdir "%PROJECT_PATH%\NODES\node4"

:: Pulisce le directory dei nodi
rmdir /s /q "%PROJECT_PATH%\NODES\node1"
rmdir /s /q "%PROJECT_PATH%\NODES\node2"
rmdir /s /q "%PROJECT_PATH%\NODES\node3"
rmdir /s /q "%PROJECT_PATH%\NODES\node4"

:: Copia i file nell'ambiente dei nodi
xcopy /s /i "%PROJECT_PATH%\publish-win64\*" "%PROJECT_PATH%\NODES\node1"
xcopy /s /i "%PROJECT_PATH%\publish-win64\*" "%PROJECT_PATH%\NODES\node2"
xcopy /s /i "%PROJECT_PATH%\publish-win64\*" "%PROJECT_PATH%\NODES\node3"
xcopy /s /i "%PROJECT_PATH%\publish-win64\*" "%PROJECT_PATH%\NODES\node4"

:: Copia gli esempi di file di ambiente
copy "%PROJECT_PATH%\env-examples\.env1" "%PROJECT_PATH%\NODES\node1\.env"
copy "%PROJECT_PATH%\env-examples\.env2" "%PROJECT_PATH%\NODES\node2\.env"
copy "%PROJECT_PATH%\env-examples\.env3" "%PROJECT_PATH%\NODES\node3\.env"
copy "%PROJECT_PATH%\env-examples\.env4" "%PROJECT_PATH%\NODES\node4\.env"