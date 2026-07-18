using System.Reflection;

namespace Socializer.Application;

/// <summary>
/// Anchor type for this assembly — pass <c>ApplicationAssemblyReference.Assembly</c>
/// to scanning-based registrations (FluentValidation, MediatR, AutoMapper, etc.) as those get added.
/// </summary>
public static class ApplicationAssemblyReference
{
    public static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}
