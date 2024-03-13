using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootingController : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    PlayerMovement playerMovement;

    [Header("Shooting Var")]
    public Transform firePoint;
    public float fireRate = 0f;
    public float fireRange = 100f;
    public float fireDamage = 15;
    private float nextFireTime = 0f;

    [Header("Reloading")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1.5f;

    [Header("Shooting Flags")]
    public bool isShooting;
    public bool isWalking;
    public bool isShootingInput;
    public bool isReloading = false;
    public bool isScopeInput;

    [Header("Sound Effects")]
    public AudioSource soundAudioSource;
    public AudioClip shootingSoundClip;
    public AudioClip reloadingSoundClip;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem bloodEffect;

    PhotonView view;

    public int playerTeam;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        currentAmmo = maxAmmo;

        if(view.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)view.Owner.CustomProperties["Team"];
            playerTeam = team;
        }
    }

    private void Update()
    {
        if (!view.IsMine)
            return;

        if(isReloading || playerMovement.isSprinting)
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            return;
        }

        isWalking = playerMovement.isMoving;
        isShootingInput = inputManager.fireInput;
        isScopeInput = inputManager.scopeInput;

        if(isShootingInput && isWalking)
        {
            if(Time.time >= nextFireTime)
            {
            nextFireTime = Time.time + 1f / fireRate; // delay shooting a bit
            Shoot();
            animator.SetBool("ShootWalk", true);
            }

            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", true);
            isShooting = true;
        }

        else if (isShootingInput)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate; // delay shooting a bit
                Shoot();
            }

            animator.SetBool("Shoot", true);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            isShooting = true;
        }

        else if(isScopeInput)
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", true);
            animator.SetBool("ShootWalk", false);
            isShooting = false;
        }

        else
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            isShooting = false;
        }

        if(inputManager.reloadInput && currentAmmo < maxAmmo)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if(currentAmmo > 0)
        {
            RaycastHit hit;
            if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
            {
                Debug.Log(hit.transform.name);

                // Extract hit transformation
                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                // Apply damage to the players
                PlayerMovement playerMovementDamage = hit.collider.GetComponent<PlayerMovement>();

                // playerMovementDamage.playerTeam != playerTeam - yeh line yeh bolti hai ki playerMovement script anusar agar
                // playerTeam 1 hai toh shootingController ke hisaab se bhi 1 hi hogi obviously matlab same team ke players ki
                // baat ho rhi hai toh agar same team ke members hai toh damage apply nhi karenge par agar hai toh karna hai!!
                if (playerMovementDamage != null && playerMovementDamage.playerTeam != playerTeam)
                {
                    // apply damage
                    playerMovementDamage.ApplyDamage(fireDamage);
                    view.RPC("RPC_Shoot", RpcTarget.All, hitPoint, hitNormal);
                }
            }

            // play muzzleflash effect
            muzzleFlash.Play();
            soundAudioSource.PlayOneShot(shootingSoundClip);
            currentAmmo--;
        }
        else
        {
            Reload();
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPoint, Vector3 hitNormal)
    {
        // going to show the blood effect on the player
        ParticleSystem blood = Instantiate(bloodEffect, hitPoint, Quaternion.LookRotation(hitNormal));
        Debug.Log("Blood effect is enabled");
        Destroy(blood.gameObject, blood.main.duration);
    }

    private void Reload()
    {
        if(!isReloading && currentAmmo < maxAmmo)
        {
            if (isShootingInput && isWalking)
            {
                animator.SetTrigger("ShootReload");
            }
            else
            {
                animator.SetTrigger("Reload");
            }

            isReloading = true;
            soundAudioSource.PlayOneShot(reloadingSoundClip);

            Invoke("FinishReloading", reloadTime); // invokes a method in time mentioned by us
        }
    }

    private void FinishReloading()
    {
        currentAmmo = maxAmmo;
        isReloading = false;

        if (isShootingInput && isWalking)
        {
            animator.ResetTrigger("ShootReload"); // reset (deactivate) a trigger parameter named "ShootReload."
        }
        else
        {
            animator.ResetTrigger("Reload");
        }
    }
}
