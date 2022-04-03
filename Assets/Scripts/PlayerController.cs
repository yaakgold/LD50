using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] WeaponLocation weaponLocationLeft, weaponLocationRight;
    [SerializeField] WeaponRotation weaponRotation;
    [SerializeField] SpriteRenderer gfx;

    Vector2 movement;

    private void Start()
    {
        if(TryGetComponent(out Health health))
        {
            health.Death.AddListener(OnDeath);
            health.TookDamage.AddListener(UpdateHealthUI);
            health.Healed.AddListener(UpdateHealthUI);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int roomWidth = RoomManager.Instance.GetRoomWidth();
        int roomHeight = RoomManager.Instance.GetRoomHeight();

        //Movement
        transform.position += (Vector3)movement * Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        Vector3 clampedPos = new Vector3 (
            Mathf.Clamp(transform.position.x, 
                    roomWidth * GameManager.Instance.currentRoom.coord.x - roomWidth * .5f, 
                    roomWidth * GameManager.Instance.currentRoom.coord.x + roomWidth * .5f),
            Mathf.Clamp(transform.position.y, 
                    roomHeight * GameManager.Instance.currentRoom.coord.y - roomHeight * .5f, 
                    roomHeight * GameManager.Instance.currentRoom.coord.y + roomHeight * .5f),
            transform.position.z);

        transform.position = clampedPos;
        
        if(movement.magnitude > 0)
        {
            GetComponentInChildren<Wobble>().doTheWobble = true;
        }
        else
        {
            GetComponentInChildren<Wobble>().doTheWobble = false;
        }

        weaponRotation.lookLocation = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        weaponRotation.lookLocation.z = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Money") && GameManager.Instance.playerInteraction)
        {
            Destroy(collision.gameObject);
            GameManager.Instance.AddMoney();
        }
    }

    #region Input
    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        weaponLocationLeft.Fire();
    }

    public void OnFire1(InputValue value)
    {
        weaponLocationRight.Fire();
    }

    public void OnInteract(InputValue value)
    {
        StartCoroutine(Interact());
    }

    private IEnumerator Interact()
    {
        GameManager.Instance.playerInteraction = true;

        yield return new WaitForSecondsRealtime(.15f);

        GameManager.Instance.playerInteraction = false;
    }
    #endregion

    public void OnDeath()
    {
        GameManager.Instance.EndGame();
    }

    public void UpdateHealthUI()
    {
        GameManager.Instance.gameUI.UpdateHealthText(GetComponent<Health>().currentHealth);
    }

    public void UpdateAge(Sprite spr)
    {
        gfx.sprite = spr;
    }

    public void SetLeftWeapon(GameObject wpn)
    {
        if (weaponLocationLeft.GetWeapon() != null)
        {
            Destroy(weaponLocationLeft.GetWeapon().gameObject);
        }

        var w = Instantiate(wpn, Vector3.zero, weaponLocationLeft.transform.rotation, weaponLocationLeft.transform);
        w.transform.localPosition = Vector3.zero;
        if(w.TryGetComponent(out Weapon weapon))
        {
            weaponLocationLeft.SetWeapon(weapon);
        }
    }

    public void SetRightWeapon(GameObject wpn)
    {
        if(weaponLocationRight.GetWeapon() != null)
        {
            Destroy(weaponLocationRight.GetWeapon().gameObject);
        }

        var w = Instantiate(wpn, Vector3.zero, weaponLocationRight.transform.rotation, weaponLocationRight.transform);
        w.transform.localPosition = Vector3.zero;
        if (w.TryGetComponent(out Weapon weapon))
        {
            weaponLocationRight.SetWeapon(weapon);
        }
    }
}
