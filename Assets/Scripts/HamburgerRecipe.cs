using UnityEngine;
using System.Collections.Generic;

public class HamburgerRecipe : MonoBehaviour
{
    public Transform previewParent; // 예시 햄버거가 쌓일 부모 오브젝트 (손님 앞 등)
    public GameObject breadBottomPrefab;
    public GameObject breadTopPrefab;

    public List<GameObject> possibleIngredients; // 치즈, 토마토, 패티 등
    public int minIngredientCount = 1;
    public int maxIngredientCount = 3;

    public float ingredientHeight = 0.02f; // 기본 높이 (프리셋 가능)

    public List<IngredientType> currentRecipe = new List<IngredientType>(); // 정답 기록용

    public void GenerateRandomHamburger()
    {
        // 1. 예시 햄버거 초기화
        foreach (Transform child in previewParent)
            Destroy(child.gameObject);

        currentRecipe.Clear();
        float currentHeight = 0f;

        // 2. 빵 아래
        Instantiate(breadBottomPrefab, previewParent.position + Vector3.up * currentHeight, Quaternion.identity, previewParent);
        currentRecipe.Add(IngredientType.BreadBottom);
        currentHeight += ingredientHeight;

        // 3. 랜덤 재료들
        int count = Random.Range(minIngredientCount, maxIngredientCount + 1);
        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, possibleIngredients.Count);
            GameObject ingredient = possibleIngredients[rand];
            IngredientType type = ingredient.GetComponent<IngredientData>().ingredientType;

            Instantiate(ingredient, previewParent.position + Vector3.up * currentHeight, Quaternion.identity, previewParent);
            currentRecipe.Add(type);
            currentHeight += ingredientHeight;
        }

        // 4. 빵 위
        Instantiate(breadTopPrefab, previewParent.position + Vector3.up * currentHeight, Quaternion.identity, previewParent);
        currentRecipe.Add(IngredientType.BreadTop);
    }
}
