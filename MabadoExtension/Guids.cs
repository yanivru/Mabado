// Guids.cs
// MUST match guids.h
using System;

namespace Reopened.MabadoExtension
{
    static class GuidList
    {
        public const string guidMabadoExtensionPkgString = "b620050d-95a8-4306-8048-1ad5b0b3f7bb";
        public const string guidMabadoExtensionCmdSetString = "f7b17d1f-50f1-4c63-9172-88445e8a828e";

        public static readonly Guid guidMabadoExtensionCmdSet = new Guid(guidMabadoExtensionCmdSetString);
    };
}