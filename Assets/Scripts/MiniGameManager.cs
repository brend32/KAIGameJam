using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [Header("Мінігри")]
    public MiniGame drillingGame;     // Q — буріння
    public MiniGame temperatureGame;  // W — температура
    public MiniGame radiationGame;    // E — радіація

    private MiniGame currentGame;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Toggle(drillingGame);

        if (Input.GetKeyDown(KeyCode.W))
            Toggle(temperatureGame);

        if (Input.GetKeyDown(KeyCode.E))
            Toggle(radiationGame);

        // ESC — закрити активну
        if (Input.GetKeyDown(KeyCode.Escape) && currentGame != null)
        {
            currentGame.Close();
            currentGame = null;
        }
    }

    void Toggle(MiniGame game)
    {
        if (game == null) return;

        // Якщо ця сама вже відкрита — закрити
        if (currentGame == game)
        {
            game.Close();
            currentGame = null;
            return;
        }

        // Закрити попередню
        if (currentGame != null)
            currentGame.Close();

        game.Open();
        currentGame = game;
    }
}