//-----------------
// <auto-generated>
//-----------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Serialization;
using Intellidesk.AcadNet.Data.Common.Editors;
using Intellidesk.AcadNet.Data.Models.Entities;
using Intellidesk.AcadNet.Data.Repositories.EF6;
using Intellidesk.AcadNet.Data.Common.Editors;
using Intellidesk.AcadNet.Data.Models;
using Intellidesk.AcadNet.Data.Repositories.EF6;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Intellidesk.AcadNet.Data.Models
{
    [Serializable(), DefaultProperty("FilterName")]
    public class FilterMetaData: BaseEntity
    {
        public FilterMetaData()
        {
            FilterName = "Default";
            LayoutID = -1;
        }

        private bool _active;
        [XmlAttribute("Active")]
        [Category("Generic"), DisplayName("Is active")]
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged(() => Active);
            }
        }

        private string _accessType;
        [XmlElement(ElementName = "AccessType")]
        [Category("Generic"), DisplayName("Access type")]
        [ItemsSource(typeof(AccessTypeItemsSource))]
        public string AccessType
        {
            get { return _accessType; }
            set
            {
                _accessType = value;
                OnPropertyChanged(() => AccessType);
            }
        }

        private string _filterName;
        [XmlElement(ElementName = "FilterName")]
        [Category("Generic"), DisplayName("Filter name")]
        public string FilterName
        {
            get { return _filterName; }
            set
            {
                _filterName = value;
                OnPropertyChanged(() => FilterName);
            }
        }

        [Browsable(false)]
        public decimal LayoutID { get; set; }

        [XmlElement(ElementName = "FSA")]
        [Category("Generic"), Browsable(true), DisplayName("FSA Uploaded")]
        public bool FSA { get; set; }

        private string _layoutName;
        [XmlElement(ElementName = "LayoutName")]
        [Category("Generic"), DisplayName("Layout name")]
        [Required(ErrorMessage = "Layout name not must be empty", AllowEmptyStrings = false)]
        [StringLength(500, ErrorMessage = "Layout name must be 500 characters or less")]
        public string LayoutName
        {
            get { return _layoutName; }
            set
            {
                _layoutName = value;
                OnPropertyChanged(() => LayoutName);
            }
        }

        private string _layoutType;
        [XmlElement(ElementName = "LayoutType")]
        [Category("Generic"), DisplayName("Layout type")]
        [ItemsSource(typeof(LayoutTypesItemsSource))]
        public string LayoutType
        {
            get { return _layoutType; }
            set
            {
                _layoutType = value;
                OnPropertyChanged(() => LayoutType);
            }
        }

        private string _layoutContents;
        [XmlElement(ElementName = "LayoutContents")]
        [Category("Generic"), DisplayName("Layout contents")]
        [Editor(typeof(MultiSelectComboBoxEditor), typeof(MultiSelectComboBoxEditor))]
        public string LayoutContents
        {
            get { return _layoutContents; }
            set
            {
                _layoutContents = value;
                OnPropertyChanged(() => LayoutContents);
            }
        }

        private string _layoutVersion;
        [XmlElement(ElementName = "LayoutVersion")]
        [Category("Generic"), DisplayName("Layout version")]
        public string LayoutVersion
        {
            get { return _layoutVersion; }
            set
            {
                _layoutVersion = value;
                OnPropertyChanged(() => LayoutVersion);
            }
        }

        private string _comment;
        [XmlElement(ElementName = "Comment")]
        [Category("Generic"), DisplayName("Comment")]
        [StringLength(500, ErrorMessage = "Layout name must be 500 characters or less")]
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(() => Comment);
            }
        }

        private string _siteName;
        [XmlElement(ElementName = "SiteName")]
        [Category("Location"), DisplayName("Site")]
        public string SiteName
        {
            get { return _siteName; }
            set
            {
                _siteName = value;
                OnPropertyChanged(() => SiteName);
            }
        }

        private string _buildingLevels;
        [XmlElement(ElementName = "BuildingLevels")]
        [Category("Location"), DisplayName("Building and Levels")]
        [Editor(typeof(MultiSelectComboBoxEditor), typeof(MultiSelectComboBoxEditor))]
        public string BuildingLevels
        {
            get { return _buildingLevels; }
            set
            {
                _buildingLevels = value;
                OnPropertyChanged(() => BuildingLevels);
            }
        }

        private string _cadFileName;
        [XmlElement(ElementName = "CADFileName")]
        [Category("System Data"), DisplayName("Cad file name")]
        [Description("Path by default there is into work directory of project")]
        [Editor(typeof(FileNameEditor), typeof(FileNameEditor))]
        public string CADFileName
        {
            get { return _cadFileName; }
            set
            {
                _cadFileName = value;
                OnPropertyChanged(() => CADFileName);
            }
        }

        private string _createdBy;
        [XmlElement(ElementName = "CreatedBy")]
        [Category("System Data"), Browsable(true), DisplayName("Created by"), ReadOnly(true)]
        public string CreatedBy
        {
            get { return _createdBy; }
            set
            {
                _createdBy = value;
                OnPropertyChanged(() => CreatedBy);
            }
        }

        private DateTime? _dateCreated;
        [XmlElement(ElementName = "DateCreated", IsNullable = true)]
        [Category("System Data"), Browsable(true), DisplayName("Date created"), ReadOnly(true)]
        public DateTime? DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                OnPropertyChanged(() => DateCreated);
            }
        }

        private string _modifiedBy;
        [XmlElement(ElementName = "ModifiedBy")]
        [Category("System Data"), Browsable(true), DisplayName("Modified by"), ReadOnly(true)]
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set
            {
                _modifiedBy = value;
                OnPropertyChanged(() => ModifiedBy);
            }
        }

        private DateTime? _dateModified;
        [XmlElement(ElementName = "DateModified", IsNullable = true)]
        [Category("System Data"), Browsable(true), DisplayName("Date modified"), ReadOnly(true)]
        public DateTime? DateModified
        {
            get { return _dateModified; }
            set
            {
                _dateModified = value;
                OnPropertyChanged(() => DateCreated);
            }
        }

        [XmlElement(ElementName = "LayoutState", IsNullable = true)]
        [Category("System Data"), Browsable(true), DisplayName("Layout state"), ReadOnly(true)]
        public short? LayoutState { get; set; }
    }
}