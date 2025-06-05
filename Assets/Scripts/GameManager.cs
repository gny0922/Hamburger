using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject countdownUI;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
   

    public float gameDuration = 180f; // 3분
    private float remainingTime;
    private bool gameRunning = false;

    public GameObject customerAsset;
   // public BurgerGenerator burgerGenerator;

    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        countdownUI.SetActive(true);

        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownUI.SetActive(false);
        StartGame(); // 카운트 끝나고 게임 시작
    }

    void StartGame()
    {
        gameRunning = true;
        remainingTime = gameDuration;

        customerAsset.SetActive(true);              // 손님 에셋 등장
        
        //  burgerGenerator.GenerateBurger();           // 예시 햄버거 생성

        // 타이머 동작 시작 → Update()에서 관리
    }

    void Update()
    {
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                gameRunning = false;
                EndGame();
            }

            UpdateTimerUI(); // 여기서 UI 갱신
        }

        if (!gameRunning) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            gameRunning = false;
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("게임 종료! 점수 표시 등...");
        // 결과 패널 표시 or 씬 전환 등 구현
       
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
