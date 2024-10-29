using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IHealth, IMana
{
    // HP와 MP가 존재
    // 먹으면 HP와 MP가 천천히 증가하는 아이템 만들기(IConsumable 상속) - ItemData.Food, ItemData_Drink
    // Food는 틱 단위로 회복
    // Drick는 틱 없이 일정하게 회복
    // 인스펙터 창에서 아이콘 표시하기

    /// <summary>
    /// 플레이어의 HP
    /// </summary>
    float hp = 100.0f;

    /// <summary>
    /// 플레이어의 최대 HP
    /// </summary>
    float maxPlayerHP = 100.0f;

    /// <summary>
    /// 플레이어의 MP
    /// </summary>
    float mp = 100.0f;

    /// <summary>
    /// 플레이어의 최대 MP
    /// </summary>
    float maxPlayerMP = 100.0f;

    /// <summary>
    /// 플레이어의 기본 공격력
    /// </summary>
    public float baseAttackPower = 5.0f;

    /// <summary>
    /// 플레이어의 기본 방어력
    /// </summary>
    public float baseDefencePower = 1.0f;

    /// <summary>
    /// 플레이어의 장비 공격력
    /// </summary>
    float attackEquipPower = 0.0f;

    /// <summary>
    /// 플레이어의 장비 방어력
    /// </summary>
    float defenceEquipPower = 0.0f;

    


    /// <summary>
    /// 플레이어의 HP를 확인하고 설정하기 위한 프로퍼티(설정은 private)
    /// </summary>
    public float HP
    {
        get => hp;
        private set
        {
            if (IsAlive)    //살아있을 때만 적용
            {
                hp = value;
                if(hp <= 0.0f)  // 0 이하로 내려가면 사망처리
                {
                    Die();
                }

                hp = Mathf.Clamp(hp, 0.0f, maxPlayerHP);
                onHealthChange?.Invoke(hp / maxPlayerHP);
                Debug.Log($"HP : {HP}");
            }
        }
    }

    /// <summary>
    /// 플레이어의 최대 HP를 확인하기 위한 프로퍼티
    /// </summary>
    public float MaxHP => maxPlayerHP;

    /// <summary>
    /// 플레이어의 생존 여부를 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// 플레이어의 MP를 확인하고 설정하기 위한 프로퍼티(설정은 private)
    /// </summary>
    public float MP
    {
        get => mp;
        private set
        {
            if (IsAlive)     // 살아있을 때만 적용
            {
                mp = Mathf.Clamp(value, 0.0f, maxPlayerMP);     // 최대/최소치를 벗어날 수 없음
                onManaChange?.Invoke(mp / maxPlayerMP);
                Debug.Log($"MP : {MP}");
            }
        }
    }
    /// <summary>
    /// 최대 MP를 확인하기 위한 프로퍼티
    /// </summary>
    public float MaxMP => maxPlayerMP;

    /// <summary>
    /// 플레이어의 최종 공격력을 확인하기 위한 프로퍼티
    /// </summary>
    public float AttackPower => baseAttackPower + attackEquipPower;

    /// <summary>
    /// 플레이어의 최종 방어력을 확인하기 위한 프로퍼티
    /// </summary>
    public float DefencePower => baseDefencePower + defenceEquipPower;



    /// <summary>
    /// HP의 변화를 알리는 델리게이트
    /// </summary>
    public event Action<float> onHealthChange;

    /// <summary>
    /// MP의 변화를 알리는 델리게이트
    /// </summary>
    public event Action<float> onManaChange;

    /// <summary>
    /// 플레이어의 사망을 알리는 델리게이트
    /// </summary>
    public event Action onDie;

    /// <summary>
    /// 즉시 HP를 변경시키는 함수
    /// </summary>
    /// <param name="heal">변화시킬 양</param>
    public void HealthHeal(float heal)
    {
        HP += heal;
    }

    /// <summary>
    /// HP를 서서히 증가시키는 함수
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">회복 기간</param>
    public void HealthRegenerate(float totalRegen, float duration)
    {
        StartCoroutine(RegenCoroutine(totalRegen, duration, true));
    }

    /// <summary>
    /// HP를 틱당 회복시키는 함수
    /// </summary>
    /// <param name="tickRegen">틱당 회복량</param>
    /// <param name="tickInterval">틱간 시간 간격</param>
    /// <param name="totalTickCount">전체 틱 횟수</param>
    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(RegenByTick(tickRegen, tickInterval, totalTickCount, true));
    }

    /// <summary>
    /// 즉시 MP를 변경시키는 함수
    /// </summary>
    /// <param name="totalRegen">변화시킬 양</param>
    /// <param name="duration"></param>
    public void ManaRegenerate(float totalRegen, float duration)
    {
        StartCoroutine(RegenCoroutine(totalRegen, duration, false));
    }

    /// <summary>
    /// MP를 서서히 증가시키는 함수
    /// </summary>
    /// <param name="tickRegen"></param>
    /// <param name="tickInterval"></param>
    /// <param name="totalTickCount"></param>
    public void ManaRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(RegenByTick(tickRegen, tickInterval, totalTickCount, false));
    }

    public void ManaRestore(float restore)
    {
        MP += restore;
    }

    /// <summary>
    /// HP나 MP를 서서히 회복시키는 코루틴
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">회복 기간</param>
    /// <param name="isHP">true면 HP회복, false면 MP회복</param>
    /// <returns></returns>
    IEnumerator RegenCoroutine(float totalRegen, float duration, bool isHP)
    {
        float regenPerSec = totalRegen / duration;  // 전체 회복량
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            if (isHP)
            {
                HP += Time.deltaTime * regenPerSec;
            }
            else
            {
                MP += Time.deltaTime * regenPerSec;
            }
            yield return null;      // 매 프레임마다 HP나 MP를 초당 regenPerSec만큼 회복
        }
    }

    /// <summary>
    /// HP나 MP를 틱당 회복시키는 코루틴
    /// </summary>
    /// <param name="tickRegen">틱당 회복량</param>
    /// <param name="tickInterval">틱간 시간 간격</param>
    /// <param name="totalTickCount">전체 틱 횟수</param>
    /// <param name="isHP">true면 HP회복, falseㅔ]</param>
    /// <returns></returns>
    IEnumerator RegenByTick(float tickRegen, float tickInterval, uint totalTickCount, bool isHP)
    {
        WaitForSeconds wait = new WaitForSeconds(tickInterval);
        for (int i = 0; i < totalTickCount; i++)
        {
            Debug.Log("RegenByTick");
            if (isHP)
            {
                HP += tickRegen;
            }
            else
            {
                MP += tickRegen;
            }
            yield return wait;
        }
    }



    /// <summary>
    /// 플레이어가 사망했을 때 실행할 함수
    /// </summary>
    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("사망");
    }

    /// <summary>
    /// 장비의 능력치를 적용하는 함수
    /// </summary>
    /// <param name="equipType">장비의 종류</param>
    /// <param name="power">능력치</param>
    public void SetEquipPower(EquipType equipType, float power)
    {
        switch(equipType)
        {
            case EquipType.Weapon:
                attackEquipPower = power;
                break;
            case EquipType.Shield:
                defenceEquipPower = power;
                break;
        }
    }
}
