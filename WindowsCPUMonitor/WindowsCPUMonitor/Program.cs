using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsCPUMonitor
{
	class Program
	{
		static float max = float.MinValue;
		static float sum = 0;
		static int iterations = 0;
		static void Main(string[] args)
		{
			var cpuCounter = new PerformanceCounter();
			cpuCounter.CategoryName = "Process";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "SpaceInvadersECS";	//Promjeniti naziv procesa koji se prati
			cpuCounter.NextValue();
			Thread.Sleep(2000);
			Console.WriteLine("Ready");
			try
			{
				while (!Console.KeyAvailable)
				{
					Thread.Sleep(1000);
					float nextValue = cpuCounter.NextValue();
					if (nextValue > max)
					{
						max = nextValue;
					}
					sum += nextValue;
					iterations++;
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Ending monitoring");
			}
			Console.WriteLine("Average: " + sum / iterations);
			Console.WriteLine("Max: " + max);
		}
	}
}
