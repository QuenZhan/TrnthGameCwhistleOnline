using UnityEngine;
using System.Collections;

public class SmrControllerUnitMinionsGuard : SmrControllerUnitMinions{
	public override void think(){
		switch(surface){
		case"move":				
			//target.transform.position=posEnemyParty();
			break;
		}
		//target.transform.position=
		if(enabled)Invoke("think",cdThink);
	}
}
