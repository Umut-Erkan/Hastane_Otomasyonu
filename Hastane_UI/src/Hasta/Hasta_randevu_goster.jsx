import { useState, useEffect } from 'react';
import './HastaStyle/Hasta_add.css';

function HastaRandevuGoster() {

    const [randevular, setRandevular] = useState([]);
    const [yukleniyor, setYukleniyor] = useState(true);
    const [hata, setHata] = useState(null);

    useEffect(() => {
        const fetchRandevular = async () => {
            try {
                const token = localStorage.getItem('hastaToken');

                if (token === null) {
                    throw new Error("Token bulunamadı");
                }

                try {
                    const payload = JSON.parse(atob(token.split('.')[1]));
                    // Farklı API'lerde e-posta değişebilir, genellikle bilinen claim isimlerini kontrol ediyoruz.
                    const email = payload.email || payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || payload.Eposta || "E-posta bulunamadı";
                    console.log("Token İçerisindeki E-Posta:", email);
                }

                catch (parsErr) {
                    console.log("Token çözümlenemedi:", parsErr);
                }

                const cevap = await fetch(`http://localhost:5160/api/Hasta/RandevuGoster`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        "Authorization": `Bearer ${token}`,
                    }
                });

                if (cevap.status === 401) {
                    throw new Error(`Giriş yapmanız gerekiyor: HTTP ${cevap.status}`);
                }

                if (cevap.status === 403) {
                    throw new Error(`Yetkisiz erişim: HTTP ${cevap.status}`);
                }


                const veri = await cevap.json();
                console.log("Server Response:", veri.mesaj);

                // Eğer cevap bir hata mesajı içeriyorsa (örneğin randevu yoksa) veya boş liste dönerse
                const isArray = Array.isArray(veri);
                if ((isArray && veri.length === 0) || (!isArray && veri.message)) {
                    setRandevular([]);
                } else {
                    setRandevular(isArray ? veri : [veri]);
                }
                console.log("Server Response:", veri);

            } catch (err) {
                setHata(err.message);
            } finally {
                setYukleniyor(false);
            }
        };

        fetchRandevular();
    }, []);

    return (
        <div className="hasta-form-container" style={{ maxWidth: '600px' }}>
            <h2>Randevularım</h2>

            {yukleniyor && <div>Yükleniyor...</div>}

            {hata && <div className="error-message">Hata: {hata}</div>}

            {!yukleniyor && randevular.length > 0 && (
                <div style={{ marginTop: '20px' }}>
                    <ul style={{ listStyleType: 'none', padding: 0 }}>
                        {randevular.map((randevu, index) => (
                            <li key={index} style={{ padding: '10px', border: '1px solid #ccc', margin: '5px 0', borderRadius: '5px' }}>
                                <strong>Randevu ID:</strong> {randevu.id} <br />
                                <strong>Doktor:</strong> {randevu.doktorName} <br />
                                <strong>Tarih:</strong> {randevu.Tarih || randevu.tarih || "Belirtilmemiş"} <br />
                                <strong>Saat:</strong> {randevu.Saat || randevu.saat || "Belirtilmemiş"}
                            </li>
                        ))}
                    </ul>
                </div>
            )}

            {!yukleniyor && !hata && randevular.length === 0 && (
                <div style={{ marginTop: '20px', color: '#666', fontWeight: 'bold' }}>Randevunuz yok.</div>
            )}
        </div>
    );
}

export default HastaRandevuGoster;
