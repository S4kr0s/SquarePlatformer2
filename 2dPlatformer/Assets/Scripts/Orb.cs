using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private OrbType orbType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            switch (orbType)
            {
                case OrbType.jump:
                    playerMovement.AddMidAirJumps(1);
                    break;
                case OrbType.dash:
                    playerMovement.AddMidAirDashs(1);
                    break;
                default:
                    return;
            }

            Destroy(this.gameObject);
        }
    }
}

public enum OrbType
{
    jump,
    dash
}
