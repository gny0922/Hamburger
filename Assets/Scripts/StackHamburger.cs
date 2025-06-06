using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackHamburger : MonoBehaviour
{
    private Rigidbody rigid;

    // ��� ���� ����
    public List<IngredientType> stackedIngredients = new List<IngredientType>();

    // ���� ����
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
        // �� ��� ������ �����ۿ� �ް�
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
                Destroy(other.gameObject); // ���� �װ� ���� ����
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

        // Collider�� ���� Ű����
        triggerCollider.size = new Vector3(triggerCollider.size.x, triggerCollider.size.y + height, triggerCollider.size.z);
        triggerCollider.center = new Vector3(triggerCollider.center.x, triggerCollider.center.y + (height / 2), triggerCollider.center.z);

        CheckHamburger();
    }

    private void CheckHamburger()
    {
        // �ӽ÷� ���� ���� �ϼ� ó��
        if (stackedIngredients.Count >= 3)
        {
            isComplete = true;
            Debug.Log("�ܹ��� �ϼ�!");
        }
    }
}
