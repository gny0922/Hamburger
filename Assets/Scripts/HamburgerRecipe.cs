using UnityEngine;
using System.Collections.Generic;

public class HamburgerRecipe : MonoBehaviour
{
    public Transform previewParent; // ���� �ܹ��Ű� ���� �θ� ������Ʈ (�մ� �� ��)
    public GameObject breadBottomPrefab;
    public GameObject breadTopPrefab;

    public List<GameObject> possibleIngredients; // ġ��, �丶��, ��Ƽ ��
    public int minIngredientCount = 1;
    public int maxIngredientCount = 3;

    public float ingredientHeight = 0.02f; // �⺻ ���� (������ ����)

    public List<IngredientType> currentRecipe = new List<IngredientType>(); // ���� ��Ͽ�

    public void GenerateRandomHamburger()
    {
        // 1. ���� �ܹ��� �ʱ�ȭ
        foreach (Transform child in previewParent)
            Destroy(child.gameObject);

        currentRecipe.Clear();
        float currentHeight = 0f;

        // 2. �� �Ʒ�
        Instantiate(breadBottomPrefab, previewParent.position + Vector3.up * currentHeight, Quaternion.identity, previewParent);
        currentRecipe.Add(IngredientType.BreadBottom);
        currentHeight += ingredientHeight;

        // 3. ���� ����
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

        // 4. �� ��
        Instantiate(breadTopPrefab, previewParent.position + Vector3.up * currentHeight, Quaternion.identity, previewParent);
        currentRecipe.Add(IngredientType.BreadTop);
    }
}
