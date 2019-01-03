using Unity.Entities;

public class PerformanceTestSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        PerformanceTest.update();
    }
}