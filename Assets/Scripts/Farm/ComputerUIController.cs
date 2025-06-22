using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public enum CropType
{
    Potato,
    Carrot,
    Tomapo,
    Tomato
}

public enum GroundType
{
    Purple,
    Brown
}

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
        // if (cropPanel.activeSelf || envPanel.activeSelf || groundPanel.activeSelf)
        // {
        //     return;
        // }

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

    //[Header("UI Panels")]
    //public GameObject cropPanel;
    //public GameObject envPanel;
    //public GameObject groundPanel;

    //[Header("Input Fields")]
    //public TMP_InputField humidityInput;
    //public TMP_InputField temperatureInput;

    //[Header("Spawn Settings")]
    //public Transform spawnPoint;
    //public GameObject[] groundPrefabs;
    //public List<CropVariant> cropVariants;

    //[Header("Greenhouse")]
    //public GameObject greenhousePrefab;

    //[Header("Harvest Sprites")]
    //public Sprite harvestedCarrotSprite;
    //public Sprite harvestedTomatoSprite;
    //public Sprite harvestedPotatoSprite;

    //private CropType selectedCrop;
    //private GroundType selectedGround;
    //private float humidity;
    //private float temperature;

    //private GameObject lastSpawnedGround;

    //GameObject player;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player");

    //    cropPanel.SetActive(false);
    //    envPanel.SetActive(false);
    //    groundPanel.SetActive(false);
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        float dist = Vector2.Distance(player.transform.position, transform.position);
    //        if (dist < 0.5f)
    //        {
    //            InteractiveComputer();
    //        }
    //        else
    //        {
    //            Debug.Log("Far to computer");
    //        }
    //    }
    //}

    //void OnMouseDown()
    //{
    //    InteractiveComputer();
    //}

    //void InteractiveComputer()
    //{
    //    // ✅ UI 중복 클릭 방지
    //    if (cropPanel.activeSelf || envPanel.activeSelf || groundPanel.activeSelf)
    //        return;

    //    // ✅ 수확 처리
    //    Plant plant = lastSpawnedGround != null
    //        ? lastSpawnedGround.GetComponentInChildren<Plant>()
    //        : null;

    //    if (plant != null && plant.isFullyGrown)
    //    {
    //        Vector3 basePos = lastSpawnedGround.transform.position + new Vector3(0, 0.3f, 0.5f);
    //        int count = 1;
    //        Sprite spriteToUse = null;

    //        switch (selectedCrop)
    //        {
    //            case CropType.Carrot:
    //                spriteToUse = harvestedCarrotSprite;
    //                count = 1;
    //                break;
    //            case CropType.Tomato:
    //                spriteToUse = harvestedTomatoSprite;
    //                count = Random.Range(1, 4);
    //                break;
    //            case CropType.Potato:
    //                spriteToUse = harvestedPotatoSprite;
    //                count = Random.Range(1, 4);
    //                break;
    //        }

    //        for (int i = 0; i < count; i++)
    //        {
    //            Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));
    //            GameObject drop = new GameObject("HarvestedItem");
    //            drop.transform.position = basePos + offset;

    //            SpriteRenderer sr = drop.AddComponent<SpriteRenderer>();
    //            sr.sprite = spriteToUse;
    //            sr.sortingOrder = 20;
    //        }

    //        Destroy(lastSpawnedGround);
    //        lastSpawnedGround = null;
    //        return;
    //    }

    //    // ✅ 땅 제거
    //    if (lastSpawnedGround != null)
    //    {
    //        Destroy(lastSpawnedGround);
    //        lastSpawnedGround = null;
    //    }

    //    cropPanel.SetActive(true);
    //}

    //public void SelectCrop(int index)
    //{
    //    selectedCrop = (CropType)index;
    //    cropPanel.SetActive(false);
    //    envPanel.SetActive(true);
    //}

    //public void OnConfirmButtonClick()
    //{
    //    string humidityText = humidityInput != null ? humidityInput.text : "null";
    //    string temperatureText = temperatureInput != null ? temperatureInput.text : "null";

    //    Debug.Log($"습도: {humidityText}, 온도: {temperatureText}");

    //    ConfirmEnvironment(humidityText, temperatureText);
    //}

    //public void ConfirmEnvironment(string humidityText, string temperatureText)
    //{
    //    if (float.TryParse(humidityText, out humidity) &&
    //        float.TryParse(temperatureText, out temperature))
    //    {
    //        EnvironmentManager.currentHumidity = humidity;
    //        EnvironmentManager.currentTemperature = temperature;

    //        envPanel.SetActive(false);
    //        groundPanel.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Invalid input for humidity or temperature.");
    //    }
    //}

    //public void SelectGround(int index)
    //{
    //    selectedGround = (GroundType)index;
    //    groundPanel.SetActive(false);
    //    PlaceFieldAndCrop();
    //}

    //void PlaceFieldAndCrop()
    //{
    //    // ✅ 땅 중복 방지
    //    if (lastSpawnedGround != null)
    //    {
    //        Destroy(lastSpawnedGround);
    //        lastSpawnedGround = null;
    //    }

    //    Vector3 groundPos = spawnPoint.position + new Vector3(0, 0.5f, 1f);
    //    lastSpawnedGround = Instantiate(groundPrefabs[(int)selectedGround], groundPos, Quaternion.identity);

    //    if (greenhousePrefab != null)
    //    {
    //        Vector3 greenhousePos = new Vector3(groundPos.x, groundPos.y, 0.5f);
    //        GameObject greenhouse = Instantiate(greenhousePrefab, greenhousePos, Quaternion.identity);
    //        greenhouse.transform.SetParent(lastSpawnedGround.transform);

    //        SpriteRenderer[] srs = greenhouse.GetComponentsInChildren<SpriteRenderer>();
    //        foreach (var sr in srs)
    //        {
    //            sr.sortingLayerName = "Default";
    //            sr.sortingOrder = 10;
    //        }
    //    }

    //    var cropPrefab = cropVariants
    //        .FirstOrDefault(cv => cv.cropType == selectedCrop && cv.groundType == selectedGround)
    //        ?.prefab;

    //    if (cropPrefab != null)
    //    {
    //        GameObject crop = Instantiate(
    //            cropPrefab,
    //            new Vector3(groundPos.x, groundPos.y + 0.04f, 0.9f),
    //            Quaternion.identity
    //        );

    //        crop.transform.SetParent(lastSpawnedGround.transform);

    //        Plant plant = crop.GetComponent<Plant>();
    //        if (plant != null)
    //        {
    //            plant.isEnvironmentSensitive = true;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"Crop prefab not found for {selectedCrop} + {selectedGround}");
    //    }
    //}
}