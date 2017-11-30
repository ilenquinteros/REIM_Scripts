using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Act2_controller : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	public GameObject victoria;
	public GameObject derrota;
	public GameObject confirmar;

	int cantidadCorrectos = 0;
	int cantidadElementos = 0;
	GameObject desde = null;
	GameObject hacia = null;
	GameObject slot = null;
	GameObject continuar;
	bool fin = false;
	List <string> elementosTotales = new List<string>() {"burro", "cocodrilo", "colibri", "condor", "iguana", "leon", "lobo", "oso", "pato", "payaso", "pinguino", "rana", "raya", "salamandra", "salmon", "sapo", "serpiente", "tiburon", "tortuga"};
	bool infoOpen = false;


	public void EscenaSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
	}

	public void LoadingSiguiente(string scene)
	{
		StartCoroutine (LoadAsynchronously (scene));
		//sesion.enviarData ();
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

		continuar = GameObject.Find ("Revisar");
		continuar.SetActive (false);
		hacia = this.gameObject.transform.GetChild (2).gameObject; //GameObject.Find ("Hacia");
		desde = this.gameObject.transform.GetChild (3).gameObject;
		cantidadElementos = hacia.transform.childCount;
		elementosAzar ();
	}

	// Update is called once per frame
	void Update () {
		int cont = 0;
		for(int i=0; i<desde.transform.childCount; i++){
			if(desde.transform.GetChild (i).gameObject.transform.childCount == 0){
				cont++;
			}
		}

		if (cont == desde.transform.childCount && !fin) {
			continuar.SetActive (true);
			//finalizar();
		} else {
			continuar.SetActive (false);
		}




	}

	public void elementosAzar(){

		List<string> elementos = new List<string>();
		List<int> usados = new List<int>();


		//Ubicar elementos de relleno en el panel "Desde"
		for (int i = 0; i < (desde.transform.childCount); i++) {
			bool libre = true;
			int index;
			do {
				index = Random.Range(0, 19);
				if(!usados.Contains(index)){
					usados.Add(index);
					libre = false;
				}
			} while(libre);

			GameObject elemento = Instantiate (Resources.Load<GameObject>("Elementos_act/Act_cuerpo/Objetos/Act_2/"+elementosTotales[index])) as GameObject;
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
	}

	public void finalizar(){

		cantidadCorrectos = 0;
		for (int i = 0; i < cantidadElementos; i++) {
			slot = hacia.transform.GetChild (i).gameObject;
			if (slot.transform.childCount==1) {
				if (slot.tag.Equals (slot.transform.GetChild (0).gameObject.tag)) {
					cantidadCorrectos++;
				}
			}
		}

		if (cantidadCorrectos == 10) {

			//ENTREGAR LLAVE, HACER METODO APARTE

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

			continuar.SetActive (false);
			GameObject.Find ("Volver_button").gameObject.GetComponent<Animator> ().enabled = true;

			victoria.SetActive (true);
			fin = true;
			sesion.setLlave ();
			sesion.setCompleto (true);
			sesion.setElemento ("-");
			sesion.enviarData ();

		} else {
			Image img = derrota.transform.GetChild (0).GetComponent<Image> ();
			img.sprite = sesion.getAyudante ();
			derrota.SetActive (true);

			AudioSource sonido = GameObject.Find ("Sonido").GetComponent<AudioSource> ();

			switch (sesion.getAyudante ().name) {
			case "biologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac2error") as AudioClip;
				break;
			case "deportista_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac2error") as AudioClip;
				break;
			case "medico_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac2error") as AudioClip;
				break;
			case "meteorologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac2error") as AudioClip;
				break;
			default:
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac2error") as AudioClip;
				break;
			}

			sonido.Play ();


		}
	}

	public void Close(){
		
		derrota.SetActive (false);
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
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac2") as AudioClip;
				break;
			case "deportista_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac2") as AudioClip;
				break;
			case "medico_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Mujer/Ac2") as AudioClip;
				break;
			case "meteorologo_avatar":
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac2") as AudioClip;
				break;
			default:
				sonido.clip = Resources.Load ("Musica/Audios/Hombre/Ac2") as AudioClip;
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
