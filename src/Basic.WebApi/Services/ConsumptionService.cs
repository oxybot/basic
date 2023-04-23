// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Services;

/// <summary>
/// Provides consumption calculation services.
/// </summary>
public class ConsumptionService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsumptionService"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public ConsumptionService(Context context, IMapper mapper, ILogger<ConsumptionService> logger)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the datasource context.
    /// </summary>
    protected Context Context { get; }

    /// <summary>
    /// Gets the configured automapper.
    /// </summary>
    protected IMapper Mapper { get; }

    /// <summary>
    /// Gets the associated logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Retrieves the time-off consumption per category for a user.
    /// </summary>
    /// <param name="user">The reference user.</param>
    /// <returns>The consumption of the <paramref name="user"/>.</returns>
    public IEnumerable<ConsumptionForList> GetConsumptionForUser(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var year = DateTime.Now.Year;
        var startOfYear = new DateOnly(DateTime.Now.Year, 1, 1);
        var endOfYear = startOfYear.AddYears(1).AddDays(-1);

        var balances = this.Context.Set<Balance>()
            .Include(b => b.Category)
            .Where(b => b.User == user && b.Year == startOfYear.Year).ToList();
        var groupedEvents = this.Context.Set<Event>()
            .Include(e => e.Category)
            .Include(e => e.CurrentStatus)
            .Where(e => e.User == user && e.StartDate >= startOfYear && e.StartDate <= endOfYear && e.Category.Mapping == EventTimeMapping.TimeOff)
            .Where(e => e.CurrentStatus.IsActive)
            .ToList()
            .GroupBy(e => e.Category);

        foreach (var category in groupedEvents)
        {
            var balance = balances.SingleOrDefault(b => b.Category == category.Key);
            var planned = category.Where(e => e.CurrentStatus.Identifier == Status.Approved && e.StartDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Today).ToList();
            var taken = category.Where(e => e.CurrentStatus.Identifier == Status.Approved && e.StartDate.ToDateTime(TimeOnly.MinValue) < DateTime.Today).ToList();
            var requested = category.Where(e => e.CurrentStatus.Identifier == Status.Requested).ToList();
            yield return new ConsumptionForList()
            {
                Category = this.Mapper.Map<EntityReference>(category.Key),
                Total = balance?.Total,
                Planned = planned.Sum(e => e.DurationTotal),
                Taken = taken.Sum(e => e.DurationTotal),
                Requested = requested.Sum(e => e.DurationTotal),
            };
        }

        foreach (var balance in balances.Where(b => !groupedEvents.Any(c => c.Key == b.Category)))
        {
            yield return new ConsumptionForList()
            {
                Category = this.Mapper.Map<EntityReference>(balance.Category),
                Total = balance.Total,
            };
        }
    }
}
