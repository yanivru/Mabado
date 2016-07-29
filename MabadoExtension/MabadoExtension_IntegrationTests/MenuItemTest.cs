using System;
using System.Globalization;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace MabadoExtension_IntegrationTests
{
    [TestClass()]
    public class MenuItemTest
    {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for lauching the command and closing the associated dialogbox
        ///</summary>
        [TestMethod]
        [HostType("VS IDE")]
        public void ConnectToLab_SolutionIsNotOpen_WarningIsShown()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                DialogBoxPurger purger = new DialogBoxPurger(NativeMethods.IDOK, @"Open solution first.");

                try
                {
                    purger.Start();

                    VsIdeTestHostContext.Dte.ExecuteCommand("Tools.ConnectToLab");
                }
                finally
                {
                    Assert.IsTrue(purger.WaitForDialogThreadToTerminate(), "The dialog box has not shown");
                }
            });
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ConnectToLab_SolutionIsOpen_DialogIsOpen()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                DialogBoxPurger purger = new DialogBoxPurger(NativeMethods.IDOK, @"Open solution first.");

                try
                {
                    purger.Start();

                    VsIdeTestHostContext.Dte.ExecuteCommand("Tools.ConnectToLab");
                }
                finally
                {
                    Assert.IsTrue(purger.WaitForDialogThreadToTerminate(), "The dialog box has not shown");
                }
            });
        }
    }
}
