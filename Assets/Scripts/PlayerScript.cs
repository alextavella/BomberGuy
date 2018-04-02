using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float speed;
	public float weight;
	public float maxLeft, maxRight;

	public Transform chaoVerificador;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb;
	private Animator animator;

	private bool isColliderPiso;

	// Use this for initialization
	void Start () {

		// Interface for components
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Move
		float move_x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
		transform.Translate (move_x, 0.0f, 0.0f);

		// Animator running
		animator.SetFloat ("pRunning", Math.Abs(move_x));

		// Limit
		float position_x = transform.position.x;
		position_x = Math.Min (position_x, maxRight);
		position_x = Math.Max (position_x, maxLeft);
		transform.position = new Vector2 (position_x, transform.position.y);

		// Verify collision with 'piso'
		isColliderPiso = Physics2D.Linecast(
			transform.position,
			chaoVerificador.position, 
			1 << LayerMask.NameToLayer(LayerUtils.BLOCK)
		);

		// Orientation view
		if (move_x > 0) { 
			spriteRenderer.flipX = false;
		} else if (move_x < 0) {
			spriteRenderer.flipX = true;
		}

		// Jump
		animator.SetBool ("pJumping", !isColliderPiso);

		if (Input.GetButtonDown (ButtonUtils.JUMP) && isColliderPiso) {
			rb.velocity = new Vector2 (0.0f, weight);
		}

		// Fire
		animator.SetBool ("pHold", Input.GetButton(ButtonUtils.HOLD));

		// Dead
//		if (life <= 0) {
//			Destroy (gameObject);
//		}
	}

//	void OnTriggerEnter2D(Collider2D c){
//		// Destroy sub enemy
//		if (c.tag == "SubInimigo") {
//			Destroy (c.gameObject);
//			life--;
//		}
//	}
}
