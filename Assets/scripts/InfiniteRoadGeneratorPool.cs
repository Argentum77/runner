using UnityEngine;
using System.Collections.Generic;

public class DebugRoadGenerator : MonoBehaviour
{
    public GameObject roadPrefab;
    public float segmentLength = 10f;
    public int segmentsAhead = 10;
    public int maxSegments = 30;
    public bool showDebugInfo = true;
    
    private Vector3 nextSpawnPos = Vector3.zero;
    private Transform player;
    private float lastSpawnZ = 0;
    private List<GameObject> activeSegments = new List<GameObject>();
    private int totalSpawned = 0;
    private int totalDeleted = 0;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (player == null)
        {
            Debug.LogError("Player not found! Add tag 'Player' to your player object.");
            return;
        }
        
        // Создаем начальную дорогу
        for (int i = 0; i < segmentsAhead; i++)
        {
            SpawnNewSegment();
        }
        lastSpawnZ = nextSpawnPos.z;
    }
    
    void Update()
    {
        if (player == null) return;
        
        // Если игрок приближается к концу дороги
        if (player.position.z + segmentLength * 5 > lastSpawnZ)
        {
            SpawnNewSegment();
            lastSpawnZ = nextSpawnPos.z;
        }
        
        // Удаляем старые сегменты
        RemoveOldSegments();
        
        // Отображаем информацию
        if (showDebugInfo)
        {
            Debug.Log($"Сегментов: {activeSegments.Count} | Создано: {totalSpawned} | Удалено: {totalDeleted}");
        }
    }
    
    void SpawnNewSegment()
    {
        GameObject newSegment = Instantiate(roadPrefab, nextSpawnPos, Quaternion.identity);
        activeSegments.Add(newSegment);
        nextSpawnPos.z += segmentLength;
        totalSpawned++;
        
        // Если сегментов слишком много - удаляем самый старый
        if (activeSegments.Count > maxSegments)
        {
            RemoveOldestSegment();
        }
    }
    
    void RemoveOldSegments()
    {
        float deleteDistance = segmentLength * 2;
        
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            GameObject segment = activeSegments[i];
            if (segment == null)
            {
                activeSegments.RemoveAt(i);
                continue;
            }
            
            float segmentZ = segment.transform.position.z;
            
            if (player.position.z - segmentZ > deleteDistance)
            {
                Destroy(segment);
                activeSegments.RemoveAt(i);
                totalDeleted++;
            }
        }
    }
    
    void RemoveOldestSegment()
    {
        if (activeSegments.Count == 0) return;
        
        GameObject oldest = activeSegments[0];
        activeSegments.RemoveAt(0);
        Destroy(oldest);
        totalDeleted++;
    }
    
    void OnDrawGizmos()
    {
        if (!showDebugInfo) return;
        
        // Рисуем позиции сегментов
        Gizmos.color = Color.green;
        foreach (GameObject segment in activeSegments)
        {
            if (segment != null)
            {
                Gizmos.DrawWireCube(segment.transform.position, new Vector3(10, 0.5f, 10));
            }
        }
        
        // Рисуем следующую позицию спавна
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(nextSpawnPos, 1f);
    }
}