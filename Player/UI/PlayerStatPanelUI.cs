using UnityEngine;
using TMPro;
using System;

public class PlayerStatPanelUI : MonoBehaviour
{
    public TMP_Text hpText;
    public TMP_Text manaText;
    public TMP_Text speedText;
    public TMP_Text damageText;
    public TMP_Text defenseText;
    public TMP_Text atkSpeedText;



    private void OnEnable()
    {
        CharacterStatEventBus.OnStatsChanged += OnStatsChanged;

    }

    private void Start()
    {
        SyncNow(); 
    }

    void SyncNow()
    {
        var stats = PlayerManager.Instance.PlayerStats;

        if (stats == null)
        {
            return;
        }

        UpdateUI(stats);
    }
    private void OnDisable()
    {
        CharacterStatEventBus.OnStatsChanged -= OnStatsChanged;
    }

    void OnStatsChanged(CharacterStats stats)
    {

        if (stats != PlayerManager.Instance.PlayerStats)
        {
            return;
        }
        UpdateUI(stats);
    }

    void UpdateUI(CharacterStats stats)
    {
        hpText.text =
            $"{stats.CurrentHealth} / {stats.MaxHealth}";

        manaText.text =
            $"{stats.CurrentMana} / {stats.MaxMana}";

        speedText.text =
            stats.Speed.ToString();

        damageText.text =
            stats.NormalDamage.ToString();

        defenseText.text =
            stats.Defense.ToString();

        atkSpeedText.text =
            stats.NormalAttackSpeed.ToString();
    }
}