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

        GameObject newMesh = Instantiate(meshObject, transform);
        newMesh.transform.localPosition = new Vector3(0, totalHeight, 0);
        newMesh.transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 180f), 0);

        totalHeight += height;
        triggerCollider.size = new Vector3(triggerCollider.size.x, triggerCollider.size.y + height, triggerCollider.size.z);
        triggerCollider.center = new Vector3(triggerCollider.center.x, triggerCollider.center.y + height / 2f, triggerCollider.center.z);

        // 최소 재료 수만 체크 (예: 3개 이상이면 완성으로 표시)
        CheckMinimumIngredients();
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
        }
    }

    public void ResetHamburger()
    {
        // CheeseRoot 같은 껍데기까지 포함해서 전부 제거
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
}