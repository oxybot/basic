// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents a file shared with a base-64 encoding.
/// </summary>
public class Base64File
{
    /// <summary>
    /// Gets or sets the data of the file as a base-64 string.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the mime-type of the file.
    /// </summary>
    public string MimeType { get; set; }
}
