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
        /// ������Ա�������Զ��岿�����и��Ǵ˷�����
        /// �Ա�Ϊ���캯�����һЩ�Զ���ĳ�ʼ������
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        /// <summary>
        /// ����֤��������Ϊ�ʵ������ݿ�ģ��
        /// </summary>
        /// <typeparam name="TObject">��������</typeparam>
        /// <param name="dbContext">���ݿ�������</param>
        /// <param name="filterStringPropertyNames">Ҫ����������</param>
        protected virtual void SetDatabaseValidationRules<TObject>(IDbContext dbContext, params string[] filterStringPropertyNames)
        {
            SetStringPropertiesMaxLength<TObject>(dbContext, filterStringPropertyNames);
            SetDecimalMaxValue<TObject>(dbContext);
        }

        /// <summary>
        /// �����ʵ������ݿ�ģ�ͽ�������֤��������Ϊ�ַ�������
        /// </summary>
        /// <typeparam name="TObject">��������</typeparam>
        /// <param name="dbContext">���ݿ�������</param>
        /// <param name="filterPropertyNames">Ҫ����������</param>
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
        ///�����ʵ������ݿ�ģ�ͽ����ֵ��֤��������ΪС������
        /// </summary>
        /// <typeparam name="TObject">��������</typeparam>
        /// <param name="dbContext">���ݿ�������</param>
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