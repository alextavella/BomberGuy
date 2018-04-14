using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float speed;
	public float weight;
	public float maxLeft, maxRight;

	public Transform chaoVerificador;
	//	public Transform ignored;
	public Transform normalBomb;
	public Transform explosionBomb;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb;
	private Animator animator;

	private GameObject bombCollision;
	private bool isColliderPiso = false;
	private bool isColliderBomb = false;

	private SpriteRenderer normalBombSpriteRenderer;
	private BombItemScript normalBombScript;
	private BombItemScript explosionBombScript;

	// Use this for initialization
	void Start () {

		// Interface for components
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();

		normalBombSpriteRenderer = normalBomb.GetComponent<SpriteRenderer>();

		normalBombScript = normalBomb.GetComponent<BombItemScript> ();
		normalBombScript.Hide ();

//		Physics.IgnoreCollision(ignored.GetComponent<Collider>(), GetComponent<Collider>());
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
			normalBombSpriteRenderer.flipX = false;
			spriteRenderer.flipX = false;
		} else if (move_x < 0) {
			normalBombSpriteRenderer.flipX = true;
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

		// Dead
		if (transform.position.y <= -6) {
			Destroy (gameObject);
			Application.LoadLevel (SceneUtils.FINISH);
		}

		// Hold bomb
		if (Input.GetButtonDown (ButtonUtils.HOLD) && isColliderBomb) {
			Destroy (bombCollision);
			normalBombScript.Show ();
		}
	}

//	void OnTriggerEnter2D(Collider2D c){
//		// Destroy sub enemy
//		print(c.tag);
//		if (c.tag == ObjectUtils.BOMB) {
//			c.isTrigger = true;
//		}
//	}

	void OnCollisionEnter2D (Collision2D c) {
		if (c.gameObject.tag == ObjectUtils.BOMB) {
			bombCollision = c.gameObject;
			isColliderBomb = true;
		}
	}
}
