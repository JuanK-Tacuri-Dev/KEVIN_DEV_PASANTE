﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Integration.Orchestrator.Backend.Domain.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class AppMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Integration.Orchestrator.Backend.Domain.Resources.AppMessages", typeof(AppMessages).Assembly);
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
        ///   Looks up a localized string similar to Domain layer error.
        /// </summary>
        public static string Exception_ArgumentException {
            get {
                return ResourceManager.GetString("Exception_ArgumentException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Api layer error.
        /// </summary>
        public static string Exception_BadRequestException {
            get {
                return ResourceManager.GetString("Exception.BadRequestException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Business exception.
        /// </summary>
        public static string Exception_BussinesException {
            get {
                return ResourceManager.GetString("Exception_BussinesException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Integration Exception.
        /// </summary>
        public static string Exception_IntegrationException {
            get {
                return ResourceManager.GetString("Exception_IntegrationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application layer error.
        /// </summary>
        public static string Exception_InvalidRequestException {
            get {
                return ResourceManager.GetString("Exception.InvalidRequestException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected error.
        /// </summary>
        public static string Exception_UnexpectedException {
            get {
                return ResourceManager.GetString("Exception.UnexpectedException", resourceCulture);
            }
        }
    }
}
