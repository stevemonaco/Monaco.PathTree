using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Monaco.PathTree
{
    internal static class ThrowHelper
    {
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowStringNullEmptyOrWhiteSpace(string paramName, [CallerMemberName] string callerName = "")
        {
            throw new ArgumentException($"'{callerName}': Parameter '{paramName}' must not be null, empty or consist only of white-space characters.", paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentNull(string paramName, [CallerMemberName] string callerName = "")
        {
            throw new ArgumentException($"'{callerName}': Parameter '{paramName}' must not be null", paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowNodeAlreadyExists(string name, [CallerMemberName] string callerName = "")
        {
            throw new ArgumentException($"'{callerName}': Node '{name}' already exists");
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowNodeNotFound(string path, [CallerMemberName] string callerName = "")
        {
            throw new KeyNotFoundException($"'{callerName}': Node '{path}' does not exist");
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowNodeIsAlreadyAttached(string nodeName, string nodePath, [CallerMemberName] string callerName = "")
        {
            throw new InvalidOperationException($"'{callerName}': Node '{nodeName}' is already attached to the tree at '{nodePath}'");
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowParentNodeNotFound(string path, [CallerMemberName] string callerName = "")
        {
            throw new KeyNotFoundException($"'{callerName}': Parent node of '{path}' does not exist");
        }


    }
}
