using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RepoOrchestrator.Models
{
    public class IndexModel
    {
        public string FullPath { get; set; }
        public string Owner { get; set; }
        public string Repo { get; set; }
        public string Branch { get; set; }
        public string IndexType { get; set; }

        public static bool TryParse(string fullPath, out IndexModel model)
        {
            // the IndexModel path goes like the following:
            // <owner>/<repo>/<branch>/<IndexType>.txt

            model = null;

            string[] segments = fullPath.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (segments.Count() < 4)
            {
                Trace.TraceWarning($"Invalid IndexModel - Not enough segments '{fullPath}'");
                return false;
            }

            model = new IndexModel()
            {
                FullPath = fullPath,
                Owner = segments[0],
                Repo = segments[1],
                Branch = string.Join(Path.AltDirectorySeparatorChar.ToString(), segments.Skip(2).Take(segments.Length - 3)),
                IndexType = Path.GetFileNameWithoutExtension(segments.Last()),
            };

            return true;
        }
    }
}