using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        
        if (player.HeldIngredient != null)
        {
            Debug.Log($"Threw away {player.HeldIngredient.State} {player.HeldIngredient.Type}");
            player.SetHeldIngredient(null);
        }
        else
        {
            Debug.Log("Hands are already empty, nothing to throw away.");
        }
    }
}