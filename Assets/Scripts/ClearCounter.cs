using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //на тумбе ничего не лежит
            if (player.HasKitchenObject())
            {
                //у игрока есть в руках объект
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // на тумбе уже лежит объект
            if(player.HasKitchenObject())
            {
                //у игрока есть объект в руках
            }
            else
            {
                //у игрока ничего нет
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
