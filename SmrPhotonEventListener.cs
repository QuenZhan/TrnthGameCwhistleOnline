using UnityEngine;
using System.Collections;

public class SmrPhotonEventListener : Photon.MonoBehaviour {
	public SmrControllerBattle battle;
	public SmrRpcRequester srr;
	public void OnPhotonPlayerConnected(PhotonPlayer player){
		battle.syncUnitsPosition();
		srr.requestPlayerJoin(player.ID+"");
	}
	public void OnPhotonPlayerDisconnected(PhotonPlayer player){}
}
