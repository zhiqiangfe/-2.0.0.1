using SqlSugar;

namespace HTHIUM.Data.Context
{
    /// <summary>
    /// SqlSugar上下文接口（Data层内部使用）
    /// </summary>
    internal interface ISqlSugarContext
    {
        ISqlSugarClient Db { get; }
    }
}
