using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        // Only throw away if the player is actually holding something
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