using UnityEngine;

[CreateAssetMenu(menuName = "Yes Chef/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [field: SerializeField]
    public IngredientType Type { get; private set; }

    [field: SerializeField]
    public int ScoreValue { get; private set; }

    [field: SerializeField]
    public float PreparationTime { get; private set; }
}