using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] WeaponLocation weaponLocationLeft, weaponLocationRight;
    [SerializeField] WeaponRotation weaponRotation;

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
        //Movement
        transform.position += (Vector3)movement * Time.deltaTime * speed;

        weaponRotation.lookLocation = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        weaponRotation.lookLocation.z = 0;
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

        yield return new WaitForSecondsRealtime(.5f);

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
}
