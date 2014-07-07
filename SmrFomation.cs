using UnityEngine;
using System.Collections;

public class SmrFomation : TrnthMonoBehaviour {
	public enum Type{circle,square}
	public Type type;
	public float radius;
	[ContextMenu ("execute")]
	public void execute(){
		var ll=transform.childCount;
		if(ll==0)return;
		foreach(Transform e in transform){
			switch(type){
			case Type.circle:
				transform.Rotate(Vector3.up*360/ll);
				e.localPosition=transform.forward*radius;
				break;
			}			
		}
	}
}
