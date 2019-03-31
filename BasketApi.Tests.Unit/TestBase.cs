using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketApi.Tests.Unit
{
    public abstract class TestBase
    {
        private Randomizer _randomizer;
        private const int MAX_INT_VALUE = 1000;

        protected TestBase()
        {
            _randomizer = new Randomizer(Guid.NewGuid().GetHashCode());
        }

        protected int GetRandomInt()
        {
            return _randomizer.Next(MAX_INT_VALUE);
        }
    }
}
