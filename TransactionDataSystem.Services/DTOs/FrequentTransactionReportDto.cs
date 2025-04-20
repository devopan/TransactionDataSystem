using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDataSystem.Services.DTOs
{
    public class HighVolumeTransactionReportDto
    {
        public List<int> TransactionIds { get; set; } = new List<int>();
    }
}
