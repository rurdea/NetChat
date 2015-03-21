using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;

namespace CriChat
{
    public partial class NetChatForm : Form
    {
        #region Members
        private DatabaseUtil _databaseUtil;
        private string _connectedUser = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public NetChatForm()
        {
            InitializeComponent();
            ChangeUiState(false);
            InitializeDatabase();
        }
        #endregion

        #region Initialize/Binding
        /// <summary>
        /// Initializez the database utility class
        /// </summary>
        private void InitializeDatabase()
        {
            AddStatusMessage("Initializing database...");
            try
            {
                if (_databaseUtil != null)
                {
                    _databaseUtil.Dispose();
                }
                _databaseUtil = new DatabaseUtil(ConfigurationManager.ConnectionStrings["CriChat.Properties.Settings.DatabaseConnectionString"].ConnectionString);
                _databaseUtil.CheckUserPermissions();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Binds the active users to the users list and messages to the message box
        /// </summary>
        private void BindActiveUsers()
        {
            // unsubscribe to change event
            lstClients.SelectedIndexChanged -= lstClients_SelectedIndexChanged;
            // save the active chat
            var activeSender = lstClients.SelectedValue;
            // get all users
            var users = _databaseUtil.GetUsers(_connectedUser);

            if (users.Count == 0)
            {
                lstClients.Items.Clear();
                lstClients.SelectionMode = SelectionMode.None;
                lstClients.Items.Add("No clients connected");
            }
            else
            {
                lstClients.DataSource = new BindingSource(users, null);
                lstClients.DisplayMember = "Value";
                lstClients.ValueMember = "Key";

                lstClients.ClearSelected();  
                lstClients.SelectionMode = SelectionMode.One;
                if (activeSender!=null)
                {
                    // reselect the active chat person from the list
                    lstClients.SelectedValue = activeSender;
                }
            }
            // check if the user went offline
            if (activeSender != null && lstClients.SelectedValue == null)
            {
                txtMessageBoard.Text = string.Concat(txtMessageBoard.Text, Environment.NewLine, "User went offline. ");
            }

            // resubscribe to selected index changed
            lstClients.SelectedIndexChanged +=new EventHandler(lstClients_SelectedIndexChanged);
        }

        /// <summary>
        /// Binds the messages between the connected user and the selected receiver
        /// </summary>
        private void BindMessages()
        {
            // clear the textbox
            txtMessageBoard.Text = string.Empty;

            if (lstClients.SelectedValue!=null)
            {
                // get the sender
                var sender = lstClients.SelectedValue.ToString();
                // get all messages
                Message[] messages = _databaseUtil.GetMessages(_connectedUser, sender);

                foreach (var message in messages)
                {
                    AddMessageToBoard(message.Sender.Equals(_connectedUser) ? "You" : message.Sender, message.MessageText, message.Date);
                }
            }
        }
        #endregion

        #region UI
        /// <summary>
        /// Changes the ui controls enable state for the appropiate state (connected/disconnected)
        /// </summary>
        /// <param name="connected"></param>
        private void ChangeUiState(bool connected)
        {
            grpSettings.Enabled = !connected;
            grpMessageBox.Enabled = grpDisconnect.Enabled = connected;
        }

        /// <summary>
        /// Shows an error dialog
        /// </summary>
        /// <param name="ex"></param>
        private void ShowError(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder();
            while (ex != null)
            {
                errorMessage.Append(ex.Message);
                ex = ex.InnerException;
            }
            var error = errorMessage.ToString();
            MessageBox.Show(error);
            AddStatusMessage(error);
        }

        /// <summary>
        /// Adds a message to the message board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="date"></param>
        private void AddMessageToBoard(string sender, string message, DateTime date)
        {
            txtMessageBoard.AppendText(string.Format("[{0}] {1}: {2}{3}", date, sender, message, Environment.NewLine));
        }

        /// <summary>
        /// Adds a status message if the status checkbox is checkeds
        /// </summary>
        /// <param name="message"></param>
        private void AddStatusMessage(string message)
        {
            if (checkBox1.Checked)
            {
                txtStatus.AppendText(string.Format("[{0}]\t{1}{2}", DateTime.Now, message, Environment.NewLine));
            }
        }
        #endregion

        #region Event Handlers
        #region Buttons Events
        /// <summary>
        /// Handler method for settings click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            if (settings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddStatusMessage("Database settings changed");
                // re initialize database
                InitializeDatabase();
            }
        }

        /// <summary>
        /// Send button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Please enter your message. ");
                return;
            }
            if (lstClients.SelectedValue==null)
            {
                MessageBox.Show("Please select a chat person to send the message to.");
                return;
            }

            _databaseUtil.InsertMessage(_connectedUser, lstClients.SelectedValue.ToString(), txtMessage.Text);
            AddMessageToBoard("You", txtMessage.Text, DateTime.Now);
            txtMessage.Text = string.Empty;
        }

        /// <summary>
        /// Handler method to connect button click event. Connects the specified user to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("Please enter your username!");
                txtUserName.Focus();
                return;
            }
            else if (!Regex.Match(txtUserName.Text, "^[a-zA-Z0-9]+$").Success)
            {
                MessageBox.Show("Invalid username (it should contain only letters and numbers)!");
                txtUserName.Focus();
                return;
            }

            try
            {
                _databaseUtil.Connect(txtUserName.Text);
                _connectedUser = txtUserName.Text;
                BindActiveUsers();
                // subscribe to events
                _databaseUtil.UserDataChanged += new EventHandler<EventArgs>(_databaseUtil_UserDataChanged);
                _databaseUtil.MessageDataChanged += new EventHandler<EventArgs>(_databaseUtil_MessageDataChanged);

                ChangeUiState(true);
                AddStatusMessage(string.Format("Connected to chat as {0}.", _connectedUser));
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }

        }

        /// <summary>
        /// Status checkbox checked changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                txtStatus.Text = string.Empty;
            }
        }

        /// <summary>
        /// Disconnects the current user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _databaseUtil.Disconnect(txtUserName.Text);
                _connectedUser = null;
                ChangeUiState(false);
                // unsubscribe to events
                _databaseUtil.UserDataChanged -= _databaseUtil_UserDataChanged;
                _databaseUtil.MessageDataChanged -= _databaseUtil_MessageDataChanged;
                AddStatusMessage("Disconnected.");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstClients.SelectedValue != null)
            {
                // bind messages when the selected index changes
                BindMessages();
                // rebind users
                BindActiveUsers();
            }
        }
        #endregion

        #region Database Events
        /// <summary>
        /// user connected/disconnected event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _databaseUtil_UserDataChanged(object sender, EventArgs e)
        {
            // make sure the call is made in the main thread
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { _databaseUtil_UserDataChanged(sender, e); }));
                return;
            }
            AddStatusMessage("Client list changed (user connected/disconnected).");
            // reload the users
            BindActiveUsers();
        }

        /// <summary>
        /// new message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _databaseUtil_MessageDataChanged(object sender, EventArgs e)
        {
            // make sure the call is made in the main thread
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { _databaseUtil_MessageDataChanged(sender, e); }));
                return;
            }
            AddStatusMessage("New message received.");
            // reload the messaged and the users (the new emssage might be from another user than the selected one)
            BindMessages();
            BindActiveUsers();
        }
        #endregion
        #endregion

        #region Overridden
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // make sure to disconnect the current user
            if (!string.IsNullOrEmpty(_connectedUser))
            {
                try
                {
                    _databaseUtil.Disconnect(_connectedUser);
                }
                catch { }
            }
        }
        #endregion

        
    }
}
