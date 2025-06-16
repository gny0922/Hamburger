using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject countdownUI;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TMPro.TextMeshPro scoreTextUI;

    public HamburgerRecipe hamburgerRecipe;
    public float gameDuration = 180f;
    public GameObject customerAsset;

    public int completedHamburgers = 0;
    public int hamburgerThreshold = 5;

    private float remainingTime;
    private bool gameRunning = false;

    public int score = 0;

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
        StartGame();
    }

    void StartGame()
    {
        gameRunning = true;
        remainingTime = gameDuration;
        completedHamburgers = 0;
        score = 0;
        customerAsset.SetActive(true);
        hamburgerRecipe.GenerateRandomOrderText();
        UpdateScoreUI();
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
            UpdateTimerUI();
        }
    }

    public void OnHamburgerCompleted()
    {
        completedHamburgers++;
        Debug.Log($"햄버거 완성! 총 {completedHamburgers}개");

        if (completedHamburgers >= hamburgerThreshold)
        {
            hamburgerRecipe.isSetOrder = true;
            Debug.Log("세트 주문 모드 활성화!");
        }

        GenerateNextOrder();
    }

    void GenerateNextOrder()
    {
        if (gameRunning)
        {
            hamburgerRecipe.GenerateRandomOrderText();
        }


    }

    void EndGame()
    {
        Debug.Log($"게임 종료! 총 {completedHamburgers}개 / 점수: {score}");
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreTextUI != null)
            scoreTextUI.text = $"score: {score}";
    }

    public bool IsGameRunning() => gameRunning;
    public int GetCompletedHamburgers() => completedHamburgers;
}
