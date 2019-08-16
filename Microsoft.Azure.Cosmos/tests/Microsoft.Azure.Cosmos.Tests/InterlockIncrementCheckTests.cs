﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------
namespace Microsoft.Azure.Cosmos.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InterlockIncrementCheckTests
    {
        [DataRow(0)]
        [DataRow(-1)]
        [DataTestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidatesMaxConcurrentOperations(int maxConcurrentOperations)
        {
            new InterlockIncrementCheck(maxConcurrentOperations);
        }

        [TestMethod]
        public void AllowsMultipleChainedOperations()
        {
            InterlockIncrementCheck check = new InterlockIncrementCheck();
            using (check.EnterLockCheck())
            {
            }

            using (check.EnterLockCheck())
            {
            }
        }

        [TestMethod]
        public async Task AllowsMultipleConcurrentOperations()
        {
            InterlockIncrementCheck check = new InterlockIncrementCheck(2);
            List<Task> tasks = new List<Task>(2);
            tasks.Add(this.RunLock(check));
            tasks.Add(this.RunLock(check));
            await Task.WhenAll(tasks);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ThrowsOnMultipleConcurrentOperations()
        {
            InterlockIncrementCheck check = new InterlockIncrementCheck(1);
            List<Task> tasks = new List<Task>(2);
            tasks.Add(this.RunLock(check));
            tasks.Add(this.RunLock(check));
            await Task.WhenAll(tasks);
        }

        private async Task RunLock(InterlockIncrementCheck check)
        {
            using (check.EnterLockCheck())
            {
                await Task.Delay(500);
            }
        }
    }
}
