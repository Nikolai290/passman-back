using FluentAssertions;
using passman_back.Domain.Core.Enums;
using Xunit;

namespace passman_back.Domain.Core.Tests.Enums {
    public class PermissionTests {
        [Fact]
        public void FullAccessGreaterThanReadonly_true() {
            var actual = Permission.FullAccess > Permission.ReadOnly;
            actual.Should().BeTrue();
        }

        [Fact]
        public void FullAccessGreaterThanNone_true() {
            var actual = Permission.FullAccess > Permission.None;
            actual.Should().BeTrue();
        }

        [Fact]
        public void ReadonlyGreaterThanNone_true() {
            var actual = Permission.ReadOnly > Permission.None;
            actual.Should().BeTrue();
        }
    }
}
