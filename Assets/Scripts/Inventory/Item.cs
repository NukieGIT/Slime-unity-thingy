using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        FlashLight,
        Dash,
        SpeedBoost
    }

    public ItemType itemType;
    public int amount;
}
