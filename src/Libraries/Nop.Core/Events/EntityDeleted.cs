
namespace Nop.Core.Events
{
    /// <summary>
    /// 用于传递已删除的实体的容器。 这不适用于通过位列删除逻辑的实体。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T> where T : BaseEntity
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
