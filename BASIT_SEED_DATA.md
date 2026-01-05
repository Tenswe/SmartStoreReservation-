# 🚀 En Basit Yöntem: Otomatik Seed Data

## Tablolar Zaten Varsa:

1. **API'yi Başlat**:
   ```bash
   cd API
   dotnet run
   ```

2. **Seed Data Otomatik Çalışır**:
   - Program.cs'de zaten seed data service var
   - API başladığında otomatik olarak güzel veriler eklenecek
   - Console'da "✅ Seed data a fost încărcat cu succes!" mesajını göreceksin

3. **Test Et**:
   - http://localhost:5269/api/v1/products
   - http://localhost:3000 (frontend)

## Eğer Hata Alırsan:

Sadece bu basit SQL'i çalıştır (mevcut verileri temizler):
```sql
-- Sadece mevcut verileri temizle
DELETE FROM reservations;
DELETE FROM product_categories;  
DELETE FROM products;
DELETE FROM cabins;
DELETE FROM categories;
DELETE FROM users;
DELETE FROM shops;

-- ID'leri sıfırla
ALTER SEQUENCE shops_id_seq RESTART WITH 1;
ALTER SEQUENCE categories_id_seq RESTART WITH 1;
ALTER SEQUENCE products_id_seq RESTART WITH 1;
ALTER SEQUENCE cabins_id_seq RESTART WITH 1;
ALTER SEQUENCE reservations_id_seq RESTART WITH 1;
ALTER SEQUENCE product_categories_id_seq RESTART WITH 1;
```

Sonra API'yi yeniden başlat, otomatik olarak güzel veriler eklenecek! 🎉

---
**Avantaj**: Hiç manuel SQL yazmana gerek yok, her şey otomatik! 🚀