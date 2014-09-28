using UnityEngine;
using System.Collections;

public class SmrControllerUnitMinionsGuard : SmrControllerUnitMinions{
	public Transform locatorGuard;
	public override void setup(SmrControllerUnitHero hero){
		//Debug.Log("dddd");
		locatorGuard=hero.formatioinLocatorRequest(this);
		if(!locatorGuard)locatorGuard=hero.transform;
	}
	public override void think(){
		switch(surface){
		case"move":
			target.transform.position=locatorGuard.position;
			break;
		}
		//target.transform.position=
		if(enabled)Invoke("think",cdThink);
	}
}
