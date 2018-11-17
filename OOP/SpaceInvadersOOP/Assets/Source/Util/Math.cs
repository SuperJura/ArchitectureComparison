using UnityEngine;

public static class Math
{
	public static float getPercentBetween(float point, float min, float max)
	{
		return (point - min) / (max - min);
	}

    public static float map(float fromSource, float toSource, float fromTarget, float toTarget, float point)
    {
        return getPercentBetween(point, fromSource, toSource) * (toTarget - fromTarget) + fromTarget;
    }


    public static bool closeEnough(float first, float second, float threshold = 0.05f)
    {
        return Mathf.Abs(first - second) <= threshold;
    }
}