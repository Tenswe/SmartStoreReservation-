# 🗄️ Supabase Kurulum Talimatları

## 1. Supabase Dashboard'a Git
- https://supabase.com adresine git
- Projenize giriş yapın: `qwfmdfjkivlfordvorwk`

## 2. SQL Editor'ı Aç
- Sol menüden "SQL Editor" seçin
- "New query" butonuna tıklayın

## 3. Sadece Seed Data Ekle
Tablolar zaten var, sadece güzel veri ekleyelim:

```sql
-- 🌟 SEED DATA PENTRU SMARTSTORE RESERVATION
-- Sadece veri ekleme (tablolar zaten var)

-- Önce mevcut test verilerini temizle (isteğe bağlı)
TRUNCATE TABLE reservations, product_categories, products, cabins, categories, users, shops RESTART IDENTITY CASCADE;

-- 1. SHOPS (Magazinele)
INSERT INTO shops (name, location) VALUES
('SmartStore Premium București', 'Calea Victoriei 120, București'),
('SmartStore Luxury Cluj', 'Strada Memorandumului 28, Cluj-Napoca'),
('SmartStore Elite Timișoara', 'Piața Unirii 1, Timișoara');

-- 2. CATEGORIES (Categoriile)
INSERT INTO categories (name) VALUES
('Rochii Elegante'),
('Costume Business'),
('Ținute Casual'),
('Accesorii Premium'),
('Încălțăminte Designer'),
('Genți de Lux');

-- 3. USERS (Utilizatorii demo)
INSERT INTO users (id, name, email, measurements, style_preferences) VALUES
('11111111-1111-1111-1111-111111111111', 'Ana Popescu', 'ana.popescu@email.com', 'M: 38, Înălțime: 165cm', 'Elegant, Modern'),
('22222222-2222-2222-2222-222222222222', 'Maria Ionescu', 'maria.ionescu@email.com', 'M: 40, Înălțime: 170cm', 'Business, Clasic'),
('33333333-3333-3333-3333-333333333333', 'Elena Dumitrescu', 'elena.dumitrescu@email.com', 'M: 36, Înălțime: 160cm', 'Casual, Trendy');

-- 4. CABINS (Cabinele pentru fiecare magazin)
INSERT INTO cabins (shop_id, cabin_number) VALUES
-- București (shop_id = 1)
(1, 1), (1, 2), (1, 3), (1, 4),
-- Cluj (shop_id = 2)  
(2, 1), (2, 2), (2, 3),
-- Timișoara (shop_id = 3)
(3, 1), (3, 2), (3, 3), (3, 4), (3, 5);

-- 5. PRODUCTS (Produsele frumoase)
INSERT INTO products (shop_id, name, size, color, stock, price, image_url) VALUES
-- București - Rochii Elegante
(1, 'Rochie Florală de Vară', 'M', 'Roz Pudră', 5, 299.99, 'https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=400&h=600&fit=crop'),
(1, 'Rochie de Seară Neagră', 'S', 'Negru', 3, 599.99, 'https://images.unsplash.com/photo-1566479179817-c0b5b4b4b1e5?w=400&h=600&fit=crop'),
(1, 'Rochie Cocktail Albastră', 'L', 'Albastru Royal', 4, 449.99, 'https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&h=600&fit=crop'),

-- București - Costume Business
(1, 'Costum Business Feminin', 'M', 'Gri Antracit', 6, 799.99, 'https://images.unsplash.com/photo-1594938298603-c8148c4dae35?w=400&h=600&fit=crop'),
(1, 'Blazer Premium cu Pantaloni', 'S', 'Bleumarin', 4, 699.99, 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=600&fit=crop'),

-- Cluj - Ținute Casual
(2, 'Jachetă Denim Vintage', 'M', 'Albastru Deschis', 8, 189.99, 'https://images.unsplash.com/photo-1551698618-1dfe5d97d256?w=400&h=600&fit=crop'),
(2, 'Pulover Cashmere', 'L', 'Bej', 5, 349.99, 'https://images.unsplash.com/photo-1434389677669-e08b4cac3105?w=400&h=600&fit=crop'),
(2, 'Pantaloni Palazzo', 'M', 'Verde Oliv', 6, 229.99, 'https://images.unsplash.com/photo-1506629905607-d405d7d3b0d2?w=400&h=600&fit=crop'),

-- Timișoara - Mix Premium
(3, 'Rochie Midi Elegantă', 'S', 'Bordo', 3, 399.99, 'https://images.unsplash.com/photo-1515372039744-b8f02a3ae446?w=400&h=600&fit=crop'),
(3, 'Costum Trei Piese', 'L', 'Negru', 2, 999.99, 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=600&fit=crop'),
(3, 'Rochie Boho Chic', 'M', 'Crem', 4, 279.99, 'https://images.unsplash.com/photo-1469334031218-e382a71b716b?w=400&h=600&fit=crop'),
(3, 'Jachetă Piele Premium', 'M', 'Maro Cognac', 3, 899.99, 'https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400&h=600&fit=crop');

-- 6. PRODUCT_CATEGORIES (Relațiile N-M)
INSERT INTO product_categories (product_id, category_id) VALUES
-- Rochii
(1, 1), (2, 1), (3, 1), (9, 1), (11, 1),
-- Costume Business  
(4, 2), (5, 2), (10, 2),
-- Casual
(6, 3), (7, 3), (8, 3), (12, 3),
-- Accesorii (jachetă piele)
(12, 4);

-- 7. REZERVĂRI DEMO (câteva rezervări pentru demonstrație)
INSERT INTO reservations (user_id, product_id, cabin_id, date, hour, access_code, duration, status) VALUES
('11111111-1111-1111-1111-111111111111', 1, 1, '2024-01-15', '14:30:00', 30, 'Confirmată', 'ABC123'),
('22222222-2222-2222-2222-222222222222', 4, 2, '2024-01-15', '15:00:00', 30, 'Confirmată', 'DEF456'),
('33333333-3333-3333-3333-333333333333', 6, 9, '2024-01-16', '10:00:00', 30, 'În Așteptare', 'GHI789');

-- Mesaj de succes
SELECT 'Seed data a fost adăugat cu succes! 🎉' as message;
```

## 4. Tabloları Kontrol Et
- Sol menüden "Table Editor" seçin
- Verilerin eklendiğini kontrol edin

## 5. API'yi Test Et
Veriler eklendikten sonra API'yi test edin.

---
**Not**: Bu script sadece veri ekler, mevcut tabloları değiştirmez! 🎉