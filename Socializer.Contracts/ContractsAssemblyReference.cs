using System.Reflection;

namespace Socializer.Contracts;

/// <summary>
/// Anchor type for this assembly — DTOs/requests/responses that cross the API boundary live here,
/// shared between Socializer.Application (mapping) and Socializer.Api (endpoints).
/// </summary>
public static class ContractsAssemblyReference
{
    public static readonly Assembly Assembly = typeof(ContractsAssemblyReference).Assembly;
}
