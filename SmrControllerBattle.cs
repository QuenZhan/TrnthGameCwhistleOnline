using UnityEngine;
using System.Collections.Generic;

public class SmrControllerBattle : MonoBehaviour {
	public SmrControllerPlayer playerMe;
	public SmrControllerPlayer playerPrefab;
	public SmrRpcRequester sr;
	public PhotonView photonView;
	public GameObject[] onBattleStart;
	public GameObject[] onVictory;
	public GameObject[] onDefeat;
	public GameObject locationBlack;
	public GameObject locationWhite;
	[RPC]public SmrControllerUnit unitCreate(string playerName){
		var player=players.find(playerName);
		Debug.Log("unit create "+playerName);
		if(!player){
			return null;
		}
		var newUnit=player.unitSpawn();
		//Debug.Log(newUnit);
		if(!newUnit)return null;
		newUnit.name=units.add(newUnit);
		//if(player.units.Count==1)player.heroSetup(newUnit);
		return newUnit;
	}
	[RPC]public void unitHpUpdate(string name,int value){	
		var unit=units.find(name);
		if(!unit)return;
		unit.hp=value;
		unit.hurt();
		if(value<1){
			unit.die();
			if(unit==unit.player.hero)disqualify(unit.player);
		}
		// Debug.Log("hp : "+value);
	}
	[RPC]public	void unitPosition(string name,Vector3 pos){
		var unit=units.find(name);
		if(!unit)return;
		var dvec=unit.transform.position-pos;
		if(dvec.magnitude>1)unit.transform.position=pos;
	}
	[RPC]public SmrControllerPlayer playerJoin(string playerName){
		Debug.Log(playerName);
		var player=players.find(playerName);
		if(player)return player;
		player=Instantiate(playerPrefab) as SmrControllerPlayer;
		player.photonPlayer=photonPlayerFind(playerName);		
		player.name=players.add(playerName,player);
		if(player.name==PhotonNetwork.player.ID+"")playerMe=player;
		switch(players.array.Length%2){
		case 0:
			player.party="white";
			player.transform.position=locationWhite.transform.position;
			countWhite+=1;
			break;
		case 1:
			player.party="black";
			player.transform.position=locationBlack.transform.position;
			countBlack+=1;
			break;
		}
		player.applyParty();
		player.isSpawning=true;

		return player;
	}
	[RPC]public void playerLeave(string playerName){	}
	[RPC]public void playerMove(string playerName,Vector3 pos){
		var player=players.find(playerName);
		if(!player){
			Debug.Log("no player : "+playerName);
			return;
		}
		player.heroMove(pos);
	}
	[RPC]public void playerFight(string playerName,Vector3 pos){
		var player=players.find(playerName);
		if(!player){
			Debug.Log("no player : "+playerName);
			return;
		}
		player.heroFight(pos);
	}
	[RPC]public void battleStart(){
		return ;
		if(players.array.Length==1){
			var npc=playerJoin("npc");
			playerMe.party="white";
			npc.party="black";
		}
		foreach(var e in players.array){
			switch(e.party){
			case"black":countBlack+=1;break;
			case"white":countWhite+=1;break;
			}
			e.applyParty();
			e.isSpawning=true;
		}
		foreach(var e in onBattleStart){e.SetActive(true);}
	}
	[RPC]public void battleEnd(string winnerParty){
		if(playerMe.party==winnerParty)	foreach(var e in onVictory)e.SetActive(true);
		else 							foreach(var e in onDefeat)e.SetActive(true);
	}
	public void applyReady(SmrControllerPlayer player){
		if(playersReady.Contains(player))return;
		playersReady.Add(player);
	}
	public void cancelReady(SmrControllerPlayer player){
		playersReady.Remove(player);	
	}
	public bool isReady{get{
		var ints=new int[]{0,0};
		foreach(var e in playersReady){
			switch(e.party){
			case"white":ints[0]+=1;break;
			case"black":ints[1]+=1;break;
			}			
		}
		if(playersReady.Count!=1){
			if(ints[0]==0||ints[1]==0)return false;			
		}
		return players.array.Length==playersReady.Count;
	}}
	public void syncUnitsPosition(){
		foreach(var e in units.array){
			sr.requestUnitPosition(e.name,e.transform.position);
		}
	}
	int countWhite=0;
	int countBlack=0;

	SmrContainer<SmrControllerPlayer> players=new SmrContainer<SmrControllerPlayer>();
	SmrContainer<SmrControllerUnit> units=new SmrContainer<SmrControllerUnit>();

	List<SmrControllerPlayer> playersReady=new List<SmrControllerPlayer>();


	void disqualify(SmrControllerPlayer player){
		player.isSpawning=false;
		switch(player.party){
		case"black":countBlack-=1;break;
		case"white":countWhite-=1;break;
		}
		if(!PhotonNetwork.isMasterClient)return;
		string winnerParty="--";
		if(countBlack==0)winnerParty="white";
		if(countWhite==0)winnerParty="black";
		photonView.RPC("battleEnd",PhotonTargets.All,winnerParty);
	}

	PhotonPlayer photonPlayerFind(string name){
		return null;
	}
	void Awake(){
	}
	void Start(){
		//Network.isMessageQueueRunning=true;
		Debug.Log("enter Room "+PhotonNetwork.player.ID);
		if(PhotonNetwork.isMasterClient){
			sr.requestPlayerJoin("npc");
			sr.requestPlayerJoin(PhotonNetwork.player.ID+"");
		}
	}
}
