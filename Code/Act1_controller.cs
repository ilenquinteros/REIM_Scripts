using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Act1_controller : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	public GameObject victoria;
	public GameObject derrota;
	public GameObject confirmar;

	//Variables de actividad
	int sistemaActual = 1;
	bool correcto = false;
	int cantidadElementos = 1;
	int cantidadCorrectos = 0;
	GameObject desde = null;
	GameObject sistema = null;
	GameObject slot = null;
	GameObject continuar;
	List <string> elementosTotales = new List<string>() {"Cerebro", "Corazon", "Pulmones", "Estomago", "Higado", "Intestinos", "Craneo", "Humero", "Radio", "Costillas", "Coxal", "Femur"};
	bool infoOpen = false;


	public void EscenaSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
	}

	public void LoadingSiguiente(string scene)
	{
		StartCoroutine (LoadAsynchronously (scene));
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
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

		continuar = GameObject.Find ("Continuar");
		continuar.SetActive (false);
		sistema = this.gameObject.transform.GetChild (sistemaActual).gameObject;
		desde = this.gameObject.transform.GetChild (5).gameObject;
		elementosAzar ();
	}

	// Update is called once per frame
	void Update () {
		cantidadCorrectos = 0;
		if (sistemaActual < 5) {
			for (int i = 0; i < cantidadElementos; i++) {
				slot = sistema.transform.GetChild (i).gameObject;
				if (slot.transform.childCount==1) {
					if (slot.name.Equals (slot.transform.GetChild (0).gameObject.name)) {
						cantidadCorrectos++;
						if (cantidadCorrectos == cantidadElementos) {
							continuar.SetActive (true);
						}
					}
				}
			}
		}

	}


	public void Continuar(){
		
		sistemaActual++;
		sistema.SetActive (false);
		if (sistemaActual < 5) {
			sistema = this.gameObject.transform.GetChild (sistemaActual).gameObject;

			GameObject.Find("Titulos").gameObject.transform.GetChild (sistemaActual-1).gameObject.SetActive (true);

			if (sistemaActual > 1) {
				GameObject.Find("Titulos").gameObject.transform.GetChild (sistemaActual-2).gameObject.SetActive(false);
			}

			sistema.SetActive (true);
			continuar.SetActive (false);
			cantidadElementos = sistema.transform.childCount;

			for (int i = 0; i < 10; i++) {
				if (desde.transform.GetChild (i).gameObject.transform.childCount == 1) {
					GameObject hijo = desde.transform.GetChild (i).gameObject.transform.GetChild (0).gameObject;
					desde.transform.GetChild (i).gameObject.transform.DetachChildren ();
					Destroy (hijo);
				}
			}
			elementosAzar ();
		}else{
			GameObject.Find("Titulos").gameObject.transform.GetChild (sistemaActual-2).gameObject.SetActive(false);
			finalizar ();
		}
	}

	public void elementosAzar(){

		List<string> elementos = new List<string>();
		List<int> usados = new List<int>();

		//Obtener elementos de panel "Hacia"
		for (int i = 0; i < cantidadElementos; i++) {
			slot = sistema.transform.GetChild (i).gameObject;
			usados.Add(elementosTotales.IndexOf (slot.name));
			elementos.Add (slot.name);
		}


		//Ubicar elementos de relleno en el panel "Desde"
		for (int i = 0; i < (desde.transform.childCount - elementos.Count); i++) {
			bool libre = true;
			int index;
			do {
				index = Random.Range(0, 12);
				if(!usados.Contains(index)){
					usados.Add(index);
					libre = false;
				}
			} while(libre);

			GameObject elemento = Instantiate (Resources.Load<GameObject>("Elementos_act/Act_cuerpo/Objetos/Act_1/"+elementosTotales[index])) as GameObject;
			elemento.name = elemento.name.Replace ("(Clone)", "");
			bool ubicado = false;


			//Ubicar aleatoreamente
			do{
				int random = Random.Range(0, 10);
				if(desde.transform.GetChild(random).transform.childCount!=1){
					elemento.transform.parent = desde.transform.GetChild (random).transform;
					ubicado = true;
				}
			}while(!ubicado);
		}



		//Ubicar elementos correctos en el panel "Desde"
		int ubicacion = 0;
		for (int i = 0; i < (elementos.Count); i++) {


			GameObject elemento = Instantiate (Resources.Load<GameObject>("Elementos_act/Act_cuerpo/Objetos/Act_1/"+elementos[i])) as GameObject;
			elemento.name = elemento.name.Replace ("(Clone)", "");
			bool ubicado = false;


			//Ubicar aleatoreamente

			do{
				if(desde.transform.GetChild(ubicacion).transform.childCount!=1){
					elemento.transform.parent = desde.transform.GetChild (ubicacion).transform;
					ubicado = true;
				}else{
					ubicacion++;
				}
			}while(!ubicado);
		}


	}

	public void finalizar(){
		for (int i = 0; i < 10; i++) {
			if(desde.transform.GetChild (i).gameObject.transform.childCount==1){
				GameObject hijo = desde.transform.GetChild (i).gameObject.transform.GetChild (0).gameObject;
				desde.transform.GetChild (i).gameObject.transform.DetachChildren ();
				Destroy (hijo);
			}
		}
		continuar.SetActive (false);
		//sistema = this.gameObject.transform.GetChild (6).gameObject;
		//sistema.SetActive (true);
		//yield WaitForSeconds (5);
		sistema.SetActive (true);
		sistema = this.gameObject.transform.GetChild (7).gameObject;
		sistema.SetActive (true);



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
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac1") as AudioClip;
				break;
			case "deportista_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac1") as AudioClip;
				break;
			case "medico_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac1") as AudioClip;
				break;
			case "meteorologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac1") as AudioClip;
				break;
			default:
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac1") as AudioClip;
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


}
