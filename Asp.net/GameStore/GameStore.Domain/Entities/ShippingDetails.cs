using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class ShippingDetails
    {
        public string WMI_MERCHANT_ID { get; set; } = "119839267508";
        public string WMI_PAYMENT_AMOUNT { get; set; }
        public string WMI_CURRENCY_ID { get; set; } = "643";
        public string WMI_DESCRIPTION { get; set; } = "Описание заказа";
        public string WMI_SUCCESS_URL { get; set; } = "http://morda566-001-site1.gtempurl.com/";
        public string WMI_FAIL_URL { get; set; } = "http://morda566-001-site1.gtempurl.com/Шутер";

        // public string WMI_CUSTOMER_EMAIL { get; set; }
        //public string WMI_EXPIRED_DATE { get; set; }
        // public string WMI_PTENABLED { get; set; }
        //public string TestCardRUB { get; set; }
        //public int WMI_SIGNATURE { get; set; }

        /*
                public string CreateSignature()
                {
                    string merchantKey = "6a776947643964677c544350437c4b5e30506a7b49737377517448";

                    SortedDictionary<string, string> formField
                  = new SortedDictionary<string, string>();

                    formField.Add("WMI_PAYMENT_AMOUNT", String.Format("{0:C2}", WMI_PAYMENT_AMOUNT));
                    formField.Add("WMI_DESCRIPTION", WMI_DESCRIPTION);
                    formField.Add("WMI_CUSTOMER_EMAIL", WMI_CUSTOMER_EMAIL);
                    formField.Add("WMI_MERCHANT_ID", WMI_MERCHANT_ID.ToString());
                    formField.Add("WMI_CURRENCY_ID", WMI_CURRENCY_ID.ToString());
                    formField.Add("WMI_EXPIRED_DATE", DateTime.Now.ToString()); 
                    formField.Add("WMI_SUCCESS_URL", WMI_SUCCESS_URL);
                    formField.Add("WMI_FAIL_URL", WMI_FAIL_URL);
                    formField.Add("WMI_PTENABLED", WMI_PTENABLED);

                    StringBuilder signatureData = new StringBuilder();

                    foreach (string key in formField.Keys)
                    {
                        signatureData.Append(formField[key]);
                    }

                    string message = signatureData.ToString() + merchantKey;
                    Byte[] bytes = Encoding.GetEncoding(1251).GetBytes(message);
                    Byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
                    string signature = Convert.ToBase64String(hash);

                    return signature;
                }
                */
    }
}
