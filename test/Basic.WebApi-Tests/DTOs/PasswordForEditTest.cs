// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Tests the <see cref="PasswordForEdit"/> class.
    /// </summary>
    [TestClass]
    public class PasswordForEditTest
    {
        /// <summary>
        /// Tests the <see cref="PasswordForEdit.Validate(ValidationContext)"/> method.
        /// </summary>
        [TestMethod]
        [DataRow]
        public void ValidateNull()
        {
            var password = new PasswordForEdit();
            Assert.ThrowsException<ArgumentNullException>(() => password.Validate(null).ToArray());
        }

        /// <summary>
        /// Tests the <see cref="PasswordForEdit.Validate(ValidationContext)"/> method
        /// with values that should be accepted.
        /// </summary>
        /// <param name="actual">The tested value.</param>
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("aaaaaaaaaaaaaaaa")]
        [DataRow("aaaBBB111___")]
        [DataRow("aaaaaa111___")]
        [DataRow("BBBBBB111___")]
        [DataRow("aaa111111___")]
        [DataRow("aaaBBB111111")]
        [DataRow("aaa___111___")]
        public void ValidateAccepted(string actual)
        {
            var password = new PasswordForEdit() { NewPassword = actual };
            var context = new ValidationContext(password);
            var results = password.Validate(context).ToArray();

            Assert.IsNotNull(results);
            if (results.Length > 0)
            {
                Assert.Fail("Password not accepted. " + results[0].ErrorMessage);
            }
        }

        /// <summary>
        /// Tests the <see cref="PasswordForEdit.Validate(ValidationContext)"/> method
        /// with values that should be rejected.
        /// </summary>
        /// <param name="actual">The tested value.</param>
        [TestMethod]
        [DataRow(" ")]
        [DataRow("aaaaaaaaaaaaaaa")]
        [DataRow("aaaBBB111__")]
        public void ValidateRejected(string actual)
        {
            var password = new PasswordForEdit() { NewPassword = actual };
            var context = new ValidationContext(password);
            var results = password.Validate(context).ToArray();

            Assert.IsNotNull(results);
            Assert.AreNotEqual(0, results.Length);
        }
    }
}
