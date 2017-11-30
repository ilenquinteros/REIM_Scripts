using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selec_mascota_controller : MonoBehaviour
{
	public Canvas Info_canvas;
	bool infoOpen = false;
	public GameObject loadingScreen;
	public Slider slider;

	string nombre_mascota = null;
	string descripcion_mascota = null;
	Sprite imagen_mascota = null;
	GameObject button_pressed = null;
	private Animator animator;
	public Sesion_controller sesion;

	public GameObject Mensaje;



	void Start(){
		sesion.setStartTime (Time.time);
	}

	public void EscenaSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
	}

	public void LoadingSiguiente(string scene)
	{
		if(Sesion_controller.mascota!=null){

			sesion.registrarMascota ();

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

	public void Confirmar(){
		if (infoOpen == false) {
			infoOpen = true;
			Info_canvas.enabled = true;
		} else if (infoOpen == true) {
			infoOpen = false;
			Info_canvas.enabled = false;
		}
	}

	public void infoPanel(){
		string sound = "";
		string mascota_select = EventSystem.current.currentSelectedGameObject.name;
		UnityEngine.UI.Text Nombre = GameObject.Find ("Nombre").GetComponent<UnityEngine.UI.Text> ();
		UnityEngine.UI.Text Descripcion = GameObject.Find ("Descripcion").GetComponent<UnityEngine.UI.Text> ();
		UnityEngine.UI.Image Imagen = GameObject.Find ("Imagen").GetComponent<UnityEngine.UI.Image> ();
		AudioSource Sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();

		switch (mascota_select) {
		case "Nombre_tigre":
			nombre_mascota = "TIGRE";
			descripcion_mascota = "El tigre vive en bosques y selvas de Asia y es el más grande de los felinos, midiendo entre un 1 metro y medio y 3 metros. Pesa entre 100 y 360 kilos. Vive entre 15 y 20 años y es una especie en grave peligro de extinción.";
			imagen_mascota = Resources.Load<Sprite>("Avatares/Mascotas/tigre_avatar");
			sound = "Musica/Audios/Hombre/Tigre";
			break;
		case "Nombre_elef":
			nombre_mascota = "ELEFANTE";
			descripcion_mascota = "El elefante es mamífero terrestre más grande del mundo y son además extremadamente inteligentes. Pueden llegar a vivir más de 70 años, llegan a medir 5 metros de altura, y pesan entre 2000 a 6000 kilogramos.";
			imagen_mascota = Resources.Load<Sprite>("Avatares/Mascotas/elefante_avatar");
			sound = "Musica/Audios/Hombre/Elefante2";
			break;
		case "Nombre_panda":
			nombre_mascota = "OSO PANDA";
			descripcion_mascota = "El panda, sólo vive en regiones de China, en las montañas donde hay grandes bosques de bambú, el cual es su principal alimento. Tiene un tamaño de 1 a 1 metro y medio y pesa unos 140 kilos. Puede llegar a vivir unos 20 años.s";
			imagen_mascota = Resources.Load<Sprite>("Avatares/Mascotas/panda_avatar");
			sound = "Musica/Audios/Hombre/Panda";
			break;
		case "Nombre_rino":
			nombre_mascota = "RINOCERONTE";
			descripcion_mascota = "El rinoceronte posee dos cuernos, siendo el primero el más grande. Estos cuernos pueden llegar a alcanzar el metro y medio. El rinoceronte negro, mide entre 1.5 y 2 metros; y pesa entre 800 y 1400 kg. Viven entre 35 a 50 años.";
			imagen_mascota = Resources.Load<Sprite>("Avatares/Mascotas/rino_avatar");
			sound = "Musica/Audios/Hombre/Rino";
			break;
		case "Nombre_leo":
			nombre_mascota = "LEOPARDO DE LAS NIEVES";
			descripcion_mascota = "El leopardo de las nieves tiene un con un pelaje voluminoso y fino, de color gris que utiliza para ocultarse. Habita en montañas, mide aproximadamente 60 cm. de alto y llega a pesar 55 kilos. Puede vivir entre 10 y 12 años.";
			imagen_mascota = Resources.Load<Sprite>("Avatares/Mascotas/leo_avatar");
			sound = "Musica/Audios/Hombre/Leoap";
			break;
		case "Nombre_polar":
			nombre_mascota = "OSO POLAR";
			descripcion_mascota = "El oso polar vive sólo donde hace mucho frío, alrededor de la región Ártica. Su grueso pelaje mantiene el calor corporal y evita así que se congelen. Vive entre 25 a 30 años. Su cuerpo mide entre 2 y 2 metros y medio. Pesa entre 400 y 730 kilos.";
			imagen_mascota = Resources.Load<Sprite>("Avatares/Mascotas/polar_avatar");
			sound = "Musica/Audios/Hombre/Polar";
			break;
		default:
			nombre_mascota = "NN";
			descripcion_mascota = "NN";
			sound = "Musica/Audios/Hombre/Polar";
			break;
		}

		Info_canvas.enabled = true;
		Nombre.text = nombre_mascota;
		Descripcion.text = descripcion_mascota;
		Imagen.sprite = imagen_mascota;
		Sonido.clip = Resources.Load (sound) as AudioClip;
	}

	public void Close(){
		Info_canvas.enabled = false;
	}

	public void seleccionMascota(){
		if (button_pressed != null) {
			animator = button_pressed.GetComponent<Animator>();
			animator.ResetTrigger ("select");
			animator.SetTrigger ("unselect");
		}

		button_pressed = EventSystem.current.currentSelectedGameObject;
		animator = button_pressed.GetComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load("Animaciones/Mascota") as RuntimeAnimatorController;
		animator.SetTrigger ("select");

		string mascota_select = EventSystem.current.currentSelectedGameObject.name;
		sesion.setMascota (mascota_select);
	}

	public void Msje(){
		if (Mensaje.activeSelf) {
			Mensaje.SetActive (false);
		}else{
			AudioSource sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();
			sonido.clip = Resources.Load ("Musica/Audios/Hombre/Mensaje2") as AudioClip;
			sonido.Play ();
			Mensaje.SetActive (true);
		}
	}
}
