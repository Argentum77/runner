using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpForce = 5f; // Сила прыжка
    private Rigidbody rb;
    private bool isGrounded;    // Проверка: на земле ли игрок?

    void Start()
    {
        // Автоматически получаем компонент Rigidbody с этого же объекта
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Если нажали Пробел И игрок находится на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Применяем физическую силу строго вверх
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Сразу ставим false, так как игрок взлетел
        }
    }

    // Проверка столкновения с землей
    private void OnCollisionEnter(Collision collision)
    {
        // Если объект, с которым столкнулись, имеет тег "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Игрок снова на земле, прыжок разрешен
        }
    }
}
