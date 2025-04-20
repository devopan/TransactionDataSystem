using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Interfaces;
using TransactionDataSystem.Services.Enums;

namespace TransactionDataSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _reportingService;
        private const string UserString = "user";
        private const string TransactionTypeString = "transactiontype";

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("users/transactions")]
        public async Task<ActionResult<IEnumerable<UserTransactionReportDto>>> GetTotalTransactionsByUser()
        {
            var report = await _reportingService.GetTotalTransactionsByUserAsync();
            return Ok(report);
        }

        [HttpGet("transactions/types")]
        public async Task<ActionResult<IEnumerable<TransactionTypeReportDto>>> GetTotalTransactionsByType()
        {
            var report = await _reportingService.GetTotalTransactionsByTypeAsync();
            return Ok(report);
        }

        [HttpGet("high-volume-transactions")]
        public async Task<ActionResult<HighVolumeTransactionReportDto>> GetHighVolumeTransactions(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] int limit,
            [FromQuery] string groupBy = UserString) // Default to grouping by user
        {
            try
            {
                // Parse dates from dd/MM/yyyy format
                if (!DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                {
                    return BadRequest("Invalid 'from' date format. Use dd/MM/yyyy.");
                }

                if (!DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                {
                    return BadRequest("Invalid 'to' date format. Use dd/MM/yyyy.");
                }

                // Set time to start of day for 'from' and end of day for 'to'
                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddTicks(-1); // End of the day

                // Determine grouping type
                var groupingType = groupBy.ToLower() == TransactionTypeString
                    ? HighVolumeGroupingType.ByTransactionType
                    : HighVolumeGroupingType.ByUser;

                var report = await _reportingService.GetHighVolumeTransactionsAsync(fromDate, toDate, limit, groupingType);
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
