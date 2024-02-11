using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // ���ڷ��ʳ�������
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce; // ��Ծ����
    public float speed; // ���ƽ�ɫ�ƶ��ٶ�
    public KeyCode jumpKey = KeyCode.Space; // ��Ծ������Ĭ��Ϊ�ո��
    private Rigidbody2D rb2d;
    private bool isGrounded; // �Ƿ�Ӵ�����
    public float immobilizeTime; // ���岻���ƶ���ʱ��
    private SpriteRenderer spriteRenderer;
    private bool isImmobilized = false;
    private Color originalColor;
    private bool canMoveFreely = false; // ���������ƶ��Ĳ�������

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��ȡSpriteRenderer���
        originalColor = spriteRenderer.color; // ����ԭʼ��ɫ
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        // �����ƶ�ʱ�����������ƶ�
        float moveVertical = canMoveFreely ? Input.GetAxis("Vertical") : 0;
        if (canMoveFreely)
        {
            // ʧȥ����ʱ�������ƶ�
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;
            rb2d.velocity = movement;
        }
        if (!isImmobilized) // ���û�б���ס
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
        Debug.Log(isGrounded);
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
        if (other.gameObject.CompareTag("Trap")) // ����Ƿ���ײ��trap�ذ�
        {
            StartCoroutine(Immobilize(immobilizeTime));
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // �Ӵ�����ʱ���µ���״̬
        }
        if (other.gameObject.CompareTag("Goal")) // ����Ƿ���ײ��Goal
        {
            spriteRenderer.color = Color.green; // ��������ɫ��Ϊ��ɫ
        }
        if (other.gameObject.CompareTag("Chaser")) // ȷ����� GameObject �� "Player" ��ǩ
        {
            ReloadScene();
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // �뿪����ʱ���µ���״̬
        }
        if (other.gameObject.CompareTag("Goal")) // ����Ƿ���ײ��Goal
        {
            spriteRenderer.color = originalColor; // ��������ɫ��Ϊԭ����ɫ
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
        rb2d.gravityScale = 0; // ���ʧȥ����
        canMoveFreely = true; // ������������ƶ�
        yield return new WaitForSeconds(duration); // �ȴ�ָ��ʱ��
        rb2d.gravityScale = originalGravity; // �ָ�����
        canMoveFreely = false; // �ָ������ƶ�����
    }
    IEnumerator Immobilize(float time)
    {
        isImmobilized = true; // ��ʼ��ס
        rb2d.velocity = Vector2.zero; // ����ֹͣ�����˶�
        //rb2d.isKinematic = true; // �� Rigidbody2D ����Ϊ Kinematic��������������Ч��
        spriteRenderer.color = Color.red; // ��������ɫ��Ϊ��ɫ
        yield return new WaitForSeconds(time); // �ȴ�һ��ʱ��
        //rb2d.isKinematic = false; // �� Rigidbody2D �ָ�
        isImmobilized = false; // �����ס״̬
        spriteRenderer.color = originalColor; // �ָ������ԭʼ��ɫ
    }
    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ��ȡ��ǰ����������
        SceneManager.LoadScene(currentSceneIndex); // ���¼��ص�ǰ����
    }
}
