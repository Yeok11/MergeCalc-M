using UnityEngine;

public class DragEvent : MonoBehaviour
{
    [SerializeField, Range(40, 100)] private int sensitive = 50;
    private Vector2 startTouchPos;
    private Vector2 endPos;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) startTouchPos = touch.position;
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 pos = touch.position - startTouchPos;
                bool isHorizontal = Mathf.Abs(pos.x) > Mathf.Abs(pos.y);

                if(isHorizontal)
                {
                    if (pos.x < -sensitive) Debug.Log("¿Þ");
                    else if (pos.x > sensitive) Debug.Log("¿À");
                }
                else
                {
                    if (pos.y < -sensitive) Debug.Log("¾Æ·¡");
                    else if (pos.y > sensitive) Debug.Log("À§");
                }
            }
        }
    }
}
