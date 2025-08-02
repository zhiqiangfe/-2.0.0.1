using Mapster;

namespace SUNWODA_SEVB.Data.Mappings
{
    /// <summary>
    /// 映射扩展方法
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// 映射到目标类型
        /// </summary>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return source.Adapt<TDestination>();
        }

        /// <summary>
        /// 映射到目标对象
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return source.Adapt(destination);
        }

        /// <summary>
        /// 映射列表
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            return source.Adapt<List<TDestination>>();
        }

        /// <summary>
        /// 投影查询
        /// </summary>
        public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable source)
        {
            return source.ProjectToType<TDestination>();
        }
    }
}
