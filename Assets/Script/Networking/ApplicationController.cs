using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;

    async void Start()
    {
        DontDestroyOnLoad(gameObject);
       await LunchInMode(SystemInfo.graphicsDeviceType==UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    async Task LunchInMode(bool IsDedicatedServer)
    {
        if (IsDedicatedServer)
        {
            
        }else{

            HostSingleton hostSingleton = Instantiate(hostPrefab);
            hostSingleton.CreateHost();
      
            ClientSingleton clientSingleton = Instantiate(clientPrefab);
            bool authenticated=await clientSingleton.CreateClient();

            


            if(authenticated)
            {
                clientSingleton.GameManager.GoToMenu();
            }

        }
    }
  
}
