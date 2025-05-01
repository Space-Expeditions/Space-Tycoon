using UnityEngine;

public class CropIconButton : MonoBehaviour
{
    public string cropName;
    public ComputerUIController controller;

    public void OnClick()
    {
        controller.SelectCrop(cropName);
    }
}
