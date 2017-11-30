using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controlador_escena : MonoBehaviour
{
	public Canvas Info_canvas;
	bool infoOpen = false;
	public GameObject loadingScreen;
	public Slider slider;
	public Text progressText;

	public void EscenaSiguiente(string scene)
	{
		SceneManager.LoadScene(scene);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive); Keep load scene
	}

	public void LoadingSiguiente(string scene)
	{
			StartCoroutine (LoadAsynchronously (scene));
	}

	IEnumerator LoadAsynchronously (string scene){
		AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

		loadingScreen.SetActive (true);

		while(!operation.isDone){
			float progress = Mathf.Clamp01 (operation.progress / .9f);

			slider.value = progress;
			progressText.text = Mathf.Round(progress * 100f) + "%";

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
}