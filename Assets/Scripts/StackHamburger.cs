using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackHamburger : MonoBehaviour
{
    private Rigidbody rigid;

    // 재료 종류 저장
    public List<IngredientType> stackedIngredients = new List<IngredientType>();

    // 쌓인 높이
    public float totalHeight = 0.005f;

    public BoxCollider triggerCollider;
    public TextMeshPro textMesh;
    public int dishNumber;

    public bool isComplete = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        textMesh?.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 안 잡고 있으면 물리작용 받게
        if (transform.parent == null)
        {
            rigid.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            IngredientData data = other.GetComponent<IngredientData>();
            if (data != null)
            {
                StackIngredient(data.meshPrefab, data.height, data.ingredientType);
                Destroy(other.gameObject); // 재료는 쌓고 나면 제거
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

        // Collider도 같이 키워줌
        triggerCollider.size = new Vector3(triggerCollider.size.x, triggerCollider.size.y + height, triggerCollider.size.z);
        triggerCollider.center = new Vector3(triggerCollider.center.x, triggerCollider.center.y + (height / 2), triggerCollider.center.z);

        CheckHamburger();
    }

    private void CheckHamburger()
    {
        // 임시로 조건 없이 완성 처리
        if (stackedIngredients.Count >= 3)
        {
            isComplete = true;
            Debug.Log("햄버거 완성!");
        }
    }
}
