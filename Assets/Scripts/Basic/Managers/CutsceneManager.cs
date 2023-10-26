using System.Collections.Generic;

public static class CutsceneManager
{
    public static readonly Dictionary<string, string> CutScenes = new Dictionary<string, string>()
    {
        { "Start", "StartGame" },
        { "End", "EndGame" },
        { "Fox", "CutsceneFox" },
        { "Balloons", "CutsceneBalloons" }
    };

    public static string GetCutscene(int levelNum)
    {
        if (levelNum == 0)
            return CutScenes["Balloons"];
        else if (levelNum == 10)
            return CutScenes["Fox"];
        else return null;
    }
}

