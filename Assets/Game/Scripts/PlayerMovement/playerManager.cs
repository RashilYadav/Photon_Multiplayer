using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerManager : MonoBehaviour
{
    InputManager inputManager;

    PlayerMovement playerMovement;
    CameraManager cameraManager;

    Animator animator;
    public bool isInteracting;

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Start()
    {
        if(!view.IsMine)
        {
            Destroy(GetComponentInChildren<CameraManager>().gameObject); // if the player is not ours then the camera of the other player will be destroyed
        }
    }

    private void Update()
    {
        if (!view.IsMine) 
        {
            return;
        }
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        if(!view.IsMine) // if the player is not ours then we dont want it to move with our keyboard and mouse
        {
            return;
        }
        playerMovement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if (!view.IsMine)
        {
            return;
        }

        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
