using UnityEngine;

public class EquipmentPlacer : MonoBehaviour
{
    public ItemToolbarPanel toolbarPanel; // 툴바 연결 (인스펙터에서 설정)

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Item selectedItem = toolbarPanel.GetSelectedItem();
            if (selectedItem == null) return;

            if (selectedItem.itemType == ItemType.Equipment && selectedItem.prefab != null)
            {
                Vector3 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;

                Instantiate(selectedItem.prefab, worldPos, Quaternion.identity);

                toolbarPanel.inventory.RemoveItem(selectedItem.Name, 1);
                Debug.Log($"🛠 {selectedItem.Name} 설치 완료");
            }
        }
    }
}
