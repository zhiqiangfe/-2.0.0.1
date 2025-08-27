using SUNWODA_SEVB.Core.Enumerations;

namespace SUNWODA_SEVB.Core.Attributes
{
    // <summary>
    /// 标记业务模块，用于自动发现和注册
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }
        public string? Category { get; set; }
        public ModuleType Type { get; set; } = ModuleType.Normal;
        public string[]? RequiredPermissions { get; set; }

        public ModuleAttribute(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
