using Unity.Entities;
using UnityEngine;

public struct BadAppleTag : IComponentData
{
}

[DisallowMultipleComponent]
public class BadAppleAuthoring : MonoBehaviour
{
    private class BadAppleAuthoringBaker : Baker<BadAppleAuthoring>
    {
        public override void Bake(BadAppleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<BadAppleTag>(entity);
        }
    }
