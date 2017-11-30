using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

	public string tag;

	void Awake(){

		GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);
		if (objs.Length > 1)
			Destroy (objs[0].gameObject);
		DontDestroyOnLoad (this.gameObject);
	}
}