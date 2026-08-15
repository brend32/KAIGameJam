using UnityEngine;

[DefaultExecutionOrder(200)]
public class Parallax : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    public bool onlyX = true;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;

        float moveX = delta.x * parallaxFactor;
        float moveY = onlyX ? 0f : delta.y * parallaxFactor;

        transform.position += new Vector3(moveX, moveY, 0f);
        lastCamPos = cam.position;
    }
}