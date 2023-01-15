// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Basic.Model;

/// <summary>
/// Indicates that the associated property is a business key and the associated values should
/// be unique to each instance.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class UniqueAttribute : Attribute
{
}
