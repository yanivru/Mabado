using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Infrastructre;
using Mabado.View;
using Mabado.View.Commands;
using Mabado.View.Domain;
using Mabado.View.Factories;
using Mabado.View.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.Shell;

namespace Reopened.MabadoExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidMabadoExtensionPkgString)]
    public sealed class MabadoExtensionPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public MabadoExtensionPackage()
        {
            Debug.WriteLine("Entering constructor for: {0}", ToString());
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine("Entering Initialize() of: {0}", ToString());
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID openSolutionCommandId = new CommandID(GuidList.guidMabadoExtensionCmdSet, (int)PkgCmdIDList.cmdOpenSolution);
                MenuCommand openSolutionMenuItem = new MenuCommand(OpenSolutionMenuItemCallback, openSolutionCommandId);
                mcs.AddCommand(openSolutionMenuItem);

                CommandID connectToLabCommandId = new CommandID(GuidList.guidMabadoExtensionCmdSet, (int)PkgCmdIDList.cmdConnectToLab);
                MenuCommand connectToLabMenuItem = new MenuCommand(ConnectToLabMenuItemCallback, connectToLabCommandId);
                mcs.AddCommand(connectToLabMenuItem);
            }
        }

        #endregion

        private void ConnectToLabMenuItemCallback(object sender, EventArgs e)
        {
            var dte = (DTE2)GetService(typeof(DTE));

            if (!dte.Solution.IsOpen)
            {
                MessageBox.Show(@"Open solution first.", @"MabaDo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var container = InitContainer(dte);

            var connectToLabCommand = container.Resolve<ConnectToLabCommand>();

            connectToLabCommand.Execute(null);
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void OpenSolutionMenuItemCallback(object sender, EventArgs e)
        {
            //DTE2 dte = (DTE2)GetService(typeof(DTE));

            //if (!dte.Solution.IsOpen)
            //{
            //    MessageBox.Show(@"Open solution first.", @"MabaDo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //IPathProvider serverPathProvider = new BranchPathProvider(dte);

            //MabadoViewBootLoader viewBootLoader = new MabadoViewBootLoader(serverPathProvider, dte.SourceControl, new OutputWindowLogger(dte));

            //viewBootLoader.LaunchSolution();
        }

        private static UnityContainer InitContainer(DTE2 dte)
        {
            var logger = new OutputWindowLogger(dte);
            logger.WriteInfo("Loading Mabado... Connect to lab");

            UnityContainer container = new UnityContainer();
            container.RegisterInstance<ILogger>(logger);
            container.RegisterInstance<ISourceControl>(new SourceControlAdapter(dte.SourceControl));
            container.RegisterInstance<IPathProvider>(new BranchPathProvider(dte));

            var mabadoViewBootLoader = container.Resolve<MabadoViewBootLoader>();
            mabadoViewBootLoader.RegisterConnectionResolverModule();
            return container;
        }
    }
}
