using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.PhysicsLogicBase;

namespace SimplexLab.LwfixPhysics.Velcro.Extensions.Controllers.ControllerBase
{
    public abstract class Controller : FilterData
    {
        private ControllerType _type;
        public bool Enabled;
        public World World;

        protected Controller(ControllerType controllerType)
        {
            _type = controllerType;
        }

        public override bool IsActiveOn(Body body)
        {
            if (body.ControllerFilter.IsControllerIgnored(_type))
                return false;

            return base.IsActiveOn(body);
        }

        public abstract void Update(Fixed32 dt);
    }
}
