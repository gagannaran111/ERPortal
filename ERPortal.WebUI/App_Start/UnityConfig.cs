using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.DataAccess.SQL;
using ERPortal.WebUI.Controllers;
using System;

using Unity;
using Unity.Injection;

namespace ERPortal.WebUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            //var ctr = new InjectionConstructor(typeof(IRepository<Organisation>));

            //  container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<IRepository<ERApplication>, SQLRepository<ERApplication>>();
            container.RegisterType<IRepository<FieldType>, SQLRepository<FieldType>>();
            container.RegisterType<IRepository<ERScreeningDetail>, SQLRepository<ERScreeningDetail>>();
            container.RegisterType<IRepository<ERScreeningInstitute>, SQLRepository<ERScreeningInstitute>>();
            container.RegisterType<IRepository<Organisation>, SQLRepository<Organisation>>();
            container.RegisterType<IRepository<UserAccount>, SQLRepository<UserAccount>>();
            container.RegisterType<IRepository<UploadFile>, SQLRepository<UploadFile>>();
            container.RegisterType<IRepository<UHCProductionMethod>, SQLRepository<UHCProductionMethod>>();
            container.RegisterType<IRepository<Comment>, SQLRepository<Comment>>();
            container.RegisterType<IRepository<ERAppActiveUsers>, SQLRepository<ERAppActiveUsers>>();
            container.RegisterType<IRepository<ForwardApplication>, SQLRepository<ForwardApplication>>();
            container.RegisterType<IRepository<AuditTrails>, SQLRepository<AuditTrails>>();
            container.RegisterType<IRepository<QueryDetails>, SQLRepository<QueryDetails>>();
            container.RegisterType<IRepository<QueryMaster>, SQLRepository<QueryMaster>>();
            container.RegisterType<IRepository<QueryUser>, SQLRepository<QueryUser>>();
            container.RegisterType<IRepository<StatusMaster>, SQLRepository<StatusMaster>>();
            container.RegisterType<IRepository<DepartmentType>, SQLRepository<DepartmentType>>();
            container.RegisterType<IRepository<ERTechniques>, SQLRepository<ERTechniques>>();
            container.RegisterType<IRepository<ERPilot>, SQLRepository<ERPilot>>();
        }
    }
}