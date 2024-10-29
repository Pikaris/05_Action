using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour, IHealth
{
    public float HP => 50.0f;

    public float MaxHP => 100.0f;

    public bool IsAlive => true;

    public event Action<float> onHealthChange;
    public event Action onDie;

    public void Die()
    {
        onDie?.Invoke();
    }

    public void HealthHeal(float heal)
    {
        onHealthChange?.Invoke(0.5f);
    }

    public void HealthRegenerate(float totalRegen, float duration)
    {
        onHealthChange?.Invoke(0.5f);
    }

    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        onHealthChange?.Invoke(0.5f);
    }
}
