using System;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Shared.Contracts
{
    internal class EnsuresException : Exception
    {
        public EnsuresException(string message) : base(message) { }
    }
}
