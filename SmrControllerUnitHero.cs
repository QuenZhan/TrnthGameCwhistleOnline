using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SmrControllerUnitHero : SmrControllerUnit{
	public GameObject prefabFormatioinLocator;
	public SmrFomation formation;
	public Transform formatioinLocatorRequest(SmrControllerUnit unit){
		if(lads.Contains(unit))return null;
		lads.Add(unit);
		//unit.target=.gameObject;
		//Debug.Log("bbbb");
		return formationLocatorAdd();
	}
	public void fomationClean(){
		foreach(Transform e in formation.transform){
			Despawn(e.gameObject);
		}
	}
	public void formationRefresh(){
		fomationClean();
		var q=from unit in lads
			where (unit!=null && unit.gameObject.activeInHierarchy)
			select unit;
		lads=new List<SmrControllerUnit>(q);
		// foreach(var e in lads.ToArray()){
		// 	e.target=formationLocatorAdd().gameObject;
		// }
	}
	public Transform formationLocatorAdd(){
		Transform e=Spawn(prefabFormatioinLocator);
		e.parent=formation.transform;
		formation.execute();
		return e;
	}
	List<SmrControllerUnit> lads=new List<SmrControllerUnit>();
}