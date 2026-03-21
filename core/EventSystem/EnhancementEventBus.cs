using System;
using UnityEngine;

public static class EnhancementEventBus
{
    public static Action<Enhancement_SO> OnEnhancementAdded;

    public static void RaiseEnhancementAdded(Enhancement_SO e)
    {
        OnEnhancementAdded?.Invoke(e);
    }
}