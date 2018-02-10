// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Data
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public interface IDatabaseInitializer
    {
        Task Initialize(IConfiguration configuration);
    }
}
