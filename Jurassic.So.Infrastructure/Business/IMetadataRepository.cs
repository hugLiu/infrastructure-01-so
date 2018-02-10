using System.Threading.Tasks;
using System.Collections.Generic;
using Jurassic.PKS.Service;

namespace Jurassic.So.Business
{
    /// <summary>元数据数据访问接口</summary>
    public interface IMetadataRepository
    {
        /// <summary>保存</summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns>返回受影响的元数据IIId集合</returns>
        Task<IEnumerable<string>> SaveAsync(MetadataCollection metadatas);
        /// <summary>更新</summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns>返回受影响的元数据IIId集合</returns>
        Task<IEnumerable<string>> UpdateAsync(MetadataCollection metadatas);
        /// <summary>删除</summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns>返回受影响的元数据IIId集合</returns>
        Task<IEnumerable<string>> DeleteAsync(MetadataCollection metadatas);
    }
}
