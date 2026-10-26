using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// WASD로 플레이어를 바닥 위(X-Z 평면)에서 이동시키는 스크립트
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("이동 속도 (초당 이동 거리)")]
    public float moveSpeed = 5f;

    // 이동 가능 여부 (Game Over 시 false로 바뀜)
    public bool canMove = true;

    // Game Over 상태인지 (다른 스크립트에서 읽기 전용으로 확인)
    public bool IsGameOver { get; private set; } = false;

    [Tooltip("플레이어 높이가 이 값보다 낮아지면 바닥에서 떨어진 것으로 보고 Game Over")]
    public float fallThreshold = -2f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 공에 맞거나 큐브에 부딪혀도 캡슐이 넘어지지 않도록 회전 고정
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // 0) 바닥 밖으로 떨어졌는지 검사
        if (!IsGameOver && transform.position.y < fallThreshold)
        {
            GameOver();
            return;
        }

        // 1) 키 입력 읽기 (입력은 Update에서)
        float x = 0f;
        float z = 0f;

#if ENABLE_INPUT_SYSTEM
        // 새 Input System
        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.wKey.isPressed) z += 1f;
            if (kb.sKey.isPressed) z -= 1f;
            if (kb.dKey.isPressed) x += 1f;
            if (kb.aKey.isPressed) x -= 1f;
        }
#else
        // 예전 Input Manager
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
#endif

        // 대각선 이동이 더 빨라지지 않도록 정규화
        moveInput = new Vector3(x, 0f, z).normalized;
    }

    void FixedUpdate()
    {
        // 2) 실제 이동 (물리 이동은 FixedUpdate에서)
        if (!canMove) return;

        Vector3 nextPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
    }


    // 다른 Collider와 부딪힌 순간 한 번 호출됨
    void OnCollisionEnter(Collision collision)
    {
        // 이미 Game Over면 무시 (공에 여러 번 맞아도 한 번만 출력)
        if (IsGameOver) return;

        // 부딪힌 대상이 Sphere(Obstacle 태그)일 때만 Game Over
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        IsGameOver = true;
        canMove = false;

        // 남아 있는 속도를 없애고, 이후 공에 맞아도 밀려나지 않도록 물리 영향 차단
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        Debug.Log("Game Over");
    }
}
