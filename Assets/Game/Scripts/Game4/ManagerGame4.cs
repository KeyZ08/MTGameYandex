using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManagerGame4 : MonoBehaviour
{
    [Header("Field Boundaries")]
    public Transform left_top;
    public Transform right_bottom;

    [Header("Enemyes")]
    public Transform enemyes;
    public Transform enemySpawn;
    public List<GameObject> enemyPrefabs;

    [Header("Parameters")]
    public int EnemyesCount;
    public float EnemyStepSeconds;
    public TextMeshProUGUI Timer;

    [Header("Heroes")]
    public HeroGame4 hero;
    public BadHeroGame4 badHero;

    [Header("Sounds")]
    [SerializeField] AudioSource exampleAccess;
    [SerializeField] AudioSource enemySpawnSound;

    private readonly EntityGame4[,] _map = new EntityGame4[12, 10];
    private Queue<StepFreezeNum> stepFreezes;
    private float _timer;
    private bool _timerFreeze;

    private void Awake()
    {
        _timerFreeze = true;
        stepFreezes = new Queue<StepFreezeNum>();
    }

    public void SetParams(int enemyseCount)
    {
        EnemyesCount = enemyseCount;
    }

    public void Start()
    {
        var n = EnemyesCount > 5 ? 5 : EnemyesCount;
        for (int i = 0; i < n; i++)
        {
            if (EnemySpawn(enemyPrefabs[Random.Range(0, 3)], (Random.Range(1, 4), Random.Range(1, 11))) == null)
            {
                EnemyesCount++;
                i--;
            }
        }
        _timerFreeze = false;
    }

    private void Update()
    {
        if (!_timerFreeze)
        {
            _timer += Time.deltaTime;
            Timer.text = (EnemyStepSeconds - (int)_timer).ToString();
            if (_timer >= EnemyStepSeconds)
            {
                StartCoroutine(EnemiesStep());
                _timer = 0;
            }
        }
    }

    public void CorrectExample(int x, int y)
    {
        hero.SetTargetAttack(TablePositionToPosition(x, y));
        exampleAccess.Play();
        if (x != y)
        {
            hero.SetTargetAttack(TablePositionToPosition(y, x));
            stepFreezes.Enqueue(new StepFreezeNum(2));
        }
        else
        {
            stepFreezes.Enqueue(new StepFreezeNum(1));
        }
    }

    public void HeroAttack(Vector2 worldPosition)
    {
        var freeze = stepFreezes.Peek();
        freeze.num -= 1;
        
        (int x, int y) = PositionToTablePosition(worldPosition);
        x -= 1; y -= 1;
        if (_map[x, y] is EntityGame4)
        {
            _map[x, y].Death();
            _map[x, y] = null;
            if (MapEmpty())
            {
                _timer = EnemyStepSeconds;
                if (EnemyesCount <= 0)
                    StartCoroutine(WaitCoroutine(2f, () => { FindAnyObjectByType<LevelUIs>().Win(); }));
                    
            }
        }
        if (freeze.num <= 0)
        {
            stepFreezes.Dequeue();
        }
    }

    private bool MapEmpty()
    {
        var flag = false;
        for (var i = 0; i < _map.GetLength(0); i++)
        {
            for (var k = 0; k < _map.GetLength(1); k++)
                if (_map[i, k] is EntityGame4)
                {
                    flag = true;
                    break;
                }
            if (flag) 
                break;
        }
        return !flag;
    }

    public void BadHeroAttack(Vector2 worldPosition)
    {
        (int x, int y) = PositionToTablePosition(worldPosition);
        EnemySpawn(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], (x, y));
        enemySpawnSound.Play();
    }

    IEnumerator EnemiesStep()
    {
        _timerFreeze = true;
        hero.IsFreezed = true;
        EnemiesMove();
        yield return new WaitForSeconds(2f);
        hero.IsFreezed = false;
        while (true)
        {
            if (EnemyesCount <= 0)
                break;
            var x = Random.Range(1, 3);
            var y = Random.Range(1, 10 + 1);
            if (_map[x - 1, y - 1] == null)
            {
                yield return StartCoroutine(BadHeroStep(x, y));
                break;
            }
        }
        _timerFreeze = false;
    }

    IEnumerator BadHeroStep(int x, int y)
    {
        hero.IsFreezed = true;
        badHero.SetTargetAttack(TablePositionToPosition(x, y));
        while (badHero.IsMoved) 
            yield return null;
        hero.IsFreezed = false;
    }



    /// <summary>
    /// Преводит координаты ячейки таблицы в мировые координаты
    /// 
    /// Координаты (от 1 до 10 включительно)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Vector2 TablePositionToPosition(int x, int y)
    {
        x -= 1;
        y -= 1;
        var offsetX = (right_bottom.position.x - left_top.position.x) / (10 - 1);
        var offsetY = (left_top.position.y - right_bottom.position.y) / (10 - 1);
        var posX = left_top.position.x + offsetX * x;
        var posY = left_top.position.y - offsetY * y;
        return new Vector2(posX, posY);
    }

    /// <summary>
    /// Преводит мировые координаты в координаты ячейки таблицы (действие, обратное функции TablePositionToPosition)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private (int x, int y) PositionToTablePosition(Vector2 pos)
    {
        var offsetX = (right_bottom.position.x - left_top.position.x) / (10 - 1);
        var offsetY = (left_top.position.y - right_bottom.position.y) / (10 - 1);
        var x = Math.Round((pos.x - left_top.position.x) / offsetX);
        var y = Math.Round((left_top.position.y - pos.y) / offsetY);
        return ((int)x + 1, (int)y + 1);
    }


    #region Enemy

    private void EnemiesMove()
    {
        HashSet<EntityGame4> movedEnemy = new HashSet<EntityGame4>();
        for (int x = 10; x >= 1; x--)
        {
            for (int y = 10; y >= 1; y--)
            {
                var obj = _map[x-1, y-1];
                if (obj == null || movedEnemy.Contains(obj)) continue;
                movedEnemy.Add(obj);
                var oldPos = (x, y);
                var potentialSteps = GetPotentialMove(obj.type, x, y);
                var step = CheckMove(obj.type, potentialSteps, oldPos);
                EnemyMove(oldPos, step);
            }
        }
    }


    /// <summary>
    /// Создает врага на сцене
    /// </summary>
    /// <param name="enemy">Префаб врага</param>
    /// <param name="positon">Координаты ячейки таблицы (от 1 до 10 включительно)</param>
    /// <returns></returns>
    private EntityGame4 EnemySpawn(GameObject enemy, (int x, int y) positon)
    {
        EnemyesCount -= 1;
        if (_map[positon.x - 1, positon.y - 1] != null)
        {
            Debug.Log("место" + positon + "уже занято!");
            return null;
        }
        var en = Instantiate(enemy, enemySpawn.position, Quaternion.identity, enemyes).GetComponent<EntityGame4>();
        en.TPto(TablePositionToPosition(positon.x, positon.y));
        en.RowPos = positon.y * 10 + positon.x;
        _map[positon.x - 1, positon.y - 1] = en;
        return en;
    }

    /// <summary>
    /// Перемещает врага по таблице
    /// 
    /// Координаты (от 1 до 10 включительно)
    /// </summary>
    /// <param name="oldPos">старые координаты</param>
    /// <param name="newPos">новые координаты</param>
    private void EnemyMove((int x, int y) oldPos, (int x, int y) newPos)
    {
        if (oldPos == newPos) return;
        oldPos.x -= 1;
        oldPos.y -= 1;

        var enemy = _map[oldPos.x, oldPos.y];
        enemy.MoveTo(TablePositionToPosition(newPos.x, newPos.y));
        enemy.RowPos = newPos.y * 10 + newPos.x;

        _map[newPos.x - 1, newPos.y - 1] = enemy;
        _map[oldPos.x, oldPos.y] = null;

        if (newPos.x > 10)
        {
            StartCoroutine(WaitCoroutine(2f, () => {
                FindAnyObjectByType<LevelUIs>().Lose(false);
            }));
        }
    }

    IEnumerator WaitCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
        yield return null;
    }

    /// <summary>
    /// Находит ячейки куда потенциально можно встать.
    /// 
    /// (x,y) - Координаты ячейки таблицы (от 1 до 10 включительно) - текущее местонахождение
    /// </summary>
    /// <param name="enemy">Тип врага - определяет как он ходит</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">Поведение данного типа врага ещё не реализовано.</exception>
    private List<(int x, int y)> GetPotentialMove(EnemyesGame4 enemy, int x, int y)
    {
        var result = new List<(int x, int y)>();
        var m = new[] { (0, 0) };
        if (enemy == EnemyesGame4.Horse)
        {
            //как конь в шахматах но берем только направления, приблежающие нас в правую сторону
            m = new[] { (2, 1), (1, 2), (2, -1), (1, -2) };       
        }
        else if (enemy == EnemyesGame4.Demon1)
        {
            //прямо и по диагонали на 2 шага
            m = new[] { (1, 0), (2, 0), (1, 1), (1, -1), (2, 2), (2, -2) };
        }
        else if (enemy == EnemyesGame4.Demon2)
        {
            //1 или 2 шага по прямой (влево нельзя)
            m = new[] { (1, 0), (2, 0), (0, 1), (0, 2), (0, -1), (0, -2) };
        }
        else
            throw new NotImplementedException();
        foreach ((int x, int y) i in m)
            result.Add((x + i.x, y + i.y));
        return result;
    }

    /// <summary>
    /// Отсеивает недостижимые позиции и возвращает ход куда пойдет враг
    /// </summary>
    /// <param name="enemy">Тип врага</param>
    /// <param name="movements">Потенциально возможные ходы</param>
    /// <returns></returns>
    private (int x, int y) CheckMove(EnemyesGame4 enemy, List<(int x, int y)> movements, (int x, int y) position)
    {
        var result = new List<(int x, int y)>();
        
        for (var i = 0; i < movements.Count; i++)
        {
            var item = movements[i];
            if (!CheckBounds(item)) continue;

            if (enemy == EnemyesGame4.Demon2)
            {
                if (_map[item.x - 1, item.y - 1] == null && Math.Sqrt(Math.Pow(item.x - position.x, 2) + Math.Pow(item.y - position.y, 2)) == 1)
                    result.Add(item);
                else if (_map[item.x - 1, item.y - 1] == null)
                {
                    (int x, int y) offset = ((item.x - position.x) / 2, (item.y - position.y) / 2);
                    (int x, int y) average = (position.x + offset.x, position.y + offset.y);
                    if (_map[average.x - 1, average.y - 1] == null)
                        result.Add(item);
                }
            }
            else
            {
                if (_map[item.x - 1, item.y - 1] == null)
                    result.Add(item);
            }
        }
        if (result.Count == 0) return position;
        var r = result[Random.Range(0, result.Count)];
        return r;

        bool CheckBounds((int x, int y) pos)
        {
            if (pos.x > 10 + 2 /* две ячейки для проигрыша */ || pos.x < 1 || pos.y < 1 || pos.y > 10)
                return false;
            return true;
        }
    }

    class StepFreezeNum
    {
        public StepFreezeNum(int n)
        {
            num = n;
        }

        public int num;
    }
    #endregion
}
