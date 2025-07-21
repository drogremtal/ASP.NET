using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement;

public class CustomerPreference
{
    // ID клиента
    public Guid CustomerId { get; set; }

    // ID предпочтения
    public Guid PreferenceId { get; set; }

    // Навигационное свойство к Customer
    public Customer Customer { get; set; }

    // Навигационное свойство к Preference
    public Preference Preference { get; set; }
}
