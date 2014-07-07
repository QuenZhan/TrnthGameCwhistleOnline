using UnityEngine;
using System.Collections;

public class SmrControllerAttack : MonoBehaviour {
	public SmrRpcRequester requester;
	public GameObject locator;
	public void attack(GameObject[] gobjs){
		GameObject ofensive=null;
		GameObject defensive=null;
		if(gobjs.Length>0){
			ofensive=gobjs[0];
			locator.transform.position=ofensive.transform.position;
			locator.SetActive(true);
			locator.SendMessage("execute");
		}
		else return;
		if(gobjs.Length>1)defensive=gobjs[1];
		else return;
		var container=ofensive.GetComponent<SmrUnitContainer>();
		if(!container)return;
		var uo=container.unit;
		container=defensive.GetComponent<SmrUnitContainer>();
		if(!container)return;
		var ud=container.unit;
		if(!ud)return;
		// Debug.Log("Attack");
		if(!ud.player)return;
		requester.requestUnitHpUpdate(ud.name,ud.hp-uo.attackDamage);
	}
}
