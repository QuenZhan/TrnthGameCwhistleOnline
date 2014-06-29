using UnityEngine;
using System.Collections;

public class SmrControllerUnitHero : SmrControllerUnit{
	public SmrControllerBattle battle;
	//public SmrRpcRequester sr;
	public GameObject[] onReady;
	public GameObject[] onPartyConfirn;
	public GameObject[] onPartyCanceled;
	public string party{
		set{
			if(player)player.party=value;
		}
		get{
			if(!player)return"no player";
			return player.party;
		}
	}
	public void applyReady(){
		battle.applyReady(player);
		if(battle.isReady){
			//sr.requestBattleStart();
			foreach(var e in onReady)e.SetActive(true);
			
		}
		foreach(var e in onPartyConfirn)e.SetActive(true);
	}
	public void cancelReady(){
		battle.cancelReady(player);
		foreach(var e in onPartyCanceled)e.SetActive(true);
	}
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag!="PartyTrigger")return;
		party=col.name;
		applyReady();
		Debug.Log(" applyReady");
	}
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag!="PartyTrigger")return;
		party="";
		cancelReady();
	}
}