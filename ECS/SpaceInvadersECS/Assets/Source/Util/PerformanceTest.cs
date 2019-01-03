using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class PerformanceTest
{
	static float fpsDelta = 0.0f;
    static PerformanceCounter cpuCounter;

    static float minFPS;
    static float maxFPS;
    static float averageFPSInFirst5Frames;
    static float averageFPS;

    static float maxCPUUsage;
    static float averageCPUUsage;
    
    static bool finished;

    public static void init()
    {
        minFPS = float.MaxValue;
        maxFPS = float.MinValue;
        averageFPSInFirst5Frames = 0;
        maxCPUUsage = float.MinValue;

        cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        finished = false;
    }

    public static void update()
    {
        if(finished) return;
		fpsDelta += (Time.unscaledDeltaTime - fpsDelta) * 0.1f;
        float fps = 1.0f / fpsDelta;

        if(Time.frameCount > 10)
        {
            if(fps < minFPS)
            {
                minFPS = fps;
            }
            if(fps > maxFPS)
            {
                maxFPS = fps;
            }
        }
        else
        {
            averageFPSInFirst5Frames += fps;
            if(Time.frameCount == 10)
            {
                averageFPSInFirst5Frames /= 10;
            }
        }
        float cpuUsage = cpuCounter.NextValue();
        if(cpuUsage > maxCPUUsage)
        {
            maxCPUUsage = cpuUsage;
        }
        averageCPUUsage += cpuUsage;
        averageFPS += fps;
    }

    public static void finish()
    {
        if(finished) return;
        finished = true;

        averageCPUUsage /= Time.frameCount;
        averageFPS /= Time.frameCount;

        Debug.Log("Min FPS: " + minFPS);
        Debug.Log("Max FPS: " + maxFPS);
        Debug.Log("Average FPS in 5 first frames: " + averageFPSInFirst5Frames);
        Debug.Log("Average FPS: " + averageFPS);
        Debug.Log("Max CPU usage: " + maxCPUUsage);
        Debug.Log("Average CPU usage: " + averageCPUUsage);
        
    }
}