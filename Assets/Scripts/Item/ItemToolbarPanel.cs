using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    int currentSelectedTool;

    private void Start()
    {
        Init();
        toolbarController.onChange += Highlight;
        Highlight(0);
    }

    public override void Onclick(int id)
    {
        toolbarController.Set(id);
        Highlight(id);
    }

    public void Highlight(int id)
    {
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }

    // ✅ 매 프레임마다 인벤토리 UI 갱신
    private void Update()
    {
        Show();
    }
}