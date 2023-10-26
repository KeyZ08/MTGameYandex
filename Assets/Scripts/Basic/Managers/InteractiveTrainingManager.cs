using System.Collections.Generic;

public static class InteractiveTrainingManager
{
	public static readonly Dictionary<string, string> Scenes = new Dictionary<string, string>()
	{
		{ "Game5", "Game5Learning" },
		{ "Game4", "Game4Learning" },
	};

	public static string GetScene(int levelNum)
	{
		if (levelNum == 10)
			return Scenes["Game5"];
		if (levelNum == 4)
			return Scenes["Game4"];
		else return null;
	}
}

