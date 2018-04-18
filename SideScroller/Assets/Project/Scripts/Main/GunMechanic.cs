using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMechanic : MonoBehaviour {

    // sounds
    private AudioSource aSource;

    public AudioClip gunShotSound;
    public AudioClip chargeSound;

    private const float MAX_DISTANCE_FROM_GUN = 0.35f;

    public int shotForceValue = 10;
    private int shotForce = 10;

    [HideInInspector]
    public int shotsAvailable = 3;

    [HideInInspector]
    public int dashesAvailable = 1;

    public float shotRecharge;
    public float dashRecharge;

    [SerializeField]
    private float shotRechargeRate = 2f;

    public float dashRechargeRate = 5f;

    public float shotRefresh;
    public float shotRefreshRate = 1f;

    [HideInInspector]
    public float dashTime = 0;
    public bool dashing = false;

    private float gravityTimeScale = 1.4f;

    [HideInInspector]
    public bool canShoot = true;
    [HideInInspector]
    public bool canDash = true;

    private bool canCharge = true;

    private bool shooting = false;
    private bool isShooting = false;
    private bool inDash = false;
    private bool isDashing = false;

    private bool startDashTime = false;

    public bool dashIsCounting = false;


    public Transform direction;

    private Rigidbody playerRigidbody;

    private Vector3 offset;

    public ZeroGravity zeroGravity;

    public LookAt lookAt;

    public PlayerMove playerMove;

    public float ShotRechargeRate
    {
        get { return shotRechargeRate; }
    }

    void Start()
    {
        shotRecharge = 0;
        playerRigidbody = GetComponent<Rigidbody>();
        aSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // If game isn't paused
        if (!PauseMenu.isPaused)
        {
            // Allows player to shoot
            Shoot();
            // Allows player to dash
            if (Pickups.isUpgraded)
            {
                Dash();
            }

        }
    }

    void Shoot()
    {
        // When in the zero gravity zone shot force is halfed
        if (zeroGravity.inZeroGravityZone)
        {
            shotForce = shotForceValue / 2;
        }
        else
        {
            shotForce = shotForceValue;
        }

        // If the gun is neither full nor out of shots then reload individual shots
        if (shotsAvailable < 3 && shotsAvailable > 0)
        {
            if (canCharge)
            {
                if (zeroGravity.inZeroGravityZone)
                {
                    shotRefresh += gravityTimeScale * Time.deltaTime;
                }
                else
                {
                    shotRefresh += Time.deltaTime;
                }
            }
        }
        else
        {
            shotRefresh = 0f;
        }

        if (shotRefresh >= shotRefreshRate)
        {
            shotsAvailable++;
            shotRefresh = 0;
        }

        if (shotsAvailable == 0)
        {
            canShoot = false;
        }

        // If gun is out of shots then start full reload
        if (!canShoot && shotsAvailable == 0)
        {
            if (canCharge)
            {
                if (zeroGravity.inZeroGravityZone)
                {
                    shotRecharge += gravityTimeScale * Time.deltaTime;
                }
                else
                {
                    shotRecharge += Time.deltaTime;
                }
            }
        }

        if (shotRecharge >= shotRechargeRate)
        {
            canShoot = true;
            shotRecharge = 0;
            shotsAvailable = 3;
        }


        if (Input.GetAxis("Trigger") == -1 && !shooting)
        {
            isShooting = true;
        }

        if (Input.GetAxis("Trigger") == -1 && shooting)
        {
            isShooting = false;
        }

        if (Input.GetAxis("Trigger") > -0.2)
        {
            shooting = false;
        }

        // Shoot
        if (lookAt.controller)
        {
            if (Input.GetAxis("Trigger") == -1 && canShoot && isShooting)
            {
                if (playerRigidbody.mass != 1.0f)
                {
                    playerRigidbody.mass = 1.0f;
                }
                shooting = true;
                shotsAvailable--;
                shotRefresh = 0f;
                ApplyForce();

                if (gunShotSound)
                {
                    aSource.clip = gunShotSound;
                    aSource.PlayOneShot(gunShotSound, 0.5f);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Shoot") && canShoot)
            {
                if (playerRigidbody.mass != 1.0f)
                {
                    playerRigidbody.mass = 1.0f;
                }
                shooting = true;
                shotsAvailable--;
                shotRefresh = 0f;
                ApplyForce();

                if (gunShotSound)
                {
                    aSource.clip = gunShotSound;
                    aSource.PlayOneShot(gunShotSound, 0.5f);
                }
            }
        }
    }

    void Dash()
    {
        // If upgrade has been picked up then the player gains the ability to dash
        if (Pickups.isUpgraded)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }

        // Checks if the player has a dash available
        if (dashesAvailable == 0)
        {
            canDash = false;
        }

        // Dash gets reloaded
        if (!canDash && dashesAvailable == 0)
        {
            dashIsCounting = true;
            if (zeroGravity.inZeroGravityZone)
            {
                dashRecharge += gravityTimeScale * Time.deltaTime;
            }
            else
            {
                dashRecharge += Time.deltaTime;
            }
        }

        if (dashRecharge >= dashRechargeRate)
        {
            canDash = true;
            dashRecharge = 0;
            dashesAvailable = 1;
        }

        if (lookAt.controller)
        {
            if (Input.GetAxis("Trigger") == 1f && !isDashing)
            {
                inDash = true;
            }

            if (Input.GetAxis("Trigger") == 1f && isDashing)
            {
                inDash = false;
            }

            if (Input.GetAxis("Trigger") < 0.2)
            {
                isDashing = false;
            }

            if (Input.GetAxis("Trigger") == 1f && inDash && canDash)
            {
                startDashTime = true;
                if (playerMove.grounded)
                    playerRigidbody.mass = 0.55f;

                dashTime = 0;
                dashing = true;
                isDashing = true;
                if (dashesAvailable > 0)
                {
                    if (zeroGravity.inZeroGravityZone)
                    {
                        shotForce = (shotForceValue / 2) * 3;
                    }
                    else
                    {
                        shotForce = shotForceValue * 3;
                    }
                    dashesAvailable--;
                    DashForce();

                }
                canCharge = true;
                canDash = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Dash") && canDash)
            {
                startDashTime = true;
                if (playerMove.grounded)
                    playerRigidbody.mass = 0.55f;

                dashTime = 0;
                dashing = true;
                if (dashesAvailable > 0)
                {
                    if (zeroGravity.inZeroGravityZone)
                    {
                        shotForce = (shotForceValue / 2) * 3;
                    }
                    else
                    {
                        shotForce = shotForceValue * 3;
                    }
                    dashesAvailable--;
                    DashForce();

                }
                canCharge = true;
                canDash = true;
            }
        }

        if (startDashTime)
        {
            if (zeroGravity.inZeroGravityZone)
            {
                dashTime += gravityTimeScale * Time.deltaTime;
            }
            else
            {
                dashTime += Time.deltaTime;
            }
        }

        if (dashTime >= 0.2f)
        {
            if (playerRigidbody.mass != 1.0f)
            {
                playerRigidbody.mass = 1.0f;
            }

            dashIsCounting = false;
            startDashTime = false;
            dashTime = 0;
            dashing = false;
            playerRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        shotForce = shotForceValue;
    }

    void ApplyForce()
    {
        playerRigidbody.velocity = Vector3.zero;

        if (Mathf.Sign(Physics.gravity.y) == -1)
        {
            offset = new Vector3(0, 1, 0);
        }
        else
        {
            offset = new Vector3(0, -1, 0);
        }

        Vector3 forceDirection = ((transform.position + offset) - direction.position).normalized;

        forceDirection.x = Mathf.Clamp(forceDirection.x, -MAX_DISTANCE_FROM_GUN, MAX_DISTANCE_FROM_GUN);
        forceDirection.y = Mathf.Sign(forceDirection.y) * MAX_DISTANCE_FROM_GUN;

        playerRigidbody.AddForce(forceDirection * shotForce);
    }

    void DashForce()
    {
        playerRigidbody.velocity = Vector3.zero;
        offset = new Vector3(0, 1, 0);
        Vector3 forceDirection = ((transform.position + offset) - direction.position).normalized;

        forceDirection.x = Mathf.Clamp(forceDirection.x, -MAX_DISTANCE_FROM_GUN, MAX_DISTANCE_FROM_GUN);
        forceDirection.y = 0;

        playerRigidbody.AddForce(forceDirection * shotForce);
    }
}