using UnityEngine;

public class MiniGame : MonoBehaviour
{
    [Header("Плавне відкриття")]
    public SmoothFade fade;

    public virtual void Open()
    {
        if (fade != null)
        {
            fade.Alpha = 1f;

            if (fade.Group != null)
            {
                fade.Group.interactable = true;
                fade.Group.blocksRaycasts = true;
            }
        }

        OnOpen();
    }

    public virtual void Close()
    {
        if (fade != null)
        {
            fade.Alpha = 0f;

            if (fade.Group != null)
            {
                fade.Group.interactable = false;
                fade.Group.blocksRaycasts = false;
            }
        }

        OnClose();
    }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}