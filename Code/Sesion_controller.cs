using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Sesion_controller : MonoBehaviour {

	//Data
	static string sesionId;
	string logOut;
	static string actividad;
	static float startTime;
	static int aciertos = 0;
	static int errores = 0;
	static string elemento = "-";
	static bool completa = false;
	static int accesoAyuda = 0;
	static float ejecucion;
	static string fecha;

	//User
	static string Usuario;
	static string Colegio;
	static string Curso;

	//Juego
	public static Sprite imagen_ayudante = null;
	public static string mascota = null;
	public static int llaves = 0;  
	public static Sesion_controller sesion;

	//DB


	void Awake(){
		if (sesion == null) {
			sesion = this;
			DontDestroyOnLoad (this);
		}else if(sesion!=this){
				Destroy(gameObject);
		}
				
	}

	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-rean.firebaseio.com/");

	}

	// Update is called once per frame
	void Update () {
	}
		

	public Sprite getAyudante(){
		return imagen_ayudante;
	}

	public string getMascota(){
		return mascota;
	}

	public void setAyudante(Sprite ayudante){
		imagen_ayudante = ayudante;
	}

	public void setMascota(string m){
		mascota = m;
	}

	public int getLlave (){
		return llaves;
	}

	public void setLlave (){
		if(llaves<3){
			llaves ++;
		}
	}

	public void resetKeys(){
		llaves = 0;
	}

	public void setSesion(string sesion){
		sesionId = sesion;
	}

	public void setStartTime (float st){
		startTime = st;
	}

	public float getStartTime(){
		return startTime;
	}

	public void setAciertos() {
		aciertos += 1;
	}

	public int getAciertos(){
		return aciertos;
	}

	public void resetAciertos(){
		aciertos = 0;
	}
		
	public void setErrores() {
		errores += 1;
	}

	public void resetErrores(){
		errores = 0;
	}

	public void setElemento (string e){
		elemento = e;
	}

	public void setCompleto(bool c){
		completa = c;
	}

	public void setAccesoAyuda() {
		accesoAyuda += 1;
	}

	public void resetAccesoAyuda(){
		accesoAyuda = 0;
	}

	public void setUsuario (string user){
		Usuario = user;
	}

	public void setColegio(string col){
		Colegio = col;
	}

	public void setCurso(string cur){
		Curso = cur;
	}

	public void setLogout(string logOut){
		this.logOut = logOut;
	}
		

	public void enviarData(){

		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		actividad = SceneManager.GetActiveScene ().name;
		ejecucion = Time.time - startTime;
		fecha = System.DateTime.Now.ToString ();
		fecha = fecha.Replace ("/", "-");

		Resultados result = new Resultados (accesoAyuda, aciertos, errores, elemento, completa, ejecucion.ToString(), fecha);
		string json = JsonUtility.ToJson(result);
		reference.Child("Colegios").Child(Colegio).Child(Curso).Child("Alumnos").Child(Usuario).Child("Sesiones").Child(sesionId).Child("Resultados").Child(actividad).Child(fecha).SetRawJsonValueAsync(json);

	}

	public void registrarAyudante(){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		string Ayudante = imagen_ayudante.name.Replace ("_avatar", "");
		//sesionId = sesionId.Replace ("/", "-");
		reference.Child("Colegios").Child(Colegio).Child(Curso).Child("Alumnos").Child(Usuario).Child("Sesiones").Child(sesionId).Child("Ayudante").SetValueAsync(Ayudante);
	}

	public void registrarMascota(){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		
		string Mascota = mascota.Replace ("_button", "");
		//sesionId = sesionId.Replace ("/", "-");
		reference.Child("Colegios").Child(Colegio).Child(Curso).Child("Alumnos").Child(Usuario).Child("Sesiones").Child(sesionId).Child("Mascota").SetValueAsync(Mascota);
	}

	public void registrarLogOut(){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		//sesionId = sesionId.Replace ("/", "-");
		reference.Child("Colegios").Child(Colegio).Child(Curso).Child("Alumnos").Child(Usuario).Child("Sesiones").Child(sesionId).Child("LogOut").SetValueAsync(logOut);
	}

}
