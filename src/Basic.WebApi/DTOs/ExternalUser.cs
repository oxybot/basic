// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents an importable user as retrieved from the external authenticator.
/// </summary>
public class ExternalUser : UserForEdit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalUser"/> class.
    /// </summary>
    public ExternalUser()
    {
        this.Importable = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the user is importable.
    /// </summary>
    public bool Importable { get; set; }
}
