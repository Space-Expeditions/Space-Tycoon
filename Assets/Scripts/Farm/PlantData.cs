using UnityEngine;

public class PlantData : MonoBehaviour
{
    public string plantName;
    public float minTemperature;
    public float maxTemperature;
    public float minHumidity;
    public float maxHumidity;
    public string soilTag;


    public string requiredSoilTag; // ✅ 식물이 원하는 땅의 태그를 직접 설정

    public SpriteRenderer soilRenderer;
    public PlantGrowthStage[] growthStages;
    public Sprite deadSprite;

    private void Reset()
    {
        string objName = gameObject.name.ToLower();

        if (objName.Contains("carrot"))
        {
            plantName = "당근";
            minTemperature = 10f;
            maxTemperature = 30f;
            minHumidity = 40f;
            maxHumidity = 70f;
            growthStages = CreateGrowthStages(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, new float[] { 5f, 5f, 10f, 20f, 30f, 40f, 50f, 60f, 70f, 80f });
        }
        else if (objName.Contains("tomato"))
        {
            plantName = "토마토";
            minTemperature = 15f;
            maxTemperature = 30f;
            minHumidity = 50f;
            maxHumidity = 80f;
            growthStages = CreateGrowthStages(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, new float[] { 5f, 5f, 10f, 20f, 30f, 40f, 50f, 60f, 70f, 80f });
        }
        else if (objName.Contains("potato"))
        {
            plantName = "감자";
            minTemperature = 12f;
            maxTemperature = 28f;
            minHumidity = 45f;
            maxHumidity = 75f;
            growthStages = CreateGrowthStages(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, new float[] { 5f, 5f, 10f, 20f, 30f, 40f, 50f, 60f, 70f, 80f });
        }
        else
        {
            plantName = "식물";
            minTemperature = 10f;
            maxTemperature = 30f;
            minHumidity = 40f;
            maxHumidity = 70f;
            growthStages = CreateGrowthStages(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, new float[] { 5f, 5f, 10f, 20f, 30f, 40f, 50f, 60f, 70f, 80f });
        }
    }

    private PlantGrowthStage[] CreateGrowthStages(string[] names, float[] durations)
    {
        PlantGrowthStage[] stages = new PlantGrowthStage[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            GameObject stageObj = new GameObject(names[i]);
            stageObj.transform.parent = this.transform;
            PlantGrowthStage stage = stageObj.AddComponent<PlantGrowthStage>();
            stage.stageName = names[i];
            stage.stageDuration = durations[i];
            stages[i] = stage;
        }

        return stages;
    }
}