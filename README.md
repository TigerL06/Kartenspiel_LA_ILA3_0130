# Kartenspiel_LA_ILA3_0130

### Gruppe:

- Cedric Tuma
- Liam Koch
- Robin Sacher
- Damian Müller
- Nikola Manojlovic

| Datum      | Version | Zusammenfassung                                                                                                                                 |
| ---------- | ------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| 10.01.2025 | 0.0.1   | Heute haben wir eine unsere Projektidee erarbeitet und den Projektantrag ausgefüllt, sowie abgeben und mit der Projektdokumentation angefangen. |
| 17.01.2025 | 0.2.0   | Heute haben wir die Informieren- und Planenphase abgeschlossen.                                                                                 |
| 24.01.2025 | 0.3.0   |                                                                                                                                                 |
| 31.01.2025 | 0.4.0   |                                                                                                                                                 |
| 21.02.2025 | 0.5.0   |                                                                                                                                                 |
| 28.02.2025 | 0.6.0   |                                                                                                                                                 |
| 07.03.2025 | 1.0.0   |                                                                                                                                                 |

## 1 Informieren

### 1.1 Anforderungen

Wir entwickeln ein Mehrspieler-Karten-Spiel, das es Spielern ermöglicht, sich in einer Lobby zu verbinden und eine Runde Karten nach den klassischen Regeln zu spielen.

In diesem Projekt setzen wir die klassische Karten-Spielmechanik in einer Webanwendung um, bei der mehrere Spieler über eine Online-Verbindung in Echtzeit interagieren können. Unser Ziel ist es, eine benutzerfreundliche Plattform zu schaffen, die Echtzeit-Updates und eine korrekte Regelumsetzung ermöglicht. Dabei lernen wir, ein Backend mit sauberer Architektur zu entwerfen, REST-APIs zu entwickeln, MongoDB für Datenverwaltung einzusetzen und Frontend und Backend effizient zu verknüpfen. Zudem möchten wir unser Verständnis von Programmierprinzipien wie Modularität, Validierung und Fehlerbehandlung vertiefen.


| A-№ | Verbindlichkeit | Typ           | Beschreibung                                                                                                                                                     |
| ---- | --------------- | ------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1    | Muss            | Randbedingung | Wir verwenden mindestens .Net Version 7.0 oder höher verewendet wird.                           |
| 2    | Muss            | Randbedingung | Das Frontend wird mit HTML, CSS und Java Script umgesetzt wird.                          |
| 3    | Muss            | Randbedingung | Als zentrale Datenbank wird MongoDB Atlas verwendet wird.                     |
| 4    | Muss            | Qualität      | Die Webseite wird als Single Page Application umgesetzt wird.                                     |
| 5    | Muss            | Funktional    | Ich kann eine Gruppe/ Raum erstellen um zu spielen.               |
| 6    | Muss            | Funktional    | Ich kann mir einen Benutzernamen geben.                    |
| 7    | Muss            | Funktional    | Man kann das Spiel in einer Gruppe/ Raum spielen.                                               |
| 8    | Muss            | Funktional    | Die Daten werden in einer MongoDB Datenbank gespeichert.       |
| 9    | Muss            | Funktional    | Das Frontend holt die Daten für das Spiel über das Backend.                            |
| 10   | Muss            | Funktional    | Beim Starten des Spiels werden die Karten ausgeteilt.                                |
| 11   | Muss            | Funktional    | Man kann nur Karten legen die von den Regeln erlaubt werden. |
| 12   | Muss            | Funktional    | Der erste Spieler ohne Karte gewinnt das Spiel.            |
| 13   | Muss            | Funktional    | Der Spielstand und die Züge der Spieler werden gespeichert.     |
| 14   | Muss            | Funktional    | Die Webseite ist für das Spielen online zugänglich.                  |

✍️ Jede User Story hat eine ganzzahlige Nummer (1, 2, 3 etc.), eine Verbindlichkeit (Muss oder Kann?), und einen Typ (Funktional, Qualität, Rand). Die User Story selber hat folgende Form: _Als ein 🤷‍♂️ möchte ich 🤷‍♂️, damit 🤷‍♂️_.

### 1.3 Testfälle

| TC-№ | Ausgangslage                                                                          | Eingabe                                                                                       | Erwartete Ausgabe                                                                                                          |
| ---- | ------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------- |
| 1.1  | Das Backend ist gestartet                                                             | Überprüfung der .NET-Version                                                                  | Die .NET-Version ist 7 oder höher.                                                                                         |
| 2.1  | Frontend gestartet und im Browser geladen                                             | Überprüfung desQuellcodes und Benutzeroberfläche                                              | Das Frontend wurde mit HTML, CSS und JavaScript erstellt.                                                                  |
| 3.1  | Eine Verbindung zur Datenbank ist erforderlich                                        | Überprüfung der Datenbankkonfiguration und Verbindungsdetailss                                | MongoDB Atlas wird als zentrale Datenbank verwendet.                                                                       |
| 4.1  | Das Frontend ist gestartet                                                            | Navigation zwischen den verschiedenen BEreichen der Website                                   | Nur die DOM wird akutalisiert, die URL bleibt unverändert.                                                                 |
| 5.1  | Der Benutzer ist auf der Website angemeldet                                           | Klick auf den Button "Gruppe/Raum erstellen"                                                  | Eine neue UNO-Spielgruppe oder ein Raum wird erstellt, und der Benutzer erhält die Möglichkeit, andere Spieler einzuladen. |
| 6.1  | Das Frontend ist gestartet und der Benutzer befindet sich auf der Registrierungsseite | Der Benutzer gibt einen Benutzernamen in das entsprechende Feld ein und speichert die Eingabe | Der Benutzername wird erfolgreich gespeichert und im Spiel angezeigt.                                                      |
| 7.1  | Der Benutzer befindet sich in einer Gruppe/Raum und das Spiel ist gestartet           | Der Benutzer wählt Karten aus und spielt sie nach den Spielregeln von UNO                     | Der Spielzug wird korrekt ausgeführt, die Kartenanzahl wird aktualisiert, und der nächste Spieler ist an der Reihe.        |
| 8.1 | Ein laufender MongoDB-Server ist verfügbar | Spielinformationen werden über das Backend gesendet| Daten werden in der MongoDB Datenbank gespeichert |
| 9.1 | Ein laufendes Backend mit einer Verbindung zur MongoDB | Anfrage von frontend für Spieldaten | Backend liefert angefragte Spieldaten |
| 10.1 | Spiel wird gestartet | Klick auf "Spiel starten" | Benutzer erhält Karten |
| 11.1 | Benutzer ist im laufenden Spiel und hat Karten auf der Hand | Benutzer versucht, eine Karte zu legen, die den Regeln entspricht | Karte wird gespielt |
| 11.2 | Benutzer ist im laufenden Spiel und hat Karten auf der Hand | Benutzer versucht eine Karte zu spielen die nicht den Regeln entspricht | Karte wird nicht gespielt und es gibt eine Fehlermeldung |
| 12.1 | Spiel ist gestartet | Der erste Spieler legt seine letzte Karte | Spieler wird als Gewinner angezeigt |
| 13.1 | Spiel ist gestartet | Ein Zug wird abgeschlossen | Der aktuelle Spielstand inklusive aller bisherigen Züge wird in der MongoDB gespeichert |
| 14.1 | Die Webseite ist vollständig implementiert | Ein Benutzer greift über einen Browser auf die Webseite zu | Die Webseite wird erfolgreich geladen und ist spielbar |

✍️ Die Nummer hat das Format `N.m`, wobei `N` die Nummer der User Story ist, die der Testfall abdeckt, und `m` von `1` an nach oben gezählt. Beispiel: Der dritte Testfall, der die zweite User Story abdeckt, hat also die Nummer `2.3`.

### 1.4 Diagramme

✍️Fügen Sie hier ein Use Case-Diagramm mit mindestens 3 Anwendungsfällen ein; und eine Skizze davon, wie Ihre Netzseite aussehen sollte.

## 2 Planen

| AP-№ | Frist | Zuständig | Beschreibung | geplante Zeit |
| ---- | ----- | --------- | ------------ | ------------- |
| 1  |   24.01.2025    |     Team      |      Festlegen, dass mindestens .NET Version 7.0 oder höher verwendet wird, um die Verwendung einer aktuellen Version sicherzustellen.        |      45 Min       |
| 2  |   24.01.2025    |     Team      |        Das Frontend wird mit HTML, CSS und Java Script umgesetzt wird.        |      15 min         |
| 3  |   24.01.2025    |     Koch, Sacher      |      Als zentrale Datenbank wird MongoDB Atlas verwendet wird.         |       15 min        |
| 4  |   24.01.2025	   |     Müller      |      Die Webseite wird als Single Page Application umgesetzt wird.        |       60 min        |
| 5  |   31.01.2025    |     Manojlovic, Tuma      |   Ich kann eine Gruppe/ Raum erstellen um zu spielen.  |       180 min        |
| 6  |   31.01.2025    |     Müller      |       Ich kann mir einen Benutzernamen geben.       |       90 min        |
| 7  |   31.01.2025    |     Manojlovc, Müller, Tuma      |       Man kann das Spiel in einer Gruppe/ Raum spielen.        |       180 min        |
| 8  |   31.01.2025    |     Koch, Sacher      |       Die Daten werden in einer MongoDB Datenbank gespeichert.       |      180 min         |
| 9  |    21.02.2025   |     Koch, Sacher      |       Das Frontend holt die Daten für das Spiel über das Backend.       |       120 min        |
| 10  |   21.02.2025    |    Tuma       |       Beim Starten des Spiels werden die Karten ausgeteilt.       |      90 min         |
| 11  |   21.02.2025    |    Manojlovic, Tuma       |      Man kann nur Karten legen die von den Regeln erlaubt werden.        |       60 min        |
| 12  |   28.02.2025    |    Manojlovic       |       Der erste Spieler ohne Karte gewinnt das Spiel.       |       60 min        |
| 13  |   28.02.2025    |    Müller       |       Der Spielstand und die Züge der Spieler werden gespeichert.       |       90 min        |
| 14  |   28.02.2025    |    Team      |       Die Webseite ist für das Spielen online zugänglich.       |        30 min       |

Total:

✍️ Die Nummer hat das Format `N.m`, wobei `N` die Nummer der User Story ist, auf die sich das Arbeitspaket bezieht, und `m` von `A` an nach oben buchstabiert. Beispiel: Das dritte Arbeitspaket, das die zweite User Story betrifft, hat also die Nummer `2.C`.

✍️ Ein Arbeitspaket sollte etwa 45' für eine Person in Anspruch nehmen. Die totale Anzahl Arbeitspakete sollte etwa Folgendem entsprechen: `Anzahl R-Sitzungen` ╳ `Anzahl Gruppenmitglieder` ╳ `4`. Wenn Sie also zu dritt an einem Projekt arbeiten, für welches zwei R-Sitzungen geplant sind, sollten Sie auf `2` ╳ `3` ╳`4` = `24` Arbeitspakete kommen. Sollten Sie merken, dass Sie hier nicht genügend Arbeitspakte haben, denken Sie sich weitere "Kann"-User Stories für Kapitel 1.2 aus.

## 3 Entscheiden

✍️ Dokumentieren Sie hier Ihre Entscheidungen und Annahmen, die Sie im Bezug auf Ihre User Stories und die Implementierung getroffen haben.

## 4 Realisieren

| AP-№ | Datum | Zuständig | geplante Zeit | tatsächliche Zeit |
| ---- | ----- | --------- | ------------- | ----------------- |
| 1.A  |       |           |               |                   |
| ...  |       |           |               |                   |

✍️ Tragen Sie jedes Mal, wenn Sie ein Arbeitspaket abschließen, hier ein, wie lang Sie effektiv dafür hatten.

## 5 Kontrollieren

| TC-№ | Datum | Resultat | Tester |
| ---- | ----- | -------- | ------ |
| 1.1  |       |          |        |
| ...  |       |          |        |

✍️ Vergessen Sie nicht, ein Fazit hinzuzufügen, welches das Test-Ergebnis einordnet.

## 6 Auswerten

✍️ Fügen Sie hier eine Verknüpfung zu Ihrem Lern-Bericht ein.
