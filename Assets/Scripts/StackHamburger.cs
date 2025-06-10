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
        if (isComplete) return;

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

        CheckHamburger();
    }

    private void CheckHamburger()
    {
        if (isComplete || hamburgerRecipe == null) return;
        if (stackedIngredients.Count < 3) return;

        if (hamburgerRecipe.CheckPlayerBurger(stackedIngredients))
        {
            isComplete = true;
            Debug.Log("정답! 햄버거 완성");

            if (textMesh != null)
            {
                textMesh.text = "완성!";
                textMesh.gameObject.SetActive(true);
            }
        }
        else
        {
            if (hamburgerRecipe.gameManager != null)
                hamburgerRecipe.gameManager.AddScore(-500);

            Debug.Log("틀렸습니다.");

            if (textMesh != null)
            {
                textMesh.text = "틀림!";
                textMesh.gameObject.SetActive(true);
                Invoke("HideText", 2f);
            }
        }
    }

    private void HideText()
    {
        if (textMesh != null)
            textMesh.gameObject.SetActive(false);
    }

    public void ResetHamburger()
    {
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
