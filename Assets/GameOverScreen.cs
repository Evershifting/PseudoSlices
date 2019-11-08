using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
   public void Restart()
    {
        EventsManager.Broadcast(EventsType.RestartGame);
    }
}
