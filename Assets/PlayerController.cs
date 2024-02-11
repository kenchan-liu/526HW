using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入命名空间以访问场景管理功能

public class PlayerController : MonoBehaviour
{
    public float jumpForce; // 跳跃力度
    public float speed; // 控制角色移动速度
    public KeyCode jumpKey = KeyCode.Space; // 跳跃按键，默认为空格键
    private Rigidbody2D rb2d;
    private bool isGrounded; // 是否接触地面
    public float immobilizeTime; // 球体不能移动的时间
    private SpriteRenderer spriteRenderer;
    private bool isImmobilized = false;
    private Color originalColor;
    public Transform enemy;
    private bool canMoveFreely = false; // 控制自由移动的布尔变量
    public GameObject success;
    public GameObject restart;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
        originalColor = spriteRenderer.color; // 保存原始颜色
        success.SetActive(false);
        restart.SetActive(false);
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        // 自由移动时，允许上下移动
        float moveVertical = canMoveFreely ? Input.GetAxis("Vertical") : 0;
        if (canMoveFreely)
        {
            // 失去重力时的自由移动
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;
            rb2d.velocity = movement;
        }
        if (enemy != null && !isImmobilized)
        {
            if (Vector3.Distance(enemy.position, transform.position) < 1.5f)
            {   
                isImmobilized = true;
            }
        }
        if (!isImmobilized) // 如果没有被定住
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
        // 如果重启的UI显示，并且玩家按下了F键，则重新加载当前场景
        if (restart.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ReloadCurrentScene();
        }
    }

    void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        rb2d.velocity = movement;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap")) // 检测是否碰撞到trap地板
        {
            StartCoroutine(Immobilize(immobilizeTime));
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 接触地面时更新地面状态
        }
        if (other.gameObject.CompareTag("Goal")) // 检测是否碰撞到Goal
        {
            spriteRenderer.color = Color.green; // 将球体颜色改为绿色
            Time.timeScale = 0; // 静止场景
            success.SetActive(true); 
            restart.SetActive(true); 
        }

    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 离开地面时更新地面状态
        }
        if (other.gameObject.CompareTag("Goal")) // 检测是否碰撞到Goal
        {
            spriteRenderer.color = originalColor; // 将球体颜色改为原本颜色
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tool"))
        {
            StartCoroutine(TemporaryLoseGravity(3f));
        }
    }
    IEnumerator TemporaryLoseGravity(float duration)
    {
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0; // 玩家失去重力
        canMoveFreely = true; // 允许玩家自由移动
        yield return new WaitForSeconds(duration); // 等待指定时间
        rb2d.gravityScale = originalGravity; // 恢复重力
        canMoveFreely = false; // 恢复正常移动限制
    }
    IEnumerator Immobilize(float time)
    {
        isImmobilized = true; // 开始定住
        rb2d.velocity = Vector2.zero; // 立即停止所有运动
        //rb2d.isKinematic = true; // 将 Rigidbody2D 设置为 Kinematic，禁用所有物理效果
        spriteRenderer.color = Color.red; // 将球体颜色改为红色
        yield return new WaitForSeconds(time); // 等待一段时间
        //rb2d.isKinematic = false; // 将 Rigidbody2D 恢复
        isImmobilized = false; // 解除定住状态
        spriteRenderer.color = originalColor; // 恢复球体的原始颜色
    }
    void ReloadCurrentScene()
    {
        Time.timeScale = 1; // 场景运动
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // 获取当前场景的索引
        SceneManager.LoadScene(sceneIndex); // 根据索引重新加载场景
    }
}
