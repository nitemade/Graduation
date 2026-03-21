using System;
using System.Diagnostics;

public static class CharacterStatEventBus
{
    public static Action<CharacterStats> OnStatsChanged;

    public static void RaiseStatsChanged(CharacterStats stats)
    {
        OnStatsChanged?.Invoke(stats);
    }
}