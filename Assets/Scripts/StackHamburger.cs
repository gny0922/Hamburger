using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackHamburger : MonoBehaviour
{
    private Rigidbody rigid;
    public List<IngredientType> stackedIngredients = new List<IngredientType>();
    public float totalHeight = 0.005f;
    public BoxCollider triggerCollider;
    public TextMeshPro textMesh;
    public int dishNumber;
    public bool isComplete = false;
    public HamburgerRecipe hamburgerRecipe;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        textMesh?.gameObject.SetActive(false);
        if (hamburgerRecipe == null)
            hamburgerRecipe = FindObjectOfType<HamburgerRecipe>();
    }

    private void Update()
    {
        if (transform.parent == null)
            rigid.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            IngredientData data = other.GetComponent<IngredientData>();
            if (data != null)
            {
                StackIngredient(data.meshPrefab, data.height, data.ingredientType);
                Destroy(other.gameObject);
            }
        }
    }

    public void StackIngredient(GameObject meshObject, float height, IngredientType ingredientType)
    {
        stackedIngredients.Add(ingredientType);
        Debug.Log($"재료 추가됨: {ingredientType} (총 {stackedIngredients.Count}개)");

        GameObject newMesh = Instantiate(meshObject, transform);
        newMesh.transform.localPosition = new Vector3(0, totalHeight, 0);
        newMesh.transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 180f), 0);

        totalHeight += height;

        // 최소 재료 수만 체크 (3개 이상이면 완성으로 표시)
        CheckMinimumIngredients();

        // 디버깅용: 현재 쌓인 재료들 출력
        DebugCurrentStack();
    }

    private void CheckMinimumIngredients()
    {
        if (stackedIngredients.Count >= 3) // 최소 재료 수
        {
            isComplete = true;
            if (textMesh != null)
            {
                textMesh.text = "준비됨";
                textMesh.color = Color.yellow; // 노란색으로 준비 상태 표시
                textMesh.gameObject.SetActive(true);
            }
            Debug.Log($"햄버거 준비 완료 (재료 {stackedIngredients.Count}개)");
        }
    }

    // 디버깅용: 현재 쌓인 재료들을 출력
    private void DebugCurrentStack()
    {
        Debug.Log("=== 현재 햄버거 상태 ===");
        Debug.Log($"재료 개수: {stackedIngredients.Count}");
        for (int i = 0; i < stackedIngredients.Count; i++)
        {
            Debug.Log($"위치 {i}: {stackedIngredients[i]}");
        }
        Debug.Log($"완성 상태: {isComplete}");
        Debug.Log("====================");
    }

    public void ResetHamburger()
    {
        Debug.Log($"햄버거 리셋 시작: {gameObject.name}");

        // 자식 중 IngredientData가 붙은 재료만 삭제
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            // IngredientData 컴포넌트가 있는 것만 삭제 (재료들)
            if (child.GetComponent<IngredientData>() != null)
            {
                Debug.Log($"재료 삭제: {child.name}");
                Destroy(child.gameObject);
            }
        }

        // 상태 초기화
        stackedIngredients.Clear();
        totalHeight = 0.005f;
        isComplete = false;

        // 텍스트 숨기기
        if (textMesh != null)
            textMesh.gameObject.SetActive(false);

        Debug.Log($"햄버거 리셋 완료: {gameObject.name}");
    }

    // 디버깅 메서드 - 외부에서 호출 가능
    public void PrintCurrentStack()
    {
        DebugCurrentStack();
    }
}