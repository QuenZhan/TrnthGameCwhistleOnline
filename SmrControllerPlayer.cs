﻿using UnityEngine;
using System.Collections.Generic;

public class SmrControllerPlayer : TRNTH.PoolBase {
	//static public List<SmrControllerPlayer> players=new List<SmrControllerPlayer>();
	public SmrControllerUnitHero hero;
	public SmrControllerBattle battle;
	public SmrRpcRequester sr;
	public PhotonPlayer photonPlayer;
	public SpawnHere spawnerHero;
	public SpawnHere spawnerMinion;
	public bool isReady;
	public bool isSpawning;
	public float cdSpawning=5;
	public int money;
	public int leadership=3;
	public string party;
	public List<SmrControllerUnit> units=new List<SmrControllerUnit>();
	[ContextMenu ("minionSpawn")]
	public SmrControllerUnit minionSpawn(){
		//if(units.Count>=leadership)return null;		
		if(!hero)return null;
		var unit=unitSpawn(false);
		//unit.transform.position=hero.transform.position+Vector3.up*3;
		return unit;
	}
	public SmrControllerUnit unitSpawn(){
		if(units.Count==0)return unitSpawn(true);
		else  return minionSpawn();
	}
	public SmrControllerUnit unitSpawn(bool isHero){
		if(!isSpawning)return null;
		SpawnHere spawner=null;
		if(isHero)spawner=spawnerHero;
		else spawner=spawnerMinion;
		if(!spawner){
			Debug.Log("no spawner");
			return null;
		}
		var unit=spawner.execute().GetComponent<SmrControllerUnit>();
		unit.player=this;
		countSumUnits+=1;
		//unit.name="Unit "+(photonPlayer!=null?photonPlayer.name:"npc") +" "+countSumUnits;
		unit.applyParty(party);
		if(isHero){
			heroSetup(unit);
		}else{
			var minion=unit.GetComponent<SmrControllerUnitMinions>();
			minion.setup(hero);
		}
		units.Add(unit);
		return unit;
	}
	[ContextMenu ("applyParty")]
	public void applyParty(){
		foreach(var e in units){
			e.applyParty(party);
		}
	}
	public void heroMove(Vector3 pos){
		//this.pos=pos;
		hero.target.transform.position=pos;
		hero.toggleMove=true;
		//heroConstraint.target=gameObject.transform;
	}
	public void heroFight(Vector3 pos){
		//this.pos=pos;
		// Debug.Log(hero.transform.position);
		hero.target.transform.position=hero.formation.transform.position;
		hero.toggleMove=false;
	}
	public void heroSetup(SmrControllerUnit unit){
		hero=unit.GetComponent<SmrControllerUnitHero>();
		hero.transform.position=transform.position+Vector3.up*3;
		//hero=hero.GetComponent<TrnthCreature>();
		//heroConstraint=hero.GetComponent<PathologicalGames.SmoothLookAtConstraint>();
		// heroConstraint.target=this.transform;
		foreach(var spawner in new SpawnHere[]{spawnerHero,spawnerMinion}){
			spawner.transform.parent=hero.transform;			
			spawner.transform.localPosition=Vector3.up*3;
		}
	}
	// private
	TrnthCreature cHero;
	PathologicalGames.SmoothLookAtConstraint heroConstraint;
	int countSumUnits;

	void Start(){
		Invoke("spawn",0);		
	}
	void spawn(){
		var list=new List<SmrControllerUnit>();
		foreach(var e in units){
			if(e.gameObject.activeInHierarchy)list.Add(e);
		}
		units=list;
		if(units.Count<leadership)sr.requestUnitCreate(name);
		if(isSpawning)Invoke("spawn",cdSpawning);
	}
}
