using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resultados : MonoBehaviour {

	public int Ayuda;
	public int Aciertos;
	public int Errores;
	public string Elemento;
	public bool Completa;
	public string TiempoEjecucion;
	public string Fecha;

	public Resultados(int ayuda, int aciertos, int errores, string elemento, bool completa, string ejecucion, string fecha){
		this.Ayuda = ayuda;
		this.Aciertos = aciertos;
		this.Errores = errores;
		this.Elemento = elemento;
		this.Completa = completa;
		this.TiempoEjecucion = ejecucion;
		this.Fecha = fecha;
	}
}
