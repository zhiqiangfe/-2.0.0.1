using System.Linq.Expressions;

namespace SUNWODA_SEVB.Tool.Converter
{
    /// <summary>
    /// 表达式转换器，将源类型的表达式转换为目标类型的表达式
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public class ExpressionConverter<TSource, TTarget> : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly Dictionary<string, string> _propertyMapping;

        public ExpressionConverter(
            ParameterExpression parameter,
            Dictionary<string, string>? propertyMapping = null
        )
        {
            _parameter = parameter;
            _propertyMapping = propertyMapping ?? new Dictionary<string, string>();
        }

        public Expression<Func<TTarget, bool>> Convert(Expression<Func<TSource, bool>> expression)
        {
            var body = Visit(expression.Body);
            return Expression.Lambda<Func<TTarget, bool>>(body, _parameter);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                var memberName = node.Member.Name;

                // 如果有属性映射，使用映射后的名称
                if (_propertyMapping.ContainsKey(memberName))
                {
                    memberName = _propertyMapping[memberName];
                }

                var targetMember = typeof(TTarget).GetProperty(memberName);
                if (targetMember != null)
                {
                    return Expression.MakeMemberAccess(_parameter, targetMember);
                }
            }

            return base.VisitMember(node);
        }
    }
}
