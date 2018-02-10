using System.Threading.Tasks;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;

namespace Jurassic.So.Business
{
    /// <summary>元数据定义数据访问接口</summary>
    public interface IMetadataDefinitionRepository
    {
        /// <summary>获得全部元数据定义</summary>
        MetadataDefinitionCollection GetAll();
    }
}
