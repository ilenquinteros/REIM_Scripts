using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mascota_reserva_controlador : MonoBehaviour {

	public Sesion_controller sesion;
	public float espera = 4.0f;
	float espera_animales = 3.89f;
	float time = 0.0f;

	string path;
	string sound;
	private Animator animator;
	public GameObject Animales;

	void Start () {
		
		switch (sesion.getMascota ()) {
		case "Tigre_button":
			path = "Animaciones/Reserva/Tigre_reserva";
			sound = "Musica/Animales/Tigre";
			break;
		case "Elefante_button":
			path = "Animaciones/Reserva/Elefante_reserva";
			sound = "Musica/Animales/Elefante";
			break;
		case "Panda_button":
			path = "Animaciones/Reserva/Panda_reserva";
			sound = "Musica/Animales/Panda";
			break;
		case "Rino_button":
			path = "Animaciones/Reserva/Rino_reserva";
			sound = "Musica/Animales/Rino";
			break;
		case "Leopardo_button":
			path = "Animaciones/Reserva/Leopardo_reserva";
			sound = "Musica/Animales/Leopardo";
			break;
		case "Polar_button":
			path = "Animaciones/Reserva/Polar_reserva";
			sound = "Musica/Animales/Polar";
			break;
		default:
			path = "Animaciones/Reserva/Tigre_reserva";
			sound = "Musica/Animales/Tigre";
			break;
		}

		animator = gameObject.GetComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
	}
	
	// Update is called once per frame
	void Update () {
		if (time <= espera) {
			time += Time.deltaTime;
		} else {
			animator.SetTrigger ("Stop");
		}

		if (time > espera_animales) {
			Animales.GetComponent<Animator> ().SetTrigger("Appear");
		}
	}
}
