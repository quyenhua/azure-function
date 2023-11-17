using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.UnitTests.ValueObjects
{
    public class ColorTests
    {
        [Test]
        public void ShouldReturnCorrectColorCode()
        {
            var code = "#FFFFFF";

            var color = Colors.From(code);

            color.Code.Should().Be(code);
        }

        [Test]
        public void ToStringReturnsCode()
        {
            var color = Colors.White;

            color.ToString().Should().Be(color.Code);
        }

        [Test]
        public void ShouldPerformImplicitConversionToColorCodeString()
        {
            string code = Colors.White;

            code.Should().Be("#FFFFFF");
        }

        [Test]
        public void ShouldPerformExplicitConversionGivenSupportedColorCode()
        {
            var color = (Colors)"#FFFFFF";

            color.Should().Be(Colors.White);
        }

        [Test]
        public void ShouldThrowUnsupportedColorExceptionGivenNotSupportedColorCode()
        {
            FluentActions.Invoking(() => Colors.From("##FF33CC"))
                .Should().Throw<UnsupportedColorException>();
        }
    }
}
