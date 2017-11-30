using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class Mascota_controlador : MonoBehaviour {
	public float speed = 3f;
	public float paddingX = 3f;
	public float paddingY = 6f;
	// Use this for initialization
	private Animator animator;
	public Sesion_controller sesion;

	void Start () {
		string path;
		string sound;
		switch (sesion.getMascota ()) {
			case "Tigre_button":
				path = "Animaciones/Tigre/Tigre";
				sound = "Musica/Animales/Tigre";
				break;
			case "Elefante_button":
				path = "Animaciones/Elefante/Elefante";
				sound = "Musica/Animales/Elefante";
				break;
			case "Panda_button":
				path = "Animaciones/Panda/Panda";
				sound = "Musica/Animales/Panda";
				break;
			case "Rino_button":
				path = "Animaciones/Rino/Rino";
				sound = "Musica/Animales/Rino";
				break;
			case "Leopardo_button":
				path = "Animaciones/Leopardo/Leopardo";
				sound = "Musica/Animales/Leopardo";
				break;
			case "Polar_button":
				path = "Animaciones/Polar/Polar";
				sound = "Musica/Animales/Polar";
				break;
		default:
				path = "Animaciones/Tigre/Tigre";
				sound = "Musica/Animales/Tigre";
				break;
		}
		animator = gameObject.GetComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;

		UnityEngine.UI.Button Sonido = GameObject.Find ("Sonido").GetComponent<UnityEngine.UI.Button> ();
		AudioSource Audio = Sonido.GetComponent<AudioSource>();
		Audio.clip = Resources.Load (sound) as AudioClip;
	}
	// Update is called once per frame
	void Update () {
		float hInput = CnInputManager.GetAxis ("Horizontal");
		transform.position += new Vector3 (hInput * speed * Time.deltaTime, 0, 0);

		float vInput = CnInputManager.GetAxis ("Vertical");
		transform.position += new Vector3 (0, vInput * speed * Time.deltaTime, 0);

		float newX = Mathf.Clamp (transform.position.x, -10 + paddingX, 10 - paddingX);
		float newY = Mathf.Clamp (transform.position.y, -10 + paddingY, 10 - paddingY);
		transform.position = new Vector3 (newX, newY, transform.position.z);

		float direccionV = CnInputManager.GetAxis ("Vertical");
		float direccionH = CnInputManager.GetAxis ("Horizontal");

		if (direccionV > 0.1f) {
			animator.SetTrigger ("Run");
		} else if (direccionV < -0.1f) {
			animator.SetTrigger ("Run_back");
		} else if(direccionH > 0.1f) {
			animator.SetTrigger ("Run_right");
		} else if (direccionH < -0.1f) {
			animator.SetTrigger ("Run_left");
		} else{
			/*
			animator.ResetTrigger ("Run_right");
			animator.ResetTrigger ("Run_left");
			animator.ResetTrigger ("Run");
			animator.ResetTrigger ("Run_back");
			*/
			animator.SetTrigger ("Idle");
		}
	}
}
