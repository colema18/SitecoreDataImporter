﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings {
	
    /// <summary>
    /// this is the class that all fields/properties should extend. 
    /// </summary>
    public class BaseMapping {

		#region Properties

		private string _newItemField;
		/// <summary>
		/// the field on the new item that the imported data should be stored in
		/// </summary>
        public string NewItemField {
			get {
				return _newItemField;
			}
			set {
				_newItemField = value;
			}
		}

		private string _HandlerClass;
		/// <summary>
		/// the class that represents the field
		/// </summary>
        public string HandlerClass {
			get {
				return _HandlerClass;
			}
			set {
			_HandlerClass = value;
			}
		}

		private string _HandlerAssembly;
		/// <summary>
		/// the assembly that the class representing this field is stored in
		/// </summary>
        public string HandlerAssembly {
			get {
				return _HandlerAssembly;
			}
			set {
				_HandlerAssembly = value;
			}
		}

        private bool _overwriteValue;
        /// <summary>
        /// the field on the new item that the imported data should be stored in
        /// </summary>
        public bool OverwriteValue
        {
            get
            {
                return _overwriteValue;
            }
            set
            {
                _overwriteValue = value;
            }
        }

		#endregion Properties

		#region Constructor

		public BaseMapping(Item i) {
			NewItemField = i.Fields["To What Field"].Value;
			HandlerClass = i.Fields["Handler Class"].Value;
			HandlerAssembly = i.Fields["Handler Assembly"].Value;
		    OverwriteValue = ((Sitecore.Data.Fields.CheckboxField) i.Fields["Overwrite"]).Checked;
		}

		#endregion Constructor

		#region Methods

		#endregion Methods
	}
}
