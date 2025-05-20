using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Convenience.Models.DataModels;

namespace Convenience.Data
{
    public class ConvenienceContext : DbContext
    {
        //コンストラクタ
        public ConvenienceContext (DbContextOptions<ConvenienceContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DBを使わないモデル用
        /// </summary>
        public ConvenienceContext()
        {

        }

        //プロパティ
        public DbSet<Convenience.Models.DataModels.ChumonJisseki> ChumonJisseki { get; set; } = default!;

        public DbSet<Convenience.Models.DataModels.ChumonJissekiMeisai> ChumonJissekiMeisai { get; set; } = default!;

        public DbSet<Convenience.Models.DataModels.ShiireMaster> ShiireMaster { get; set; } = default!;

        public DbSet<Convenience.Models.DataModels.ShiireSakiMaster> ShiireSakiMaster { get; set; } = default!;

        public DbSet<Convenience.Models.DataModels.ShohinMaster> ShohinMaster { get; set; } = default!;
    }
}
