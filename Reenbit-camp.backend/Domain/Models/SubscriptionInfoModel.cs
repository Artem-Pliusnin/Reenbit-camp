using Domain.Enums;

namespace Domain.Models;

public sealed record SubscriptionInfoModel(
    string SubscriptionId, 
    SubscriptionStatus SubscriptionStatus,  
    DateTime? SubscriptionPeriodEnd,
    bool CancelAtPeriodEnd);