using System;
using GoPay.Model.Payments;
using Xunit;

namespace GoPay.Tests
{
    public class RecurrentPaymentTests
    {
        [Fact]
        public void GPConnectorTestCreateRecurrentPayment()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            var basePayment = CreatePaymentTests.createBasePayment();

            var recurrence = new Recurrence
            {
                Cycle = RecurrenceCycle.WEEK,
                Period = 1,
                DateTo = new DateTime(2018, 4, 1)
            };

            basePayment.Recurrence = recurrence;

            try
            {
                var result = connector.GetAppToken().CreatePayment(basePayment);
                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Payment id: {0}", result.Id);
                Console.WriteLine("Payment gw_url: {0}", result.GwUrl);
                Console.WriteLine("Payment instrument: {0}", result.PaymentInstrument);
                Console.WriteLine("Recurrence: {0}", result.Recurrence);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Recurrent payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }


        [Fact]
        public void GPConnectorTestVoidRecurrency()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            long id = 3049249619;
            try
            {
                var result = connector.GetAppToken().VoidRecurrency(id);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Void Recurrency result: {0}", result);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Void recurrency ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //Handle
                }
            }
        }
    }
}