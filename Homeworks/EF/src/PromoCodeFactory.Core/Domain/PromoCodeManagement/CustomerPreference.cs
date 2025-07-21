using System;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement;

public class CustomerPreference
{
    // ID клиента
    public Guid CustomerId { get; set; }

    // ID предпочтения
    public Guid PreferenceId { get; set; }

    // Навигационное свойство к Customer
    public virtual  Customer Customer { get; set; }

    // Навигационное свойство к Preference
    public virtual Preference Preference { get; set; }
}
