using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

// Handle moves logic for a player entitity in network
public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEntity playerEntity;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject tokenPrefabServer;
     [SerializeField] private GameObject tokenPrefabClient;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float fallSpeed = 4f;

    private Vector2 previousMovementInput;
    private List<GameObject> currentTokens = new List<GameObject>();
    private Material mat;
    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return; }

        rb.velocity = (Vector2)transform.right * previousMovementInput.x * movementSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return; }
        previousMovementInput = context.ReadValue<Vector2>();
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return; }
        if(!playerEntity.IsTurn.Value) return;

        if(context.performed)
        {
            SoundManager.GetInstance().Play(AudioSFX.DROP);
            currentTokens.Clear();
            // Client call the Server to say "i'm a client and i want to drop a token here"
            DropServerRpc(); 
            DropDummyToken();
        }      
    }

    [ServerRpc]
    // From client to Server
    // The Server will instantiate a token who will collide with the board
    // Server Side the board will get updated (or not) 
    private void DropServerRpc()
    {
        GameObject currentToken = Instantiate(tokenPrefabServer, transform.position, Quaternion.identity, GameManagerNetwork.Instance.gameObject.transform);
        currentToken.GetComponent<Rigidbody>().velocity = new Vector3(0f, -fallSpeed, 0f);    
        currentToken.GetComponent<TokenVisual>().OwnerId = OwnerClientId;

        currentTokens.Add(currentToken);
        //Server calls all Clients to also drop a dummy on their side (including himself)
        DropDummyClientRpc();
    }

    [ClientRpc]
    // From Server to Clients
    private void DropDummyClientRpc()
    {
        //Do not spawn another dummy token if you are the one who called the function
        if (IsOwner) { return; }

        DropDummyToken();
    }

    // To avoid waiting time and visual lag, we also drop a "dummy" token on the client
    // This dummy is only visual, but follow same position as the token on the server
    private void DropDummyToken()
    {
        GameObject currentToken = Instantiate(tokenPrefabClient, transform.position, Quaternion.identity, GameManagerNetwork.Instance.gameObject.transform);
        currentToken.GetComponent<Rigidbody>().velocity = new Vector3(0f, -fallSpeed, 0f);    
        if(mat == null) mat = GetComponent<MeshRenderer>().material;
        currentToken.GetComponent<MeshRenderer>().material = mat;
        currentTokens.Add(currentToken);
    }

    public void DestroyUnvalidTokens()
    {
        foreach(GameObject token in currentTokens)
        {
            Destroy(token);
        }
    }
}
