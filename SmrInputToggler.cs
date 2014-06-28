using UnityEngine;
using System.Collections;

public class SmrInputToggler : TrnthInputToggler {
	public SmrControllerPlayer player;
	public void setPlayer(GameObject gobj){
		player=gobj.GetComponent<SmrControllerPlayer>();
		enabled=true;
	}
	void Update(){
		bool yes=input.isHold;
		toggle(yes);
		if(yes)player.heroMove(input.locator.transform.position,yes);
	}
	void OnInputUp(){
		toggle(false);	
		if(player)player.heroMove(input.locator.transform.position,false);
	}
}
