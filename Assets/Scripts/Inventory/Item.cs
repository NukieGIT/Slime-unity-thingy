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

    public Color GetColor()
    {
        switch (itemType)
        {
            case ItemType.FlashLight: return ItemAssets.instance.flashLight;
            case ItemType.Dash: return ItemAssets.instance.dash;
            case ItemType.SpeedBoost: return ItemAssets.instance.speedBoost;
        }
        return ItemAssets.instance.none;
    }

}
