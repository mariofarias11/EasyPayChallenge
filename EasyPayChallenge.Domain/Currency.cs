using System.ComponentModel;

namespace EasyPayChallenge.Domain;

public enum Currency
{
    [Description("US Dollar")]
    USD,
        
    [Description("Euro")]
    EUR,
        
    [Description("Brazilian Real")]
    BRL,
        
    [Description("Canadian Dollar")]
    CAD
}