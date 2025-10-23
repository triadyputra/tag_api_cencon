using cenconApi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using cenconApi.Model.ViewForm;

namespace cenconApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReqOpenClose> ReqOpenClose { get; set; }

        public DbSet<ViewOrderCenconResult> ViewOrderCenconResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tambahkan index untuk performa query
            builder.Entity<ReqOpenClose>()
                .HasIndex(x => x.NOWO);

            builder.Entity<ReqOpenClose>()
                .HasIndex(x => x.WSID);

            builder.Entity<ReqOpenClose>()
                .HasIndex(x => x.Tanggal);


            // tidak dihubungkan ke view/tabel fisik
            builder.Entity<ViewOrderCenconResult>()
                .HasNoKey() // penting
                .ToView(null); // tidak dihubungkan ke view/tabel fisik
        }
   
    }
}
