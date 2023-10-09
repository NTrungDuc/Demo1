using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Input
    bool iJump;
    bool Swap1;
    bool Swap2;
    bool Swap3;
    bool getItem;
    bool fDown;

    bool isSwap = false;
    private bool isJump = false;
    private bool isDodge = false;
    bool isFireReady =true;
    
    //Grenade
    [SerializeField] private GameObject[] grenades;
    public int hasGrenades;

    //infor player
    public int ammo;
    public int coin;
    public int heart;

    public int maxAmmo;
    public int maxCoin;
    public int maxHeart;
    public int maxHasGrenades;

    //movement
    Vector3 moveVec;
    [SerializeField] private float speed;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private Constant Constant;
    
    [SerializeField] private float jumpForce;

    //interation item
    GameObject nearObject;
    Weapons equipWeapon;
    int equipWeaponIndex = -1;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private bool[] hasWeapons;
    float fireDelay;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        movement();
        Jump();
        Attack();
        Interation();
        Swap();
        Dodge();
    }
    private void GetInput()
    {
        iJump = Input.GetKeyUp(KeyCode.Space);
        Swap1 = Input.GetKeyUp(KeyCode.Alpha1);
        Swap2 = Input.GetKeyUp(KeyCode.Alpha2);
        Swap3 = Input.GetKeyUp(KeyCode.Alpha3);
        getItem = Input.GetKeyUp(KeyCode.F);
        fDown = Input.GetMouseButtonDown(0);
    }
    private void movement()
    {
        if (!isSwap && isFireReady)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h != 0f || v != 0f)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
                animator.SetBool(Constant.ANIM_RUN, true);
            }
            if (h == 0 & v == 0)
            {
                animator.SetBool(Constant.ANIM_RUN, false);
            }

            moveVec = new Vector3(h, 0, v) * speed;
            
        }
        else if(isSwap || !isFireReady)
        {
            moveVec = Vector3.zero;
        }
        rb.velocity = new Vector3(moveVec.x, rb.velocity.y, moveVec.z);
    }
    private void Jump()
    {
        if(iJump && moveVec == Vector3.zero && !isJump && !isSwap && !isDodge)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(Constant.ANIM_JUMP);
            isJump=true;
        }
    }
    private void Attack()
    {
        if (equipWeapon == null)
            return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            animator.SetTrigger(equipWeapon.type == Weapons.Type.Melle ? Constant.ANIM_SWING : Constant.ANIM_SHOT);
            fireDelay = 0;
        }
    }
    private void Dodge()
    {
        if (iJump && moveVec != Vector3.zero && !isJump && !isSwap && !isDodge)
        {
            speed *= 2;
            animator.SetTrigger(Constant.ANIM_DODGE);
            isDodge=true;
            Invoke("outDodge", 0.5f);
        }
    }
    private void outDodge()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    private void Swap()
    {
        if (Swap1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (Swap2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (Swap3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;
        int weaponIndex = -1;
        if (Swap1) weaponIndex = 0;
        if (Swap2) weaponIndex = 1;
        if (Swap3) weaponIndex = 2;
        if((Swap1 || Swap2 ||Swap3) && !isJump && !isSwap)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapons>();
            equipWeapon.gameObject.SetActive(true);
            isSwap = true;
            animator.SetTrigger(Constant.ANIM_SWAP);
            Invoke("outSwap", 0.4f);
        }
    }
    private void outSwap()
    {
        isSwap=false;
    }
    private void Interation()
    {
        if(getItem && nearObject!=null && !isJump)
        {
            if (nearObject.gameObject.tag == "Weapon")
            {
                Item item=nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    public void takeDamage(int damage)
    {
        heart -= damage;
        if(heart <= 0)
        {
            //die
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item=other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin) coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    heart += item.value;
                    if (heart > maxHeart) heart = maxHeart;
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades) hasGrenades = maxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject=other.gameObject;
        }   
    }
    private void OnTriggerExit(Collider other)
    {
        nearObject=null;
    }
}
