using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	private Rigidbody2D ballRB;
	private Vector2 kick = new Vector2(0, 0);
	private System.Random rnd = new System.Random();
	private bool firstTime = true;



	// Use this for initialization
	void Start () {
	}

	void Awake(){
		ballRB = gameObject.GetComponent<Rigidbody2D> ();
		kick.Set ((float)rnd.NextDouble()*200.0f-100, 0.0f);
	}

	public void OnRespawn() {
		gameObject.transform.Translate (gameObject.transform.position * -1);
		gameObject.transform.Translate (0.0f, 5.36f, 0.0f);
		ballRB.velocity = Vector3.zero;
		ballRB.angularVelocity = 0.0f;
		kick.Set ((float)rnd.NextDouble()*200.0f-100, 0.0f);
		firstTime = true;
	}

	void FixedUpdate() {
		if (firstTime) {
			ballRB.AddForce (kick);
			firstTime = false;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
