using UnityEngine;
using System.Collections;
using Multinet;
using Multinet.Net;
using Multinet.Genetic;
using Multinet.Math;
using Multinet.Utils;
using Multinet.Net.Impl;
using System.Collections.Generic;


public class PlayerTrainingController : MonoBehaviour {

	public long timeOfLive = 30000;
	public string saveGenomeToPath = "/";
	public int lifes = 1;

	private static System.Random rnd = new System.Random();
	private static GeneticA genetic;
	private static int currentGenomeIdx=-1;
	private static int currentGeneration=-1;
	private static NeuralNet currentNet;
	private static double[] statistic = new double[]{double.MaxValue, double.MinValue, 0.0};
	private static List<Genome> population;
	private static Genome currentGenome;
	private static double fitness;
	private static Vector2 action = new Vector2(0,0);
	private static System.DateTime startTime;

	private float score;
	private UnityEngine.UI.Text scoreDisplay;
	private GameObject ball;
	private Rigidbody2D myRigidybody2d;


	private void UpdateHUD() {

		long time = (long)((System.DateTime.Now - startTime).TotalMilliseconds/1000.0f + 0.5);
		if (scoreDisplay != null) {
			scoreDisplay.text = string.Format ("Score: {0}\t\tLifes: {1}\t\tLiveTime: {2}", score, lifes, time);
		}
	}

	public int GetLifes() {
		return lifes;
	}

	void Start () {
		ball = GameObject.Find ("Ball");
		scoreDisplay = GameObject.Find ("ScoreDisplay").GetComponent<UnityEngine.UI.Text>();
		myRigidybody2d = gameObject.GetComponent<Rigidbody2D> ();
		//rnd = new System.Random ();
		if (genetic == null) {
			
			genetic = new GeneticA(50);
			genetic.Elitism = 1;
			genetic.SurvivalRate = 0.30;
			genetic.MutationRate = 0.0005;
			genetic.MinPopulationSize = 50;

			genetic.GenomeBuilder = () =>
			{
				Genome gen = new Genome();
				Chromossome cr = new Chromossome();
				cr.AddGene(0, rnd.NextDouble());
				cr.AddGene(1, rnd.NextDouble());
				cr.AddGene(2, rnd.NextDouble());
				cr.AddGene(3, rnd.NextDouble());
				cr.AddGene(4, rnd.NextDouble());
				cr.AddGene(5, rnd.NextDouble());
				cr.AddGene(6, rnd.NextDouble());
				cr.AddGene(7, rnd.NextDouble());
				cr.AddGene(8, rnd.NextDouble());
				cr.AddGene(9, rnd.NextDouble());
				cr.AddGene(10, rnd.NextDouble());
				cr.AddGene(11, rnd.NextDouble());
				cr.AddGene(12, rnd.NextDouble());
				cr.AddGene(13, rnd.NextDouble());
				cr.AddGene(14, rnd.NextDouble());
				cr.AddGene(15, rnd.NextDouble());
				cr.AddGene(16, rnd.NextDouble());
				cr.AddGene(17, rnd.NextDouble());
				cr.AddGene(18, rnd.NextDouble());
				gen.AddChromossome(0, cr);
				return gen;
			};

			genetic.Translator = (Genome gen) =>
			{
				NeuralNet net = new NeuralNet();

				net.NumericalMethod = new Multinet.Math.Impl.RungeKuttaMethod();
				int n1 = net.CreateNeuron();
				int n2 = net.CreateNeuron();
				int n3 = net.CreateNeuron();
				int n4 = net.CreateNeuron();
				int n5 = net.CreateNeuron();

				Neuron ne1 = net[n1];
				Neuron ne2 = net[n2];
				Neuron ne3 = net[n3];
				Neuron ne4 = net[n4];
				Neuron ne5 = net[n5];

				ne1.Implementation = new Beer1995Neuron();
				ne2.Implementation = new Beer1995Neuron();
				ne3.Implementation = new Beer1995Neuron();
				ne4.Implementation = new Beer1995Neuron();
				ne5.Implementation = new Beer1995Neuron();

				//ne1.Implementation = new HNeuron();
				//ne2.Implementation = new HNeuron();
				//ne3.Implementation = new HNeuron();

				Chromossome cr = gen.GetChromossome(0);

				double wamp =60.0;
				double wshift = 30.0;

				net.CreateSynapse(n1, n4, wamp * BitArrayUtils.ToNDouble(cr.GetGene(0))-wshift);
				net.CreateSynapse(n2, n4, wamp * BitArrayUtils.ToNDouble(cr.GetGene(1))-wshift);
				net.CreateSynapse(n3, n4, wamp * BitArrayUtils.ToNDouble(cr.GetGene(2))-wshift); 
				net.CreateSynapse(n1, n5, wamp * BitArrayUtils.ToNDouble(cr.GetGene(3))-wshift);
				net.CreateSynapse(n2, n5, wamp * BitArrayUtils.ToNDouble(cr.GetGene(4))-wshift);
				net.CreateSynapse(n3, n5, wamp * BitArrayUtils.ToNDouble(cr.GetGene(5))-wshift);
				net.CreateSynapse(n4, n5, wamp * BitArrayUtils.ToNDouble(cr.GetGene(6))-wshift);
				net.CreateSynapse(n5, n4, wamp * BitArrayUtils.ToNDouble(cr.GetGene(7))-wshift);

				ne1.Implementation.UseNumericalMethod = false;
				ne1.Implementation["inputgain"] = 10 * BitArrayUtils.ToNDouble(cr.GetGene(8));
				ne1.Implementation["outputgain"] = 1.0;
				ne1.Implementation["inputweight"] = 0.0;
				ne1.Implementation["sensorweight"] = 1.0;
				ne1.TimeConst = 100 * BitArrayUtils.ToNDouble(cr.GetGene(9)) + 0.001;
				ne1.Implementation["bias"] = 0.0;

				ne2.Implementation.UseNumericalMethod = false;
				ne2.Implementation["inputgain"] = BitArrayUtils.ToNDouble(cr.GetGene(10))*10;
				ne2.Implementation["outputgain"] = 1.0;
				ne2.Implementation["inputweight"] = 0.0;
				ne2.Implementation["sensorweight"] = 1.0;
				ne2.TimeConst = 100 * BitArrayUtils.ToNDouble(cr.GetGene(11)) + 0.001;
				ne2.Implementation["bias"] = 0.0;

				ne3.Implementation.UseNumericalMethod = false;
				ne3.Implementation["inputgain"] = BitArrayUtils.ToNDouble(cr.GetGene(12))*10;
				ne3.Implementation["outputgain"] = 1.0;
				ne3.Implementation["inputweight"] = 0.0;
				ne3.Implementation["sensorweight"] = 1.0;
				ne3.TimeConst = 100 * BitArrayUtils.ToNDouble(cr.GetGene(13)) + 0.001;
				ne3.Implementation["bias"] = 0.0f;

				ne4.Implementation["inputgain"] = 1.0;
				ne4.Implementation["outputgain"] = 1.0;
				ne4.Implementation["inputweight"] = 1.0;
				ne4.Implementation["sensorweight"] = 0.0;
				ne4.TimeConst = 100 * BitArrayUtils.ToNDouble(cr.GetGene(14)) + 0.001;
				ne4.Implementation["bias"] = BitArrayUtils.ToNDouble(cr.GetGene(15))*10 - 5.0;


				ne5.Implementation["inputgain"] = 1.0;
				ne5.Implementation["outputgain"] = BitArrayUtils.ToNDouble(cr.GetGene(16))*10;
				ne5.Implementation["inputweight"] = 1.0;
				ne5.Implementation["sensorweight"] = 0.0;
				ne5.TimeConst = 100 * BitArrayUtils.ToNDouble(cr.GetGene(17)) + 0.001;
				ne5.Implementation["bias"] = 0.0f;

				net.NumericalMethod["step"] = 4.0 * BitArrayUtils.ToNDouble(cr.GetGene(18)) + 0.05;
				return net;
			};
			Time.timeScale = 0; //PAUSE
			genetic.init ();
			NextGenome ();
			Time.timeScale = 1; //RESUME
		}

		UpdateHUD ();
	}

	private void NextGenome() {
		rnd = new System.Random ();
		startTime = System.DateTime.Now;
		if (currentGenome != null) {
			currentGenome.Value = fitness + 10 * (lifes+1);
			genetic.UpdateStatistic (fitness, statistic);
			Debug.Log (string.Format ("Gen({0}) => Current Fitness({1}): {2}", currentGeneration, currentGenomeIdx, fitness));

			fitness = 0;
			score = 0;
		}

		if (currentGeneration == -1) {
			population = genetic.Population;
			genetic.InitEvaluation (statistic);
			DecrementLifes ();
			currentGeneration++;
			UpdateHUD ();
		}

		currentGenomeIdx++;
		if (currentGenomeIdx >= population.Count) {
			Time.timeScale = 0.0f; //PAUSE


			genetic.EndEvaluation (statistic);

			population.Sort ();

			int count = population.Count;

			if (count > 0) {
				Genome gen = population [count - 1];
				gen.Serialize (string.Format("{0}/Genome{1}_{2}.bin", saveGenomeToPath, currentGeneration, (currentGenomeIdx-1)));
			}

			genetic.NextGeneration (statistic);
			Debug.Log (string.Format ("Generation {0} statistic", currentGeneration)); 
			Debug.Log (string.Format ("MIN, MAX, AVG = {0};  {1};  {2}", statistic [0], statistic [1], statistic [2] / genetic.PopulationSize));
			population = genetic.Population;
			genetic.InitEvaluation (statistic);	
			currentGeneration++;
			currentGenomeIdx = 0;
			Time.timeScale = 1.0f;//RESUME
		} 


		currentGenome = population [currentGenomeIdx];
		currentNet = (NeuralNet)genetic.Translator (currentGenome);

		startTime = System.DateTime.Now;
	}


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.name=="Ball") {
			score += 1.0f;
			fitness += 10;
			UpdateHUD ();
		}
	}

	public void DecrementLifes(){
		lifes--;
		if (lifes < 0)
			lifes = 0;
		UpdateHUD ();
	}

	public void RespawnPositionOnly() {
		gameObject.transform.Translate (gameObject.transform.position * -1);
		gameObject.transform.Translate (0.0001f, -3.34f, 0.0f);
	}

	public void OnRespawn() {
		score = 0;
		RespawnPositionOnly ();
		NextGenome ();
	}

	void FixedUpdate () {

		if (currentNet != null) {
			
			float bx = ball.transform.position.x;
			float by = ball.transform.position.y;
			float x = gameObject.transform.position.x;
			float y = gameObject.transform.position.y;


			currentNet [0].ProccessInput (bx);
			currentNet [1].ProccessInput (by);
			currentNet [2].ProccessInput (x);
			currentNet.Proccess ();

			float d = (float)currentNet [4].GetOutput ();


			d = (2.0f * d - 1.0f);

			action.Set (x+d, y);

			myRigidybody2d.MovePosition (action);

			double deltatime = (System.DateTime.Now - startTime).TotalMilliseconds;

			if (deltatime >= timeOfLive) {
				startTime = System.DateTime.Now;
				RespawnPositionOnly ();
				NextGenome ();
			}


			if (deltatime > 1000) {
				fitness += 0.1f/System.Math.Abs(bx-x) * Time.fixedDeltaTime;
			}

			if (deltatime > 1000) {
				//Debug.Log (string.Format("INPUT: {0}, {1}, {2};  OUT: {3}", bx, by, x, d));
				UpdateHUD ();
			}
		}
	}
}
