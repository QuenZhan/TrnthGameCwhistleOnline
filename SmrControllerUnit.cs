using UnityEngine;
using System.Collections;

public class SmrControllerUnit : MonoBehaviour {
	public SmrControllerPlayer player;
	public int hp=10;
	public int attackDamage=1;
	public string type;
	public string surface="move";
	public GameObject target;
	public GameObject[] whileMoving;
	public GameObject[] whileFighting;
	public GameObject[] whileHurt;
	public GameObject[] whileDead;
	public GameObject[] attackers;
	public GameObject[] attackReceivers;
	public string party{
		get{return _party;}
		set{applyParty(value);}

	}
	public void applyParty(string party){
		int lAttacker=20;
		int lReceiver=20;
		_party=party;
		switch(party){
		case"black":
			lAttacker=20;
			lReceiver=23;
			break;
		case"white":
			lAttacker=22;
			lReceiver=21;
			break;
		}
		foreach(var e in attackers){e.layer=lAttacker;}
		foreach(var e in attackReceivers){e.layer=lReceiver;}
	}
	public bool toggleMove{
		set{
			foreach(var e in whileMoving){e.SetActive(false);}
			foreach(var e in whileFighting){e.SetActive(false);}
			if(value){
				surface="move";
				foreach(var e in whileMoving){e.SetActive(true);}
			}else {
				surface="fight";
				foreach(var e in whileFighting){e.SetActive(true);}
			}
		}
	}
	public void hurt(){
		foreach(var e in whileHurt){
			e.SetActive(true);
		}
	}
	public void die(){
		foreach(var e in whileDead){
			e.SetActive(true);
		}
		
	}
	string _party;
}
