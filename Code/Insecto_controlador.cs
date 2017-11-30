using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insecto_controlador : MonoBehaviour {
	
	public float speed = 3;
	public int rangoI = 1;
	public int rangoH = 3;

	private float timeX = 0.0f;
	private float timeY = 0.0f;

	float esperaX = 0.0f;
	float esperaY = 0.0f;

	int X = 0;
	int Y = 0;


	public void setEsperaX(float x){
		esperaX = x;
	}

	public void setEsperaY(float y){
		esperaY = y;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		timeX += Time.deltaTime;
		timeY += Time.deltaTime;


		if (X == 1) {
			transform.position += new Vector3 (1*speed, 0, 0);
			transform.localScale =  new Vector3(-1, 1, 1);
		} else {
			transform.position -= new Vector3 (1*speed, 0, 0);
			transform.localScale =  new Vector3(1, 1, 1);
		}

		if (Y == 1) {
			transform.position += new Vector3 (0, 1*speed, 0);
		} else {
			transform.position -= new Vector3 (0, 1*speed, 0);
		}


		if (timeX >= esperaX) {

			timeX = timeX - esperaX;

			X = Random.Range(0, 2);
			esperaX = Random.Range(rangoI, rangoH);
		}

		if (timeY >= esperaY) {

			timeY = timeY - esperaY;

			Y = Random.Range(0, 2);
			esperaY = Random.Range(rangoI, rangoH);
		}


	}

	public void OnTriggerEnter2D(Collider2D col){
		if(col.CompareTag("Borde_up")){
				Y = 0;
				timeY = 0.0f;
				esperaY = 1.0f;
		}
		if(col.CompareTag("Borde_bottom")){
				Y = 1;
				timeY = 0.0f;
				esperaY = 1.0f;
		}
		if(col.CompareTag("Borde_right")){
				X = 0;
				timeX = 0.0f;
				esperaX = 1.0f;
		}
		if(col.CompareTag("Borde_left")){
				X = 1;
				timeX = 0.0f;
				esperaX = 1.0f;
				

		}
	}
}
