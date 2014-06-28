using UnityEngine;
using System.Collections;

public class SmrControllerUnitHero : SmrControllerUnit{
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
		var ctr=SmrControllerBattle.ctr;
		ctr.applyReady(player);
	}
	public void cancelReady(){
		var ctr=SmrControllerBattle.ctr;
		ctr.cancelReady(player);
	}
}