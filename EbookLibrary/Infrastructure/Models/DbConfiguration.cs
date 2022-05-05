using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class DbConfiguration
    {
        public const string ConnectionString = @"Server=DESKTOP-P8F7PSV\SQLEXPRESS;Database=EbookLibrary;Integrated Security=True;";
        public const string ContributorConnectionString = @"Server=HEATHEN;Database=EbookLibrary;Integrated Security=True;";
    }
}
