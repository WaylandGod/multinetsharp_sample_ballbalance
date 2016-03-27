using System;

using Multinet;
using Multinet.Genetic;
using Multinet.Math;
using Multinet.Net;
using Multinet.Utils;
using Multinet.Net.Impl;

namespace BallKeeperImpl
{
	public sealed class Implementations
	{
		private Implementations ()
		{
		}

		public static Genome buildGenome() {

			Random rnd = new Random ();
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
		}
			
		public static Evaluable translateGenome(Genome gen)
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
		}
	}
}

