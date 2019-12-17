using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil{

	public static GameObject Instantiate(GameObject prefab, Vector3 pos){
		GameObject instance = null;
		instance = GameObject.Instantiate (prefab);
		instance.transform.position = pos;

		return instance;
	}

	public static void Destroy(GameObject gameObject){
		GameObject.Destroy (gameObject);
	}

}
