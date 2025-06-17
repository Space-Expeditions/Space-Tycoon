using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public enum CropType
{
    Potato,
    Carrot,
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

    private CropType selectedCrop;
    private GroundType selectedGround;
    private float humidity;
    private float temperature;

    private GameObject lastSpawnedGround;

    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        cropPanel.SetActive(false);
        envPanel.SetActive(false);
        groundPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist < 0.5f)
            {
                InteractiveComputer();
            }
            else
            {
                Debug.Log("Far to computer");
            }
        }
    }

    void OnMouseDown()
    {
        InteractiveComputer();
    }

    void InteractiveComputer()
    {
        // ✅ UI 중복 클릭 방지
        if (cropPanel.activeSelf || envPanel.activeSelf || groundPanel.activeSelf)
            return;

        // ✅ 수확 처리
        Plant plant = lastSpawnedGround != null
            ? lastSpawnedGround.GetComponentInChildren<Plant>()
            : null;

        if (plant != null && plant.isFullyGrown)
        {
            Vector3 basePos = lastSpawnedGround.transform.position + new Vector3(0, 0.3f, 0.5f);
            int count = 1;
            Sprite spriteToUse = null;

            switch (selectedCrop)
            {
                case CropType.Carrot:
                    spriteToUse = harvestedCarrotSprite;
                    count = 1;
                    break;
                case CropType.Tomato:
                    spriteToUse = harvestedTomatoSprite;
                    count = Random.Range(1, 4);
                    break;
                case CropType.Potato:
                    spriteToUse = harvestedPotatoSprite;
                    count = Random.Range(1, 4);
                    break;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));
                GameObject drop = new GameObject("HarvestedItem");
                drop.transform.position = basePos + offset;

                SpriteRenderer sr = drop.AddComponent<SpriteRenderer>();
                sr.sprite = spriteToUse;
                sr.sortingOrder = 20;
            }

            Destroy(lastSpawnedGround);
            lastSpawnedGround = null;
            return;
        }

        // ✅ 땅 제거
        if (lastSpawnedGround != null)
        {
            Destroy(lastSpawnedGround);
            lastSpawnedGround = null;
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
        if (float.TryParse(humidityText, out humidity) &&
            float.TryParse(temperatureText, out temperature))
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
        // ✅ 땅 중복 방지
        if (lastSpawnedGround != null)
        {
            Destroy(lastSpawnedGround);
            lastSpawnedGround = null;
        }

        Vector3 groundPos = spawnPoint.position + new Vector3(0, 0.5f, 1f);
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
        }
        else
        {
            Debug.LogError($"Crop prefab not found for {selectedCrop} + {selectedGround}");
        }
    }
}
