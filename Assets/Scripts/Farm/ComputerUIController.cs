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
    public static ComputerUIController currentComputer;

    [Header("UI Panels")]
    public GameObject cropPanel;
    public GameObject envPanel;
    public GameObject groundPanel;

    [Header("Input Fields")]
    public TMP_InputField humidityInput;
    public TMP_InputField temperatureInput;

    [Header("Spawn Settings")]
    public GameObject[] groundPrefabs;
    public List<CropVariant> cropVariants;
    public GameObject greenhousePrefab;

    [Header("Seed Icon Manager")]
    public SeedIconManager seedIconManager;

    private CropType selectedCrop;
    private GroundType selectedGround;
    private float humidity;
    private float temperature;

    private Vector3 spawnOrigin;
    private Dictionary<Vector3, GameObject> spawnedGrounds = new();

    void Start()
    {
        Transform canvas = GameObject.Find("FarmingCanvas")?.transform;

        cropPanel = canvas.Find("ComputerUI/CropPanel")?.gameObject;
        envPanel = canvas.Find("ComputerUI/EnvPanel")?.gameObject;
        groundPanel = canvas.Find("ComputerUI/GroundPanel")?.gameObject;

        humidityInput = canvas.Find("ComputerUI/EnvPanel/Humidity")?.GetComponent<TMP_InputField>();
        temperatureInput = canvas.Find("ComputerUI/EnvPanel/Temperature")?.GetComponent<TMP_InputField>();

        cropPanel?.SetActive(false);
        envPanel?.SetActive(false);
        groundPanel?.SetActive(false);
    }

    void OnMouseDown()
    {
        if (cropPanel.activeSelf || envPanel.activeSelf || groundPanel.activeSelf)
            return;

        currentComputer = this;
        currentComputer.spawnOrigin = transform.position;

        if (currentComputer.spawnedGrounds.TryGetValue(currentComputer.spawnOrigin, out GameObject existingGround))
        {
            Plant plant = existingGround.GetComponentInChildren<Plant>();
            if (plant != null && plant.isFullyGrown)
            {
                currentComputer.HarvestPlant(plant, currentComputer.spawnOrigin);
                Destroy(existingGround);
                currentComputer.spawnedGrounds.Remove(currentComputer.spawnOrigin);
                return;
            }

            Destroy(existingGround);
            currentComputer.spawnedGrounds.Remove(currentComputer.spawnOrigin);
        }

        if (seedIconManager != null && InventoryManager.instance != null)
        {
            seedIconManager.inventory = InventoryManager.instance.inventoryContainer;
            seedIconManager.UpdateSeedIcons();
        }

        cropPanel?.SetActive(true);
    }

    void HarvestPlant(Plant plant, Vector3 basePos)
    {
        basePos += new Vector3(0, 1f, 0.5f);

        if (plant.plantData != null && plant.plantData.plantName == "토감")
        {
            Item tomato = ItemDatabase.instance.GetItemByName("Tomato");
            Item potato = ItemDatabase.instance.GetItemByName("Potato");

            for (int i = 0; i < Random.Range(1, 4); i++)
                ItemSpawnManager.instance.SpawnItem(basePos + RandomOffset(), tomato, 1);

            for (int i = 0; i < Random.Range(1, 4); i++)
                ItemSpawnManager.instance.SpawnItem(basePos + RandomOffset(), potato, 1);
        }
        else
        {
            string itemName = selectedCrop.ToString();
            int count = selectedCrop == CropType.Carrot ? 1 : Random.Range(1, 4);
            Item harvestedItem = ItemDatabase.instance.GetItemByName(itemName);

            if (harvestedItem != null)
            {
                for (int i = 0; i < count; i++)
                    ItemSpawnManager.instance.SpawnItem(basePos + RandomOffset(), harvestedItem, 1);
            }
            else
            {
                Debug.LogError($"ItemDatabase에서 {itemName} 아이템을 찾을 수 없습니다.");
            }
        }
    }

    Vector3 RandomOffset() => new(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));

    public static void StaticSelectCrop(int index)
    {
        currentComputer?.SelectCrop(index);
    }

    void SelectCrop(int index)
    {
        selectedCrop = (CropType)index;
        cropPanel?.SetActive(false);
        envPanel?.SetActive(true);
    }

    public static void StaticOnConfirmButtonClick()
    {
        currentComputer?.OnConfirmButtonClick();
    }

    void OnConfirmButtonClick()
    {
        string humidityText = humidityInput?.text ?? "null";
        string temperatureText = temperatureInput?.text ?? "null";
        ConfirmEnvironment(humidityText, temperatureText);
    }

    void ConfirmEnvironment(string humidityText, string temperatureText)
    {
        if (float.TryParse(humidityText, out humidity) && float.TryParse(temperatureText, out temperature))
        {
            EnvironmentManager.currentHumidity = humidity;
            EnvironmentManager.currentTemperature = temperature;

            envPanel?.SetActive(false);
            groundPanel?.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid input for humidity or temperature.");
        }
    }

    public static void StaticSelectGround(int index)
    {
        currentComputer?.SelectGround(index);
    }

    void SelectGround(int index)
    {
        selectedGround = (GroundType)index;
        groundPanel?.SetActive(false);
        PlaceFieldAndCrop();
    }

    void PlaceFieldAndCrop()
    {
        string seedName = selectedCrop + " Seed";
        if (!InventoryManager.instance.inventoryContainer.HasSeed(seedName))
        {
            Debug.LogWarning($"❌ {seedName} 씨앗이 없습니다. 작물을 생성할 수 없습니다.");
            return;
        }

        Vector3 groundPos = currentComputer.spawnOrigin + new Vector3(0, 1f, 1f);
        GameObject newGround = Instantiate(currentComputer.groundPrefabs[(int)currentComputer.selectedGround], groundPos, Quaternion.identity);
        currentComputer.spawnedGrounds[currentComputer.spawnOrigin] = newGround;

        if (currentComputer.greenhousePrefab != null)
        {
            GameObject greenhouse = Instantiate(currentComputer.greenhousePrefab, new Vector3(groundPos.x, groundPos.y, 0.5f), Quaternion.identity);
            greenhouse.transform.SetParent(newGround.transform);

            foreach (var sr in greenhouse.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.sortingLayerName = "Default";
                sr.sortingOrder = 10;
            }
        }

        var cropPrefab = currentComputer.cropVariants.FirstOrDefault(cv => cv.cropType == currentComputer.selectedCrop && cv.groundType == currentComputer.selectedGround)?.prefab;

        if (cropPrefab != null)
        {
            GameObject crop = Instantiate(cropPrefab, new Vector3(groundPos.x, groundPos.y + 0.04f, 0.9f), Quaternion.identity);
            crop.transform.SetParent(newGround.transform);

            Plant plant = crop.GetComponent<Plant>();
            if (plant != null)
            {
                plant.isEnvironmentSensitive = true;
            }

            InventoryManager.instance.inventoryContainer.ConsumeSeed(seedName, 1);
            currentComputer.seedIconManager?.UpdateSeedIcons();
        }
        else
        {
            Debug.LogError($"Crop prefab not found for {currentComputer.selectedCrop} + {currentComputer.selectedGround}");
        }
    }
}
