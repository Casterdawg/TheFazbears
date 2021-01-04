using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Climbing : State
{
    private PlayerStateController privatePlayer;

    //When the player enters the climb state, reset the players jump count and add an input listener
    public override void Enter(PlayerStateController player)
    {
        Debug.Log("Ledge found");
        player.jumpCount = 0;
        privatePlayer = player;

        player.masterInput.Player.LetGo.performed += ReleaseLedge;

        //Could add method to make sure you are perfectly parallel with the ledge. I don't know how to do that yet though
    }

    public override void Update(PlayerStateController player)
    {
        //If ledge shuffeling is added, then add checks for it here. I don't think it is needed though
    }

    //When exiting the climb state, disable the ReleaseLedge listener
    public override void Exit(PlayerStateController player)
    {
        player.masterInput.Player.LetGo.performed -= ReleaseLedge;
    }

    //When the player presses the S key, have the player fall and enter the airborn state
    private void ReleaseLedge(InputAction.CallbackContext context)
    {
        privatePlayer.SetState(new Airborn());
    }
}
