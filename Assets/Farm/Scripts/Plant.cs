using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant : MonoBehaviour
{
    public PlantData plantData;

    private float growthTimer = 0f;
    private bool isGrowing = true;
    private int currentStageIndex = 0;

    private float environmentCheckTimer = 0f;
    private float environmentCheckInterval = 2f;
    private int badEnvironmentCounter = 0;
    private int maxBadEnvironmentCount = 1;

    private SpriteRenderer spriteRenderer;

    public float currentTemperature = 22f;
    public float currentHumidity = 60f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (plantData != null && plantData.growthStages.Length > 0)
        {
            UpdateStageVisual(plantData.growthStages[0].stageSprite);
        }
    }

    private void Update()
    {
        if (!isGrowing) return;

        environmentCheckTimer += Time.deltaTime;

        if (environmentCheckTimer >= environmentCheckInterval)
        {
            environmentCheckTimer = 0f;

            if (!CheckEnvironmentConditions())
            {
                badEnvironmentCounter++;
                if (badEnvironmentCounter >= maxBadEnvironmentCount)
                {
                    Die();
                    return;
                }
            }
            else
            {
                badEnvironmentCounter = 0;
            }
        }

        if (CheckEnvironmentConditions())
        {
            growthTimer += Time.deltaTime;

            if (currentStageIndex < plantData.growthStages.Length)
            {
                if (growthTimer >= plantData.growthStages[currentStageIndex].stageDuration)
                {
                    AdvanceGrowthStage();
                }
            }
        }
    }

    private bool CheckEnvironmentConditions()
    {
        if (plantData == null || plantData.soilRenderer == null)
        {
            Debug.LogWarning("CheckEnvironmentConditions 실패: plantData 또는 soilRenderer가 null입니다.");
            return false;
        }

        // ✅ 식물이 원하는 땅 태그와 실제 땅 태그 비교
        string requiredTag = plantData.requiredSoilTag;
        string actualTag = plantData.soilRenderer.tag;
        bool soilMatch = actualTag == requiredTag;

        bool tempMatch = currentTemperature >= plantData.minTemperature &&
                         currentTemperature <= plantData.maxTemperature;

        bool humidityMatch = currentHumidity >= plantData.minHumidity &&
                             currentHumidity <= plantData.maxHumidity;

        Debug.Log($"[{plantData.plantName}] → 온도 OK: {tempMatch}, 습도 OK: {humidityMatch}, 땅 OK: {soilMatch}");

        return soilMatch && tempMatch && humidityMatch;
    }

    private void AdvanceGrowthStage()
    {
        currentStageIndex++;
        badEnvironmentCounter = 0;

        if (currentStageIndex >= plantData.growthStages.Length)
        {
            isGrowing = false;
            Debug.Log(plantData.plantName + " has fully grown!");
            return;
        }

        var stage = plantData.growthStages[currentStageIndex];
        if (stage != null)
        {
            UpdateStageVisual(stage.stageSprite);
        }
    }

    private void UpdateStageVisual(Sprite newSprite)
    {
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    private void Die()
    {
        isGrowing = false;
        Debug.Log(plantData.plantName + " has died due to poor environment!");

        if (plantData.deadSprite != null)
        {
            spriteRenderer.sprite = plantData.deadSprite;
        }
    }
}
