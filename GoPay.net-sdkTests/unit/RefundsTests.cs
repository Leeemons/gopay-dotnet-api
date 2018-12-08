
using System;
using GoPay.Common;
using GoPay.Model.Payments;
using GoPay.Model.Payment;
using System.Collections.Generic;
using Xunit;

namespace GoPay.Tests
{    
    public class RefundsTests
    {

        [Fact]
        public void GPConnectorTestRefund()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            long id = 3049215286;
            try
            {
                var refundResult = connector.GetAppToken().RefundPayment(id, 1000);
                Assert.NotNull(refundResult);
                Assert.NotEqual(0, refundResult.Id);

                Console.WriteLine("Refund with amount result: {0}", refundResult);
            }
            catch (GPClientException exception)
            {
                Console.WriteLine("CHYBA refundu");
                var err = exception.Error;
                DateTime date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //Handle
                }
            }
        }
    }
}
