using NUnit.Framework;
using Solvers.Test;

namespace Solvers.App.Test
{
    public class AssertionTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanAssert()
        {
            Assert.Pass();
        }
    }
}