using UnityEngine;
using UnityEngine.UI;

public class Stove : MonoBehaviour, IInteractable
{
    private enum SlotState { Empty, Cooking, Finished }

    private class StoveSlot
    {
        public SlotState State = SlotState.Empty;
        public float Timer = 0f;
        public Ingredient CurrentIngredient = null;
        public Slider ProgressBar;

        
        public Transform Point;
        public GameObject CurrentVisual;
    }

    [Header("Settings")]
    public float cookTime = 6.0f;

    [Header("UI & Visuals")]
    public Slider slot1ProgressBar;
    public Slider slot2ProgressBar;
    public Transform slot1Point;
    public Transform slot2Point;
    public GameObject rawMeatPrefab;
    public GameObject cookedMeatPrefab;

    private StoveSlot[] slots = new StoveSlot[2];

    private void Awake()
    {
        
        slots[0] = new StoveSlot { ProgressBar = slot1ProgressBar, Point = slot1Point };
        slots[1] = new StoveSlot { ProgressBar = slot2ProgressBar, Point = slot2Point };

        if (slot1ProgressBar != null) slot1ProgressBar.gameObject.SetActive(false);
        if (slot2ProgressBar != null) slot2ProgressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (var slot in slots)
        {
            if (slot.State == SlotState.Cooking)
            {
                slot.Timer += Time.deltaTime;

                if (slot.ProgressBar != null)
                {
                    slot.ProgressBar.value = slot.Timer / cookTime;
                }

                if (slot.Timer >= cookTime)
                {
                    slot.CurrentIngredient.State = IngredientState.Cooked;
                    slot.State = SlotState.Finished;

                    
                    if (slot.CurrentVisual != null) Destroy(slot.CurrentVisual);
                    if (cookedMeatPrefab != null && slot.Point != null)
                    {
                        slot.CurrentVisual = Instantiate(cookedMeatPrefab, slot.Point.position, slot.Point.rotation, slot.Point);
                    }
                }
            }
        }
    }

    public void Interact(PlayerController player)
    {
        
        if (player.HeldIngredient == null)
        {
            foreach (var slot in slots)
            {
                if (slot.State == SlotState.Finished)
                {
                    player.SetHeldIngredient(slot.CurrentIngredient);
                    slot.CurrentIngredient = null;
                    slot.State = SlotState.Empty;

                    if (slot.ProgressBar != null) slot.ProgressBar.gameObject.SetActive(false);

                    
                    if (slot.CurrentVisual != null) Destroy(slot.CurrentVisual);
                    return;
                }
            }
        }

        
        if (player.HeldIngredient != null &&
            player.HeldIngredient.Type == IngredientType.Meat &&
            player.HeldIngredient.State == IngredientState.Raw)
        {
            foreach (var slot in slots)
            {
                if (slot.State == SlotState.Empty)
                {
                    slot.CurrentIngredient = player.HeldIngredient;
                    player.SetHeldIngredient(null);

                    slot.State = SlotState.Cooking;
                    slot.Timer = 0f;

                    if (slot.ProgressBar != null)
                    {
                        slot.ProgressBar.gameObject.SetActive(true);
                        slot.ProgressBar.value = 0f;
                    }

                    
                    if (rawMeatPrefab != null && slot.Point != null)
                    {
                        slot.CurrentVisual = Instantiate(rawMeatPrefab, slot.Point.position, slot.Point.rotation, slot.Point);
                    }
                    return;
                }
            }
        }
    }
}