using UnityEngine;
using System.Collections;

public class SmrControllerUnitMinions : SmrControllerUnit{
	public float cdThink;
	public void reset(){
		hp=10;
	}
	public SmrControllerUnitHero hero{
		get{
			if(!player)return null;
			if(!player.hero)return null;
			return player.hero;
		}
	}
	public virtual void setup(SmrControllerUnitHero hero){
		
	}
	public virtual void think(){
		switch(surface){
		case"move":
			target.transform.position=hero.transform.position+Random.insideUnitSphere*0;
			break;
		}
		//target.transform.position=
		if(enabled)Invoke("think",cdThink);
	}

	void OnSpawned(){
		reset();
	}
	void Start(){
		Invoke("think",cdThink);
	}
}
