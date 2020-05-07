using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Scripts
{
    [AddComponentMenu("")]
    public class NetworkManagerMorse : NetworkManager
    {
        public Transform Helper;
        public Transform Prisoner;
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? Helper : Prisoner;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);

            if (numPlayers == 2)
            {
                
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
    }
}
