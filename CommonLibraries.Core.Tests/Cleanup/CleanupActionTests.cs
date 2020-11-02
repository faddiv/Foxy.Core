using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace CommonLibraries.Core.Cleanup
{
    public class CleanupActionTests
    {
        [Fact]
        public void CleanupAction_calls_the_ctor_action_on_dispose()
        {
            var mock = new Mock<Action>();

            var disposabe = new CleanupAction(mock.Object);
            disposabe.Dispose();

            mock.Verify(e => e(), Times.Once);
        }

        [Fact]
        public void CleanupAction_ctor_param_required()
        {
            Func<CleanupAction> a = () => new CleanupAction(null);

            a.Should().Throw<ArgumentNullException>();
        }
    }
}
