using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OrderDeliveryZone : MonoBehaviour
{
    public BoxCollider deliveryZone;
    public List<StackHamburger> hamburgersInZone = new List<StackHamburger>();
    public List<GameObject> friesInZone = new List<GameObject>();
    public List<GameObject> colasInZone = new List<GameObject>();

    public TextMeshProUGUI statusText;
    public HamburgerRecipe hamburgerRecipe;
    public GameManager gameManager;

    private void Start()
    {
        if (hamburgerRecipe == null)
            hamburgerRecipe = FindObjectOfType<HamburgerRecipe>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        UpdateStatusText();
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"OnTriggerEnter: {other.name}");

        // StackHamburger 컴포넌트를 직접 찾기 (GetComponentInChildren 대신)
        StackHamburger hamburger = other.GetComponentInParent<StackHamburger>();
        if (hamburger != null)
        {
            Debug.Log($"햄버거 발견: {hamburger.name}, 완성 상태: {hamburger.isComplete}");
            if (!hamburgersInZone.Contains(hamburger))
            {
                hamburgersInZone.Add(hamburger);
                Debug.Log($"햄버거 등록됨 (완성 여부 상관없음): {hamburger.name}");
            }
        }

        if (other.CompareTag("Fries") && !friesInZone.Contains(other.gameObject))
        {
            friesInZone.Add(other.gameObject);
            Debug.Log($"감자튀김 추가됨. 총 개수: {friesInZone.Count}");
        }

        if (other.CompareTag("Cola") && !colasInZone.Contains(other.gameObject))
        {
            colasInZone.Add(other.gameObject);
            Debug.Log($"콜라 추가됨. 총 개수: {colasInZone.Count}");
        }

        UpdateStatusText();
    }

    private void OnTriggerExit(Collider other)
    {
        StackHamburger hamburger = other.GetComponent<StackHamburger>();
        if (hamburger != null)
        {
            hamburgersInZone.Remove(hamburger);
            Debug.Log($"햄버거 제거됨. 남은 개수: {hamburgersInZone.Count}");
        }

        if (other.CompareTag("Fries"))
        {
            friesInZone.Remove(other.gameObject);
            Debug.Log($"감자튀김 제거됨. 남은 개수: {friesInZone.Count}");
        }

        if (other.CompareTag("Cola"))
        {
            colasInZone.Remove(other.gameObject);
            Debug.Log($"콜라 제거됨. 남은 개수: {colasInZone.Count}");
        }

        UpdateStatusText();
    }

    public void ManualCheckOrder()
    {
        Debug.Log($"수동 체크 시작 - 햄버거 수: {hamburgersInZone.Count}");
        Debug.Log("주문 체크 시작");

        if (hamburgerRecipe == null || gameManager == null)
        {
            Debug.LogError("HamburgerRecipe 또는 GameManager가 null입니다!");
            return;
        }

        
        // null이거나 파괴된 오브젝트들을 리스트에서 제거
        CleanupDestroyedObjects();

        if (!hamburgerRecipe.isSetOrder)
        {
            Debug.Log("단품 주문 체크");
            if (hamburgersInZone.Count > 0)
            {
                StackHamburger burger = hamburgersInZone[0];

                Debug.Log("정답 레시피: " + string.Join(" → ", hamburgerRecipe.currentRecipe));
                Debug.Log("플레이어 제출: " + string.Join(" → ", burger.stackedIngredients));

                Debug.Log($"햄버거 체크: 재료 개수 {burger.stackedIngredients.Count}");

                // 재료 디버깅
                for (int i = 0; i < burger.stackedIngredients.Count; i++)
                {
                    Debug.Log($"재료 {i}: {burger.stackedIngredients[i]}");
                }

                // 최소 재료 수 체크 및 레시피 매치 체크
                bool hasMinIngredients = burger.stackedIngredients.Count >= 3;
                bool recipeMatch = hasMinIngredients && hamburgerRecipe.CheckPlayerBurger(burger.stackedIngredients);

                Debug.Log($"최소 재료: {hasMinIngredients}, 레시피 매치: {recipeMatch}");

                // 리스트에서 먼저 제거한 후 오브젝트 파괴
                hamburgersInZone.RemoveAt(0);
                burger.ResetHamburger();

                if (recipeMatch)
                {
                    Debug.Log("올바른 햄버거! 점수 획득");
                    if (colasInZone.Count > 0)
                    {
                        GameObject cola = colasInZone[0];
                        colasInZone.RemoveAt(0);
                        Destroy(cola);
                        gameManager.AddScore(1500);
                        Debug.Log("콜라 보너스 포함 1500점!");
                    }
                    else
                    {
                        gameManager.AddScore(1000);
                        Debug.Log("기본 1000점!");
                    }
                }
                else
                {
                    Debug.Log("잘못된 햄버거 - 500점 감점");
                    gameManager.AddScore(-500);
                }

                gameManager.OnHamburgerCompleted();
            }
            else
            {
                Debug.Log("햄버거 없음 - 500점 감점");
                gameManager.AddScore(-500);
                gameManager.OnHamburgerCompleted();
            }
        }
        else
        {
            Debug.Log("세트 주문 체크");
            if (hamburgersInZone.Count > 0)
            {
                StackHamburger burger = hamburgersInZone[0];
                bool hasMinIngredients = burger.stackedIngredients.Count >= 3;
                bool burgerCorrect = hasMinIngredients && hamburgerRecipe.CheckPlayerBurger(burger.stackedIngredients);
                bool friesOK = friesInZone.Count > 0;
                bool colaOK = colasInZone.Count > 0;

                Debug.Log($"세트 체크 - 햄버거: {burgerCorrect}, 감자튀김: {friesOK}, 콜라: {colaOK}");

                // 리스트에서 먼저 제거한 후 오브젝트들 파괴
                hamburgersInZone.RemoveAt(0);
                burger.ResetHamburger();

                if (friesOK)
                {
                    GameObject fries = friesInZone[0];
                    friesInZone.RemoveAt(0);
                    Destroy(fries);
                }

                if (colaOK)
                {
                    GameObject cola = colasInZone[0];
                    colasInZone.RemoveAt(0);
                    Destroy(cola);
                }

                if (burgerCorrect && friesOK && colaOK)
                {
                    Debug.Log("완벽한 세트 주문 완성! 1700점!");
                    gameManager.AddScore(1700);
                }
                else
                {
                    Debug.Log("불완전한 세트 주문 - 500점 감점");
                    gameManager.AddScore(-500);
                }

                gameManager.OnHamburgerCompleted();
            }
            else
            {
                Debug.Log("햄버거 없음 - 500점 감점");
                gameManager.AddScore(-500);
                gameManager.OnHamburgerCompleted();
            }
        }

        UpdateStatusText();
    }


    
    // 안전하게 햄버거 오브젝트를 파괴하는 메서드
    private void DestroyHamburgerSafely(StackHamburger burger)
    {
        if (burger != null && burger.gameObject != null)
        {
            // 자식 오브젝트들(재료들)도 함께 파괴됨
            Destroy(burger.gameObject);
            Debug.Log($"햄버거 오브젝트 파괴됨: {burger.name}");
        }
        else
        {
            Debug.LogWarning("파괴하려는 햄버거가 null이거나 이미 파괴되었습니다.");
        }
    }

    // 파괴된 오브젝트들을 리스트에서 정리하는 메서드
    private void CleanupDestroyedObjects()
    {
        // null이거나 파괴된 햄버거들 제거
        for (int i = hamburgersInZone.Count - 1; i >= 0; i--)
        {
            if (hamburgersInZone[i] == null || hamburgersInZone[i].gameObject == null)
            {
                hamburgersInZone.RemoveAt(i);
            }
        }

        // null이거나 파괴된 감자튀김들 제거
        for (int i = friesInZone.Count - 1; i >= 0; i--)
        {
            if (friesInZone[i] == null)
            {
                friesInZone.RemoveAt(i);
            }
        }

        // null이거나 파괴된 콜라들 제거
        for (int i = colasInZone.Count - 1; i >= 0; i--)
        {
            if (colasInZone[i] == null)
            {
                colasInZone.RemoveAt(i);
            }
        }
    }

    

    private void UpdateStatusText()
    {
        if (statusText == null) return;

        if (!hamburgerRecipe.isSetOrder)
        {
            statusText.text = $"햄버거 필요: {hamburgersInZone.Count}/1";
        }
        else
        {
            statusText.text =
                $"세트 주문\n햄버거: {hamburgersInZone.Count}/1\n감자튀김: {friesInZone.Count}/1\n콜라: {colasInZone.Count}/1";
        }
    }
}