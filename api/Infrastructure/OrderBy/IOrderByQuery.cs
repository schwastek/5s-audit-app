using System.Collections.Generic;

namespace Infrastructure.OrderBy;

public interface IOrderByQuery
{
    IReadOnlyList<OrderByInstruction> OrderBy { get; }
}
