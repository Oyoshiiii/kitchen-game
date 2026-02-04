using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipesSOArray;

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    private int cuttingProgress;

    /// <summary>
    /// принимает объект игрока и осуществляет механику взаимодействия с тумбочками (положить/взять объект)
    /// </summary>
    /// <param name="player"> игрок </param>
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //на тумбе ничего не лежит
            if (player.HasKitchenObject())
            {
                //у игрока есть в руках объект
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalized = 0
                    });
                }
            }
        }
        else
        {
            // на тумбе уже лежит объект
            if (player.HasKitchenObject())
            {
                //у игрока есть объект в руках
            }
            else
            {
                //у игрока ничего нет
                GetKitchenObject().SetKitchenObjectParent(player);

                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
            }
        }
    }

    /// <summary>
    /// принимает объект игрока и осуществляет механику нарезки
    /// </summary>
    /// <param name="player"> игрок </param>
    public override void InteractAlternative(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO cutKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
            }
        }
    }

    /// <summary>
    /// принимает объект для нарезки и достает из рецепта результат нарезки
    /// </summary>
    /// <param name="inputKitchenObjectSO"> объект для нарезки </param>
    /// <returns> нарезанная версия </returns>
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO.output;
    }

    /// <summary>
    /// принимает объект для нарезки и достает из рецепта результат нарезки
    /// </summary>
    /// <param name="inputKitchenObjectSO"> объект для нарезки </param>
    /// <returns> есть ли у объекта рецепт </returns>
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    /// <summary>
    /// принимает объект для нарезки и проверяет наличие его в каком-либо из рецептов массива
    /// </summary>
    /// <param name="inputKitchenObjectSO"> объект для нарезки </param>
    /// <returns> рецепт </returns>
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipesSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
