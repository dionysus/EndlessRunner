using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecycle {
	void Restart();
	void ShutDown();
}

public class RecycleGameObject : MonoBehaviour {

	private List<IRecycle> recycleComponents;

	void Awake(){
		// Gets list of all components, tests if they have the IR interface,
		// if so, adds it to the list as a recyclable component
		var components = GetComponents<MonoBehaviour> ();
		recycleComponents = new List<IRecycle> ();

		foreach(var component in components) {
			if (component is IRecycle) { // does component implement interface?
				recycleComponents.Add (component as IRecycle);
			}
		}
	}

	public void Restart(){
		gameObject.SetActive(true);
		foreach (var component in recycleComponents) {
			component.Restart();
		}
	}

	public void ShutDown(){
		gameObject.SetActive(false);
		foreach (var component in recycleComponents) {
			component.ShutDown();
		}
	}
}
