using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ���������ռ��Է��ʳ���������

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

    public Transform enemy;

    private bool canMoveFreely = false; // ���������ƶ��Ĳ�������
    public float FreeFlytime;
    public GameObject success;
    public GameObject restart;

    // Cannon launch direction indicator
    public LineRenderer directionIndicator;
    public float launchSpeed;  // ���Ʒ����ٶȵĲ���

    public Transform CannonPlace;
    
    public Vector2 launchDirection = Vector2.right;
    public float forceMagnitude = 1000f;
    public bool launch = false;

    // gravity tool
    public Transform gravityTool;
    public int possession = 0;
    public GameObject gtool;

    // nisemono navmesh
    public Navigation navi;
    public Transform fbuild;
    public GameObject fgoal;
    public bool haveftool = false;

    public bool isCooldown = false; // �����Ƿ�����ȴ״̬
    public float cooldownTime = 0.6f; // �������ȴʱ��

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��ȡSpriteRenderer���
        originalColor = spriteRenderer.color; // ����ԭʼ��ɫ
        success.SetActive(false);
        restart.SetActive(false);
    }

    void UpdateDirectionIndicator()
    {
        if (directionIndicator != null)
        {
            directionIndicator.SetPosition(0, transform.position);
            directionIndicator.SetPosition(1, transform.position + new Vector3(launchDirection.x, launchDirection.y, 0) * 6);
        }
    }
    void FixedUpdate()
    {
        if (launch)
        {
            LaunchPlayer();
            launch = false;
        }
    }

    void LaunchPlayer()
    {
        rb2d.AddForce(launchDirection * forceMagnitude,ForceMode2D.Impulse); 
        GetComponent<Collider2D>().sharedMaterial.bounciness = 0.2f; //
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
        if (enemy != null && !isImmobilized)
        {
            if (Vector3.Distance(enemy.position, transform.position) < 0.8f)
            {   
                isImmobilized = true;
            }
        }
        if (!isImmobilized) // ���û�б���ס
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            if (Vector3.Distance(CannonPlace.position, transform.position) < 0.5f)
            {
                UpdateDirectionIndicator();
                directionIndicator.enabled = true;

                // �������䷽��
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    launchDirection = RotateVector2(launchDirection, 5); // ��ʱ����ת
                    UpdateDirectionIndicator(); // ���·���ָʾ��
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    launchDirection = RotateVector2(launchDirection, -5); // ˳ʱ����ת
                    UpdateDirectionIndicator(); // ���·���ָʾ��
                }
                // ����
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    // ʹ��Rigidbody2D������һ�����ٶ�������
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = new Vector2(launchDirection.x, launchDirection.y) * launchSpeed;
                    }
                    launch = true;
                }
            }
            else
            {
                directionIndicator.enabled = false;
            }
            if (Vector2.Distance(fbuild.position, transform.position) < 0.5f)
            {
                haveftool = true;
                fgoal.SetActive(false);
            }
            if (Vector2.Distance(gravityTool.position, transform.position) < 0.5f)
            {
                possession += 1;
                gtool.SetActive(false);

            }
            if (possession > 0)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    possession -= 1;
                    // time duration
                    StartCoroutine(Gupside(3f));
                }
                /*else if (Input.GetKeyDown(KeyCode.H))
                {
                    possession -= 1;
                    // time duration
                    StartCoroutine(Grightside(3f));
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    possession -= 1;
                    // time duration
                    StartCoroutine(Gleftside(3f));
                }*/

            }
            if (haveftool)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    fgoal.SetActive(true);
                    navi.getconfused = true;
                    StartCoroutine(FakeGoal(3f));
                    haveftool = false;
                }
            }
        }
        // ���������UI��ʾ��������Ұ�����F���������¼��ص�ǰ����
        if (restart.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ReloadCurrentScene();
        }
        // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home");
        }
    }
    IEnumerator FakeGoal(float duration)
    {
        Debug.Log("fuse");
        navi.agent.SetDestination(fbuild.position);
        Debug.Log(navi.agent.destination);
        yield return new WaitForSeconds(duration);
        navi.getconfused = false;
        Debug.Log(navi.agent.destination);
    }
    IEnumerator Gupside(float duration)
    {
        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(originalGravity.x, -originalGravity.y);
        Debug.Log(Physics2D.gravity);
        yield return new WaitForSeconds(duration);
        Physics2D.gravity = originalGravity;
    }
    /*IEnumerator Grightside(float duration)
    {
        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(-originalGravity.y, originalGravity.x);
        Debug.Log(Physics2D.gravity);
        yield return new WaitForSeconds(duration); 
        Physics2D.gravity = originalGravity;
    }
    IEnumerator Gleftside(float duration)
    {
        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(originalGravity.y, originalGravity.x);
        Debug.Log(Physics2D.gravity);
        yield return new WaitForSeconds(duration); 
        Physics2D.gravity = originalGravity; // �ָ�����
    }*/

    void Jump()
    {
        Vector2 v = new Vector2(Physics2D.gravity.x, -Physics2D.gravity.y/9.8f);
        rb2d.AddForce(v * jumpForce, ForceMode2D.Impulse);
    }
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        rb2d.velocity = movement;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap") && !isCooldown) // ����Ƿ���ײ��trap�ذ�
        {
            StartCoroutine(Immobilize(immobilizeTime, other.gameObject));
            StartCoroutine(Cooldown());
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // �Ӵ�����ʱ���µ���״̬
        }
        if (other.gameObject.CompareTag("Goal")) // ����Ƿ���ײ��Goal
        {
            spriteRenderer.color = Color.green; // ��������ɫ��Ϊ��ɫ
            Time.timeScale = 0; // ��ֹ����
            success.SetActive(true); 
            restart.SetActive(true); 
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
    Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tool"))
        {
            StartCoroutine(TemporaryLoseGravity(FreeFlytime));
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
    IEnumerator Immobilize(float time, GameObject other)
    {
        isImmobilized = true;
        rb2d.velocity = Vector2.zero;
        spriteRenderer.color = Color.red;
        SpriteRenderer renderer = other.GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        yield return new WaitForSeconds(time);
        isImmobilized = false;
        spriteRenderer.color = originalColor;
        renderer.color = originalColor;
    }
    IEnumerator Cooldown()
    {
        isCooldown = true; // ��ʼ��ȴ
        yield return new WaitForSeconds(cooldownTime); // �ȴ���ȴʱ��
        isCooldown = false; // ������ȴ
    }
    void ReloadCurrentScene()
    {
        Time.timeScale = 1; // �����˶�
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // ��ȡ��ǰ����������
        SceneManager.LoadScene(sceneIndex); // �����������¼��س���
        possession = 0;
        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = originalGravity;
    }
}
