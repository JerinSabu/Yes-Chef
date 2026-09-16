using UnityEngine;

public class Refrigerator : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        
        if (player.HeldIngredient == null)
        {
            Ingredient newIngredient = new Ingredient(IngredientType.Vegetable, IngredientState.Raw);
            player.SetHeldIngredient(newIngredient);
            Debug.Log("Dispensed a raw Vegetable");
        }
        else
        {
            
            IngredientType currentType = player.HeldIngredient.Type;
            IngredientType nextType;

            switch (currentType)
            {
                case IngredientType.Vegetable:
                    nextType = IngredientType.Meat;
                    break;
                case IngredientType.Meat:
                    nextType = IngredientType.Cheese;
                    break;
                case IngredientType.Cheese:
                default:
                    nextType = IngredientType.Vegetable;
                    break;
            }

            
            Ingredient cycledIngredient = new Ingredient(nextType, IngredientState.Raw);
            player.SetHeldIngredient(cycledIngredient);
            Debug.Log($"Swapped for a raw {nextType}");
        }
    }
}