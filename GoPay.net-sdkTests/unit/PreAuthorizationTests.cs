using System;
using Xunit;
using System;
using GoPay.Model.Payments;
using GoPay.Model.Payment;

namespace GoPay.Tests

{
    public class PreAuthorizationTests
    {
        [Fact]
        public void GPConnectorTestCapturePayment()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            long id = 3049249190;
            try
            {
                var result = connector.GetAppToken().CapturePayment(id);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Capture payment result: {0}", result);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Capture payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //Handle
                }
            }
        }

        [Fact]
        public void GPConnectorTestCreatePreAuthorizedPayment()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            var basePayment = CreatePaymentTests.createBasePayment();
            basePayment.PreAuthorization = true;

            try
            {
                var result = connector.GetAppToken().CreatePayment(basePayment);
                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Payment id: {0}", result.Id);
                Console.WriteLine("Payment gw_url: {0}", result.GwUrl);
                Console.WriteLine("Payment instrument: {0}", result.PaymentInstrument);
                Console.WriteLine("PreAuthorization: {0}", result.PreAuthorization);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("PreAuthorized payment ERROR");
                var err = exception.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestVoidAuthorization()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            long id = 3049249125;
            try
            {
                var result = connector.GetAppToken().VoidAuthorization(id);
                Assert.NotEqual(0, result.Id);

                Console.WriteLine("Void Authorization result: {0}", result);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("Void authorization ERROR");
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