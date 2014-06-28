using UnityEngine;
using System.Collections;

public class SmrInputToggler : TrnthInputToggler {
	public SmrControllerPlayer player;
	public SmrControllerBattle ctrBattle;
	public void setPlayer(GameObject gobj){
		player=gobj.GetComponent<SmrControllerPlayer>();
		enabled=true;
	}
	public void toggleMove(bool isMove){
		//player.heroMove(input.locator.transform.position,isMove);
		ctrBattle.heroMove(input.locator.transform.position,isMove);
	}
	void Update(){
		bool yes=input.isHold;
		toggle(yes);
		if(yes)toggleMove(yes);
	}
	void OnInputUp(){
		toggle(false);	
		if(player)toggleMove(false);
	}
}
