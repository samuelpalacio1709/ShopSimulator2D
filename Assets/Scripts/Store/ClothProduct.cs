using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UIProductController))]
public abstract class ClothProduct : MonoBehaviour, IProduct
{
    public Action<IProduct> onSelected;
    [SerializeField] ProductSO productInfo;
    [SerializeField] Button button;
    [SerializeField] UIProductController UIProductController;
    public ProductSO ProductInfo { 
        get => productInfo;
    }
    public GameObject canvasObject => this.gameObject;
    public Action<IProduct> OnSelected
    {
        get => onSelected;
        set => onSelected = value;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Select);
    }

    public void Select()
    {
        Debug.Log("Selected");
        OnSelected?.Invoke(this);
        UIProductController.ShowBorder(true);
    }
    public void Deselect()
    {
        Debug.Log("Deselected");
        UIProductController.ShowBorder(false);

    }

    public void FillInfo(ProductSO productInfo)
    {
        this.productInfo=productInfo;
        UIProductController.UpdateUI(productInfo.productIcon);

    }

    
    public abstract void TryProduct();

}
