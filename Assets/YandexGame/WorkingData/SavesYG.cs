
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "DefaultName!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        public int ActiveLevel = int.MinValue;
        public int LastLevel = 0;
        public int PlayerMoney = 0;

        public int LastInteractiveTraining = -1;
        public int LastTraining = -1;

        public int FirstGame = 0;
        public int EndGame = 0;

        public float Music;
        public float Sounds;

        public string InfinityGameSettings = null;
        public string Statistic = null;
        public string Shop = null;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
