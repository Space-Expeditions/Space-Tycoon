using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A / D
        float moveY = Input.GetAxisRaw("Vertical");   // W / S

        Vector3 move = new Vector3(moveX, moveY, 0f).normalized;

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
