using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketStacker : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    public Transform stackRoot;           // 스택 기준 위치
    public float stackHeight = 0.05f;     // 재료 하나당 높이
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
        // 스택 증가
        stackCount++;

        // 현재 소켓 위치를 새 위치로 위로 올림
        Vector3 nextPos = stackRoot.position + Vector3.up * (stackCount * stackHeight);
        socketInteractor.transform.position = nextPos;
    }
}
