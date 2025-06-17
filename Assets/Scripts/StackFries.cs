using UnityEngine;

public class StackFries : MonoBehaviour
{
    public GameObject meshPrefab;
    private bool isPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        if (other.name.Contains("Tray"))
        {
            isPlaced = true;

            // 감자튀김 오브젝트를 트레이 위에 붙이기
            GameObject newFries = Instantiate(meshPrefab, other.transform);
            newFries.transform.localPosition = new Vector3(0.15f, 0.02f, 0f); // 햄버거 오른쪽
            newFries.transform.localRotation = Quaternion.identity;

            Destroy(gameObject); // 원래 감자튀김 제거
        }
    }
}
