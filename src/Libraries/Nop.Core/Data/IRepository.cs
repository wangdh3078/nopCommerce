using System.Collections.Generic;
using System.Linq;

namespace Nop.Core.Data
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 通过标识符获取实体
        /// </summary>
        /// <param name="id">标识符</param>
        /// <returns>实体对象</returns>
        T GetById(object id);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        void Insert(T entity);

        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        void Update(T entity);

        /// <summary>
        ///更新实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        void Delete(T entity);

        /// <summary>
        /// 删除实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// 获取表
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        ///获取启用“无跟踪”的表（EF功能）仅在仅为只读操作加载记录时使用
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
