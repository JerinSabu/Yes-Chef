using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public List<IngredientType> PendingIngredients { get; private set; }
    public float SpawnTime { get; private set; }
    public int BaseScore { get; private set; }

    public Order(float time)
    {
        SpawnTime = time;
        PendingIngredients = new List<IngredientType>();

        // 50% chance to be 2 or 3 ingredients
        int ingredientCount = Random.value > 0.5f ? 3 : 2;

        for (int i = 0; i < ingredientCount; i++)
        {
            // Randomly pick Vegetable (0), Meat (1), or Cheese (2)
            IngredientType randomIngredient = (IngredientType)Random.Range(0, 3);

            if (randomIngredient == IngredientType.Vegetable) BaseScore += 20; 
            else if (randomIngredient == IngredientType.Meat) BaseScore += 30; 
            else if (randomIngredient == IngredientType.Cheese) BaseScore += 10; 


            PendingIngredients.Add(randomIngredient);
        }
    }

    // Checks if the ingredient is needed, and removes it from the list if it is
    public bool TryDeliverIngredient(IngredientType type)
    {
        if (PendingIngredients.Contains(type))
        {
            PendingIngredients.Remove(type);
            return true;
        }
        return false;
    }

    // The order is done when there are no pending ingredients left
    public bool IsComplete()
    {
        return PendingIngredients.Count == 0;
    }
}