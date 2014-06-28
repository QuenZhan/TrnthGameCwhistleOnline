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
		if(playersReady.Count==1){
			var npc=playerCreate(null).GetComponent<SmrControllerPlayer>();
			playersReady[0].party="white";
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
	}
	public GameObject playerCreate(PhotonPlayer photonPlayer){
		//GameObject obj=PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,playerPrefab.transform.rotation,0);
		var player=Instantiate(playerPrefab) as SmrControllerPlayer;
		player.photonPlayer=photonPlayer;
		players.Add(player);
		// var player=obj.GetComponent<SmrControllerPlayer>();
		//playerSpawnerAppend(player);
		return player.gameObject;
	}
	public void playerSpawnerAppend(SmrControllerPlayer player){
		if(player.spawnerMinion&&player.spawnerHero)return;
		player.spawnerMinion=Instantiate(spawnerMinion) as SpawnHere;
		player.spawnerHero=Instantiate(spawnerHero) as SpawnHere;
	}
	public void heroMove(Vector3 pos,bool isMove){
		photonView.RPC("serverHeroMove",PhotonTargets.MasterClient,PhotonNetwork.playerName,pos,isMove);
	}
	public void unitHpUpdate(SmrControllerUnit unit,int value){
		photonView.RPC("serverUnitHpUpdate",PhotonTargets.MasterClient,unit.name,value);
	}
	[RPC]public void clientUnitHpUpdate(string unitName,int value){

	}
	[RPC]public void clientHeroMove(string playerName,Vector3 pos,bool isMove){
		foreach(var e in SmrControllerPlayer.players.ToArray()){
			if(e.photonPlayer.name!=playerName)continue;
			e.heroMove(pos,isMove);
			break;
		}
	}
	[RPC]void serverUnitHpUpdate(string unitName,int value){
		photonView.RPC("clientUnitHpUpdate",PhotonTargets.All,unitName,isMove);
	}
	[RPC]void serverHeroMove(string playerName,Vector3 pos,bool isMove){		
		photonView.RPC("clientHeroMove",PhotonTargets.All,playerName,pos,isMove);
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
	List<SmrControllerPlayer> players=new List<SmrControllerPlayer>();
	List<SmrControllerUnit> units=new List<SmrControllerUnit>();
	
	int countWhite=0;
	int countBlack=0;
	void Start(){
		playerMe=playerCreate(PhotonNetwork.player).GetComponent<SmrControllerPlayer>();
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
