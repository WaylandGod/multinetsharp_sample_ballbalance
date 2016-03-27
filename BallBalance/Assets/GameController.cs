using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	private GameObject ball;
	private GameObject keeper;

	// Use this for initialization
	void Start () {
		ball = GameObject.Find ("Ball");
		keeper = GameObject.Find ("PPlayer");
	}

	void Awake() {
		//Time.captureFramerate = 1000;
		//Time.captureFramerate = 1;
	}

	void FixedUpdate() {
		float y = ball.transform.position.y;
		float x = ball.transform.position.x;


		if (y <= -3.5f || y >= 5.8f || x <= -4.4f || x >= 4.2) {
			PlayerTrainingController training = keeper.GetComponent<PlayerTrainingController> ();
			BallController ballctl = ball.GetComponent<BallController> ();
			if (training != null && training.isActiveAndEnabled ) {
				training.DecrementLifes ();
				if (training.GetLifes() > 0) {
					ballctl.OnRespawn();
					training.RespawnPositionOnly ();
				} else {
					ballctl.OnRespawn ();
					training.OnRespawn();
					SceneManager.LoadScene ("firstmain");
				}
			} else {
				PlayerController controller = keeper.GetComponent<PlayerController> ();
				if (controller != null && controller.isActiveAndEnabled ) {
					if (controller.lifes > 0) {
						controller.DecrementLifes ();
						ballctl.OnRespawn ();
						controller.OnRespawn();
					} else {
						controller.OnRespawn();
						ballctl.OnRespawn ();
						SceneManager.LoadScene ("firstmain");
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
