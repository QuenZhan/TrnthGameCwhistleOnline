using UnityEngine;
using System.Collections.Generic;

public class SmrContainer<T> where T:Object{
	public T find(string name){
		if(!dict.ContainsKey(name))return null;		
		return dict[name];
	}
	public T[] array{
		get{
			T[] arr=new T[dict.Count];
			dict.Values.CopyTo(arr, 0);
			return arr;
		}
	}
	public string add(string key,T e){
		dict.Add(key,e);
		count+=1;
		return key;
	}
	public string add(T e){
		return add(typeof(T).Name+"_"+count,e);
	}
	Dictionary<string,T> dict=new Dictionary<string,T>();
	int count=0;
}
