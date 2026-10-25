; Inno Setup Localization file format
;
; Sections are in this order: LangOptions, Languages, Messages, CustomMessages
; Section headers are case-sensitive and should not be altered
; Sections are separated by two lines consisting of ";" (the comment character) to give visual separation in the file
; Items in the  LangOptions, Languages, and CustomMessages sections are ordered with "en" first, 
; then the other languages in alphabetical order by language code
; Messages in the CustomMessages sections are sorted by key
; Line endings are CRLF
; File encoding is UTF-8 with BOM
;
;
[LangOptions]
DialogFontSize=9
DialogFontName="Segoe UI"
WelcomeFontSize=14
WelcomeFontName="Segoe UI"
;
;
[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "ca"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"
Name: "it"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "ko"; MessagesFile: "compiler:Languages\Korean.isl"
Name: "nl"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "pt"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "sk"; MessagesFile: "compiler:Languages\Slovak.isl"
;
;
[Messages]
en.SetupWindowTitle = Setup - {#MyAppName} {#MyAppVersion}
ca.SetupWindowTitle = Instal·lar - {#MyAppName} {#MyAppVersion}
de.SetupWindowTitle = Installieren - {#MyAppName} {#MyAppVersion}
es.SetupWindowTitle = Instalar - {#MyAppName} {#MyAppVersion}
fr.SetupWindowTitle = Installer - {#MyAppName} {#MyAppVersion}
it.SetupWindowTitle = Installazione di {#MyAppName} {#MyAppVersion}
ko.SetupWindowTitle = 설치 - {#MyAppName} {#MyAppVersion}
nl.SetupWindowTitle = Setup - {#MyAppName} {#MyAppVersion}
pt.SetupWindowTitle = Instalação do {#MyAppName} {#MyAppVersion}
sk.SetupWindowTitle = Sprievodca inštaláciou - {#MyAppName} {#MyAppVersion}
;
;
[CustomMessages]
en.AppIsRunning=is running, please close it to continue with the installation.
en.DeleteConfigFiles=Do you want to remove the settings and history files and registry entries?%n%nSelect 'No' if you plan to reinstall the program.
en.NotSelfContained64=This will install the framework-dependent x64 (64-bit) version of %1 version %2.%n%nThis version requires an existing installation of .NET 10 Desktop Runtime.%n%nIt is recommended that you close all other applications before continuing.%n%nClick 'Next' to continue, or 'Cancel' to exit Setup.
en.SelfContainedx64=This will install the self-contained x64 (64-bit) version of %1 version %2.%n%nIt is recommended that you close all other applications before continuing.%n%nClick 'Next' to continue, or 'Cancel' to exit Setup.
en.ViewReadme=View the ReadMe file
;
;
ca.AppIsRunning=està en execució, tanqueu-lo per continuar amb la instal·lació.
ca.DeleteConfigFiles=Voleu eliminar els fitxers de configuració, l'historial i les entrades del registre?%n%nSeleccioneu "No" si teniu previst reinstal·lar el programa.
ca.NotSelfContained64=Això instal·larà la versió x64 (64 bits) dependent del framework de %1 versió %2.%n%nAquesta versió requereix una instal·lació existent de .NET 10 Desktop Runtime.%n%nEs recomana que tanqueu totes les altres aplicacions abans de continuar.%n%nFeu clic a "Següent" per continuar o a "Cancel·la" per sortir del programa d'instal·lació.
ca.SelfContainedx64=Això instal·larà la versió autònoma x64 (64 bits) de %1 versió %2.%n%nEs recomana tancar totes les altres aplicacions abans de continuar.%n%nFeu clic a "Següent" per continuar o a "Cancel·la" per sortir del programa d'instal·lació.
ca.ViewReadme=Veure el fitxer ReadMe
;
;
de.AppIsRunning=ausgeführt wird, schließen Sie es bitte, um mit der Installation fortzufahren.
de.DeleteConfigFiles=Möchten Sie die Einstellungs- und Verlaufsdateien sowie die Registrierungseinträge entfernen?%n%nWählen Sie „Nein“, wenn Sie das Programm neu installieren möchten.
de.NotSelfContained64=Dadurch wird die frameworkabhängige x64-Version (64-Bit) von %1 Version %2 installiert.%n%nDiese Version erfordert eine bestehende Installation der .NET 10 Desktop Runtime.%n%nEs wird empfohlen, alle anderen Anwendungen vor dem Fortfahren zu schließen.%n%nKlicken Sie auf „Weiter“, um fortzufahren, oder auf „Abbrechen“, um die Installation zu beenden.
de.SelfContainedx64=Dadurch wird die eigenständige x64-Version (64-Bit) von %1 Version %2 installiert. Es wird empfohlen, vor dem Fortfahren alle anderen Anwendungen zu schließen. Klicken Sie auf „Weiter“, um fortzufahren, oder auf „Abbrechen“, um die Installation zu beenden.
de.ViewReadme=Lesen Sie die ReadMe-Datei
;
;
es.AppIsRunning=se está ejecutando, por favor ciérrelo para continuar con la instalación.
es.DeleteConfigFiles=¿Desea eliminar la configuración y los archivos de historial y entradas del registro?%n%nSeleccione 'No' si planea reinstalar el programa.
es.NotSelfContained64=Esto instalará la versión x64 (64 bits) dependiente del marco de %1 versión %2.%n%nEsta versión requiere una instalación existente de .NET 10 Desktop Runtime.%n%nSe recomienda cerrar todas las demás aplicaciones antes de continuar.%n%nHaga clic en 'Siguiente' para continuar o en 'Cancelar' para salir de la configuración.
es.SelfContainedx64=Esto instalará la versión x64 (64 bits) independiente portátil de %1 versión %2.%n%nSe recomienda cerrar todas las demás aplicaciones antes de continuar.%n%nHaga clic en 'Siguiente' para continuar o en 'Cancelar' para salir de la configuración.
es.ViewReadme=Abrir el archivo Léame
;
;
fr.AppIsRunning=est en cours d'exécution, veuillez le fermer pour poursuivre l'installation.
fr.DeleteConfigFiles=Voulez-vous supprimer les fichiers de paramètres et d'historique ainsi que les entrées de registre ?%n%nSélectionnez « Non » si vous prévoyez de réinstaller le programme.
fr.NotSelfContained64=Cela installera la version x64 (64 bits) dépendante du framework de %1 version %2.%n%nCette version nécessite une installation existante de .NET 10 Desktop Runtime.%n%nIl est recommandé de fermer toutes les autres applications avant de continuer.%n%nCliquez sur « Suivant » pour continuer ou sur « Annuler » pour quitter l’installation.
fr.SelfContainedx64=Ceci installera la version x64 (64 bits) autonome de %1 version %2.%n%nIl est recommandé de fermer toutes les autres applications avant de continuer.%n%nCliquez sur « Suivant » pour continuer, ou sur « Annuler » pour quitter l'installation.
fr.ViewReadme=Voir le fichier ReadMe
;
;
it.AppIsRunning=è in esecuzione, per continuare l'installazione chiudi l'applicazione.
it.DeleteConfigFiles=Vuoi rimuovere le impostazioni, i file cronologia e le voci del registro?%n%nSe hai intenzione di reinstallare il programma seleziona 'No'.
it.NotSelfContained64=Verrà installata la versione x64 (64 bit) dipendente dal framework %1 %2.%n%nQuesta versione richiede che sia già installato .NET 10 Desktop Runtime.%n%nPrima di continuare l'installazione ti consigliamo di chiudere tutte le altre applicazioni.%n%nPer continuare seleziona 'Avanti' o 'Annulla' per uscire dall'installazione.
it.SelfContainedx64=Verrà installata la versione x64 (64 bit) standalone portatile di %1 %2.%n%nPrima di continuare l'installazione ti consigliamo di chiudere tutte le altre applicazioni.%n%nPer continuare seleziona 'Avanti' o 'Annulla' per uscire dall'installazione.
it.ViewReadme=Visualizza file 'ReadMe'
it.NameAndVersion=%1 %2
it.LaunchProgram=Esegui %1
it.AdditionalIcons=Collegamenti:
it.CreateDesktopIcon=Crea collegamento programma sul &desktop
it.CreateQuickLaunchIcon=Crea collegamento programma nella &barra 'Avvio veloce'
;
;
ko.AppIsRunning=가 실행 중입니다. 설치를 계속하려면 닫으세요.
ko.DeleteConfigFiles=설정 및 기록 파일과 레지스트리 항목을 제거하시겠습니까?%n%n프로그램을 다시 설치할 계획이면 '아니오'를 선택하세요.
ko.NotSelfContained64=이렇게 하면 %1 버전 %2의 프레임워크 종속 x64 (64비트) 버전이 설치됩니다.%n%n이 버전은 .NET 10 Desktop Runtime의 기존 설치가 필요합니다.%n%n계속하기 전에 다른 모든 응용 프로그램을 닫는 것이 좋습니다.%n%n계속하려면 '다음'을 클릭하거나 설치 프로그램을 종료하려면 '취소'를 클릭합니다.
ko.SelfContainedx64=이렇게 하면 %1 버전 %2의 독립 실행형 x64 (64비트) 버전이 설치됩니다.%n%n계속하기 전에 다른 모든 응용 프로그램을 닫으세요.%n%n계속하려면 '다음'을 클릭하거나 설치 프로그램을 종료하려면 '취소'를 클릭합니다.
ko.ViewReadme=ReadMe 파일 보기
;
;
nl.AppIsRunning=wordt uitgevoerd, sluit deze dan af om door te gaan met de installatie.
nl.DeleteConfigFiles=Wilt u de instellingen, gegevensbestanden en registerwaarden verwijderen?%n%nKies 'NEE' als u het programma opnieuw wilt installeren.
nl.NotSelfContained64=Hiermee wordt de x64 (64-bits) frameworkafhankelijke versie van %1 versie %2 geïnstalleerd.%n%nDeze versie vereist een bestaande installatie van .NET 10 Desktop Runtime.%n%nHet wordt aanbevolen om alle andere toepassingen te sluiten voordat u doorgaat.%n%nKlik op 'Volgende' om door te gaan of op 'Annuleren' om de installatie af te sluiten.
nl.SelfContainedx64=Hiermee wordt de draagbare x64 (64-bits) zelfstandige versie van %1 versie %2 geïnstalleerd.%n%nHet wordt aanbevolen om alle andere toepassingen te sluiten voordat u doorgaat.%n%nKlik op 'Volgende' om door te gaan of op 'Annuleren' om de installatie af te sluiten.
nl.ViewReadme=Open de ReadMe file
;
;
pt.AppIsRunning=está em execução, feche-o para continuar com a instalação.
pt.DeleteConfigFiles=Você deseja remover os arquivos de configurações?%n%nSelecione 'Não' se você planeja reinstalar.
pt.NotSelfContained64=Isso instalará a versão x64 (64 bits) dependente da estrutura %1 versão %2.%n%nEsta versão requer uma instalação existente do .NET 10 Desktop Runtime.%n%nRecomenda-se fechar todos os outros aplicativos antes de continuar.%n%nClique em 'Avançar' para continuar ou em 'Cancelar' para sair da Instalação.
pt.SelfContainedx64=Isso instalará a versão x64 (64 bits) independente do %1 versão %2.%n%nRecomenda-se fechar todos os outros aplicativos antes de continuar.%n%nClique em 'Avançar' para continuar ou em 'Cancelar' para sair da Instalação.
pt.ViewReadme=Ver o arquivo leiame
;
;
sk.AppIsRunning=beží, zatvorte ho, aby ste mohli pokračovať v inštalácii.
sk.DeleteConfigFiles=Chcete odstrániť súbory nastavení?%n%nAk plánujete preinštalovať, vyberte 'Nie'.
sk.NotSelfContained64=Týmto sa nainštaluje x64 (64-bitová) verzia %1 verzie %2 závislá od frameworku.%n%nTáto verzia vyžaduje existujúcu inštaláciu .NET 10 Desktop Runtime.%n%nPred pokračovaním sa odporúča zatvoriť všetky ostatné aplikácie.%n%nKliknite na tlačidlo „Ďalej“ pre pokračovanie alebo na tlačidlo „Zrušiť“ pre ukončenie inštalácie.
sk.SelfContainedx64=Týmto sa nainštaluje samostatná x64 (64-bitová) verzia %1 verzie %2.%n%nPred pokračovaním sa odporúča zatvoriť všetky ostatné aplikácie.%n%nKliknite na tlačidlo „Ďalej“ pre pokračovanie alebo na tlačidlo „Zrušiť“ pre ukončenie inštalácie.
sk.ViewReadme=Zobraziť súbor ReadMe
