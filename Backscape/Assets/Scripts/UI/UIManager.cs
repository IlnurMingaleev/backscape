using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private void Awake()
    {
        Cursor.visible = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        startServerButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            
        });

        startHostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            
        });
        startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            
            
        });
    }

    // Update is called once per frame
    void Update()
    {
        //playersInGameText.text = $"PLayers in game: {PlayersManager.Instance.PlayersInGame}";

    }
}
