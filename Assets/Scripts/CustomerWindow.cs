using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomerWindow : MonoBehaviour, IInteractable
{
    public Order CurrentOrder { get; private set; }
    [Header("UI References")]
    public TextMeshProUGUI timerText; 
    public Transform iconContainer;
    public GameObject iconPrefab;
    public GameObject scorePopupPrefab;

    [Header("Ingredient Icons")]
    public Sprite vegetableSprite;
    public Sprite meatSprite;
    public Sprite cheeseSprite;

    private float respawnTimer = 0f;
    private bool isWaitingForNewOrder = false;
    private const float RESPAWN_DELAY = 5f;

    
    public void ResetWindow()
    {
        CurrentOrder = new Order(Time.time);
        isWaitingForNewOrder = false;
        respawnTimer = 0f;
        UpdateUI();
    }

    private void Update()
    {
        
        if (GameManager.Instance == null || !GameManager.Instance.isGameActive) return;

        if (isWaitingForNewOrder)
        {
            respawnTimer -= Time.deltaTime;
            UpdateUI();

            if (respawnTimer <= 0)
            {
                GenerateNewOrder();
            }
        }
        else if (CurrentOrder != null)
        {
            UpdateUI();
        }
    }

    private void GenerateNewOrder()
    {
        CurrentOrder = new Order(Time.time);
        isWaitingForNewOrder = false;
        UpdateUI();
    }

    public void Interact(PlayerController player)
    {
        
        if (GameManager.Instance == null || !GameManager.Instance.isGameActive) return;
        if (isWaitingForNewOrder || CurrentOrder == null) return;

        Ingredient held = player.HeldIngredient;
        if (held != null)
        {
            if (!IsIngredientPrepared(held))
            {
                Debug.Log($"{held.Type} is not fully prepared yet!");
                return;
            }

            if (CurrentOrder.TryDeliverIngredient(held.Type))
            {
                player.SetHeldIngredient(null);
                UpdateUI();

                if (CurrentOrder.IsComplete())
                {
                    CompleteOrder();
                }
            }
        }
    }

    private bool IsIngredientPrepared(Ingredient ingredient)
    {
        if (ingredient.Type == IngredientType.Vegetable && ingredient.State == IngredientState.Chopped) return true;
        if (ingredient.Type == IngredientType.Meat && ingredient.State == IngredientState.Cooked) return true;
        if (ingredient.Type == IngredientType.Cheese && ingredient.State == IngredientState.Raw) return true;
        return false;
    }

    private void CompleteOrder()
    {
        
        int finalScore = GameManager.Instance.AddScoreForOrder(CurrentOrder, Time.time - CurrentOrder.SpawnTime);

        
        if (scorePopupPrefab != null)
        {
            GameObject popup = Instantiate(scorePopupPrefab, transform.position + (Vector3.up * 1.5f), Quaternion.identity);
            popup.GetComponent<ScorePopup>().Setup(finalScore);
        }

        CurrentOrder = null;
        isWaitingForNewOrder = true;
        respawnTimer = RESPAWN_DELAY;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText == null) return;

        
        if (iconContainer != null)
        {
            foreach (Transform child in iconContainer)
            {
                Destroy(child.gameObject);
            }
        }

        if (isWaitingForNewOrder)
        {
            timerText.text = $"Next order in:\n{respawnTimer:F1}s";
        }
        else if (CurrentOrder != null)
        {
            float activeTime = Time.time - CurrentOrder.SpawnTime;
            timerText.text = $"Time: {activeTime:F1}s";

            
            if (iconContainer != null && iconPrefab != null)
            {
                foreach (var item in CurrentOrder.PendingIngredients)
                {
                    GameObject newIcon = Instantiate(iconPrefab, iconContainer);
                    Image iconImage = newIcon.GetComponent<Image>();

                    if (item == IngredientType.Vegetable) iconImage.sprite = vegetableSprite;
                    else if (item == IngredientType.Meat) iconImage.sprite = meatSprite;
                    else if (item == IngredientType.Cheese) iconImage.sprite = cheeseSprite;
                }
            }
        }
    }
}
