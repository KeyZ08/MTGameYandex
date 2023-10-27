using System;
using System.Collections.Generic;

namespace Game
{
    public static class Map
    {
        public static readonly List<Level> Levels = new List<Level>()
        {
            //0,1,10
            new LevelGame2(0, new TrainingSettings(0, 0, true), new WinSettings(5), (5, 50)),
            new LevelGame1(0, new TrainingSettings(0, 1, false), new WinSettings(10), new [] {0, 1, 10 }, 15, true),
            new LevelGame2(0, new TrainingSettings(0, 1, false), new WinSettings(5), (1, 4)),
            new LevelGame1(0, new TrainingSettings(0, 1, false), new WinSettings(10), new [] {0, 1, 10 }, 15, false),
            //2
            new LevelGame4(new TrainingSettings(2, 1, true), new WinSettings(25), 10),
            new LevelGame1(1, new TrainingSettings(2, 4, false), new WinSettings(10), new [] {1, 2, 10 }, 15, true),
            new LevelGame4(new TrainingSettings(2, 4, false), new WinSettings(25), 10),
            new LevelGame3(new TrainingSettings(2, 4, false), new WinSettings(15, "Получено:", false, "+ Ягоды дружбы."),
                new[]{(9, int.MinValue, 0), (9, 1, int.MinValue), (int.MinValue, 7, 14) }, Ingredients.Berries),
            //3
            new LevelGame1(1, new TrainingSettings(3, 5, true), new WinSettings(10), new [] {2, 3}, 15, true),
            new LevelGame2(1, new TrainingSettings(3, 5, false), new WinSettings(5), (3, 18)),
            new LevelGame5(2, new TrainingSettings(4, 6, true), new WinSettings(40), new List<int>(){2,3,2,3,10,10 }, (3, 2)),
            //4
            new LevelGame4(new TrainingSettings(5, 7, true), new WinSettings(25), 10),
            new LevelGame1(2, new TrainingSettings(5, 7, false), new WinSettings(10), new [] {2,3,4}, 15, true),
            new LevelGame5(2, new TrainingSettings(5, 7, false), new WinSettings(40), new List<int>(){1,1,3,7,8,4}, (2, 4)),
            //5
            new LevelGame1(3, new TrainingSettings(6, 8, true), new WinSettings(10), new [] {3,4,5}, 15, false),
            new LevelGame2(3, new TrainingSettings(6, 8, false), new WinSettings(5), (5, 35)),
            new LevelGame3(new TrainingSettings(6, 8, false), new WinSettings(15, "Получено:", false, "+ Ветка доброты."),
                new[]{(int.MinValue, 8, 40), (5, 0, int.MinValue), (4, int.MinValue, 32) }, Ingredients.Branch),
            //6
            new LevelGame1(3, new TrainingSettings(7, 9, true), new WinSettings(10), new [] {4,5,6}, 15, true),
            new LevelGame5(3, new TrainingSettings(7, 9, false), new WinSettings(40), new List<int>(){1,2,3,4,5,6}, (5, 2)),
            new LevelGame4(new TrainingSettings(7, 9, false), new WinSettings(25), 10),
            //7
            new LevelGame1(4, new TrainingSettings(8, 9, true), new WinSettings(10), new [] {5,6,7}, 15, true),
            new LevelGame5(4, new TrainingSettings(8, 9, false), new WinSettings(40), new List<int>(){5,6,7,10,10,2}, (7, 3)),
            new LevelGame3(new TrainingSettings(8, 8, false), new WinSettings(15, "Получено:", false, "+ Учебник математики."),
                new[]{(2, 3, int.MinValue), (7, int.MinValue, 49), (int.MinValue, 5, 30) }, Ingredients.Textbook),
            //8
            new LevelGame1(4, new TrainingSettings(9, 10, true), new WinSettings(10),  new [] {6,7,8}, 15, true),
            new LevelGame2(4, new TrainingSettings(9, 10, false), new WinSettings(5), (8, 24)),
            new LevelGame4(new TrainingSettings(9, 10, false), new WinSettings(25), 10),
            //9
            new LevelGame1(5, new TrainingSettings(10, 11, true), new WinSettings(10), new [] {7,8,9}, 25, true),
            new LevelGame5(5, new TrainingSettings(10, 11, false), new WinSettings(40), new List<int>(){5,6,2,1,3,0}, (9, 8)),
            new LevelGame4(new TrainingSettings(10, 11, false), new WinSettings(25), 10),
            new LevelGame3(new TrainingSettings(10, 11, false), new WinSettings(15, "Получено:", false, "+ Коготь монстра."),
                new[]{(8, int.MinValue, 48), (7, int.MinValue, 42), (int.MinValue, 9, 81) }, Ingredients.Claw),
        };
    }


    /// <summary>
    /// Параметры для обучения на уровне
    /// </summary>
    public class TrainingSettings
    {
        public readonly int TrainingBook;
        public readonly int ActivePage;
        public readonly bool ActiveOnLoadScene;

        public TrainingSettings(int trainingBook, int activePage, bool activeOnLoadScene)
        {
            TrainingBook = trainingBook;
            ActivePage = activePage;
            ActiveOnLoadScene = activeOnLoadScene;
        }
    }

    public class WinSettings
    {
        public readonly int MoneyCount;
        public readonly string Message;
        public readonly bool Centered;
        public readonly string SubMessage;

        public WinSettings(int moneyCount, string message = "Получено:", bool centered = false, string subMessage = "")
        {
            if (moneyCount < 0) throw new ArgumentException("moneyCount < 0");
            Message = message;
            Centered = centered;
            MoneyCount = moneyCount;
            SubMessage = subMessage;
        }
    }
    #region LevelSettings

    public abstract class Level
    {
        public readonly TrainingSettings Training;
        public readonly WinSettings Win;
        public readonly string SceneName;

        public Level(string scene, TrainingSettings training, WinSettings win)
        {
            SceneName = scene;
            Training = training;
            Win = win;
        }
    }

    /// <summary>
    /// Основной геймплей
    /// </summary>
    public class LevelGame1 : Level
    {
        public readonly int BackgroundImage;

        public readonly bool[] UsedNums = new bool[] { true, true, true, true, true, true, true, true, true, true, true };
        public readonly int ExamplesCount = 10;
        public readonly bool NullInResult = true;

        /// <summary>
        /// Основной геймплей
        /// </summary>
        public LevelGame1(int imgIndex, TrainingSettings training, WinSettings win, int[] usedNums, int examplesCount, bool nullInResult)
            : base("Game1", training, win)
        {
            BackgroundImage = imgIndex;

            UsedNums = new bool[11];
            foreach (int i in usedNums)
            {
                UsedNums[i] = true;
            }
            ExamplesCount = examplesCount;
            NullInResult = nullInResult;
        }
    }

    /// <summary>
    /// Шарики
    /// </summary>
    public class LevelGame2 : Level
    {
        public readonly int BackgroundImage;
        public readonly (int f1, int f2, int result) Example;

        /// <summary>
        /// Шарики
        /// </summary>
        public LevelGame2(int imgIndex, TrainingSettings training, WinSettings win, (int f1, int result) example)
            : base("Game2", training, win)
        {
            BackgroundImage = imgIndex;
            Example = (example.f1, int.MinValue, example.result);
        }
    }

    /// <summary>
    /// Сундук
    /// </summary>
    public class LevelGame3 : Level
    {
        public readonly (int f1, int f2, int result)[] Examples;
        public readonly Ingredients Ingredient;

        /// <summary>
        /// Сундук
        /// </summary>
        public LevelGame3(TrainingSettings training, WinSettings win, (int f1, int f2, int result)[] examples, Ingredients obj)
            : base("Game3", training, win)
        {
            if (examples.Length != 3) throw new ArgumentException();
            Examples = examples;
            Ingredient = obj;
        }
    }

    /// <summary>
    /// Лесной бой
    /// </summary>
    public class LevelGame4 : Level
    {
        public readonly int EnemyCount;

        /// <summary>
        /// Лесной бой
        /// </summary>
        public LevelGame4(TrainingSettings training, WinSettings win, int enemyCount) : base("Game4", training, win)
        {
            if (enemyCount < 1) throw new ArgumentException();
            EnemyCount = enemyCount;
        }
    }

    /// <summary>
    /// Дележка сокровищ
    /// </summary>
    public class LevelGame5 : Level
    {
        public readonly List<int> Choices;
        public readonly int BackgroundImage;
        public readonly (int bag1, int bag2) Bags;

        /// <summary>
        /// Дележка сокровищ
        /// </summary>
        public LevelGame5(int imgIndex, TrainingSettings training, WinSettings win, List<int> choices, (int bag1, int bag2) bags)
           : base("Game5", training, win)
        {
            Choices = choices;
            BackgroundImage = imgIndex;
            Bags = bags;
        }
    }

    #endregion
}
