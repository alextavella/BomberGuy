using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItemScript : MonoBehaviour {

	public void Show(){
		GetComponent<SpriteRenderer>().enabled = true;
	}

	public void Hide(){
		GetComponent<SpriteRenderer>().enabled = false;
	}
}
