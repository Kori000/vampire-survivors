using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统通用集合命名空间
using UnityEngine; // 引入 Unity 引擎命名空间

public class MapController : MonoBehaviour // 定义一个名为 MapController 的类，继承自 MonoBehaviour
{
    public List<GameObject> terrainChunks; // 表示地形块的列表 - Prefabs/Chunks 中预定义好的地图块, 其中有 八个方向的 GameObject 和 一堆 Props (石头之类的)
    public GameObject player; // 定义一个公有变量 player，表示玩家对象
    public float checkerRadius; // 定义一个公有变量 checkerRadius，表示检测半径
    Vector2 noTerrainPosition; // 定义一个私有变量 noTerrainPosition，表示没有地形的位置
    public LayerMask terrainMask; // 定义一个公有变量 terrainMask，表示地形层的遮罩

    PlayerMovement pm; // 定义一个私有变量 pm，表示玩家移动的脚本

    public GameObject currentChunk;
    [Header("Optimization")]
    public List<GameObject> spawnedChunks; // 定义一个公有变量 spawnedChunks，表示已生成的地形块
    GameObject latestChunk; // 定义一个私有变量 latestChunk，表示最近生成的地形块
    public float maxOpDist; // 定义一个公有变量 maxOpDist，表示最大优化距离, 超过这个距离就把其他的地形块移除, 必须大于地图块的半径
    float opDist; // 定义一个私有变量 opDist，表示优化距离
    float optimizerCooldown;
    public float optimizerCooldownDur; // 定义一个公有变量 optimizerCooldownDur，表示优化器冷却时间

    void Start() // Unity 的生命周期方法之一，在游戏对象创建时调用
    {
        pm = FindObjectOfType<PlayerMovement>(); // 找到场景中的 PlayerMovement 脚本并赋值给 pm
    }

    // Update 是 Unity 的生命周期方法之一，每帧调用一次
    void Update()
    {
        ChunkChecker(); // 调用 ChunkChecker 方法
        ChunkOptimizer(); // 调用 ChunkOptimizer 方法
    }

    void ChunkChecker() // 检查周围是否需要生成新的地形块
    {

        if (!currentChunk)
        {
            return;
        }


        // 检查玩家移动的方向并在相应的位置检测是否有地形块
        if (pm.moveDir.x > 0 && pm.moveDir.y == 0) // 向右
        {

            // 如果在当前地形块的右侧位置的指定半径范围内没有检测到地形块
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {

                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk(); // 生成新的地形块
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0) // 向左
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0) // 向上
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0) // 向下
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0) // 向右上
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0) // 向左上
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0) // 向右下
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y < 0) // 向左下
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk() // 生成新的地形块
    {
        int rand = Random.Range(0, terrainChunks.Count); // 从地形块列表中随机选择一个地形块
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity); // 在没有地形的位置生成新的地形块
        spawnedChunks.Add(latestChunk); // 记录已生成的地形块
    }

    void ChunkOptimizer() // 优化地形块，移除远离玩家的地形块
    {
        // optimizerCooldown -= Time.deltaTime; // 减少优化器的冷却时间

        // if (optimizerCooldown <= 0f) // 如果冷却时间小于等于 0
        // {
        //     optimizerCooldown = optimizerCooldownDur; // 重置冷却时间
        // }
        // else
        // {
        //     return; // 否则返回
        // }

        foreach (GameObject chunk in spawnedChunks) // 遍历所有已生成的地形块
        {

            opDist = Vector3.Distance(player.transform.position, chunk.transform.position); // 计算玩家与地形块之间的距离
            if (opDist > maxOpDist) // 如果距离大于最大优化距离
            {
                chunk.SetActive(false); // 禁用地形块
            }
            else
            {
                chunk.SetActive(true); // 启用地形块
            }
        }
    }
}
