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

            // ����Ƣ�� ������Ʈ�� Ʈ���� ���� ���̱�
            GameObject newFries = Instantiate(meshPrefab, other.transform);
            newFries.transform.localPosition = new Vector3(0.15f, 0.02f, 0f); // �ܹ��� ������
            newFries.transform.localRotation = Quaternion.identity;

            Destroy(gameObject); // ���� ����Ƣ�� ����
        }
    }
}
