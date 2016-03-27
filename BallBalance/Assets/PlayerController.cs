using UnityEngine;
using System.Collections;
using Multinet.Net;
using Multinet.Utils;
using Multinet.Genetic;
using Multinet;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerController : MonoBehaviour {

	private int score;
	private UnityEngine.UI.Text scoreDisplay;
	public int lifes=3;
	public bool useBot;
	private static Vector2 action = new Vector2(0,0);
	private NeuralNet net = null;
	public string botPath = "";
	private GameObject ball;
	private Rigidbody2D myRigidybody2d;

	// Use this for initialization
	void Start () {
		ball = GameObject.Find ("Ball");
		scoreDisplay = GameObject.Find ("ScoreDisplay").GetComponent<UnityEngine.UI.Text>();
		myRigidybody2d = gameObject.GetComponent<Rigidbody2D> ();
		score = 0;
		if (useBot && System.IO.File.Exists(botPath)) {
			FileStream stream = new FileStream (botPath, FileMode.Open);
			BinaryFormatter form = new BinaryFormatter ();
			Genome gen = (Genome) form.Deserialize (stream);
			stream.Close();

			net = (NeuralNet) BallKeeperImpl.Implementations.translateGenome (gen);
		}
	}


	public void OnRespawn() {
		scoreDisplay.text = string.Format ("Score: {0}\tLifes: {1}", score, lifes);
	}


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.name=="Ball") {
			score++;
			scoreDisplay.text = string.Format ("Score: {0}\tLifes: {1}", score, lifes);
		}
	}

	public void DecrementLifes(){
		lifes--;
	}

	void FixedUpdate () {
		if (net != null) {
			float bx = ball.transform.position.x;
			float by = ball.transform.position.y;
			float x = gameObject.transform.position.x;
			float y = gameObject.transform.position.y;


			net [0].ProccessInput (bx);
			net [1].ProccessInput (by);
			net [2].ProccessInput (x);
			net.Proccess ();

			float d = (float)net [4].GetOutput ();


			d = (2.0f * d - 1.0f);

			action.Set (x + d, y);

			myRigidybody2d.MovePosition (action);
		}
	}
}
