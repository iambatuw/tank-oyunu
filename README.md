# Tank Düellosu

Tank Düellosu, aynı bilgisayarda iki kişiyle veya bilgisayara karşı oynanan bir tank savaş oyunudur. Orman, çöl ve ay üssü arenaları; beş farklı tank; garaj, altın ve güçlendirmeler içerir.

![Ana menü](UI-Preview/main-currency-fixed.png)

## Oyuna başla

1. GitHub sayfasındaki **Code → Download ZIP** seçeneğiyle dosyaları indirin.
2. ZIP dosyasının **tamamını** bir klasöre çıkartın.
3. Ana klasördeki **Oyunu Başlat.cmd** dosyasına çift tıklayın. İsterseniz doğrudan `Build/TankDuel.exe` dosyasını da açabilirsiniz.

Oynamak için Unity, Git veya bir geliştirici hesabı gerekmez. Windows 10/11, 64 bit bilgisayar kullanın. Okul bilgisayarına klasörün tamamını kopyalayarak oyun oynanabilir; `Build` klasöründeki veri dosyaları ve `.dll` dosyaları da gereklidir. Yalnızca `.exe` dosyasını ayırırsanız oyun açılmaz. Ayarlar bölümünden grafik düzeyini değiştirebilirsiniz. Bilgisayarın uygulama çalıştırma kısıtlamaları varsa okulun yetkili kişisinden yardım alın.

## Kontroller

| İşlem | 1. oyuncu | 2. oyuncu |
| --- | --- | --- |
| Tankı sür ve döndür | W, A, S, D | Yön tuşları |
| Ateş et | Boşluk | Enter |
| Duraklat / devam et | Esc | Esc |

Ateş tuşunu basılı tutarak atışı güçlendirebilirsiniz. Tek oyunculu modda ikinci tankı bilgisayar yönetir. Kolay zorlukta standart, orta zorlukta orta, zor zorlukta ağır tank kullanır. İki oyunculu modda ikinci oyuncu da garajdan tank seçebilir. Garajdaki satın alımlar altınla yapılır ve seçimler kaydedilir.

## Ekranlar

| Garaj | Ayarlar |
| --- | --- |
| ![Garaj](UI-Preview/garage-currency-fixed.png) | ![Ayarlar](UI-Preview/settings-1080p.png) |

Tek oyunculu modda kamera oyuncunun tankını yakından takip eder. Rakip uzaklaştığında tankları küçülten bir uzaklaştırma yerine ekran kenarında rakibin yönü gösterilir. İki oyunculu modda iki tank birlikte kadraja alınır. Üstteki can ve süre göstergeleri için boşluk bırakılır. Raunt duyuruları, arka plandan ayrılan bir panelde gösterilir.

![Oyun içi görünüm](UI-Preview/gameplay-player-follow.png)

## Zırh ve vuruş yönü

Doğrudan mermi çarpışmalarında tankın vurulan gövde yönü hasarı değiştirir:

| Vuruş | Hasar çarpanı |
| --- | --- |
| Ön zırh | ×0,75 |
| Yan gövde | ×1,00 |
| Arka gövde | ×1,50 |

Patlama hasarı mesafeyle azalır. Merminin doğrudan çarpmadığı, yalnızca patlamaya yakın olan tanklara yön çarpanı uygulanmaz. Kalkan ve geçici dokunulmazlık etkileri ayrıca geçerlidir.

![Raunt duyurusu](UI-Preview/round-message-1080p.png)

## Geliştirme ve lisans

Proje Unity **6000.6.4f1** ile geliştirilmiştir. Bu herkese açık depoda oyunun Windows sürümü, özgün menü ve oyun yönetimi kodları, ayarlar ve ekran görüntüleri bulunur. Projede kullanılan **Tanks!** içeriğinin ham varlıkları ve bu içeriğe dayanan sahneler, Unity Asset Store lisansı nedeniyle depoya konmamıştır. Bu nedenle depodaki kaynaklar tek başına yeniden derlenebilir tam bir Unity projesi değildir. Geliştirme için ilgili Tanks! içeriğini kendi lisansınızla edinmeniz ve yerel proje dosyalarına eklemeniz gerekir.

Depodaki MIT lisansı yalnızca bu projeye ait özgün kaynaklar için geçerlidir; Tanks! varlıklarının veya diğer üçüncü taraf içeriklerin lisansını değiştirmez. Üçüncü taraf lisans bilgileri yerel Unity projesindeki `Assets/_Tanks/Tanks!_Third-PartyNotice.txt` dosyasında bulunur.

### Tam Unity projesini başka bilgisayara taşıma

Yerel projede hazırlanan `Export/Tank Düellosu Unity Projesi.zip`, `Assets`, `Packages` ve `ProjectSettings` klasörlerini, üç arena sahnesini ve kullanılan oyun kodlarını içerir. Bu arşiv lisanslı ham varlıkları da içerdiği için herkese açık GitHub deposuna yüklenmez. Kendi bilgisayarlarınız arasında taşımak için kullanabilirsiniz.

Arşivi çıkartın, Unity Hub üzerinden çıkartılan klasörü proje olarak ekleyin ve Unity **6000.6.4f1** ile açın. İlk açılışta Unity gerekli paketleri internetten indirir; ardından `Assets/Scenes/Duel_Jungle.unity` sahnesini açıp oynatın. Windows için yeniden derlemek isterseniz Windows Build Support kurulu olmalıdır. Arşivde geliştirme önbellekleri, kişisel oturum dosyaları ve Unity MCP bağlantısı bulunmaz.

### Dosyalar

| Klasör | İçerik |
| --- | --- |
| `Build` | Unity kurulmadan çalıştırılabilen Windows oyunu |
| `Assets/Scripts` | Özgün menü, kamera kadrajı, rakip göstergesi ve zırh hesabı kodları |
| `Assets/Editor` | Sahne hazırlama ve derleme araçları |
| `ProjectSettings`, `Packages` | Unity sürümü, paket ve proje ayarları |
| `Licenses` | Yazı tipi ve üçüncü taraf lisans bildirimleri |

Inter yazı tipi SIL Open Font License ile dağıtılır; [lisans metni](Licenses/Inter-OFL.txt) pakete eklenmiştir. Günlüğe kaydedilen oturum bilgileri, anahtarlar, `.env` dosyaları ve yerel Unity proje arşivi GitHub yüklemesine dahil edilmez.
