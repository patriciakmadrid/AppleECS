using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(TimerSystem))]
public partial struct BadAppleSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        new SpawnBadAppleJob { ECB = ecb }.Schedule();
    }

    [BurstCompile]
    private partial struct SpawnBadAppleJob : IJobEntity
    {
        public EntityCommandBuffer ECB;

        private void Execute(in LocalTransform transform, in AppleSpawner spawner, ref Timer timer)
        {
            if (timer.Value > 0)
                return;

            timer.Value = spawner.Interval * 2; // Make bad apples spawn less frequently
            var badAppleEntity = ECB.Instantiate(spawner.Prefab);
            ECB.SetComponent(badAppleEntity, LocalTransform.FromPosition(transform.Position));
        }
    }
}
