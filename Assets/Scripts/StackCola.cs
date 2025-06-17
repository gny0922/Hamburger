using UnityEngine;

public class StackCola : MonoBehaviour
{
    public GameObject meshPrefab;
    private bool isPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        if (other.name.Contains("Tray"))
        {
            isPlaced = true;

            GameObject newCola = Instantiate(meshPrefab, other.transform);
            newCola.transform.localPosition = new Vector3(-0.15f, 0.02f, 0f); // ÇÜ¹ö°Å ¿ÞÂÊ
            newCola.transform.localRotation = Quaternion.identity;

            Destroy(gameObject);
        }
    }
}
