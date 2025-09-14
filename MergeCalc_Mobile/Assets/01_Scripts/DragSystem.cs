using UnityEngine;
using UnityEngine.Events;

public class DragSystem : MonoBehaviour
{
    [SerializeField, Range(40, 100)] private int sensitive = 50;
    private Vector2 startTouchPos;

    internal UnityAction<Direction> dragEvent;

    private void Update()
    {
        if (!GameData.canDrag) return;

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
                    if (pos.x < -sensitive) dragEvent?.Invoke(Direction.Left);
                    else if (pos.x > sensitive) dragEvent?.Invoke(Direction.Right);
                }
                else
                {
                    if (pos.y < -sensitive) dragEvent?.Invoke(Direction.Down);
                    else if (pos.y > sensitive) dragEvent?.Invoke(Direction.Up);
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W)) dragEvent?.Invoke(Direction.Up);
        else if (Input.GetKeyDown(KeyCode.A)) dragEvent?.Invoke(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.S)) dragEvent?.Invoke(Direction.Down);
        else if (Input.GetKeyDown(KeyCode.D)) dragEvent?.Invoke(Direction.Right);
#endif
    }
}