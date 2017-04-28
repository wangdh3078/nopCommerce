using System.Linq;
using System.Linq.Dynamic;
using FluentValidation;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Localization;

namespace Nop.Web.Framework.Validators
{
    public abstract class BaseNopValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseNopValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// 开发人员可以在自定义部分类中覆盖此方法，
        /// 以便为构造函数添加一些自定义的初始化代码
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        /// <summary>
        /// 将验证规则设置为适当的数据库模型
        /// </summary>
        /// <typeparam name="TObject">对象类型</typeparam>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="filterStringPropertyNames">要跳过的属性</param>
        protected virtual void SetDatabaseValidationRules<TObject>(IDbContext dbContext, params string[] filterStringPropertyNames)
        {
            SetStringPropertiesMaxLength<TObject>(dbContext, filterStringPropertyNames);
            SetDecimalMaxValue<TObject>(dbContext);
        }

        /// <summary>
        /// 根据适当的数据库模型将长度验证规则设置为字符串属性
        /// </summary>
        /// <typeparam name="TObject">对象类型</typeparam>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="filterPropertyNames">要跳过的属性</param>
        protected virtual void SetStringPropertiesMaxLength<TObject>(IDbContext dbContext, params string[] filterPropertyNames)
        {
            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !filterPropertyNames.Contains(p.Name))
                .Select(p => p.Name).ToArray();

            var maxLength = dbContext.GetColumnsMaxLength(dbObjectType.Name, names);
            var expression = maxLength.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, string>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).Length(0, maxLength[expr.Key]);
            }
        }

        /// <summary>
        ///根据适当的数据库模型将最大值验证规则设置为小数属性
        /// </summary>
        /// <typeparam name="TObject">对象类型</typeparam>
        /// <param name="dbContext">数据库上下文</param>
        protected virtual void SetDecimalMaxValue<TObject>(IDbContext dbContext)
        {
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(decimal))
                .Select(p => p.Name).ToArray();

            var maxValues = dbContext.GetDecimalMaxValue(dbObjectType.Name, names);
            var expression = maxValues.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, decimal>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).IsDecimal(maxValues[expr.Key]).WithMessage(string.Format(localizationService.GetResource("Nop.Web.Framework.Validators.MaxDecimal"), maxValues[expr.Key] - 1));
            }
        }
    }
}