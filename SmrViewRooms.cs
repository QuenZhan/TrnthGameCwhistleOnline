using UnityEngine;
using System.Collections;

public class SmrViewRooms : MonoBehaviour {
	public UITable table;
	public SmrTabelCellRoom prfabeTableCellRoom;
	[ContextMenu ("refresh")]
	public void refresh(){
		if(!prfabeTableCellRoom||!table)return;
		clean();
		foreach(var e in PhotonNetwork.GetRoomList()){
			var tcRoom=Instantiate(prfabeTableCellRoom) as SmrTabelCellRoom;
			tcRoom.transform.parent=table.transform;
			tcRoom.transform.localScale=Vector3.one;
			tcRoom.pRoom=e;
			tcRoom.refresh();
		}
		table.Reposition();
	}
	public void clean(){
		foreach(Transform e in table.transform)Destroy(e.gameObject);
	}
	void OnEanble(){
		refresh();
	}
	void Start(){
		refresh();
	}
}
