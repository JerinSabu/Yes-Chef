using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private CharacterController controller;
    private PlayerControls inputActions;
    private Vector2 moveInput;

    [Header("Animation")]
    private Animator animator;
    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int isHoldingHash = Animator.StringToHash("IsHolding");

    [Header("Interaction")]
    public float interactionRadius = 1.5f;
    public LayerMask interactableLayer;

    public Ingredient HeldIngredient { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        
        inputActions = new PlayerControls();

        
        inputActions.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (move.magnitude >= 0.1f)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(move);
            animator.SetBool(isWalkingHash, true);
        }
        else
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    private void TryInteract()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer);

        float closestDistance = Mathf.Infinity;
        IInteractable closestInteractable = null;

        foreach (var hit in hitColliders)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        closestInteractable?.Interact(this);
    }

    public void SetHeldIngredient(Ingredient ingredient)
    {
        HeldIngredient = ingredient;
        animator.SetBool(isHoldingHash, HeldIngredient != null);
    }
}