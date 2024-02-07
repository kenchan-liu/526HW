using UnityEngine;

public class PlayerJump2D : MonoBehaviour
{
    public float jumpForce = 3.0f; // 跳跃力量
    public KeyCode jumpKey = KeyCode.Space; // 跳跃键

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jump!");
        }
    }

}