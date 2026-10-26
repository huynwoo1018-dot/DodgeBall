using UnityEngine;

// 일정 시간마다 공중의 랜덤 위치에 Sphere를 생성하는 스크립트
public class SphereSpawner : MonoBehaviour
{
    [Tooltip("떨어뜨릴 Sphere 프리팹 (Rigidbody 포함)")]
    [SerializeField] private GameObject spherePrefab;

    [Tooltip("생성 간격 (초)")]
    [SerializeField] private float spawnInterval = 3f;

    [Tooltip("생성 높이")]
    [SerializeField] private float spawnHeight = 10f;

    [Tooltip("생성 범위 (바닥 중심 기준 ± 거리). 10x10 바닥이면 4.5 정도")]
    [SerializeField] private float spawnRange = 4.5f;

    [Tooltip("생성된 Sphere가 자동 삭제되기까지 시간 (초)")]
    [SerializeField] private float sphereLifetime = 10f;

    // 생성 여부 (Game Over 시 false로 바꿀 수 있음)
    private bool isSpawning = true;

    private float timer = 0f;

    void Update()
    {
        if (!isSpawning) return;

        // 시간을 누적하다가 간격에 도달하면 생성
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            SpawnSphere();
        }
    }

    void SpawnSphere()
    {
        if (spherePrefab == null)
        {
            Debug.LogWarning("SphereSpawner: spherePrefab이 비어 있습니다.");
            return;
        }

        // 바닥 범위 안에서 X, Z를 랜덤으로 뽑고 높이는 고정
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(x, spawnHeight, z);

        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);

        // 계속 쌓이지 않도록 일정 시간 뒤 삭제
        Destroy(sphere, sphereLifetime);
    }
}
