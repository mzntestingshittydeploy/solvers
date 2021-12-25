using ProtoBuf;

namespace Solvers.App.Events
{
    [ProtoContract]
    public class CreatedSolver
    {
        [ProtoMember(1)]
        public long Id { get; set; }

        [ProtoMember(2)]
        public Guid UserToken { get; set; }
    }
}
