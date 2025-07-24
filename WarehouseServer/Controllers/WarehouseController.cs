using Microsoft.AspNetCore.Mvc;
using WarehouseModel;
using WarehouseModel.Contracts;
using WarehouseServer.Interfaces;

namespace WarehouseServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IWarehouseDbController _dbController;

        public WarehouseController(ILogger<WarehouseController> logger, IWarehouseDbController dbController)
        {
            _logger = logger;
            _dbController = dbController;
        }

        [HttpPost]
        [Route(WarehouseControllerRoutes.AddClientOrGetVersion)]
        public async Task<IActionResult> AddClientOrGetVersion(
            [FromQuery] string clientId,
            CancellationToken cancellationToken)
        {
            int result;
            try
            {
                result = await _dbController.AddClientOrGetVersion(clientId, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route(WarehouseControllerRoutes.AddWarehouseRecord)]
        public async Task<IActionResult> AddWarehouseRecord(
            [FromQuery] string clientId,
            [FromQuery] string recordId,
            [FromQuery] int recordCount,
            CancellationToken cancellationToken)
        {
            var record = new WarehouseRecord(recordId, recordCount);
            try
            {
                await _dbController.AddWarehouseRecord(clientId, record, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpGet]
        [Route(WarehouseControllerRoutes.GetAllWarehouseRecords)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            List<WarehouseRecord> result; 
            try
            {
                result = await _dbController.GetAll(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
            return Ok(result);
        }
    }
}
