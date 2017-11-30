using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mapa_controller : MonoBehaviour
{
	bool infoOpen = false;
	static bool aviso = true;
	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	public Canvas Aviso_reserva;

	/*
	public void EscenaSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
	}
	*/

	void Start(){
		sesion.setStartTime (Time.time);
		sesion.setCompleto (false);
		sesion.resetAciertos ();
		sesion.resetErrores ();
		sesion.resetAccesoAyuda ();
		sesion.setElemento ("-");
		sesion.enviarData ();

		switch(sesion.getLlave()){
		case 1:
			GameObject.Find ("key_1").GetComponent<Image> ().sprite = Resources.Load<Sprite>("Llaves/key_1");
			break;
		case 2:
			GameObject.Find ("key_1").GetComponent<Image> ().sprite = Resources.Load<Sprite>("Llaves/key_1");
			GameObject.Find ("key_2").GetComponent<Image> ().sprite = Resources.Load<Sprite>("Llaves/key_2");
			break;
		case 3:
			GameObject.Find ("key_1").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Llaves/key_1");
			GameObject.Find ("key_2").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Llaves/key_2");
			GameObject.Find ("key_3").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Llaves/key_3");
			GameObject.Find ("Entrada").SetActive (false);
			if (aviso) {
				GameObject.Find("Imagen_aviso").GetComponent<Image>().sprite = sesion.getAyudante ();
				infoPanel (Aviso_reserva);
				aviso = false;
			}

			break;
		}
	}

	public void LoadingSiguiente(string scene)
	{
		if (scene.Equals ("login_scene")) {
			sesion.setLogout (System.DateTime.Now.ToString());
			sesion.registrarLogOut ();
			sesion.enviarData ();
		}
		StartCoroutine (LoadAsynchronously (scene));
	}

	IEnumerator LoadAsynchronously (string scene){


		AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

		loadingScreen.SetActive (true);

		while(!operation.isDone){
			float progress = Mathf.Clamp01 (operation.progress / .9f);

			slider.value = progress;

			yield return null;
		}
	}

	public void infoPanel(Canvas Info_canvas){
		if (infoOpen == false) {
			infoOpen = true;
			Image Imagen = GameObject.Find("Imagen").GetComponent<Image> ();
			Imagen.sprite = sesion.getAyudante ();
			Info_canvas.enabled = true;
			sesion.setAccesoAyuda ();

			AudioSource sonido = GameObject.Find ("Instruccion").GetComponent<AudioSource> ();

			switch (sesion.getAyudante ().name) {
			case "biologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Mapa") as AudioClip;
				break;
			case "deportista_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Mapa") as AudioClip;
				break;
			case "medico_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Mapa") as AudioClip;
				break;
			case "meteorologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Mapa") as AudioClip;
				break;
			default:
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Mapa") as AudioClip;
				break;
			}

			sonido.Play ();
		} else if (infoOpen == true) {
			infoOpen = false;
			Info_canvas.enabled = false;
		}
	}

	public void Confirmar(Canvas Salir){
		if (infoOpen == false) {
			infoOpen = true;
			Salir.enabled = true;
		} else if (infoOpen == true) {
			infoOpen = false;
			Salir.enabled = false;
		}
	}
}