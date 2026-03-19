using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :
    Singleton<PlayerManager>,
    ISaveable<PlayerSaveData>
{
    private PlayerController player;
    private CharacterStats stats;
    private PlayerSaveData data;

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

        runtime.enhancementData.SetAll(data.enhancements);

        runtime.Recalculate();

        stats.SetHealth(data.currentHealth);
        stats.SetMana(data.currentMana);
    }
}