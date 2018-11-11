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
}