using PropostaService.Domain.ValueObjects;

namespace PropostaService.Tests.Domain.ValueObjects;

public class ProposalDescriptionTests
{
    [Fact]
    public void Constructor_WithValidValue_ShouldCreateProposalDescription()
    {
        // Arrange
        var value = "This is a valid proposal description with more than 10 characters";

        // Act
        var description = new ProposalDescription(value);

        // Assert
        Assert.Equal(value.Trim(), description.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrEmptyValue_ShouldThrowArgumentException(string invalidValue)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ProposalDescription(invalidValue));
        Assert.Equal("Descrição é obrigatória.", exception.Message);
    }

    [Theory]
    [InlineData("short")]
    [InlineData("123456789")]
    public void Constructor_WithTooShortValue_ShouldThrowArgumentException(string shortValue)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ProposalDescription(shortValue));
        Assert.Equal("Descrição deve ter pelo menos 10 caracteres.", exception.Message);
    }

    [Fact]
    public void Constructor_WithTooLongValue_ShouldThrowArgumentException()
    {
        // Arrange
        var longValue = new string('a', 501);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ProposalDescription(longValue));
        Assert.Equal("Descrição não pode exceder 500 caracteres.", exception.Message);
    }

    [Fact]
    public void Constructor_WithWhitespaceValue_ShouldTrimValue()
    {
        // Arrange
        var value = "  This is a proposal description with whitespace  ";

        // Act
        var description = new ProposalDescription(value);

        // Assert
        Assert.Equal(value.Trim(), description.Value);
    }

    [Fact]
    public void ImplicitOperator_StringToProposalDescription_ShouldCreateProposalDescription()
    {
        // Arrange
        var value = "This is a valid proposal description with more than 10 characters";

        // Act
        ProposalDescription description = value;

        // Assert
        Assert.Equal(value.Trim(), description.Value);
    }

    [Fact]
    public void ImplicitOperator_ProposalDescriptionToString_ShouldReturnValue()
    {
        // Arrange
        var value = "This is a valid proposal description with more than 10 characters";
        var description = new ProposalDescription(value);

        // Act
        string result = description;

        // Assert
        Assert.Equal(value.Trim(), result);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var value = "This is a valid proposal description with more than 10 characters";
        var description1 = new ProposalDescription(value);
        var description2 = new ProposalDescription(value);

        // Act & Assert
        Assert.True(description1.Equals(description2));
        Assert.Equal(description1.GetHashCode(), description2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var description1 = new ProposalDescription("First description");
        var description2 = new ProposalDescription("Second description");

        // Act & Assert
        Assert.False(description1.Equals(description2));
    }

    [Fact]
    public void Equals_WithNullObject_ShouldReturnFalse()
    {
        // Arrange
        var description = new ProposalDescription("Test description");

        // Act & Assert
        Assert.False(description.Equals(null));
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var value = "This is a valid proposal description with more than 10 characters";
        var description = new ProposalDescription(value);

        // Act
        var result = description.ToString();

        // Assert
        Assert.Equal(value.Trim(), result);
    }

    [Fact]
    public void Constructor_WithExactly10Characters_ShouldCreateProposalDescription()
    {
        // Arrange
        var value = "1234567890";

        // Act
        var description = new ProposalDescription(value);

        // Assert
        Assert.Equal(value, description.Value);
    }

    [Fact]
    public void Constructor_WithExactly500Characters_ShouldCreateProposalDescription()
    {
        // Arrange
        var value = new string('a', 500);

        // Act
        var description = new ProposalDescription(value);

        // Assert
        Assert.Equal(value, description.Value);
    }
}
