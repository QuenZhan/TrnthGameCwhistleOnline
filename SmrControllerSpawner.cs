using UnityEngine;
using System.Collections;

public class SmrControllerSpawner : MonoBehaviour {
	public GameObject target;
	public SmrControllerBattle battle;
	public SpawnHere spawner;
	public bool repeat=true;
	public string party;
	public float cooldown;
	void Start(){
		Invoke("execute",cooldown);
	}
	void execute(){
		var obj=spawner.execute();
		if(obj){
			var unit=obj.GetComponent<SmrControllerUnit>();
			unit.target.transform.position=target.transform.position;
			unit.party=party;			
		}
		if(repeat)Invoke("execute",cooldown);
	}
}
