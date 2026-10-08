# Kontrol sonuçları — 8 Ekim 2026

## Menü ve altın

- Altın göstergesi ana menüde maç kurulum kartının, garajda gezinme çubuğunun içine yerleştirildi.
- 1920 × 1080 oyun görünümünde ana menü ve garaj görüntü üzerinden kontrol edildi.
- 2.147.483.647 altın metni kesilmeden sığdı; bakiye değiştirilmeksizin metin alanı sınandı.

## Kamera

- Tek oyunculu modda oyuncu merkezde takip edilir; uzak rakip yakınlaştırmayı bozmaz, yönü ekran kenarında gösterilir.
- İki oyunculu modda iki aktif tankın da görünmesi için kadraj ayarlanır.
- Üç farklı yükseklikte, 16:9, 16:10 ve 4:3 oranlarında toplam 3.240 hareketli konum kontrolü yapıldı; takip edilmesi gereken tanklar güvenli kadrajın dışına çıkmadı.
- Duraklatma sonrası hareket ve yakınlaştırma kontrol edildi; geçersiz kamera değerleri oluşmadı.
- Orman arenasında maç içi görünüm ve takip ayrıca kontrol edildi.

## Hasar

- Gerçek tank gövdesi ve mermi patlaması üzerinden ön, yan ve arka çarpışmalar denendi.
- Ön ×0,75; yan ×1,00; arka ×1,50 hasar çarpanları doğrulandı.
- Aynı patlamayı ikinci kez tetiklemek ilave hasar vermedi.
- Patlama mesafesi ve mevcut kalkan sistemi korunur; yön etkisi yalnızca doğrudan çarpılan tanka uygulanır.

## Derleme

- Windows 64 bit derlemesi başarılı: 0 hata, yaklaşık 178 MB.
- Oluşturulan Windows paketi açılış kontrolünü istisna hatası olmadan geçti.
- Tam Unity proje arşivi ayrı bir klasöre açıldı. Unity gerekli paketleri indirdi, varlıkları içe aktardı ve hem oyun hem editör kodlarını 0 derleme hatasıyla derledi; işlem 0 çıkış koduyla tamamlandı.
- Unity'nin eski proje ayarı biçimleri ve geliştirme paketleri için uyarıları hâlâ mevcut.
- Bu kontroller belirli bir okul bilgisayarının performansını veya uygulama çalıştırma izinlerini ölçmez.
