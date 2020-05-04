using DwarfEngine;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CharacterResource : CharacterAbility, IProgressSource
{
    public IntReactiveProperty currentResource;
    public Subject<float> currentResourceNormalized { get; private set; }

    [Header("Properties")]
    public string resourceName;
    public int maximumResource;

    [Header("Resource Bar")]
    public bool hasResourceBar;
    public string _targetBar;
    public Vector3 _barOffset;
    public string targetBar => _targetBar;
    public Vector3 barOffset => _barOffset;

    private void Start()
    {
        currentResource = new IntReactiveProperty(maximumResource);
        currentResourceNormalized = new Subject<float>();

        currentResource
            .Select(resource => (float)resource / maximumResource)
            .Subscribe(currentResourceNormalized)
            .AddTo(this);

        if (hasResourceBar)
        {
            UIManager.Instance.SetProgressBar(gameObject, this, currentResourceNormalized); // Set up the resource bar
            currentResourceNormalized.OnNext(currentResource.Value); // Set resource bar value for the first time 
        }
    }

    public bool Consume(int consumeAmount)
    {
        if (!CheckResource(consumeAmount))
        {
            return false;
        }
        else
        {
            currentResource.Value -= consumeAmount;
            return true;
        }
    }
    
    public bool CheckResource(int requestedAmount)
    {
        return (currentResource.Value - requestedAmount) >= 0;
    }

    public bool IsFull()
    {
        return currentResource.Value == maximumResource;
    }
}
