using System.Collections.Generic;

namespace Infrastructure.OrderBy;

/// <summary>
/// High-level orchestrator for resolving client-provided ORDER BY instructions
/// into executable domain sort instructions.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="OrderByService{TContext}"/> is intended to be the primary entry point
/// for handling ORDER BY logic in application code. It combines parsing, validation,
/// and mapping into a single, easy-to-use service so callers do not need to work with
/// lower-level components directly.
/// </para>
/// <para>
/// The generic type parameter <typeparamref name="TContext"/> represents the domain
/// context being sorted (for example, a specific entity type). It is used solely for
/// dependency injection so the correct <see cref="IOrderByMap{TContext}"/> can be
/// resolved automatically, without requiring keyed services or factories.
/// </para>
/// </remarks>
public interface IOrderByService<TContext>
{
    IReadOnlyList<OrderByInstruction> Resolve(string? orderBy);
    IReadOnlyList<OrderByInstruction> Resolve(IReadOnlyList<OrderByInstruction> orderByInstructions);
}

public class OrderByService<TContext> : IOrderByService<TContext>
{
    private readonly IOrderByMap<TContext> _map;

    public OrderByService(IOrderByMap<TContext> map)
    {
        _map = map;
    }

    public IReadOnlyList<OrderByInstruction> Resolve(string? orderByQuery)
    {
        var parameters = OrderByParser.Parse(orderByQuery);
        var instructions = Resolve(parameters);

        return instructions;
    }

    public IReadOnlyList<OrderByInstruction> Resolve(IReadOnlyList<OrderByInstruction> orderByInstructions)
    {
        var instructions = OrderByMapper.Map(orderByInstructions, _map);

        return instructions;
    }
}
