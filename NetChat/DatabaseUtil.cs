using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Permissions;

namespace CriChat
{
    #region Message Class
    public class Message
    {
        public string Sender
        {
            get;
            private set;
        }
        public string Receiver
        {
            get;
            private set;
        }
        public DateTime Date
        {
            get;
            private set;
        }
        public string MessageText
        {
            get;
            private set;
        }
        
        public Message(string sender, string receiver, string message, DateTime date)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.MessageText = message;
            this.Date = date;
        }
    }
    #endregion

    public class DatabaseUtil : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString
        {
            get;
            private set;
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> UserDataChanged;
        public event EventHandler<EventArgs> MessageDataChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public DatabaseUtil(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Make sure the sql dependency can work
        /// </summary>
        public void CheckUserPermissions()
        {
            try
            {
                SqlClientPermission permissions = new SqlClientPermission(PermissionState.Unrestricted);

                //if we cann Demand() it will throw an exception if the current user
                //doesnt have the proper permissions
                permissions.Demand();

            }
            catch(Exception ex)
            {
                throw new Exception("The current user does not have enough rights to run this application on the current machine or the database connection string is invalid. ", ex);
            }
        }

        #region SQL Dependency
        private string _connectedUserName;
        /// <summary>
        /// Initializes the sql dependency for the current user
        /// </summary>
        /// <param name="username"></param>
        private void InitializeUsersSqlDependency()
        {
            if (this._connectedUserName == null)
            {// return if no user connected
                return;
            }

            // create sql dependency for users table
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select username  
                                                         from dbo.users where username<>@connectedUser", 
                                                        con))
                {
                    cmd.Notification = null;
                    cmd.Parameters.Add(new SqlParameter("connectedUser", _connectedUserName));
                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += new OnChangeEventHandler(dependency_user_OnChange);
                    cmd.ExecuteReader();
                }
            }
        }
        void dependency_user_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (this.UserDataChanged != null)
            {
                this.UserDataChanged(this, null);
            }

            // reinitialize the dependency
            InitializeUsersSqlDependency();
        }

        /// <summary>
        /// Initializes the sql dependency for the current user for messages
        /// </summary>
        /// <param name="username"></param>
        private void InitializeMessagesSqlDependency()
        {
            if (this._connectedUserName == null)
            {// return if no user connected
                return;
            }

            // monitor messages
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select message from dbo.messages where receiver=@connectedUser and isnew=1 and isarchived=0",
                                                        con))
                {
                    cmd.Notification = null;
                    cmd.Parameters.Add(new SqlParameter("connectedUser", _connectedUserName));
                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += new OnChangeEventHandler(dependency_messages_OnChange);
                    cmd.ExecuteReader();
                }
            }
        }
        void dependency_messages_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (this.MessageDataChanged != null)
            {
                this.MessageDataChanged(this, null);
            }

            // reinitialize the dependency
            InitializeMessagesSqlDependency();
        }
        #endregion
        #endregion

        #region Database Operations
        /// <summary>
        /// Checks whether the specified username is already in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserConnected(string username)
        {
            return ((int)this.ExecuteScalar("select count(*) from users where username=@username", new KeyValuePair<string, string>("username", username))) > 0;
        }

        /// <summary>
        /// Connects the specified username
        /// </summary>
        /// <param name="username"></param>
        public void Connect(string username)
        {
            if (this.IsUserConnected(username))
            {
                throw new Exception("User already connected, please choose a different username and try again.");
            }
            try
            {
                this.ExecuteNonQuery("insert into users ([username]) values (@username)", new KeyValuePair<string, string>("username", username));
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error. ", ex);
            }

            // start the sql dependency
            SqlDependency.Start(this.ConnectionString);
            this._connectedUserName = username;
            InitializeUsersSqlDependency();
            InitializeMessagesSqlDependency();
        }

        /// <summary>
        /// Disconnects the specified usernames
        /// </summary>
        /// <param name="username"></param>
        public void Disconnect(string username)
        {
            if (!this.IsUserConnected(username))
            {
                throw new Exception("User not connected.");
            }
            try
            {
                this.ExecuteNonQuery("delete from users where username=@username", new KeyValuePair<string, string>("username", username));
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error. ", ex);
            }
            // stop the sql dependency
            SqlDependency.Stop(this.ConnectionString);
            // archive messages from this sender
            ArchiveMessages(username);
            this._connectedUserName = null;
        }

        /// <summary>
        /// Get the users and message counts for the specified connected user
        /// </summary>
        /// <param name="connectedUser"></param>
        /// <returns></returns>
        public Dictionary<string,string> GetUsers(string connectedUser)
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            using (DataTable dt = this.ExecuteTable(@"select username, 
                                                            (select count(*) from messages where sender=username and receiver=@connectedUser and isnew=1 and isarchived=0) as NewMessages 
                                                      from users where username<>@connectedUser",
                                                      new KeyValuePair<string,string> ("connectedUser", connectedUser)))
            {
                foreach (DataRow row in dt.Rows)
                {
                    var username = row["username"].ToString();
                    var display = row["username"].ToString();
                    var newMessages = row["newmessages"].ToString();
                    if (newMessages != "0")
                    {   
                        display = string.Format("{0} ({1})", display, newMessages);
                    }
                    users.Add(username, display);
                }
            }
            return users;
        }

        /// <summary>
        /// Gets the messages for the specified sender and receiver
        /// </summary>
        /// <param name="connectedUser"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public Message[] GetMessages(string connectedUser, string sender)
        {
            List<Message> messages = new List<Message>();
            using (DataTable dt = this.ExecuteTable(@"select sender, receiver, message, date from messages 
                                                      where isarchived=0 and 
                                                            (sender=@sender or receiver=@sender) and 
                                                            (receiver=@receiver or sender=@receiver) order by date;",
                                                    new KeyValuePair<string, string>("sender", sender),
                                                    new KeyValuePair<string, string>("receiver", connectedUser)))
            {
                foreach (DataRow row in dt.Rows)
                {
                    messages.Add(new Message(row["sender"].ToString(), row["receiver"].ToString(), row["message"].ToString(), Convert.ToDateTime(row["date"])));
                }
            }
            UpdateMessagesReadStatus(sender, connectedUser);
            return messages.ToArray();
        }

        /// <summary>
        /// Updates the read status for the specified sender and receiver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        public void UpdateMessagesReadStatus(string sender, string receiver)
        {
            this.ExecuteNonQuery("update messages set isnew=0 where sender=@sender and receiver=@receiver and isarchived=0",
                                 new KeyValuePair<string, string>("sender", sender),
                                 new KeyValuePair<string, string>("receiver", receiver));
        }

        /// <summary>
        /// Adds a message to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="message"></param>
        public void InsertMessage(string sender, string receiver, string message)
        {
            this.ExecuteNonQuery("insert into messages (sender, receiver, message, date, isnew, isarchived) values (@sender,@receiver,@message,getdate(), 1, 0)",
                                 new KeyValuePair<string, string>("sender", sender),
                                 new KeyValuePair<string, string>("receiver", receiver),
                                 new KeyValuePair<string, string>("message", message));
        }

        /// <summary>
        /// Archive messages for the specified user
        /// </summary>
        /// <param name="sender"></param>
        public void ArchiveMessages(string sender)
        {
            this.ExecuteNonQuery("update messages set isarchived=1 where sender=@sender or receiver=@sender",
                                 new KeyValuePair<string, string>("sender", sender));
        }
        #endregion

        #region sql helper methods
        private DataTable ExecuteTable(string sqlCommand, params KeyValuePair<string, string>[] sqlParams)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlCommand, con))
                {
                    if (sqlParams != null)
                    {
                        foreach (KeyValuePair<string, string> param in sqlParams)
                        {
                            cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                        }
                    }
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        private int ExecuteNonQuery(string sqlCommand, params KeyValuePair<string, string>[] sqlParams)
        {
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlCommand, con))
                {
                    if (sqlParams != null)
                    {
                        foreach (KeyValuePair<string, string> param in sqlParams)
                        {
                            cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        private object ExecuteScalar(string sqlCommand, params KeyValuePair<string, string>[] sqlParams)
        {
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlCommand, con))
                {
                    if (sqlParams != null)
                    {
                        foreach (KeyValuePair<string, string> param in sqlParams)
                        {
                            cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                        }
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        public void Dispose()
        {
            // reset event handlers
            SqlDependency.Stop(this.ConnectionString);
        }
    }
}
