﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.32559
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClipboardChangerPlant.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClipboardChangerPlant.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon clip2net {
            get {
                object obj = ResourceManager.GetObject("clip2net", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon klipper {
            get {
                object obj = ResourceManager.GetObject("klipper", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon klipperActiveProcess {
            get {
                object obj = ResourceManager.GetObject("klipperActiveProcess", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon klipperEmpty {
            get {
                object obj = ResourceManager.GetObject("klipperEmpty", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon klipperFault {
            get {
                object obj = ResourceManager.GetObject("klipperFault", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon klipperInProgress {
            get {
                object obj = ResourceManager.GetObject("klipperInProgress", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon klipperSuccess {
            get {
                object obj = ResourceManager.GetObject("klipperSuccess", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;ClipboardChangerPlant&gt;
        ///    &lt;RequestHandlers&gt;
        ///      &lt;RequestHandler name=&quot;Youtube&quot; type=&quot;ClipboardChangerPlant.RequestHandling.Specialized.SimpleHandler&quot;&gt;
        ///        &lt;ShouldBeShorted&gt;True&lt;/ShouldBeShorted&gt;
        ///        &lt;SuccessIconResourceName&gt;youtube&lt;/SuccessIconResourceName&gt;
        ///        &lt;MatchRegExpressions&gt;http://www.youtube.com/watch.*|&lt;/MatchRegExpressions&gt;
        ///      &lt;/RequestHandler&gt;
        ///
        ///      &lt;RequestHandler name=&quot;Clip2NetWithoutWebRequest&quot; type=&quot;ClipboardChangerPlant.RequestHandling.Specialized.Clip2NetWithout [rest of string was truncated]&quot;;.
        /// </summary>
        public static string XmlConfiguration {
            get {
                return ResourceManager.GetString("XmlConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon youtube {
            get {
                object obj = ResourceManager.GetObject("youtube", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
    }
}
