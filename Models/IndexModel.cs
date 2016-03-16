using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RepoOrchestrator.Models
{
    public class IndexModel
    {
        public string RepoPath { get; set; }
        public string BranchPath { get; set; }
        public string IndexType { get; set; }

        public static bool TryParse(string filePath, out IndexModel model)
        {
            // the IndexModel path goes like the following:
            // <repo>/<branch>/<IndexType>.txt

            model = null;

            string[] segments = filePath.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (segments.Count() < 3)
            {
                Trace.TraceWarning($"Invalid IndexModel - Not enough segments '{filePath}'");
                return false;
            }

            model = new IndexModel()
            {
                RepoPath = segments[0],
                BranchPath = string.Join(Path.AltDirectorySeparatorChar.ToString(), segments.Skip(1).Take(segments.Length - 2)),
                IndexType = Path.GetFileNameWithoutExtension(segments.Last()),
            };

            return true;
        }
    }
}