using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [Header("Stats & Attributes")]
    [SerializeField] float atkRange;
    [SerializeField] int atkDmg;
    [SerializeField] float atkCooldown;



    [SerializeField] float speed = 4f;
    [SerializeField] Attack attack;

    Vector2 facingDirection;
    public static PlayerControls playerInstance;
    bool cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInstance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        facingDirection = context.action.ReadValue<Vector2>();
        float xaxis = facingDirection.x;
        float yaxis = facingDirection.y;
        int direction = 0;
        if(Mathf.Abs(xaxis) > Mathf.Abs(yaxis))
        {
            direction = xaxis > 0 ? 3 : 2;
        } 
        else 
        {
            direction = yaxis > 0 ? 1 : 0;
        }
        anim.SetInteger("Direction", direction);
        rb.linearVelocity = speed * facingDirection;
    }

    // TODO: Link up player input with using the fairy item
    public void OnHeal(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        // Try to find a fairy item that is a child of the player
        // If found call the relevant function from FairyItem.cs
    }

    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        // Try to find a bomb item that is a child of the player
        // If found call the relevant function from BombItem.cs
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!cooldown)
        {
            Vector3 pt = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (Vector2)pt - (Vector2)transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
            Quaternion facingAngle = Quaternion.Euler(0, 0, angle);
            Attack a = Instantiate(attack, transform.position, facingAngle);
            a.SetDamage(atkDmg,gameObject);
            cooldown = true;
            Invoke("ResetCooldown", atkCooldown);
        }
    }


    void ResetCooldown()
    {
        cooldown=false;
    }
}
