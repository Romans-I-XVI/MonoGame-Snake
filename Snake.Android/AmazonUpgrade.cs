#if ADS && AMAZON
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using com.amazon.device.iap.cpt;
using Microsoft.Xna.Framework.Input.Touch;

namespace Snake
{
    public class AmazonUpgrade : Upgrade
    {
        private const string SKU = "com.romansixvigaming.snake.noads";

        private bool _checked_with_server = false;

        private IAmazonIapV2 amazonIapV2;

        public AmazonUpgrade()
        {
            amazonIapV2 = AmazonIapV2Impl.Instance;
            amazonIapV2.SetCurrentAndroidActivity(Microsoft.Xna.Framework.Game.Activity);
            this.amazonIapV2.AddPurchaseResponseListener(purchaseResponseCallback);
            this.amazonIapV2.AddGetPurchaseUpdatesResponseListener(getPurchaseUpdatesResponseCallback);

            checkUpgrade();
        }

        public override void onKeyDown(KeyboardEventArgs e)
        {
            base.onKeyDown(e);
            if (e.Key == Keys.Help)
            {
                if (!IsUpgraded)
                {
                    DoUpgrade();
                }
            }
        }

        public override void DoUpgrade()
        {
            SkuInput sku = new SkuInput();
            sku.Sku = SKU;
            amazonIapV2.Purchase(sku);
        }

        private void checkUpgrade()
        {
            if (!IsUpgraded && !_checked_with_server)
            {
                checkServerForUpgrade();
                _checked_with_server = true;
            }
        }

        private void checkSaveForUpgrade()
        {

        }

        private void checkServerForUpgrade()
        {
            ResetInput reset_input = new ResetInput();
            reset_input.Reset = true;
            amazonIapV2.GetPurchaseUpdates(reset_input);
        }

        private void purchaseResponseCallback(PurchaseResponse eventName)
        {
            if (eventName.PurchaseReceipt != null)
            {
                if (eventName.PurchaseReceipt.Sku == SKU)
                {
                    if (eventName.Status == "SUCCESSFUL" && eventName.PurchaseReceipt.CancelDate == 0)
                    {
                        IsUpgraded = true;
                    }
                    else
                    {
                        IsUpgraded = false;
                    }
                    notifyFullfillment(eventName.PurchaseReceipt, true);
                }
                else
                {
                    notifyFullfillment(eventName.PurchaseReceipt, false);
                }
            }
        }

        private void getPurchaseUpdatesResponseCallback(GetPurchaseUpdatesResponse eventName)
        {
            if (eventName.Receipts.Count > 0)
            {
                bool found_active_purchase = false;
                foreach (var item in eventName.Receipts)
                {
                    if (item.Sku == SKU)
                    {
                        if (item.CancelDate == 0)
                        {
                            found_active_purchase = true;
                        }
                        notifyFullfillment(item, true);
                    }
                    else
                    {
                        notifyFullfillment(item, false);
                    }
                }
                IsUpgraded = found_active_purchase;
            }
            else
            {
                IsUpgraded = false;
            }
        }

        private void notifyFullfillment(PurchaseReceipt receipt, bool fulfilled)
        {
            NotifyFulfillmentInput notify_fulfillment = new NotifyFulfillmentInput();
            notify_fulfillment.ReceiptId = receipt.ReceiptId;
            notify_fulfillment.FulfillmentResult = fulfilled ? "FULFILLED" : "UNAVAILABLE";
            amazonIapV2.NotifyFulfillment(notify_fulfillment);
        }
    }
}
#endif