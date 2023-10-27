using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManagerGame1 : MonoBehaviour
{
    public HeroGame1 hero;
    public MultiplierGame1 multiplier;
    public IEnumerator<(int a, int b, int c)> exampels;
    public List<GameObject> magics;

    public Transform enemies;
    public List<GameObject> monsters;

    public AudioSource MagicHit;

    private int _monsterCount;

    private int ExamplesCount;
    private bool NullInResult;
    private bool[] UsedNums;
    private GameManagerSetParams manager;

    public void SetParams(bool[] usedNums, int examplesCount, bool nullInResult)
    {
        UsedNums = usedNums;
        ExamplesCount = examplesCount;
        NullInResult = nullInResult;
    }

    public void Start()
    {
        manager = GetComponent<GameManagerSetParams>();
        if (monsters.Count == 0) throw new ArgumentNullException("Ќет монстров");
        exampels = ExamplesGenerator(UsedNums, ExamplesCount, NullInResult);
        _monsterCount = ExamplesCount;
        ResetMultiplier();
        MonsterCreate();
    }

    public void MagicHitSound()
    {
        MagicHit.Play();
    }

    public void MonsterDeath()
    {
        if (_monsterCount == 0 && FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 1)
            manager.Win();
    }

    private void MonsterCreate()
    {
        _monsterCount -= 1;
        Instantiate(monsters[Random.Range(0, monsters.Count)], enemies);
    }

    public void ResetMultiplier()
    {
        if (exampels.MoveNext())
        {
            var (a, b, c) = exampels.Current;
            multiplier.Create(a, b, c, 4);
        }
        else 
        {
            Debug.Log("ѕримеры кончились!"); 
        }
    }

    public void CorrectExample(int[] nums)
    {
        if (manager is InfinityGameManager)
        {
            Statistic.Current.AddRight(nums);
        }
        hero.Attack();
        if (_monsterCount > 0) 
            MonsterCreate();
        Instantiate(magics[Random.Range(0, magics.Count)], hero.magicCast.transform.position, Quaternion.AngleAxis(90f, new Vector3(0,0,90)), hero.transform.parent);
    }

    public void MistakeExample(int[] nums)
    {
        if (manager is InfinityGameManager)
        {
            Statistic.Current.AddIncorrect(nums);
        }
    }

    /// <summary>
    /// √енератор примеров по установленным параметрам
    /// </summary>
    /// <param name="variants">массив длинны 11 ([0...10]) из 0 и 1. ≈диница означает что число по индексу можно использовать в примере </param>
    /// <param name="count">количество примеров</param>
    /// <param name="NullInResult">должно ли быть пусто после равно</param>
    /// <returns>—писок сгенерированных примеров</returns>
    /// <exception cref="ArgumentException"></exception>
    private IEnumerator<(int a, int b, int c)> ExamplesGenerator(bool[] variants, int count, bool NullInResult)
    {
        if (variants.Length != 11) throw new ArgumentException();

        var variantsNum = new List<int>();
        for (var i = 0; i < variants.Length; i++)
            if (variants[i]) variantsNum.Add(i);

        for (var i = 0; i < count; i++)
        {
            var a = variantsNum[Random.Range(0, variantsNum.Count)];
            var b = Random.Range(1, 11);
            var c = a * b;
            if (NullInResult)
            {
                c = int.MinValue;
            }
            else
            {
                a = int.MinValue;
                if (Random.Range(0, 2) == 1)
                    (a, b) = (b, a);
            }
            yield return (a,b,c);
        }
    }
}
