using UnityEngine;
using UnityEngine.UI;

public class WireTemperature : MonoBehaviour
{
    public Gradient Gradient;
    public EnvTesterDevice TesterDevice;
    public Image Image;
    public float Max;

    public void Update()
    {
        Image.color = Gradient.Evaluate(TesterDevice.DisplayedTemperature / Max);
    }
}