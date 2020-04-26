using DwarfEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterResource : CharacterAbility
{
    public string resourceName;
    public int maxResource;

    private int currentResource;

    public bool Consume(int consumeAmount)
    {
        if (!CheckResource(consumeAmount))
        {
            return false;
        }
        else
        {
            currentResource -= consumeAmount;
            return true;
        }
    }
    
    public bool CheckResource(int requestedAmount)
    {
        return (currentResource - requestedAmount) >= 0;
    }
}
