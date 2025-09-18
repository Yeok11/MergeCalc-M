using UnityEngine;

public class EditorController : MonoBehaviour
{
    [ContextMenu("PlayerPrefs Reset")]
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
