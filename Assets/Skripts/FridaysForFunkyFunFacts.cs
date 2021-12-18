// Photon:
// "public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)"
// erlaubt das Synchronisieren von Stats. Wenn der Spieler meinem Client geh�rt schicke ich Daten, wenn der Spieler wem anders geh�rt empfange ich Daten.
// Das sorgt daf�r dass man in Unity nicht die Leben von anderen Spielern ver�ndern kann.
// Die Funktion �bertr�gt Werte nur wenn sie ver�ndert sind mit einer Rate von ca 20/s

// RPCs sind Methoden, die, wenn sie aufgerufen werden, alle Clients �ber eine �nderung benachrichtigen
// So kann man die Leben von anderen ver�ndern!