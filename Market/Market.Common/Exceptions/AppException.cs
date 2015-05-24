using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// An exception definition to act as a container for all the exceptions happened in business level.
/// You can set a error code for each kind of exception, and this exception can give an appropriate message for it.
/// </summary>
public class AppException : Exception
{
    /// <summary>
    /// A collection of objects of type <see cref="ResourceManager"/>. 
    /// </summary>
    private static ISet<ResourceManager> resourceManagerSet = new HashSet<ResourceManager>();

    /// <summary>
    /// The default resources manager for exception handling. This is required. When there is no additional resources managers in 
    /// the list, this is where we are going to use to find the error message definition.
    /// </summary>
    private static ResourceManager resourceManager;

    /// <summary>
    /// object will be used to implement a lock
    /// </summary>
    private static object lockObj = new object();

    /// <summary>
    /// Error code of this exception.
    /// It works like an ID of some exception, so we can find an appropriate explanation for this exception.
    /// </summary>
    private int errorCode;

    /// <summary>
    /// Parameters related to this exception
    /// </summary>
    private object[] args;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppException"/> class.
    /// </summary>
    /// <param name="errorCode">Tells what kind of exception this is.</param>
    public AppException(int errorCode)
    {
        this.errorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppException"/> class.
    /// And tells what kind of exception this is.
    /// </summary>
    /// <param name="errorCode">Tells what kind of exception this is.</param>
    /// <param name="args">Tells some arguments for this exception so we can generate error message with more details.</param>
    public AppException(int errorCode, params object[] args)
    {
        this.errorCode = errorCode;
        this.args = args;
    }

    /// <summary>
    /// Gets defined error message with error code
    /// </summary>
    public string ErrorMsg
    {
        get
        {
            string msg = resourceManager.GetString("ErrCode" + this.errorCode);
            if (msg == null)
            {
                foreach (ResourceManager resMgr in resourceManagerSet)
                {
                    if ((msg = resMgr.GetString("ErrCode" + this.errorCode)) != null)
                    {
                        break;
                    }
                }
            }

            if (msg != null && this.args != null)
            {
                msg = string.Format(msg, this.args);
            }

            return msg;
        }
    }

    /// <summary>
    /// Gets the error code of this exception.
    /// </summary>
    public int ErrorCode
    {
        get { return this.errorCode; }
    }

    /// <summary>
    /// Gets the parameters for this exception.
    /// </summary>
    public object[] Params
    {
        get { return this.args; }
    }

    /// <summary>
    /// Override the Message from base class so we can provide a more appropriate version.
    /// </summary>
    public override string Message
    {
        get
        {
            return this.ErrorMsg;
        }
    }

    /// <summary>
    /// Initializes this object with an object of <see cref="ResourceManager"/> class.
    /// </summary>
    /// <param name="resourceManager">The resources manager will be used to the default one for holding the error message definitions.</param>
    public static void Init(ResourceManager resourceManager)
    {
        AppException.resourceManager = resourceManager;
    }

    /// <summary>
    /// Add ResourceManager into list
    /// </summary>
    /// <param name="resourceManager">Add a resource manager into list.</param>
    public static void AddResourceBundle(ResourceManager resourceManager)
    {
        lock (lockObj)
        {
            if (resourceManager != null)
            {
                resourceManagerSet.Add(resourceManager);
            }
        }
    }

    /// <summary>
    /// Remove ResourceManager from set
    /// </summary>
    /// <param name="resourceManager">The resource manager will be removed.</param>
    public static void RmoveResourceBundle(ResourceManager resourceManager)
    {
        lock (lockObj)
        {
            if (resourceManager != null)
            {
                resourceManagerSet.Remove(resourceManager);
            }
        }
    }
}