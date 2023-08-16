using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

[RequireComponent(typeof(UIStoreController))]
public class StoreController : MonoBehaviour
{
    [SerializeField] UIStoreController UIStore;
    [SerializeField] ProductSO[] availableProducts;
    private WearablesManager wearablesManager => WearablesManager.Instance;
    private PurchaseManager purchaseManager => PurchaseManager.Instance;
    private List<IProduct> products = new List<IProduct>();
    private IProduct selectedProduct;
    private void Start()
    {
        OpenStore();
    }
    public void BuyProduct()
    {
        purchaseManager.CreatePurchase(selectedProduct);
        UIStore.LaunchPromptToWearNewProduct(selectedProduct);
        wearablesManager.CreateWearable(selectedProduct.ProductInfo.iD);
    }
    public void SelectProduct(ProductSO product)
    {
        UIStore.SetPriceText(product.productPrice);
    }

    public void OpenStore()
    {
        CreateProducts();
        if(products.Count > 0)
        {
            UIStore.BuyButton.onClick.AddListener(BuyProduct);
        }
       
    }
  
    public void CreateProducts()
    {
        foreach (var productInfo in availableProducts)
        {
            GameObject newProduct = UIStore.CreateNewProduct();
            IProduct productCreated;
            newProduct.TryGetComponent<IProduct>(out productCreated);
            if (newProduct == null) continue;

            AddProductInfo(productCreated, productInfo);
            SubscribeToProductEvents(productCreated);
            products.Add(productCreated);
        }

    }

    private void  AddProductInfo(IProduct productCreated, ProductSO productInfo)
    {
        productCreated.FillInfo(productInfo);
    }

    private void SubscribeToProductEvents(IProduct product)
    {
        product.OnSelected += SelectProduct;
    }

    private void SelectProduct(IProduct product)
    {
        if (product == selectedProduct) return;

        if(selectedProduct != null)
        {
            selectedProduct.Deselect(); //Deselect the actual selected product
        }
        selectedProduct = product;
        UIStore.SelectProductUI(product);
    }

}


