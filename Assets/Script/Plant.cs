using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant : MonoBehaviour
{
    public PlantData plantData;
    public bool isEnvironmentSensitive = false;

    private float growthTimer = 0f;
    private bool isGrowing = true;
    private int currentStageIndex = 0;

    private float environmentCheckTimer = 0f;
    private float environmentCheckInterval = 2f;
    private int badEnvironmentCounter = 0;
    private int maxBadEnvironmentCount = 1;

    private SpriteRenderer spriteRenderer;

    public bool isFullyGrown = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (plantData != null && plantData.growthStages.Length > 0)
        {
            UpdateStageVisual(plantData.growthStages[0].stageSprite);
        }

        // 시작 시 이미 6단계면 성장 완료로 처리
        if (currentStageIndex == 6)
        {
            isFullyGrown = true;
        }
    }

    private void Update()
    {
        if (!isGrowing) return;

        if (!isEnvironmentSensitive)
        {
            growthTimer += Time.deltaTime;
            if (CanAdvanceStage())
            {
                AdvanceGrowthStage();
            }
            return;
        }

        environmentCheckTimer += Time.deltaTime;
        bool conditionsOk = CheckEnvironmentConditions();

        if (environmentCheckTimer >= environmentCheckInterval)
        {
            environmentCheckTimer = 0f;

            if (!conditionsOk)
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

        if (conditionsOk)
        {
            growthTimer += Time.deltaTime;

            if (CanAdvanceStage())
            {
                AdvanceGrowthStage();
            }
        }
        else
        {
            growthTimer = 0f;
        }
    }

    private bool CanAdvanceStage()
    {
        return currentStageIndex < plantData.growthStages.Length &&
               growthTimer >= plantData.growthStages[currentStageIndex].stageDuration;
    }

    private void AdvanceGrowthStage()
    {
        currentStageIndex++;
        growthTimer = 0f;
        badEnvironmentCounter = 0;

        // ✅ 6단계 이상이면 성장 완료로 처리
        if (currentStageIndex == 6)
        {
            isFullyGrown = true;
            Debug.Log($"{plantData.plantName} has fully grown!");
        }
        else
        {
            isFullyGrown = false;
        }

        // 단계가 배열 범위 내에 있으면 시각 갱신
        if (currentStageIndex < plantData.growthStages.Length)
        {
            var stage = plantData.growthStages[currentStageIndex];
            if (stage != null)
            {
                UpdateStageVisual(stage.stageSprite);
            }
        }
    }

    private void UpdateStageVisual(Sprite newSprite)
    {
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    private bool CheckEnvironmentConditions()
    {
        if (plantData == null || plantData.soilRenderer == null)
        {
            Debug.LogWarning("CheckEnvironmentConditions 실패: plantData 또는 soilRenderer가 null입니다.");
            return false;
        }

        string requiredTag = plantData.requiredSoilTag;
        string actualTag = plantData.soilRenderer.tag;
        bool soilMatch = actualTag == requiredTag;

        bool tempMatch = EnvironmentManager.currentTemperature >= plantData.minTemperature &&
                         EnvironmentManager.currentTemperature <= plantData.maxTemperature;

        bool humidityMatch = EnvironmentManager.currentHumidity >= plantData.minHumidity &&
                             EnvironmentManager.currentHumidity <= plantData.maxHumidity;

        if (!soilMatch) Debug.Log($"❌ 땅 태그 불일치: 필요={requiredTag}, 현재={actualTag}");
        if (!tempMatch) Debug.Log($"❌ 온도 불일치: {EnvironmentManager.currentTemperature}도 (요구 {plantData.minTemperature}~{plantData.maxTemperature})");
        if (!humidityMatch) Debug.Log($"❌ 습도 불일치: {EnvironmentManager.currentHumidity}% (요구 {plantData.minHumidity}~{plantData.maxHumidity})");

        return soilMatch && tempMatch && humidityMatch;
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
