using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.GroupOrganization.AnAssociations
{
    public class AnAssociationInput
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 社团名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 社团简介
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 学校地址
        /// </summary>
        public string SchoolAddress { get; set; }



        public AnAssociationInput()
        { }

        public AnAssociationInput(AnAssociation an)
        {
            ID = an.ID;
            Name = an.Name;
            Description = an.Description;
            SortCode = an.SortCode;
            SchoolAddress = an.SchoolAddress;
        }

        public void MapTo(AnAssociation an)
        {
            an.ID = ID;
            an.Name = Name;
            an.Description = Description;
            an.SortCode = SortCode;
            an.SchoolAddress = SchoolAddress;
        }
    }
}
