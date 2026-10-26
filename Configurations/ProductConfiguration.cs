using Halabsk.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halabsk.Net.Data.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasMaxLength(160)
            .IsRequired();

        // decimal(18,2) safely represents the allowed 0.01–999999.99 range
        // and stores currency with exactly two fractional digits.
        builder.Property(product => product.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(product => product.Image)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasIndex(product => new { product.CategoryId, product.Id });

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Product { Id = 1, CategoryId = 1, Name = "بنطلون كاجوال بيج", Price = 520m, Description = "ستايل عملي وسهل التنسيق.", Image = "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 2, CategoryId = 1, Name = "بنطلون أسود أوفر سايز", Price = 610m, Description = "قصة عصرية ولمسة streetwear.", Image = "https://images.unsplash.com/photo-1506629905607-d9c297d2bf5c?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 3, CategoryId = 1, Name = "بنطلون جينز أزرق فاتح", Price = 580m, Description = "جينز يومي مريح للمشاوير والخروجات.", Image = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 4, CategoryId = 1, Name = "بنطلون جينز غامق slim", Price = 640m, Description = "قصة slim تعطي مظهرًا أنيقًا.", Image = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 5, CategoryId = 1, Name = "بنطلون لينن أبيض", Price = 690m, Description = "خفيف ومناسب للأجواء الصيفية.", Image = "https://images.unsplash.com/photo-1507679799987-c73779587ccf?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 6, CategoryId = 1, Name = "بنطلون كارغو زيتوني", Price = 730m, Description = "طابع عملي وجيوب متعددة.", Image = "https://images.unsplash.com/photo-1514996937319-344454492b37?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 7, CategoryId = 1, Name = "بنطلون كارغو أسود", Price = 760m, Description = "مظهر قوي ومناسب للوك streetwear.", Image = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 8, CategoryId = 1, Name = "بنطلون رياضي رمادي", Price = 430m, Description = "مريح للاستخدام اليومي والرياضة.", Image = "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 9, CategoryId = 1, Name = "بنطلون رياضي أسود", Price = 450m, Description = "خامة ناعمة مع استرتش عملي.", Image = "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 10, CategoryId = 1, Name = "بنطلون تشينو كحلي", Price = 620m, Description = "مناسب للجامعة والعمل والخروجات.", Image = "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 11, CategoryId = 1, Name = "بنطلون تشينو بيج فاتح", Price = 610m, Description = "لون هادئ وسهل التنسيق.", Image = "https://images.unsplash.com/photo-1551232864-3f0890e580d9?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 12, CategoryId = 1, Name = "بنطلون formal أسود", Price = 710m, Description = "إطلالة رسمية أنيقة بقصة مستقيمة.", Image = "https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 13, CategoryId = 1, Name = "بنطلون formal رمادي", Price = 705m, Description = "مناسب للمناسبات والاجتماعات.", Image = "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 14, CategoryId = 1, Name = "بنطلون wide leg بني", Price = 670m, Description = "قصة واسعة بلمسة عصرية.", Image = "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 15, CategoryId = 1, Name = "بنطلون wide leg أوف وايت", Price = 675m, Description = "مظهر ناعم مع راحة طوال اليوم.", Image = "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 16, CategoryId = 1, Name = "بنطلون ripped denim", Price = 650m, Description = "تفاصيل ripped لستايل شبابي.", Image = "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 17, CategoryId = 1, Name = "بنطلون mom fit أزرق", Price = 620m, Description = "قصة مريحة ولوك trendy.", Image = "https://images.unsplash.com/photo-1554412933-514a83d2f3c8?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 18, CategoryId = 1, Name = "بنطلون slim بيج", Price = 595m, Description = "مقاس slim بخامة خفيفة.", Image = "https://images.unsplash.com/photo-1487412947147-5cebf100ffc2?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 19, CategoryId = 1, Name = "بنطلون slim كحلي", Price = 605m, Description = "ستايل أنيق ومناسب لكل الأوقات.", Image = "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 20, CategoryId = 1, Name = "بنطلون summer relaxed", Price = 560m, Description = "خامة تنفسية للمواسم الحارة.", Image = "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 21, CategoryId = 2, Name = "جاكيت أسود خفيف", Price = 780m, Description = "مناسب للخروجات واللوك الأنيق.", Image = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 22, CategoryId = 2, Name = "جاكيت دنيم أزرق", Price = 860m, Description = "قطعة أساسية لستايل كاجوال جريء.", Image = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 23, CategoryId = 2, Name = "جاكيت أوفر سايز بني", Price = 910m, Description = "تفاصيل بسيطة بلمسة فخمة.", Image = "https://images.unsplash.com/photo-1523398002811-999ca8dec234?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 24, CategoryId = 2, Name = "جاكيت bomber أسود", Price = 840m, Description = "ستايل bomber عصري ومريح.", Image = "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 25, CategoryId = 2, Name = "جاكيت bomber زيتي", Price = 845m, Description = "لون مميز يناسب الخروجات اليومية.", Image = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 26, CategoryId = 2, Name = "جاكيت leather أسود", Price = 1150m, Description = "خامة قوية ولمعة أنيقة.", Image = "https://images.unsplash.com/photo-1507679799987-c73779587ccf?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 27, CategoryId = 2, Name = "جاكيت leather بني", Price = 1180m, Description = "مظهر كلاسيكي بلمسة عصرية.", Image = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 28, CategoryId = 2, Name = "جاكيت padded رمادي", Price = 980m, Description = "دافئ وخفيف في نفس الوقت.", Image = "https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 29, CategoryId = 2, Name = "جاكيت padded كحلي", Price = 995m, Description = "مثالي للبرد الخفيف.", Image = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 30, CategoryId = 2, Name = "جاكيت utility بيج", Price = 890m, Description = "ستايل عملي مع جيوب متعددة.", Image = "https://images.unsplash.com/photo-1551232864-3f0890e580d9?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 31, CategoryId = 2, Name = "جاكيت utility أسود", Price = 900m, Description = "عملي ومرن للتنسيقات اليومية.", Image = "https://images.unsplash.com/photo-1487412947147-5cebf100ffc2?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 32, CategoryId = 2, Name = "جاكيت cropped denim", Price = 870m, Description = "قصة قصيرة بستايل شبابي.", Image = "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 33, CategoryId = 2, Name = "جاكيت varsity أسود", Price = 940m, Description = "روح شبابية مستوحاة من varsity.", Image = "https://images.unsplash.com/photo-1554412933-514a83d2f3c8?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 34, CategoryId = 2, Name = "جاكيت varsity رمادي", Price = 950m, Description = "مريح ومميز للجامعة والخروجات.", Image = "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 35, CategoryId = 2, Name = "جاكيت صوفي بيج", Price = 1020m, Description = "خامة صوفية بلمسة ناعمة.", Image = "https://images.unsplash.com/photo-1514996937319-344454492b37?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 36, CategoryId = 2, Name = "جاكيت صوفي زيتي", Price = 1030m, Description = "إحساس دافئ ومظهر هادئ.", Image = "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 37, CategoryId = 2, Name = "جاكيت خفيف كريمي", Price = 790m, Description = "لون ناعم وخامة عملية.", Image = "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 38, CategoryId = 2, Name = "جاكيت خفيف أزرق غامق", Price = 800m, Description = "سهل التنسيق مع معظم الإطلالات.", Image = "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 39, CategoryId = 2, Name = "جاكيت formal رمادي", Price = 1120m, Description = "أنيق وخفيف للمظهر الراقي.", Image = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 40, CategoryId = 2, Name = "جاكيت formal أسود", Price = 1140m, Description = "مناسب للمشاوير المهمة والمناسبات.", Image = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 41, CategoryId = 3, Name = "تيشيرت أبيض أوفر سايز", Price = 450m, Description = "قماش مريح ولمسة عصرية مناسبة لكل يوم.", Image = "https://images.unsplash.com/photo-1523398002811-999ca8dec234?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 42, CategoryId = 3, Name = "تيشيرت أسود سادة", Price = 430m, Description = "ستايل نظيف وسهل التنسيق.", Image = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 43, CategoryId = 3, Name = "تيشيرت بيج basic", Price = 420m, Description = "قطعة يومية بخامة ناعمة.", Image = "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 44, CategoryId = 3, Name = "تيشيرت كحلي basic", Price = 425m, Description = "لون هادئ وسهل مع الجينز.", Image = "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 45, CategoryId = 3, Name = "تيشيرت مطبوع front", Price = 470m, Description = "طباعة أمامية لستايل شبابي.", Image = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 46, CategoryId = 3, Name = "تيشيرت مطبوع back", Price = 480m, Description = "طباعة خلفية بلمسة streetwear.", Image = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 47, CategoryId = 3, Name = "تيشيرت أوف وايت heavy", Price = 510m, Description = "خامة أثقل ومظهر premium.", Image = "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 48, CategoryId = 3, Name = "تيشيرت أخضر زيتوني", Price = 455m, Description = "لون مميز للمشاوير اليومية.", Image = "https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 49, CategoryId = 3, Name = "تيشيرت رمادي ملانج", Price = 440m, Description = "عملي ومريح طوال اليوم.", Image = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 50, CategoryId = 3, Name = "تيشيرت fit slim أبيض", Price = 460m, Description = "قصة slim لإطلالة أنظف.", Image = "https://images.unsplash.com/photo-1554412933-514a83d2f3c8?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 51, CategoryId = 3, Name = "تيشيرت fit slim أسود", Price = 465m, Description = "ستايل بسيط بمظهر مرتب.", Image = "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 52, CategoryId = 3, Name = "تيشيرت striped أبيض وأسود", Price = 490m, Description = "خطوط كلاسيكية ولمسة عصرية.", Image = "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 53, CategoryId = 3, Name = "تيشيرت striped أزرق", Price = 495m, Description = "خطوط ناعمة وتنسيق سهل.", Image = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 54, CategoryId = 3, Name = "تيشيرت oversize بني", Price = 500m, Description = "ستايل واسع مناسب للوك casual.", Image = "https://images.unsplash.com/photo-1507679799987-c73779587ccf?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 55, CategoryId = 3, Name = "تيشيرت oversize رمادي", Price = 505m, Description = "خامة ناعمة وقصة مريحة.", Image = "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 56, CategoryId = 3, Name = "تيشيرت summer yellow", Price = 435m, Description = "لون صيفي ولمسة مرحة.", Image = "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 57, CategoryId = 3, Name = "تيشيرت summer pink", Price = 438m, Description = "خفيف ومبهج للأجواء الصيفية.", Image = "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 58, CategoryId = 3, Name = "تيشيرت minimal logo", Price = 470m, Description = "لوجو بسيط على الصدر.", Image = "https://images.unsplash.com/photo-1514996937319-344454492b37?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 59, CategoryId = 3, Name = "تيشيرت vintage print", Price = 520m, Description = "طابع vintage بطباعة مميزة.", Image = "https://images.unsplash.com/photo-1551232864-3f0890e580d9?auto=format&fit=crop&w=900&q=80" },
            new Product { Id = 60, CategoryId = 3, Name = "تيشيرت daily essential", Price = 410m, Description = "قطعة أساسية للبس اليومي.", Image = "https://images.unsplash.com/photo-1487412947147-5cebf100ffc2?auto=format&fit=crop&w=900&q=80" }
        );
    }
}
