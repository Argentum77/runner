using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject pref;
    public Transform spawnpoint;
    int mv = 0;
    public int maxObjectsCount = 50;
    public string objectTag = "Ground";
    
    // Список для хранения всех созданных объектов
    private List<GameObject> spawnedObjects = new List<GameObject>();
    
    // Расстояние, на котором объекты будут удаляться (позади spawnpoint)
    public float destroyDistance = 50f;

    void Start()
    {
        if (spawnpoint == null)
        {
            Debug.LogError("Spawnpoint не назначен! Пожалуйста, назначьте его в инспекторе.");
            return;
        }

        // Создаем начальные объекты
        for (int a = 0; a < maxObjectsCount; a++)
        {
            Vector3 newPosition = spawnpoint.position + new Vector3(0, 0, mv);
            GameObject obj = Instantiate(pref, newPosition, spawnpoint.rotation);
            spawnedObjects.Add(obj); // Добавляем в список
            mv += 10;
        }
    }

    void Update()
    {
        if (spawnpoint == null) return;

        // Удаляем объекты, которые остались позади
        RemoveObjectsBehind();

        int currentCount = GameObject.FindGameObjectsWithTag(objectTag).Length;
        
        // Создаем новые объекты, если нужно
        if (currentCount < maxObjectsCount)
        {
            Vector3 newPosition = spawnpoint.position + new Vector3(0, 0, mv);
            GameObject obj = Instantiate(pref, newPosition, spawnpoint.rotation);
            spawnedObjects.Add(obj);
            mv += 10;
        }
        
        Debug.Log("Объектов на сцене: " + currentCount);
    }

    // Метод для удаления объектов сзади
    void RemoveObjectsBehind()
    {
        // Проходим по списку в обратном порядке (для безопасного удаления)
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            // Проверяем, находится ли объект позади spawnpoint
            float distance = spawnedObjects[i].transform.position.z - spawnpoint.position.z;
            
            // Если объект слишком далеко позади - удаляем
            if (distance < -destroyDistance)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
