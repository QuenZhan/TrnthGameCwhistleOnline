using UnityEngine;
using System.Collections;

public class SmrPhotonEventListener : Photon.MonoBehaviour {
	public SmrControllerBattle battle;
	public SmrRpcRequester srr;
	public void OnPhotonPlayerConnected(PhotonPlayer player){
		battle.syncUnitsPosition();
	}
	public void OnPhotonPlayerDisconnected(PhotonPlayer player){}
}
