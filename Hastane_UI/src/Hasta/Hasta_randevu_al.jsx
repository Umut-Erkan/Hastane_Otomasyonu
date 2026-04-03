import { useState } from 'react';
import './HastaStyle/Hasta_add.css';

function HastaRandevuAl() {

    const [tc, setTc] = useState("");
    const [doktorId, setDoktorId] = useState("");
    const [tarih, setTarih] = useState("");
    const [saat, setSaat] = useState("");

    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [mesaj, setMesaj] = useState("");

    const handleSumbit = async (e) => {
        e.preventDefault();
        setYukleniyor(true);
        setHata(null);

        try {
            const cevap = await fetch('http://localhost:5160/api/Randevu/Randevu Al', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Tc: tc,
                    DoktorId: doktorId,
                    Tarih: tarih,
                    Saat: saat,
                }),
            });

            if (!cevap.ok) {
                throw new Error(`Sunucu hatası: HTTP ${cevap.status}`);
            }

            const veri = await cevap.json();
            setMesaj("Randevu başarıyla alındı!");
            console.log("Server Response:", veri);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container">
            <h2>Randevu Al</h2>
            <form onSubmit={handleSumbit}>
                <div>
                    <label>Hasta TC:</label>
                    <input type="text" value={tc} onChange={(e) => setTc(e.target.value)} required />
                </div>
                <div>
                    <label>Doktor ID:</label>
                    <input type="text" value={doktorId} onChange={(e) => setDoktorId(e.target.value)} required />
                </div>
                <div>
                    <label>Randevu Tarihi:</label>
                    <input type="date" value={tarih} onChange={(e) => setTarih(e.target.value)} required />
                </div>
                <div>
                    <label>Randevu Saati:</label>
                    <input type="time" value={saat} onChange={(e) => setSaat(e.target.value)} required />
                </div>

                <button type="submit" disabled={yukleniyor}>
                    {yukleniyor ? "İşleniyor..." : "Randevu Al"}
                </button>
            </form>

            {hata && <div className="error-message">Hata: {hata}</div>}
            {mesaj && <div className="success-message">{mesaj}</div>}
        </div>
    );
}

export default HastaRandevuAl;
