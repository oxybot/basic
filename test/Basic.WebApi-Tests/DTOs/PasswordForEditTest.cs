// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Tests the <see cref="PasswordForEdit"/> class.
/// </summary>
public class PasswordForEditTest
{
    /// <summary>
    /// Tests the <see cref="PasswordForEdit.Validate(ValidationContext)"/> method.
    /// </summary>
    [Fact]
    public void ValidateNull()
    {
        var password = new PasswordForEdit();
        Assert.Throws<ArgumentNullException>(() => password.Validate(null).ToArray());
    }

    /// <summary>
    /// Tests the <see cref="PasswordForEdit.Validate(ValidationContext)"/> method
    /// with values that should be accepted.
    /// </summary>
    /// <param name="actual">The tested value.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("aaaaaaaaaaaaaaaa")]
    [InlineData("aaaBBB111___")]
    [InlineData("aaaaaa111___")]
    [InlineData("BBBBBB111___")]
    [InlineData("aaa111111___")]
    [InlineData("aaaBBB111111")]
    [InlineData("aaa___111___")]
    public void ValidateAccepted(string actual)
    {
        var password = new PasswordForEdit() { NewPassword = actual, ConfirmPassword = actual };
        var context = new ValidationContext(password);
        var results = password.Validate(context).ToArray();

        Assert.NotNull(results);
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
    [Theory]
    [InlineData(" ")]
    [InlineData("1234")]
    [InlineData("123456")]
    [InlineData("aaaaaaaaaaaaaaa")]
    [InlineData("_&;__&;__'_'___")]
    [InlineData("01234567891011")]
    [InlineData("aaaBBB111__")]
    public void ValidateRejected(string actual)
    {
        var password = new PasswordForEdit() { NewPassword = actual, ConfirmPassword = actual };
        var context = new ValidationContext(password);
        var results = password.Validate(context).ToArray();

        Assert.NotEmpty(results);
    }
}
