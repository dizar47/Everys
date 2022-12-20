using Everys.BL.Services;

namespace Everys.Tests
{
    [TestClass]
    public class StockServiceTests
    {
        [TestMethod]
        public async Task StockServiceTests_Success()
        {
            var stockService = new StockService();

            var response = await stockService.GetStockAsync("Nam tempor diam dictum");

            Assert.AreNotEqual(0, response.Items.Length);
            Assert.AreEqual(6, response.Items.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void StockServiceTests_Failure()
        {
            var stockService = new StockService();

            var tasks = Enumerable.Range(0, 100).Select(async x => await stockService.GetStockAsync("Nam tempor diam dictum " + x.ToString()));

            Task.WaitAll(tasks.ToArray());
        }
    }
}