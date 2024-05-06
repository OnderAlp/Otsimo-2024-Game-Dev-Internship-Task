using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoinClickGetColor : MonoBehaviour
{
    [SerializeField]
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit) )
        {
            if (hit.collider != null && hit.collider.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
            {
                color = renderer.color;
                Debug.Log(renderer.color);
            }
        }
        
    }
}
