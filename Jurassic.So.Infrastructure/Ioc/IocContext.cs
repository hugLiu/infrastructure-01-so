using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Jurassic.So.Infrastructure
{
    /// <summary>IOC容器上下文</summary>
    /// <remarks>配置文件中容器名称为container</remarks>
    public static class IocContext
    {
        /// <summary>Unity容器</summary>
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            container.LoadConfiguration("container");
            return container;
        });
        /// <summary>Unity容器实例</summary>
        public static IUnityContainer Instance
        {
            get { return Container.Value; }
        }
    }
}