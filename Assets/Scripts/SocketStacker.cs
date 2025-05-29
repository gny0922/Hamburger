using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketStacker : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    public Transform stackRoot;           // ���� ���� ��ġ
    public float stackHeight = 0.05f;     // ��� �ϳ��� ����
    private int stackCount = 0;

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnIngredientPlaced);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnIngredientPlaced);
    }

    private void OnIngredientPlaced(SelectEnterEventArgs args)
    {
        // ���� ����
        stackCount++;

        // ���� ���� ��ġ�� �� ��ġ�� ���� �ø�
        Vector3 nextPos = stackRoot.position + Vector3.up * (stackCount * stackHeight);
        socketInteractor.transform.position = nextPos;
    }
}
