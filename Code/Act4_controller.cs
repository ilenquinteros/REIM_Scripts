using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Act4_controller : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	public GameObject hacia;
	public GameObject victoria;
	public GameObject confirmar;
	bool infoOpen = false;

	public GameObject Mensaje;
	public GameObject Libre;

	float tiempo = 0.0f;
	float espera = 2.0f;
	bool salir = true;
	float reloj = 60.0f;
	public GameObject guiReloj;


	public void LoadingSiguiente(string scene)
	{
		StartCoroutine (LoadAsynchronously (scene));
		//sesion.enviarData ();
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



	void Start () {
		sesion.setStartTime (Time.time);
		sesion.setCompleto (false);
		sesion.resetAciertos ();
		sesion.resetErrores ();
		sesion.resetAccesoAyuda ();
		sesion.setElemento ("-");
		sesion.enviarData ();

		//Genera 10 invertebrados al azar
		for (int i = 0; i < 10; i++) {
			GameObject insecto = Instantiate (Resources.Load<GameObject>("Elementos_act/Act_invert/Objetos/Act_4/"+setInsecto())) as GameObject;
			insecto.transform.SetParent (GameObject.Find("Inicio").transform);
			insecto.name = insecto.name.Replace ("(Clone)", "");
			insecto.transform.position = GameObject.Find ("Inicio").transform.position;
		}
	}

	// Update is called once per frame
	void Update () {

		reloj -= Time.deltaTime;
		guiReloj.GetComponent<Text> ().text = "" + reloj.ToString ("f0");


		if (reloj < 0.0f && salir) {
			finalizar ();
		}

		GameObject.Find("Cantidad").GetComponent<Text> ().text = "Llevas "+sesion.getAciertos()+" capturados";

		//Quita los hijos de los elementos de error y acierto
		GameObject[] slots_area = GameObject.FindGameObjectsWithTag ("Slot_area");
		foreach (GameObject slot in slots_area) {
			if (slot.transform.childCount > 0) {
				slot.transform.GetChild (0).transform.SetParent (GameObject.Find("Insectos").transform);
			}
		}

		//Ubica insectos al azar en la pantalla
		for (int i = 0; i < hacia.transform.childCount; i++) {
			GameObject slot = hacia.transform.GetChild (i).gameObject;
			if (slot.transform.childCount > 0) {
				//Si hay acierto genera uno nuevo en la esquina inferior derecha
				if (slot.tag.Equals (slot.transform.GetChild (0).tag)) {
					Destroy (slot.transform.GetChild(0).gameObject);
					GameObject insecto = Instantiate (Resources.Load<GameObject>("Elementos_act/Act_invert/Objetos/Act_4/"+setInsecto())) as GameObject;
					insecto.transform.SetParent (GameObject.Find("Inicio").transform);

					Mensaje.SetActive (true);
					Text Texto = Mensaje.GetComponent<Text> ();
					Texto.text = "!Lo has atrapado!";
					Texto.color = Color.blue;

					switch (slot.tag) {
					case "Insecto":
						reloj += 3.0f;
						Mensaje.transform.GetChild(0).GetComponent<Text>().text = "+3";
						break;
					case "Aracnido":
						reloj += 4.0f;
						Mensaje.transform.GetChild(0).GetComponent<Text>().text = "+4";
						break;
					case "Anelido":
						reloj += 2.0f;
						Mensaje.transform.GetChild(0).GetComponent<Text>().text = "+2";
						break;
					case "Molusco":
						reloj += 1.0f;
						Mensaje.transform.GetChild(0).GetComponent<Text>().text = "+1";
						break;
					default:
						reloj += 0.0f;
						Mensaje.transform.GetChild(0).GetComponent<Text>().text = "";
						break;
					}


				//Si hay error devuelve el invertebrado al centro de la pantalla
				} else {
					slot.transform.GetChild(0).transform.SetParent (GameObject.Find("Error").transform);
					Mensaje.transform.GetChild(0).GetComponent<Text>().text = "";
					Mensaje.SetActive (true);
					Text Texto = Mensaje.GetComponent<Text> ();
					Texto.text = "!Se ha escapado!";
					Texto.color = Color.red;
				}
			}
		}

		if (Mensaje.activeSelf) {
			tiempo += Time.deltaTime;
			if (tiempo >= espera) {
				Mensaje.SetActive (false);
			}
		} else {
			tiempo = 0.0f;
		}

	}


	//Insecto al azar
	public string setInsecto(){
		int insecto = Random.Range (0, 4);
		switch(insecto){
			case 0:
				return "Abeja";
				break;
			case 1:
				return "Araña";
				break;
			case 2:
				return "Caracol";
				break;
			case 3:
				return "Lombriz";
				break;
			default:
				return "Abeja";
				break;
		}
	}
		
	public void infoPanel(Canvas Info_canvas){
		if (infoOpen == false) {
			infoOpen = true;
			UnityEngine.UI.Image Imagen = GameObject.Find ("Imagen").GetComponent<UnityEngine.UI.Image> ();
			Imagen.sprite = sesion.getAyudante ();
			Info_canvas.enabled = true;
			sesion.setAccesoAyuda ();


			AudioSource sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();

			switch (sesion.getAyudante ().name) {
			case "biologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac4") as AudioClip;
				break;
			case "deportista_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac4") as AudioClip;
				break;
			case "medico_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac4") as AudioClip;
				break;
			case "meteorologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac4") as AudioClip;
				break;
			default:
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac4") as AudioClip;
				break;
			}

			sonido.Play ();


		} else if (infoOpen == true) {
			infoOpen = false;
			Info_canvas.enabled = false;
		}
	}


	public void finalizar(){
		confirmar.SetActive (false);
		guiReloj.SetActive (false);
		Mensaje.transform.GetChild(0).GetComponent<Text>().text = "";
		if (salir) {

			//Se ocultan parte de los elementos de la pantalla
			GameObject.Find("Cantidad").gameObject.GetComponent<Text>().enabled = false;
			GameObject.Find ("Ducto").gameObject.SetActive (false);		
			GameObject.Find ("Image").gameObject.SetActive (false);
			GameObject.Find ("Bordes").gameObject.SetActive (false);
			GameObject.Find ("Insectos").gameObject.SetActive (false);
			GameObject.Find ("Nombres").gameObject.SetActive (false);
			GameObject.Find ("Hacia").gameObject.SetActive (false);


			Image Fondo = GameObject.Find ("Background").GetComponent<Image> ();
			Fondo.sprite = Resources.Load <Sprite> ("Fondos/fondo_act4_2");
			Libre.SetActive (true);

			int insectos = sesion.getAciertos ();
			if (insectos > 20) {
				insectos = 20;
			}

			//Se muestra el mensaje de cuantos invertebrados fueron capturados
			Mensaje.SetActive (true);
			Text Texto = Mensaje.GetComponent<Text> ();
			Texto.text = "Capturaste "+sesion.getAciertos ()+" invertebrados";
			Texto.color = Color.white;
			espera = 200.0f;

			//Se liberan invertebrados
			for (int i = 0; i < insectos; i++) {
				GameObject insecto = Instantiate (Resources.Load<GameObject> ("Elementos_act/Act_invert/Objetos/Act_4/" + setInsecto ())) as GameObject;


				insecto.GetComponent<Insecto_controlador> ().setEsperaX(100.0f);
				insecto.GetComponent<Insecto_controlador> ().setEsperaY(100.0f);
				insecto.GetComponent<Insecto_controlador> ().speed = 4.0f;

				insecto.transform.SetParent (GameObject.Find ("Canvas").transform);
				insecto.name = insecto.name.Replace ("(Clone)", "");
				insecto.transform.position = GameObject.Find ("Libre").transform.position;
			}
			salir = false;

			//Si la cantidad de insectos es mayor a 0, gana una llave
			if (insectos > 0) {
				Image img = victoria.transform.GetChild (1).GetComponent<Image> ();
				img.sprite = sesion.getAyudante ();
				Image img_ll = victoria.transform.GetChild (2).GetComponent<Image> ();

				switch(sesion.getLlave()){
				case 0:
					img_ll.sprite = Resources.Load<Sprite>("Llaves/key_1");
					break;
				case 1:
					img_ll.sprite = Resources.Load<Sprite>("Llaves/key_2");
					break;
				case 2:
					img_ll.sprite = Resources.Load<Sprite>("Llaves/key_3");
					break;
				case 3:
					victoria.transform.GetChild (2).gameObject.SetActive (false);
					break;
				default:
					victoria.transform.GetChild (2).gameObject.SetActive (false);
					break;
				}



				AudioSource sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();

				switch (sesion.getAyudante ().name) {
				case "biologo_avatar":
					sonido.clip = Resources.Load ("Musica/Audios/Hombre/Feliz") as AudioClip;
					break;
				case "deportista_avatar":
					sonido.clip = Resources.Load ("Musica/Audios/Mujer/Feliz") as AudioClip;
					break;
				case "medico_avatar":
					sonido.clip = Resources.Load ("Musica/Audios/Mujer/Feliz") as AudioClip;
					break;
				case "meteorologo_avatar":
					sonido.clip = Resources.Load ("Musica/Audios/Hombre/Feliz") as AudioClip;
					break;
				default:
					sonido.clip = Resources.Load ("Musica/Audios/Hombre/Feliz") as AudioClip;
					break;
				}

				sonido.Play ();

				GameObject.Find ("Volver_button").gameObject.GetComponent<Animator> ().enabled = true;

				victoria.SetActive (true);
				sesion.setLlave ();
				sesion.setCompleto (true);
				sesion.setElemento ("-");
				sesion.enviarData ();
			}

		} else {
			LoadingSiguiente ("mapa_scene");
		}
	}

	public void Confirmar(){
		if (confirmar.activeSelf) {
			confirmar.SetActive (false);
		}else{
			confirmar.SetActive (true);
		}
	}
}
