using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class ComputerUIController : MonoBehaviour
{
    public GameObject carrotIconPrefab;
    public GameObject tomatoIconPrefab;
    public GameObject potatoIconPrefab;

    public Transform iconParent;
    public GameObject uiPanel;

    public Plant carrotPlant, tomatoPlant, potatoPlant;
    public Plant targetPlant;

    public InputField temperatureInput;
    public InputField humidityInput;
    public InputField soilTagInput;

    private List<GameObject> currentIcons = new List<GameObject>();

    private void OnMouseDown()
    {
        ShowCropIcons();
    }

    void ShowCropIcons()
    {
        ClearCropIcons();

        Vector3 startPos = new Vector3(-110f, 0f, 0f);
        float spacing = 110f;

        GameObject[] icons = {
            Instantiate(carrotIconPrefab, iconParent),
            Instantiate(tomatoIconPrefab, iconParent),
            Instantiate(potatoIconPrefab, iconParent)
        };

        for (int i = 0; i < icons.Length; i++)
        {
            RectTransform rt = icons[i].GetComponent<RectTransform>();
            rt.anchoredPosition = startPos + new Vector3(i * spacing, 0f, 0f);

            CropIconButton iconBtn = icons[i].GetComponent<CropIconButton>();
            iconBtn.controller = this;

            currentIcons.Add(icons[i]);
        }
    }

    public void SelectCrop(string cropName)
    {
        switch (cropName)
        {
            case "Carrot": targetPlant = carrotPlant; break;
            case "Tomato": targetPlant = tomatoPlant; break;
            case "Potato": targetPlant = potatoPlant; break;
        }

        ClearCropIcons();
        uiPanel.SetActive(true);
    }

    public void ApplySettings()
    {
        if (targetPlant == null) return;

        if (float.TryParse(temperatureInput.text, out float temp))
            targetPlant.currentTemperature = temp;

        if (float.TryParse(humidityInput.text, out float hum))
            targetPlant.currentHumidity = hum;

        targetPlant.plantData.soilTag = soilTagInput.text;

        Debug.Log($"적용됨: 온도 {temp}, 습도 {hum}, 태그 {targetPlant.plantData.soilTag}");
    }

    void ClearCropIcons()
    {
        foreach (GameObject icon in currentIcons)
        {
            Destroy(icon);
        }
        currentIcons.Clear();
    }
}
