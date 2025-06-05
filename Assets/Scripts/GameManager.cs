using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject countdownUI;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
   

    public float gameDuration = 180f; // 3��
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
        StartGame(); // ī��Ʈ ������ ���� ����
    }

    void StartGame()
    {
        gameRunning = true;
        remainingTime = gameDuration;

        customerAsset.SetActive(true);              // �մ� ���� ����
        
        //  burgerGenerator.GenerateBurger();           // ���� �ܹ��� ����

        // Ÿ�̸� ���� ���� �� Update()���� ����
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

            UpdateTimerUI(); // ���⼭ UI ����
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
        Debug.Log("���� ����! ���� ǥ�� ��...");
        // ��� �г� ǥ�� or �� ��ȯ �� ����
       
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
