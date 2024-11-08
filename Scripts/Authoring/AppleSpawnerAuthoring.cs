using Unity.Entities;
using UnityEngine;

public struct AppleSpawner : IComponentData
{
    public Entity Prefab;
    public float Interval;
}

[DisallowMultipleComponent]
public class AppleSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private float appleSpawnInterval = 1f;

    private class AppleSpawnerAuthoringBaker : Baker<AppleSpawnerAuthoring>
    {
        public override void Bake(AppleSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new AppleSpawner
            {
                Prefab = GetEntity(authoring.applePrefab, TransformUsageFlags.Dynamic),
                Interval = authoring.appleSpawnInterval
            });
            AddComponent(entity, new Timer { Value = 2f });
        }
    }
}

