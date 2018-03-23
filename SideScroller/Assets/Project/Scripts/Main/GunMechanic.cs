using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMechanic : MonoBehaviour {

    private const float MAX_DISTANCE_FROM_GUN = 0.35f;

    public int shotForceValue = 10;
    private int shotForce = 10;

    [HideInInspector]
    public int shotsAvailable = 3;

    [HideInInspector]
    public int dashesAvailable = 1;

    [HideInInspector]
    public float chargeUpTime;

    private float chargeUpNeeded = 1f;
    private float maxChargeTime = 2f;

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

    public bool charging = false;

    public Transform direction;

    private Rigidbody playerRigidbody;

    private Vector3 offset;

    public ZeroGravity zeroGravity;

    public float ShotRechargeRate
    {
        get { return shotRechargeRate; }
    }

    void Start()
    {
        shotRecharge = 0;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // If game isn't paused
        if (!PauseMenu.isPaused)
        {
            // Allows player to shoot
            Shoot();
            // Allows player to dash
            Dash();
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

        // If upgrade has been picked up then the player gains the ability to dash
        if (Pickups.isUpgraded)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
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

        // Shoot
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            shotsAvailable--;
            maxChargeTime = 0f;
            shotRefresh = 0f;
            ApplyForce();
        }
    }

    void Dash()
    {
        // Checks if the player has a dash available
        if (dashesAvailable == 0)
        {
            canDash = false;
        }

        // Dash gets reloaded
        if (!canDash && dashesAvailable == 0)
        {
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

        if (Input.GetMouseButtonDown(1))
        {
            maxChargeTime = 2f;
        }

        if (Input.GetMouseButton(1) && canDash)
        {
            canCharge = false;
            charging = true;
            if (zeroGravity.inZeroGravityZone)
            {
                chargeUpTime += gravityTimeScale * Time.deltaTime;
            }
            else
            {
                chargeUpTime += Time.deltaTime;
            }

            if (chargeUpTime >= maxChargeTime)
            {
                playerRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                charging = false;
                dashesAvailable = 0;
            }
            else
            {
                playerRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }
        else
        {
            if (zeroGravity.inZeroGravityZone)
            {
                dashTime += gravityTimeScale * Time.deltaTime;
            }
            else
            {
                dashTime += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(1))
            {
                dashTime = 0;
                dashing = true;
                charging = false;
                if (chargeUpTime >= chargeUpNeeded && dashesAvailable > 0)
                {
                    playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                    if (zeroGravity.inZeroGravityZone)
                    {
                        shotForce = (shotForceValue / 2) * 3;
                    }
                    else
                    {
                        shotForce = shotForceValue * 3;
                    }
                    chargeUpTime = 0;
                    dashesAvailable--;
                    DashForce();
                }
                else
                {
                    dashesAvailable = 0;
                }
                canCharge = true;
                canDash = true;
            }

            if (dashTime >= 0.2f)
            {
                dashTime = 0;
                dashing = false;
                playerRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            shotForce = shotForceValue;
            chargeUpTime = 0;
        }
    }

    void ApplyForce()
    {
        playerRigidbody.velocity = Vector3.zero;
        offset = new Vector3(0, 1, 0);
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