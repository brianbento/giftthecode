using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Data
{
    public interface IDatabaseInitializer
    {
        Task Initialize(IConfiguration configuration);
    }
}
