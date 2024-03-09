using System.Threading.Tasks;
using UnityEngine;

// Based on code from the online course "Make Online Games Using Unity's NEW Multiplayer Framework"
// GameDev.tv
public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode();
    }

    private async Task LaunchInMode()
    {
        HostSingleton hostSingleton = Instantiate(hostPrefab);
        hostSingleton.CreateHost();

        ClientSingleton clientSingleton = Instantiate(clientPrefab);
        bool authenticated = await clientSingleton.CreateClient();

        if (authenticated)
        {
            clientSingleton.GameManager.GoToMenu();
        }
        
    }
}
