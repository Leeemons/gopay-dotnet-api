using System;
using System.Text;
using GoPay.Account;
using GoPay.Common;
using Xunit;

namespace GoPay.Tests
{
    public class CommonMethodTests
    {
        [Fact]
        public void GPConnectorTest()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            connector.GetAppToken();

            Console.WriteLine("Token expires in: {0}", connector.AccessToken.ExpiresIn);

            Assert.NotNull(connector.AccessToken);
            Assert.NotNull(connector.AccessToken.Token);
        }

        [Fact]
        public void GPConnectorTestPaymentInstrumentRoot()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            try
            {
                var instrumentsList = connector.GetAppToken().GetPaymentInstruments(TestUtils.GOID, Currency.CZK);
                Assert.NotNull(instrumentsList);

                Console.WriteLine("List of enabled payment instruments for shop with go_id: {0} - OK", TestUtils.GOID);
                Console.WriteLine(instrumentsList.ToString());
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("List of enabled payment instruments ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }

        [Fact]
        public void GPConnectorTestStatementGenerating()
        {
            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);

            var accountStatement = new AccountStatement
            {
                DateFrom = new DateTime(2017, 1, 1),
                DateTo = new DateTime(2017, 2, 27),
                GoID = TestUtils.GOID,
                Currency = Currency.CZK,
                Format = StatementGeneratingFormat.CSV_A
            };

            try
            {
                var statement = connector.GetAppToken().GetStatement(accountStatement);
                Assert.NotNull(statement);

                var content = Encoding.UTF8.GetString(statement);

                Console.WriteLine("Content of Array to string: {0}", content);
                Console.WriteLine(
                    "----------------------------------------------------------------------------------------");

                Console.Write("Byte content: ");
                for (var i = 0; i < statement.Length; i++) Console.Write(" {0}", statement[i]);
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("Generating account statement ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }


        [Fact]
        public void GPConnectorTestStatus()
        {
            long id = 3049249619;

            var connector = new GPConnector(TestUtils.API_URL, TestUtils.CLIENT_ID, TestUtils.CLIENT_SECRET);
            try
            {
                var payment = connector.GetAppToken().PaymentStatus(id);
                Assert.NotEqual(0, payment.Id);

                Console.WriteLine("Payment id: {0}", payment.Id);
                Console.WriteLine("Payment gw_url: {0}", payment.GwUrl);
                Console.WriteLine("Payment state: {0}", payment.State);
                Console.WriteLine("Payment instrument: {0}", payment.PaymentInstrument);
                Console.WriteLine("PreAuthorization: {0}", payment.PreAuthorization);
                Console.WriteLine("Recurrence: {0}", payment.Recurrence);
            }
            catch (GPClientException ex)
            {
                Console.WriteLine("Payment status ERROR");
                var err = ex.Error;
                var date = err.DateIssued;
                foreach (var element in err.ErrorMessages)
                {
                    //
                }
            }
        }
    }
}