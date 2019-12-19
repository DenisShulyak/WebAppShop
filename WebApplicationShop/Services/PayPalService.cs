using BraintreeHttp;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationShop.Models;

namespace WebApplicationShop.Services
{
    public class PayPalService
    {
        private const string APPROVAL_URL = "approval_url";

        public async Task<string> CreateInvoice(Basket basket)
        {
            var environment = new SandboxEnvironment("AZTu0aTctY3TsQRanLBGIjRYVhzo7rc25etnkVduypxV38zDdRja0Z6_adpN7nakww62w667wNh4_OKT", "EPT6TcCPEuAbNrCatN0_FyrWFTGtO6-1c77lhSj_pMrIx3o2V09BnpZnhLe3CfGO0wtW0IULHGI4yrGc");
            var client = new PayPalHttpClient(environment);

            int totalPrice = 0;
            foreach (var product in basket.Products)
            {
                totalPrice += product.Cost;
            }

            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        ItemList = new ItemList(),
                        Amount = new Amount()
                        {
                            Total = totalPrice.ToString(),
                            Currency = "USD"
                        }
                    }

                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = "https://example.com/cancel",
                    ReturnUrl = "https://example.com/return"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                HttpResponse response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();
                return result.Links.FirstOrDefault(link => link.Rel == APPROVAL_URL).Href;
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                return null;
            }
        }
    }
}
