using ContratacaoService.Domain.ValueObjects;

namespace ContratacaoService.Tests.Domain.ValueObjects;

public class PolicyNumberTests
{
    [Fact]
    public void Constructor_WithValidValue_ShouldCreatePolicyNumber()
    {
        // Arrange
        var value = "POL-20240101120000-abc123";

        // Act
        var policyNumber = new PolicyNumber(value);

        // Assert
        Assert.Equal(value, policyNumber.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidValue_ShouldThrowArgumentException(string invalidValue)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new PolicyNumber(invalidValue));
        Assert.Equal("Número da apólice inválido.", exception.Message);
    }

    [Fact]
    public void Generate_ShouldCreatePolicyNumberWithCorrectFormat()
    {
        // Act
        var policyNumber = PolicyNumber.Generate();

        // Assert
        Assert.NotNull(policyNumber.Value);
        Assert.StartsWith("POL-", policyNumber.Value);
        Assert.Equal(25, policyNumber.Value.Length);
    }

    [Fact]
    public void Generate_ShouldCreateUniquePolicyNumbers()
    {
        // Act
        var policyNumber1 = PolicyNumber.Generate();
        var policyNumber2 = PolicyNumber.Generate();

        // Assert
        Assert.NotEqual(policyNumber1.Value, policyNumber2.Value);
    }

    [Fact]
    public void ImplicitOperator_StringToPolicyNumber_ShouldCreatePolicyNumber()
    {
        // Arrange
        var value = "POL-20240101120000-abc123";

        // Act
        PolicyNumber policyNumber = value;

        // Assert
        Assert.Equal(value, policyNumber.Value);
    }

    [Fact]
    public void ImplicitOperator_PolicyNumberToString_ShouldReturnValue()
    {
        // Arrange
        var value = "POL-20240101120000-abc123";
        var policyNumber = new PolicyNumber(value);

        // Act
        string result = policyNumber;

        // Assert
        Assert.Equal(value, result);
    }
}
