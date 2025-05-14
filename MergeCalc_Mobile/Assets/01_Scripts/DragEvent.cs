using UnityEngine;

public class DragEvent : MonoBehaviour
{
    [SerializeField, Range(40, 100)] private int sensitive = 50;
    private Vector2 startTouchPos;

    [SerializeField] private Board board;

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
                    if (Mathf.Abs(pos.x) > sensitive) Debug.Log("좌우 드래그");

                    if (pos.x < -sensitive) board.OnDragEvent(Dir.Left);
                    else if (pos.x > sensitive) board.OnDragEvent(Dir.Right);
                }
                else
                {
                    if (Mathf.Abs(pos.y) > sensitive) Debug.Log("상하 드래그");

                    if (pos.y < -sensitive) board.OnDragEvent(Dir.Down);
                    else if (pos.y > sensitive) board.OnDragEvent(Dir.Up);
                }
            }
        }
    }
}
