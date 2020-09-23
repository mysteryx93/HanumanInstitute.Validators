using System;
using System.Collections.Generic;
using Xunit;

#pragma warning disable CS8604 // Possible null reference argument.
namespace HanumanInstitute.Validators.Tests
{
    public class PreconditionsTests
    {
        [Fact]
        public void CheckNotNull_Value_ReturnsValue()
        {
            int? value = 5;

            var result = value.CheckNotNull(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void CheckNotNull_Null_ThrowsException()
        {
            int? value = null;

            void Act()
            {
                value.CheckNotNull(nameof(value));
            }

            Assert.Throws<ArgumentNullException>(nameof(value), Act);
        }

        [Fact]
        public void CheckNotNullOrEmpty_Value_ReturnsValue()
        {
            var value = "a";

            var result = value.CheckNotNull(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void CheckNotNullOrEmpty_NullString_ThrowsException()
        {
            string value = null;

            void Act() => value.CheckNotNullOrEmpty(nameof(value));

            Assert.Throws<ArgumentNullException>(nameof(value), Act);
        }

        [Fact]
        public void CheckNotNullOrEmpty_EmptyString_ThrowsException()
        {
            var value = string.Empty;

            void Act()
            {
                value.CheckNotNullOrEmpty(nameof(value));
            }

            Assert.Throws<ArgumentException>(nameof(value), Act);
        }

        [Fact]
        public void CheckNotNullOrEmpty_NullList_ThrowsException()
        {
            List<string> value = null;

            void Act()
            {
                value.CheckNotNullOrEmpty(nameof(value));
            }

            Assert.Throws<ArgumentNullException>(nameof(value), Act);
        }

        [Fact]
        public void CheckNotNullOrEmpty_EmptyList_ThrowsException()
        {
            var value = Array.Empty<string>();

            void Act()
            {
                value.CheckNotNullOrEmpty(nameof(value));
            }

            Assert.Throws<ArgumentException>(nameof(value), Act);
        }

        [Theory]
        [InlineData(1, null, false, null, false)]
        [InlineData(1, 0, true, null, false)]
        [InlineData(0, 0, true, null, false)]
        [InlineData(1.1, 1, false, null, false)]
        [InlineData(double.MinValue * .99, double.MinValue, false, null, false)]
        [InlineData(double.MaxValue, double.MaxValue * .99, false, null, false)]
        [InlineData(0, null, true, 0, true)]
        [InlineData(0.9, null, true, 1.0, false)]
        [InlineData(double.MinValue, null, true, double.MinValue, true)]
        [InlineData(double.MaxValue * .99, null, true, double.MaxValue, false)]
        [InlineData(0.1, 0, false, 1, false)]
        [InlineData(-10, -10, true, -1, true)]
        [InlineData(-1, -10, true, -1, true)]
        [InlineData(double.MaxValue, double.MinValue, true, double.MaxValue, true)]
        [InlineData(double.MinValue, double.MinValue, true, double.MaxValue, true)]
        [InlineData(double.MaxValue * .99, double.MinValue, true, double.MaxValue, false)]
        [InlineData(double.MinValue * .99, double.MinValue, false, double.MaxValue, true)]
        public void CheckRange_Valid_ReturnsValue(double value, double? min, bool minInclusive, double? max, bool maxInclusive)
        {
            var result = value.CheckRange(nameof(value), min, minInclusive, max, maxInclusive);

            Assert.Equal(value, result);
        }

        [Fact]
        public void CheckRange_GreaterThanInclusive_ThrowsException()
        {
            var value = -2;

            void Act()
            {
                value.CheckRange(nameof(value), -1);
            }

            var exception = Assert.Throws<ArgumentOutOfRangeException>(nameof(value), Act);
            Assert.StartsWith("value must be greater than or equal to -1.", exception.Message, StringComparison.InvariantCulture);
        }

        [Fact]
        public void CheckRange_GreaterThanExclusive_ThrowsException()
        {
            var value = 0;

            void Act()
            {
                value.CheckRange(nameof(value), 0, false);
            }

            var exception = Assert.Throws<ArgumentOutOfRangeException>(nameof(value), Act);
            Assert.StartsWith("value must be greater than 0.", exception.Message, StringComparison.InvariantCulture);
        }

        [Fact]
        public void CheckRange_LessThanInclusive_ThrowsException()
        {
            var value = -2;

            void Act()
            {
                value.CheckRange(nameof(value), max: -10);
            }

            var exception = Assert.Throws<ArgumentOutOfRangeException>(nameof(value), Act);
            Assert.StartsWith("value must be less than or equal to -10.", exception.Message, StringComparison.InvariantCulture);
        }

        [Fact]
        public void CheckRange_LessThanExclusive_ThrowsException()
        {
            var value = -10;

            void Act()
            {
                value.CheckRange(nameof(value), max: -10, maxInclusive: false);
            }

            var exception = Assert.Throws<ArgumentOutOfRangeException>(nameof(value), Act);
            Assert.StartsWith("value must be less than -10.", exception.Message, StringComparison.InvariantCulture);
        }

        [Fact]
        public void CheckRange_RangeInclusive_ThrowsException()
        {
            var value = 9;

            void Act()
            {
                value.CheckRange(nameof(value), 10, true, 20, true);
            }

            var exception = Assert.Throws<ArgumentOutOfRangeException>(nameof(value), Act);
            Assert.StartsWith("value must be between 10 and 20.", exception.Message, StringComparison.InvariantCulture);
        }

        [Fact]
        public void CheckRange_RangeExclusive_ThrowsException()
        {
            var value = 20;

            void Act()
            {
                value.CheckRange(nameof(value), 10, false, 20, false);
            }

            var exception = Assert.Throws<ArgumentOutOfRangeException>(nameof(value), Act);
            Assert.StartsWith("value must be greater than 10 and less than 20.", exception.Message, StringComparison.InvariantCulture);
        }
    }
}
