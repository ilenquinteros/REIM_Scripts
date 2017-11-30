using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Login_usuario : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public Sesion_controller sesion;
	DatabaseReference reference;
	public GameObject Mensaje;

	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-rean.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Login(string scene){
		UnityEngine.UI.InputField rut = GameObject.Find ("Usuario_input").GetComponent<UnityEngine.UI.InputField> ();
		UnityEngine.UI.InputField pass = GameObject.Find ("Contraseña_input").GetComponent<UnityEngine.UI.InputField> ();

		string passDB = "";
		string colegioDB = "";
		string cursoDB = "";

		DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("Usuarios")
			.GetValueAsync().ContinueWith(task =>
				{
					if (task.IsFaulted)
					{

					}
					else if (task.IsCompleted)
					{
						DataSnapshot snapshot = task.Result;
						DataSnapshot user = snapshot.Child(rut.text);
						if(!user.HasChildren){
							Mensaje.SetActive(true);
						}
						IDictionary dictUser = (IDictionary)user.Value;
						passDB = dictUser["Contrasena"].ToString();
						colegioDB = dictUser["Colegio"].ToString();
						cursoDB = dictUser["Curso"].ToString();

						if (pass.text.Equals (passDB)) {
							sesion.setSesion(System.DateTime.Now.ToString ());
							sesion.setUsuario(rut.text);
							sesion.setColegio(colegioDB);
							sesion.setCurso(cursoDB);

							LoadingSiguiente(scene);
						} else  {
							Mensaje.SetActive(true);
						}
					}
				});
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

	public void Cerrar(){
		if (Mensaje.activeSelf) {
			Mensaje.SetActive (false);
		}else{
			Mensaje.SetActive (true);
		}
	}

	/*
	 * 
	 public IEnumerator Login (string user, string pass){

		Debug.Log (user+" "+pass);

		WWWForm form = new WWWForm ();
		form.AddField ("usernamePost", user);
		form.AddField ("passwordPost", pass);

		WWW www = new WWW (LoginURL, form);

		yield return www;

		Debug.Log (www.text);
	}
	*/
}