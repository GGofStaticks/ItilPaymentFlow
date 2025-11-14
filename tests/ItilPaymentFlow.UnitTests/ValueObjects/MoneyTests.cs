using ItilPaymentFlow.Domain.ValueObjects;
using Xunit;

namespace ItilPaymentFlow.UnitTests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void From_Creates_ValueObject_With_Rounded_Amount()
    {
        var money = Money.From(10.129m, "usd");
        Assert.Equal(10.13m, money.Amount);
        Assert.Equal("USD", money.Currency);
    }
}

