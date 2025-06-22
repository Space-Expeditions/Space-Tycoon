using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; }

    public event Action<int> OnGoldChanged;

    const string GoldKey = "PlayerGold";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGold(); // 시작 시 골드 불러오기
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        SaveGold(); // 변경된 골드 저장
        OnGoldChanged?.Invoke(Gold);
    }

    public bool TrySpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            SaveGold(); // 변경된 골드 저장
            OnGoldChanged?.Invoke(Gold);
            return true;
        }
        return false;
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, Gold);
        PlayerPrefs.Save(); // 저장 강제 적용
    }

    private void LoadGold()
    {
        Gold = PlayerPrefs.GetInt(GoldKey, 0); // 기본값은 0
    }
}
