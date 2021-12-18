// Photon:
// "public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)"
// erlaubt das Synchronisieren von Stats. Wenn der Spieler meinem Client gehört schicke ich Daten, wenn der Spieler wem anders gehört empfange ich Daten.
// Das sorgt dafür dass man in Unity nicht die Leben von anderen Spielern verändern kann.
// Die Funktion überträgt Werte nur wenn sie verändert sind mit einer Rate von ca 20/s

// RPCs sind Methoden, die, wenn sie aufgerufen werden, alle Clients über eine Änderung benachrichtigen
// So kann man die Leben von anderen verändern!