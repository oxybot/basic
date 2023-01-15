// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents an agreement toward a client.
/// </summary>
public class Agreement : BaseModel, IWithStatus<AgreementStatus>, IWithAttachments<AgreementAttachment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Agreement"/> class.
    /// </summary>
    public Agreement()
    {
        this.Items = new List<AgreementItem>();
        this.Invoices = new List<Invoice>();
        this.Statuses = new List<AgreementStatus>();
        this.Attachments = new List<AgreementAttachment>();
    }

    /// <summary>
    /// Gets or sets the parent client.
    /// </summary>
    [Required]
    public virtual Client Client { get; set; }

    /// <summary>
    /// Gets or sets the internal code of the agreement.
    /// </summary>
    [Required]
    [Unique]
    [MaxLength(50)]
    public string InternalCode { get; set; }

    /// <summary>
    /// Gets or sets the title of the agreement.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the owner of the agreement.
    /// </summary>
    public virtual User Owner { get; set; }

    /// <summary>
    /// Gets or sets the signature date of the agreement.
    /// </summary>
    public DateOnly? SignatureDate { get; set; }

    /// <summary>
    /// Gets or sets the private notes associated to the agreement.
    /// </summary>
    public string PrivateNotes { get; set; }

    /// <summary>
    /// Gets the items associated to the agreement.
    /// </summary>
    public virtual ICollection<AgreementItem> Items { get; }

    /// <summary>
    /// Gets the invoices attached to the agreement.
    /// </summary>
    public virtual ICollection<Invoice> Invoices { get; }

    /// <summary>
    /// Gets the statuses associated to the agreement.
    /// </summary>
    public virtual ICollection<AgreementStatus> Statuses { get; }

    /// <summary>
    /// Gets the list of the attachments.
    /// </summary>
    public virtual ICollection<AgreementAttachment> Attachments { get; }
}
