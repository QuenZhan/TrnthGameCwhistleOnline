using UnityEngine;
using System.Collections.Generic;

public class SmrControllerPlayer : TRNTH.PoolBase {
	//static public List<SmrControllerPlayer> players=new List<SmrControllerPlayer>();
	public SmrControllerUnitHero hero;
	public SmrControllerBattle battle;
	public PhotonPlayer photonPlayer;
	public SpawnHere spawnerHero;
	public SpawnHere spawnerMinion;
	public bool isReady;
	public bool isSpawning;
	public int money;
	public int leadership=3;
	public string party;
	public Dictionary<string,SmrControllerUnit> units;
	public override void Awake(){
		base.Awake();
		players.Add(this);
		units=new Dictionary<string,SmrControllerUnit>();
		//SmrControllerBattle.ctr.playerSpawnerAppend(this);
	}
	public void heroMove(Vector3 pos,bool isMove){
		if(isMove)rpcHeroMove(pos);
		else rpcHeroFiight(pos);
		// view.RPC("server",PhotonTargets.MasterClient,isMove?"rpcHeroMove":"rpcHeroFiight",pos);
	}
	public void unitHpUpdate(SmrControllerUnit unit,int value){
		// view.RPC("serverUnitHpUpdate",PhotonTargets.MasterClient,unit.name,value);
	}
	public void battleStart(){
		isSpawning=true;
	}
	[ContextMenu ("minionSpawn")]
	public SmrControllerUnit minionSpawn(){
		if(units.Count>=leadership)return null;		
		if(!hero)return null;
		var unit=unitSpawn(false);
		unit.transform.position=hero.transform.position+Vector3.up*3;
		return unit;
	}
	public SmrControllerUnit unitSpawn(bool isHero){
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
		unit.name="Unit "+(photonPlayer!=null?photonPlayer.name:"npc") +" "+countSumUnits;
		unit.applyParty(party);
		units.Add(unit.name,unit);
		return unit;
	}
	[ContextMenu ("applyParty")]
	public void applyParty(){
		foreach(var e in units.Values){
			e.applyParty(party);
		}
	}
	// RPCs server
	[RPC]void server(string clientMethod,Vector3 pos){
		// view.RPC(clientMethod,PhotonTargets.All,pos);
	}
	[RPC]void serverUnitHpUpdate(string key,int value){
		// view.RPC("rpcUnitHpUpdate",PhotonTargets.AllBuffered,key,value);
	}
	// RPCs client
	[RPC]void rpcUnitCreate(string type){}
	[RPC]void rpcUnitHpUpdate(string key,int value){
		if(!units.ContainsKey(key))return;
		var unit=units[key];
		if(!unit)return;
		unit.hp=value;
		unit.hurt();
		if(value<1){
			unit.die();
			units.Remove(unit.name);
			if(unit==hero)lose();
		}
		Debug.Log("hp : "+value);
		// if(index<0||index>=units.Count)return;
	}
	[RPC]void rpcHeroMove(Vector3 pos){
		this.pos=pos;
		cHero.targetPersitant=gameObject;
		hero.toggleMove=true;
		heroConstraint.target=gameObject.transform;
	}
	[RPC]void rpcHeroFiight(Vector3 pos){
		this.pos=pos;
		cHero.targetPersitant=null;
		hero.toggleMove=false;
	}
	[RPC]void rpcReadyUpdate(bool isReady){

	}
	// private
	TrnthCreature cHero;
	PathologicalGames.SmoothLookAtConstraint heroConstraint;
	TRNTH.Alarm a=new TRNTH.Alarm();
	int countSumUnits;
	void lose(){
		battle.lose(this);
		isSpawning=false;
	}
	void OnDestroy(){
		players.Remove(this);
		//PhotonNetwork.Destroy(gameObject);
		// if(hero)Despawn(hero.gameObject);
	}
	void Start(){
		var unit=unitSpawn(true);
		if(!unit){
			Debug.Log(" create hero failed");
			return ;
		}
		foreach(var spawner in new SpawnHere[]{spawnerHero,spawnerMinion}){
			spawner.transform.position=Vector3.up*3+pos;
			spawner.transform.parent=tra;			
		}
		hero=unit.GetComponent<SmrControllerUnitHero>();
		hero.transform.position=transform.position+Vector3.up*3;
		cHero=hero.GetComponent<TrnthCreature>();
		heroConstraint=hero.GetComponent<PathologicalGames.SmoothLookAtConstraint>();
		heroConstraint.target=this.transform;
	}
	void Update(){
		if(isSpawning){
			a.routine(5,delegate(){
					minionSpawn();			
				});			
		}
	}
}
