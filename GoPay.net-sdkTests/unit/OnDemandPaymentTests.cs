using System;
using GoPay.Common;
using GoPay.Model.Payments;
using Xunit;

namespace GoPay.Tests

{
    public class OnDemandPaymentTests
    {
        [Fact]
        public void GPConnectorTestNextOnDemand()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            try
            {
                var nextPayment = new NextPayment
                {
                    OrderNumber = "OnDemand4321",
                    Amount = 4000,
                    Currency = Currency.CZK,
                    OrderDescription = "OnDemand4321Description"
                };
                nextPayment.Items.Add(new OrderItem {Name = "OnDemand First Item", Amount = 2000, Count = 2});

                var onDemandPayment = connector.GetAppToken().CreateRecurrentPayment(3049249957, nextPayment);

                Console.WriteLine("OnDemand payment id: {0}", onDemandPayment.Id);
                Console.WriteLine("OnDemand payment gw_url: {0}", onDemandPayment.GwUrl);
                Console.WriteLine("OnDemand Payment instrument: {0}", onDemandPayment.PaymentInstrument);
                Console.WriteLine("OnDemand recurrence: {0}", onDemandPayment.Recurrence);
                Console.WriteLine("OnDemand amount: {0}", onDemandPayment.Amount);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Creating next on demand payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestOnDemand()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            var basePayment = CreatePaymentTests.createBasePayment();

            var onDemandRecurrence = new Recurrence
            {
                Cycle = RecurrenceCycle.ON_DEMAND,
                DateTo = new DateTime(2018, 4, 1)
            };

            basePayment.Recurrence = onDemandRecurrence;

            try
            {
                var result = connector.GetAppToken().CreatePayment(basePayment);
                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Payment id: {0}", result.Id);
                Console.WriteLine("Payment gw_url: {0}", result.GwUrl);
                Console.WriteLine("Payment instrument: {0}", result.PaymentInstrument);
                Console.WriteLine("Recurrence: {0}", result.Recurrence);
                Console.WriteLine("Payment amount: {0}", result.Amount);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Creating OnDemand payment ERROR");
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