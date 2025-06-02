using UnityEngine;

public class IngredientDispenser : MonoBehaviour
{
    public GameObject ingredientPrefab;   // 생성할 재료 프리팹
    public Transform spawnPoint;          // 재료 생성 위치
    public float spawnCooldown = 2f;      // 재료 생성 간격(초)

    public Transform controllerTransform; // 레이 쏘는 컨트롤러 위치(예: VR 오른손)

    private float lastSpawnTime = 0f;

    void Update()
    {
        // 컨트롤러 위치와 방향으로 레이 발사
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
        RaycastHit hit;

        // 레이가 재료통(자기 자신)을 향하고 있는지 확인
        if (Physics.Raycast(ray, out hit, 5f)) // 최대 거리 5m, 필요에 따라 조절
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                // 쿨타임 확인 후 재료 생성
                if (Time.time - lastSpawnTime > spawnCooldown)
                {
                    SpawnIngredient();
                    lastSpawnTime = Time.time;
                }
            }
        }
    }

    void SpawnIngredient()
    {
        Instantiate(ingredientPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Ingredient spawned!");
    }
} 
