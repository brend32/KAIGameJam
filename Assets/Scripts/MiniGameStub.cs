using UnityEngine;

public class MiniGameStub : MiniGame
{
    [Header("Назва заглушки (для дебагу)")]
    public string stubName;

    protected override void OnOpen()
    {
        Debug.Log($"Відкрито заглушку: {stubName}");
    }

    protected override void OnClose()
    {
        Debug.Log($"Закрито заглушку: {stubName}");
    }
}