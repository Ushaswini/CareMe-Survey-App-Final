using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using Homework05.MVC_Controllers;
using Homework05.API_Controllers;

namespace Homework05
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IAccountController, AccountController>();
            // container.Resolve<ISurveyStore>().Initialize();
            container.RegisterType<IController, HomeController>("home");

            return container;
        }
    }
}