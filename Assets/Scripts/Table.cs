using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour, IInteractable
{
    private enum StationState { Empty, Chopping, Finished }
    private StationState currentState = StationState.Empty;

    [Header("Settings")]
    public float chopTime = 2.0f;
    private float currentChopTimer = 0f;
    private Ingredient currentIngredient;

    [Header("UI & Visuals")]
    public Slider progressBar;
    public Transform itemPoint; 
    public GameObject rawVegetablePrefab;
    public GameObject choppedVegetablePrefab;

    private GameObject currentVisual;

    private void Start()
    {
        if (progressBar != null) progressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentState == StationState.Chopping)
        {
            currentChopTimer += Time.deltaTime;

            if (progressBar != null)
            {
                progressBar.value = currentChopTimer / chopTime;
            }

            if (currentChopTimer >= chopTime)
            {
                FinishChopping();
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (currentState == StationState.Empty)
        {
            if (player.HeldIngredient != null &&
                player.HeldIngredient.Type == IngredientType.Vegetable &&
                player.HeldIngredient.State == IngredientState.Raw)
            {
                currentIngredient = player.HeldIngredient;
                player.SetHeldIngredient(null);

                currentState = StationState.Chopping;
                currentChopTimer = 0f;

                if (progressBar != null)
                {
                    progressBar.gameObject.SetActive(true);
                    progressBar.value = 0f;
                }

                
                if (rawVegetablePrefab != null && itemPoint != null)
                {
                    currentVisual = Instantiate(rawVegetablePrefab, itemPoint.position, itemPoint.rotation, itemPoint);
                }
            }
        }
        else if (currentState == StationState.Finished)
        {
            if (player.HeldIngredient == null)
            {
                player.SetHeldIngredient(currentIngredient);
                currentIngredient = null;
                currentState = StationState.Empty;

                if (progressBar != null) progressBar.gameObject.SetActive(false);

                
                if (currentVisual != null) Destroy(currentVisual);
            }
        }
    }

    private void FinishChopping()
    {
        currentIngredient.State = IngredientState.Chopped;
        currentState = StationState.Finished;

        
        if (currentVisual != null) Destroy(currentVisual);

        if (choppedVegetablePrefab != null && itemPoint != null)
        {
            currentVisual = Instantiate(choppedVegetablePrefab, itemPoint.position, itemPoint.rotation, itemPoint);
        }
    }
}