using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;

namespace PromoCodeFactory.WebHost.Models;

public class PreferenceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<CustomerPreference> CustomersLink { get; set; } = new List<CustomerPreference>();
}
