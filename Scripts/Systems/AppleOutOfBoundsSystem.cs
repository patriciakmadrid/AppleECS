using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class AppleOutOfBoundsSystem : SystemBase
{
    private EntityQuery m_BasketQuery;

    protected override void OnCreate()
    {
        m_BasketQuery = GetEntityQuery(typeof(BasketIndex));
    }

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var didMiss = false;

        foreach (var (transform, bottomY, apple) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<AppleBottomY>>().WithEntityAccess())
        {
            if (transform.ValueRO.Position.y < bottomY.ValueRO.Value)
            {
                ecb.DestroyEntity(apple);
                didMiss = true;
            }
        }

        if (didMiss)
        {
            var basketCount = m_BasketQuery.CalculateEntityCount();

            foreach (var (index, basket) in SystemAPI.Query<RefRO<BasketIndex>>().WithEntityAccess())
            {
                if (index.ValueRO.Value == basketCount - 1)
                {
                    ecb.DestroyEntity(basket);
                }
            }

            // destroy all apples
            foreach (var (_, apple) in SystemAPI.Query<RefRO<AppleTag>>().WithEntityAccess())
            {
                ecb.DestroyEntity(apple);
            }

            // if there are no baskets left, change scene to MainMenu
            if (basketCount - 1 == 0)
            {
                // changed from "changing scene to MainMenu" to "reloading scene"
                Debug.Log("No baskets left, reloading scene");
                // changed from 0 --> Easy
                SceneManager.LoadScene("Easy");
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
