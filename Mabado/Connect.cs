using System;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Infrastructre;
using Mabado.View.Domain;
using Mabado.View.Factories;
using Microsoft.VisualStudio.CommandBars;

namespace Mabado
{
    /// <summary>The object for implementing an Update-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        /// <summary>Implements the constructor for the Update-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Update-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Update-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Update-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;

            _addInInstance = (AddIn)addInInst;
            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;
                string toolsMenuName = "Tools";

                //Place the command on the tools menu.
                //Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
                Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"];

                //Find the Tools command bar on the MenuBar command bar:
                CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
                CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

                //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Update-in,
                //  just make sure you also update the QueryStatus/Exec method to include the new command names.
                try
                {
                    //Update a command to the Commands collection:
                    Command commandConnectToLab = commands.AddNamedCommand2(_addInInstance, "ConnectToLab", "Connect To Lab", "Executes the command for Mabado", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    Command commandLaunchSolution = commands.AddNamedCommand2(_addInInstance, "OpenSolution", "Open Solution", "Executes the command for Mabado", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);

                    //Update a control for the command to the tools menu:
                    if ((commandLaunchSolution != null) && (toolsPopup != null))
                    {
                        commandLaunchSolution.AddControl(toolsPopup.CommandBar, 1);
                    }

                    if ((commandConnectToLab != null) && (toolsPopup != null))
                    {
                        commandConnectToLab.AddControl(toolsPopup.CommandBar, 2);
                    }
                }
                catch (System.ArgumentException)
                {
                    //If we are here, then the exception is probably because a command with that name
                    //  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
                }
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Update-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Update-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Update-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName == "Mabado.Connect.ConnectToLab")
                {
                    status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }

                if (commandName == "Mabado.Connect.OpenSolution")
                {
                    status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
                IPathProvider serverPathProvider = new BranchPathProvider(_applicationObject);

                MabadoViewBootLoader viewBootLoader = new MabadoViewBootLoader(serverPathProvider, _applicationObject.SourceControl, new OutputWindowLogger(_applicationObject));

				if(commandName == "Mabado.Connect.ConnectToLab")
				{
                    if(_applicationObject.Solution.IsOpen)
                    {
                        viewBootLoader.ConnectToLab();
                    }
                    else
                    {
                        MessageBox.Show("Open solution first.", "MabaDo", MessageBoxButtons.OK);
                    }

				    handled = true;
				}

                if (commandName == "Mabado.Connect.OpenSolution")
                {
                    if(_applicationObject.Solution.IsOpen)
                    {
                        viewBootLoader.LaunchSolution();
                    }
                    else
                    {
                        MessageBox.Show("Open solution first.", "MabaDo", MessageBoxButtons.OK);
                    }

                    handled = true;
                }
			}
		}

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}