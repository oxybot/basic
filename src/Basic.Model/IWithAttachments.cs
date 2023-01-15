// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Basic.Model;

/// <summary>
/// Marks a class that supports attachments.
/// </summary>
/// <typeparam name="TAttachment">The contrete type of attachments.</typeparam>
public interface IWithAttachments<TAttachment>
    where TAttachment : BaseAttachment
{
    /// <summary>
    /// Gets the associated attachments.
    /// </summary>
    public ICollection<TAttachment> Attachments { get; }
}
