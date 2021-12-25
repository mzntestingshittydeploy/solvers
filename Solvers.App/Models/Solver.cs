using ProtoBuf;

namespace Solvers.App.Models
{
    public class Solver
    {
        public Solver()
        {
            Name = "";
            Image = "";
        }

        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Image { get; set; }
    }
}
