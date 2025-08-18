using UnityEngine;

[CreateAssetMenu(menuName = "SO/Mode")]
public class ModeSO : ScriptableObject
{
    public Mode mode;
    public string firLineMes, secLineMes;
    public bool showScore = false;
}
