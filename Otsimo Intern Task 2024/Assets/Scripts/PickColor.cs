using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickColor : MonoBehaviour
{
    public RawImage ColorPallete;
    public Image test;

    public Vector2 hotSpot = Vector2.zero;

    RawImage ColorPalleteSprite;
    Texture2D tex;
    Color32 col;

    Rect r;
    Vector2 localPoint;

    int px;
    int py;

    void Start()
    {
        ColorPalleteSprite = ColorPallete.GetComponent<RawImage>();
        tex = ColorPallete.texture as Texture2D;
        r = ColorPallete.rectTransform.rect;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ColorPallete.rectTransform, Input.mousePosition, null, out localPoint);

            if (localPoint.x > r.x && localPoint.y > r.y && localPoint.x < (r.width + r.x) && localPoint.y < (r.height + r.y))
            {
                px = Mathf.Clamp(0, (int)(((localPoint.x - r.x) * tex.width) / r.width), tex.width);
                py = Mathf.Clamp(0, (int)(((localPoint.y - r.y) * tex.height) / r.height), tex.height);

                col = tex.GetPixel(px, py);
                test.color = col;
            }
            else
            {
                Debug.Log("Outside of the Wheel.");
            }
        }
    }

    public void SelectWheelColor()
    {
        UIManager.Instance.SelectColorWheelColor(col);
        UIManager.Instance.SetDrawingActivation(true);
        UIManager.Instance.DisableColorWheelPanel();
    }
}
