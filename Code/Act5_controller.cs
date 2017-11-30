using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Act5_controller : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	public GameObject desde;
	public GameObject botones;
	public GameObject victoria;
	public GameObject confirmar;
	public GameObject ciclo;
	bool infoOpen = false;

	public GameObject Mensaje;
	public GameObject Libre;

	string textoActual = null;
	Button botonAnterior = null;
	Button botonActual = null;
	bool destruir = false;
	float tiempo = 0.0f;
	float espera = 2.0f;
	bool activo = false;
	int contador = 0;

	List <string> elementosTotales = new List<string>() {"Lago", "Rio", "FlujoSub", "Condensacion", "Evaporacion", "Deshielo", "Precipitacion", "Infiltracion","Lago", "Rio", "FlujoSub", "Condensacion", "Evaporacion", "Deshielo", "Precipitacion", "Infiltracion"};

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




	// Use this for initialization
	void Start () {
		sesion.setStartTime (Time.time);
		sesion.setCompleto (false);
		sesion.resetAciertos ();
		sesion.resetErrores ();
		sesion.resetAccesoAyuda ();
		sesion.setElemento ("-");
		sesion.enviarData ();
		elementosAzar();
		Ocultar ();
	}
	
	// Update is called once per frame
	void Update () {

		if (activo) {
			tiempo += Time.deltaTime;
			if (tiempo >= espera) {
				Ocultar ();
				if (destruir) {
					Destruir ();
				}
			}
		} else {
			tiempo = 0.0f;
		}

		if (contador == 8) {
			GameObject.Find ("Memoria").SetActive (false);
			ciclo.SetActive (true);
			contador++;
		}
		if (contador == 9) {
			int cantidadCorrectos = 0;
			for (int i = 0; i < 8; i++) {
				GameObject slot = ciclo.transform.GetChild (i).gameObject;
				if (slot.transform.childCount==1) {
					if (slot.name.Equals (slot.transform.GetChild (0).gameObject.name)) {
						cantidadCorrectos++;
						if (cantidadCorrectos == 8) {
							contador++;
							Finalizar ();
						}
					}
				}
			}
		}
	}

	public void elementosAzar(){

		List<int> usados = new List<int>();

		for(int i = 0; i < botones.transform.childCount; i++){
			bool libre = true;
			do {
				int index = Random.Range(0, 16);
				if(!usados.Contains(index)){
					usados.Add(index);
					string estado = elementosTotales [index];
					botones.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text = estado;
					libre = false;
				}
			} while(libre);
		}
	}


	public void Ver(Button boton){
		if (destruir) {
			Destruir ();
		}
		if (activo) {
			Ocultar ();


			boton.transform.GetChild(0).GetComponent<Text>().enabled = true;
			boton.GetComponent<Image> ().color = setElemento (boton.transform.GetChild (0).GetComponent<Text> ().text);

		} else {
			

			boton.transform.GetChild(0).GetComponent<Text>().enabled = true;
			boton.GetComponent<Image> ().color = setElemento (boton.transform.GetChild (0).GetComponent<Text> ().text);
		}
		if (textoActual != null && !botonAnterior.Equals(boton)) {
			if (boton.transform.GetChild(0).gameObject.GetComponent<Text>().text.Equals (textoActual)) {
				for (int i = 0; i < desde.transform.childCount; i++) {
					GameObject hijo = desde.transform.GetChild (i).gameObject;
					if (hijo.transform.childCount == 0) {
						GameObject etapa = Instantiate (Resources.Load<GameObject> ("Elementos_act/Act_clima/Objetos/" + textoActual)) as GameObject;
						etapa.name = etapa.name.Replace ("(Clone)", "");
						etapa.transform.SetParent (hijo.transform);
						i = desde.transform.childCount;
						botonActual = boton;
						destruir = true;
						activo = true;
						contador++;
						sesion.setAciertos ();
						sesion.enviarData ();
					}
				}
			} else {
				activo = true;
				sesion.setErrores ();
				sesion.enviarData ();
			}
		} else {
			botonAnterior = boton;
			textoActual = boton.transform.GetChild(0).gameObject.GetComponent<Text> ().text;
		}	
	}

	public void Ocultar (){
		activo = false;
		textoActual = null;
		for(int i = 0; i < botones.transform.childCount; i++){
			botones.transform.GetChild (i).gameObject.transform.GetChild(0).GetComponent<Text>().enabled = false;
			Color color = new Color32 (141, 137, 255,255);
			botones.transform.GetChild (i).gameObject.GetComponent<Image> ().color = color;
		}
	}

	public void Destruir(){
		botonActual.GetComponent<Image> ().enabled = false;
		botonAnterior.GetComponent<Image> ().enabled = false;
		destruir = false;
		botonActual = null;
		botonAnterior = null;
	}

	public void Finalizar(){
		
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

	public void Confirmar(){
		if (confirmar.activeSelf) {
			confirmar.SetActive (false);
		}else{
			confirmar.SetActive (true);
		}
	}



	public Color setElemento (string elemento){



		Color color;

		switch (elemento){
		case "Lago":
			color = new Color32 (197, 101, 135, 255);
			//array = new string[] { "C56587FF", "Lago" };
			break;
		case "Rio":
			color = new Color32 (101, 197, 110, 255);
			//array = new string[] {"65C56EFF","Río"};
			break;
		case "FlujoSub":
			color = new Color32 (197, 190, 101, 255);
			//array = new string[] {"C5BE65FF","Flujo Subterráneo"};
			break;
		case "Condensacion":
			color = new Color32 (197, 101, 101, 255);
			//array = new string[] {"C56565FF","Condensación"};
			break;
		case "Evaporacion":
			color = new Color32 (101, 197, 177, 255);
			//array = new string[] {"65C5B1FF","Evaporación"};
			break;
		case "Deshielo":
			color = new Color32 (142, 142, 142, 255);
			//array = new string[] {"8E8E8EFF","Deshielo"};
			break;
		case "Precipitacion":
			color = new Color32 (109, 101, 197, 255);
			//array = new string[] {"6D65C5FF","Precipitación"};
			break;
		case "Infiltracion":
			color = new Color32 (197, 101, 182, 255);
			//array = new string[] {"C565B6FF","Infiltración"};
			break;
		default:
			color = new Color32 (255, 255, 255, 255);
			//array = new string[] {"FFFFFFFF",""};
			break;
		}
			
		Debug.Log (elemento);
		Debug.Log (color);
		return color;
	}
}
