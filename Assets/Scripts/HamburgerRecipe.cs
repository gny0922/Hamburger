using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HamburgerRecipe : MonoBehaviour
{
    [Header("재료 설정")]
    public List<GameObject> possibleIngredients; // 랜덤 재료 프리팹
    public int minIngredientCount = 1;
    public int maxIngredientCount = 3;

    [Header("UI")]
    public TextMeshProUGUI orderTextUI;

    [HideInInspector]
    public List<IngredientType> currentRecipe = new List<IngredientType>();
    [HideInInspector]
    public bool isSetOrder = false; // 세트 주문 여부
    [HideInInspector]
    public GameManager gameManager;

    public int orderCount = 0;

    // 햄버거 전용 대사
    private readonly List<string> burgerOnlyDialogs = new List<string>
    {
        "햄버거 좀 만들어줄래요? 순서는 {0}이에요!",
        "이 조합 진짜 맛있어요~ {0} 순서로 부탁해요!",
        "꼭 {0} 순서대로 만들어주세요!",
        "오늘은 이거 먹고 싶어요! 👉 {0}",
        "{0} 순서대로 쌓아주세요~ 실수는 노노!"
    };

    // 감자튀김 포함 대사 (5번 이상)
    private readonly List<string> burgerAndFriesDialogs = new List<string>
    {
        "햄버거는 {0} 순서로! 그리고 감자튀김도 주세요!",
        "오늘은 햄버거({0})랑 감튀까지 부탁드려요~",
        "햄버거 {0} 순서로 만들고 감자튀김 하나 추가요!",
        "배고파서 많이 먹을래요! {0} 햄버거랑 감튀요!",
        "{0} 햄버거에 바삭한 감자튀김도 같이 주세요!"
    };

    // 세트 전체 대사 (7번 이상)
    private readonly List<string> fullSetDialogs = new List<string>
    {
        "{0} 순서 햄버거랑 감자튀김, 콜라까지! 세트로 주세요!",
        "풀 세트로 주세요! {0} 순서 햄버거에 감튀랑 콜라도요.",
        "{0} 순서로 햄버거 만들고 감자튀김이랑 콜라도 챙겨주세요!",
        "햄버거({0}), 감자튀김 하나, 콜라 한 잔 주세요!",
        "오늘 스트레스 받아서 많이 먹을래요! 햄버거({0}), 감튀, 콜라요!",
        "세트 메뉴로 {0} 햄버거, 감자튀김, 콜라 부탁해요!",
        "완전체 세트! {0} 햄버거 + 감튀 + 콜라로 주세요!"
    };

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        GenerateRandomOrderText();
    }

    public void GenerateRandomOrderText()
    {
        orderCount++;
        currentRecipe.Clear();

        // 1. 빵 아래
        currentRecipe.Add(IngredientType.BreadBottom);

        // 2. 랜덤 재료
        int count = Random.Range(minIngredientCount, maxIngredientCount + 1);
        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, possibleIngredients.Count);
            IngredientType type = possibleIngredients[rand].GetComponent<IngredientData>().ingredientType;
            currentRecipe.Add(type);
        }

        // 3. 빵 위
        currentRecipe.Add(IngredientType.BreadTop);

        // 4. 텍스트용 레시피 문자열
        string recipeString = string.Join(" → ", currentRecipe);

        // 5. 주문 타입 결정 및 대사 선택
        string template = "";
        if (orderCount < 5)
        {
            // 햄버거만
            isSetOrder = false;
            template = burgerOnlyDialogs[Random.Range(0, burgerOnlyDialogs.Count)];
            Debug.Log($"주문 {orderCount}: 햄버거 단품");
        }
        else if (orderCount < 7)
        {
            // 햄버거 + 감자튀김 (부분 세트)
            isSetOrder = true; // 세트 주문으로 처리 (감자튀김 필요)
            template = burgerAndFriesDialogs[Random.Range(0, burgerAndFriesDialogs.Count)];
            Debug.Log($"주문 {orderCount}: 햄버거 + 감자튀김");
        }
        else
        {
            // 풀 세트 (햄버거 + 감자튀김 + 콜라)
            isSetOrder = true;
            template = fullSetDialogs[Random.Range(0, fullSetDialogs.Count)];
            Debug.Log($"주문 {orderCount}: 풀 세트");
        }

        // 6. UI 출력
        string finalText = string.Format(template, recipeString);
        if (orderTextUI != null)
            orderTextUI.text = finalText;
        else
            Debug.LogWarning(" orderTextUI가 비어 있음");

        Debug.Log($"새 주문 생성 - 세트 주문: {isSetOrder}, 레시피: {recipeString}");
    }

    //  OrderDeliveryZone에서 호출됨 – 정답 비교 (개선된 버전)
    public bool CheckPlayerBurger(List<IngredientType> playerBurger)
    {
        Debug.Log("=== 햄버거 체크 시작 ===");
        Debug.Log($"플레이어 햄버거 재료 수: {playerBurger.Count}");
        Debug.Log($"정답 레시피 재료 수: {currentRecipe.Count}");

        // 재료 개수가 다르면 실패
        if (playerBurger.Count != currentRecipe.Count)
        {
            Debug.Log($" 재료 개수 불일치: 플레이어 {playerBurger.Count} vs 레시피 {currentRecipe.Count}");
            return false;
        }

        // 각 재료를 순서대로 비교
        for (int i = 0; i < currentRecipe.Count; i++)
        {
            Debug.Log($"위치 {i}: 플레이어={playerBurger[i]} vs 레시피={currentRecipe[i]}");

            if (currentRecipe[i] != playerBurger[i])
            {
                Debug.Log($" 재료 불일치 위치 {i}: 플레이어 {playerBurger[i]} vs 레시피 {currentRecipe[i]}");
                return false;
            }
        }

        Debug.Log(" 햄버거 레시피 완벽 일치!");
        return true;
    }

    // 디버깅용 메서드
    public void DebugCurrentOrder()
    {
        Debug.Log($"현재 주문 {orderCount}번 - 세트: {isSetOrder}");
        Debug.Log($"레시피: {string.Join(" → ", currentRecipe)}");

        Debug.Log("=== 현재 레시피 상세 ===");
        for (int i = 0; i < currentRecipe.Count; i++)
        {
            Debug.Log($"위치 {i}: {currentRecipe[i]}");
        }
    }
}