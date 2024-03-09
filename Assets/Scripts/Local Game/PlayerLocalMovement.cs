using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handle movement for local player entity
public class PlayerLocalMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject tokenPrefab;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float fallSpeed = 4f;

    private Vector2 previousMovementInput;
    private bool moveAllowed;
    
    public bool MoveAllowed { 
        get => moveAllowed;
        set
        { 
            moveAllowed = value;
            GetComponent<Player>().SetVisible(moveAllowed);
        }  
    }
    public GameObject CurrentToken;


    private void FixedUpdate()
    {
        rb.velocity = (Vector2)transform.right * previousMovementInput.x * movementSpeed;
    }

    public void OnMove(Vector2 input)
    {
        if(!MoveAllowed) return;
        previousMovementInput = input;
    }

    public void OnDrop(bool isIA)
    {
        if(!MoveAllowed && !isIA) return;
        MoveAllowed = false; 

        CurrentToken = Instantiate(tokenPrefab, transform.position, Quaternion.identity);
        CurrentToken.GetComponent<Rigidbody>().velocity = new Vector3(0f, -fallSpeed, 0f);    
    }
}
