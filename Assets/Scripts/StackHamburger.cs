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
        Debug.Log($"��� �߰���: {ingredientType} (�� {stackedIngredients.Count}��)");

        GameObject newMesh = Instantiate(meshObject, transform);
        newMesh.transform.localPosition = new Vector3(0, totalHeight, 0);
        newMesh.transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 180f), 0);

        totalHeight += height;

        // �ּ� ��� ���� üũ (3�� �̻��̸� �ϼ����� ǥ��)
        CheckMinimumIngredients();

        // ������: ���� ���� ���� ���
        DebugCurrentStack();
    }

    private void CheckMinimumIngredients()
    {
        if (stackedIngredients.Count >= 3) // �ּ� ��� ��
        {
            isComplete = true;
            if (textMesh != null)
            {
                textMesh.text = "�غ��";
                textMesh.color = Color.yellow; // ��������� �غ� ���� ǥ��
                textMesh.gameObject.SetActive(true);
            }
            Debug.Log($"�ܹ��� �غ� �Ϸ� (��� {stackedIngredients.Count}��)");
        }
    }

    // ������: ���� ���� ������ ���
    private void DebugCurrentStack()
    {
        Debug.Log("=== ���� �ܹ��� ���� ===");
        Debug.Log($"��� ����: {stackedIngredients.Count}");
        for (int i = 0; i < stackedIngredients.Count; i++)
        {
            Debug.Log($"��ġ {i}: {stackedIngredients[i]}");
        }
        Debug.Log($"�ϼ� ����: {isComplete}");
        Debug.Log("====================");
    }

    public void ResetHamburger()
    {
        Debug.Log($"�ܹ��� ���� ����: {gameObject.name}");

        // �ڽ� �� IngredientData�� ���� ��Ḹ ����
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            // IngredientData ������Ʈ�� �ִ� �͸� ���� (����)
            if (child.GetComponent<IngredientData>() != null)
            {
                Debug.Log($"��� ����: {child.name}");
                Destroy(child.gameObject);
            }
        }

        // ���� �ʱ�ȭ
        stackedIngredients.Clear();
        totalHeight = 0.005f;
        isComplete = false;

        // �ؽ�Ʈ �����
        if (textMesh != null)
            textMesh.gameObject.SetActive(false);

        Debug.Log($"�ܹ��� ���� �Ϸ�: {gameObject.name}");
    }

    // ����� �޼��� - �ܺο��� ȣ�� ����
    public void PrintCurrentStack()
    {
        DebugCurrentStack();
    }
}