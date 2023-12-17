using System;
using UnityEngine;

public class PData : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PGrounded grounded;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private GameObject backpackPickaxe;
    
    
    
    private static string PickaxeObtainedKey = "pickaxeObtained";
    private static string PlayerPositionKey = "playerPosition";
    
    private Vector2 _playerPosition;

    private void Awake()
    {
        LoadPlayerPosition();
    }

    private void Start()
    {
        bool pickaxeObtained = false;
        try
        {
            pickaxeObtained = ES3.Load(PickaxeObtainedKey, false);
        }
        catch (Exception e)
        {
            // ignored
        }
        
        Debug.Log($"Pickaxe obtained: {pickaxeObtained}");
        
        if (pickaxeObtained)
            EnablePickaxe();
        else
            DisablePickaxe();
    }

    private void OnEnable()
    {
        PauseManager.OnMainMenuButton += MainMenuSavePlayerPosition;
        Application.wantsToQuit += QuitSavePlayerPosition;
        grounded.OnGroundedChanged += GroundedSavePlayerPosition;
        ObtainPickaxe.OnPickaxeObtained += EnablePickaxe;
    }

    private void OnDisable()
    {
        PauseManager.OnMainMenuButton -= MainMenuSavePlayerPosition;
        Application.wantsToQuit -= QuitSavePlayerPosition;
        grounded.OnGroundedChanged -= GroundedSavePlayerPosition;
        ObtainPickaxe.OnPickaxeObtained -= EnablePickaxe;
    }

    private void LateUpdate()
    {
        _playerPosition = playerRb.position;
    }

    private void EnablePickaxe()
    {
        Debug.Log("Pickaxe enabled");
        pickaxe.enabled = true;
        backpackPickaxe.SetActive(true);
    }
    private void DisablePickaxe()
    {
        Debug.Log("Pickaxe disabled");
        pickaxe.enabled = false;
        backpackPickaxe.SetActive(false);
    }
    private void GroundedSavePlayerPosition(bool wasGrounded, bool isGrounded)
    {
        SavePlayerPosition();
    }
    private bool QuitSavePlayerPosition()
    {
        if(grounded.IsGrounded)
            SavePlayerPosition();
        return true;
    }
    private void MainMenuSavePlayerPosition()
    {
        if(grounded.IsGrounded)
            SavePlayerPosition();
    }
    private void SavePlayerPosition()
    {
        ES3.Save(PlayerPositionKey, _playerPosition);
    }
    private void LoadPlayerPosition()
    {
        try
        {
            Vector2 pos = ES3.Load<Vector2>(PlayerPositionKey);
            playerRb.position = pos;
            playerRb.transform.position = pos;
        }
        catch (Exception e)
        {
            // ignored
        }

        playerRb.velocity = Vector2.zero;
    }
}
