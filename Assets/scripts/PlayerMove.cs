using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Скорость движения игрока (можно менять в Инспекторе)
    public float speed = 5f; 

    void Update()
    {
        // Получаем ввод с клавиатуры (W/S или стрелки вверх/вниз)
        float moveX = Input.GetAxis("Horizontal"); 
        // Получаем ввод с клавиатуры (A/D или стрелки влево/вправо)
        //float moveZ = Input.GetAxis("Vertical");
        float moveZ = 2f;   

        // Создаем вектор направления движения по осям X и Z
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);

        // Двигаем игрока плавно, независимо от кадров (Time.deltaTime)
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
