using UnityEngine;
using System.Collections;

public class SmrControllerUnitHero : SmrControllerUnit{
	public SmrFomation formation;
	public GameObject[] onReady;
	public GameObject[] onPartyConfirn;
	public GameObject[] onPartyCanceled;
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
}