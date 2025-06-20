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

   
    public HamburgerRecipe hamburgerRecipe;
    public GameManager gameManager;

    private void Start()
    {
        if (hamburgerRecipe == null)
            hamburgerRecipe = FindObjectOfType<HamburgerRecipe>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

     
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {other.name}");

        // StackHamburger 컴포넌트를 일관되게 찾기
        StackHamburger hamburger = other.GetComponentInParent<StackHamburger>();
        if (hamburger != null)
        {
            Debug.Log($"햄버거 발견: {hamburger.name}, 완성 상태: {hamburger.isComplete}");
            if (!hamburgersInZone.Contains(hamburger))
            {
                hamburgersInZone.Add(hamburger);
                Debug.Log($"햄버거 등록됨: {hamburger.name}");
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

        
    }

    private void OnTriggerExit(Collider other)
    {
        // Enter와 동일한 방식으로 햄버거 찾기
        StackHamburger hamburger = other.GetComponentInParent<StackHamburger>();
        if (hamburger != null && hamburgersInZone.Contains(hamburger))
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
        //CleanupDestroyedObjects();

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

                // 리스트에서 제거 (리셋 전에)
                hamburgersInZone.Remove(burger);

                // 리셋
                burger.ResetHamburger();
                hamburgersInZone.Add(burger);

                if (recipeMatch)
                {
                    
                      gameManager.AddScore(1000);
                      Debug.Log("기본 1000점!");
                    
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

                // 주문 단계에 따라 감자튀김/콜라 필요 여부 결정
                int orderNum = hamburgerRecipe.orderCount;
                bool needsFries = orderNum >= 5;
                bool needsCola = orderNum >= 7;

                bool friesOK = !needsFries || friesInZone.Count > 0;
                bool colaOK = !needsCola || colasInZone.Count > 0;

                Debug.Log($"세트 체크 - 햄버거: {burgerCorrect}, 감자튀김: {friesOK} (필요함: {needsFries}), 콜라: {colaOK} (필요함: {needsCola})");

                // 리스트에서 제거
                hamburgersInZone.Remove(burger);
                burger.ResetHamburger();
                hamburgersInZone.Add(burger);
                // 감자튀김 처리
                if (friesInZone.Count > 0)
                {
                    GameObject fries = friesInZone[0];
                    friesInZone.RemoveAt(0);
                    Destroy(fries);
                }

                // 콜라 처리
                if (colasInZone.Count > 0)
                {
                    GameObject cola = colasInZone[0];
                    colasInZone.RemoveAt(0);
                    Destroy(cola);
                }

                // 최종 판정
                if (burgerCorrect && friesOK && colaOK)
                {
                    Debug.Log("정확한 세트 주문 처리 완료! +1700점");
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

       
    }




}