using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Slot_controller : MonoBehaviour, IDropHandler {

	public Sesion_controller sesion;

	public GameObject item {
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}


	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		if (!item) {
			Drag_controller.itemBeingDragged.transform.SetParent (transform);
			Drag_controller.itemBeingDragged.transform.localScale = new Vector3 (1f, 1f, 0);
			//GetComponent<UnityEngine.UI.Image> ().enabled = false;
			revisar(Drag_controller.itemBeingDragged.gameObject, this.gameObject);
			if (Drag_controller.itemBeingDragged.transform.childCount > 0 && SceneManager.GetActiveScene ().name!="a5_ciclo_agua") {
				Drag_controller.itemBeingDragged.transform.GetChild (0).gameObject.GetComponent<Text> ().enabled = false;
			}
		}
	}
	#endregion

	public void revisar(GameObject item, GameObject slot){
		if(slot.tag != "Slot"){
			if (item.name == slot.name || ((item.tag == slot.tag) && (item.tag!="Untagged"))) {
				sesion.setAciertos ();
				sesion.setElemento (item.name);
				sesion.enviarData ();
			} else {
				sesion.setErrores ();
				sesion.setElemento (item.name);
				sesion.enviarData ();
			}
		}
	
	}
}
