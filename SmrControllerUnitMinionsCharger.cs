using UnityEngine;
using System.Collections;

public class SmrControllerUnitMinionsCharger : SmrControllerUnitMinions{
	public override void think(){
		switch(surface){
		case"move":				
			target.transform.position=posEnemyParty();
			break;
		}
		//target.transform.position=
		if(enabled)Invoke("think",cdThink);
	}
	Vector3 posEnemyParty(){
		switch(party){
		case"black":return battle.locationWhite.transform.position;
		case"white":return battle.locationBlack.transform.position;
		}
		return Vector3.zero;
	}
}
