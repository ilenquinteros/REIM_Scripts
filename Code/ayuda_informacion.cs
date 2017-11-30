using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ayuda_informacion : MonoBehaviour {

	public Canvas Info_canvas;
	bool infoOpen = false;
	public Sesion_controller sesion;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void infoPanel(){
		if (infoOpen == false) {
			infoOpen = true;
			UnityEngine.UI.Image Imagen = GameObject.Find ("Imagen").GetComponent<UnityEngine.UI.Image> ();
			Imagen.sprite = sesion.getAyudante ();
			Info_canvas.enabled = true;
		} else if (infoOpen == true) {
			infoOpen = false;
			Info_canvas.enabled = false;
		}
	}
}
