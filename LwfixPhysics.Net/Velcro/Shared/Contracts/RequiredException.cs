using System;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Shared.Contracts
{
    public class RequiredException : Exception
    {
        public RequiredException(string message) : base(message) { }
    }
}
