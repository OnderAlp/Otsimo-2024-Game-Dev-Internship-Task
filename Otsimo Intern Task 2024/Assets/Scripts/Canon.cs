using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject effect;
    public void RotateCanon(Vector3 mousePos)
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = new Vector2(mousePos.x - 7, mousePos.y - 4);

        transform.up = direction;

        GetComponent<Animator>().SetTrigger("Fire");
    }
}
