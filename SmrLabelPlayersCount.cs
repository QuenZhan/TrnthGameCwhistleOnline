using UnityEngine;
using System.Collections;

public class SmrLabelPlayersCount : MonoBehaviour {
	public UILabel label;
	public enum Type{all,inRoom,inLobby}
	public Type type;
	public void refresh(){
		switch(type){
		case Type.all:
			label.text=PhotonNetwork.countOfPlayers+"";
			break;
		case Type.inRoom:
			label.text=PhotonNetwork.countOfPlayersInRooms+"";
			break;
		case Type.inLobby:
			label.text=PhotonNetwork.countOfPlayersOnMaster+"";
			break;
		}
	}
	void Update(){
		refresh();
	}
}
