﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Sitecore.SharedSource.DataImporter.Mappings.Fields;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using Sitecore.Web.UI.WebControls;

namespace Sitecore.SharedSource.DataImporter.Providers {
	public class CSVDataMap : BaseDataMap {

		#region Properties

		private string _FieldDelimiter = string.Empty;
		public string FieldDelimiter {
			get {
				return (_FieldDelimiter.Equals(string.Empty)) ? "," : _FieldDelimiter;
			}
			set {
				_FieldDelimiter = value;
			}
		}

		private string _EncodingType = string.Empty;
		public string EncodingType {
			get {
				return _EncodingType;
			}
			set {
				_EncodingType = value;
			}
		}

		#endregion Properties

        #region Constructor

		public CSVDataMap(Database db, string ConnectionString, Item importItem)
            : base(db, ConnectionString, importItem) {

			FieldDelimiter = importItem["Field Delimiter"];
			EncodingType = importItem["Encoding Type"];
		}

        #endregion Constructor

        #region Override Methods

		/// <summary>
		/// uses the query field to retrieve file data
		/// </summary>
		/// <returns></returns>
        public override IEnumerable<object> GetImportData() {

			if (!File.Exists(this.Query)) {
				Log("Error", string.Format("the file: '{0}' could not be found. Try moving the file under the webroot.", this.Query));
				return Enumerable.Empty<object>();
			}

			Encoding et = Encoding.GetEncoding(1252);
			int ei = -1;
			if(!EncodingType.Equals("")) {
				Encoding eTemp; 
				if(int.TryParse(EncodingType, out ei))
					eTemp = Encoding.GetEncoding(ei);
				else
					eTemp = Encoding.GetEncoding(EncodingType);
				if (eTemp != null)
					et = eTemp;
			}

			byte[] bytes = GetFileBytes(this.Query);
			string data = et.GetString(bytes);

			//split urls by breaklines
			List<string> lines = SplitString(data, "\n");
			
			return lines;
        }

	    public override IEnumerable<object> SyncDeletions()
	    {
	        throw new NotImplementedException();
	    }

	    public override void TakeHistorySnapshot()
	    {
	        throw new NotImplementedException();
	    }

	    /// <summary>
		/// There is no custom data for this type
		/// </summary>
		/// <param name="newItem"></param>
		/// <param name="importRow"></param>
		public override void ProcessCustomData(ref Item newItem, object importRow) {
		}

		/// <summary>
		/// gets a field value from an item
		/// </summary>
		/// <param name="importRow"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		protected override string GetFieldValue(object importRow, string fieldName) {
			
			string item = importRow as string;
			List<string> cols = SplitString(item, FieldDelimiter);
			
			int pos = -1;
			string s = string.Empty;
			if(int.TryParse(fieldName, out pos) && (cols[pos] != null))
				s = cols[pos];
			return s;
		}

		#endregion Override Methods

        #region Methods
		
		protected List<string> SplitString(string str, string splitter){
			return str.Split(new string[] { splitter }, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		protected byte[] GetFileBytes(string filePath) {
			//open the file selected
			FileInfo f = new FileInfo(filePath);
			FileStream s = f.OpenRead();
			byte[] bytes = new byte[s.Length];
			s.Position = 0;
			s.Read(bytes, 0, int.Parse(s.Length.ToString()));
			return bytes;
		}

        #endregion Methods
    }
}
