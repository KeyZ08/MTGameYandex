using System.Linq;
using UnityEngine;

public class StatisticView : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject StatisticRowPrefab;
    public Transform SpawnTarget;

    void Start()
    {
        var objects = Statistic.Statistics.OrderBy(x=>x.Date).Reverse().ToList();
        for (int i = 0; i < objects.Count; i++)
        {
            var obj = Instantiate(StatisticRowPrefab, SpawnTarget).GetComponent<StatisticRow>();
            obj.SetValues(objects[i].Date, objects[i].Right, objects[i].Incorrect, objects[i].RightCount, objects[i].IncorrectCount);
        }
    }
}
