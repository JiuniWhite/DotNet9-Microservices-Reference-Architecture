using System;
namespace Catalog.Core.Entities.Base
{
	public interface IMustHaveTenant
	{
        public string? TenantId { get; set; }
    }
}

