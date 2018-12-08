using System;
using System.Collections.Generic;
using GoPay.Common;
using GoPay.EETProp;
using GoPay.Model.Payment;
using GoPay.Model.Payments;
using Xunit;

namespace GoPay.Tests

{
    public class EetTests
    {
        private BasePayment createEETBasePayment()
        {
            var addParams = new List<AdditionalParam>();
            addParams.Add(new AdditionalParam {Name = "AdditionalKey", Value = "AdditionalValue"});

            var addItems = new List<OrderItem>();
            addItems.Add(new OrderItem
            {
                Name = "Pocitac Item1",
                Amount = 119990,
                Count = 1,
                VatRate = VatRate.RATE_4,
                ItemType = ItemType.ITEM,
                Ean = "1234567890123",
                ProductURL = @"https://www.eshop123.cz/pocitac"
            });
            addItems.Add(new OrderItem
            {
                Name = "Oprava Item2",
                Amount = 19960,
                Count = 1,
                VatRate = VatRate.RATE_3,
                ItemType = ItemType.ITEM,
                Ean = "1234567890189",
                ProductURL = @"https://www.eshop123.cz/pocitac/oprava"
            });

            var allowedInstruments = new List<PaymentInstrument>();
            allowedInstruments.Add(PaymentInstrument.BANK_ACCOUNT);
            allowedInstruments.Add(PaymentInstrument.PAYMENT_CARD);

            var swifts = new List<string>();
            swifts.Add("GIBACZPX");
            swifts.Add("RZBCCZPP");

            var baseEETPayment = new BasePayment
            {
                Callback = new Callback
                {
                    ReturnUrl = @"https://eshop123.cz/return",
                    NotificationUrl = @"https://eshop123.cz/notify"
                },

                OrderNumber = "EET4321",
                Amount = 139950,
                Currency = Currency.CZK,
                OrderDescription = "EET4321Description",

                Lang = "CS",

                AdditionalParams = addParams,

                Items = addItems,

                Target = new Target
                {
                    GoId = TestUtils.GOID_EET,
                    Type = Target.TargetType.ACCOUNT
                },

                Payer = new Payer
                {
                    AllowedPaymentInstruments = allowedInstruments,
                    AllowedSwifts = swifts,
                    //DefaultPaymentInstrument = PaymentInstrument.BANK_ACCOUNT,
                    //PaymentInstrument = PaymentInstrument.BANK_ACCOUNT,
                    Contact = new PayerContact
                    {
                        Email = "test@test.gopay.cz"
                    }
                }
            };

            return baseEETPayment;
        }

        private Payment createEETPaymentObject(GPConnector connector, BasePayment baseEETPayment)
        {
            Payment result = null;

            try
            {
                result = connector.GetAppToken().CreatePayment(baseEETPayment);
                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("EET Payment id: {0}", result.Id);
                Console.WriteLine("EET Payment gw_url: {0}", result.GwUrl);
                Console.WriteLine("EET Payment instrument: {0}", result.PaymentInstrument);
                Console.WriteLine(baseEETPayment.Eet);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Create EET payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }

                throw;
            }

            return result;
        }

        [Fact]
        public void GPConnectorTestCreateEETPayment()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);

            var baseEETPayment = createEETBasePayment();

            var eet = new EET
            {
                CelkTrzba = 139950,
                ZaklDan1 = 99165,
                Dan1 = 20825,
                ZaklDan2 = 17357,
                Dan2 = 2603,
                Mena = Currency.CZK
            };

            baseEETPayment.Eet = eet;

            var result = createEETPaymentObject(connector, baseEETPayment);
        }

        [Fact]
        public void GPConnectorTestCreateRecurrentEETPayment()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);

            var baseEETPayment = createEETBasePayment();

            /*
            Recurrence recurrence = new Recurrence()
            {
                Cycle = RecurrenceCycle.WEEK,
                Period = 1,
                DateTo = new DateTime(2018, 4, 1)
            };
            baseEETPayment.Recurrence = recurrence;
            */


            var onDemandRecurrence = new Recurrence
            {
                Cycle = RecurrenceCycle.ON_DEMAND,
                DateTo = DateTime.Today.AddDays(1)
            };
            baseEETPayment.Recurrence = onDemandRecurrence;


            var eet = new EET
            {
                CelkTrzba = 139950,
                ZaklDan1 = 99165,
                Dan1 = 20825,
                ZaklDan2 = 17357,
                Dan2 = 2603,
                Mena = Currency.CZK
            };

            baseEETPayment.Eet = eet;

            var result = createEETPaymentObject(connector, baseEETPayment);
            Console.WriteLine(result.Recurrence);
        }

        [Fact]
        public void GPConnectorTestEETPaymentRefund()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);

            var refundedItems = new List<OrderItem>();
            refundedItems.Add(new OrderItem
            {
                Name = "Pocitac Item1",
                Amount = 119990,
                Count = 1,
                VatRate = VatRate.RATE_4,
                ItemType = ItemType.ITEM,
                Ean = "1234567890123",
                ProductURL = @"https://www.eshop123.cz/pocitac"
            });
            refundedItems.Add(new OrderItem
            {
                Name = "Oprava Item2",
                Amount = 19960,
                Count = 1,
                VatRate = VatRate.RATE_3,
                ItemType = ItemType.ITEM,
                Ean = "1234567890189",
                ProductURL = @"https://www.eshop123.cz/pocitac/oprava"
            });

            var eet = new EET
            {
                CelkTrzba = 139950,
                ZaklDan1 = 99165,
                Dan1 = 20825,
                ZaklDan2 = 17357,
                Dan2 = 2603,
                Mena = Currency.CZK
            };

            var refundObject = new RefundPayment
            {
                Amount = 139950,
                Items = refundedItems,
                Eet = eet
            };

            try
            {
                var refundEETPayment = connector.GetAppToken().RefundPayment(3049250113, refundObject);
                Console.WriteLine("EET refund result: {0}", refundEETPayment);
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("EET Payment refund ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestEETPReceiptFindByFilter()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);

            var filter = new EETReceiptFilter
            {
                DateFrom = new DateTime(2017, 3, 2),
                DateTo = new DateTime(2017, 4, 2),
                IdProvoz = 11
            };

            try
            {
                var receipts = connector.GetAppToken().FindEETReceiptsByFilter(filter);

                foreach (var currReceipt in receipts) Console.WriteLine(currReceipt);
                Console.WriteLine(receipts.Count);
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("EET Receipt by filter ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestEETPReceiptFindByPaymentId()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);

            try
            {
                var receipts = connector.GetAppToken().GetEETReceiptByPaymentId(3049205133);

                foreach (var currReceipt in receipts) Console.WriteLine(currReceipt);
                Console.WriteLine(receipts.Count);
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("EET Receipt by payment ID ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestEETStatus()
        {
            long id = 3049250282;

            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);
            try
            {
                var payment = connector.GetAppToken().PaymentStatus(id);
                Assert.NotEqual(0, payment.Id);

                Console.WriteLine("EET Payment id: {0}", payment.Id);
                Console.WriteLine("EET Payment gw_url: {0}", payment.GwUrl);
                Console.WriteLine("EET Payment state: {0}", payment.State);
                Console.WriteLine("EET Payment instrument: {0}", payment.PaymentInstrument);
                Console.WriteLine("EET PreAuthorization: {0}", payment.PreAuthorization);
                Console.WriteLine("EET Recurrence: {0}", payment.Recurrence);
                Console.WriteLine(payment.EetCode);
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("EET Payment status ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestNextOnDemandEET()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID_EET, TestUtils.CLIENT_SECRET_EET);

            try
            {
                var nextPayment = new NextPayment
                {
                    OrderNumber = "EETOnDemand4321",
                    Amount = 2000,
                    Currency = Currency.CZK,
                    OrderDescription = "EETOnDemand4321Description"
                };
                nextPayment.Items.Add(new OrderItem
                {
                    Name = "OnDemand Prodlouzena zaruka",
                    Amount = 2000,
                    Count = 1,
                    VatRate = VatRate.RATE_4,
                    ItemType = ItemType.ITEM,
                    Ean = "12345678901234",
                    ProductURL = @"https://www.eshop123.cz/pocitac/prodlouzena_zaruka"
                });

                var eet = new EET
                {
                    CelkTrzba = 2000,
                    ZaklDan1 = 1580,
                    Dan1 = 420,
                    Mena = Currency.CZK
                };
                nextPayment.Eet = eet;

                var onDemandEETPayment = connector.GetAppToken().CreateRecurrentPayment(3049250282, nextPayment);

                Console.WriteLine("OnDemand payment id: {0}", onDemandEETPayment.Id);
                Console.WriteLine("OnDemand payment gw_url: {0}", onDemandEETPayment.GwUrl);
                Console.WriteLine("OnDemand EET Payment instrument: {0}", onDemandEETPayment.PaymentInstrument);
                Console.WriteLine("OnDemand recurrence: {0}", onDemandEETPayment.Recurrence);
                Console.WriteLine("OnDemand amount: {0}", onDemandEETPayment.Amount);
                Console.Write(onDemandEETPayment.EetCode);
                Console.WriteLine(nextPayment.Eet);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Creating next on demand EET payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }
    }
}