#if !NET6_0_OR_GREATER
#pragma warning disable IDE0060 // Remove unused parameter

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.Versioning;

/// <summary>
/// Records the operating system (and minimum version) that supports an API. Multiple attributes can be
/// applied to indicate support on multiple operating systems.
/// </summary>
/// <remarks>
/// Callers can apply a <see cref="SupportedOSPlatformAttribute " />
/// or use guards to prevent calls to APIs on unsupported operating systems.
///
/// A given platform should only be specified once.
/// </remarks>
[AttributeUsage(
    AttributeTargets.Assembly |
    AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Enum |
    AttributeTargets.Event |
    AttributeTargets.Field |
    AttributeTargets.Interface |
    AttributeTargets.Method |
    AttributeTargets.Module |
    AttributeTargets.Property |
    AttributeTargets.Struct,
    AllowMultiple = true, Inherited = false)]
internal sealed class SupportedOSPlatformAttribute : Attribute
{
    public SupportedOSPlatformAttribute(string platformName)
    {
    }
}

#endif
