using System.IO;
using UnityEditor;
using UnityEngine; 

[CreateAssetMenu(fileName = "newCell", menuName = "New Cell")]
public class Cell : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public Color32 backColor = Color.white;
    public float xToYRatio = 1;
    private void OnValidate()
    {
        name = Path.GetFileNameWithoutExtension( AssetDatabase.GetAssetPath(this));
    }
}
