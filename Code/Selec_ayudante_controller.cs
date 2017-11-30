using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selec_ayudante_controller : MonoBehaviour
{
	public Canvas Info_canvas;
	bool infoOpen = false;
	public GameObject loadingScreen;
	public Slider slider;
	GameObject ayudante_select = null;
	private Animator animator;
	public Sesion_controller sesion;
	public GameObject Mensaje;


	void Start(){
		sesion.setStartTime (Time.time);
		sesion.setAyudante (null);
		sesion.setMascota (null);
		sesion.resetKeys ();
	}

	public void EscenaSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
	}

	public void LoadingSiguiente(string scene)
	{
		if(Sesion_controller.imagen_ayudante!=null){

			sesion.registrarAyudante ();

			StartCoroutine (LoadAsynchronously (scene));
			//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
		}else{
			Msje ();
		}
		//RECOPILACION DE DATOS
		sesion.enviarData ();
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
		
	public void infoPanel(){
		Sprite imagen_ayudante = null;
		string texto = "";
		string sound = "";
		string ayudante_select = EventSystem.current.currentSelectedGameObject.name;
		Image Imagen = GameObject.Find ("Imagen").GetComponent<Image> ();
		Text Descripcion = GameObject.Find ("Descripcion").GetComponent<Text> ();
		AudioSource Sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();

		switch (ayudante_select) {
		case "Nombre_biologo":
			imagen_ayudante = Resources.Load<Sprite> ("Avatares/Ayudantes/biologo_avatar");
			texto = "Los biólogos estudian todos los seres vivos, desde las plantas y animales hasta los más pequeños organismos.";
			sound = "Musica/Audios/Hombre/Biologo";
			break;
		case "Nombre_deportista":
			imagen_ayudante = Resources.Load<Sprite> ("Avatares/Ayudantes/deportista_avatar");
			texto = "Los deportistas profesionales practican distintos deportes en público, inviertiendo mucho tiempo entrenado y practicando.";
			sound = "Musica/Audios/Mujer/Deportista";
			break;
		case "Nombre_medico":
			imagen_ayudante = Resources.Load<Sprite>("Avatares/Ayudantes/medico_avatar");
			texto = "Los doctores diagnostican, tratan y ayudan a prevenir enfermedades. Entregan apoyo y tratamiento a sus pacientes.";
			sound = "Musica/Audios/Mujer/Medico";
			break;
		case "Nombre_meteorologo":
			imagen_ayudante = Resources.Load<Sprite>("Avatares/Ayudantes/meteorologo_avatar");
			texto = "Los meteorólogos estudian el clima de la Tierra. Usan diversas herramientas para comprender el comportamiento del clima.";
			sound = "Musica/Audios/Hombre/Meteoro";
			break;
		default:
			imagen_ayudante = Resources.Load<Sprite>("Avatares/Ayudantes/biologo_avatar");
			texto = "";
			break;
		}
		if (infoOpen == false) {
			infoOpen = true;
			Info_canvas.enabled = true;
			Imagen.sprite = imagen_ayudante;
			Descripcion.text = texto;
			Sonido.clip = Resources.Load (sound) as AudioClip;
		} else if (infoOpen == true) {
			infoOpen = false;
			Info_canvas.enabled = false;
		}
	}


	public void Seleccion_ayudante(){

		if (ayudante_select != null) {
			animator = ayudante_select.GetComponent<Animator>();
			animator.ResetTrigger ("select");
			animator.SetTrigger ("unselect");
		}

		Sprite imagen_ayudante = null;
		UnityEngine.UI.Image Imagen = GameObject.Find ("Imagen").GetComponent<UnityEngine.UI.Image> ();
		ayudante_select = EventSystem.current.currentSelectedGameObject;
		animator = ayudante_select.GetComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load("Animaciones/Mascota") as RuntimeAnimatorController;
		animator.SetTrigger ("select");

		switch (ayudante_select.name) {
		case "Biologo_button":
			imagen_ayudante = Resources.Load<Sprite> ("Avatares/Ayudantes/biologo_avatar");
			sesion.setAyudante (imagen_ayudante);
			break;
		case "Deportista_button":
			imagen_ayudante = Resources.Load<Sprite> ("Avatares/Ayudantes/deportista_avatar");
			sesion.setAyudante (imagen_ayudante);
			break;
		case "Medico_button":
			imagen_ayudante = Resources.Load<Sprite>("Avatares/Ayudantes/medico_avatar");
			sesion.setAyudante (imagen_ayudante);
			break;
		case "Meteorologo_button":
			imagen_ayudante = Resources.Load<Sprite>("Avatares/Ayudantes/meteorologo_avatar");
			sesion.setAyudante (imagen_ayudante);	
			break;
		default:
			imagen_ayudante = null;
			break;
		}
	}

	public void Msje(){
		if (Mensaje.activeSelf) {
			Mensaje.SetActive (false);
		}else{
			AudioSource sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();
			sonido.clip = Resources.Load ("Musica/Audios/Hombre/Mensaje1") as AudioClip;
			sonido.Play ();
			Mensaje.SetActive (true);
		}
	}
}