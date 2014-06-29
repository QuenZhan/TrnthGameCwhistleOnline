using UnityEngine;
using System.Collections;

public class SmrInputToggler : TrnthInputToggler {
	public SmrControllerBattle battle;
	public SmrRpcRequester requester;
	public void toggleMove(bool isMove){
		if(!battle.playerMe)return;
		if(isMove)	requester.requestPlayerMove		(battle.playerMe.name,input.locator.transform.position);
		else 		requester.requestPlayerFight	(battle.playerMe.name,input.locator.transform.position);
		
	}
	void Update(){
		bool yes=input.isHold;
		if(yes)toggleMove(yes);
	}
	void OnInputUp(){
		toggle(false);	
		toggleMove(false);
	}
}
