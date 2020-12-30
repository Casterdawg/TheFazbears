using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Climbing : State
{
    private PlayerStateController privatePlayer;

    public override void Enter(PlayerStateController player)
    {
        Debug.Log("Ledge found");
        player.jumpCount = 0;
        privatePlayer = player;

        player.masterInput.Player.LetGo.performed += ReleaseLedge;

        //Could add method to make sure you are perfectly parrele with the ledge. Don't know how to do that yet
    }

    public override void Update(PlayerStateController player)
    {
        //If ledge shuffeling is added, then add checks for it here. I don't think it is needed though
    }

    public override void Exit(PlayerStateController player)
    {
        player.masterInput.Player.LetGo.performed -= ReleaseLedge;
    }

    private void ReleaseLedge(InputAction.CallbackContext context)
    {
        privatePlayer.SetState(new Airborn());
    }
}
