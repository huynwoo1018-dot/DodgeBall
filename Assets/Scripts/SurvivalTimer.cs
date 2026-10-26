using UnityEngine;
using UnityEngine.UI;

// 생존 시간을 세고 화면 좌상단 Text에 표시하는 스크립트
public class SurvivalTimer : MonoBehaviour
{
    [Tooltip("Game Over 여부를 확인할 플레이어")]
    public PlayerController player;

    [Tooltip("시간을 표시할 UI Text")]
    public Text timeText;

    private float survivalTime = 0f;

    void Update()
    {
        // 플레이어가 Game Over가 아닐 때만 시간 증가 → Game Over 후에는 시간 정지
        if (player != null && !player.IsGameOver)
        {
            survivalTime += Time.deltaTime;
        }

        if (timeText != null)
        {
            // 소수점 첫째 자리까지 초 단위로 표시
            timeText.text = "Time: " + survivalTime.ToString("F1") + "s";
        }
    }
}
