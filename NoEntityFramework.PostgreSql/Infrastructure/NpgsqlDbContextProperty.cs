using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoEntityFramework
{
    public partial class NpgsqlDbContext : INpgsqlDbContext
    {
        public RelationalDbOptions Options
        {
            get
            {
                return _options;
            }
        }
    }
}
