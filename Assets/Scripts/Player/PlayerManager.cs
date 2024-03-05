using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int playerHealth;
    public int playerStress;
    public int playerHunger;
    public int playerStamina;

    public PlayerData(int playerHealth, int playerStress, int playerHunger, int playerStamina)
    {
        this.playerHealth = playerHealth;
        this.playerStress = playerStress;
        this.playerHunger = playerHunger;
        this.playerStamina = playerStamina;
    }
}

public class PlayerManager : MonoBehaviour
{

}
