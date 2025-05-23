﻿using Convenience.Models.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Convenience.Models.DataModels
{

    /// <summary>
    /// 注文実績明細DTO
    /// </summary>
    /// <Remarks>
    /// 主キー：注文コード、注文先コード、仕入商品コード、商品コード
    /// </Remarks>
    [Table("chumon_jisseki_meisai")]
    [PrimaryKey(nameof(ChumonId), nameof(ShiireSakiId), nameof(ShiirePrdId), nameof(ShohinId))]
    public class ChumonJissekiMeisai
    {

        [Column("chumon_code")]
        [DisplayName("注文コード")]
        [MaxLength(20)]
        [Required]
        public string ChumonId { get; set; }

        [Column("shiire_saki_code")]
        [DisplayName("仕入先コード")]
        [MaxLength(10)]
        [Required]
        public string ShiireSakiId { get; set; }

        [Column("shiire_prd_code")]
        [DisplayName("仕入商品コード")]
        [MaxLength(10)]
        [Required]
        public string ShiirePrdId { get; set; }

        [Column("shohin_code")]
        [DisplayName("商品コード")]
        [MaxLength(10)]
        [Required]
        public string ShohinId { get; set; }

        [Column("chumon_su")]
        [DisplayName("注文数")]
        [Precision(10, 2)]

        [RegularExpression(@"^-?\d+(\.\d{1,2})?$", ErrorMessage = "{0}は数値（小数点以下2桁まで）で入力してください")]
        [Range(0, 999.99, ErrorMessage = "{0}は、{1}～{2}の範囲で入力してください")]
        [Required(ErrorMessage = "{0}は、必須入力項目です")]
        public decimal ChumonSu { get; set; }

        [Column("chumon_zan")]
        [DisplayName("注文残")]
        [Precision(10, 2)]
        public decimal ChumonZan { get; set; }

        [NotMapped]
        [DisplayName("前回注文数")]
        [Precision(10, 2)]
        public decimal? LastChumonSu { get; set; }

        [ForeignKey(nameof(ChumonId))]
        public virtual ChumonJisseki? ChumonJisseki { get; set; }

        [ForeignKey(nameof(ShiireSakiId) + "," + nameof(ShiirePrdId) + "," + nameof(ShohinId))]
        public virtual ShiireMaster? ShiireMaster { get; set; }

        public virtual ICollection<ShiireJisseki>? ShiireJisseki { get; set; }

        [Timestamp]
        public uint Version { get; set; }
    }
}