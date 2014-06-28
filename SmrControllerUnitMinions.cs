using UnityEngine;
using System.Collections;

public class SmrControllerUnitMinions : SmrControllerUnit{
	public void reset(){
		hp=10;
	}
	public GameObject hero{
		get{
			if(!player)return null;
			if(!player.hero)return null;
			return player.hero.gameObject;
		}
	}
	void OnSpawned(){
		reset();
	}
}
