using UnityEngine;
using System.Collections;

public class SmrTabelCellRoom : MonoBehaviour {
	public RoomInfo pRoom;
	public UILabel labelName;
	public UILabel labelStatus;
	public void refresh(){
		labelName.text=pRoom.name;
		labelStatus.text=pRoom.maxPlayers+"";
	}
	public void join(){
		PhotonNetwork.JoinRoom(pRoom.name);
	}
	void Start(){
		if(pRoom!=null)refresh();
	}
}
