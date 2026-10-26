using UnityEngine;

// 붙은 오브젝트를 Y축 기준으로 계속 회전시키는 스크립트
public class Rotator : MonoBehaviour
{
    [Tooltip("Y축 회전 속도 (초당 각도)")]
    [SerializeField] private float rotationSpeed = 90f;

    void Update()
    {
        // Time.deltaTime을 곱해서 프레임 수와 상관없이 '초당' 90도로 회전
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
