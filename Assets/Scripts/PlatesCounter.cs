using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    private int platesSpawnAmount;
    private int platesSpawnAmountMax = 4;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if(platesSpawnAmount < platesSpawnAmountMax)
            {
                platesSpawnAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if(platesSpawnAmount > 0)
            {
                platesSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
