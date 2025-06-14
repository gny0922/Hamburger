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
    public bool isComplete = false; // 최소 재료 수 충족 여부만 표시
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
        triggerCollider.size = new Vector3(triggerCollider.size.x, triggerCollider.size.y + height, triggerCollider.size.z);
        triggerCollider.center = new Vector3(triggerCollider.center.x, triggerCollider.center.y + height / 2f, triggerCollider.center.z);

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
        Debug.Log("햄버거 리셋");

        // 자식 오브젝트들(재료들) 모두 제거
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        stackedIngredients.Clear();
        totalHeight = 0.005f;
        isComplete = false;

        triggerCollider.size = new Vector3(triggerCollider.size.x, 0.1f, triggerCollider.size.z);
        triggerCollider.center = new Vector3(triggerCollider.center.x, 0.05f, triggerCollider.center.z);

        if (textMesh != null)
            textMesh.gameObject.SetActive(false);
    }

    // 디버깅용 메서드 - 외부에서 호출 가능
    public void PrintCurrentStack()
    {
        DebugCurrentStack();
    }
}