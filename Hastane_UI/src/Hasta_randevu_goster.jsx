import { useState } from 'react';
import './Hasta_add.css';

function HastaRandevuGoster() {

    const [tc, setTc] = useState("");
    const [randevular, setRandevular] = useState([]);

    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);

    const handleSumbit = async (e) => {
        e.preventDefault();
        setYukleniyor(true);
        setHata(null);
        setRandevular([]);

        try {
            const cevap = await fetch(`http://localhost:5160/api/Hasta/RandevuGoster`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });





            if (!cevap.ok) {
                throw new Error(`Sunucu hatası: HTTP ${cevap.status}`);
            }

            const veri = await cevap.json();
            // Varsayılan olarak api'nin bir liste döndüğünü varsayıyoruz
            setRandevular(Array.isArray(veri) ? veri : [veri]);
            console.log("Server Response:", veri);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container" style={{ maxWidth: '600px' }}>
            <h2>Randevu Görüntüle</h2>
            <form onSubmit={handleSumbit}>
                <div>
                    <label>Sorgulanacak TC Kimlik No:</label>
                    <input type="text" value={tc} onChange={(e) => setTc(e.target.value)} required />
                </div>
                <button type="submit" disabled={yukleniyor}>
                    {yukleniyor ? "Sorgulanıyor..." : "Randevuları Getir"}
                </button>
            </form>

            {hata && <div className="error-message">Hata: {hata}</div>}

            {randevular.length > 0 && (
                <div style={{ marginTop: '20px' }}>
                    <h3>Bulunan Randevular:</h3>
                    <ul style={{ listStyleType: 'none', padding: 0 }}>
                        {randevular.map((randevu, index) => (
                            <li key={index} style={{ padding: '10px', border: '1px solid #ccc', margin: '5px 0', borderRadius: '5px' }}>
                                <strong>Doktor:</strong> {randevu.DoktorAdi || randevu.doktorId || "Belirtilmemiş"} <br />
                                <strong>Tarih:</strong> {randevu.Tarih || randevu.tarih || "Belirtilmemiş"} <br />
                                <strong>Saat:</strong> {randevu.Saat || randevu.saat || "Belirtilmemiş"}
                            </li>
                        ))}
                    </ul>
                </div>
            )}

            {!yukleniyor && !hata && randevular.length === 0 && tc && (
                <div style={{ marginTop: '20px', color: '#666' }}>Gösterilecek randevu bulunamadı.</div>
            )}
        </div>
    );
}

export default HastaRandevuGoster;
