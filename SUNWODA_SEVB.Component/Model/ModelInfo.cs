using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SUNWODA_SEVB.Component.Model
{
    public class ModelInfo
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? FilePath { get; set; }
        public Model3D? Model { get; set; }
        public object? Tag { get; set; } // 用于存储额外信息

        public ModelInfo(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }
    }
}
