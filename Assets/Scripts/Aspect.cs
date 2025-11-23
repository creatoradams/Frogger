using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Aspect : MonoBehaviour
{
    [Tooltip("Target width = 14")]
    public float aspect_width = 14f;
    [Tooltip("Target height = 16")]
    public float aspect_height = 16f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyAspect(); 
    }

    void ApplyAspect()
    {
        float targetAspect = aspect_width / aspect_height;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // add letterbox at the top & bottom
            Rect rect = new Rect(0, (1f - scaleHeight) / 2f, 1f, scaleHeight);
            cam.rect = rect; 
        }
        else
        {
            // add pillarbox left & right
            float scaleWidth = 1f / scaleHeight;
            Rect rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
            cam.rect = rect;
        }
    }
}
