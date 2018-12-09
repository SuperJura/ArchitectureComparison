using Unity.Mathematics;
using static Unity.Mathematics.math;

public static class Math
{
    public static float3 right(quaternion rotation)
    {
        return mul(rotation, float3(1, 0, 0));
    }
    
	public static float getPercentBetween(float point, float min, float max)
	{
		return (point - min) / (max - min);
	}
}