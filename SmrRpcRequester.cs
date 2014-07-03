using UnityEngine;
using System.Collections;

public class SmrRpcRequester : MonoBehaviour {
	public PhotonView photonView;
	public void requestUnitCreate(string playerName){
		if(isServer)photonView.RPC("unitCreate",PhotonTargets.AllBuffered,playerName);
	}
	public void requestUnitHpUpdate(string unitName,int value){
		if(isServer)photonView.RPC("unitHpUpdate",PhotonTargets.AllBuffered,unitName,value);
	}
	public void requestPlayerJoin(string name){
		if(isServer)photonView.RPC("playerJoin",PhotonTargets.AllBuffered,name);
	}
	public void requestPlayerLeave(string name){
		if(isServer)photonView.RPC("playerLeave",PhotonTargets.AllBuffered,name);	
	}
	public void requestPlayerMove(string name,Vector3 pos){
		photonView.RPC("serverPlayerMove",PhotonTargets.MasterClient,name,pos);
	}
	public void requestPlayerFight(string name,Vector3 pos){
		photonView.RPC("serverPlayerFight",PhotonTargets.MasterClient,name,pos);
	}
	public void requestBattleStart(){
		if(isServer)photonView.RPC("battleStart",PhotonTargets.AllBuffered);
	}
	public void requestBattleEnd(string party){
		if(isServer)photonView.RPC("battleEnd",PhotonTargets.AllBuffered,party);
	}
	public void requestUnitPosition(string name,Vector3 pos){
		if(isServer)photonView.RPC("unitPosition",PhotonTargets.Others,name,pos);
	}
	[RPC]void serverPlayerMove(string name,Vector3 pos){
		photonView.RPC("playerMove",PhotonTargets.All,name,pos);
		//Debug.Log("ddd");
	}
	[RPC]void serverPlayerFight(string name,Vector3 pos){
		photonView.RPC("playerFight",PhotonTargets.All,name,pos);
	}
	bool isServer;
	void Awake(){
		isServer=PhotonNetwork.isMasterClient;
	}
}
