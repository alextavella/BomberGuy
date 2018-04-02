using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGunScript : MonoBehaviour 
{
	public GameObject bomb;
	public float minInterval, maxInterval; 
	public float maxLeft, maxRight;

	IEnumerator Start () {
		
		Vector2 positionBomb = new Vector2(
			Random.Range(maxLeft, maxRight),
			transform.position.y
		);

//		print (positionBomb);
		print(bomb);
		Instantiate (bomb, positionBomb, transform.rotation);

		yield return new WaitForSeconds (Random.Range(minInterval, maxInterval));
		StartCoroutine (Start ());
	}
}
