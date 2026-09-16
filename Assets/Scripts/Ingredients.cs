public enum IngredientType
{
    Vegetable,
    Meat,
    Cheese
}

public enum IngredientState
{
    Raw,
    Chopped,
    Cooked
}

public class Ingredient
{
    public IngredientType Type { get; private set; }
    public IngredientState State { get; set; }

    public Ingredient(IngredientType type, IngredientState state = IngredientState.Raw)
    {
        Type = type;
        State = state;
    }
}