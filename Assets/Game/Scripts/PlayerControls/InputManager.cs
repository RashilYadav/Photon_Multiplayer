using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerMovement playerMovement;

    public Vector2 movementInput;
    public Vector2 cameraMovementInput;

    public float verticalInput;
    public float horizontalInput;
    public float cameraInputX;
    public float cameraInputY;

    public float movementAmount;

    [Header("Input Buttons Flag")]
    public bool bInput;
    public bool jumpInput;
    public bool fireInput;
    public bool reloadInput;
    public bool scopeInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            // PlayerMovement is the actionMap that we created, Movement is the sction name that we gave
            // When the player performs a movement input (like pressing a key or moving a joystick), update the movementInput variable with the corresponding vector value of that input.
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraMovement.performed += i => cameraMovementInput = i.ReadValue<Vector2>();
            // When the player performs the 'B' action (like pressing a specific button), set the bInput variable to true.
            playerControls.PlayerActions.B.performed += i => bInput = true;
            playerControls.PlayerActions.B.canceled += i => bInput = false;

            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Fire.performed += i => fireInput = true;
            playerControls.PlayerActions.Fire.canceled += i => fireInput = false;
            playerControls.PlayerActions.Reload.performed += i => reloadInput = true;
            playerControls.PlayerActions.Scope.performed += i => scopeInput = true;
            playerControls.PlayerActions.Scope.canceled += i => scopeInput = false;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraMovementInput.x;
        cameraInputY = cameraMovementInput.y;

        movementAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.ChangeAnimatorValues(0, movementAmount, playerMovement.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if(bInput && movementAmount > 0.5)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if(jumpInput)
        {
            jumpInput = false;
            playerMovement.HandleJumping();
        }
    }
}
