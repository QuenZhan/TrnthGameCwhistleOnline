using UnityEngine;
using System.Collections;

public class SmrControllerUnitMinions : SmrControllerUnit{
	public float cdThink;
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
	void Start(){
		Invoke("think",cdThink);
	}
	void think(){
		switch(surface){
		case"move":
			target.transform.position=hero.transform.position+Random.insideUnitSphere*0;
			break;
		}
		//target.transform.position=
		if(enabled)Invoke("think",cdThink);
	}
}
