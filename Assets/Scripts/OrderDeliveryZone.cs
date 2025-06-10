using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        StackHamburger hamburger = other.GetComponent<StackHamburger>();
        if (hamburger != null && hamburger.isComplete && !hamburgersInZone.Contains(hamburger))
            hamburgersInZone.Add(hamburger);

        if (other.CompareTag("Fries") && !friesInZone.Contains(other.gameObject))
            friesInZone.Add(other.gameObject);

        if (other.CompareTag("Cola") && !colasInZone.Contains(other.gameObject))
            colasInZone.Add(other.gameObject);

        UpdateStatusText();
    }

    private void OnTriggerExit(Collider other)
    {
        StackHamburger hamburger = other.GetComponent<StackHamburger>();
        if (hamburger != null)
            hamburgersInZone.Remove(hamburger);

        if (other.CompareTag("Fries"))
            friesInZone.Remove(other.gameObject);

        if (other.CompareTag("Cola"))
            colasInZone.Remove(other.gameObject);

        UpdateStatusText();
    }

    public void ManualCheckOrder()
    {
        CheckOrderComplete();
    }

    private void CheckOrderComplete()
    {
        if (hamburgerRecipe == null) return;

        if (!hamburgerRecipe.isSetOrder)
        {
            if (hamburgersInZone.Count > 0)
            {
                CompleteHamburgerOrder();
            }
        }
        else
        {
            if (hamburgersInZone.Count > 0 && friesInZone.Count > 0 && colasInZone.Count > 0)
            {
                CompleteSetOrder();
            }
        }
    }

    private void CompleteHamburgerOrder()
    {
        if (hamburgersInZone.Count > 0)
        {
            Destroy(hamburgersInZone[0].gameObject);
            hamburgersInZone.RemoveAt(0);

            if (colasInZone.Count > 0 && hamburgerRecipe.isSetOrder)
            {
                Destroy(colasInZone[0]);
                colasInZone.RemoveAt(0);
                gameManager.AddScore(1500);
            }
            else
            {
                gameManager.AddScore(1000);
            }

            gameManager.OnHamburgerCompleted();
        }

        UpdateStatusText();
    }

    private void CompleteSetOrder()
    {
        if (hamburgersInZone.Count > 0)
        {
            Destroy(hamburgersInZone[0].gameObject);
            hamburgersInZone.RemoveAt(0);
        }
        if (friesInZone.Count > 0)
        {
            Destroy(friesInZone[0]);
            friesInZone.RemoveAt(0);
        }
        if (colasInZone.Count > 0)
        {
            Destroy(colasInZone[0]);
            colasInZone.RemoveAt(0);
        }

        gameManager.AddScore(1700);
        gameManager.OnHamburgerCompleted();
        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        if (statusText == null) return;

        if (!hamburgerRecipe.isSetOrder)
            statusText.text = $"ÇÜ¹ö°Å ÇÊ¿ä: {hamburgersInZone.Count}/1";
        else
            statusText.text = $"¼¼Æ® ÁÖ¹®\nÇÜ¹ö°Å: {hamburgersInZone.Count}/1\n°¨ÀÚÆ¢±è: {friesInZone.Count}/1\nÄÝ¶ó: {colasInZone.Count}/1";
    }
}
