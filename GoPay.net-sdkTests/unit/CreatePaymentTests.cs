using System;
using System.Collections.Generic;
using GoPay.Common;
using GoPay.Model.Payment;
using GoPay.Model.Payments;
using Xunit;

namespace GoPay.Tests

{
    public class CreatePaymentTests
    {
        public static BasePayment createBasePayment()
        {
            var addParams = new List<AdditionalParam>();
            addParams.Add(new AdditionalParam {Name = "AdditionalKey", Value = "AdditionalValue"});

            var addItems = new List<OrderItem>();
            addItems.Add(new OrderItem {Name = "First Item", Amount = 1700, Count = 1});

            var allowedInstruments = new List<PaymentInstrument>();
            allowedInstruments.Add(PaymentInstrument.BANK_ACCOUNT);
            allowedInstruments.Add(PaymentInstrument.PAYMENT_CARD);

            var swifts = new List<string>();
            swifts.Add("GIBACZPX");
            swifts.Add("RZBCCZPP");

            var basePayment = new BasePayment
            {
                Callback = new Callback
                {
                    ReturnUrl = @"https://eshop123.cz/return",
                    NotificationUrl = @"https://eshop123.cz/notify"
                },

                OrderNumber = "4321",
                Amount = 1700,
                Currency = Currency.CZK,
                OrderDescription = "4321Description",

                Lang = "CS",

                AdditionalParams = addParams,

                Items = addItems,

                Target = new Target
                {
                    GoId = TestUtils.GOID,
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

            return basePayment;
        }


        [Fact]
        public void GPConnectorTestCreatePayment()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            var basePayment = createBasePayment();
            try
            {
                var result = connector.GetAppToken().CreatePayment(basePayment);
                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Payment id: {0}", result.Id);
                Console.WriteLine("Payment gw_url: {0}", result.GwUrl);
                Console.WriteLine("Payment instrument: {0}", result.PaymentInstrument);
                Console.WriteLine(result.Payer.Contact);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Create payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPCOnnectorTestPaymentStatis()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            var basePayment = createBasePayment();

            try
            {
                var result = connector.GetAppToken().CreatePayment(basePayment);
                var payment = connector.GetAppToken().PaymentStatus(result.Id);

                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Payment id: {0}", payment.Id);
                Console.WriteLine("Payment state: {0}", payment.State);
                Console.WriteLine("Payment gw_url: {0}", payment.GwUrl);
                Console.WriteLine("Payment instrument: {0}", payment.PaymentInstrument);
                Console.WriteLine(result.Payer.Contact);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Create payment ERROR");
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