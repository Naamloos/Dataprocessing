using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DatabaseHelper.Models
{
    [Table("globalterrorismdatabase")]
    public class TerrorismEvent
    {
        public long eventid { get; set; }
        public int? iyear { get; set; }
        public int? imonth { get; set; }
        public int? iday { get; set; }
        public string approxdate { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool extended { get; set; }
        public string resolution { get; set; }
        public int? country { get; set; }
        public string country_txt { get; set; }
        public int? region { get; set; }
        public string region_txt { get; set; }
        public string provstate { get; set; }
        public string city { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int? specificity { get; set; }
        public int? vicinity { get; set; }
        public string location { get; set; }
        public string summary { get; set; }
        public int? crit1 { get; set; }
        public int? crit2 { get; set; }
        public int? crit3 { get; set; }
        public int? doubtterr { get; set; }
        public int? alternative { get; set; }
        public string alternative_txt { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool multiple { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool success { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool suicide { get; set; }
        public int? attacktype1 { get; set; }
        public string attacktype1_txt { get; set; }
        public int? attacktype2 { get; set; }
        public string attacktype2_txt { get; set; }
        public int? attacktype3 { get; set; }
        public string attacktype3_txt { get; set; }
        public int? targtype1 { get; set; }
        public string targtype1_txt { get; set; }
        public int? targsubtype1 { get; set; }
        public string targsubtype1_txt { get; set; }
        public string corp1 { get; set; }
        public string target1 { get; set; }
        public int? natlty1 { get; set; }
        public string natlty1_txt { get; set; }
        public int? targtype2 { get; set; }
        public string targtype2_txt { get; set; }
        public int? targsubtype2 { get; set; }
        public string targsubtype2_txt { get; set; }
        public string corp2 { get; set; }
        public string target2 { get; set; }
        public int? natlty2 { get; set; }
        public string natlty2_txt { get; set; }
        public int? targtype3 { get; set; }
        public string targtype3_txt { get; set; }
        public int? targsubtype3 { get; set; }
        public string targsubtype3_txt { get; set; }
        public string corp3 { get; set; }
        public string target3 { get; set; }
        public int? natlty3 { get; set; }
        public string natlty3_txt { get; set; }
        public string gname { get; set; }
        public string gsubname { get; set; }
        public string gname2 { get; set; }
        public string gsubname2 { get; set; }
        public string gname3 { get; set; }
        public string gsubname3 { get; set; }
        public string motive { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool guncertain1 { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool guncertain2 { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool guncertain3 { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool individual { get; set; }
        public int? nperps { get; set; }
        public int? nperpcap { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool claimed { get; set; }
        public int? claimmode { get; set; }
        public string claimmode_txt { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool claim2 { get; set; }
        public int? claimmode2 { get; set; }
        public string claimmode2_txt { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool claim3 { get; set; }
        public int? claimmode3 { get; set; }
        public string claimmode3_txt { get; set; }
        public string compclaim { get; set; }
        public int? weaptype1 { get; set; }
        public string weaptype1_txt { get; set; }
        public int? weapsubtype1 { get; set; }
        public string weapsubtype1_txt { get; set; }
        public int? weaptype2 { get; set; }
        public string weaptype2_txt { get; set; }
        public int? weapsubtype2 { get; set; }
        public string weapsubtype2_txt { get; set; }
        public int? weaptype3 { get; set; }
        public string weaptype3_txt { get; set; }
        public int? weapsubtype3 { get; set; }
        public string weapsubtype3_txt { get; set; }
        public int? weaptype4 { get; set; }
        public string weaptype4_txt { get; set; }
        public int? weapsubtype4 { get; set; }
        public string weapsubtype4_txt { get; set; }
        public string weapdetail { get; set; }
        public int? nkill { get; set; }
        public int? nkillus { get; set; }
        public int? nkillter { get; set; }
        public int? nwound { get; set; }
        public int? nwoundus { get; set; }
        public int? nwoundte { get; set; }
        public int? property { get; set; }
        public int? propextent { get; set; }
        public string propextent_txt { get; set; }
        public int? propvalue { get; set; }
        public string propcomment { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool ishostkid { get; set; }
        public int? nhostkid { get; set; }
        public int? nhostkidus { get; set; }
        public int? nhours { get; set; }
        public int? ndays { get; set; }
        public string divert { get; set; }
        public string kidhijcountry { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool ransom { get; set; }
        public int? ransomamt { get; set; }
        public int? ransomamtus { get; set; }
        public int? ransompaid { get; set; }
        public int? ransompaidus { get; set; }
        public string ransomnote { get; set; }
        public int? hostkidoutcome { get; set; }
        public string hostkidoutcome_txt { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public bool nreleased { get; set; }
        public string addnotes { get; set; }
        public string scite1 { get; set; }
        public string scite2 { get; set; }
        public string scite3 { get; set; }
        public string dbsource { get; set; }

        [XmlElement(ElementName = "int_log")]
        [JsonPropertyName("int_log")]
        public int? INT_LOG { get; set; }

        [XmlElement(ElementName = "int_ideo")]
        [JsonPropertyName("int_ideo")]
        public int? INT_IDEO { get; set; }

        [XmlElement(ElementName = "int_misc")]
        [JsonPropertyName("int_misc")]
        public int? INT_MISC { get; set; }

        [XmlElement(ElementName = "int_any")]
        [JsonPropertyName("int_any")]
        public int? INT_ANY { get; set; }

        public string related { get; set; }
    }
}
