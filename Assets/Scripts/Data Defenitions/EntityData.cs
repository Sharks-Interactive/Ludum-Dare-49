using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
    [Tooltip("Starting health")]
    public int Health = 5000;

    [Tooltip("Human name for this entity")]
    [TextArea(3, 10)]
    public string DisplayName;

    [Tooltip("One line description for this entity")]
    [TextArea(3, 10)]
    public string ShortDescription;

    public Sprite EntitySprite;

    public Gradient[] TeamColors = new Gradient[2];
}
