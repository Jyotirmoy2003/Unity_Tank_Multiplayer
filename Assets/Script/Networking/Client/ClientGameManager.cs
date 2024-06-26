using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager 
{
    private const string MenuSceneName="Menu";
    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();;

        AuthState authState=await AuthenticationWrapper.DoAuth();

        if(authState==AuthState.Authenticated)
        {
            return true;
        }else{
            return false;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

}
