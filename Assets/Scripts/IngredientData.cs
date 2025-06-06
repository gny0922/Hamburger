using UnityEngine;

public class IngredientData : MonoBehaviour
{
    public IngredientType ingredientType;
    public float height = 0.02f;  // 각 재료의 높이
    public GameObject meshPrefab; // 쌓일 때 복제될 메쉬 프리팹
}
