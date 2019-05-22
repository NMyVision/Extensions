using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConverterTests
{
    public static class AssertHelper
    {
        /// <summary>
        /// Adds Assert for cases where we want an exception
        /// </summary>
        /// <param name="action">Code to test that should cause an exception.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void ShouldThrowException(Action action, string message)
        {
            bool failed = true;
            string m = string.Empty;
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                failed = false;
                m = ex.Message;
            }

            if (failed)
                Assert.Fail($" { message } : {m}");
        }
    }
}