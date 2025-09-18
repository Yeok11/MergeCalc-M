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
            Touch _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began) startTouchPos = _touch.position;
            else if (_touch.phase == TouchPhase.Ended)
            {
                Vector2 _pos = _touch.position - startTouchPos;
                bool isHorizontal = Mathf.Abs(_pos.x) > Mathf.Abs(_pos.y);

                if(isHorizontal)
                {
                    if (_pos.x < -sensitive) dragEvent?.Invoke(Direction.Left);
                    else if (_pos.x > sensitive) dragEvent?.Invoke(Direction.Right);
                }
                else
                {
                    if (_pos.y < -sensitive) dragEvent?.Invoke(Direction.Down);
                    else if (_pos.y > sensitive) dragEvent?.Invoke(Direction.Up);
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) dragEvent?.Invoke(Direction.Up);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) dragEvent?.Invoke(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) dragEvent?.Invoke(Direction.Down);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) dragEvent?.Invoke(Direction.Right);
#endif
    }
}