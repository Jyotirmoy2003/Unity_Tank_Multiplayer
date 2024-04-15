using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

public static class AuthenticationWrapper 
{
    public static AuthState authState{get;private set;} = AuthState.NotAuthenticated;
    public static async Task<AuthState> DoAuth(int MaxTries=5)
    {
        if(authState==AuthState.Authenticated) return authState;


        authState=AuthState.Authenticting;
        int tries=0;

        while(authState==AuthState.Authenticting && tries<MaxTries)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            //check if authentication is successfull
            if(AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
            {
                authState=AuthState.Authenticated;
                break;
            }

            tries++;
            await Task.Delay(1000);
        }

        return authState;
       
    }

}

public enum AuthState{
    NotAuthenticated,
    Authenticting,
    Authenticated,
    Error,
    TimeOut
}
