using UnityEngine;
using System.Collections.Generic;

public class SmrControllerBattle : MonoBehaviour {
	static public SmrControllerBattle ctr;
	public SmrControllerPlayer playerMe;
	public SmrControllerPlayer playerPrefab;
	public SmrInputToggler input;
	public PhotonView photonView;
	public SpawnHere spawnerHero;
	public SpawnHere spawnerMinion;
	public GameObject[] onBattleStart;
	public GameObject[] onCountDownStart;
	public GameObject[] onCountDownCancel;
	public GameObject[] onVictory;
	public GameObject[] onDefeat;
	public void applyReady(SmrControllerPlayer player){
		if(playersReady.Contains(player))return;
		playersReady.Add(player);
		if(isPlayersReady()){
			countDownStart();
		}
	}
	public void cancelReady(SmrControllerPlayer player){
		playersReady.Remove(player);
		countDownCancel();
	}
	public void countDownCancel(){
		foreach(var e in onCountDownCancel){e.SetActive(true);}
	}
	public void countDownStart(){
		foreach(var e in onCountDownStart){e.SetActive(true);}
	}
	public void countDownFinish(){
		if(!PhotonNetwork.isMasterClient)return;
		//photonView.RPC("serverReady",PhotonTargets.MasterClient);
		photonView.RPC("clientReady",PhotonTargets.All);
	}
	public void battleStart(){
		if(SmrControllerPlayer.players.Count==1){
			var npc=playerCreate().GetComponent<SmrControllerPlayer>();
			SmrControllerPlayer.players[0].party="white";
			npc.party="black";
			playersReady.Add(npc);
		}
		foreach(var e in playersReady.ToArray()){
			switch(e.party){
			case"black":countBlack+=1;break;
			case"white":countWhite+=1;break;
			}
			e.applyParty();
			e.battleStart();
		}
		foreach(var e in onBattleStart){e.SetActive(true);}
	}
	public void lose(SmrControllerPlayer player){
		switch(player.party){
		case"black":countBlack-=1;break;
		case"white":countWhite-=1;break;
		}
		if(!PhotonNetwork.isMasterClient)return;
		string winnerParty="--";
		if(countBlack==0)winnerParty="white";
		if(countWhite==0)winnerParty="black";
		photonView.RPC("clientBattleEnd",PhotonTargets.All,winnerParty);
	}
	public void cleanPlayers(){
		foreach(var e in SmrControllerPlayer.players){
			// PhotonNetwork.Destroy(e.gameObject);
		}
	}
	public GameObject playerMeCreate(){
		playerMe=playerCreate().GetComponent<SmrControllerPlayer>();
		return playerMe.gameObject;
	}
	public GameObject playerCreate(){
		//GameObject obj=PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,playerPrefab.transform.rotation,0);
		GameObject obj=Instantiate(playerPrefab);
		var player=obj.GetComponent<SmrControllerPlayer>();
		//playerSpawnerAppend(player);
		return obj;
	}
	public void playerSpawnerAppend(SmrControllerPlayer player){
		if(player.spawnerMinion&&player.spawnerHero)return;
		player.spawnerMinion=Instantiate(spawnerMinion) as SpawnHere;
		player.spawnerHero=Instantiate(spawnerHero) as SpawnHere;
	}
	[RPC]void serverReady(){
		photonView.RPC("clientReady",PhotonTargets.All);
	}
	[RPC]void clientReady(){
		battleStart();
	}
	[RPC]void clientBattleEnd(string winnerParty){
		if(playerMe.party==winnerParty)	foreach(var e in onVictory)e.SetActive(true);
		else 							foreach(var e in onDefeat)e.SetActive(true);
	}
	List<SmrControllerPlayer> playersReady=new List<SmrControllerPlayer>();
	
	int countWhite=0;
	int countBlack=0;
	void Start(){
		playerMe=playerCreate();
		input.player=playerMe;
		input.enabled=true;
	}
	void Awake(){
		ctr=this;
	}
	bool isPlayersReady(){
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
		return SmrControllerPlayer.players.Count==playersReady.Count;
	}
}
