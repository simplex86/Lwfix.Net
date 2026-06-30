using System;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Shared.Contracts
{
    public class EnsuresException : Exception
    {
        public EnsuresException(string message) : base(message) { }
    }
}
