using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :
    Singleton<PlayerManager>,
    ISaveable<PlayerSaveData>
{
    private PlayerController player;
    private CharacterStats stats;
    private PlayerSaveData data;

    public PlayerController Player => player; 
    public CharacterStats PlayerStats => stats;
    public void RegisterPlayer(PlayerController p)
    {
        player = p;
        stats = p.GetComponent<CharacterStats>();

        if (data != null)
            Load();
    }

    public PlayerSaveData GetSaveData()
    {
        PlayerSaveData data = new PlayerSaveData();

        data.position = player.transform.position;

        data.currentHealth = stats.CurrentHealth;
        data.currentMana = stats.CurrentMana;

        var runtime = stats.RuntimeData;

        data.profession = runtime.profession;

        data.baseStats = runtime.baseStats;
        data.bonusStats = runtime.bonusStats;

        data.enhancements =
            runtime.enhancementData.GetAll();

        return data;
    }

    public void LoadSaveData(PlayerSaveData data)
    {
        this.data = data;
        
    }
    private void Load()
    {
        if (player == null)
        {
            return;
        }

        player.transform.position = data.position;

        var runtime = stats.RuntimeData;

        runtime.profession = data.profession;

        runtime.baseStats = data.baseStats;
        runtime.bonusStats = data.bonusStats;

        stats.ResetBonusStats();// TODO:÷ÿ÷√◊¥Ã¨


        runtime.enhancementData.SetAll(data.enhancements);

        EnhancementManager.Instance.RebuildEnhancements(data.enhancements);

        runtime.Recalculate();

        stats.SetHealth(data.currentHealth);
        stats.SetMana(data.currentMana);
    }
}