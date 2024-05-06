using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBall : MonoBehaviour
{
    public GameObject effect;

    Vector3 mousePos;
    float speed;
    Color paintBallColor;

    void Awake()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        speed = 20.0f * Time.deltaTime;
        paintBallColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f);
        GetComponent<SpriteRenderer>().color = paintBallColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, mousePos) < 0.1f)
        {
            UIManager.Instance.PaintBallHit(paintBallColor, mousePos);
            GameObject effectTemp = Instantiate(effect, transform.position, Quaternion.identity);
            effectTemp.GetComponent<ExplosionEffect>().ChangeColor(paintBallColor);
            Destroy(this.gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, mousePos, speed);

    }
}
