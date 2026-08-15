using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public MiniGame drillingGame;
    public MiniGame temperatureGame;
    public MiniGame radiationGame;

    private MiniGame currentGame;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Toggle(drillingGame);

        if (Input.GetKeyDown(KeyCode.W))
            Toggle(temperatureGame);

        if (Input.GetKeyDown(KeyCode.E))
            Toggle(radiationGame);

        if (Input.GetKeyDown(KeyCode.Escape) && currentGame != null)
            CloseCurrent();
    }

    void Toggle(MiniGame game)
    {
        if (game == null) return;

        if (currentGame == game)
        {
            CloseCurrent();
            return;
        }

        if (currentGame != null)
            currentGame.Close();

        game.Open();
        currentGame = game;

        PlayerMovement.CanMove = false; // <-- заблокувати рух
    }

    void CloseCurrent()
    {
        if (currentGame != null)
        {
            currentGame.Close();
            currentGame = null;
        }

        PlayerMovement.CanMove = true; // <-- дозволити рух
    }
}