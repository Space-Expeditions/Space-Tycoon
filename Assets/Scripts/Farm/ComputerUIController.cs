using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public enum CropType { Potato, Carrot, Tomapo, Tomato }
public enum GroundType { Purple, Brown }

[System.Serializable]
public class CropVariant
{
    public CropType cropType;
    public GroundType groundType;
    public GameObject prefab;
}

public class ComputerUIController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject cropPanel;
    public GameObject envPanel;
    public GameObject groundPanel;

    [Header("Input Fields")]
    public TMP_InputField humidityInput;
    public TMP_InputField temperatureInput;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public GameObject[] groundPrefabs;
    public List<CropVariant> cropVariants;

    [Header("Greenhouse")]
    public GameObject greenhousePrefab;

    [Header("Harvest Sprites")]
    public Sprite harvestedCarrotSprite;
    public Sprite harvestedTomatoSprite;
    public Sprite harvestedPotatoSprite;

    [Header("Seed Icon Manager")]
    public SeedIconManager seedIconManager;

    private CropType selectedCrop;
    private GroundType selectedGround;
    private float humidity;
    private float temperature;

    private GameObject lastSpawnedGround;

    void Start()
    {
        cropPanel.SetActive(false);
        envPanel.SetActive(false);
        groundPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        if (cropPanel.activeSelf || envPanel.activeSelf || groundPanel.activeSelf)
            return;

        Plant plant = lastSpawnedGround != null ? lastSpawnedGround.GetComponentInChildren<Plant>() : null;

        if (plant != null && plant.isFullyGrown)
        {
            Vector3 basePos = lastSpawnedGround.transform.position + new Vector3(0, 0.3f, 0.5f);

            if (plant.plantData != null && plant.plantData.plantName == "토감")
            {
                Item tomato = ItemDatabase.instance.GetItemByName("Tomato");
                Item potato = ItemDatabase.instance.GetItemByName("Potato");

                int tomatoCount = Random.Range(1, 4);
                int potatoCount = Random.Range(1, 4);

                for (int i = 0; i < tomatoCount; i++)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));
                    Vector3 spawnPos = basePos + offset;
                    ItemSpawnManager.instance.SpawnItem(spawnPos, tomato, 1);
                }

                for (int i = 0; i < potatoCount; i++)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));
                    Vector3 spawnPos = basePos + offset;
                    ItemSpawnManager.instance.SpawnItem(spawnPos, potato, 1);
                }
            }
            else
            {
                int count = 1;
                string itemName = null;

                switch (selectedCrop)
                {
                    case CropType.Carrot:
                        itemName = "Carrot";
                        count = 1;
                        break;
                    case CropType.Tomato:
                        itemName = "Tomato";
                        count = Random.Range(1, 4);
                        break;
                    case CropType.Potato:
                        itemName = "Potato";
                        count = Random.Range(1, 4);
                        break;
                }

                if (itemName != null)
                {
                    Item harvestedItem = ItemDatabase.instance.GetItemByName(itemName);

                    if (harvestedItem != null)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));
                            Vector3 spawnPos = basePos + offset;
                            ItemSpawnManager.instance.SpawnItem(spawnPos, harvestedItem, 1);
                        }
                    }
                    else
                    {
                        Debug.LogError($"ItemDatabase에서 {itemName} 아이템을 찾을 수 없습니다.");
                    }
                }
            }

            Destroy(lastSpawnedGround);
            lastSpawnedGround = null;
            return;
        }

        if (lastSpawnedGround != null)
        {
            Destroy(lastSpawnedGround);
            lastSpawnedGround = null;
        }

        if (seedIconManager != null && InventoryManager.instance != null)
        {
            seedIconManager.inventory = InventoryManager.instance.inventoryContainer;
            seedIconManager.UpdateSeedIcons();
        }

        cropPanel.SetActive(true);
    }

    public void SelectCrop(int index)
    {
        selectedCrop = (CropType)index;
        cropPanel.SetActive(false);
        envPanel.SetActive(true);
    }

    public void OnConfirmButtonClick()
    {
        string humidityText = humidityInput != null ? humidityInput.text : "null";
        string temperatureText = temperatureInput != null ? temperatureInput.text : "null";

        Debug.Log($"습도: {humidityText}, 온도: {temperatureText}");
        ConfirmEnvironment(humidityText, temperatureText);
    }

    public void ConfirmEnvironment(string humidityText, string temperatureText)
    {
        if (float.TryParse(humidityText, out humidity) && float.TryParse(temperatureText, out temperature))
        {
            EnvironmentManager.currentHumidity = humidity;
            EnvironmentManager.currentTemperature = temperature;

            envPanel.SetActive(false);
            groundPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid input for humidity or temperature.");
        }
    }

    public void SelectGround(int index)
    {
        selectedGround = (GroundType)index;
        groundPanel.SetActive(false);
        PlaceFieldAndCrop();
    }

    void PlaceFieldAndCrop()
    {
        if (lastSpawnedGround != null)
        {
            Destroy(lastSpawnedGround);
            lastSpawnedGround = null;
        }

        string seedName = selectedCrop.ToString() + " Seed";
        if (!InventoryManager.instance.inventoryContainer.HasSeed(seedName))
        {
            Debug.LogWarning($"❌ {seedName} 씨앗이 없습니다. 작물을 생성할 수 없습니다.");
            return;
        }

        Vector3 groundPos = spawnPoint.position + new Vector3(0, 1f, 1f);
        lastSpawnedGround = Instantiate(groundPrefabs[(int)selectedGround], groundPos, Quaternion.identity);

        if (greenhousePrefab != null)
        {
            Vector3 greenhousePos = new Vector3(groundPos.x, groundPos.y, 0.5f);
            GameObject greenhouse = Instantiate(greenhousePrefab, greenhousePos, Quaternion.identity);
            greenhouse.transform.SetParent(lastSpawnedGround.transform);

            SpriteRenderer[] srs = greenhouse.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srs)
            {
                sr.sortingLayerName = "Default";
                sr.sortingOrder = 10;
            }
        }

        var cropPrefab = cropVariants
            .FirstOrDefault(cv => cv.cropType == selectedCrop && cv.groundType == selectedGround)
            ?.prefab;

        if (cropPrefab != null)
        {
            GameObject crop = Instantiate(
                cropPrefab,
                new Vector3(groundPos.x, groundPos.y + 0.04f, 0.9f),
                Quaternion.identity
            );

            crop.transform.SetParent(lastSpawnedGround.transform);

            Plant plant = crop.GetComponent<Plant>();
            if (plant != null)
            {
                plant.isEnvironmentSensitive = true;
            }

            InventoryManager.instance.inventoryContainer.ConsumeSeed(seedName, 1);
            seedIconManager?.UpdateSeedIcons();
        }
        else
        {
            Debug.LogError($"Crop prefab not found for {selectedCrop} + {selectedGround}");
        }
    }
}