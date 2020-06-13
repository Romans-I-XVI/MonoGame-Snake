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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Snake
{
	public class AmazonUpgrade : Upgrade
	{
		private const string SKU = "com.romansixvigaming.snake.noads";
		private IAmazonIapV2 amazonIapV2;

		public AmazonUpgrade() {
			this.amazonIapV2 = AmazonIapV2Impl.Instance;
			this.amazonIapV2.SetCurrentAndroidActivity(Game.Activity);
			this.amazonIapV2.AddPurchaseResponseListener(this.PurchaseResponseCallback);
			this.amazonIapV2.AddGetPurchaseUpdatesResponseListener(this.GetPurchaseUpdatesResponseCallback);

			this.CheckUpgrade();
		}

		public override void DoUpgrade() {
			var sku = new SkuInput {
				Sku = AmazonUpgrade.SKU
			};
			this.amazonIapV2.Purchase(sku);
		}

		private void CheckUpgrade() {
			var reset_input = new ResetInput();
			reset_input.Reset = true;
			this.amazonIapV2.GetPurchaseUpdates(reset_input);
			this.FetchingIsUpgraded = true;
		}

		private void PurchaseResponseCallback(PurchaseResponse event_name) {
			if (event_name.PurchaseReceipt != null) {
				if (event_name.PurchaseReceipt.Sku == AmazonUpgrade.SKU) {
					if (event_name.Status == "SUCCESSFUL" && event_name.PurchaseReceipt.CancelDate == 0)
						Upgrade.IsUpgraded = true;
					else
						Upgrade.IsUpgraded = false;
					this.NotifyFullfillment(event_name.PurchaseReceipt, true);
				} else {
					this.NotifyFullfillment(event_name.PurchaseReceipt, false);
				}
			}
		}

		private void GetPurchaseUpdatesResponseCallback(GetPurchaseUpdatesResponse event_name) {
			if (event_name.Receipts.Count > 0) {
				bool found_active_purchase = false;
				foreach (var item in event_name.Receipts)
					if (item.Sku == AmazonUpgrade.SKU) {
						if (item.CancelDate == 0)
							found_active_purchase = true;
						this.NotifyFullfillment(item, true);
					} else {
						this.NotifyFullfillment(item, false);
					}

				Upgrade.IsUpgraded = found_active_purchase;
			} else {
				Upgrade.IsUpgraded = false;
			}
			this.FetchingIsUpgraded = false;
		}

		private void NotifyFullfillment(PurchaseReceipt receipt, bool fulfilled) {
			var notify_fulfillment = new NotifyFulfillmentInput {
				ReceiptId = receipt.ReceiptId,
				FulfillmentResult = fulfilled ? "FULFILLED" : "UNAVAILABLE"
			};
			this.amazonIapV2.NotifyFulfillment(notify_fulfillment);
		}
	}
}
#endif