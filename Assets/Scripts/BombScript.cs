using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

	public GameObject step1;
	public GameObject step2;
	public GameObject step3;
	public GameObject explosion;
	public float timerDuration;
	public float timerExplosion;

	private BombItemScript explosionBomb;
	private List<BombItemScript> bombs = new List<BombItemScript> ();

	private bool onStartBomb = false;
	private bool onBlock;
	private GameObject block;

	private float step = 0;

	void Start () 
	{	
//		GameObject step1Obj = (GameObject)Instantiate(step1);

		bombs.Add (step1.GetComponent<BombItemScript> ());
		bombs.Add (step2.GetComponent<BombItemScript> ());
		bombs.Add (step3.GetComponent<BombItemScript> ());
		bombs.Add (explosion.GetComponent<BombItemScript> ());

		UpdateBombs (step);
	}

	void Update () 
	{
		if (transform.position.y < -6.0f)
			Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		if (!onStartBomb) {
			onBlock = (c.gameObject.tag == ObjectUtils.BLOCK);

			if (onBlock) {
				block = c.gameObject;
				StartCoroutine (StartBlinkBomb (timerDuration));
				onStartBomb = true;
			}
		}
	}

	private IEnumerator StartBlinkBomb(float timer) 
	{
		NextStepBomb ();
		yield return new WaitForSeconds(timer);
		StartCoroutine(StartRedBomb(timerExplosion));
	}

	private IEnumerator StartRedBomb(float timer) 
	{
		NextStepBomb ();
		yield return new WaitForSeconds(timer);
		StartCoroutine(DestroyBlock());
	}

	private IEnumerator DestroyBlock()
	{
		NextStepBomb ();

		yield return new WaitForSeconds(GameUtils.BOMB_ON_TIMER);
		DestroyObject(block);

		yield return new WaitForSeconds(GameUtils.BOMB_EXPLOSION_TIMER);
		DestroyObject(gameObject);
	}

	private void NextStepBomb()
	{
		step++;
		step = step >= bombs.Count ? 0 : step;
		UpdateBombs (step);
	}

	private void DestroyObject(GameObject go)
	{
		bool isValid = go.scene.name != null;
		if (GameObject.Find(go.name) && isValid)
			Destroy (go);
	}

	private void UpdateBombs(float step)
	{
		for (int i = 0; i < bombs.Count; i++) {
			BombItemScript bomb = bombs [i];
			if (step == i) {
				bomb.Show ();
			} else {
				bomb.Hide ();
			}
		}
	}
}
