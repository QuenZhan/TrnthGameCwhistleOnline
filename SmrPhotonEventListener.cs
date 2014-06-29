using UnityEngine;
using System.Collections;

public class SmrPhotonEventListener : Photon.MonoBehaviour {
	public SmrRpcRequester srr;
	public void OnPhotonPlayerConnected(PhotonPlayer player){
		srr.requestPlayerJoin(player.ID+"");
	}
	public void OnPhotonPlayerDisconnected(PhotonPlayer player){}
}
