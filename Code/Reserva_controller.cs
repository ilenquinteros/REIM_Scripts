using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reserva_controller : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	public GameObject Canvas;
	public GameObject Ayudante;

	float espera = 8.0f;
	float time = 0.0f;

	public void LoadingSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	// Use this for initialization
	void Start () {
		sesion.setStartTime (Time.time);
		sesion.setCompleto (false);
		sesion.resetAciertos ();
		sesion.resetErrores ();
		sesion.resetAccesoAyuda ();
		sesion.setElemento ("-");
		sesion.enviarData ();
	}

	void Update (){
		if (time <= espera) {
			time += Time.deltaTime;
		} else {
			Image imagen = Ayudante.GetComponent<Image> ();
			imagen.sprite = sesion.getAyudante ();
			Canvas.SetActive (true);

			AudioSource sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();
			Debug.Log (sesion.getAyudante ().name);
			switch (sesion.getAyudante ().name) {
			case "biologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Feliz") as AudioClip;
				break;
			case "deportista_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Felizmascota") as AudioClip;
				break;
			case "medico_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Felizmascota") as AudioClip;
				break;
			case "meteorologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Felizmascota") as AudioClip;
				break;
			default:
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Felizmascota") as AudioClip;
				break;
			}

			sonido.Play ();
		}
	}

	public void Continuar(){
		sesion.resetKeys ();
		LoadingSiguiente ("mapa_scene");
	}

	public void Nuevo (){
		sesion.setSesion(System.DateTime.Now.ToString ());
		LoadingSiguiente ("ayudante_scene");

	}

}
