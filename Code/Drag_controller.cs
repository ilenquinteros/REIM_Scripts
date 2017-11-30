using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Drag_controller : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;
	public float onDragSize;
	public float endDragSize;


	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
		itemBeingDragged.transform.localScale = new Vector3 (onDragSize, onDragSize, 0);
		if (itemBeingDragged.transform.childCount > 0) {
			itemBeingDragged.transform.GetChild (0).gameObject.GetComponent<Text> ().enabled = true;
		}
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		if (itemBeingDragged.transform.childCount > 0 && SceneManager.GetActiveScene ().name!="a5_ciclo_agua") {
			itemBeingDragged.transform.GetChild (0).gameObject.GetComponent<Text> ().enabled = false;
		}
		itemBeingDragged.transform.localScale = new Vector3 (endDragSize, endDragSize, 0);
		itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (transform.parent == startParent) {
			transform.position = startPosition;
		}

	}

	#endregion

}
